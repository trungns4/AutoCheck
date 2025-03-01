using System;

namespace MXTools
{
  public class MemorySharpHolder
  {
    private static Process.NET.ProcessSharp _sharp = null;

    public static Process.NET.ProcessSharp GetMemorySharp()
    {
      if (_sharp == null || _sharp.Native == null || _sharp.Native?.HasExited == true)
      {
        _sharp = Utils.CreateMemorySharp();
      }

      if (_sharp?.Native?.HasExited == true)
      {
        return null;
      }
      return _sharp;
    }
  }
}
