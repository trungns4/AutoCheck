using Binarysharp.MemoryManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools
{
  public class MemorySharpHolder
  {
    private static MemorySharp _sharp = null;

    public static MemorySharp GetMemorySharp()
    {
      if (_sharp == null)
      {
        _sharp = Utils.CreateMemorySharp();
      }
      else
      {
        if (_sharp.IsRunning == false)
        {
          _sharp = Utils.CreateMemorySharp();
        }
      }

      if (_sharp?.IsRunning == false)
      {
        return null;
      }
      return _sharp;
    }
  }
}
