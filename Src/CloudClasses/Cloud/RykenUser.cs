// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.RykenUser
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;

namespace myTube.Cloud
{
  public class RykenUser : DataObject
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
