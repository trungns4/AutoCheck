using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement.Memory;
using Binarysharp.MemoryManagement;
using MXTools.Properties;
using log4net;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public long TotalBytes(MemorySharp sharp)
    {
      long totalBytes = 0;
      foreach (var region in sharp.Memory.Regions)
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
    public bool Scan(int hp, int offset, Action<long> totalFunc, Action<long> progressFunc, Action<long, int> doneFunc)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      MemorySharp sharp = Utils.CreateMemorySharp();
      if (sharp == null)
      {
        return false;
      }

      var total = TotalBytes(sharp);
      totalFunc(total);
      _stopFlag = false;

      Task.Run(() =>
      {
        int current = 0;
        int regionIndex = 0;
        bool found = false;
        foreach (var region in sharp.Memory.Regions)
        {
          try
          {
            if (_stopFlag)
            {
              log.InfoFormat("Scanning stopped");
              doneFunc(0, 0);
              return;
            }

            progressFunc(current);
            var rs = region.Information.RegionSize;

            if (regionIndex < offset)
            {
              current += rs;
              continue;
            }

            byte[] buffer = sharp.Read<byte>(region.BaseAddress, rs, false);
            current += rs;

            int adr = Find(buffer, hp);
            if (adr >= 0)
            {
              log.InfoFormat("Found address at {0}", regionIndex);

              _address = region.BaseAddress.ToInt64() + adr;
              _offset = (regionIndex / 100) * 100;
              doneFunc(_address, _offset);
              found = true;
              break;
            }
          }
          catch
          {
            log.Error($"Region {regionIndex} could not accessed");
          }
          finally
          {
            regionIndex++;
          }
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
