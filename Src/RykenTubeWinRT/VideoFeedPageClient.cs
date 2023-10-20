// Decompiled with JetBrains decompiler
// Type: RykenTube.VideoFeedPageClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RykenTube
{
  public class VideoFeedPageClient : VideoListClient
  {
    private List<YouTubeEntry> items;
    private int currentPage;
    private Queue<string> continuation = new Queue<string>();
    private List<string> allContinuations = new List<string>();
    private string feedUrl;
    private bool loadNextPage;
    private string currentString;

    public VideoFeedPageClient(string feedUrl, int howMany)
      : base(feedUrl, howMany)
    {
      this.feedUrl = feedUrl;
      this.items = new List<YouTubeEntry>();
      this.version = -1;
      this.UseAPIKey = false;
    }

    public override bool CanLoadPage(int page) => page == 0 || this.items.Count >= page * this.howMany || this.continuation != null;

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      if (page != 0 && this.items.Count < page * this.howMany && this.continuation.Count > 0)
      {
        string str = this.continuation.Dequeue();
        this.loadNextPage = true;
        this.BaseAddress.BaseAddress = str;
        this.BaseAddress.BaseAddress = "https://www.youtube.com/browse_ajax?action_continuation=1";
      }
      else
      {
        this.BaseAddress.RemoveValue("action_continuation");
        this.BaseAddress.RemoveValue("continuation");
        this.BaseAddress.RemoveValue("target_id");
        this.BaseAddress.RemoveValue("direct_render");
        this.BaseAddress.BaseAddress = this.feedUrl;
        this.loadNextPage = false;
      }
      this.currentPage = page;
    }

    protected override async Task<string> GetResultString(int page)
    {
      if (page == 0 || this.currentString == null || this.loadNextPage)
      {
        if (page == 0)
        {
          this.items.Clear();
          this.continuation.Clear();
          this.allContinuations.Clear();
        }
        this.currentString = await base.GetResultString(page);
        int num = this.loadNextPage ? 1 : 0;
      }
      return this.currentString;
    }

    protected virtual bool ShouldIncludeURLInfo(YouTubeURLInfo info) => true;

    protected override async Task<YouTubeEntry[]> ParseOther(string s)
    {
      VideoFeedPageClient videoFeedPageClient = this;
      int startIndex1 = 0;
      string str1 = s;
      string str2 = s;
      try
      {
        JObject jobject = JObject.Parse(str1);
        if (jobject["content_html"] != null)
          str1 = (string) jobject["content_html"];
        if (jobject["load_more_widget_html"] != null)
          str2 = (string) jobject["load_more_widget_html"];
      }
      catch
      {
      }
      int startIndex2;
      while ((startIndex2 = str2.IndexOf("/browse_ajax?action_continuation", startIndex1)) != -1)
      {
        startIndex1 = startIndex2 + 1;
        int num = str2.IndexOf('"', startIndex2);
        if (num != -1)
        {
          string str3 = WebUtility.HtmlDecode(str2.Substring(startIndex2, num - startIndex2));
          if (!videoFeedPageClient.allContinuations.Contains(str3))
          {
            videoFeedPageClient.allContinuations.Add(str3);
            videoFeedPageClient.continuation.Enqueue(str3);
          }
        }
      }
      if (videoFeedPageClient.items.Count == 0 || videoFeedPageClient.loadNextPage)
      {
        YouTubeURLInfo[] idsFromQuotes1 = YouTubeURLHelper.GetIDsFromQuotes(str1, YouTubeURLType.Video, "data-context-item-id=\"");
        YouTubeURLInfo[] idsFromQuotes2 = YouTubeURLHelper.GetIDsFromQuotes(str1, YouTubeURLType.Video, "videoId\":\"");
        foreach (YouTubeURLInfo youTubeUrlInfo in idsFromQuotes1)
        {
          YouTubeURLInfo i = youTubeUrlInfo;
          if (i.Type == YouTubeURLType.Video && videoFeedPageClient.items.FirstOrDefault<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == i.ID)) == null && videoFeedPageClient.ShouldIncludeURLInfo(i))
          {
            List<YouTubeEntry> items = videoFeedPageClient.items;
            YouTubeEntry youTubeEntry = new YouTubeEntry();
            youTubeEntry.NeedsRefresh = true;
            youTubeEntry.ID = i.ID;
            items.Add(youTubeEntry);
          }
        }
        foreach (YouTubeURLInfo youTubeUrlInfo in idsFromQuotes2)
        {
          YouTubeURLInfo i = youTubeUrlInfo;
          if (i.Type == YouTubeURLType.Video && videoFeedPageClient.items.FirstOrDefault<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == i.ID)) == null && videoFeedPageClient.ShouldIncludeURLInfo(i))
          {
            List<YouTubeEntry> items = videoFeedPageClient.items;
            YouTubeEntry youTubeEntry = new YouTubeEntry();
            youTubeEntry.NeedsRefresh = true;
            youTubeEntry.ID = i.ID;
            items.Add(youTubeEntry);
          }
        }
        if (idsFromQuotes1.Length == 0 && idsFromQuotes2.Length == 0)
        {
          string content = str1;
          string[] strArray = new string[2]
          {
            "href=\"",
            "videoId\":\""
          };
          foreach (YouTubeURLInfo youTubeUrlInfo in YouTubeURLHelper.GetIDsFromHtml(content, strArray))
          {
            YouTubeURLInfo i = youTubeUrlInfo;
            if (i.Type == YouTubeURLType.Video && videoFeedPageClient.items.FirstOrDefault<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == i.ID)) == null && videoFeedPageClient.ShouldIncludeURLInfo(i))
            {
              List<YouTubeEntry> items = videoFeedPageClient.items;
              YouTubeEntry youTubeEntry = new YouTubeEntry();
              youTubeEntry.NeedsRefresh = true;
              youTubeEntry.ID = i.ID;
              items.Add(youTubeEntry);
            }
          }
        }
      }
      int num1 = videoFeedPageClient.currentPage * videoFeedPageClient.howMany;
      if (num1 >= videoFeedPageClient.items.Count)
        return new YouTubeEntry[0];
      int length = Math.Min(videoFeedPageClient.items.Count - num1, videoFeedPageClient.howMany);
      YouTubeEntry[] other = new YouTubeEntry[length];
      for (int index = 0; index < length; ++index)
        other[index] = videoFeedPageClient.items[num1 + index];
      return other;
    }

    public override void DisposeOverride()
    {
      this.continuation?.Clear();
      this.continuation = (Queue<string>) null;
      this.allContinuations?.Clear();
      this.allContinuations = (List<string>) null;
      this.items?.Clear();
      this.items = (List<YouTubeEntry>) null;
      base.DisposeOverride();
    }
  }
}
