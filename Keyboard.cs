using System;
using System.Runtime.InteropServices;

namespace MXTools
{
  public class Keyboard
  {
    private delegate void KeybdEventDelegate(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    private static KeybdEventDelegate _originalKeybdEvent;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    public static void Init()
    {
      // Get the original keybd_event function address
      IntPtr hUser32 = GetModuleHandle("user32.dll");
      IntPtr funcAddr = GetProcAddress(hUser32, "keybd_event");

      if (funcAddr != IntPtr.Zero)
      {
        _originalKeybdEvent = Marshal.GetDelegateForFunctionPointer<KeybdEventDelegate>(funcAddr);
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
