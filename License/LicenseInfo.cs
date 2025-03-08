using System;
using Newtonsoft.Json;

namespace MXTools
{
  public class LicenseInfo
  {
    public string User { get; set; }

    public DateTime ExpireDate { get; set; }

    public DateTime Created { get; set; }

    public string CreatedBy { get; set; }

    public string Note { get; set; }

    public LicenseInfo()
    {
      User = "";
      ExpireDate = DateTime.MinValue;
      Created = DateTime.MinValue;
      CreatedBy = "";
      Note = "";
    }
  }
}
