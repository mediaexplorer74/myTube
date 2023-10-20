// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Data.CustomResolver
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myTube.Cloud.Data
{
  internal class CustomResolver : DefaultContractResolver
  {
    protected override IList<JsonProperty> CreateProperties(
      Type type,
      MemberSerialization memberSerialization)
    {
      IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);
      JsonProperty jsonProperty = properties.Where<JsonProperty>((Func<JsonProperty, bool>) 
          (p => p.PropertyName == "DateCreated")).FirstOrDefault<JsonProperty>();
      if (jsonProperty == null)
        return properties;
      jsonProperty.PropertyType = typeof (string);
      return properties;
    }
  }
}
