using System;
using System.Threading;
using System.Windows.Forms;

namespace AutoCheck
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
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
      }
    }
  }
}
