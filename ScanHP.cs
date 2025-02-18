using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement.Memory;
using Binarysharp.MemoryManagement;
using AutoCheck.Properties;
using log4net;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Net;

namespace AutoCheck
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
    private long TotalBytes(MemorySharp sharp, out List<RemoteRegion> errRegions)
    {
      long totalBytes = 0;
      errRegions = new List<RemoteRegion>();
      foreach (var region in sharp.Memory.Regions)
      {
        try
        {
          var rs = region.Information.RegionSize;
          totalBytes += rs;
        }
        catch
        {
          errRegions.Add(region);
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

      for (int i = 0; i <= buffer.Length - 28; i++)
      {
        if (_stopFlag)
        {
          return -1;
        }

        int a = BitConverter.ToInt32(buffer, i);
        int b = BitConverter.ToInt32(buffer, i + 8);
        int c = BitConverter.ToInt32(buffer, i + 16);
        int d = BitConverter.ToInt32(buffer, i + 24);

        if (a == c && a == number && b == d)
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
    public bool Scan(int hp, int offset, Action<long> totalFunc, Action<long> progress, Action<long, int> done)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      MemorySharp sharp = Utils.CreateMemorySharp();
      if (sharp == null)
      {
        return false;
      }

      var total = TotalBytes(sharp, out List<RemoteRegion> errRegions);
      totalFunc(total);
      _stopFlag = false;

      Task.Run(() =>
      {
        int current = 0;
        double percelOffset = offset;
        foreach (var region in sharp.Memory.Regions)
        {
          try
          {
            if (errRegions.Contains(region))
            {
              continue;
            }

            if (_stopFlag)
            {
              done(0, 0);
              return;
            }

            progress(current);
            var rs = region.Information.RegionSize;

            if (((double)current) < (double)(total * percelOffset / 100.0))
            {
              current += rs;
              continue;
            }

            byte[] buffer = sharp.Read<byte>(region.BaseAddress, rs, false);
            current += rs;

            int adr = Find(buffer, hp);
            if (adr >= 0)
            {
              _address = region.BaseAddress.ToInt64() + adr;
              _offset = (int)((double)(current) / (double)(total) * 100) - 10;
              _offset = Math.Min(Math.Max(1, _offset), 99);

              done(_address, _offset);
              return;
            }
          }
          catch { }
        }
      });

      return true;
    }
  }
}
