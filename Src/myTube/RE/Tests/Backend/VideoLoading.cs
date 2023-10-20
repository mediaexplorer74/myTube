// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.VideoLoading
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TestFramework;
using Windows.System.Threading;

namespace myTube.Tests.Backend
{
  public class VideoLoading
  {
    private string[] vevoVideoIDs = new string[3]
    {
      "CevxZvSJLk8",
      "fBf2v4mLM8k",
      "lSooYPG-5Rg"
    };
    private string[] videos60FPS = new string[2]
    {
      "lfwjzNB--5k",
      "_zPm3SSj6W8"
    };
    private string[] normalVideos = new string[3]
    {
      "G2W41pvvZs0",
      "_gPHTIJfRK8",
      "pLPPsxjzRnc"
    };
    private const double seconds = 0.05;

    [TestMethod("VEVO videos", Parameters = "vevoVideoIDs")]
    public async Task VevoVideos(string id)
    {
      string str = await App.UpdateCipher(0.0);
      if ((await this.loadVideo(id)).Decipher)
        return;
      Test.Warn("The signature for this video was not deciphered.");
    }

    [TestMethod("60 FPS videos", Parameters = "videos60FPS")]
    public async Task Videos60FPS(string id)
    {
      YouTubeInfo youTubeInfo = await this.loadVideo(id);
      youTubeInfo.Allow60FPS = true;
      if (!youTubeInfo.HasQuality(YouTubeQuality.HD1080p60) && !youTubeInfo.HasQuality(YouTubeQuality.HD60))
        throw new TestFailureException("Video does not contain 60 FPS quality");
    }

    [TestMethod("Normal Videos", Parameters = "normalVideos")]
    public async Task NormalVideos(string id)
    {
      if (!(await this.loadVideo(id)).Enciphered)
        return;
      Test.Warn("Video was enciphered, when it should not have been");
    }

    [TestMethod("Metadata Perfomance", Parameters = "normalVideos")]
    public async Task NormalVideosPerformance(string id)
    {
      YouTubeInfo youTubeInfo = await this.testPerformance(id, TimeSpan.FromSeconds(0.05), true, false, false);
    }

    [TestMethod("Metadata Perfomance (Private API)", Parameters = "normalVideos")]
    public async Task NormalVideosPerformancePrivate(string id)
    {
      YouTubeInfo youTubeInfo = await this.testPerformance(id, TimeSpan.FromSeconds(0.05), true, false, false);
    }

    [TestMethod("Metadata Perfomance (Navigate Page)", Parameters = "normalVideos")]
    public async Task NormalVideosPerformanceNavigatePage(string id)
    {
      YouTubeInfo youTubeInfo = await this.testPerformance(id, TimeSpan.FromSeconds(0.05), true, false, true);
    }

    [TestMethod("Metadata Perfomance (Vevo)", Parameters = "vevoVideoIDs")]
    public async Task NormalVideosPerformanceVevo(string id)
    {
      YouTubeInfo youTubeInfo = await this.testPerformance(id, TimeSpan.FromSeconds(0.05), true, true, false);
    }

    [TestMethod("Metadata Perfomance (Vevo - Navigate Page)", Parameters = "vevoVideoIDs")]
    public async Task NormalVideosPerformanceVevoNavigatePage(string id)
    {
      YouTubeInfo youTubeInfo = await this.testPerformance(id, TimeSpan.FromSeconds(0.05), true, true, true);
    }

    private async Task<YouTubeInfo> testPerformance(
      string id,
      TimeSpan shortestTime,
      bool watchPage,
      bool decipher,
      bool navigatePage)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      VideoLoading.\u003C\u003Ec__DisplayClass12_1 cDisplayClass121 = new VideoLoading.\u003C\u003Ec__DisplayClass12_1();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass121.watchPage = watchPage;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass121.decipher = decipher;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass121.navigatePage = navigatePage;
      Test.Log("Testing performance of parsing metadata for video ID " + id);
      Test.Log("It should not take longer than " + (object) shortestTime.TotalSeconds + " seconds to parse the metadata.");
      VideoInfoLoader videoInfoLoader = new VideoInfoLoader();
      // ISSUE: reference to a compiler-generated field
      videoInfoLoader.UseNavigatePage = cDisplayClass121.navigatePage;
      YouTubeInfo info2;
      try
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        VideoLoading.\u003C\u003Ec__DisplayClass12_0 cDisplayClass120 = new VideoLoading.\u003C\u003Ec__DisplayClass12_0();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.CS\u0024\u003C\u003E8__locals1 = cDisplayClass121;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        YouTubeInfo youTubeInfo = await videoInfoLoader.LoadInfo(id, cDisplayClass120.CS\u0024\u003C\u003E8__locals1.watchPage, cDisplayClass120.CS\u0024\u003C\u003E8__locals1.decipher);
        if (youTubeInfo == null)
          throw new TestFailureException("Failed to load info for " + id);
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.watch = new Stopwatch();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.str = youTubeInfo.OriginalString;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.CS\u0024\u003C\u003E8__locals1.watchPage = youTubeInfo.WatchPage;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.CS\u0024\u003C\u003E8__locals1.decipher = youTubeInfo.Decipher;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.CS\u0024\u003C\u003E8__locals1.navigatePage = youTubeInfo.UseNavigatePage;
        // ISSUE: reference to a compiler-generated field
        Test.Log("Metadata string contains " + (object) cDisplayClass120.str.Length + " characters");
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.info2 = (YouTubeInfo) null;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.time = TimeSpan.Zero;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.iterations = 50;
        GC.Collect();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.tcs = new TaskCompletionSource<bool>();
        // ISSUE: method pointer
        await ThreadPool.RunAsync(new WorkItemHandler((object) cDisplayClass120, __methodptr(\u003CtestPerformance\u003Eb__0)));
        // ISSUE: reference to a compiler-generated field
        int num = await cDisplayClass120.tcs.Task ? 1 : 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.time = TimeSpan.FromSeconds(cDisplayClass120.time.TotalSeconds / (double) cDisplayClass120.iterations);
        // ISSUE: reference to a compiler-generated field
        Test.Log("It took " + (object) cDisplayClass120.time.TotalSeconds + " seconds (average) to parse the metadata for this video");
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass120.time > shortestTime)
          throw new TestFailureException("Took too long to parse the metadata for this video");
        // ISSUE: reference to a compiler-generated field
        info2 = cDisplayClass120.info2;
      }
      catch (TestFailureException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new TestFailureException("Failed to load info for " + id);
      }
      return info2;
    }

    private async Task<YouTubeInfo> loadVideo(string id)
    {
      Test.Log("Getting info for video ID " + id);
      VideoInfoLoader videoInfoLoader = new VideoInfoLoader();
      YouTubeInfo youTubeInfo1;
      try
      {
        YouTubeInfo youTubeInfo2 = await videoInfoLoader.LoadInfoAllMethods(id);
        if (youTubeInfo2 == null)
          throw new TestFailureException("Failed to load info for " + id);
        if (!youTubeInfo2.WatchPage)
          Test.Warn("This video was downloaded from YouTube's private API, rather than the watch page");
        youTubeInfo1 = youTubeInfo2;
      }
      catch
      {
        throw new TestFailureException("Failed to load info for " + id);
      }
      return youTubeInfo1;
    }
  }
}
