// Decompiled with JetBrains decompiler
// Type: myTube.DashMediaStreamSource
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Media.Core;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Buffer = Windows.Storage.Streams.Buffer;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpCompletionOption = Windows.Web.Http.HttpCompletionOption;
using HttpMethod = Windows.Web.Http.HttpMethod;
using HttpRequestMessage = Windows.Web.Http.HttpRequestMessage;
using HttpResponseMessage = Windows.Web.Http.HttpResponseMessage;

namespace myTube
{
  public class DashMediaStreamSource
  {
    private const uint BufferCapacity = 8388608;
    private MediaStreamSource mss;
    private XElement dashMpd;
    private XElement period;
    private XElement[] adaptationSets;
    private XNamespace ns = (XNamespace) "urn:mpeg:DASH:schema:MPD:2011";
    private int itag;
    private uint offset;
    private Buffer buffer;
    private List<OffsetAndTimeStamp> times;
    private DataWriter writer;
    private TimeSpan timeOffset = TimeSpan.Zero;
    private HttpClient mainClient;

    public MediaStreamSource MediaStreamSource => this.mss;

    public DashMediaStreamSource(XElement dashMpd, YouTubeInfo info, YouTubeQuality qual)
    {
      this.itag = int.Parse(info.GetLink(qual)[nameof (itag)]);
      this.dashMpd = dashMpd;
      this.period = dashMpd.Element(this.ns + "Period");
      this.adaptationSets = this.period.Elements(this.ns + "AdaptationSet").ToArray<XElement>();
      this.mss = new MediaStreamSource((IMediaStreamDescriptor) new VideoStreamDescriptor(this.getProperties("video/mp4", this.itag)));
      MediaStreamSource mss1 = this.mss;
      // ISSUE: method pointer
      //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MediaStreamSource, MediaStreamSourceSampleRequestedEventArgs>>(
      //    new Func<TypedEventHandler<MediaStreamSource, MediaStreamSourceSampleRequestedEventArgs>, EventRegistrationToken>(mss1.add_SampleRequested), new Action<EventRegistrationToken>(mss1.remove_SampleRequested), new TypedEventHandler<MediaStreamSource, MediaStreamSourceSampleRequestedEventArgs>((object) this, __methodptr(mss_SampleRequested)));
      MediaStreamSource mss2 = this.mss;
      // ISSUE: method pointer
      //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MediaStreamSource, MediaStreamSourceStartingEventArgs>>(
      //    new Func<TypedEventHandler<MediaStreamSource, MediaStreamSourceStartingEventArgs>, EventRegistrationToken>(mss2.add_Starting), new Action<EventRegistrationToken>(mss2.remove_Starting), new TypedEventHandler<MediaStreamSource, MediaStreamSourceStartingEventArgs>((object) this, __methodptr(mss_Starting)));
      MediaStreamSource mss3 = this.mss;
      // ISSUE: method pointer
      //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MediaStreamSource, object>>(
      //    new Func<TypedEventHandler<MediaStreamSource, object>, EventRegistrationToken>(mss3.add_Paused), new Action<EventRegistrationToken>(mss3.remove_Paused), new TypedEventHandler<MediaStreamSource, object>((object) this, __methodptr(mss_Paused)));
    }

    private Uri getBaseUri(string mime, int id) => new Uri(this.getXMLForQuality(mime, id).Element(this.ns + "BaseURL").Value);

    private uint[] getInitializationRange(string mime, int id)
    {
      string[] strArray = this.getXMLForQuality(mime, id).Element(this.ns + "SegmentBase").Element(this.ns + "Initialization").Attribute((XName) "range").Value.Split('-');
      return new uint[2]
      {
        uint.Parse(strArray[0]),
        uint.Parse(strArray[1])
      };
    }

    private uint[] getIndexRange(string mime, int id)
    {
      string[] strArray = this.getXMLForQuality(mime, id).Element(this.ns + "SegmentBase").Attribute((XName) "indexRange").Value.Split('-');
      return new uint[2]
      {
        uint.Parse(strArray[0]),
        uint.Parse(strArray[1])
      };
    }

    private VideoEncodingProperties getProperties(string mime, int id)
    {
      XElement xmlForQuality = this.getXMLForQuality(mime, id);
      uint num1 = uint.Parse(xmlForQuality.Attribute((XName) "width").Value);
      uint num2 = uint.Parse(xmlForQuality.Attribute((XName) "height").Value);
      int num3 = (int) uint.Parse(xmlForQuality.Attribute((XName) "bandwidth").Value);
      double num4 = double.Parse(xmlForQuality.Attribute((XName) "frameRate").Value, (IFormatProvider) CultureInfo.InvariantCulture);
      uint num5 = 1;
      double num6 = num4;
      for (; num4 % 1.0 != 0.0 && num5 < 7500U; ++num5)
        num4 += num6;
      uint num7 = (uint) num4;
      VideoEncodingProperties h264 = VideoEncodingProperties.CreateH264();
      h264.Width = (num1);
      h264.Height = (num2);
      h264.PixelAspectRatio.Denominator = (1U);
      h264.PixelAspectRatio.Numerator = (1U);
      h264.FrameRate.Denominator = (num5);
      h264.FrameRate.Numerator = (num7);
      h264.Subtype = ("H264");
      return h264;
    }

    private XElement getXMLForQuality(string mime, int id)
    {
      foreach (XElement element in this.getAdaptationSet(mime).Elements(this.ns + "Representation"))
      {
        XAttribute xattribute = element.Attribute((XName) nameof (id));
        if (xattribute != null && xattribute.Value == id.ToString())
          return element;
      }
      return (XElement) null;
    }

    private XElement getAdaptationSet(string mime)
    {
      foreach (XElement adaptationSet in this.adaptationSets)
      {
        XAttribute xattribute = adaptationSet.Attribute((XName) "mimeType");
        if (xattribute != null && xattribute.Value == mime)
          return adaptationSet;
      }
      return (XElement) null;
    }

    private void mss_Paused(MediaStreamSource sender, object args)
    {
    }

    private async void mss_Starting(
      MediaStreamSource sender,
      MediaStreamSourceStartingEventArgs args)
    {
      MediaStreamSourceStartingRequestDeferral deferral = args.Request.GetDeferral();
      HttpClient client = new HttpClient();
      uint[] range = this.getIndexRange("video/mp4", this.itag);
            //RnD
      //client.DefaultRequestHeaders.Range = new RangeHeaderValue(new long?((long) range[0]), new long?((long) range[1]));
      Uri baseUri = this.getBaseUri("video/mp4", this.itag);
      try
      {
                //RnD
                byte[] byteArrayAsync = default;//await client.GetByteArrayAsync(baseUri);
        Encoding.UTF8.GetString(byteArrayAsync, 0, byteArrayAsync.Length);
        this.times = new MediaBoxReader(byteArrayAsync).ReadSIDX().Offsets();
      }
      catch
      {
        throw;
      }
      uint[] initializationRange = this.getInitializationRange("video/mp4", this.itag);
      //RnD
            //client.DefaultRequestHeaders.Range = new RangeHeaderValue(new long?((long) initializationRange[0]), new long?((long) initializationRange[1]));
      this.offset = Math.Max(range[1], initializationRange[1]);
      try
      {
        string stringAsync = await client.GetStringAsync(baseUri);
      }
      catch
      {
        throw;
      }
      this.mainClient = new HttpClient();
      this.buffer = new Buffer(8388608U);
      this.writer = new DataWriter();
      this.timeOffset = TimeSpan.Zero;
      deferral.Complete();
    }

    private async void mss_SampleRequested(
      MediaStreamSource sender,
      MediaStreamSourceSampleRequestedEventArgs args)
    {
      if (!(args.Request.StreamDescriptor is VideoStreamDescriptor))
        return;
      VideoStreamDescriptor desc = args.Request.StreamDescriptor as VideoStreamDescriptor;
      MediaStreamSourceSampleRequestDeferral deff = args.Request.GetDeferral();
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, this.getBaseUri("video/mp4", this.itag));
      ((IDictionary<string, string>) httpRequestMessage.Headers).Add("Range", "bytes=" + this.offset.ToString() + "-" + (this.offset + 8388608U).ToString());
      HttpResponseMessage resp = await this.mainClient.SendRequestAsync(httpRequestMessage, (HttpCompletionOption) 1);
      IBuffer buff = await resp.Content.ReadAsBufferAsync();
      string str = await resp.Content.ReadAsStringAsync();
      MediaStreamSample fromBuffer = MediaStreamSample.CreateFromBuffer(buff, this.timeOffset);
      this.timeOffset += TimeSpan.FromSeconds((double) desc.EncodingProperties.FrameRate.Numerator / (double) desc.EncodingProperties.FrameRate.Denominator);
      this.offset += 8388608U;
      args.Request.Sample = fromBuffer;
      deff.Complete();
      desc = (VideoStreamDescriptor) null;
      deff = (MediaStreamSourceSampleRequestDeferral) null;
      resp = (HttpResponseMessage) null;
      buff = (IBuffer) null;
    }
  }
}
