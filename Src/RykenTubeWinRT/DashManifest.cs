// Decompiled with JetBrains decompiler
// Type: RykenTube.DashManifest
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.Web.Http;

//RnD
using Buffer = Windows.Storage.Streams.Buffer;

namespace RykenTube
{
  public class DashManifest
  {
    private XElement dashMpd;
    private XElement period;
    private XElement[] adaptationSets;
    private XNamespace ns = (XNamespace) "urn:mpeg:DASH:schema:MPD:2011";
    private int itag;
    private uint offset;
    private Buffer buffer;
    private List<OffsetAndTimeStamp> times;
    private TimeSpan timeOffset = TimeSpan.Zero;

    public DashManifest(XElement dashMpd)
    {
      this.dashMpd = dashMpd;
      this.period = dashMpd.Element(this.ns + "Period");
      this.adaptationSets = this.period.Elements(this.ns + "AdaptationSet").ToArray<XElement>();
    }

    public void ChangeSegmentListToSegmentTemplate()
    {
      XElement segmentList = this.getSegmentList(this.period);
      if (segmentList == null)
        return;
      segmentList.Name = this.ns + "SegmentTemplate";
    }

    public void MoveSegmentTemplateToAdaptationSet(string mime)
    {
      XElement segmentTemplate = this.getSegmentTemplate(this.period);
      if (segmentTemplate == null)
        return;
      this.getAdaptationSet(mime)?.Add((object) segmentTemplate);
    }

    private XElement getSegmentList(XElement parent) => parent.Element(this.ns + "SegmentList");

    private XElement getSegmentTemplate(XElement parent) => parent.Element(this.ns + "SegmentTemplate");

    private XAttribute getProfilesAttribute() => this.dashMpd.Attribute((XName) "profiles");

    public string GetProfiles() => this.getProfilesAttribute().Value;

    public void SetProfiles(string profiles) => this.getProfilesAttribute().Value = profiles;

    public async Task SetSegmentRanges(string mime, int id)
    {
      HttpClient httpClient = new HttpClient();
      uint[] range = this.getIndexRange(mime, id);
      ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Range", "bytes=" + (object) range[0] + "-" + (object) range[1]);
      Uri baseUri = this.getBaseUri(mime, id);
      try
      {
        byte[] array = (await httpClient.GetBufferAsync(baseUri)).ToArray();
        Encoding.UTF8.GetString(array, 0, array.Length);
        this.times = new MediaBoxReader(array).ReadSIDX().Offsets();
        XElement xmlForQuality = this.getXMLForQuality(mime, id);
        xmlForQuality.Element(this.ns + "SegmentList")?.Remove();
        XElement xelement = new XElement(this.ns + "SegmentList");
        for (int index = 0; index < this.times.Count; ++index)
        {
          XElement content = new XElement(this.ns + "SegmentURL");
          long num = 0;
          if (index < this.times.Count - 1)
            num = this.times[index + 1].ByteOffset - 1L + (long) (range[1] + 1U);
          content.Add((object) new XAttribute((XName) "mediaRange", (object) ((this.times[index].ByteOffset + (long) range[1]).ToString() + "-" + (object) num)));
          xmlForQuality.Add((object) content);
        }
      }
      catch
      {
        throw;
      }
    }

    public uint GetBitrate(string mime, int id) => uint.Parse(((this.getXMLForQuality(mime, id) ?? throw new NullReferenceException("This quality doesn't exist")).Attribute((XName) "bandwidth") ?? throw new NullReferenceException("Element lacks the bandwidth attribute")).Value);

    public void RemoveAdaptationSet(string mime) => this.getAdaptationSet(mime)?.Remove();

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
      //RnD
      //h264.put_Width(num1);
      //h264.put_Height(num2);
      //h264.PixelAspectRatio.put_Denominator(1U);
      //h264.PixelAspectRatio.put_Numerator(1U);
      //h264.FrameRate.put_Denominator(num5);
      //h264.FrameRate.put_Numerator(num7);
      //h264.put_Subtype("H264");
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

    public async Task<IInputStream> GetInputStream()
    {
      InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
      using (DataWriter writer = new DataWriter((IOutputStream) ras))
      {
        try
        {
          int num = (int) writer.WriteString(this.dashMpd.ToString());
        }
        catch
        {
        }
        int num1 = (int) await (IAsyncOperation<uint>) writer.StoreAsync();
        writer.DetachStream();
      }
      return ras.GetInputStreamAt(0UL);
    }

    public string GetString() => this.dashMpd.ToString();
  }
}
