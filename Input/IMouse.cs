using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Input
{
  public interface IMouse
  {
    void RightButtonDown();
    void RightButtonUp();
    public bool Init();
    public void Destroy();
  }
}
