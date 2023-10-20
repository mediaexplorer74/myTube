// Decompiled with JetBrains decompiler
// Type: RykenTube.SearchClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RykenTube
{
  public class SearchClient : VideoListClient
  {
    public string RelatedToVideoId;
    public string ChannelId;
    public YouTubeTime Time;
    public RykenTube.EventType? EventType;
    public RykenTube.Category? Category;
    public YouTubeOrder OrderBy;
    private HttpClient suggestionsClient;
    private string suggestion = "";
    private bool suggBusy;
    public string SearchTerm = "";

    public event EventHandler<SearchSuggestionEventArgs> SuggestionsCompleted;

    public bool SuggestionsBusy => this.suggBusy;

    public SearchClient(int num)
      : base("https://gdata.youtube.com/feeds/api/videos", num)
    {
      this.NeedsRefresh = true;
      this.UseAccessToken = false;
      this.version = 3;
      this.suggestionsClient = new HttpClient((HttpMessageHandler) new HttpClientHandler()
      {
        UseProxy = false
      });
      this.suggestionsClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      if (this.version == 2)
      {
        this.BaseAddress.BaseAddress = "https://gdata.youtube.com/feeds/api/videos?v=2";
        this.BaseAddress.SetValue("v", (object) this.version);
        this.BaseAddress.SetValue("format", (object) 5);
      }
      else if (this.version == 3)
      {
        this.RefreshInternally = true;
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/search";
        this.ClearParts(new Part[1]);
        this.BaseAddress["type"] = "video";
      }
      this.UseCache("videoSearch", new GroupCacheInfo()
      {
        MaxAge = TimeSpan.FromHours(6.0),
        MaxItems = 50
      });
      this.UseRandomQuery = false;
    }

    protected override string GetCacheName()
    {
      string lower = this.SearchTerm.Trim().ToLower();
      int num = (int) this.Time;
      string str1 = num.ToString();
      num = (int) this.OrderBy;
      string str2 = num.ToString();
      string cacheName = lower + str1 + str2;
      if (this.ChannelId != null)
        cacheName += this.ChannelId;
      return cacheName;
    }

    protected override void SetBaseAddress(int page)
    {
      string str = "";
      this.BaseAddress.SetValue("q", (object) this.SearchTerm.Trim().ToLower());
      if (this.version == 2)
      {
        switch (this.Time)
        {
          case YouTubeTime.AllTime:
            str = "all_time";
            break;
          case YouTubeTime.ThisMonth:
            str = "this_month";
            break;
          case YouTubeTime.ThisWeek:
            str = "this_week";
            break;
          case YouTubeTime.Today:
            str = "today";
            break;
        }
        this.BaseAddress.SetValue("time", (object) str);
        switch (this.OrderBy)
        {
          case YouTubeOrder.Relevance:
            str = "relevance";
            break;
          case YouTubeOrder.Published:
            str = "published";
            break;
          case YouTubeOrder.ViewCount:
            str = "viewCount";
            break;
          case YouTubeOrder.Rating:
            str = "rating";
            break;
        }
        this.BaseAddress.SetValue("orderby", (object) str);
      }
      else if (this.version == 3)
      {
        if (this.ChannelId == null)
          this.BaseAddress.RemoveValue("channelId");
        else
          this.BaseAddress.SetValue("channelId", (object) this.ChannelId);
        if (this.EventType.HasValue)
        {
          switch (this.EventType.Value)
          {
            case RykenTube.EventType.Completed:
              this.BaseAddress.SetValue("eventType", (object) "completed");
              break;
            case RykenTube.EventType.Live:
              this.BaseAddress.SetValue("eventType", (object) "live");
              break;
            case RykenTube.EventType.Upcoming:
              this.BaseAddress.SetValue("eventType", (object) "upcoming");
              break;
          }
        }
        else
          this.BaseAddress.RemoveValue("eventType");
        if (this.Category.HasValue)
          this.BaseAddress.SetValue("videoCategoryId", (object) (int) this.Category.Value);
        else
          this.BaseAddress.RemoveValue("videoCategoryId");
        if (this.RelatedToVideoId != null)
          this.BaseAddress.SetValue("relatedToVideoId", (object) this.RelatedToVideoId);
        else
          this.BaseAddress.RemoveValue("relatedToVideoId");
        switch (this.Time)
        {
          case YouTubeTime.ThisMonth:
            this.PublishedAfter = DateTime.UtcNow - TimeSpan.FromDays(30.5);
            this.UsePublishedAfter = true;
            break;
          case YouTubeTime.ThisWeek:
            this.PublishedAfter = DateTime.UtcNow - TimeSpan.FromDays(7.0);
            this.UsePublishedAfter = true;
            break;
          case YouTubeTime.Today:
            this.PublishedAfter = DateTime.UtcNow - TimeSpan.FromDays(1.0);
            this.UsePublishedAfter = true;
            break;
          default:
            this.UsePublishedAfter = false;
            break;
        }
        switch (this.OrderBy)
        {
          case YouTubeOrder.Relevance:
            this.BaseAddress["order"] = "relevance";
            break;
          case YouTubeOrder.Published:
            this.BaseAddress["order"] = "date";
            break;
          case YouTubeOrder.ViewCount:
            this.BaseAddress["order"] = "viewCount";
            break;
          case YouTubeOrder.Rating:
            this.BaseAddress["order"] = "rating";
            break;
          default:
            this.BaseAddress.RemoveValue("order");
            break;
        }
        if (YouTube.Region != (RegionInfo) null)
          this.BaseAddress["regionCode"] = YouTube.Region.CountryCode;
        else
          this.BaseAddress.RemoveValue("regionCode");
      }
      base.SetBaseAddress(page);
    }

    public async Task<string[]> GetSuggestions(string term)
    {
      if (term.Length == 0)
        return new string[0];
      this.suggBusy = true;
      this.suggestion = term;
      URLConstructor urlConstructor = new URLConstructor("http://clients1.google.com/complete/search")
      {
        ["q"] = term
      };
      urlConstructor["client"] = urlConstructor["gs_ri"] = "youtube";
      urlConstructor["gl"] = "us";
      urlConstructor["ds"] = "yt";
      urlConstructor["cp"] = term.Length.ToString();
      urlConstructor["gs_rn"] = "23";
      string str1 = await this.suggestionsClient.GetStringAsync(urlConstructor.ToUri(UriKind.Absolute, URLDisplayMode.ExcludeNullValues));
      if (str1.Contains("window.google.ac.h"))
        str1 = str1.Replace("window.google.ac.h", "");
      try
      {
        int startIndex = str1.IndexOf('[');
        int num = str1.LastIndexOf(']');
        JArray jarray = (JArray) JArray.Parse(str1.Substring(startIndex, num - startIndex + 1))[1];
        string[] suggestions = new string[jarray.Count];
        for (int index = 0; index < suggestions.Length; ++index)
          suggestions[index] = (string) jarray[index][(object) 0];
        return suggestions;
      }
      catch
      {
      }
      string str2 = Regex.Unescape(str1);
      List<string> stringList = new List<string>();
      bool flag = false;
      string str3 = "";
      for (int index = 0; index < str2.Length; ++index)
      {
        char ch = str2[index];
        if (ch == '"' || ch == '"')
        {
          flag = !flag;
          if (!flag)
          {
            if (str3.Length > 1 && str3 != this.suggestion && str3.Length > 0)
            {
              if (!stringList.Contains(str3))
              {
                try
                {
                  str3 = Regex.Unescape(str3);
                }
                catch
                {
                }
                stringList.Add(str3);
              }
            }
            str3 = "";
          }
        }
        if (flag && ch != '"')
          str3 += ch.ToString();
      }
      try
      {
        stringList.RemoveAt(stringList.Count - 1);
      }
      catch
      {
      }
      this.suggBusy = false;
      return stringList.ToArray();
    }

    public void CancelSuggestions()
    {
      try
      {
        this.suggestionsClient.CancelPendingRequests();
      }
      catch
      {
      }
    }
  }
}
