using Binarysharp.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using WindowsInput.Native;

namespace AutoCheck
{
  internal class Utils

  {   // Windows API functions
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern short VkKeyScan(char ch);

    public static MemorySharp CreateMemorySharp()
    {
      var processId = GetProcessId();
      if (processId == null)
      {
        return null;
      }
      return new MemorySharp(processId.Value);
    }

    public static int? GetProcessId()
    {
      var windowName = System.Configuration.ConfigurationManager.AppSettings["window"].ToUpper();

      IntPtr hWnd = FindWindow(null, windowName);
      if (hWnd == IntPtr.Zero)
      {
        return null;
      }

      // Get process ID
      GetWindowThreadProcessId(hWnd, out uint processId);
      if (processId == 0)
      {
        return null;
      }

      return (int)processId;
    }

    public static bool IsWindowActive(int id)
    {
      return (GetWindowThreadProcessId(GetForegroundWindow(), out uint pid), pid == id).Item2;
    }

    public static VirtualKeyCode KeyCode(char ch)
    {
      short vkey = VkKeyScan(ch);
      return (VirtualKeyCode)(vkey & 0xFF);
    }

    public static int GetConfigInt(string name, int def = 0)
    {
      if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(name))
      {
        if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings[name], out int ret))
        {
          return ret;
        }
      }
      return def;
    }

    public static double GetConfigDouble(string name, double def = 0)
    {
      if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(name))
      {
        if (double.TryParse(System.Configuration.ConfigurationManager.AppSettings[name], out double ret))
        {
          return ret;
        }
      }
      return def;
    }

    public static string GetConfigString(string name, string def = "")
    {
      if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(name))
      {
        return System.Configuration.ConfigurationManager.AppSettings[name];
      }
      return def;
    }
  }
}
