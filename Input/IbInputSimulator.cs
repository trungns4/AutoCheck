namespace MXTools.Archive
{
  using System.Runtime.InteropServices;

  public static class IbInputSimulator
  {
    private const string DllName = "IbInputSimulator.dll";

    // Error Enum
    public enum Error : uint
    {
      Success,
      InvalidArgument,
      LibraryNotFound,
      LibraryLoadFailed,
      LibraryError,
      DeviceCreateFailed,
      DeviceNotFound,
      DeviceOpenFailed
    }

    // SendType Enum
    public enum SendType : uint
    {
      AnyDriver = 0,
      SendInput = 1,
      Logitech = 2,
      LogitechGHubNew = 6,
      Razer = 3,
      DD = 4,
      MouClassInputInjection = 5
    }

    // Hook Code Enum
    public enum HookCode : uint
    {
      Off,
      On,
      InitOnly,
      Destroy
    }

    // Move Mode Enum
    public enum MoveMode : uint
    {
      Absolute = 0,
      Relative = 1
    }

    // Mouse Button Enum
    public enum MouseButton : uint
    {
      LeftDown = 0x02,
      LeftUp = 0x04,
      Left = LeftDown | LeftUp,

      RightDown = 0x08,
      RightUp = 0x10,
      Right = RightDown | RightUp,

      MiddleDown = 0x20,
      MiddleUp = 0x40,
      Middle = MiddleDown | MiddleUp,

      XButton1Down = 0x81,
      XButton1Up = 0x101,
      XButton1 = XButton1Down | XButton1Up,

      XButton2Down = 0x82,
      XButton2Up = 0x102,
      XButton2 = XButton2Down | XButton2Up
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardModifiers
    {
      public bool LCtrl;
      public bool LShift;
      public bool LAlt;
      public bool LWin;
      public bool RCtrl;
      public bool RShift;
      public bool RAlt;
      public bool RWin;
    }

    // Initialization
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern Error IbSendInit(SendType type, uint flags, nint argument);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void IbSendDestroy();

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void IbSendSyncKeyStates();

    // Keyboard Events
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern bool IbSendKeybdDown(ushort vk);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern bool IbSendKeybdUp(ushort vk);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern bool IbSendKeybdDownUp(ushort vk, KeyboardModifiers modifiers);

    // Mouse Events
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern bool IbSendMouseMove(uint x, uint y, MoveMode mode);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern bool IbSendMouseClick(MouseButton button);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern bool IbSendMouseWheel(int movement);

    // Hooking
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void IbSendInputHook(HookCode code);

    // Windows Input API
    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern uint IbSendInput(uint cInputs, nint pInputs, int cbSize);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void IbSend_mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, ulong dwExtraInfo);

    [DllImport(DllName, CallingConvention = CallingConvention.StdCall)]
    public static extern void IbSend_keybd_event(byte bVk, byte bScan, uint dwFlags, ulong dwExtraInfo);
  }

}
