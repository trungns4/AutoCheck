using LicenseMaker;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MXTools
{
  internal static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      // If there are arguments, run in silent mode
      if (args.Length > 0)
      {
        RunSilently(args);
        return;
      }

      // Otherwise, start the GUI
      ApplicationConfiguration.Initialize();
      Application.Run(new Form1());
    }

    static void RunSilently(string[] args)
    {
      if (args.Length <= 0)
      {
        return;
      }

      LicenseInfo lic = new LicenseInfo
      {
        User = "dev",
        Created = DateTime.Now,
        CreatedBy = "MXTools",
        ExpireDate = DateTime.Now.AddDays(10),
        Note = "For development only. Contact hmh3ba@gmail.com for more information."
      };


      LicenseMaker maker = new LicenseMaker(Resources.private_key);
      maker.CreateLicense(lic, args[0]);
      Console.WriteLine(@"Create license file {args[0]}");
    }
  }
}
