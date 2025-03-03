using System;
using System.Runtime.InteropServices;

namespace MXTools
{
  public class InputSender
  {
    private delegate uint SendInputDelegate(uint nInputs, IntPtr pInputs, int cbSize);
    private static SendInputDelegate _originalSendInput;

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
      public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct MOUSEINPUT
    {
      public int dx;
      public int dy;
      public uint mouseData;
      public uint dwFlags;
      public uint time;
      public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct HARDWAREINPUT
    {
      public uint uMsg;
      public ushort wParamL;
      public ushort wParamH;
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    public static void Init()
    {
      // Get the original SendInput function address
      IntPtr hUser32 = GetModuleHandle("user32.dll");
      IntPtr funcAddr = GetProcAddress(hUser32, "SendInput");

      if (funcAddr != IntPtr.Zero)
      {
        _originalSendInput = Marshal.GetDelegateForFunctionPointer<SendInputDelegate>(funcAddr);
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
      inputs[0].u.ki.dwExtraInfo = IntPtr.Zero;

      IntPtr pInputs = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(INPUT)) * inputs.Length);
      Marshal.StructureToPtr(inputs[0], pInputs, false);

      _originalSendInput((uint)inputs.Length, pInputs, Marshal.SizeOf(typeof(INPUT)));

      Marshal.FreeHGlobal(pInputs);
    }

    public static void RightButtonDown()
    {
      if (_originalSendInput == null) return;

      INPUT[] inputs = new INPUT[1];
      inputs[0].type = 0; // Mouse input
      inputs[0].u.mi.dx = 0;
      inputs[0].u.mi.dy = 0;
      inputs[0].u.mi.mouseData = 0;
      inputs[0].u.mi.dwFlags = 0x0008; // MOUSEEVENTF_RIGHTDOWN
      inputs[0].u.mi.time = 0;
      inputs[0].u.mi.dwExtraInfo = IntPtr.Zero;

      IntPtr pInputs = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(INPUT)) * inputs.Length);
      Marshal.StructureToPtr(inputs[0], pInputs, false);

      _originalSendInput((uint)inputs.Length, pInputs, Marshal.SizeOf(typeof(INPUT)));

      Marshal.FreeHGlobal(pInputs);
    }

    public static void RightButtonUp()
    {
      if (_originalSendInput == null) return;

      INPUT[] inputs = new INPUT[1];
      inputs[0].type = 0; // Mouse input
      inputs[0].u.mi.dx = 0;
      inputs[0].u.mi.dy = 0;
      inputs[0].u.mi.mouseData = 0;
      inputs[0].u.mi.dwFlags = 0x0010; // MOUSEEVENTF_RIGHTUP
      inputs[0].u.mi.time = 0;
      inputs[0].u.mi.dwExtraInfo = IntPtr.Zero;

      IntPtr pInputs = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(INPUT)) * inputs.Length);
      Marshal.StructureToPtr(inputs[0], pInputs, false);

      _originalSendInput((uint)inputs.Length, pInputs, Marshal.SizeOf(typeof(INPUT)));

      Marshal.FreeHGlobal(pInputs);
    }
  }
}
