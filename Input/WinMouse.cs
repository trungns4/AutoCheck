using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Input
{
  public class WinMouseSI : IMouse
  {
    public void RightButtonDown() => SendInputEx.RightButtonDown();
    public void RightButtonUp() => SendInputEx.RightButtonUp();
    public bool Init() => true;
    public void Destroy() { }
  }

  public class WinMouseME : IMouse
  {
    public void RightButtonDown() => MouseEvent.RightButtonDown();
    public void RightButtonUp() => MouseEvent.RightButtonUp();
    public bool Init() => true;
    public void Destroy() { }
  }
}
