using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Input
{
  class WinKeyboard : IKeyboard
  {
    public WinKeyboard()
    {
    }

    public void KeyDown(byte key)
    {
      Keyboard.KeyDown(key);
    }

    public void KeyUp(byte key)
    {
      Keyboard.KeyUp(key);
    }

    public void Init()
    {

    }

    public void Destroy()
    {

    }
  }
}
