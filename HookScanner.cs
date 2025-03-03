using log4net;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MXTools
{
  public class HookScanner
  {
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll")]
    private static extern IntPtr LoadLibrary(string lpLibFileName);

    [DllImport("kernel32.dll")]
    private static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

    private const uint PAGE_EXECUTE_READWRITE = 0x40;

    public static void CheckAndRestoreHook(string functionName)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      IntPtr hUser32 = LoadLibrary("user32.dll");
      IntPtr funcAddr = GetProcAddress(hUser32, functionName);

      if (funcAddr == IntPtr.Zero)
      {
        log.Error($"Failed to locate {functionName}.");
        return;
      }

      byte[] currentBytes = new byte[5];
      Marshal.Copy(funcAddr, currentBytes, 0, 5);

      // Get original bytes from a fresh module load
      IntPtr hTempUser32 = LoadLibrary("user32.dll");
      IntPtr cleanFuncAddr = GetProcAddress(hTempUser32, functionName);

      byte[] originalBytes = new byte[5];
      Marshal.Copy(cleanFuncAddr, originalBytes, 0, 5);

      if (!CompareArrays(originalBytes, currentBytes))
      {
        log.Info($"{functionName} is hooked! Restoring...");

        uint oldProtect;
        VirtualProtect(funcAddr, originalBytes.Length, PAGE_EXECUTE_READWRITE, out oldProtect);
        Marshal.Copy(originalBytes, 0, funcAddr, originalBytes.Length);
        VirtualProtect(funcAddr, originalBytes.Length, oldProtect, out oldProtect);

        log.Info($"{functionName} has been restored.");
      }
      else
      {
        log.Info($"{functionName} is clean.");
      }
    }

    private static bool CompareArrays(byte[] arr1, byte[] arr2)
    {
      for (int i = 0; i < arr1.Length; i++)
      {
        if (arr1[i] != arr2[i]) return false;
      }
      return true;
    }
  }

  //class Program
  //{
  //  static void Main()
  //  {
  //    HookScanner.CheckAndRestoreHook("keybd_event");
  //    HookScanner.CheckAndRestoreHook("SendInput");
  //  }
  //}
}
