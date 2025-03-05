using log4net;
using log4net.Config;
using MXTools.Helpers;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace MXTools
{
  internal static class Program
  {
    static Mutex mutex = new Mutex(true, "__MXTOOL__");

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      if (mutex.WaitOne(TimeSpan.Zero, true))
      {
        // Initialize log4net from App.config
        XmlConfigurator.Configure();

        ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        log.Debug("Application started");

        //Utils.ProtectProcess();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
      }
      else
      {
        string exeName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
        IntPtr hWnd = Utils.FindWindowByExeName(exeName);
        if (hWnd != IntPtr.Zero)
        {
          Utils.ShowWindow(hWnd);
          Utils.SetForegroundWindow(hWnd);
        }
      }
    }
  }
}
