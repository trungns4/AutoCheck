using MXTools.Properties;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using Binarysharp.MemoryManagement.Modules;
using Binarysharp.MemoryManagement.Native;
using Gma.System.MouseKeyHook;
using NAudio.Gui;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using log4net;
using WindowsInput.Native;
using System.Collections;

namespace MXTools
{
  public partial class Form1 : Form
  {
    private MemorySharp _sharp = null;

    private long _addr = 0;

    private AutoKeyThread _threadQ = null;
    private AutoKeyThread _threadW = null;
    private AutoQWEThread _qweThread = null;
    private AutoMouseThread _mThread = null;
    private TimeWarning _timerWarning = null;
    private bool _starting = false;

    private bool m_bForcingClose = false;

    private IKeyboardMouseEvents m_GlobalHook;
    private Settings _settings = new Settings();
    private bool _enableStart = true;

    public Form1()
    {
      InitializeComponent();
    }

    private void OnFormLoad(object sender, EventArgs e)
    {
      LoadData();

      _About.Text = this.GetType().Assembly.GetName().Version.ToString() + " © by Alex";
      _threadQ = new AutoKeyThread('q', _settings.Q, (current, max) =>
      {
        BeginInvoke(() =>
        {
          m_QPBar.Maximum = max;
          m_QPBar.Minimum = 0;
          m_QPBar.Value = Math.Min(current, max);
        });
      });
      _threadW = new AutoKeyThread('w', _settings.W, (current, max) =>
      {
        BeginInvoke(() =>
        {
          m_WPBar.Maximum = max;
          m_WPBar.Minimum = 0;
          m_WPBar.Value = Math.Min(current, max);
        });
      });

      _qweThread = new AutoQWEThread(_settings.QWE, (count) =>
      {
        BeginInvoke(() => m_KeyCount.Text = count.ToString());
      });

      _mThread = new AutoMouseThread(_settings.M);
      _timerWarning = new TimeWarning(_settings.T);

      _timerWarning.Update = (rmain) =>
      {
        var ts = TimeSpan.FromSeconds(rmain);
        BeginInvoke(new Action(() => { _WarnTime.Text = $"{ts.Minutes:D1}:{ts.Seconds:D2}"; }));
      };

      UpdateUIByData();

      m_GlobalHook = Hook.GlobalEvents();
      m_GlobalHook.KeyUp += OnKeyUp;
      m_GlobalHook.KeyDown += OnKeyDown;

      m_NotifyIcon.Visible = true;
      m_ShowMenu.Visible = !this.Visible;
      m_HideMenu.Visible = this.Visible;

      m_CloseMenu.Click += OnCloseMenu_Click;
      m_HideMenu.Click += OnHideMenu_Click;
      m_ShowMenu.Click += OnShowMenu_Click;
      m_StartMenu.Click += OnStartMenuClick;
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
        if ((e.KeyCode == System.Windows.Forms.Keys.D0 ||
        e.KeyCode == System.Windows.Forms.Keys.Divide) && e.Control)
        {
          ToggleStartStop();
        }
        else if (e.KeyCode == System.Windows.Forms.Keys.A && e.Control)
        {
          _settings.M._auto = !_settings.M._auto;
          m_AutoMouse.BeginInvoke((System.Windows.Forms.MethodInvoker)(() => m_AutoMouse.Checked = _settings.M._auto));
        }
        else if (e.KeyCode == System.Windows.Forms.Keys.Multiply && e.Control)
        {
          var sharp = Utils.CreateMemorySharp();
          if (sharp != null && sharp.Windows.MainWindow != null)
          {
            WindowHider.ShowWindow(sharp.Windows.MainWindow.Handle);
          }
        }
        else if (e.KeyCode == System.Windows.Forms.Keys.Oem3 && e.Control)
        {
          var sharp = Utils.CreateMemorySharp();
          if (sharp != null && sharp.Windows.MainWindow != null)
          {
            WindowHider.HideWindow(sharp.Windows.MainWindow.Handle);
          }
        }
      }
      finally
      {
      }
    }
    //----------------------------------------------------------------------------------
    private void OnStartMenuClick(object sender, EventArgs e)
    {
      ToggleStartStop();
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

      m_NotifyIcon.Dispose();
    }
    //----------------------------------------------------------------------------------
    private void OnScanClicked(object sender, EventArgs e)
    {
      _enableStart = false;
      ScanHPForm f = new ScanHPForm();
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
      _settings.W._auto = m_AutoW.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnAutoQCheckedChanged(object sender, EventArgs e)
    {
      _settings.Q._auto = m_AutoQ.Checked;
    }
    //----------------------------------------------------------------------------------
    private bool Start()
    {
      Utils.CloseApps();
      _sharp = Utils.CreateMemorySharp();
      if (_sharp == null)
      {
        MessageBox.Show("Could not read the memory", Resources.MsgBoxCaption);
        return false;
      }

      //m_StartButton.Text = "Stop";
      m_StartButton.Image = Resources.stop_16;
      m_ScanButton.Enabled = false;

      m_StartMenu.Text = "Stop";
      _SettingsButton.Enabled = false;

      Thread.Sleep(50);
      return true;
    }
    //----------------------------------------------------------------------------------
    private bool Stop()
    {
      //m_StartButton.Text = "Start";
      m_StartButton.Image = Resources.play_16;
      m_ScanButton.Enabled = true;

      m_StartMenu.Text = "Start";
      _SettingsButton.Enabled = true;

      _sharp?.Dispose();
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
        if (_threadQ.IsRunning())
        {
          _threadQ.Stop();
          _threadW.Stop();
          _qweThread.Stop();
          _mThread.Stop();
          _timerWarning.Stop();
          Stop();
        }
        else
        {
          if (Start())
          {
            _threadQ.Start(_sharp, _addr, _addr + 16);
            _threadW.Start(_sharp, _addr + 8, _addr + 24);
            _qweThread.Start(_sharp);
            _mThread.Start(_sharp);
            _timerWarning.Start();
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
    private string GetAddressFile()
    {
      string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(exeDirectory, "addr.json");
    }
    //----------------------------------------------------------------------------------
    private void SaveAddress()
    {
      string file = GetAddressFile();
      var data = new Dictionary<string, long>
      {
        { "addr", _addr }
      };
      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      File.WriteAllText(file, json);
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
          var data = JsonConvert.DeserializeObject<Dictionary<string, long>>(json);
          _addr = data["addr"];
        }
      }
      catch
      {
        _addr = 0;
      }
    }
    //----------------------------------------------------------------------------------
    private void SaveData()
    {
      _settings.SaveData();
      SaveAddress();
    }
    //----------------------------------------------------------------------------------
    private void LoadData()
    {
      try
      {
        LoadAddress();
        _settings.LoadData();
      }
      catch
      {
        _settings = new Settings();
        MessageBox.Show("Load data failed", Resources.MsgBoxCaption);
      }
    }
    //----------------------------------------------------------------------------------
    private void UpdateUIByData()
    {
      m_AutoQ.Checked = _settings.Q._auto;
      m_AutoW.Checked = _settings.W._auto;
      m_QChk.Checked = _settings.QWE._q;
      m_WChk.Checked = _settings.QWE._w;
      m_EChk.Checked = _settings.QWE._e;

      m_VolumeCtrl.Minimum = 0;
      m_VolumeCtrl.Maximum = 100;
      m_VolumeCtrl.Value = (int)(Math.Min(_settings.Q._warnVolume, 1.0f) * 100f);

      m_AutoMouse.Checked = _settings.M._auto;
    }
    //----------------------------------------------------------------------------------
    private void OnVolumeValueChanged(object sender, EventArgs e)
    {
      _settings.Q._warnVolume = (float)m_VolumeCtrl.Value / (float)m_VolumeCtrl.Maximum;
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
    //----------------------------------------------------------------------------------
    private void OnQChkChanged(object sender, EventArgs e)
    {
      _settings.QWE._q = m_QChk.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnEChkChanged(object sender, EventArgs e)
    {
      _settings.QWE._e = m_EChk.Checked;
    }
    //----------------------------------------------------------------------------------
    private void OnWChkChanged(object sender, EventArgs e)
    {
      _settings.QWE._w = m_WChk.Checked;
    }
    //----------------------------------------------------------------------------------
    private void _SettingsButton_Click_1(object sender, EventArgs e)
    {
      _enableStart = false;
      SettingsForm settingsForm = new SettingsForm(_settings);
      if (settingsForm.ShowDialog() == DialogResult.OK)
      {
        _settings.CopyFrom(settingsForm.Settings);
        UpdateUIByData();
      }
      _enableStart = true;
    }
    //----------------------------------------------------------------------------------
    private void m_AutoMouse_CheckedChanged(object sender, EventArgs e)
    {
      _settings.M._auto = m_AutoMouse.Checked;
    }
  }
}
