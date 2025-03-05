using log4net;
using MXTools.Archive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MXTools.Input
{
  class DriverKeyboard : IKeyboard
  {
    private ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private bool _error = true;

    public void KeyDown(byte key)
    {
      if (_error == false)
        IbInputSimulator.IbSendKeybdDown((ushort)key);
    }

    public void KeyUp(byte key)
    {
      if (_error == false)
        IbInputSimulator.IbSendKeybdUp((ushort)key);
    }

    public void Init()
    {
      if (IbInputSimulator.Error.Success == IbInputSimulator.IbSendInit(IbInputSimulator.SendType.Logitech, 0, 0))
      {
        _error = false;
        _log.Info("Successfully loaded driver keyboard");
      }
      else
      {
        _log.Info("Fail to load driver keyboard");
      }
    }

    public void Destroy()
    {
      IbInputSimulator.IbSendDestroy();
    }
  }
}
