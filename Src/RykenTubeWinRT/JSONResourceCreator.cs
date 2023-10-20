// Decompiled with JetBrains decompiler
// Type: RykenTube.JSONResourceCreator
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RykenTube
{
  public static class JSONResourceCreator
  {
    public static JObject CreateVideoResource(YouTubeEntry entry)
    {
      JObject json1 = new JObject();
      json1.SetValue("kind", "youtube#video");
      json1.SetValue("id", entry.ID);
      json1.SetValue("snippet.title", entry.Title);
      json1.SetValue("snippet.description", entry.Description);
      json1.SetValue("snippet.channelId", entry.Author);
      json1.SetValue("snippet.channelTitle", entry.AuthorDisplayName);
      if (entry.Tags.Count > 0)
        json1.SetValue<string>("snippet.tags", (IEnumerable<string>) entry.Tags);
      if (entry.Category != Category.All)
        json1.SetValue("snippet.categoryId", (long) entry.Category);
      string str1 = (string) null;
      DateTimeOffset? publishAt = entry.PublishAt;
      if (publishAt.HasValue)
      {
        JObject json2 = json1;
        publishAt = entry.PublishAt;
        DateTime utcDateTime = publishAt.Value.UtcDateTime;
        json2.SetValue("status.publishAt", utcDateTime);
      }
      json1.SetValue("status.embeddable", new bool?(entry.Embeddable));
      json1.SetValue("status.publicStatsVisible", new bool?(entry.PublicStatsVisible));
      switch (entry.PrivacyStatus)
      {
        case PrivacyStatus.Public:
          str1 = "public";
          break;
        case PrivacyStatus.Private:
          str1 = "private";
          break;
        case PrivacyStatus.Unlisted:
          str1 = "unlisted";
          break;
      }
      json1.SetValue("status.privacyStatus", str1);
      json1.SetValue("status.embeddable", new bool?(entry.Embeddable));
      string str2 = (string) null;
      switch (entry.License)
      {
        case License.YouTube:
          str2 = "youtube";
          break;
        case License.CreativeCommons:
          str2 = "creativeCommon";
          break;
      }
      json1.SetValue("status.license", str2);
      return json1;
    }

    public static void SetValue(this JToken json, string path, DateTime value) => json.setValue(path, new JValue(value));

    public static void SetValue(this JToken json, string path, long value) => json.setValue(path, new JValue(value));

    public static void SetValue(this JToken json, string path, ulong value) => json.setValue(path, new JValue(value));

    public static void SetValue(this JToken json, string path, double value) => json.setValue(path, new JValue(value));

    public static void SetValue(this JToken json, string path, string value)
    {
      if (value == null)
        json.setValue(path, (JValue) null);
      else
        json.setValue(path, new JValue(value));
    }

    public static void SetValue(this JToken json, string path, bool? value)
    {
      if (value.HasValue)
        json.setValue(path, new JValue((object) value));
      else
        json.setValue(path, (JValue) null);
    }

    public static void SetValue<T>(this JToken json, string path, IEnumerable<T> value) => json.setCollection<T>(path, value);

    private static JObject findAndCreateParent(this JToken json, string path)
    {
      string[] source = path.Split('.');
      ((IEnumerable<string>) source).LastOrDefault<string>();
      if (!(json is JObject andCreateParent))
        return (JObject) null;
      for (int index = 0; index < source.Length - 1; ++index)
      {
        JToken jtoken = andCreateParent[source[index]];
        if (jtoken != null)
        {
          if (!(jtoken is JObject))
            return (JObject) null;
          andCreateParent = jtoken as JObject;
        }
        else
        {
          JObject jobject = new JObject();
          andCreateParent.Add(source[index], (JToken) jobject);
          andCreateParent = jobject;
        }
      }
      return andCreateParent;
    }

    private static void setValue(this JToken json, string path, JValue value)
    {
      string key = ((IEnumerable<string>) path.Split('.')).LastOrDefault<string>();
      JToken andCreateParent = (JToken) json.findAndCreateParent(path);
      if (andCreateParent[(object) key] == null && (value == null || value.Value == null))
        return;
      andCreateParent[(object) key] = (JToken) value;
    }

    private static void setCollection<T>(this JToken json, string path, IEnumerable<T> value)
    {
      string key = ((IEnumerable<string>) path.Split('.')).LastOrDefault<string>();
      JToken andCreateParent = (JToken) json.findAndCreateParent(path);
      if (andCreateParent[(object) key] == null && value == null)
        return;
      JArray jarray = new JArray((object) value);
      andCreateParent[(object) key] = (JToken) jarray;
    }

    public static T GetValue<T>(this JToken json, T fallBack, params string[] path)
    {
      JToken jtoken = json;
      for (int index = 0; index < path.Length; ++index)
      {
        jtoken = jtoken[(object) path[index]];
        if (jtoken == null)
          break;
      }
      return jtoken != null ? jtoken.ToObject<T>() : fallBack;
    }

    public static bool GetValue<T>(
      this JToken json,
      out T outValue,
      T fallBack,
      params string[] path)
    {
      JToken jtoken = json;
      for (int index = 0; index < path.Length; ++index)
      {
        jtoken = jtoken[(object) path[index]];
        if (jtoken == null)
          break;
      }
      if (jtoken != null)
      {
        outValue = jtoken.ToObject<T>();
        return true;
      }
      outValue = fallBack;
      return false;
    }
  }
}
