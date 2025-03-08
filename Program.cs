using log4net;
using log4net.Config;
using MXTools.Helpers;
using MXTools.License;
using MXTools.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace MXTools
{
  internal static class Program
  {
    private static readonly Mutex mutex = new(true, "__MXTOOL__");

    static bool IsRunningAsAdmin()
    {
      using var identity = WindowsIdentity.GetCurrent();
      var principal = new WindowsPrincipal(identity);
      return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      if (mutex.WaitOne(TimeSpan.Zero, true))
      {
        if (IsRunningAsAdmin() == false)
        {
          MessageBox.Show("Please Run As Administrator",
              Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }


        // Initialize log4net from App.config
        XmlConfigurator.Configure();

        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        log.Info(Resources.AppStart);

        long ticks = 0;
        if (AppLicense.Instance.ReadLicense(ref ticks) == false)
        {
          MessageBox.Show(Resources.LicenseExpired,
              Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

          log.Fatal(Resources.LicenseExpired);
          return;
        }

        var date = new DateTime(ticks);
        if (date < DateTime.Now)
        {
          MessageBox.Show(Resources.LicenseExpired,
                Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);

          log.Fatal(Resources.LicenseExpired);
          return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
      }
      else
      {
        string exeName = System.IO.Path.GetFileName(Environment.ProcessPath);
        IntPtr hWnd = Win32.FindWindowByExeName(exeName);
        if (hWnd != IntPtr.Zero)
        {
          Win32.ShowWindow(hWnd);
          Win32.SetForegroundWindow(hWnd);
        }
      }
    }
  }
}
