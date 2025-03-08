using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Newtonsoft.Json;

namespace MXTools
{
  public class LicenseMaker
  {
    private byte[] _privateKey;

    public LicenseMaker(byte[] key)
    {
      _privateKey = key;
    }

    public void CreateLicense(LicenseInfo licenseInfo, string outputFile)
    {
      string json = JsonConvert.SerializeObject(licenseInfo, Newtonsoft.Json.Formatting.Indented);
      string signature = SignData(json);

      File.WriteAllText(outputFile, json + "\n--SIGNATURE--\n" + signature);
    }

    private string SignData(string data)
    {
      using (RSA rsa = RSA.Create())
      {
        rsa.ImportFromPem(Encoding.UTF8.GetChars(_privateKey));
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        byte[] signedBytes = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        string base64Signature = Convert.ToBase64String(signedBytes);

        // Break into lines of 80 characters
        return BreakIntoLines(base64Signature, 80);
      }
    }


    private string BreakIntoLines(string input, int lineLength)
    {
      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < input.Length; i += lineLength)
      {
        sb.AppendLine(input.Substring(i, Math.Min(lineLength, input.Length - i)));
      }
      return sb.ToString().TrimEnd(); // Remove trailing newline
    }
  }
}
