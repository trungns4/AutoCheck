using log4net;
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
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private bool _error = true;

    public void KeyDown(byte key)
    {
      if (_error == false)
        DriverInputSimulator.IbSendKeybdDown((ushort)key);
    }

    public void KeyUp(byte key)
    {
      if (_error == false)
        DriverInputSimulator.IbSendKeybdUp((ushort)key);
    }

    public bool Init()
    {
      try
      {
        if(DriverInputSimulator.Error.Success == DriverInputSimulator.IbSendInit(DriverInputSimulator.SendType.Logitech, 0, 0))
        {
          _error = false;
          _log.Info("Successfully loaded driver keyboard");
          return true;
        }
        else
        {
          _log.Info("Fail to load driver keyboard");
          return false;
        }
      }
      catch
      {
        _log.Info("Fail to load driver keyboard");
        _error = true;
        return false;
      }
    }

    public void Destroy()
    {
      DriverInputSimulator.IbSendDestroy();
    }
  }
}
