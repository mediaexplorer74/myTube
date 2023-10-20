// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Data.DataObjectConverter
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myTube.Cloud.Data
{
  internal class DataObjectConverter : JsonConverter
  {
    private Type[] supportedTypes = new Type[3]
    {
      typeof (DateTime),
      typeof (string),
      typeof (long)
    };

    public override bool CanRead => true;

    public override bool CanWrite => false;

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      if (reader.Value != null)
      {
        if (objectType == typeof (string))
        {
          string s = reader.Value.ToString();
          DateTime result;
          return DateTime.TryParse(s, out result) ? (object) result : (object) s;
        }
        if (objectType == typeof (DateTime))
          return (object) (DateTime) reader.Value;
        if (objectType == typeof (long))
          return (object) (long) reader.Value;
      }
      return (object) null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => throw new NotImplementedException();

    public override bool CanConvert(Type objectType) => ((IEnumerable<Type>) this.supportedTypes).Contains<Type>(objectType);
  }
}
