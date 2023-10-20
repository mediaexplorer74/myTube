// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Threading.Tasks;

namespace RykenTube
{
  public class PlaylistClient : VideoListClient
  {
    protected string id;
    private string token;

    public string ID => this.id;

    public PlaylistClient(string playlistID, int howMany, string accessToken = null)
      : base("https://gdata.youtube.com/feeds/api/playlists/" + playlistID, howMany)
    {
      this.DefaultCacheGroupName = "playlistClient";
      this.IsPlaylist = true;
      this.UseRandomQuery = false;
      this.version = 3;
      if (this.version == 2)
      {
        this.id = playlistID;
        if (this.id == "watchlater")
          this.BaseAddress.BaseAddress = "https://gdata.youtube.com/feeds/api/users/default/watch_later";
        this.BaseAddress.SetValue("v", (object) "2.1");
      }
      else if (this.version == 3)
      {
        this.NeedsRefresh = true;
        this.ClearParts(new Part[1]);
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/playlistItems";
        this.BaseAddress["playlistId"] = playlistID;
        this.id = playlistID;
        this.UseFields = true;
        this.FieldsV3 = "kind, nextPageToken, items(kind, id, snippet(title, resourceId(videoId), playlistId), contentDetails(videoId), status)";
        this.UseFieldsOnRefreshClient = true;
        this.RefreshClientFields = "kind, items(kind, id, snippet(title, publishedAt, channelId, thumbnails, description, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
      }
      if (accessToken == null)
        return;
      this.token = accessToken;
    }

    public PlaylistClient(int howMany)
      : base("https://gdata.youtube.com/feeds/api/playlists/", howMany)
    {
      this.IsPlaylist = true;
      this.ClearParts(new Part[1]);
      this.version = 3;
      if (this.version != 3)
        return;
      this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/playlistItems";
      this.UseFields = true;
      this.FieldsV3 = "kind, nextPageToken, items(kind, id, snippet(title, resourceId(videoId), playlistId), contentDetails(videoId), status)";
      this.UseFieldsOnRefreshClient = true;
      this.RefreshClientFields = "kind, items(kind, id, snippet(title, publishedAt, channelId, thumbnails, description, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
    }

    public static PlaylistClient GetWatchLaterClient(int howMany, string token = null)
    {
      PlaylistClient watchLaterClient = new PlaylistClient("watchlater", howMany);
      if (watchLaterClient.version == 2)
      {
        watchLaterClient.BaseAddress.BaseAddress = "https://gdata.youtube.com/feeds/api/users/default/watch_later";
        watchLaterClient.BaseAddress.SetValue("v", (object) "2.1");
      }
      if (token != null)
        watchLaterClient.token = token;
      return watchLaterClient;
    }

    public static PlaylistClient GetLikesClient(int howMany, string token = null) => (PlaylistClient) new PlaylistClient.LikesClient(howMany);

    protected override Task<YouTubeEntry[]> StringDownloaded(string result) => base.StringDownloaded(result);

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      if (this.token != null && !YouTube.IsSignedIn)
        this.BaseAddress.SetValue("access_token", (object) this.token);
      if (this.id != "watchlater")
      {
        if (this.id == null)
          return;
        this.BaseAddress["playlistId"] = this.id;
      }
      else
      {
        if (YouTube.UserInfo == null)
          return;
        if (YouTube.UserInfo.ID.Length > 4)
          this.BaseAddress["playlistId"] = this.id = YouTube.UserInfo.WatchLaterPlaylist;
        else
          this.BaseAddress["playlistId"] = "WL";
      }
    }

    public override TypeConstructor GetTypeConstructor() => new TypeConstructor(this.GetType(), new object[3]
    {
      (object) this.id,
      (object) this.howMany,
      !string.IsNullOrWhiteSpace(this.token) ? (object) this.token : (!string.IsNullOrWhiteSpace(YouTube.AccessToken) ? (object) YouTube.AccessToken : (object) (string) null)
    });

    protected override string GetCacheName() => this.id;

    protected override YouTubeClient<YouTubeEntry> GetDuplicateOverride() => (YouTubeClient<YouTubeEntry>) new PlaylistClient(this.id, this.howMany, this.AccessToken);

    public class LikesClient : PlaylistClient
    {
      private static string getBaseAddress()
      {
        string baseAddress = "LL";
        if (YouTube.UserInfo != null)
        {
          string str = YouTube.UserInfo.ID;
          if (str.StartsWith("UC"))
            str = str.Substring(2);
          baseAddress = "LL" + str;
        }
        return baseAddress;
      }

      internal LikesClient(int howMany)
        : base(PlaylistClient.LikesClient.getBaseAddress(), howMany)
      {
        this.UseAccessToken = true;
      }
    }
  }
}
