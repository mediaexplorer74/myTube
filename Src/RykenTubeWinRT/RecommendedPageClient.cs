// Decompiled with JetBrains decompiler
// Type: RykenTube.RecommendedPageClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class RecommendedPageClient : VideoFeedPageClient
  {
    public RecommendedPageClient(int howMany)
      : base("https://www.youtube.com/feed/recommended", howMany)
    {
      this.RequiresSignIn = true;
      this.CacheRequiresSameAccount = true;
      this.UseCache("recommended", new GroupCacheInfo()
      {
        MaxAge = TimeSpan.FromHours(8.0),
        MaxItems = 50
      });
    }

    protected override string GetCacheName() => "reco";
  }
}
