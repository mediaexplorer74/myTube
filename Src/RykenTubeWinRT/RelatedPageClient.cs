// Decompiled with JetBrains decompiler
// Type: RykenTube.RelatedPageClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace RykenTube
{
  public class RelatedPageClient : VideoListClient
  {
    private string id;

    public RelatedPageClient(string videoID)
      : base("https://www.youtube.com/watch", 15)
    {
      this.UseAccessToken = false;
      this.UseFields = false;
      this.UseAPIKey = false;
      this.version = -1;
      this.id = videoID;
      this.NeedsRefresh = true;
    }

    protected override void SetPagination(int page)
    {
    }

    protected override void SetBaseAddress(int page) => this.BaseAddress["v"] = this.id;

    protected override async Task<string> GetResultString(int page)
    {
      string resultString;
      try
      {
        resultString = await base.GetResultString(page);
      }
      catch
      {
        throw;
      }
      return resultString;
    }

    protected override async Task<YouTubeEntry[]> ParseOther(string s)
    {
      RelatedPageClient relatedPageClient = this;
      List<string> stringList = new List<string>();
      string content = s;
      int startIndex = content.IndexOf("class=\"alerts-wrapper\"");
      if (startIndex != -1)
        content = content.Substring(startIndex);
      foreach (YouTubeURLInfo youTubeUrlInfo in YouTubeURLHelper.GetIDsFromHtml(content))
      {
        if (youTubeUrlInfo.Type == YouTubeURLType.Video && !youTubeUrlInfo.OriginalURL.EndsWith("...") && youTubeUrlInfo.ID != relatedPageClient.id && !stringList.Contains(youTubeUrlInfo.ID))
          stringList.Add(youTubeUrlInfo.ID);
      }
      YouTubeEntry[] other = new YouTubeEntry[stringList.Count];
      for (int index1 = 0; index1 < other.Length; ++index1)
      {
        YouTubeEntry[] youTubeEntryArray = other;
        int index2 = index1;
        YouTubeEntry youTubeEntry = new YouTubeEntry();
        youTubeEntry.ID = stringList[index1];
        youTubeEntry.NeedsRefresh = relatedPageClient.NeedsRefresh;
        youTubeEntryArray[index2] = youTubeEntry;
      }
      return other;
    }

    protected override string GetCacheName() => this.id;

    public async Task<YouTubeEntry> GetUpNext()
    {
      RelatedPageClient relatedPageClient = this;
      using (HttpClient client = new HttpClient())
      {
        URLConstructor urlConstructor = new URLConstructor("https://www.youtube.com/watch");
        urlConstructor["v"] = relatedPageClient.id;
        if (YouTube.IsSignedIn)
          urlConstructor["access_token"] = YouTube.AccessToken;
        else if (relatedPageClient.AccessToken != null)
          urlConstructor["access_token"] = relatedPageClient.AccessToken;
        string content = await client.GetStringAsync(urlConstructor.ToUri());
        int startIndex = content.IndexOf("class=\"watch-sidebar-body\"");
        if (startIndex != -1)
          content = content.Substring(startIndex);
        YouTubeURLInfo[] array = ((IEnumerable<YouTubeURLInfo>) YouTubeURLHelper.GetIDsFromHtml(content)).Where<YouTubeURLInfo>((Func<YouTubeURLInfo, bool>) (i => i.Type == YouTubeURLType.Video)).ToArray<YouTubeURLInfo>();
        if (array.Length == 0)
          return (YouTubeEntry) null;
        YouTubeURLInfo youTubeUrlInfo = array[0];
        return await relatedPageClient.GetRefreshClient().GetInfo(youTubeUrlInfo.ID);
      }
    }
  }
}
