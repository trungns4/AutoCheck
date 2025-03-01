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

      int a, b, c, d;
      for (int i = 0; i <= buffer.Length - 28; i++)
      {
        if (_stopFlag)
        {
          return -1;
        }

        a = BitConverter.ToInt32(buffer, i);

        if (a != number)
        {
          continue;
        }

        c = BitConverter.ToInt32(buffer, i + 16);
        if (c != a)
        {
          continue;
        }

        b = BitConverter.ToInt32(buffer, i + 8);
        d = BitConverter.ToInt32(buffer, i + 24);

        if (b == d)
        {
          return i;
        }
      }
      return -1;
    }
    //--------------------------------------------------------------------------------------------
    public void Stop()
    {
      _stopFlag = true;
    }
    //--------------------------------------------------------------------------------------------
    private bool ScanRegion(MemorySharp sharp, RemoteRegion region,
        ref int regionIndex, ref long current,
        int hp, Action<long> progressFunc, out int adr)
    {
      adr = 0;
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      try
      {
        progressFunc(current);
        Thread.Sleep(10);

        var rs = region.Information.RegionSize;
        if (rs < 24)
        {
          log.Info($"Ignored small region size {regionIndex} {rs}");
          return false;
        }

        //byte[] tempBuffer = sharp.Read<byte>(region.BaseAddress, rs, false);

        byte[] tempBuffer = MemoryCore.ReadBytes(sharp.Handle, region.BaseAddress, rs);

        //log.Info($"Read {rs:N0} bytes of the region #{regionIndex}");

        int tempAdr = Find(tempBuffer, hp);
        if (tempAdr >= 0)
        {
          adr = tempAdr;
          return true;
        }
        current += rs;
      }
      catch
      {
        //log.Error($"Could not read the region #{regionIndex}");
        return false;
      }
      finally
      {
        regionIndex++;
      }

      return false;
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

      var regions = sharp.Memory.Regions.Skip(offset).ToList();

      var total = TotalBytes(regions);
      totalFunc(total);
      _stopFlag = false;

      log.InfoFormat($"Scanning {total:N0} bytes over {regions.Count()} regions");

      Task.Run(() =>
      {
        long current = 0;
        int regionIndex = offset;
        bool found = false;

        foreach (var region in regions)
        {
          if (_stopFlag)
          {
            log.InfoFormat("Scanning stopped");
            doneFunc(0, 0);
            return;
          }

          if (ScanRegion(sharp, region, ref regionIndex, ref current, hp, progressFunc, out int addr))
          {
            _address = region.BaseAddress.ToInt64() + addr;
            _offset = (regionIndex / 100) * 100;
            doneFunc(_address, _offset);
            log.InfoFormat($"Found address {_address:N0} at {regionIndex}/{region.Information.RegionSize:N0} offset {_offset}");
            found = true;
            break;
          }
          Thread.Sleep(10);
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
