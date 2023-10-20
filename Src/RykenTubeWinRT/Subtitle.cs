// Decompiled with JetBrains decompiler
// Type: RykenTube.Subtitle
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class Subtitle
  {
    public static Dictionary<SubtitleFormat, string> FormatStrings = new Dictionary<SubtitleFormat, string>()
    {
      {
        SubtitleFormat.TTS,
        "tts"
      },
      {
        SubtitleFormat.SBV,
        "sbv"
      },
      {
        SubtitleFormat.VTT,
        "vtt"
      }
    };
    private string text;

    [DefaultValue(SubtitlePlacement.Bottom)]
    public SubtitlePlacement Placement { get; set; }

    public string Text
    {
      get => this.text;
      set => this.text = value.Replace("&nbsp;", "\n").Replace("&gt;", ">").Replace("&lt;", "<");
    }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public Subtitle(string text, TimeSpan startTime, TimeSpan endTime)
    {
      this.Text = text;
      this.StartTime = startTime;
      this.EndTime = endTime;
    }

    public static async Task<SubtitleDeclaration[]> GetLanguageList(string videoID)
    {
      XElement[] array = XElement.Parse(await new HttpClient().GetStringAsync(new Uri("https://video.google.com/timedtext?hl=en&type=list&v=" + videoID, UriKind.Absolute))).Elements((XName) "track").ToArray<XElement>();
      SubtitleDeclaration[] languageList = new SubtitleDeclaration[array.Length];
      for (int index = 0; index < array.Length; ++index)
        languageList[index] = new SubtitleDeclaration(array[index], videoID);
      return languageList;
    }
  }
}
