using System;
using System.Runtime.InteropServices;

namespace MXTools.Input
{
  public class KeybdEvent
  {
    private delegate void KeybdEventDelegate(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    private static KeybdEventDelegate _originalKeybdEvent;
    private static bool _isInitialized = false;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetProcAddress(nint hModule, string procName);

    public static void Init()
    {
      if (_isInitialized) return;

      nint hUser32 = GetModuleHandle("user32.dll");
      nint funcAddr = GetProcAddress(hUser32, "keybd_event");

      if (funcAddr != nint.Zero)
      {
        _originalKeybdEvent = Marshal.GetDelegateForFunctionPointer<KeybdEventDelegate>(funcAddr);
        _isInitialized = true;
      }
    }


    public static void KeyDown(byte key)
    {
      _originalKeybdEvent?.Invoke(key, 0, 0, 0);
    }

    public static void KeyUp(byte key)
    {
      _originalKeybdEvent?.Invoke(key, 0, 2, 0);
    }
  }
}
