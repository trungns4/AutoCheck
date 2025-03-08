using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MXTools.Properties;
using Newtonsoft.Json;

namespace MXTools
{
  public static class LicenseCrypto
  {
    public static string EncryptLicense(LicenseInfo license, string key)
    {
      string json = JsonConvert.SerializeObject(license);
      byte[] keyBytes = DeriveKey(key);
      byte[] iv = new byte[16]; // AES IV (zeroed for simplicity)

      using (Aes aes = Aes.Create())
      {
        aes.Key = keyBytes;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using (MemoryStream ms = new MemoryStream())
        using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
          byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
          cs.Write(jsonBytes, 0, jsonBytes.Length);
          cs.FlushFinalBlock();
          return Convert.ToBase64String(ms.ToArray());
        }
      }
    }

    public static LicenseInfo DecryptLicense(string encryptedText, string key)
    {
      byte[] keyBytes = DeriveKey(key);
      byte[] iv = new byte[16]; // Must match encryption IV
      byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

      using (Aes aes = Aes.Create())
      {
        aes.Key = keyBytes;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using (MemoryStream ms = new MemoryStream(encryptedBytes))
        using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
        using (StreamReader sr = new StreamReader(cs))
        {
          string json = sr.ReadToEnd();
          return JsonConvert.DeserializeObject<LicenseInfo>(json);
        }
      }
    }

    private static byte[] DeriveKey(string key)
    {
      using (SHA256 sha256 = SHA256.Create())
      {
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(key)); // 256-bit key
      }
    }
  }
}
