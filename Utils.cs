using log4net;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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

    [DllImport("ntdll.dll")]
    private static extern int NtSetInformationProcess(IntPtr processHandle, int processInformationClass, ref int processInformation, int processInformationLength);

    private const int ProcessDenyHandleAccess = 0x1D; // ProcessInformationClass = 29 (PROCESSINFOCLASS::ProcessAccessToken)

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
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

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

    ////----------------------------------------------------------------------------------
    //public static MemorySharp CreateMemorySharp()
    //{
    //  var processId = GetProcessId();
    //  if (processId == null)
    //  {
    //    log.Error("Failed to find process ID.");
    //    return null;
    //  }
    //  return new MemorySharp(processId.Value);
    //}

    //----------------------------------------------------------------------------------
    public static int? GetProcessId()
    {
      var windowName = GetConfigString("window").ToUpper();
      var app = GetConfigString("app");

      IntPtr hWnd = FindWindow(null, windowName);
      if (hWnd != IntPtr.Zero)
      {
        GetWindowThreadProcessId(hWnd, out uint processId);
        if (processId != 0)
        {
          return (int)processId;
        }
      }

      var processes = Process.GetProcesses()
        .Where(p => p.ProcessName.Contains(app, StringComparison.OrdinalIgnoreCase));

      return processes.FirstOrDefault()?.Id;
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
    public static Keys KeyCode(char ch)
    {
      short vkey = VkKeyScan(ch);
      return (Keys)(vkey & 0xFF);
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
      return GlobalSettings.Instance.GetConfigString(name) ?? def;
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
    //---------------------------------------------------------------------------------- 
    public static IntPtr FindWindowByExeName(string exeName)
    {
      IntPtr foundWindow = IntPtr.Zero;
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
      }, IntPtr.Zero);

      return foundWindow;
    }

    public static void ProtectProcess()
    {
      int denyHandleValue = 1;
      IntPtr currentProcess = Process.GetCurrentProcess().Handle;

      int status = NtSetInformationProcess(currentProcess, ProcessDenyHandleAccess, ref denyHandleValue, sizeof(int));

      if (status == 0)
        Console.WriteLine("✅ Process handle access blocked successfully!");
      else
        Console.WriteLine($"❌ Failed to block handle access. Status: 0x{status:X}");
    }
  }
}
