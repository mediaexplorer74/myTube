// Decompiled with JetBrains decompiler
// Type: myTube.Tests.Backend.CastingTest
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using RykenUPnP;
using RykenUPnP.Clients;
using RykenUPnP.Devices;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TestFramework;

namespace myTube.Tests.Backend
{
  public class CastingTest
  {
    public static CastingDevice[] devices;

    [TestMethod("List DLNA Devices")]
    public async Task ListDLNADevices()
    {
      DLNAClient dlnaClient = new DLNAClient();
      Test.Log("Searching for DLNA/UPnP devices...");
      Stopwatch watch = new Stopwatch();
      watch.Start();
      CancellationToken token = new CancellationTokenSource(TimeSpan.FromSeconds(5.0)).Token;
      CastingTest.devices = await dlnaClient.GetAvailableDevices(token);
      watch.Stop();
      if (CastingTest.devices.Length == 0)
      {
        Test.Warn("No devices found, ignoring test.");
      }
      else
      {
        Test.Log("Found " + (object) CastingTest.devices.Length + (CastingTest.devices.Length > 1 ? (object) " devices" : (object) " device") + " in " + (object) watch.Elapsed.TotalSeconds + " seconds: ");
        foreach (CastingDevice device in CastingTest.devices)
          Test.Log(device.Name);
      }
    }

    [TestMethod("Play on DLNA Device", Parameters = "devices", ShouldBeSeparate = true)]
    public async Task PlayOnDevice(CastingDevice device)
    {
      DLNAClient client = new DLNAClient();
      Test.Log("Getting video information");
      Task<YouTubeInfo> task = new VideoInfoLoader().LoadInfoAllMethods("Nc3b_KlikwI");
      CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10.0));
      string uriString = (await task).GetLink(YouTubeQuality.HD).ToString();
      MediaInfo minfo = new MediaInfo()
      {
        Uri = new Uri(uriString),
        Id = "Nc3b_KlikwI",
        MIME = "video/mp4",
        Title = "Test Video"
      };
      minfo.Uri = new Uri("http://dl.dropboxusercontent.com/u/67357310/test.mp4?dl=1");
      TransportInfo transportInfo1 = await client.GetTransportInfo(device);
      int num1 = await client.SetMedia(new MediaInfo(), device) ? 1 : 0;
      TransportInfo transportInfo2 = await client.GetTransportInfo(device);
      int num2 = await client.Stop(device) ? 1 : 0;
      await Task.Delay(500);
      if (!await client.SetMedia(minfo, device))
        throw new TestFailureException("Failed to set media on device");
      Test.Log("Sucessfully set media on " + device.Name);
      if (!await client.Play(device))
        throw new TestFailureException("Failed to send play command to device");
      Test.Log("Sucessfully played on " + device.Name);
      Test.Log("Waiting 15 seconds before pausing");
      await Task.Delay(15000);
      if (!await client.Pause(device))
        throw new TestFailureException("Failed to send pause command to device");
      Test.Log("Sucessfully paused on " + device.Name);
      await Task.Delay(2000);
      if (!await client.Stop(device))
        throw new TestFailureException("Failed to send stop command to device");
      Test.Log("Sucessfully stopped playback on " + device.Name);
    }
  }
}
