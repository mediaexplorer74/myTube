// Decompiled with JetBrains decompiler
// Type: RykenTube.UserClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Threading.Tasks;

namespace RykenTube
{
  public class UserClient : VideoListClient
  {
    private string user;
    private UserFeed Type;

    public UserClient(UserFeed type, string name, int num)
      : base((YouTube.HTTPS ? "https" : "http") + "://gdata.youtube.com/feeds/api/users/" + name + "/" + (type == UserFeed.Uploads ? "uploads" : "favorites"), num)
    {
      this.Type = type;
      this.user = name;
      this.version = 3;
      this.UseRandomQuery = false;
      this.DefaultCacheGroupName = "userVideos";
      this.UseCache("userVideos", new GroupCacheInfo()
      {
        MaxItems = 200,
        MaxAge = TimeSpan.FromMinutes(15.0)
      });
      this.CacheRequiresSameAccount = true;
      if (this.version == 2)
      {
        this.BaseAddress.SetValue("v", (object) "2.1");
        this.UseAccessToken = false;
      }
      else
      {
        if (this.version != 3)
          return;
        this.UseAccessToken = true;
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/playlistItems";
        this.UseFields = true;
        this.FieldsV3 = "kind, nextPageToken, items(kind, id, snippet(title, resourceId(videoId), playlistId), contentDetails(videoId), status)";
        this.UseFieldsOnRefreshClient = true;
        this.RefreshClientFields = "kind, items(kind, id, snippet(title, publishedAt, channelId, thumbnails, description, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
        this.NeedsRefresh = true;
        this.ClearParts(Part.Snippet, Part.ContentDetails);
        string str = "";
        switch (type)
        {
          case UserFeed.Uploads:
            str = "UU";
            break;
          case UserFeed.Favorites:
            str = "FL";
            this.ClearParts(new Part[1]);
            break;
        }
        this.BaseAddress["playlistId"] = str + UserInfo.RemoveUCFromID(name);
      }
    }

    protected override Task<YouTubeEntry[]> StringDownloaded(string result) => base.StringDownloaded(result);

    protected override string GetCacheName() => UserInfo.RemoveUCFromID(this.user) + (object) (int) this.Type;
  }
}
