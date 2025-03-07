using System;
using System.Runtime.InteropServices;

namespace MXTools.Input
{
  public class KernelKeyboard : IKeyboard
  {
    private IntPtr driverHandle = IntPtr.Zero;
    private delegate int EntryPointDelegate();
    private delegate void KeyEventDelegate(byte key, bool keyDown);

    private EntryPointDelegate? entryPoint;
    private KeyEventDelegate? sendKeyEvent;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    public void Init()
    {
      driverHandle = GetModuleHandle("MxDriver.sys");  // Get the mapped driver
      if (driverHandle == IntPtr.Zero)
      {
        throw new Exception("Failed to get kernel driver handle.");
      }

      // Find EntryPoint function
      IntPtr entryPointAddr = GetProcAddress(driverHandle, "EntryPoint");
      if (entryPointAddr == IntPtr.Zero)
      {
        throw new Exception("Failed to locate EntryPoint.");
      }
      entryPoint = (EntryPointDelegate)Marshal.GetDelegateForFunctionPointer(entryPointAddr, typeof(EntryPointDelegate));

      // Find Key Event Function (assuming `SendKeyEvent` is exported)
      IntPtr keyEventAddr = GetProcAddress(driverHandle, "SendKeyEvent");
      if (keyEventAddr == IntPtr.Zero)
      {
        throw new Exception("Failed to locate SendKeyEvent.");
      }
      sendKeyEvent = (KeyEventDelegate)Marshal.GetDelegateForFunctionPointer(keyEventAddr, typeof(KeyEventDelegate));

      Console.WriteLine("Kernel driver initialized.");
      entryPoint(); // Call the driver entry point
    }

    public void KeyDown(byte key)
    {
      sendKeyEvent?.Invoke(key, true);
      Console.WriteLine($"Key Down: {key}");
    }

    public void KeyUp(byte key)
    {
      sendKeyEvent?.Invoke(key, false);
      Console.WriteLine($"Key Up: {key}");
    }

    public void Destroy()
    {
      driverHandle = IntPtr.Zero;
      Console.WriteLine("Kernel driver destroyed.");
    }
  }
}
