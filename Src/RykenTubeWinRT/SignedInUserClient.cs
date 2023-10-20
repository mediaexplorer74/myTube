// Decompiled with JetBrains decompiler
// Type: RykenTube.SignedInUserClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;

namespace RykenTube
{
  public class SignedInUserClient : VideoListClient
  {
    public UserFeed Type;

    public SignedInUserClient(UserFeed type, int num)
      : base("https://gdata.youtube.com/feeds/api/users/default/", num)
    {
      this.RequiresSignIn = true;
      this.Type = type;
      string str = "";
      this.UseAccessToken = true;
      this.UseAPIKey = true;
      this.version = 3;
      if (this.version == 2)
      {
        switch (type)
        {
          case UserFeed.Uploads:
            str = "uploads";
            break;
          case UserFeed.Favorites:
            str = "favorites";
            this.Fields = "entry(title,yt:statistics(@viewCount),yt:rating,yt:accessControl,yt:favoriteId,media:group(yt:videoid,media:thumbnail(@yt:name,@url),media:credit,media:description,yt:duration,yt:uploaded))";
            break;
          case UserFeed.Subscriptions:
            str = "newsubscriptionvideos";
            break;
          case UserFeed.History:
            str = "watch_history";
            break;
          case UserFeed.Recommended:
            str = "recommendations";
            this.BaseAddress.SetValue("key", (object) YouTube.DeveloperKey);
            this.UseRandomQuery = false;
            break;
        }
        this.BaseAddress.BaseAddress += str;
        this.BaseAddress.SetValue("v", (object) "2.1");
      }
      else
      {
        if (this.version != 3)
          return;
        switch (type)
        {
          case UserFeed.Subscriptions:
            this.Mine = false;
            this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/activities";
            this.BaseAddress.SetValue("home", (object) "true");
            this.ClearParts(Part.ContentDetails);
            this.NeedsRefresh = true;
            this.UsePublishedAfter = true;
            this.PublishedAfter = DateTime.UtcNow - TimeSpan.FromDays(2.0);
            this.JsonFilter = (Func<JToken, bool>) (j =>
            {
              JToken jtoken1 = j[(object) "snippet"];
              if (jtoken1 != null)
                return !((string) jtoken1[(object) nameof (type)] != "upload");
              JToken jtoken2 = j[(object) "contentDetails"];
              return jtoken2 == null || jtoken2[(object) "upload"] != null;
            });
            break;
          case UserFeed.History:
            this.UseAccessToken = true;
            this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/playlistItems";
            this.NeedsRefresh = true;
            this.ClearParts(Part.ContentDetails);
            this.UseCache("youtubeHistory", new GroupCacheInfo()
            {
              MaxItems = 100,
              MaxAge = TimeSpan.FromHours(1.0)
            });
            break;
          case UserFeed.Recommended:
            this.Mine = false;
            this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/activities";
            this.BaseAddress.SetValue("home", (object) "true");
            this.ClearParts(Part.ContentDetails);
            this.NeedsRefresh = true;
            this.UsePublishedAfter = true;
            this.PublishedAfter = DateTime.UtcNow - TimeSpan.FromDays(2.0);
            this.JsonFilter = (Func<JToken, bool>) (j =>
            {
              JToken jtoken3 = j[(object) "snippet"];
              if (jtoken3 != null)
                return !((string) jtoken3[(object) nameof (type)] != "recommendation");
              JToken jtoken4 = j[(object) "contentDetails"];
              return jtoken4 == null || jtoken4[(object) "recommendation"] != null;
            });
            break;
          case UserFeed.Feed:
            this.Mine = false;
            this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/activities";
            this.BaseAddress.SetValue("home", (object) "true");
            this.RefreshInternally = false;
            this.ClearParts(Part.Snippet, Part.ContentDetails);
            this.NeedsRefresh = true;
            this.UsePublishedAfter = true;
            this.PublishedAfter = DateTime.UtcNow - TimeSpan.FromDays(5.0);
            this.JsonFilter = (Func<JToken, bool>) (j =>
            {
              JToken jtoken = j[(object) "snippet"];
              return jtoken == null || !((string) jtoken[(object) nameof (type)] == "playlistItem");
            });
            break;
        }
      }
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      if (this.Type != UserFeed.History)
        return;
      this.BaseAddress["playlistId"] = "HL" + UserInfo.RemoveUCFromID(YouTube.UserInfo.ID);
    }
  }
}
