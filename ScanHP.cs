using log4net;
using MXTools.Properties;
using System;
using System.Reflection;
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
    public long TotalBytes(Process.NET.ProcessSharp sharp)
    {
      long totalBytes = 0;
      foreach (var region in sharp.MemoryFactory.Regions)
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

      Process.NET.ProcessSharp sharp = MemorySharpHolder.GetMemorySharp();
      if (sharp == null)
      {
        MessageBox.Show("The process is not running", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        doneFunc(0, 0);
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
        foreach (var region in sharp.MemoryFactory.Regions)
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

            if (rs < 24)
            {
              log.InfoFormat("Ignored small region size {0} {1}", regionIndex, rs);
              continue;
            }

            byte[] buffer = sharp.Memory.Read<byte>(region.BaseAddress, rs);
            current += rs;

            int adr = Find(buffer, hp);
            if (adr >= 0)
            {
              _address = region.BaseAddress.ToInt64() + adr;
              _offset = (regionIndex / 100) * 100;
              doneFunc(_address, _offset);
              found = true;
              log.InfoFormat("Found address {0} at {1} offset {2}", _address, regionIndex, _offset);
              break;
            }
          }
          catch
          {
            //log.Error($"Region {regionIndex} could not accessed");
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
