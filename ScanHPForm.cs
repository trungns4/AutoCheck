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
using Newtonsoft.Json;

namespace AutoCheck
{
  public partial class ScanHPForm : Form
  {
    private bool _scanning = false;
    private bool _stop = false;
    public ScanHPForm()
    {
      InitializeComponent();
    }

    public long GetAddress()
    {
      if (string.IsNullOrEmpty(_AdrBox.Text))
      {
        return 0;
      }

      if (long.TryParse(_AdrBox.Text, NumberStyles.HexNumber,
                            CultureInfo.InvariantCulture, out long number))
      {
        return number;
      }

      return 0;
    }

    private string GetDataFile()
    {
      string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(exeDirectory, "hp.json");
    }

    private void SaveData()
    {
      string file = GetDataFile();
      var data = new Dictionary<string, string>();

      data.Add("HP", _InputBox.Text);

      string json = JsonConvert.SerializeObject(data, Formatting.Indented);
      File.WriteAllText(file, json);
    }

    private void LoadData()
    {
      string file = GetDataFile();
      if (File.Exists(file) == true)
      {
        string json = File.ReadAllText(file);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        if (data.ContainsKey("HP"))
        {
          _InputBox.Text = data["HP"];
        }
      }
    }

    private async void _Scan_Click(object sender, EventArgs e)
    {
      await Task.Run(() => Scan());
    }

    private void _OKButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }

    private void _CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }

    private long TotalBytes(MemorySharp sharp)
    {
      long totalBytes = 0;
      foreach (var region in sharp.Memory.Regions)
      {
        try
        {
          totalBytes += region.Information.RegionSize;
        }
        catch { }
      }
      return totalBytes;
    }

    private int Find(byte[] buffer, int number)
    {
      if (buffer.Length < 28) // Ensure enough bytes for d
      {
        return -1;
      }

      for (int i = 0; i <= buffer.Length - 28; i++) // Ensure safe access for d
      {
        if (_stop) // Ensure _stop is thread-safe if used in multithreading
        {
          return -1;
        }

        int a = BitConverter.ToInt32(buffer, i);
        int b = BitConverter.ToInt32(buffer, i + 8);
        int c = BitConverter.ToInt32(buffer, i + 16);
        int d = BitConverter.ToInt32(buffer, i + 24);

        if (a == c && a == number && b == d) // Simplified condition
        {
          return i;
        }
      }
      return -1;
    }


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
          MessageBox.Show("Please enter a number");
          return;
        }

        MemorySharp sharp = Utils.CreateMemorySharp();
        if (sharp == null)
        {
          MessageBox.Show("Could not find the window");
          return;
        }

        _ProgBar.BeginInvoke(new Action(() => _ProgBar.Visible = true));
        _ProgBar.BeginInvoke(new Action(() => _ProgBar.Maximum = (int)(TotalBytes(sharp))));
        _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = ""));

        foreach (var region in sharp.Memory.Regions)
        {
          try
          {
            byte[] buffer = sharp.Read<byte>(region.BaseAddress, region.Information.RegionSize, false);
            _ProgBar.BeginInvoke(new Action(() => _ProgBar.Value += region.Information.RegionSize));

            Thread.Sleep(20);

            if (_stop)
            {
              _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = "Not found"));
              return;
            }

            int adr = Find(buffer, val);
            if (adr >= 0)
            {
              _AdrBox.BeginInvoke(new Action(() => _AdrBox.Text = (region.BaseAddress.ToInt64() + adr).ToString("X")));
              return;
            }
          }
          catch { }
        }
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

    private void ScanHPForm_Load(object sender, EventArgs e)
    {
      LoadData();
    }

    private void ScanHPForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      SaveData();
    }

    private void ScanHPForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (_scanning)
      {
        e.Cancel = true;
      }
    }

    private void _StopButton_Click(object sender, EventArgs e)
    {
      _stop = true;
    }
  }
}
