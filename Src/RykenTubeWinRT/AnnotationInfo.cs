// Decompiled with JetBrains decompiler
// Type: RykenTube.AnnotationInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using UriTester;
using Windows.Foundation;

namespace RykenTube
{
  public class AnnotationInfo
  {
    private AnnotationStyle style;
    private URLConstructor url;
    private AnnotationType type;
    private string text = "";
    private int bgColor;
    private int fgColor = 16777215;
    private double bgOpacity = 1.0;
    private double borderOpacity;
    private TimeSpan startTime = TimeSpan.Zero;
    private TimeSpan endTime = TimeSpan.Zero;
    private Rect rect = new Rect(0.0, 0.0, 0.01, 0.01);
    private Uri channelImageUri;
    private string channelId;
    private double channelImageOpacity = 1.0;
    private string channelName;

    public AnnotationStyle Style => this.style;

    public URLConstructor URL => this.url;

    public AnnotationType Type => this.type;

    public double BorderOpacity => this.borderOpacity;

    public int BGColor => this.bgColor;

    public int FGColor => this.fgColor;

    public double BGOpacity => this.bgOpacity;

    public string Text => this.text;

    public TimeSpan StartTime => this.startTime;

    public TimeSpan EndTime => this.endTime;

    public Rect Rect => this.rect;

    public Uri ChannelImageUri => this.channelImageUri;

    public string ChannelId => this.channelId;

    public double ChannelImageOpacity => this.channelImageOpacity;

    public string ChannelName => this.channelName;

    public bool HasAction => this.url != null || this.channelId != null;

    public AnnotationInfo()
    {
    }

    public AnnotationInfo(XElement xml)
    {
      string str1 = xml.GetAttribute(nameof (style)).Value;
      switch (xml.GetAttribute(nameof (type)).Value)
      {
        case "highlight":
          this.type = AnnotationType.Highlight;
          break;
        case nameof (text):
          this.type = AnnotationType.Text;
          break;
        case "branding":
          this.type = AnnotationType.Branding;
          this.endTime = TimeSpan.MaxValue;
          break;
      }
      switch (str1)
      {
        case "popup":
          this.style = AnnotationStyle.Popup;
          break;
        case "highlightText":
          this.style = AnnotationStyle.HighlightText;
          break;
      }
      this.text = xml.GetElement("TEXT").Value;
      XElement xelement1 = xml.Element((XName) "action");
      XElement xelement2 = xml.Element((XName) "data");
      if (xelement2 != null)
      {
        try
        {
          JObject jobject = JObject.Parse(xelement2.Value);
          string uriString = (string) jobject["image_url"];
          if (uriString != null)
          {
            try
            {
              this.channelImageUri = new Uri(uriString);
            }
            catch
            {
            }
          }
          string str2 = (string) jobject["channel_name"];
          if (str2 != null)
          {
            this.bgOpacity = 0.0;
            this.channelName = str2;
            double num = 0.07;
            this.rect = new Rect(1.0 - num, 0.0, num, num);
            this.channelImageOpacity = 0.5;
          }
          string str3 = (string) jobject["channel_id"];
          if (str3 != null)
            this.channelId = str3;
        }
        catch
        {
        }
      }
      if (xelement1 != null)
      {
        XElement x = xelement1.Element((XName) nameof (url));
        if (x != null)
          this.url = new URLConstructor(x.GetAttribute("value").Value);
      }
      XElement xelement3 = xml.Element((XName) "segment");
      if (xelement3 != null)
      {
        IEnumerable<XElement> source = xelement3.Elements((XName) "movingRegion");
        XElement[] array = source.Elements<XElement>((XName) "rectRegion").ToArray<XElement>();
        if (array.Length == 0)
          array = source.Elements<XElement>((XName) "anchoredRegion").ToArray<XElement>();
        if (array.Length > 1)
        {
          string time1 = array[0].GetAttribute("t").Value;
          TimeSpan time2 = this.parseTime(array[1].GetAttribute("t").Value);
          TimeSpan time3 = this.parseTime(time1);
          if (time2 < time3)
          {
            this.startTime = time2;
            this.endTime = time3;
          }
          else
          {
            this.startTime = time3;
            this.endTime = time2;
          }
          double result1 = 0.0;
          double result2 = 0.0;
          double result3 = 0.0;
          double result4 = 0.0;
          double.TryParse(array[0].GetAttribute("h").Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result3);
          double.TryParse(array[0].GetAttribute("w").Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result4);
          double.TryParse(array[0].GetAttribute("y").Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2);
          double.TryParse(array[0].GetAttribute("x").Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
          this.rect = new Rect(result1 / 100.0, result2 / 100.0, result4 / 100.0, result3 / 100.0);
        }
      }
      XElement x1 = xml.Element((XName) "appearance");
      if (x1 == null)
        return;
      int.TryParse(x1.GetAttribute(nameof (bgColor)).Value, out this.bgColor);
      int.TryParse(x1.GetAttribute(nameof (fgColor)).Value, out this.fgColor);
      double.TryParse(x1.GetAttribute("bgAlpha").Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out this.bgOpacity);
      double.TryParse(x1.GetAttribute("borderAlpha").Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out this.borderOpacity);
    }

    private TimeSpan parseTime(string time)
    {
      string[] strArray = time.Split(':');
      double result1 = 0.0;
      double result2 = 0.0;
      double result3 = 0.0;
      if (strArray.Length >= 1)
        double.TryParse(strArray[strArray.Length - 1], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
      if (strArray.Length >= 2)
        double.TryParse(strArray[strArray.Length - 2], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result2);
      if (strArray.Length >= 3)
        double.TryParse(strArray[strArray.Length - 3], NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result3);
      return new TimeSpan((int) result3, (int) result2, (int) result1);
    }

    public static List<AnnotationInfo> GetAnnotations(XElement xml)
    {
      List<AnnotationInfo> annotations = new List<AnnotationInfo>();
      foreach (XElement element in xml.GetElement("annotations").Elements((XName) "annotation"))
        annotations.Add(new AnnotationInfo(element));
      return annotations;
    }
  }
}
