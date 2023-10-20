// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.AppSettings
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace myTube.Cloud
{
  public class AppSettings : DataObject
  {
    public string AppName { get; set; }

    public Dictionary<string, AppSettingsValue> Settings { get; set; } 
            = new Dictionary<string, AppSettingsValue>();

        // fallback = null
        public T GetSettingValue<T>(UserMode mode, string name, T fallback = default)
        {
            return this.Settings == null || !this.Settings.ContainsKey(name) 
                ? fallback : this.Settings[name].GetValue<T>(mode, fallback);
        }

        public bool RemoveSetting(string name)
        {
            return this.Settings.ContainsKey(name) && this.Settings.Remove(name);
        }

        public bool CreateSetting(string name, AppSettingsType type)
    {
      if (this.Settings.ContainsKey(name))
        return false;
      this.Settings.Add(name, new AppSettingsValue()
      {
        Name = name,
        Type = type,
        Values = new Dictionary<UserMode, JToken>()
      });
      return true;
    }

        public AppSettingsValue GetSetting(string name)
        {
            return this.Settings.ContainsKey(name) ? this.Settings[name] : (AppSettingsValue)null;
        }
    }
}
