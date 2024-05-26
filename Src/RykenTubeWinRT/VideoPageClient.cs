// RykenTube.VideoPageClient

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RykenTube
{
  public class VideoPageClient : VideoListClient
  {
    private string address;

    public VideoPageClient(string page)
      : base(page, 15)
    {
      this.address = page;
      this.UseAccessToken = true;
      this.UseFields = false;
      this.UseAPIKey = false;
      this.version = -1;
      this.RefreshInternally = true;
    }

    protected override void SetPagination(int page)
    {
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.BaseAddress.BaseAddress = this.address;
    }

    protected override async Task<YouTubeEntry[]> ParseOther(string s)
    {
      YouTubeURLInfo[] idsFromHtml = YouTubeURLHelper.GetIDsFromHtml(s);
      List<YouTubeEntry> source = new List<YouTubeEntry>();
      foreach (YouTubeURLInfo youTubeUrlInfo in idsFromHtml)
      {
        YouTubeURLInfo i = youTubeUrlInfo;
        if (source.Count<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == i.ID)) <= 0 && i.Type == YouTubeURLType.Video)
        {
          List<YouTubeEntry> youTubeEntryList = source;
          YouTubeEntry youTubeEntry = new YouTubeEntry();
          youTubeEntry.ID = i.ID;
          youTubeEntryList.Add(youTubeEntry);
        }
      }
      return source.ToArray();
    }
  }
}
