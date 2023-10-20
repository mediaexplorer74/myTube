// Decompiled with JetBrains decompiler
// Type: RykenTube.RelatedClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class RelatedClient : VideoListClient
  {
    public string ID;

    public RelatedClient(string id, int num)
      : base("https://gdata.youtube.com/feeds/api/videos/" + id + "/related", num)
    {
      this.version = 3;
      this.UseRandomQuery = false;
      if (this.version == 3)
      {
        this.NeedsRefresh = true;
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/search?part=snippet&relatedToVideoId=" + id + "&type=video";
      }
      else if (this.version == 2)
        this.BaseAddress.SetValue("v", (object) 2);
      this.ID = id;
      this.UseAccessToken = false;
      this.ClearParts(new Part[1]);
      this.UseCache("relatedVideos", new GroupCacheInfo()
      {
        MaxItems = 100,
        MaxAge = TimeSpan.FromDays(4.0)
      });
    }

    protected override string GetCacheName() => this.ID;
  }
}
