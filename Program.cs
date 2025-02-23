using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace MXTools
{
  internal static class Program
  {
    static Mutex mutex = new Mutex(true, "_AUTOCHECK_");



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

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
      }
      else
      {
        // Bring existing instance to the foreground
        IntPtr hWnd = Utils.FindWindow("MXTools"); 
        if (hWnd != IntPtr.Zero)
        {
          Utils.ShowWindow(hWnd);
          Utils.SetForegroundWindow(hWnd);
        }
      }
    }
  }
}
