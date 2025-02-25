using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Native;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using WindowsInput.Native;

namespace MXTools
{
  internal class Utils
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(Utils));

    // Windows API functions
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

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    private const int SW_RESTORE = 9;
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

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

    //----------------------------------------------------------------------------------
    public static void GetWindowRectangle(IntPtr hWnd, out int left, out int top, out int right, out int bottom)
    {
      if (GetWindowRect(hWnd, out RECT rect))
      {
        left = rect.Left;
        top = rect.Top;
        right = rect.Right;
        bottom = rect.Bottom;
      }
      else
      {
        left = top = right = bottom = 0;
      }
    }

    //----------------------------------------------------------------------------------
    public static MemorySharp CreateMemorySharp()
    {
      var processId = GetProcessId();
      if (processId == null)
      {
        log.Error("Failed to find process ID.");
        return null;
      }
      return new MemorySharp(processId.Value);
    }

    //----------------------------------------------------------------------------------
    public static int? GetProcessId()
    {
      var windowName = GetConfigString("window").ToUpper();
      IntPtr hWnd = FindWindow(null, windowName);
      if (hWnd == IntPtr.Zero) return null;

      GetWindowThreadProcessId(hWnd, out uint processId);
      return processId == 0 ? (int?)null : (int)processId;
    }

    //----------------------------------------------------------------------------------
    public static bool IsWindowActive(int id)
    {
      IntPtr foregroundWindow = GetForegroundWindow();
      if (foregroundWindow == IntPtr.Zero)
      {
        return false;
      }

      GetWindowThreadProcessId(foregroundWindow, out uint pid);
      return pid == (uint)id;
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
      try
      {
        Process process = Process.GetProcessById(processId);
        return process.MainWindowHandle;
      }
      catch (Exception ex)
      {
        log.Error($"Error getting main window handle for PID {processId}: {ex.Message}");
        return IntPtr.Zero;
      }
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
      return int.TryParse(GetConfigString(name), out int result) ? result : def;
    }

    //----------------------------------------------------------------------------------
    public static double GetConfigDouble(string name, double def = 0)
    {
      return double.TryParse(GetConfigString(name), out double result) ? result : def;
    }

    //----------------------------------------------------------------------------------
    public static string GetConfigString(string name, string def = "")
    {
      return System.Configuration.ConfigurationManager.AppSettings[name] ?? def;
    }

    //----------------------------------------------------------------------------------
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
      return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
    }

    //----------------------------------------------------------------------------------
    public static T Get<T>(JObject settings, string key, T defaultValue)
    {
      if (settings.TryGetValue(key, out JToken value))
      {
        try
        {
          return value.Type == JTokenType.Null ? defaultValue : value.Value<T>();
        }
        catch
        {
          log.Warn($"Warning: Failed to convert key '{key}' to {typeof(T).Name}. Using default value.");
          return defaultValue;
        }
      }
      else
      {
        log.Warn($"Warning: Key '{key}' is missing. Using default value.");
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
      return (GetAsyncKeyState(0x12) & 0x8000) != 0; // 0x12 = VK_MENU (Alt key)
    }

    //----------------------------------------------------------------------------------
    public static bool IsKeyHeld(char key)
    {
      int vKey = VkKeyScan(key) & 0xFF;
      return (GetAsyncKeyState(vKey) & 0x8000) != 0;
    }

    //----------------------------------------------------------------------------------
    public static void CloseApps()
    {
      var apps = GetConfigString("closeApps").Split(";");

      foreach (var app in apps)
      {
        var processes = Process.GetProcesses()
                               .Where(p => p.ProcessName.StartsWith(app, StringComparison.OrdinalIgnoreCase))
                               .ToList();

        foreach (Process process in processes)
        {
          try
          {
            process.Kill();
            process.WaitForExit();
            log.Info($"Terminated app {app}");
          }
          catch (Exception ex)
          {
            log.Error($"Failed to terminate {app}: {ex.Message}");
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
    public static IntPtr FindWindowByTitle(string lpWindowName)
    {
      return FindWindow(null, lpWindowName);
    }

    //----------------------------------------------------------------------------------
    public static void ToggleWindow(IntPtr hwnd)
    {
      if (hwnd == IntPtr.Zero) return;

      ShowWindow(hwnd, IsWindowVisible(hwnd) ? SW_HIDE : SW_SHOW);
    }
  }
}
