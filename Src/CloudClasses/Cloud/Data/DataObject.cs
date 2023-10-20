// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Data.DataObject
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace myTube.Cloud.Data
{
  public abstract class DataObject
  {
    private string id;
    private DateTime dateCreated = DateTime.MinValue;
    private DateTime dateModified = DateTime.MinValue;
    private static DataObjectConverter converter = new DataObjectConverter();

    public string Id
    {
      get => this.id;
      set => this.id = value;
    }

    [JsonProperty]
    public DateTime DateCreated
    {
      get => this.dateCreated;
      internal set => this.dateCreated = value;
    }

    [JsonProperty]
    public DateTime DateModified
    {
      get => this.dateModified;
      internal set => this.dateModified = value;
    }

    public void SetDateModified() => this.DateModified = DateTime.UtcNow;

    public void SetDateCreated() => this.DateCreated = DateTime.UtcNow;

    static DataObject()
    {
      JsonSerializerSettings sett = new JsonSerializerSettings()
      {
        Formatting = Formatting.Indented
      };
      JsonConvert.DefaultSettings = (Func<JsonSerializerSettings>) (() => sett);
    }

    public static T ToObjectSafe<T>(string json) => JToken.Parse(json).ToObject<T>();

    public static T ToObject<T>(string json) => JsonConvert.DeserializeObject<T>(json, (JsonConverter) DataObject.converter);

    public static string ToJson(object o) => JsonConvert.SerializeObject(o, (JsonConverter) DataObject.converter);

    public static T FromJsonStream<T>(Stream stream)
    {
      using (StreamReader streamReader = new StreamReader(stream))
        return DataObject.ToObject<T>(streamReader.ReadToEnd());
    }

    public static Stream ToJsonStream(object o) => (Stream) new MemoryStream(Encoding.UTF8.GetBytes(DataObject.ToJson(o)));
  }
}
