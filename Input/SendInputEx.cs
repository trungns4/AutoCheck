using System;
using System.Runtime.InteropServices;

namespace MXTools.Input
{
  public class SendInputEx
  {
    private delegate uint SendInputDelegate(uint nInputs, nint pInputs, int cbSize);
    private static SendInputDelegate _originalSendInput;
    private static bool _isInitialized = false;

    [StructLayout(LayoutKind.Sequential)]
    private struct INPUT
    {
      public int type;
      public InputUnion u;
    }

    [StructLayout(LayoutKind.Explicit)]
    private struct InputUnion
    {
      [FieldOffset(0)] public MOUSEINPUT mi;
      [FieldOffset(0)] public KEYBDINPUT ki;
      [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KEYBDINPUT
    {
      public ushort wVk;
      public ushort wScan;
      public uint dwFlags;
      public uint time;
      public nint dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
      public int dx;
      public int dy;
      public uint mouseData;
      public uint dwFlags;
      public uint time;
      public nint dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
      public uint uMsg;
      public ushort wParamL;
      public ushort wParamH;
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern nint GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetProcAddress(nint hModule, string procName);

    public static void Init()
    {
      if (_isInitialized) return;

      nint hUser32 = GetModuleHandle("user32.dll");
      nint funcAddr = GetProcAddress(hUser32, "SendInput");

      if (funcAddr != nint.Zero)
      {
        _originalSendInput = Marshal.GetDelegateForFunctionPointer<SendInputDelegate>(funcAddr);
        _isInitialized = true; 
      }
    }

    public static void SendKey(ushort key, bool keyDown)
    {
      if (_originalSendInput == null) return;

      INPUT[] inputs = new INPUT[1];
      inputs[0].type = 1; // Keyboard input
      inputs[0].u.ki.wVk = key;
      inputs[0].u.ki.wScan = 0;
      inputs[0].u.ki.dwFlags = keyDown ? 0u : 2u; // 0 = key down, 2 = key up
      inputs[0].u.ki.time = 0;
      inputs[0].u.ki.dwExtraInfo = nint.Zero;

      SendInputWrapper(inputs);
    }

    public static void RightButtonDown()
    {
      if (_originalSendInput == null) return;

      INPUT[] inputs = new INPUT[1];
      inputs[0].type = 0; // Mouse input
      inputs[0].u.mi.dwFlags = 0x0008; // MOUSEEVENTF_RIGHTDOWN

      SendInputWrapper(inputs);
    }

    public static void RightButtonUp()
    {
      if (_originalSendInput == null) return;

      INPUT[] inputs = new INPUT[1];
      inputs[0].type = 0; // Mouse input
      inputs[0].u.mi.dwFlags = 0x0010; // MOUSEEVENTF_RIGHTUP

      SendInputWrapper(inputs);
    }

    private static void SendInputWrapper(INPUT[] inputs)
    {
      nint pInputs = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(INPUT)) * inputs.Length);
      try
      {
        Marshal.StructureToPtr(inputs[0], pInputs, false);
        _originalSendInput((uint)inputs.Length, pInputs, Marshal.SizeOf(typeof(INPUT)));
      }
      finally
      {
        Marshal.FreeHGlobal(pInputs);
      }
    }
  }
}
