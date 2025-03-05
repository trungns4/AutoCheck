using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Helpers
{
  class Utils
  {
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
      return value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;
    }  
  }
}
