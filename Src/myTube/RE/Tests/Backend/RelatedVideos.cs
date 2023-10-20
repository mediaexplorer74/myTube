// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.RelatedVideos
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System.Threading.Tasks;
using TestFramework;

namespace myTube.Tests.Backend
{
  public class RelatedVideos
  {
    [TestMethod("Related Videos (Authorized)")]
    public async Task GetRelatedAuthorized()
    {
      Test.Log("Getting related videos (authorized) from 10 most popular YouTube videos");
      if (!YouTube.IsSignedIn)
        Test.Warn("You are not signed in");
      YouTubeEntry[] youTubeEntryArray = await new FeedClient(YouTubeFeed.Popular, Category.All, YouTubeTime.Today, 10).GetFeed(0);
      for (int index = 0; index < youTubeEntryArray.Length; ++index)
      {
        YouTubeEntry v = youTubeEntryArray[index];
        RelatedClient relatedClient = new RelatedClient(v.ID, 10);
        relatedClient.UseAccessToken = true;
        Test.Log("Getting related videos for (authorized) for " + v.ID);
        if ((await relatedClient.GetFeed(0)).Length == 0)
          throw new TestFailureException("There are no related videos for " + v.ID);
        v = (YouTubeEntry) null;
      }
      youTubeEntryArray = (YouTubeEntry[]) null;
    }

    [TestMethod("Related Videos (Unauthorized)")]
    public async Task GetRelatedUnauthorized()
    {
      Test.Log("Getting related videos (unauthorized) from 10 most popular YouTube videos");
      YouTubeEntry[] youTubeEntryArray = await new FeedClient(YouTubeFeed.Popular, Category.All, YouTubeTime.Today, 10).GetFeed(0);
      for (int index = 0; index < youTubeEntryArray.Length; ++index)
      {
        YouTubeEntry v = youTubeEntryArray[index];
        RelatedClient relatedClient = new RelatedClient(v.ID, 10);
        relatedClient.UseAccessToken = false;
        Test.Log("Getting related videos for (unauthorized) for " + v.ID);
        if ((await relatedClient.GetFeed(0)).Length == 0)
          throw new TestFailureException("There are no related videos for " + v.ID);
        v = (YouTubeEntry) null;
      }
      youTubeEntryArray = (YouTubeEntry[]) null;
    }
  }
}
