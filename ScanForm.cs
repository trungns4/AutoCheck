using Binarysharp.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoCheck
{
  public partial class ScanForm : Form
  {

    private long _curAddr = 0;
    private long _maxAddr = 0;

    public long CurAddr => _curAddr;
    public long MaxAddr => _maxAddr;

    public ScanForm()
    {
      InitializeComponent();
    }

    private void m_OKButton_Click(object sender, EventArgs e)
    {
      List<long> values = new List<long>();
      foreach (DataGridViewRow row in m_Grid.Rows)
      {
        DataGridViewCheckBoxCell cell = row.Cells[m_CheckCol.Index] as DataGridViewCheckBoxCell;
        if (cell.Value != null && (bool)(cell.Value) == true)
        {
          long adr = (long)row.Tag;
          values.Add(adr);
        }
      }

      if (values.Count == 2)
      {
        _curAddr = values.Min();
        _maxAddr = values.Max();
        DialogResult = DialogResult.OK;
      }
      else
      {
        MessageBox.Show("Please select 2 rows");
      }
    }

    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
    }

    private void EnableButtons(bool enable)
    {
      var buttons = new Button[] { m_ScanButton, m_ReScanButton, m_OKButton, m_CancelButton };

      foreach (Button b in buttons)
      {
        b.Enabled = enable;
      }
    }

    private void m_ScanButton_Click(object sender, EventArgs e)
    {
      try
      {
        EnableButtons(false);
        m_Grid.Rows.Clear();

        if (false == int.TryParse(m_NumberBox.Text, out int val))
        {
          MessageBox.Show("Please enter a number");
          return;
        }

        using (MemorySharp sharp = Utils.CreateMemorySharp())
        {
          if (sharp == null)
          {
            MessageBox.Show("Could not find the window");
            return;
          }
          foreach (var region in sharp.Memory.Regions)
          {
            try
            {
              try
              {
                // Read memory as byte array
                byte[] buffer = sharp.Read<byte>(region.BaseAddress, region.Information.RegionSize, false);
                // Scan for the integer
                for (int i = 0; i < buffer.Length - 3; i++)
                {
                  int foundValue = BitConverter.ToInt32(buffer, i);
                  if (foundValue == val)
                  {
                    var ir = m_Grid.Rows.Add();
                    var row = m_Grid.Rows[ir];

                    var adr = region.BaseAddress.ToInt64() + i;

                    row.Cells[m_AddressCol.Index].Value = adr.ToString("X");
                    row.Cells[m_ValueCol.Index].Value = val.ToString();
                    row.Tag = adr;
                  }
                }
              }
              catch
              {

              }
            }
            catch
            {
            }
          }
        }
      }
      finally
      {
        EnableButtons(true);
      }
    }

    private void m_ReScanButton_Click(object sender, EventArgs e)
    {
      try
      {
        EnableButtons(false);

        using (MemorySharp sharp = Utils.CreateMemorySharp())
        {
          if (sharp == null)
          {
            MessageBox.Show("Could not find the window");
            return;
          }

          foreach (DataGridViewRow row in m_Grid.Rows)
          {
            long adr = (long)row.Tag;
            try
            {
              var val = sharp.Read<int>((IntPtr)adr, false);
              row.Cells[m_ValueCol.Index].Value = val.ToString();
            }
            catch
            {
              row.Cells[m_ValueCol.Index].Value = "x";
            }
          }
        }
      }
      finally
      {
        EnableButtons(true);
      }
    }

    private void ScanForm_Load(object sender, EventArgs e)
    {

    }

    private void button1_Click(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in m_Grid.Rows)
      {
        if (row.Cells[m_ValueCol.Index].Value != null && (string)(row.Cells[m_ValueCol.Index].Value) == m_NumberBox.Text)
        {
          row.Visible = false;
        }
      }
    }

    private void button2_Click(object sender, EventArgs e)
    {
      foreach (DataGridViewRow row in m_Grid.Rows)
      {
        row.Visible = true;
      }
    }

    private void OnAuto(object sender, EventArgs e)
    {
      if (m_Grid.Rows.Count < 2)
      {
        return;
      }

      foreach (DataGridViewRow row in m_Grid.Rows)
      {
        row.Visible = false;
      }

      for (int i = 0; i < m_Grid.Rows.Count - 1; i++)
      {
        long adr1 = (long)m_Grid.Rows[i].Tag;
        long adr2 = (long)m_Grid.Rows[i + 1].Tag;

        if ((adr2 - adr1) == 16)
        {
          m_Grid.Rows[i].Visible = true;
          m_Grid.Rows[i + 1].Visible = true;
        }
      }
    }
  }
}
