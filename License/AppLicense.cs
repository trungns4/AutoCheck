using MXTools.Properties;
using System;
using System.IO;

namespace MXTools.License
{
  class AppLicense
  {
    private static readonly Lazy<AppLicense> _instance = new Lazy<AppLicense>(() => new AppLicense());
    public static AppLicense Instance => _instance.Value;

    private string _license;
    private readonly string _key = "A3F7D9B4C286E15F8D0E4A1B92C5736D";

    private AppLicense() { }

    private void Store(LicenseInfo lic)
    {
      _license = LicenseCrypto.EncryptLicense(lic, _key);
    }

#nullable enable
    public LicenseInfo? GetLicense()
    {
      if (string.IsNullOrEmpty(_license))
        return null;

      return LicenseCrypto.DecryptLicense(_license, _key);
    }
#nullable disable
    public bool ReadLicense(ref long ticks)
    {
      ticks = 0;
      try
      {
        string file = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), Resources.License);
        if (File.Exists(file) == false)
        {
          return false;
        }

        LicenseReader lr = new LicenseReader(Res.public_key);
        var li = lr.ReadLicense(file);

        if (li == null)
        {
          return false;
        }
        else
        {
          Store(li);
          ticks = li.ExpireDate.Ticks;
        }

        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
