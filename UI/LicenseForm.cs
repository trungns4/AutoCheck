using MXTools.License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools.UI
{
  public partial class LicenseForm : Form
  {
    public LicenseForm()
    {
      InitializeComponent();
    }

    private void OnOKClicked(object sender, EventArgs e)
    {
      DialogResult = DialogResult.OK;
    }

    private void LicenseForm_Load(object sender, EventArgs e)
    {
      var lic = AppLicense.Instance.GetLicense();

      if (lic != null)
      {
        string frm = "dd/MM/yyyy";
        _UserBox.Text = lic.User;
        _CreatedBox.Text = lic.Created.ToString(frm);
        _ByBox.Text = lic.CreatedBy;
        _ExpireBox.Text = lic.ExpireDate.ToString(frm);
      }
    }
  }
}
