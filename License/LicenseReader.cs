using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace MXTools
{
  public class LicenseReader
  {
    private readonly byte[] _publicKey;

    public LicenseReader(byte[] key)
    {
      _publicKey = key;
    }


#nullable enable
    public LicenseInfo? ReadLicense(string licenseFile)
    {
      string[] parts = File.ReadAllText(licenseFile)
                           .Split(new[] { "\n--SIGNATURE--\n" }, StringSplitOptions.None);

      if (parts.Length != 2)
      {
        return null;
      }
      string json = parts[0];
      string signature = RemoveLineBreaks(parts[1]); // Ensure the signature is valid

      if (!VerifySignature(json, signature))
      {
        return null;
      }
      return JsonConvert.DeserializeObject<LicenseInfo>(json);
    }
#nullable disable

    private bool VerifySignature(string data, string signature)
    {
      using (RSA rsa = RSA.Create())
      {
        rsa.ImportFromPem(Encoding.UTF8.GetChars(_publicKey));
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] signatureBytes = Convert.FromBase64String(signature);
        return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
      }
    }

    private string RemoveLineBreaks(string input)
    {
      return input.Replace("\r", "").Replace("\n", "").Trim();
    }
  }
}
