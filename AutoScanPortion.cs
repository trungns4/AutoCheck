using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCheck.Properties;
using Binarysharp.MemoryManagement;
using System.Windows.Forms;
using log4net;
using System.Reflection;
using WindowsInput;
using System.Threading;
using System.Diagnostics;

namespace AutoCheck
{
  internal class AutoScanPortion
  {
    public AutoScanPortion()
    {
    }
    //---------------------------------------------------------------------------------------------------------
    private int FindInteger(byte[] buffer, int a)
    {
      for (int i = 0; i <= buffer.Length - 4; i++)
      {
        int value = BitConverter.ToInt32(buffer, i);
        if (value == a)
          return i;
      }
      return -1;
    }
    //---------------------------------------------------------------------------------------------------------
    private int Read(MemorySharp sharp, long addr)
    {
      try
      {
        return sharp.Read<int>((IntPtr)addr, false);
      }
      catch
      {
        return -1;
      }
    }
    //---------------------------------------------------------------------------------------------------------
    public bool Scan(int portion, Action<int> step)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      MemorySharp sharp = Utils.CreateMemorySharp();
      if (sharp == null)
      {
        MessageBox.Show("Could not find the window", Resources.MsgBoxCaption);
        return false;
      }

      List<long> addresses = new List<long>();

      foreach (var region in sharp.Memory.Regions)
      {
        try
        {
          byte[] buffer = sharp.Read<byte>(region.BaseAddress, region.Information.RegionSize, false);
          var addr = FindInteger(buffer, portion);

          if (addr > 0)
          {
            addresses.Add(region.BaseAddress.ToInt64() + addr);
          }
        }
        catch { }
      }

      step(addresses.Count);

      int portionCount = portion - 1;

      log.InfoFormat("Found {0} addresses", addresses.Count);

      while (addresses.Count > 0 && portionCount > 0)
      {
        var sw = Stopwatch.StartNew();
        while (sw.ElapsedMilliseconds < 3000)
        {
          if (Utils.IsWindowActive(sharp.Pid) == false)
          {
            Utils.SetWindowActive(sharp.Pid); 
          }
        }

        if (Utils.IsWindowActive(sharp.Pid))
        {

          Input.Simulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);
          Thread.Sleep(32);

          Input.Simulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
          Thread.Sleep(32);

          addresses.RemoveAll(x => Read(sharp, x) != portionCount);

          if (addresses.Count == 1)
          {
            log.DebugFormat("Found address:", addresses.First().ToString());
            break;
          }

          step(addresses.Count);
          log.Debug($"Addess count {addresses.Count}");
        }
      }
      return true;
    }
  }
}
