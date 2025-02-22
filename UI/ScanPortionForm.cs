using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoCheck.Properties;

namespace AutoCheck
{
  public partial class ScanPortionForm : Form
  {
    public ScanPortionForm()
    {
      InitializeComponent();
    }

    private void _ScanButton_Click(object sender, EventArgs e)
    {
      AutoScanPortion asp = new AutoScanPortion();

      Task.Run(() => asp.Scan((int)(_PortionBox.Value),
        step =>
        {
          _AddressBox.BeginInvoke(new Action(() => { _AddressBox.Text = step.ToString(); }));
        }
      ));
    }
  }
}
