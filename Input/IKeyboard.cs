using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Input
{
  public interface IKeyboard
  {
    public void KeyDown(byte key);

    public void KeyUp(byte key);

    public void Init();

    public void Destroy();
  }
}
