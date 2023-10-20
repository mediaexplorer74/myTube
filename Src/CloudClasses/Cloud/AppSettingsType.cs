// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.AppSettingsType
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace myTube.Cloud
{
  [JsonConverter(typeof (StringEnumConverter))]
  public enum AppSettingsType
  {
    Boolean,
    String,
    FloatingPoint,
    Integer,
  }
}
