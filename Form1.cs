using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using Binarysharp.MemoryManagement.Modules;
using Binarysharp.MemoryManagement.Native;
using Gma.System.MouseKeyHook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AutoCheck
{
  public partial class Form1 : Form
  {
    private MemorySharp _sharp = null;

    private AutoKeyThread _threadQ;
    private AutoKeyThread _threadW;
    private AutoQWEThread _qweThread = null;
    private bool _starting = false;
    private long _addr = 0;

    private System.Windows.Forms.Timer m_Timer;
    private bool m_bForcingClose = false;
    private bool m_bAutoHide = false;
    private string m_window = "";

    private IKeyboardMouseEvents m_GlobalHook;

    public Form1()
    {
      InitializeComponent();
    }

    private void OnFormLoad(object sender, EventArgs e)
    {
      _About.Text = this.GetType().Assembly.GetName().Version.ToString();
      _threadQ = new AutoKeyThread('q', new TextBoxAdapter(_HPBox));
      _threadW = new AutoKeyThread('w', new TextBoxAdapter(m_ManaBox));

      _qweThread = new AutoQWEThread(m_KeyCount);

      LoadConfig();
      LoadData();

      m_GlobalHook = Hook.GlobalEvents();
      m_GlobalHook.KeyUp += OnKeyUp;
      m_GlobalHook.KeyDown += OnKeyDown;

      m_NotifyIcon.Visible = true;
      m_ShowMenu.Visible = !this.Visible;
      m_HideMenu.Visible = this.Visible;

      m_CloseMenu.Click += OnCloseMenu_Click;
      m_HideMenu.Click += OnHideMenu_Click;
      m_ShowMenu.Click += OnShowMenu_Click;
      m_AutoHideMenu.Click += OnAutoHideMenu_Click;
      m_StartMenu.Click += OnStartMenuClick;

      m_window = System.Configuration.ConfigurationManager.AppSettings["window"].ToUpper();
      m_Timer = new System.Windows.Forms.Timer();
      m_Timer.Interval = 3000;
      m_Timer.Tick += OnTimer_Tick;
      m_Timer.Start();
    }
    //----------------------------------------------------------------------------------
    private void OnKeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == System.Windows.Forms.Keys.ControlKey)
      {
        e.Handled = false;
      }
    }
    //----------------------------------------------------------------------------------
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == System.Windows.Forms.Keys.D0 && e.Control)
      {
        ToggleStartStop();
        e.Handled = true;
      }
    }
    //----------------------------------------------------------------------------------
    private void LoadConfig()
    {
      _threadW.Scale = Utils.GetConfigDouble("scaleW", 0.99);
      _threadQ.Scale = Utils.GetConfigDouble("scaleQ", 0.99);

      _threadQ.WarnScale = Utils.GetConfigDouble("WarnScale", 0.99);
      _threadQ.WarnVolume = (float)(Utils.GetConfigDouble("WarnVolume", 1));
      _threadQ.WarnSound = Utils.GetConfigString("WarnSound", "alarm.mp3");

      m_VolumeCtrl.Minimum = 0;
      m_VolumeCtrl.Maximum = 100;
      m_VolumeCtrl.Value = (int)(Math.Min(_threadQ.WarnVolume, 1.0f) * 100f);

      _threadW.WarnScale = 0;

      _threadQ.KeyDelay = Utils.GetConfigInt("QKeyDelay", 16);
      _threadQ.KeyThreadOptTime = Utils.GetConfigInt("QThreadOptTime", 32);
      _threadQ.MemThreadDelay = Utils.GetConfigInt("QMemThreadDelay", 10);
      _threadQ.WarnThreadDelay = Utils.GetConfigInt("QWarnThreadDelay", 10);

      _threadW.KeyDelay = Utils.GetConfigInt("WKeyDelay", 16);
      _threadW.KeyThreadOptTime = Utils.GetConfigInt("WKeyThreadOptTime", 32);
      _threadW.MemThreadDelay = Utils.GetConfigInt("WMemThreadDelay", 16);
      _threadW.WarnThreadDelay = Utils.GetConfigInt("WWarnThreadDelay", 16);

      _qweThread.Delay = Utils.GetConfigInt("QWEDelay", 16);
      _qweThread.ThreadOptTime = Utils.GetConfigInt("QWEThreadOptTime", 16);
    }
    //----------------------------------------------------------------------------------
    private void OnStartMenuClick(object sender, EventArgs e)
    {
      ToggleStartStop();
    }
    //----------------------------------------------------------------------------------
    private void OnAutoHideMenu_Click(object sender, EventArgs e)
    {
      m_bAutoHide = !m_bAutoHide;
      m_AutoHideMenu.Checked = m_bAutoHide;
    }
    //----------------------------------------------------------------------------------
    private void OnTimer_Tick(object sender, EventArgs e)
    {
      CheckWindows();
    }
    //----------------------------------------------------------------------------------
    private void OnShowMenu_Click(object sender, EventArgs e)
    {
      ShowMe(true);
    }
    //----------------------------------------------------------------------------------
    private void OnHideMenu_Click(object sender, EventArgs e)
    {
      ShowMe(false);
    }
    //----------------------------------------------------------------------------------
    private void OnCloseMenu_Click(object sender, EventArgs e)
    {
      m_bForcingClose = true;
      Close();
    }
    //----------------------------------------------------------------------------------
    private void m_ContextMenu_Opened(object sender, EventArgs e)
    {
      m_AutoHideMenu.Checked = m_bAutoHide;
    }
    //----------------------------------------------------------------------------------
    private void OnFormClosing(object sender, FormClosingEventArgs e)
    {
      if (m_bForcingClose == false && e.CloseReason == CloseReason.UserClosing)
      {
        m_NotifyIcon.Visible = true;
        ShowMe(false);
        e.Cancel = true;
      }
    }
    //----------------------------------------------------------------------------------
    private void OnFormClosed(object sender, FormClosedEventArgs e)
    {
      m_GlobalHook.KeyUp -= OnKeyUp;
      m_GlobalHook.KeyDown -= OnKeyDown;

      //It is recommened to dispose it
      m_GlobalHook.Dispose();

      SaveData();
      Stop();

      m_Timer.Tick -= OnTimer_Tick;
      m_Timer.Stop();
    }
    //----------------------------------------------------------------------------------
    private void OnScanClicked(object sender, EventArgs e)
    {
      ScanHPForm f = new ScanHPForm();
      if (f.ShowDialog() == DialogResult.OK)
      {
        _addr = f.GetAddress();
      }
    }
    //----------------------------------------------------------------------------------
    private void OnAutoWCheckedChanged(object sender, EventArgs e)
    {
      _threadW.Auto = m_AutoW.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnAutoQCheckedChanged(object sender, EventArgs e)
    {
      _threadQ.Auto = m_AutoQ.Checked;
    }
    //----------------------------------------------------------------------------------
    private bool Start()
    {
      _sharp = Utils.CreateMemorySharp();
      if (_sharp == null)
      {
        MessageBox.Show("Could not read the memory");
        return false;
      }

      m_StartButton.Text = "Stop";
      m_ScanButton.Enabled = false;

      m_StartMenu.Text = "Stop";

      var stopwatch = System.Diagnostics.Stopwatch.StartNew();
      while (stopwatch.ElapsedMilliseconds < 10) { /* Do nothing */ }
      return true;
    }
    //----------------------------------------------------------------------------------
    private bool Stop()
    {
      _threadQ.Stop();
      _threadW.Stop();

      m_StartButton.Text = "Start";
      m_ScanButton.Enabled = true;

      m_StartMenu.Text = "Start";

      _sharp?.Dispose();
      return true;
    }
    //----------------------------------------------------------------------------------
    private void ToggleStartStop()
    {
      try
      {
        if (_starting == true)
        {
          return;
        }

        m_StartButton.Enabled = false;
        _starting = true;
        if (_threadQ.IsRunning() || _threadW.IsRunning() || _qweThread.IsRunning())
        {
          _threadQ.Stop();
          _threadW.Stop();
          _qweThread.Stop();
          Stop();
        }
        else
        {
          _threadQ.Auto = m_AutoQ.Checked;
          _threadW.Auto = m_AutoW.Checked;

          _qweThread.QEnable = m_QChk.Checked;
          _qweThread.WEnable = m_WChk.Checked;
          _qweThread.EEnable = m_EChk.Checked;

          if (Start())
          {
            _threadQ.Start(_sharp, _addr, _addr + 16);
            _threadW.Start(_sharp, _addr + 8, _addr + 24);
            _qweThread.Start(_sharp);
          }
        }
      }
      finally
      {
        _starting = false;
        m_StartButton.Enabled = true;
      }
    }
    //----------------------------------------------------------------------------------
    private void OnStartButtonClicked(object sender, EventArgs e)
    {
      ToggleStartStop();
    }
    //----------------------------------------------------------------------------------
    private string GetDataFile()
    {
      string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(exeDirectory, "data.json");
    }
    //----------------------------------------------------------------------------------
    private void SaveData()
    {
      string file = GetDataFile();
      var data = new Dictionary<string, string>();

      data.Add("ADDR", _addr.ToString("X"));
      data.Add("AutoQ", m_AutoQ.Checked ? "True" : "False");
      data.Add("AutoW", m_AutoW.Checked ? "True" : "False");
      data.Add("Q", m_QChk.Checked ? "True" : "False");
      data.Add("W", m_WChk.Checked ? "True" : "False");
      data.Add("E", m_EChk.Checked ? "True" : "False");

      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      File.WriteAllText(file, json);
    }
    //----------------------------------------------------------------------------------
    private void LoadData()
    {
      string file = GetDataFile();
      if (File.Exists(file) == true)
      {
        string json = File.ReadAllText(file);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        _addr = 0;
        if (data.ContainsKey("ADDR"))
        {
          if (long.TryParse(data["ADDR"], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long result))
          {
            _addr = result;
          }
        }

        m_AutoQ.Checked = (data["AutoQ"].ToUpper() == "TRUE");
        m_AutoW.Checked = (data["AutoW"].ToUpper() == "TRUE");

        if (data.ContainsKey("Q"))
          m_QChk.Checked = (data["Q"].ToUpper() == "TRUE");

        if (data.ContainsKey("W"))
          m_WChk.Checked = (data["W"].ToUpper() == "TRUE");

        if (data.ContainsKey("E"))
          m_EChk.Checked = (data["E"].ToUpper() == "TRUE");
      }
    }
    //----------------------------------------------------------------------------------
    private void OnVolumeValueChanged(object sender, EventArgs e)
    {
      _threadQ.WarnVolume = (float)m_VolumeCtrl.Value / (float)m_VolumeCtrl.Maximum;
    }
    //--------------------------------------------------------------------------------------------
    private void CheckWindows()
    {
      m_NotifyIcon.Text = string.Format("Status: {0}/{1}", _threadQ.Value, _threadW.Value);

      if (m_bAutoHide == false)
      {
        return;
      }

      if (m_window == "*" || FindWindow(m_window) == true)
      {
        return;
      }

      ShowMe(false);
    }
    //----------------------------------------------------------------------------------
    private void ShowMe(bool show)
    {
      if (show)
      {
        this.Show();
        m_ShowMenu.Visible = false;
        m_HideMenu.Visible = true;
      }
      else
      {
        this.Hide();
        m_ShowMenu.Visible = true;
        m_HideMenu.Visible = false;
      }
    }
    //----------------------------------------------------------------------------------
    private bool FindWindow(string name)
    {
      foreach (Process process in Process.GetProcesses())
      {
        if (!String.IsNullOrEmpty(process.MainWindowTitle))
        {
          if (process.MainWindowTitle.ToUpper().Contains(name.ToUpper()))
          {
            return true;
          }
        }
      }
      return false;
    }
    //----------------------------------------------------------------------------------
    private void m_CloseButton_Click(object sender, EventArgs e)
    {
      m_bForcingClose = true;
      Close();
    }
    //----------------------------------------------------------------------------------
    private void m_NotifyIcon_Click(object sender, EventArgs e)
    {
      ShowMe(true);
    }
  }
}
