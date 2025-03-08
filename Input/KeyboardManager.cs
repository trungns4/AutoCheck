using log4net;
using MXTools.Input;
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
      if (kbOpt == "win-si")
      {
        _keyboard = new WinKeyboardSI();
        _log.Info("Loaded Windows Keyboard (SendInput)");
      }
      else if (kbOpt == "win-ke" || kbOpt == "win")
      {
        _keyboard = new WinKeyboardKE();
        _log.Info("Loaded Windows Keyboard (kbd_event)");
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
