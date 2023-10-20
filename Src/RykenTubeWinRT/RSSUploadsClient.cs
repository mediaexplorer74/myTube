// Decompiled with JetBrains decompiler
// Type: RykenTube.RSSUploadsClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class RSSUploadsClient : VideoListClient
  {
    public string UserId { get; set; }

    public RSSUploadsClient()
      : base("https://www.youtube.com/feeds/videos.xml", 15)
    {
      this.version = 2;
      this.UseAccessToken = false;
      this.UseAPIKey = false;
      this.UseFields = false;
      this.UseRandomQuery = false;
      this.NeedsRefresh = false;
      this.RefreshInternally = false;
    }

    public RSSUploadsClient(string userId)
      : this()
    {
      this.UserId = userId;
    }

    protected override void SetBaseAddress(int page)
    {
      this.BaseAddress["channel_id"] = this.UserId;
      base.SetBaseAddress(page);
      this.BaseAddress.RemoveValue("start-index");
      this.BaseAddress.RemoveValue("max-results");
    }

    protected override async Task<YouTubeEntry[]> ParseV2(XDocument xdoc)
    {
      xdoc.Element(YouTube.Atom("feed"));
      List<XElement> list = xdoc.Descendants(YouTube.Atom("entry")).ToList<XElement>();
      YouTubeEntry[] v2 = new YouTubeEntry[list.Count];
      for (int index = 0; index < list.Count; ++index)
        v2[index] = YouTubeEntry.FromRSS(list[index]);
      return v2;
    }
  }
}
