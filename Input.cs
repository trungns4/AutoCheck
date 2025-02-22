using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace AutoCheck
{
  internal class Input
  {
    private InputSimulator _is = new InputSimulator();
    private static Input _instance;

    private Input()
    {
    }

    public static Input Instance
    {
      get
      {
        if (_instance == null)
        {
          _instance = new Input();
        }
        return _instance;
      }
    }

    public static InputSimulator Simulator
    {
      get
      {
        return Instance._is;
      }
    }
  }
}
