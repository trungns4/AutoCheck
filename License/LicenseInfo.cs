using System;
using Newtonsoft.Json;

namespace MXTools
{
  public class LicenseInfo
  {
    [JsonProperty("user")]
    public string User { get; set; }

    [JsonProperty("expiredDate")]
    public DateTime ExpiredDate { get; set; }

    [JsonProperty("created")]
    public DateTime Created { get; set; }

    [JsonProperty("createdBy")]
    public string CreatedBy { get; set; }

    public LicenseInfo()
    {
      User = "";
      ExpiredDate = DateTime.MinValue;
      Created = DateTime.MinValue;
      CreatedBy = "";
    }
  }
}
