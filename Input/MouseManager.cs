using log4net;
using System;
using System.Reflection;

namespace MXTools.Input
{
  public static class MouseManager
  {
    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly IMouse _mouse;

    static MouseManager()
    {
      string mouseOpt = GlobalSettings.Instance.GetConfigString("mouse", "win");
      if (mouseOpt == "win-si")
      {
        _mouse = new WinMouseSI();
        _log.Info("Loaded Windows Mouse (SendInput)");
      }
      else if (mouseOpt == "win-me" || mouseOpt == "win")
      {
        _mouse = new WinMouseME();
        _log.Info("Loaded Windows Mouse (mouse_event)");
      }
    }

    public static IMouse Active => _mouse;
  }
}
