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

    public void Init()
    {
      try
      {
        if (DriverInputSimulator.Error.Success == DriverInputSimulator.IbSendInit(DriverInputSimulator.SendType.Logitech, 0, 0))
        {
          _error = false;
          _log.Info("Successfully loaded driver keyboard");
        }
        else
        {
          _log.Info("Fail to load driver keyboard");
        }
      }
      catch
      {
        _log.Info("Fail to load driver keyboard");
      }
    }

    public void Destroy()
    {
      DriverInputSimulator.IbSendDestroy();
    }
  }
}
