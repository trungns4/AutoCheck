using System;
using System.Runtime.InteropServices;

namespace MXTools.Input
{
  public class MouseEvent
  {
    private delegate void MouseEventDelegate(uint dwFlags, int dx, int dy, uint dwData, nuint dwExtraInfo);
    private static MouseEventDelegate _originalMouseEvent;
    private static bool _isInitialized = false;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetProcAddress(nint hModule, string procName);

    // Mouse event constants
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

    /// <summary>
    /// Initializes the function pointer for mouse_event to bypass hooks.
    /// </summary>
    public static void Init()
    {
      if (_isInitialized) return;  // Prevent duplicate initialization

      nint hUser32 = GetModuleHandle("user32.dll");
      nint funcAddr = GetProcAddress(hUser32, "mouse_event");

      if (funcAddr != nint.Zero)
      {
        _originalMouseEvent = Marshal.GetDelegateForFunctionPointer<MouseEventDelegate>(funcAddr);
        _isInitialized = true;  // Mark as initialized
      }
    }

    /// <summary>
    /// Simulates pressing the right mouse button down.
    /// </summary>
    public static void RightButtonDown()
    {
      _originalMouseEvent?.Invoke(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, nuint.Zero);
    }

    /// <summary>
    /// Simulates releasing the right mouse button.
    /// </summary>
    public static void RightButtonUp()
    {
      _originalMouseEvent?.Invoke(MOUSEEVENTF_RIGHTUP, 0, 0, 0, nuint.Zero);
    }
  }
}
