// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistPageClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RykenTube
{
  public class PlaylistPageClient : VideoListClient
  {
    public string PlaylistID { get; set; }

    public PlaylistPageClient(string playlistId)
      : base("https://www.youtube.com/playlist?list=" + playlistId, 15)
    {
      this.PlaylistID = playlistId;
      this.UseAPIKey = false;
      this.version = -1;
      this.IsPlaylist = true;
    }

    protected override void SetBaseAddress(int page)
    {
      this.BaseAddress["list"] = this.PlaylistID;
      base.SetBaseAddress(page);
    }

    protected override async Task<YouTubeEntry[]> ParseOther(string s)
    {
      YouTubeURLInfo[] idsFromHtml = YouTubeURLHelper.GetIDsFromHtml(s);
      List<YouTubeEntry> source = new List<YouTubeEntry>();
      foreach (YouTubeURLInfo youTubeUrlInfo in idsFromHtml)
      {
        YouTubeURLInfo i = youTubeUrlInfo;
        if (i.Type == YouTubeURLType.Video && source.FirstOrDefault<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == i.ID)) == null)
        {
          List<YouTubeEntry> youTubeEntryList = source;
          YouTubeEntry youTubeEntry = new YouTubeEntry();
          youTubeEntry.ID = i.ID;
          youTubeEntry.NeedsRefresh = true;
          youTubeEntryList.Add(youTubeEntry);
        }
      }
      return source.ToArray();
    }

    public override TypeConstructor GetTypeConstructor() => new TypeConstructor(this.GetType(), new object[1]
    {
      (object) this.PlaylistID
    });
  }
}
