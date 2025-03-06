using log4net;
using MXTools.Helpers;
using MXTools.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools
{
  public partial class ScanHPForm : Form
  {
    private bool _scanning = false;
    public ScanHPForm()
    {
      InitializeComponent();
    }

    //--------------------------------------------------------------------------------------------
    private void ScanHPForm_Load(object sender, EventArgs e)
    {
      LoadData();
    }
    //--------------------------------------------------------------------------------------------
    private void ScanHPForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      SaveData();
    }
    //--------------------------------------------------------------------------------------------
    private void ScanHPForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_scanning)
      {
        e.Cancel = true;
      }
    }
    //--------------------------------------------------------------------------------------------
    private void Scan_Click(object sender, EventArgs e)
    {
      if (_scanning)
      {
        return;
      }
      Scan();
    }
    //--------------------------------------------------------------------------------------------
    private void OKButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }
    //--------------------------------------------------------------------------------------------
    private void CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }
    //--------------------------------------------------------------------------------------------
    public ulong GetAddress()
    {
      if (ulong.TryParse(_AdrBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong number))
      {
        return number;
      }
      return 0;
    }
    //--------------------------------------------------------------------------------------------
    private static string GetDataFile()
    {
      string exeDirectory = Path.GetDirectoryName(Environment.ProcessPath);
      return Path.Combine(exeDirectory, "hp.json");
    }
    //--------------------------------------------------------------------------------------------
    private void SaveData()
    {
      string file = GetDataFile();
      var data = new Dictionary<string, string>
      {
        { "HP", _InputBox.Text },
        { "OFFSET", m_OffsetBox.Value.ToString() }
      };

      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      File.WriteAllText(file, json);
    }
    //--------------------------------------------------------------------------------------------
    private void LoadData()
    {
      var offset = 1;
      _InputBox.Text = "";
      string file = GetDataFile();
      if (File.Exists(file) == true)
      {
        string json = File.ReadAllText(file);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        if (data.TryGetValue("HP", out string value))
        {
          _InputBox.Text = value;
        }
        if (data.ContainsKey("OFFSET"))
        {
          if (int.TryParse(data["OFFSET"], out int number))
          {
            offset = Utils.Clamp<int>(number, 0, 99);
          }
        }
      }
      m_OffsetBox.Value = Utils.Clamp<decimal>(offset, 0, 99);
    }
    //--------------------------------------------------------------------------------------------
    private void OnStart(double total)
    {
      var buttons = new Button[] { _OKButton, _CancelButton, _ScanButton };
      foreach (var item in buttons)
      {
        item.Enabled = false;
      }

      _StopButton.Enabled = true;

      _ProgBar.Visible = true;
      _ProgBar.Minimum = 0;
      _ProgBar.Maximum = (int)(total);
    }
    //--------------------------------------------------------------------------------------------
    private void OnProgress(double progress)
    {
      int val = Math.Min((int)progress, _ProgBar.Maximum);
      if (val != _ProgBar.Value)
      {
        _ProgBar.Value = val;
      }
      _AdrBox.Text = $"{_ProgBar.Value}%";
    }
    //--------------------------------------------------------------------------------------------
    private void OnDone(ulong adr, int ofs, EventHandler handler)
    {
      if (adr > 0 & ofs >= 0)
      {
        _AdrBox.Text = adr.ToString("X");
        m_OffsetBox.Text = ofs.ToString();
      }
      else
      {
        _AdrBox.Text = "Not found";
      }
      _ProgBar.Visible = false;

      var buttons = new Button[] { _OKButton, _CancelButton, _ScanButton };
      foreach (var item in buttons)
      {
        item.Enabled = true;
      }
      _StopButton.Enabled = false;
      _StopButton.Click -= handler;
      _scanning = false;
    }
    //--------------------------------------------------------------------------------------------
    private void Scan()
    {
      if (_scanning)
      {
        return;
      }

      try
      {
        if (MxSharp.Instance.EnsureAttached() == false)
        {
          MessageBox.Show("App is not running", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        _scanning = true;
        if (false == int.TryParse(_InputBox.Text, out int val))
        {
          MessageBox.Show("Please enter HP", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        if (false == int.TryParse(m_OffsetBox.Text, out int offset))
        {
          MessageBox.Show("Please enter offset", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }

        CancellationTokenSource cts = new();
        void stopButtonClickHandler(object sender, EventArgs e)
        {
          cts.Cancel();
        }
        _StopButton.Click += stopButtonClickHandler;

        Task.Run(() =>
        {
          MxSharp.Instance.ScanMemory(val, offset,
          total =>
          {
            BeginInvoke(() => OnStart(total));
          },
          progress =>
          {
            BeginInvoke(() => OnProgress(progress));
            return (cts.Token.IsCancellationRequested == false);
          },
          (adr, ofs) =>
          {
            BeginInvoke(() => OnDone(adr, ofs, stopButtonClickHandler));
          },
          cts.Token);
        });
      }
      finally
      {
      }
    }
    //--------------------------------------------------------------------------------------------
  }
}
