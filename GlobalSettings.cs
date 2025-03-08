using System;
using System.Collections.Generic;
using System.Configuration;

public sealed class GlobalSettings
{
  private static readonly Lazy<GlobalSettings> _instance = new Lazy<GlobalSettings>(() => new GlobalSettings());

  public static GlobalSettings Instance => _instance.Value;

  private readonly Dictionary<string, string> _settingsCache;

  private readonly int _threadJoinWaitingTime;

  public int ThreadJoinWaitingTime => _threadJoinWaitingTime;

  private GlobalSettings()
  {
    _settingsCache = new Dictionary<string, string>();

    foreach (string key in ConfigurationManager.AppSettings)
    {
      _settingsCache[key.ToUpper().Trim()] = ConfigurationManager.AppSettings[key];
    }

    _threadJoinWaitingTime = GetConfigInt("thread-join", 500);
  }

  public string GetConfigString(string name, string def = "")
  {
    return _settingsCache.TryGetValue(name.ToUpper().Trim(), out var value) ? value : def;
  }

  public int GetConfigInt(string name, int def = 0)
  {
    return _settingsCache.TryGetValue(name.ToUpper().Trim(), out var value) && int.TryParse(value, out var result)
        ? result
        : def;
  }
}
