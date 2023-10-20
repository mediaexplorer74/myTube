// Decompiled with JetBrains decompiler
// Type: myTube.Media.myTubeMediaStreamSourceContainer
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.Media.Core;

namespace myTube.Media
{
  public class myTubeMediaStreamSourceContainer
  {
    private YouTubeInfo info;
    private MediaStreamSource mss;
    private long videoBufferOffset;
    private const long VideoSampleSize = 32768;
    private HttpClient client;
    private URLConstructor link;

    public myTubeMediaStreamSourceContainer(YouTubeInfo Info)
    {
      this.info = Info;
      this.client = new HttpClient();
    }

    public MediaStreamSource GetMediaStreamSource(YouTubeQuality qual)
    {
      if (this.mss == null)
        this.link = this.info.GetLink(qual);
      return this.mss;
    }

    private async void mss_SampleRequested(
      MediaStreamSource sender,
      MediaStreamSourceSampleRequestedEventArgs args)
    {
      MediaStreamSourceSampleRequestDeferral defferal = args.Request.GetDeferral();
      this.client.DefaultRequestHeaders.ExpectContinue = new bool?(false);
      this.client.DefaultRequestHeaders.Range.Ranges.Clear();
      this.client.DefaultRequestHeaders.Range.Ranges.Add(new RangeItemHeaderValue(new long?(this.videoBufferOffset), new long?(this.videoBufferOffset + 32768L)));
      this.videoBufferOffset += 32768L;
      WindowsRuntimeStreamExtensions.AsInputStream(await this.client.GetStreamAsync(this.link.ToUri(UriKind.Absolute)));
      defferal.Complete();
    }
  }
}
