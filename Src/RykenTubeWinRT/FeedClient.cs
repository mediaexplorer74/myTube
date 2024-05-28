// RykenTube.FeedClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class FeedClient : VideoListClient
  {
    private YouTubeFeed type;
    private string b = "";
    private int number;
    private YouTubeTime time;
    private Category cat;

    public Category Category
    {
      get => this.cat;
      set => this.cat = value;
    }

    public FeedClient(YouTubeFeed Type, Category cat, YouTubeTime time, int numPerPage)
      : base("https://gdata.youtube.com/feeds/api/standardfeeds/", numPerPage)
    {
      this.UseCache("popularFeed", new GroupCacheInfo()
      {
        MaxItems = 100,
        MaxAge = TimeSpan.FromHours(3.0)
      });
      this.version = 3;
      this.number = numPerPage;
      this.type = Type;
      this.time = time;
      this.cat = cat;
      this.UseAccessToken = false;
      this.RefreshInternally = false;
      this.UseRandomQuery = false;
      this.UseFields = true;
      this.FieldsV3 = "kind, nextPageToken, items(kind, id, snippet(publishedAt, channelId, title, description, thumbnails, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
    }

    protected override string GetCacheName()
    {
      string cacheName = "t" + (object) (int) this.time + "c" + (object) (int) this.cat;
       
       //RnD
       //if (YouTube.Region != (RegionInfo) null)
        cacheName = cacheName + "r" + YouTube.Region.CountryCode;

      return cacheName;
    }

        private string getRegion()
        {
            return YouTube.Region != RegionInfo.Global ? YouTube.Region.CountryCode + "/" : "";
        }

        protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);

            /*
      if (this.version == 2)
      {
        if (this.type == YouTubeFeed.Trending)
          this.b = "https://gdata.youtube.com/feeds/api/standardfeeds/" + this.getRegion() + "on_the_web";
        else if (this.type == YouTubeFeed.Featured)
          this.b = "https://gdata.youtube.com/feeds/api/standardfeeds/" + this.getRegion() + "recently_featured";
        else if (this.type == YouTubeFeed.TopRated)
          this.b = "https://gdata.youtube.com/feeds/api/standardfeeds/" + this.getRegion() + "top_rated";
        else if (this.type == YouTubeFeed.Popular)
          this.b = "https://gdata.youtube.com/feeds/api/standardfeeds/" + this.getRegion() + "most_popular";
        else if (this.type == YouTubeFeed.Recent)
          this.b = "https://gdata.youtube.com/feeds/api/standardfeeds/" + this.getRegion() + "most_recent";
        switch (this.cat)
        {
          case Category.FilmAnimation:
            this.b += "_Film";
            break;
          case Category.AutoVehicles:
            this.b += "_Autos";
            break;
          case Category.Music:
            this.b += "_Music";
            break;
          case Category.Pets:
            this.b += "_Animals";
            break;
          case Category.Sports:
            this.b += "_Sports";
            break;
          case Category.TravelEvents:
            this.b += "_Travel";
            break;
          case Category.Gaming:
            this.b += "_Games";
            break;
          case Category.People:
            this.b += "_People";
            break;
          case Category.Comedy:
            this.b += "_Comedy";
            break;
          case Category.Entertainment:
            this.b += "_Entertainment";
            break;
          case Category.News:
            this.b += "_News";
            break;
          case Category.HowTo:
            this.b += "_Howto";
            break;
          case Category.Education:
            this.b += "_Education";
            break;
          case Category.ScienceTech:
            this.b += "_Tech";
            break;
          case Category.NonProfit:
            this.b += "_Nonprofit";
            break;
        }
        this.BaseAddress.BaseAddress = this.b;
        switch (this.time)
        {
          case YouTubeTime.AllTime:
            this.b = "all_time";
            break;
          case YouTubeTime.Today:
            this.b = "today";
            break;
        }
        this.BaseAddress.SetValue("time", (object) this.b);
      }
      */

      if (1==1)//else
      {
        if (this.version != 3)
          return;

        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/videos";
        this.BaseAddress["chart"] = "mostPopular";

        if (YouTube.Region != RegionInfo.Global)
          this.BaseAddress["regionCode"] = YouTube.Region.CountryCode;
        else
          this.BaseAddress.RemoveValue("regionCode");
        if (this.cat == Category.All)
          this.BaseAddress.RemoveValue("videoCategoryId");
        else
          this.BaseAddress.SetValue("videoCategoryId", (object) (int) this.cat);
      }

    }
  }
}
