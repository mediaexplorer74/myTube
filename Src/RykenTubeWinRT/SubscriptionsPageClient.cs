// Decompiled with JetBrains decompiler
// Type: RykenTube.SubscriptionsPageClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Threading.Tasks;

namespace RykenTube
{
  public class SubscriptionsPageClient : VideoFeedPageClient
  {
    private WebSiteMode mode;

    public SubscriptionsPageClient()
      : this(WebSiteMode.Desktop)
    {
    }

    public SubscriptionsPageClient(WebSiteMode mode)
      : base("https://www.youtube.com/feed/subscriptions?flow=2", 30)
    {
      this.RequiresSignIn = true;
      this.mode = mode;
      if (mode == WebSiteMode.Mobile)
        this.UserAgent = "Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Mobile Safari/535.19 ";
      this.UseAccessToken = true;
      this.UseFields = false;
      this.UseAPIKey = false;
      this.version = -1;
      this.UseFieldsOnRefreshClient = true;
      this.RefreshClientFields = "kind, nextPageToken, items(kind, id, snippet(publishedAt, channelId, title, description, thumbnails, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
    }

    protected override string GetCacheName()
    {
      string str1 = "s";
      if (YouTube.UserInfo != null && YouTube.UserInfo.ID != null)
      {
        string str2 = str1 + YouTube.UserInfo.ID;
      }
      return base.GetCacheName();
    }

    protected override Task<YouTubeEntry[]> ParseOther(string s) => base.ParseOther(s);

    protected override bool ShouldIncludeURLInfo(YouTubeURLInfo info) => !info.OriginalURL.Contains("&list=") && !info.OriginalURL.Contains("youtu.be") && base.ShouldIncludeURLInfo(info);
  }
}
