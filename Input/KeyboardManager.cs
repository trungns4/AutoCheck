using log4net;
using System;
using System.Reflection;

namespace MXTools.Input
{
  public sealed class KeyboardManager
  {
    private ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private static readonly Lazy<KeyboardManager> _instance =
        new Lazy<KeyboardManager>(() => new KeyboardManager());

    private IKeyboard _keyboard;

    public static KeyboardManager Instance => _instance.Value;
    public IKeyboard Current => _keyboard;

    private KeyboardManager()
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
  }
}
