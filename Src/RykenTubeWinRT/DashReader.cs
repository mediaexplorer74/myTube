// Decompiled with JetBrains decompiler
// Type: RykenTube.DashReader
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Linq;
using System.Xml.Linq;

namespace RykenTube
{
  public class DashReader
  {
    private XElement xml;
    private static XNamespace ns = (XNamespace) "urn:mpeg:DASH:schema:MPD:2011";
    private XElement period;
    private XElement[] adaptationSets;

    public XElement XML => this.xml;

    public DashReader(XElement xml)
    {
      this.xml = xml;
      this.period = xml.Element(DashReader.ns + "Period");
      this.adaptationSets = this.period.Elements(DashReader.ns + "AdaptationSet").ToArray<XElement>();
    }

    public DashReader(string xml)
      : this(XElement.Parse(xml))
    {
    }

    private XElement getAdaptationSetFromMime(string mime)
    {
      foreach (XElement adaptationSet in this.adaptationSets)
      {
        XAttribute xattribute = adaptationSet.Attribute((XName) "mimeType");
        if (xattribute != null && xattribute.Value.Trim() == mime.Trim())
          return adaptationSet;
      }
      return (XElement) null;
    }

    public void SetCodecForAdaptationSet(string codec, string mimeType)
    {
      XElement adaptationSetFromMime = this.getAdaptationSetFromMime(mimeType);
      if (adaptationSetFromMime == null)
        return;
      XAttribute xattribute = adaptationSetFromMime.Attribute((XName) "codecs");
      if (xattribute == null)
      {
        XAttribute content = new XAttribute((XName) "codecs", (object) codec);
        adaptationSetFromMime.Add((object) content);
      }
      else
        xattribute.Value = codec;
    }
  }
}
