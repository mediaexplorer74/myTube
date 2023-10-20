// Decompiled with JetBrains decompiler
// Type: RykenTube.LikesClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class LikesClient : VideoListClient
  {
    private Rating rating;

    public LikesClient(Rating rating, int howMany)
      : base("https://www.googleapis.com/youtube/v3/videos", howMany)
    {
      this.DefaultCacheGroupName = "liked";
      this.CacheRequiresSameAccount = true;
      this.rating = rating != Rating.None ? rating : throw new InvalidOperationException("Can't use Rating.None");
      this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/videos";
      this.UseAccessToken = true;
      this.UseAPIKey = true;
      this.NeedsRefresh = false;
      this.RefreshInternally = false;
      this.UseFields = true;
      this.FieldsV3 = "kind, nextPageToken, items(kind, id, snippet(publishedAt, channelId, title, description, thumbnails, channelTitle, liveBroadcastContent, categoryId), statistics,contentDetails(duration, licensedContent, projection))";
      this.RequiresSignIn = true;
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      if (this.rating == Rating.Like)
      {
        this.BaseAddress["myRating"] = "like";
      }
      else
      {
        if (this.rating != Rating.Dislike)
          return;
        this.BaseAddress["myRating"] = "dislike";
      }
    }

    protected override string GetCacheName() => string.Format("{0}", (object) (int) this.rating);
  }
}
