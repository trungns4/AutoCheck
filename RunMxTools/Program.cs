using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Security.Principal;
using System.Windows.Forms;

namespace RunMxTools
{
  class Program
  {
    static bool IsRunningAsAdmin()
    {
      var identity = WindowsIdentity.GetCurrent();
      var principal = new WindowsPrincipal(identity);
      return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    static string GetAppPath()
    {
      var thisPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(thisPath, @"..\..\..\bin\x64\Release\net8.0-windows\MxTools.exe");
    }

    static string GetKDUPath()
    {
      var thisPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(thisPath, @"..\..\..\Loader\kdu.exe");
    }

    static void StartKdu()
    {
      if (IsRunningAsAdmin() == false)
      {
        MessageBox.Show("Please Run As Administrator",
            "Run", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      ProcessStartInfo psi = new ProcessStartInfo
      {
        FileName = GetKDUPath(),
        Arguments = "-prv 1 -pse \"" + GetAppPath() + "\"",
        UseShellExecute = false,  // Run as a normal process
        CreateNoWindow = true,    // Prevents a new window
        WindowStyle = ProcessWindowStyle.Hidden // Fully hides it
      };

      try
      {
        Process.Start(psi);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Error starting process: " + ex.Message);
      }
    }


    static void Main(string[] args)
    {
      StartKdu();
    }
  }
}
