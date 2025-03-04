using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MXTools.Helpers
{
  internal class Utils
  {
    private static readonly ILog log = LogManager.GetLogger(typeof(Utils));

    // Windows API functions
    [DllImport("user32.dll", SetLastError = true)]
    private static extern nint FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(nint hWnd, out uint processId);

    [DllImport("user32.dll")]
    private static extern nint GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern bool SetForegroundWindow(nint hWnd);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(nint hWnd, out RECT lpRect);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    public static extern short VkKeyScan(char ch);

    [DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(nint hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(nint hWnd);

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

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, nint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowText(nint hWnd, StringBuilder lpString, int nMaxCount);

    private delegate bool EnumWindowsProc(nint hWnd, nint lParam);

    //----------------------------------------------------------------------------------
    public static void GetWindowRectangle(nint hWnd, out int left, out int top, out int right, out int bottom)
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
    public static bool IsWindowActive(int id)
    {
      nint foregroundWindow = GetForegroundWindow();
      if (foregroundWindow == nint.Zero)
      {
        return false;
      }

      GetWindowThreadProcessId(foregroundWindow, out uint pid);
      return pid == (uint)id;
    }

    //----------------------------------------------------------------------------------
    public static nint GetMainWindowHandle(int processId)
    {
      try
      {
        Process process = Process.GetProcessById(processId);
        return process.MainWindowHandle;
      }
      catch (Exception ex)
      {
        log.Error($"Error getting main window handle for PID {processId}: {ex.Message}");
        return nint.Zero;
      }
    }

    //----------------------------------------------------------------------------------
    public static Keys KeyCode(char ch)
    {
      short vkey = VkKeyScan(ch);
      return (Keys)(vkey & 0xFF);
    }


    //----------------------------------------------------------------------------------
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
      return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
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
      var apps = GlobalSettings.Instance.GetConfigString("closeApps").Split(";");

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
    public static void ShowWindow(nint hWnd)
    {
      ShowWindow(hWnd, SW_RESTORE);
    }
    //---------------------------------------------------------------------------------- 
    public static nint FindWindowByExeName(string exeName)
    {
      nint foundWindow = nint.Zero;
      uint currentProcessId = (uint)Process.GetCurrentProcess().Id;

      EnumWindows((hWnd, lParam) =>
      {
        if (GetWindowThreadProcessId(hWnd, out uint processId) != 0)
        {
          if (processId != currentProcessId) // ✅ Exclude the current process
          {
            try
            {
              Process proc = Process.GetProcessById((int)processId);
              if (proc.MainModule.FileName.EndsWith(exeName, StringComparison.OrdinalIgnoreCase))
              {
                foundWindow = hWnd;
                return false; // Stop searching after finding the first match
              }
            }
            catch
            {
              // Process might have exited, ignore exceptions
            }
          }
        }
        return true; // Continue enumeration
      }, nint.Zero);

      return foundWindow;
    }
  }
}
