using log4net;
using System;
using System.Reflection;

namespace MXTools.Input
{
  public static class KeyboardManager
  {
    private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly IKeyboard _keyboard;

    static KeyboardManager()
    {
      string kbOpt = GlobalSettings.Instance.GetConfigString("keyboard", "win");
      if (kbOpt == "win")
      {
        _keyboard = new WinKeyboard();
        _log.Info("Loaded Windows Keyboard");
      }
      else
      {
        _keyboard = new DriverKeyboard();
        _log.Info("Loaded Driver Keyboard");
      }
    }

    public static IKeyboard Active => _keyboard;
  }
}
