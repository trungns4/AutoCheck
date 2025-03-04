using MXTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdapterTest
{
  class Program
  {
    static void TestFindProcess()
    {
      //var pid = MXTools.Helpers.FindProcess("ILSpy.exe");

      //if (pid != 0)
      //{
      //  Console.WriteLine($"Process found {pid}");
      //}
      //else
      //{
      //  Console.WriteLine("Process not found");
      //}
    }

    static void TestMemoryInfo()
    {
      var pid = MxSharp.FindProcess("main.wf");

      if (pid != 0)
      {
        Console.WriteLine($"Process found {pid}");
      }
      else
      {
        Console.WriteLine("Process not found");
        return;
      }

      if (MXTools.MxSharp.Instance.Attach(pid) == false)
      {
        Console.WriteLine($"Could not attach to process {pid}");
        return;
      }

      ulong totals = 0;
      ulong size = 0;

      if (MXTools.MxSharp.Instance.GetMemoryInfo(ref size, ref totals))
      {
        Console.WriteLine($"Process has {size} regions with {totals:N0} bytes");
      }
      else
      {
        Console.WriteLine("Could not retrieve information");
      }

      CancellationTokenSource cts = new CancellationTokenSource();

      Task.Run(() =>
      {
        MxSharp.Instance.ScanMemory(16113, 3, (current) =>
        {
          Console.WriteLine($"Read {current:N0} bytes");
          return true;
        },
        (adr, offset) =>
        {
          if (adr > 0)
          {
            Console.WriteLine($"Found at {adr:N0} {offset}%");

            var curMana = MxSharp.Instance.ReadMemory(adr + 8);
            var maxMana = MxSharp.Instance.ReadMemory(adr + 24);

            Console.WriteLine($"Values {curMana} {maxMana}");

          }
          else
          {
            Console.WriteLine("Not found");
          }
        },
        cts.Token);
      });

      Console.WriteLine("Waiting...");
      while (cts.IsCancellationRequested == false)
      {
        Console.WriteLine("Press C to quit");
        var info = Console.ReadKey();
        if (info.Key == ConsoleKey.C)
        {
          cts.Cancel();
        }
      }
    }

    static void Main(string[] args)
    {
      TestMemoryInfo();

      Console.ReadLine();
    }
  }
}
