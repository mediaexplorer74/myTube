// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.PopularTests
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestFramework;

namespace myTube.Tests.Backend
{
  public class PopularTests
  {
    private Array categories = Enum.GetValues(typeof (Category));

    [TestMethod(DisplayName = "Popular videos", Parameters = "categories")]
    public async Task PopularCategories(Enum value)
    {
      Category cat = (Category) value;
      Test.Log("Checking popular category " + (object) cat);
      FeedClient client = new FeedClient(YouTubeFeed.Popular, cat, YouTubeTime.Today, 10);
      List<YouTubeEntry> vids = new List<YouTubeEntry>();
      for (int i = 0; i < 3; ++i)
      {
        YouTubeEntry[] feed = await client.GetFeed(i);
        Test.Log("Found " + (object) feed.Length + " popular videos in the category " + (object) cat + " on page " + (object) i);
        if (feed.Length == 0)
          throw new TestFailureException("No videos were returned on page " + (object) i);
        foreach (YouTubeEntry youTubeEntry in feed)
        {
          int num = 0;
          using (List<YouTubeEntry>.Enumerator enumerator = vids.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              if (enumerator.Current.ID == youTubeEntry.ID)
              {
                Test.Warn("Duplicate video found");
                ++num;
              }
            }
          }
          if (num > 0)
            Test.Warn(num.ToString() + " duplicate videos found!");
          vids.Add(youTubeEntry);
        }
      }
    }
  }
}
