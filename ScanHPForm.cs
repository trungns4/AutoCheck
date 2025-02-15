using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using Newtonsoft.Json;

namespace AutoCheck
{
  public partial class ScanHPForm : Form
  {
    private bool _scanning = false;
    private bool _stop = false;
    private int _offset = 50;

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
    private void _StopButton_Click(object sender, EventArgs e)
    {
      _stop = true;
    }
    //--------------------------------------------------------------------------------------------
    private async void _Scan_Click(object sender, EventArgs e)
    {
      await Task.Run(() => Scan());
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
      _offset = 1;
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
            _offset = Math.Min(99, Math.Max(1, number));
          }
        }
      }
      m_OffsetBox.Value = _offset;
    }
    //--------------------------------------------------------------------------------------------
    private long TotalBytes(MemorySharp sharp, out List<RemoteRegion> errRegions)
    {
      long totalBytes = 0;
      errRegions = new List<RemoteRegion>();
      foreach (var region in sharp.Memory.Regions)
      {
        try
        {
          var rs = region.Information.RegionSize;
          totalBytes += rs;
        }
        catch
        {
          errRegions.Add(region);
        }
      }
      return totalBytes;
    }
    //--------------------------------------------------------------------------------------------
    private int Find(byte[] buffer, int number)
    {
      if (buffer.Length < 28)
      {
        return -1;
      }

      for (int i = 0; i <= buffer.Length - 28; i++)
      {
        if (_stop)
        {
          return -1;
        }

        int a = BitConverter.ToInt32(buffer, i);
        int b = BitConverter.ToInt32(buffer, i + 8);
        int c = BitConverter.ToInt32(buffer, i + 16);
        int d = BitConverter.ToInt32(buffer, i + 24);

        if (a == c && a == number && b == d)
        {
          return i;
        }
      }
      return -1;
    }

    //--------------------------------------------------------------------------------------------
    protected void Scan()
    {
      if (_scanning)
      {
        return;
      }

      var buttons = new Button[] { _OKButton, _CancelButton, _ScanButton };

      foreach (var item in buttons)
      {
        item.BeginInvoke(new Action(() => item.Enabled = false));
      }

      _stop = false;

      try
      {
        _scanning = true;
        if (false == int.TryParse(_InputBox.Text, out int val))
        {
          MessageBox.Show("Please enter HP");
          return;
        }

        if (false == int.TryParse(m_OffsetBox.Text, out int offset) || offset <= 0 || offset >= 100)
        {
          MessageBox.Show("Please enter offset");
          return;
        }

        MemorySharp sharp = Utils.CreateMemorySharp();
        if (sharp == null)
        {
          MessageBox.Show("Could not find the window");
          return;
        }

        long total = TotalBytes(sharp, out List<RemoteRegion> errRegions);

        _ProgBar.BeginInvoke(new Action(() => _ProgBar.Visible = true));
        _ProgBar.BeginInvoke(new Action(() => _ProgBar.Minimum = 0));
        _ProgBar.BeginInvoke(new Action(() => _ProgBar.Maximum = (int)(total + 1)));
        _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = "Scanning..."));

        int current = 0;
        double percelOffset = offset;
        foreach (var region in sharp.Memory.Regions)
        {
          try
          {
            if (errRegions.Contains(region))
            {
              continue;
            }

            var rs = region.Information.RegionSize;

            if (((double)current) < (double)(total * percelOffset / 100.0))
            {
              current += rs;
              _ProgBar.BeginInvoke(new Action(() => _ProgBar.Value = Math.Min(current, _ProgBar.Maximum)));
              continue;
            }

            byte[] buffer = sharp.Read<byte>(region.BaseAddress, rs, false);
            _ProgBar.BeginInvoke(new Action(() => _ProgBar.Value = Math.Min(current, _ProgBar.Maximum)));
            Thread.Sleep(10);

            current += rs;
            if (_stop)
            {
              _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = "Not found"));
              return;
            }

            int adr = Find(buffer, val);
            if (adr >= 0)
            {
              _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = (region.BaseAddress.ToInt64() + adr).ToString("X")));
              _offset = (int)((double)(current) / (double)(total) * 100) - 10;
              _offset = Math.Min(Math.Max(0, _offset), 99);
              m_OffsetBox.Value = _offset;
              return;
            }
          }
          catch { }
        }

        _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = "Not Found"));
      }
      finally
      {
        _ProgBar.BeginInvoke(new Action(() => _ProgBar.Visible = false));
        _scanning = false;
        foreach (var item in buttons)
        {
          item.BeginInvoke(new Action(() => item.Enabled = true));
        }
      }
    }
  }
}
