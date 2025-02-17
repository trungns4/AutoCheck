using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace AutoCheck
{
  internal static class Program
  {
    static Mutex mutex = new Mutex(true, "_AUTOCHECK_");

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    private const int SW_RESTORE = 9;

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
        IntPtr hWnd = FindWindow(null, "AutoCheck"); 
        if (hWnd != IntPtr.Zero)
        {
          ShowWindow(hWnd, SW_RESTORE);
          SetForegroundWindow(hWnd);
        }
      }
    }
  }
}
