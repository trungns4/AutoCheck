using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCheck
{
  public class Voice
  {
    [JsonProperty("file")]
    public string File;

    [JsonProperty("text")]
    public string Text;

    [JsonProperty("key")]
    public string Key;

    [JsonProperty("order")]
    public int Order;

    public Voice()
    {
      File = "";
      Text = "";
      Key = "";
      Order = 1;
    }
  }
}
