using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QCheck
{

  internal static class Program
  {
    [DllImport("user32.dll", SetLastError = true)]
    static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);


    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


    static Mutex mutex = new Mutex(true, "_QCHECK_");

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      if (mutex.WaitOne(TimeSpan.Zero, true))
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());

        // release mutex after the form is closed.
        mutex.ReleaseMutex();
        mutex.Dispose();
      }
      else
      {
        //var appName = System.Reflection.Assembly.GetExecutingAssembly().Location.ToUpper();

        //Process[] processlist = Process.GetProcesses();
        //foreach (Process process in processlist)
        //{
        //  try
        //  {
        //    string fullPath = process.MainModule?.FileName;
        //    if (fullPath?.ToUpper() == appName)
        //    {
        //      var style = GetWindowLong(process.MainWindowHandle, -16);
        //      if ((style & 0x10000000) != 0)
        //      {
        //        ShowWindow(process.MainWindowHandle, 0x06);
        //        break;
        //      }
        //    }
        //  }
        //  catch
        //  {

        //  }
        //}
      }
    }
  }
}
