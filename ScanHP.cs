using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using log4net;
using Microsoft.VisualBasic.Logging;
using MXTools.Properties;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools
{
  internal class ScanHP
  {
    private volatile bool _stopFlag = false;

    private long _address = 0;
    private int _offset = 0;

    public ScanHP()
    {
    }

    //--------------------------------------------------------------------------------------------
    public long TotalBytes(List<RemoteRegion> regions)
    {
      long totalBytes = 0;
      foreach (var region in regions)
      {
        try
        {
          var rs = region.Information.RegionSize;
          totalBytes += rs;
        }
        catch
        {
        }
      }
      return totalBytes;
    }
    //--------------------------------------------------------------------------------------------
    private int Find(byte[] buffer, int number)
    {
      if (buffer.Length < 28)
      {
        return -1;
      }

      int result = -1;
      object lockObj = new object(); // Protect shared result
      int maxIndex = buffer.Length - 28;

      Parallel.For(0, maxIndex, (i, state) =>
      {
        if (_stopFlag || result != -1)
        {
          state.Stop();
          return;
        }

        int a = Unsafe.As<byte, int>(ref buffer[i]);
        if (a != number) return;

        int c = Unsafe.As<byte, int>(ref buffer[i + 16]);
        if (c != a) return;

        int b = Unsafe.As<byte, int>(ref buffer[i + 8]);
        int d = Unsafe.As<byte, int>(ref buffer[i + 24]);

        if (b == d)
        {
          lock (lockObj) // Ensure only one thread writes to result
          {
            if (result == -1) result = i;
          }
          state.Stop(); // Stop further processing
        }
      });

      return result;
    }
    //--------------------------------------------------------------------------------------------
    public void Stop()
    {
      _stopFlag = true;
    }
    //--------------------------------------------------------------------------------------------
    private bool ScanRegion(MemorySharp sharp, RemoteRegion region, int hp, out int adr)
    {
      adr = 0;
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      try
      {
        var rs = region.Information.RegionSize;
        if (rs < 24)
        {
          return false;
        }

        byte[] tempBuffer = MemoryCore.ReadBytes(sharp.Handle, region.BaseAddress, rs);
        int tempAdr = Find(tempBuffer, hp);
        if (tempAdr >= 0)
        {
          adr = tempAdr;
          return true;
        }
      }
      catch
      {
        //log.Error($"Could not read the region #{regionIndex}");
        return false;
      }
      return false;
    }
    //--------------------------------------------------------------------------------------------
    private int RegionSize(RemoteRegion region)
    {
      try
      {
        return region.Information.RegionSize;
      }
      catch
      {
        return 0;
      }
    }
    //--------------------------------------------------------------------------------------------
    public bool Scan(int hp, int offset, Action<long> totalFunc, Action<long> progressFunc, Action<long, int> doneFunc)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      var sharp = MemorySharpHolder.GetMemorySharp();
      if (sharp == null)
      {
        MessageBox.Show("The process is not running", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        doneFunc(0, 0);
        return false;
      }

      offset = Math.Min(Math.Max(offset, 0), 99);
      var regions = sharp.Memory.Regions.ToList();
      var total = TotalBytes(regions);
      totalFunc(total);
      _stopFlag = false;

      double start = (double)(total) * (double)offset / 100.0;

      log.InfoFormat($"Scanning {total:N0} bytes over {regions.Count} regions");

      Task.Run(() =>
      {
        long current = 0;
        int regionIndex = offset;
        bool found = false;

        foreach (var region in regions)
        {
          progressFunc(current);
          if (_stopFlag)
          {
            log.InfoFormat("Scanning stopped");
            doneFunc(0, 0);
            return;
          }

          if (current < start)
          {
            current += RegionSize(region);
            regionIndex++;
            continue;
          }

          if (ScanRegion(sharp, region, hp, out int addr))
          {
            _address = region.BaseAddress.ToInt64() + addr;
            _offset = Utils.Clamp<int>((int)((double)current / total * 100) - 5, 0, 99);
            doneFunc(_address, _offset);
            log.InfoFormat($"Found address {_address:N0} at {regionIndex}/{region.Information.RegionSize:N0} offset {_offset}");
            found = true;
            break;
          }
          current += RegionSize(region);
          regionIndex++;
        }

        if (found == false)
        {
          log.InfoFormat("Not found");
          doneFunc(0, 0);
        }
      });


      return false;
    }
  }
}
