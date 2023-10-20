// Decompiled with JetBrains decompiler
// Type: RykenTube.CaptionsDeclaration
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using UriTester;

namespace RykenTube
{
  public class CaptionsDeclaration
  {
    protected XElement xml;
    private URLConstructor baseAddress;
    private string origLang;

    public string LanguageTranslated => this.xml.GetAttribute("lang_translated").Value;

    public string LanguageOriginal => this.xml.GetAttribute("lang_original").Value;

    public string LanguageCode => this.xml.GetAttribute("lang_code").Value;

    public CaptionsDeclaration(XElement Xml) => this.xml = Xml;

    public CaptionsDeclaration(XElement Xml, URLConstructor BaseAddress, string originalLang)
      : this(Xml)
    {
      this.baseAddress = new URLConstructor(this.baseAddress.ToString());
      this.baseAddress.SetValue("lang", (object) this.LanguageOriginal);
      this.baseAddress.SetValue("tlang", (object) this.LanguageTranslated);
    }

    public virtual string GetLink(SubtitleFormat format) => throw new NotImplementedException();

    public async Task<Subtitle[]> GetCaptions(SubtitleFormat format)
    {
      string stringAsync = await new HttpClient().GetStringAsync(this.GetLink(format));
      Subtitle[] captions = default;
      switch (format)
      {
        case SubtitleFormat.TTS:
          captions = this.parseTTS(stringAsync);
          break;
        case SubtitleFormat.SBV:
          captions = this.parseSBV(stringAsync);
          break;
        case SubtitleFormat.VTT:
          captions = this.parseVTT(stringAsync);
          break;
      }
      TimeSpan timeSpan = TimeSpan.MinValue;
      for (int index = 0; index < captions.Length; ++index)
      {
        captions[index].Placement = !(captions[index].StartTime < timeSpan) ? SubtitlePlacement.Bottom : SubtitlePlacement.Top;
        timeSpan = captions[index].EndTime;
      }
      return captions;
    }

    private Subtitle[] parseTTS(string data) => throw new NotImplementedException();

    private Subtitle[] parseSBV(string data)
    {
      string[] strArray1 = data.Split('\n');
      int num = 0;
      TimeSpan result1 = TimeSpan.Zero;
      TimeSpan result2 = TimeSpan.Zero;
      List<Subtitle> subtitleList = new List<Subtitle>();
      string text = (string) null;
      foreach (string str in strArray1)
      {
        if (num == 0)
        {
          string[] strArray2 = str.Split(',');
          if (strArray2.Length > 1)
          {
            TimeSpan.TryParse(strArray2[0], out result1);
            TimeSpan.TryParse(strArray2[1], out result2);
          }
        }
        else if (!string.IsNullOrWhiteSpace(str))
        {
          text = !string.IsNullOrWhiteSpace(text) ? text + "\n" + str : str;
        }
        else
        {
          Subtitle subtitle = new Subtitle(text, result1, result2);
          subtitleList.Add(subtitle);
          text = (string) null;
          result1 = result2 = TimeSpan.Zero;
          num = 0;
          continue;
        }
        ++num;
      }
      return subtitleList.ToArray();
    }

    private Subtitle[] parseVTT(string data)
    {
      List<string> list = ((IEnumerable<string>) data.Split('\n')).ToList<string>();
      int num = 0;
      TimeSpan result1 = TimeSpan.Zero;
      TimeSpan result2 = TimeSpan.Zero;
      List<Subtitle> subtitleList = new List<Subtitle>();
      string text = (string) null;
      list.RemoveRange(0, 4);
      foreach (string str in list)
      {
        if (num == 0)
        {
          string[] strArray = str.Split(new string[1]
          {
            "-->"
          }, StringSplitOptions.RemoveEmptyEntries);
          if (strArray.Length > 1)
          {
            TimeSpan.TryParse(strArray[0].Trim(), out result1);
            TimeSpan.TryParse(strArray[1].Trim(), out result2);
          }
        }
        else if (!string.IsNullOrWhiteSpace(str))
        {
          text = !string.IsNullOrWhiteSpace(text) ? text + "\n" + str : str;
        }
        else
        {
          Subtitle subtitle = new Subtitle(text, result1, result2);
          subtitleList.Add(subtitle);
          text = (string) null;
          result1 = result2 = TimeSpan.Zero;
          num = 0;
          continue;
        }
        ++num;
      }
      return subtitleList.ToArray();
    }
  }
}
