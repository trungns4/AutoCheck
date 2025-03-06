using System;
using System.Collections.Generic;
using System.Configuration;

namespace MXTools
{
  public sealed class GlobalSettings
  {
    private static readonly Lazy<GlobalSettings> _instance = new(() => new GlobalSettings());

    public static GlobalSettings Instance => _instance.Value;

    private readonly Dictionary<string, string> _settingsCache;

    private GlobalSettings()
    {
      _settingsCache = [];

      foreach (string key in ConfigurationManager.AppSettings)
      {
        _settingsCache[key.ToUpper().Trim()] = ConfigurationManager.AppSettings[key];
      }
    }

    public string GetConfigString(string name, string def = "")
    {
      return _settingsCache.TryGetValue(name.ToUpper().Trim(), out var value) ? value : def;
    }
  }
}