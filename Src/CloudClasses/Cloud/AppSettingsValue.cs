// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.AppSettingsValue
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myTube.Cloud
{
  public class AppSettingsValue
  {
    private static UserMode[] UserModes;

    public Dictionary<UserMode, JToken> Values { get; set; }

    public AppSettingsType Type { get; set; }

    public string Name { get; set; }

    public void SetValue(UserMode mode, object value)
    {
      if (this.Values.ContainsKey(mode))
        this.Values[mode] = JToken.FromObject(value);
      else
        this.Values.Add(mode, JToken.FromObject(value));
    }

    //                                    fallback = null
    public T GetValue<T>(UserMode mode, T fallback = default)
    {
      if (this.Values == null)
        return fallback;
      if (AppSettingsValue.UserModes == null)
        AppSettingsValue.UserModes = Enum.GetValues(typeof (UserMode)).Cast<UserMode>().ToArray<UserMode>();
      for (int index = Array.IndexOf<UserMode>(AppSettingsValue.UserModes, mode); index >= 0; --index)
      {
        if (this.Values.ContainsKey(AppSettingsValue.UserModes[index]))
        {
          JToken jtoken = this.Values[AppSettingsValue.UserModes[index]];
          if (jtoken != null)
          {
            try
            {
              return jtoken.ToObject<T>();
            }
            catch
            {
            }
          }
        }
      }
      return fallback;
    }

    public bool RemoveValue(UserMode mode) => this.Values.ContainsKey(mode) && this.Values.Remove(mode);

    public bool HasValue(UserMode mode) => this.Values.ContainsKey(mode);
  }
}
