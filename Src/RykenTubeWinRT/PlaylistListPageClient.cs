// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistListPageClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RykenTube
{
  public class PlaylistListPageClient : FeedPageClient<PlaylistEntry>
  {
    private PlaylistType playlistType;
    private string channelId;
    private Dictionary<string, string> headers = new Dictionary<string, string>();
    private PlaylistEntryClient refreshClient;

    public PlaylistListPageClient(PlaylistType playlistType, string channelId, int howMany)
      : base(howMany)
    {
      this.IsPlaylist = true;
      this.version = 0;
      this.playlistType = playlistType;
      if (channelId != null)
        this.channelId = "UC" + UserInfo.RemoveUCFromID(channelId);
      this.UseAPIKey = false;
      this.UseAccessToken = true;
      this.UseFields = false;
      this.UseRandomQuery = false;
      this.RefreshInternally = true;
      this.headers.Add("Referer", "https://www.youtube.com/");
      this.headers.Add("X-SPF-Previous", "https://www.youtube.com/");
      this.headers.Add("X-SPF-Referer", "https://www.youtube.com/");
    }

    public PlaylistListPageClient(PlaylistType playlistType, int howMany)
      : this(playlistType, (string) null, howMany)
    {
    }

    public PlaylistListPageClient(PlaylistType playlistType, string channelId)
      : this(playlistType, channelId, 15)
    {
    }

    public PlaylistListPageClient(PlaylistType playlistType)
      : this(playlistType, (string) null, 15)
    {
    }

    protected override void SetBaseAddress(int page)
    {
      if (this.playlistType == PlaylistType.MyMix)
        this.BaseAddress.BaseAddress = "https://www.youtube.com/feed/music";
      else if (this.channelId != null)
      {
        this.BaseAddress.BaseAddress = "https://www.youtube.com/channel/" + this.channelId + "/playlists?sort=dd";
      }
      else
      {
        if (YouTube.UserInfo == null)
          throw new InvalidOperationException("Must be signed in");
        this.BaseAddress.BaseAddress = "https://www.youtube.com/channel/" + YouTube.UserInfo.ID + "/playlists?sort=dd";
      }
      switch (this.playlistType)
      {
        case PlaylistType.All:
          this.BaseAddress["view"] = "58";
          break;
        case PlaylistType.Created:
          this.BaseAddress["view"] = "1";
          break;
        case PlaylistType.Saved:
          this.BaseAddress["view"] = "52";
          break;
        default:
          this.BaseAddress.RemoveValue("view");
          break;
      }
      base.SetBaseAddress(page);
    }

    protected override IDictionary<string, string> GetHeaders() => (IDictionary<string, string>) this.headers;

    protected override async Task<PlaylistEntry[]> InternalRefresh(
      PlaylistEntry[] items,
      EntryClient<PlaylistEntry> refreshClient)
    {
      PlaylistEntry[] playlistEntryArray = await base.InternalRefresh(items, refreshClient);
      if (this.channelId == null || YouTube.IsSignedIn && YouTube.UserInfo.ID == this.channelId)
      {
        foreach (PlaylistEntry playlistEntry in playlistEntryArray)
        {
          if (this.playlistType == PlaylistType.Saved)
            playlistEntry.Bookmarked = true;
          if (this.playlistType != PlaylistType.Created && YouTube.UserInfo != null && playlistEntry.AuthorID == YouTube.UserInfo.ID)
            playlistEntry.Exists = false;
        }
      }
      return playlistEntryArray;
    }

    protected override IEnumerable<PlaylistEntry> GetItems(string s)
    {
      string content = s;
      int startIndex = content != null ? content.IndexOf("class=\"alerts-wrapper\"") : throw new NullReferenceException("The body is null");
      if (startIndex != -1)
        content = content.Substring(startIndex);
      List<YouTubeURLInfo> list = ((IEnumerable<YouTubeURLInfo>) YouTubeURLHelper.GetIDsFromHtml(content)).Where<YouTubeURLInfo>((Func<YouTubeURLInfo, bool>) (id => id.Type == YouTubeURLType.Playlist || id.HasSecondaryInfo("list"))).ToList<YouTubeURLInfo>();
      List<string> stringList = new List<string>();
      foreach (YouTubeURLInfo youTubeUrlInfo in list)
      {
        string str = youTubeUrlInfo.Type == YouTubeURLType.Playlist ? youTubeUrlInfo.ID : youTubeUrlInfo.GetSecondaryInfo("list");
        if (str != null && !stringList.Contains(str))
          stringList.Add(str);
      }
      PlaylistEntry[] items = new PlaylistEntry[stringList.Count];
      for (int index1 = 0; index1 < stringList.Count; ++index1)
      {
        PlaylistEntry[] playlistEntryArray = items;
        int index2 = index1;
        PlaylistEntry playlistEntry = new PlaylistEntry();
        playlistEntry.ID = stringList[index1];
        playlistEntry.Bookmarked = this.playlistType == PlaylistType.Saved;
        playlistEntryArray[index2] = playlistEntry;
      }
      return (IEnumerable<PlaylistEntry>) items;
    }

    protected override EntryClient<PlaylistEntry> GetRefreshClientOverride()
    {
      if (this.refreshClient == null)
        this.refreshClient = new PlaylistEntryClient();
      return (EntryClient<PlaylistEntry>) this.refreshClient;
    }
  }
}
