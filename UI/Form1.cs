using Gma.System.MouseKeyHook;
using log4net;
using MXTools.Helpers;
using MXTools.Input;
using MXTools.Properties;
using MXTools.Threads;
using MXTools.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MXTools
{
  public partial class Form1 : Form
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private ulong _addr = 0;
    private AutoKeyThread _threadQ = null;
    private AutoKeyThread _threadW = null;
    private AutoQWEThread _qweThread = null;
    private AutoMouseThread _mThread = null;
    private TimeWarning _timerWarning = null;
    private GeneralThread _gnThread = null;

    private bool _starting = false;
    private bool m_bForcingClose = false;

    private IKeyboardMouseEvents m_GlobalHook;
    private ThreadSettings _settings = new();
    private bool _enableStart = true;

    public Form1()
    {
      InitializeComponent();

      SendInputEx.Init();
      KeybdEvent.Init();
      MouseEvent.Init();

      if (KeyboardManager.Active.Init() == false)
      {
        MessageBox.Show("Loading keyboard fails", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      if (MouseManager.Active.Init() == false)
      {
        MessageBox.Show("Loading mouse fails.", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      if (false == MxSharp.Instance.Attach(GlobalSettings.Instance.GetConfigString("app")))
      {
        _log.Warn("App is not running yet");
      }
      else
      {
        _log.Warn($"App is running. Attached to {MxSharp.Instance.PID()}");
      }

    }
    //----------------------------------------------------------------------------------
    protected override void WndProc(ref Message m)
    {
      if (m.Msg == 0x0010) // WM_CLOSE
      {
        _log.Info("Received CLOSE request");
        if (m_bForcingClose == false)
        {
          ShowMe(false);
          return;
        }
      }
      base.WndProc(ref m);
    }
    //----------------------------------------------------------------------------------
    private void Align()
    {
      int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
      int formWidth = this.Width;

      int x = (screenWidth - formWidth) / 2;
      int y = 0;

      this.Location = new System.Drawing.Point(x, y);
    }
    //----------------------------------------------------------------------------------
    private void OnFormLoad(object sender, EventArgs e)
    {
      try
      {
        Align();
        LoadData();

        this.Text = $"{Resources.AppTitle} {this.GetType().Assembly.GetName().Version} {Resources.Author}";

        ForegroundWindowCheck.Instance.Start();

        _threadQ = new AutoKeyThread('q', _settings.Q, (cur, max) =>
        {
          BeginInvoke(() =>
          {
            if (max >= 0 && cur >= 0 && _HPBar.Maximum != max || _HPBar.Value != cur)
            {
              _HPBar.Maximum = max;
              _HPBar.Value = Math.Min(cur, max);
            }
          });
        });
        _threadW = new AutoKeyThread('w', _settings.W, (cur, max) =>
        {
          BeginInvoke(() =>
          {
            if (max >= 0 && cur >= 0 && _ManaBar.Maximum != max || _ManaBar.Value != cur)
            {
              _ManaBar.Maximum = max;
              _ManaBar.Value = Math.Min(cur, max);
            }
          });
        });

        _qweThread = new AutoQWEThread(_settings.QWE, (count) =>
        {
          BeginInvoke(() => m_KeyCount.Text = count.ToString());
        });

        _mThread = new AutoMouseThread(_settings.M);
        _timerWarning = new TimeWarning(_settings.T)
        {
          Update = (rmain) =>
          {
            var ts = TimeSpan.FromSeconds(rmain);
            BeginInvoke(new Action(() => { _WarnTime.Text = $"{ts.Minutes:D1}:{ts.Seconds:D2}"; }));
          }
        };

        _gnThread = new GeneralThread();

        UpdateUIByData();

        m_GlobalHook = Hook.GlobalEvents();
        m_GlobalHook.KeyUp += OnKeyUp;
        m_GlobalHook.KeyDown += OnKeyDown;
        m_GlobalHook.MouseClick += OnMouseClick;

        m_NotifyIcon.Visible = true;
        m_ShowMenu.Visible = !this.Visible;
        m_HideMenu.Visible = this.Visible;

        m_CloseMenu.Click += OnCloseMenu_Click;
        m_HideMenu.Click += OnHideMenu_Click;
        m_ShowMenu.Click += OnShowMenu_Click;
        m_StartMenu.Click += OnStartMenuClick;

        if (MxSharp.Instance.EnsureAttached())
        {
          UpdateToogleButton(Win32.GetMainWindowHandle((int)MxSharp.Instance.PID()));
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when loading: {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void OnMouseClick(object sender, MouseEventArgs e)
    {
      try
      {
        if (e.Button == MouseButtons.XButton1 || e.Button == MouseButtons.XButton2)
        {
          ToggleStartStop();
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when handling X button click {ex.Message}");
      }
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
      try
      {
        if (e.Control)
        {
          switch (e.KeyCode)
          {
            case System.Windows.Forms.Keys.D0:
              ToggleStartStop();
              break;

            case System.Windows.Forms.Keys.A:
              {
                _settings.M.Auto = !_settings.M.Auto;
                BeginInvoke((System.Windows.Forms.MethodInvoker)(() => m_AutoMouse.Checked = _settings.M.Auto));
              }
              break;

            case System.Windows.Forms.Keys.Divide:
            case System.Windows.Forms.Keys.Multiply:
              {
                if (MxSharp.Instance.EnsureAttached())
                {
                  var handle = Win32.GetMainWindowHandle((int)MxSharp.Instance.PID());
                  WindowManipulate.ShowWindow(handle);
                  UpdateToogleButton(handle);
                }
              }
              break;

            case System.Windows.Forms.Keys.Oem3:
              {
                if (MxSharp.Instance.EnsureAttached())
                {
                  var handle = Win32.GetMainWindowHandle((int)MxSharp.Instance.PID());
                  if (handle != nint.Zero)
                  {
                    WindowManipulate.HideWindow(handle);
                    UpdateToogleButton(handle);
                  }
                }
              }
              break;

            case System.Windows.Forms.Keys.Oemcomma:
              {
                ShowMe(false);
              }
              break;

            case System.Windows.Forms.Keys.OemPeriod:
              {
                ShowMe(true);
                Align();
                Win32.SetForegroundWindow(Handle);
              }
              break;
          }
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when handling key down event: {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void OnStartMenuClick(object sender, EventArgs e)
    {
      try
      {
        ToggleStartStop();
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when handling start menu click: {ex.Message}");
      }
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
    private void ContextMenu_Opened(object sender, EventArgs e)
    {
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
      try
      {
        m_GlobalHook.KeyUp -= OnKeyUp;
        m_GlobalHook.KeyDown -= OnKeyDown;
        m_GlobalHook.MouseClick -= OnMouseClick;
        m_GlobalHook.Dispose();

        SaveData();
        Stop();

        m_NotifyIcon.Visible = false;
        m_NotifyIcon.Dispose();

        ForegroundWindowCheck.Instance.Stop();
        KeyboardManager.Active.Destroy();
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred after the main form closed {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void OnScanClicked(object sender, EventArgs e)
    {
      _enableStart = false;
      ScanHPForm f = new();
      if (f.ShowDialog() == DialogResult.OK)
      {
        _addr = f.GetAddress();
        SaveAddress();
      }
      _enableStart = true;
    }
    //----------------------------------------------------------------------------------
    private void OnAutoWCheckedChanged(object sender, EventArgs e)
    {
      _settings.W.Auto = m_AutoW.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnAutoQCheckedChanged(object sender, EventArgs e)
    {
      _settings.Q.Auto = m_AutoQ.Checked;
    }
    //----------------------------------------------------------------------------------
    private bool Start()
    {
      try
      {
        _enableStart = false;
        if (MxSharp.Instance.EnsureAttached() == false)
        {
          MessageBox.Show("The process is not running", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
          _enableStart = true;
          return false;
        }

        _log.Info($"Starting with PID {MxSharp.Instance.PID()}");

        m_StartButton.Image = Resources.stop_16;
        m_ScanButton.Enabled = false;

        m_StartMenu.Text = "Stop";
        _SettingsButton.Enabled = false;
        _InfoButton.Enabled = false;

        Thread.Sleep(50);
        _enableStart = true;

        Win32.CloseApps();
        ForegroundWindowCheck.Instance.ForceForegroundCheck();
        return true;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when starting: {ex.Message}");
        return false;
      }
    }
    //----------------------------------------------------------------------------------
    private bool Stop()
    {
      m_StartButton.Image = Resources.play_16;
      m_ScanButton.Enabled = true;

      m_StartMenu.Text = "Start";
      _SettingsButton.Enabled = true;
      _InfoButton.Enabled = true;

      return true;
    }
    //----------------------------------------------------------------------------------
    private void ToggleStartStop()
    {
      if (_enableStart == false)
      {
        return;
      }
      try
      {
        if (_starting == true)
        {
          return;
        }

        m_StartButton.Enabled = false;
        _starting = true;
        _enableStart = false;
        if (_threadQ.IsRunning())
        {
          _threadQ.Stop();
          _threadW.Stop();
          _qweThread.Stop();
          _mThread.Stop();
          _timerWarning.Stop();
          _gnThread.Stop();
          Stop();
        }
        else
        {
          if (Start())
          {
            _threadQ.Start(_addr, _addr + 16);
            _threadW.Start(_addr + 8, _addr + 24);
            _qweThread.Start();
            _mThread.Start();
            _timerWarning.Start();
            _gnThread.Start();
          }
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when toggle start/stop: {ex.Message}");
      }
      finally
      {
        _starting = false;
        _enableStart = true;
        m_StartButton.Enabled = true;
      }
    }
    //----------------------------------------------------------------------------------
    private void OnStartButtonClicked(object sender, EventArgs e)
    {
      ToggleStartStop();
    }
    //----------------------------------------------------------------------------------
    private static string GetAddressFile()
    {
      string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(exeDirectory, "addr.json");
    }
    //----------------------------------------------------------------------------------
    private void SaveAddress()
    {
      try
      {
        string file = GetAddressFile();
        var data = new Dictionary<string, ulong>
      {
        { "addr", _addr }
      };
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(file, json);
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when saving address: {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void LoadAddress()
    {
      try
      {
        string file = GetAddressFile();
        if (File.Exists(file) == true)
        {
          string json = File.ReadAllText(file);
          var data = JsonConvert.DeserializeObject<Dictionary<string, ulong>>(json);
          _addr = data["addr"];
        }
      }
      catch (Exception ex)
      {
        _addr = 0;
        _log.Fatal($"An error occurred when loading data: {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void SaveData()
    {
      try
      {
        _settings.SaveData();
        SaveAddress();
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred when saving data: {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void LoadData()
    {
      try
      {
        LoadAddress();
        _settings.LoadData();
      }
      catch (Exception ex)
      {
        _settings = new ThreadSettings();
        MessageBox.Show("Load data failed", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        _log.Fatal($"An error occurred when loading data: {ex.Message}");
      }
    }
    //----------------------------------------------------------------------------------
    private void UpdateUIByData()
    {
      m_AutoQ.Checked = _settings.Q.Auto;
      m_AutoW.Checked = _settings.W.Auto;
      m_QChk.Checked = _settings.QWE.Q;
      m_WChk.Checked = _settings.QWE.W;
      m_EChk.Checked = _settings.QWE.E;

      m_AutoMouse.Checked = _settings.M.Auto;
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
    private void CloseButton_Click(object sender, EventArgs e)
    {
      m_bForcingClose = true;
      Close();
    }
    //----------------------------------------------------------------------------------
    private void NotifyIcon_Click(object sender, EventArgs e)
    {
      ShowMe(true);
    }
    //----------------------------------------------------------------------------------
    private void OnQChkChanged(object sender, EventArgs e)
    {
      _settings.QWE.Q = m_QChk.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnEChkChanged(object sender, EventArgs e)
    {
      _settings.QWE.E = m_EChk.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnWChkChanged(object sender, EventArgs e)
    {
      _settings.QWE.W = m_WChk.Checked;
    }
    //----------------------------------------------------------------------------------
    private void SettingsButton_Click_1(object sender, EventArgs e)
    {
      _enableStart = false;
      SettingsForm settingsForm = new(_settings);
      if (settingsForm.ShowDialog() == DialogResult.OK)
      {
        _settings.CopyFrom(settingsForm.Settings);
        UpdateUIByData();
      }
      _enableStart = true;
    }
    //----------------------------------------------------------------------------------
    private void AutoMouse_CheckedChanged(object sender, EventArgs e)
    {
      _settings.M.Auto = m_AutoMouse.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnToogleMXClicked(object sender, EventArgs e)
    {
      if (MxSharp.Instance.EnsureAttached())
      {
        var handle = Win32.GetMainWindowHandle((int)MxSharp.Instance.PID());
        if (handle != nint.Zero)
        {
          WindowManipulate.ToggleWindow(handle);
          UpdateToogleButton(handle);
        }
      }
    }
    //----------------------------------------------------------------------------------
    private void OnMinimizeClicked(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
    }
    //----------------------------------------------------------------------------------
    private void UpdateToogleButton(IntPtr handle)
    {
      _ToggleMXButton.Image = WindowManipulate.IsShowing(handle) ? Resources.hide_16 : Resources.window_16;
    }
    //----------------------------------------------------------------------------------
    private void OnInfolicked(object sender, EventArgs e)
    {
      _enableStart = false;
      LicenseForm f = new LicenseForm();
      f.ShowDialog();
      _enableStart = true;
    }
  }
}
