// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.ExceptionData
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using myTube.Cloud.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace myTube.Cloud
{
  public class ExceptionData : DataObject
  {
    public string TypeName { get; set; }

    public string Message { get; set; }

    public string EventHandlerMessage { get; set; }

    public string StackTrace { get; set; }

    public List<string> Versions { get; set; }

    public List<string> Devices { get; set; }

    [JsonIgnore]
    public double ScoreProperty => this.Score();

    [JsonIgnore]
    public string MinVersion
    {
      get
      {
        if (this.Versions.Count > 0)
        {
          Version version1 = (Version) null;
          foreach (string version2 in this.Versions)
          {
            Version result;
            if (Version.TryParse(version2, out result) && (version1 == (Version) null || result < version1))
              version1 = result;
          }
          if (version1 != (Version) null)
            return version1.ToString();
        }
        return "1.0.0.0";
      }
    }

    [JsonIgnore]
    public string MaxVersion
    {
      get
      {
        if (this.Versions.Count > 0)
        {
          Version version1 = (Version) null;
          foreach (string version2 in this.Versions)
          {
            Version result;
            if (Version.TryParse(version2, out result) && (version1 == (Version) null || result > version1))
              version1 = result;
          }
          if (version1 != (Version) null)
            return version1.ToString();
        }
        return "1.0.0.0";
      }
    }

    [DefaultValue(0)]
    public int Times { get; set; }

    [DefaultValue(false)]
    public bool CausedCrash { get; set; }

    public string AppName { get; set; }

    public ExceptionData()
    {
      this.Versions = new List<string>();
      this.Devices = new List<string>();
    }

    public ExceptionData(Exception ex)
      : this()
    {
      this.TypeName = ex.GetType().ToString();
      this.Message = ex.Message;
      this.StackTrace = ex.StackTrace;
    }

    public double Score()
    {
      double num1 = (double) this.Times / 2.0;
      if (num1 < 2.0)
        num1 = 2.0;
      double num2 = num1 * Math.Max((double) this.Devices.Count / 4.0, 1.0) * Math.Max((double) this.Versions.Count / 2.0, 1.0);
      if (this.CausedCrash)
      {
        double num3 = num2 + 1000.0;
        num2 = num3 <= 0.0 ? num3 * 0.5 : num3 * 2.0;
      }
      TimeSpan timeSpan1 = DateTime.UtcNow - this.DateCreated;
      TimeSpan timeSpan2 = TimeSpan.FromDays(3.0);
      if (timeSpan1 <= timeSpan2)
        num2 += (timeSpan2.TotalDays - timeSpan1.TotalDays) * 300.0;
      double num4 = num2 - (DateTime.UtcNow - this.DateModified).TotalMinutes;
      Version version1 = (Version) null;
      foreach (string version2 in this.Versions)
      {
        Version result;
        if (Version.TryParse(version2, out result) && (version1 == (Version) null || result > version1))
          version1 = result;
      }
      return num4;
    }
  }
}
