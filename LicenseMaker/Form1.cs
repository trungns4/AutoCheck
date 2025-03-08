using LicenseMaker;

namespace MXTools
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
      LicenseInfo lic = new LicenseInfo
      {
        User = _UserBox.Text,
        Created = DateTime.Now,
        CreatedBy = "MXTools",
        ExpireDate = _ExpireBox.Value
      };

      SaveFileDialog saveFileDialog = new SaveFileDialog
      {
        Filter = "License Files (*.lic)|*.lic",
        Title = "Save License File",
        DefaultExt = "lic",
        AddExtension = true,
        OverwritePrompt = true
      };

      if (saveFileDialog.ShowDialog() != DialogResult.OK)
        return;

      LicenseMaker maker = new LicenseMaker(Resources.private_key);
      maker.CreateLicense(lic, saveFileDialog.FileName);
    }

    private void OnLoadClicked(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog
      {
        Filter = "License Files (*.lic)|*.lic",
        Title = "Select a License File",
        CheckFileExists = true,
        Multiselect = false
      };

      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
      }
    }
  }
}
