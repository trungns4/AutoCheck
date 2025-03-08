using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Input
{
  class WinKeyboardSI : IKeyboard
  {
    public void Destroy()
    {
    }

    public void Init()
    {
    }

    public void KeyDown(byte key) => SendInputEx.SendKey(key, true);
    public void KeyUp(byte key) => SendInputEx.SendKey(key, false);
  }

  class WinKeyboardKE : IKeyboard
  {
    public void Destroy()
    {
    }

    public void Init()
    {
    }

    public void KeyDown(byte key) => KeybdEvent.KeyDown(key);
    public void KeyUp(byte key) => KeybdEvent.KeyUp(key);
  }
}
