using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MXTools.Properties;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using Newtonsoft.Json;
using WindowsInput;

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
    private void _Scan_Click(object sender, EventArgs e)
    {
      Scan();
    }
    //--------------------------------------------------------------------------------------------
    private void _OKButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }
    //--------------------------------------------------------------------------------------------
    private void _CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }
    //--------------------------------------------------------------------------------------------
    public long GetAddress()
    {
      if (long.TryParse(_AdrBox.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out long number))
      {
        return number;
      }
      return 0;
    }
    //--------------------------------------------------------------------------------------------
    private string GetDataFile()
    {
      string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(exeDirectory, "hp.json");
    }
    //--------------------------------------------------------------------------------------------
    private void SaveData()
    {
      string file = GetDataFile();
      var data = new Dictionary<string, string>();

      data.Add("HP", _InputBox.Text);
      data.Add("OFFSET", m_OffsetBox.Value.ToString());

      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      File.WriteAllText(file, json);
    }
    //--------------------------------------------------------------------------------------------
    private void LoadData()
    {
      int maxOffset = 9999;
      MemorySharp sharp = Utils.CreateMemorySharp();
      if (sharp != null)
      {
        maxOffset = (int)(sharp.Memory.Regions.Count() - 1);
      }

      var offset = 1;
      _InputBox.Text = "";
      string file = GetDataFile();
      if (File.Exists(file) == true)
      {
        string json = File.ReadAllText(file);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        if (data.ContainsKey("HP"))
        {
          _InputBox.Text = data["HP"];
        }
        if (data.ContainsKey("OFFSET"))
        {
          if (int.TryParse(data["OFFSET"], out int number))
          {
            offset = Math.Min(maxOffset, Math.Max(0, number));
          }
        }
      }
      m_OffsetBox.Value = offset;
    }
    //--------------------------------------------------------------------------------------------
    private void OnStart(long total)
    {
      var buttons = new Button[] { _OKButton, _CancelButton, _ScanButton };
      foreach (var item in buttons)
      {
        item.Enabled = false;
      }

      _ProgBar.Visible = true;
      _ProgBar.Minimum = 0;
      _ProgBar.Maximum = (int)(total + 1);
    }
    //--------------------------------------------------------------------------------------------
    private void OnProgress(long progress)
    {
      _ProgBar.Value = Math.Min((int)progress, _ProgBar.Maximum);
      _AdrBox.Text = "Scanning...";
      _ProgBar.Refresh();
    }
    //--------------------------------------------------------------------------------------------
    private void OnDone(long adr, int ofs, EventHandler handler)
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
        _scanning = true;
        if (false == int.TryParse(_InputBox.Text, out int val))
        {
          MessageBox.Show("Please enter HP", Resources.MsgBoxCaption);
          return;
        }

        if (false == int.TryParse(m_OffsetBox.Text, out int offset))
        {
          MessageBox.Show("Please enter offset", Resources.MsgBoxCaption);
          return;
        }

        ScanHP scan = new ScanHP();
        EventHandler stopButtonClickHandler = (sender, e) =>
        {
          scan.Stop();
        };
        _StopButton.Click += stopButtonClickHandler;

        scan.Scan(val, offset,
        total =>
        {
          BeginInvoke(() => OnStart(total));
        },
        progress =>
        {
          BeginInvoke(() => OnProgress(progress));
        },
        (adr, ofs) =>
        {
          BeginInvoke(() => OnDone(adr, ofs, stopButtonClickHandler));
        });
      }
      finally
      {
      }
    }
  }
}
