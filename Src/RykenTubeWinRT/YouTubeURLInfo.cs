// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeURLInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;

namespace RykenTube
{
  public struct YouTubeURLInfo
  {
    public YouTubeURLType Type;
    public string ID;
    public string OriginalURL;
    private Dictionary<string, string> SecondaryInfo;

    public override string ToString() => this.Type.ToString() + ", " + this.ID;

    public bool HasSecondaryInfo(string key) => this.SecondaryInfo != null && this.SecondaryInfo.ContainsKey(key);

    public string GetSecondaryInfo(string key) => this.SecondaryInfo != null && this.SecondaryInfo.ContainsKey(key) ? this.SecondaryInfo[key] : (string) null;

    public void AddSecondaryInfo(string key, string value)
    {
      if (this.SecondaryInfo == null)
        this.SecondaryInfo = new Dictionary<string, string>();
      if (!this.SecondaryInfo.ContainsKey(key))
        this.SecondaryInfo.Add(key, value);
      else
        this.SecondaryInfo[key] = value;
    }
  }
}
