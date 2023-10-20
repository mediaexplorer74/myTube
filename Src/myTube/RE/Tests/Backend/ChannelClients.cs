// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.ChannelClients
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.Threading.Tasks;
using TestFramework;

namespace myTube.Tests.Backend
{
  public class ChannelClients
  {
    public string[] channelIds = new string[3]
    {
      "UCq6VFHwMzcMXbuKyG7SQYIg",
      "UCzsizX25bx5_iUSM0ztdBFg",
      "UCNCFHPDH1z0YUCp1L-5Ds1w"
    };

    [TestMethod("RSS Feed Test", Parameters = "channelIds")]
    public async Task RSSFeedTest(string channelId)
    {
      YouTubeEntry[] feed = await new RSSUploadsClient(channelId).GetFeed(0);
      Test.Log("Found " + (object) feed.Length + " for channel ID " + channelId);
      foreach (YouTubeEntry youTubeEntry in feed)
      {
        if (youTubeEntry.GetThumb(ThumbnailQuality.Low) == (Uri) null)
          throw new TestFailureException("Thumbnail not found for one of the videos");
      }
    }
  }
}
