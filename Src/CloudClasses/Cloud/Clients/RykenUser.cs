// TEMP
// Type: myTube.Cloud.Clients.ClientData 

using myTube.Cloud.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace myTube.Cloud.Clients
{
    // Fake class (TEMP
  public class ClientData : DataObject
  {
    private UserMode userMode;

    public int LoginCount { get; set; }

    public string Name { get; set; }

    public DateTime LastLogin { get; set; }

    [DefaultValue(UserMode.Normal)]
    [JsonConverter(typeof (StringEnumConverter))]
    public UserMode UserMode { get; set; }
  }
}
