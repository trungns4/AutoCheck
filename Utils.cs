using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Native;
using log4net;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern short VkKeyScan(char ch);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    public struct POINT
    {
      public int X;
      public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
      public int Left;
      public int Top;
      public int Right;
      public int Bottom;
    }

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_RESTORE = 9;

    public static void GetWindowRectangle(IntPtr hWnd, out int left, out int top, out int right, out int bottom)
    {
      GetWindowRect(hWnd, out RECT rect);
      left = rect.Left;
      top = rect.Top;
      right = rect.Right;
      bottom = rect.Bottom;
    }
    //----------------------------------------------------------------------------------
    public static MemorySharp CreateMemorySharp()
    {
      var processId = GetProcessId();
      if (processId == null)
      {
        return null;
      }
      return new MemorySharp(processId.Value);
    }
    //----------------------------------------------------------------------------------
    public static int? GetProcessId()
    {
      var windowName = GetConfigString("window").ToUpper();

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
    //----------------------------------------------------------------------------------
    public static bool IsWindowActive(int id)
    {
      return (GetWindowThreadProcessId(GetForegroundWindow(), out uint pid), pid == id).Item2;
    }
    //----------------------------------------------------------------------------------
    public static void SetWindowActive(int processId)
    {
      IntPtr hwnd = GetMainWindowHandle(processId);
      if (hwnd != IntPtr.Zero)
      {
        SetForegroundWindow(hwnd);
      }
    }
    //----------------------------------------------------------------------------------
    public static IntPtr GetMainWindowHandle(int processId)
    {
      Process process = Process.GetProcessById(processId);
      return process.MainWindowHandle;
    }
    //----------------------------------------------------------------------------------
    public static VirtualKeyCode KeyCode(char ch)
    {
      short vkey = VkKeyScan(ch);
      return (VirtualKeyCode)(vkey & 0xFF);
    }
    //----------------------------------------------------------------------------------
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
    //----------------------------------------------------------------------------------
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
    //----------------------------------------------------------------------------------
    public static string GetConfigString(string name, string def = "")
    {
      if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(name))
      {
        return System.Configuration.ConfigurationManager.AppSettings[name];
      }
      return def;
    }
    //----------------------------------------------------------------------------------
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
      if (value.CompareTo(min) < 0) return min;
      if (value.CompareTo(max) > 0) return max;
      return value;
    }
    //----------------------------------------------------------------------------------
    public static T Get<T>(JObject settings, string key, T defaultValue)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      if (settings.TryGetValue(key, out JToken value))
      {
        try
        {
          // Handle null values
          if (value.Type == JTokenType.Null)
          {
            return defaultValue;
          }

          return value.Value<T>();
        }
        catch
        {
          log.WarnFormat($"Warning: Failed to convert key '{key}' to {typeof(T).Name}. Using default value.");
          return defaultValue;
        }
      }
      else
      {
        log.WarnFormat($"Warning: Key '{key}' is missing. Using default value.");
        return defaultValue;
      }
    }
    //----------------------------------------------------------------------------------
    public static void GetMouse(out int x, out int y)
    {
      GetCursorPos(out POINT p);
      x = p.X;
      y = p.Y;
    }

    //----------------------------------------------------------------------------------
    public static bool IsAlt()
    {
      if ((GetAsyncKeyState(0x12) & 0x8000) != 0) // 0x12 is VK_MENU (Alt key)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    //----------------------------------------------------------------------------------
    public static bool IsKeyHeld(char key)
    {
      int vKey = char.ToUpper(key); // Convert to uppercase to match virtual key codes
      return (GetAsyncKeyState(vKey) & 0x8000) != 0;
    }
    //----------------------------------------------------------------------------------
    public static void CloseApps()
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      var apps = GetConfigString("closeApps").Split(";");

      foreach (var app in apps)
      {
        var processes = Process.GetProcesses()
                                   .Where(p => p.ProcessName.StartsWith(app, StringComparison.OrdinalIgnoreCase))
                                   .ToList();

        if (processes.Count > 0)
        {
          foreach (Process process in processes)
          {
            try
            {
              process.Kill();
              process.WaitForExit();
              log.InfoFormat("Terminated app {0}", app);
            }
            catch
            {
            }
          }
        }
      }
    }

    //----------------------------------------------------------------------------------
    public static void ShowWindow(IntPtr hWnd)
    {
      ShowWindow(hWnd, SW_RESTORE);
    }
    //----------------------------------------------------------------------------------
    public static IntPtr FindWindow(string lpWindowName)
    {
      return FindWindow(null, lpWindowName);
    }
  }
}
