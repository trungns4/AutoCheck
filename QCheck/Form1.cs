using NAudio.MediaFoundation;
using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace QCheck
{
  public partial class Form1 : Form
  {
    private List<Voice> m_Voices;
    private bool m_Playing;

    private WaveOutEvent m_outputDevice;
    private AudioFileReader m_audioFile;

    private Timer m_Timer;
    private bool m_bForcingClose = false;
    private List<string> m_WindowNames = new List<string>();
    private bool m_bAutoHide = true;

    public Form1()
    {
      InitializeComponent();
    }

    //--------------------------------------------------------------------------------------------
    private void OnClosed(object sender, FormClosedEventArgs e)
    {
      m_Timer.Tick -= OnTimer_Tick;
      m_Timer.Stop();
    }
    //--------------------------------------------------------------------------------------------
    private void OnLoad(object sender, EventArgs e)
    {
      var wnames = System.Configuration.ConfigurationManager.AppSettings["windows"];

      m_WindowNames = wnames?.Split("#;".ToCharArray()).ToList();

      LoadFileList();
      ShowFiles();

      m_Timer = new Timer();
      m_Timer.Interval = 500;
      m_Timer.Tick += OnTimer_Tick;
      m_Timer.Start();

      m_NotifyIcon.Visible = true;
      m_ShowMenu.Visible = !this.Visible;
      m_HideMenu.Visible = this.Visible;

      m_CloseMenu.Click += OnCloseMenuClicked;
      m_HideMenu.Click += OnHideMenuClicked;
      m_ShowMenu.Click += OnShowMenuClicked;
    }
    //--------------------------------------------------------------------------------------------
    private void OnShowMenuClicked(object sender, EventArgs e)
    {
      ShowMe(true);
    }
    //--------------------------------------------------------------------------------------------
    private void OnHideMenuClicked(object sender, EventArgs e)
    {
      ShowMe(false);
    }
    //--------------------------------------------------------------------------------------------
    private void OnCloseMenuClicked(object sender, EventArgs e)
    {
      m_bForcingClose = true;
      Close();
    }
    //--------------------------------------------------------------------------------------------
    private void OnTimer_Tick(object sender, EventArgs e)
    {
      CheckWindows();
    }
    //--------------------------------------------------------------------------------------------
    private void OnAutoHideClicked(object sender, EventArgs e)
    {
      m_bAutoHide = !m_bAutoHide;
      m_AutoHideMenu.Checked = m_bAutoHide;
    }
    //--------------------------------------------------------------------------------------------
    private void OnOpenedContextMenu(object sender, EventArgs e)
    {
      m_AutoHideMenu.Checked = m_bAutoHide;
    }
    //--------------------------------------------------------------------------------------------
    private string GetVoicesFolder()
    {
      var exeFolder = System.Reflection.Assembly.GetEntryAssembly().Location;
      return Path.Combine(Directory.GetParent(Path.Combine(exeFolder, "..", "..")).FullName, "voices");
    }
    //--------------------------------------------------------------------------------------------
    private void Play(string fileShortName)
    {
      if (m_Playing == true)
      {
        return;
      }
      var file1 = Path.Combine(GetVoicesFolder(), string.Format("{0}", fileShortName));
      if (File.Exists(file1))
      {
        foreach (DataGridViewRow row in _Grid.Rows)
        {
          var btnCell = row.Cells[_PlayCol.Index] as DataGridViewButtonCell;
          btnCell.Value = ".";
        }

        m_outputDevice = new WaveOutEvent();
        m_audioFile = new AudioFileReader(file1)
        {
          Volume = 0.8f
        };

        m_outputDevice.Init(m_audioFile);
        m_Playing = true;
        m_outputDevice.PlaybackStopped += OutputDevice_PlaybackStopped;
        m_outputDevice.Play();
      }
    }
    //--------------------------------------------------------------------------------------------
    private void OutputDevice_PlaybackStopped(object sender, StoppedEventArgs e)
    {
      m_Playing = false;
      foreach (DataGridViewRow row in _Grid.Rows)
      {
        var btnCell = row.Cells[_PlayCol.Index] as DataGridViewButtonCell;
        btnCell.Value = ">>";
      }
      try
      {
        if (m_outputDevice != null)
        {
          m_outputDevice.Dispose();
          m_audioFile.Dispose();
        }
      }
      catch
      {

      }
    }

    //--------------------------------------------------------------------------------------------
    private void LoadFileList()
    {
      var fileList = Path.Combine(GetVoicesFolder(), string.Format("{0}", "files.json"));
      var fileContent = File.ReadAllText(fileList);
      m_Voices = JsonConvert.DeserializeObject<List<Voice>>(fileContent);
    }
    //--------------------------------------------------------------------------------------------
    private void ShowFiles()
    {
      _Grid.Rows.Clear();

      foreach (var voice in m_Voices.OrderBy(x => x.Order))
      {
        var rowId = _Grid.Rows.Add();
        var row = _Grid.Rows[rowId];

        row.Cells[0].Value = voice.Text;
        row.Cells[1].Value = ">>";
        row.Tag = voice;
      }
    }
    //--------------------------------------------------------------------------------------------
    private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
      var grid = (DataGridView)sender;

      if (e.RowIndex >= 0 && e.ColumnIndex == _PlayCol.Index)
      {
        var voice = grid.Rows[e.RowIndex].Tag as Voice;
        Play(voice.File);
      }
    }
    //--------------------------------------------------------------------------------------------
    private bool FindWindow(string name)
    {
      Process[] processlist = Process.GetProcesses();
      foreach (Process process in processlist)
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
    //--------------------------------------------------------------------------------------------
    private void CheckWindows()
    {
      if (m_bAutoHide == false)
      {
        return;
      }

      foreach (var name in m_WindowNames)
      {
        if (name == "*" || FindWindow(name) == true)
        {
          return;
        }
      }

      ShowMe(false);
    }
    //--------------------------------------------------------------------------------------------
    private void OnClosing(object sender, FormClosingEventArgs e)
    {
      if (m_bForcingClose == false && e.CloseReason == CloseReason.UserClosing)
      {
        m_NotifyIcon.Visible = true;
        ShowMe(false);
        e.Cancel = true;
      }
    }
    //--------------------------------------------------------------------------------------------
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

  }
}
