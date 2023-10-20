// Decompiled with JetBrains decompiler
// Type: RykenTube.SubtitleDeclaration
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Xml.Linq;
using UriTester;

namespace RykenTube
{
  public class SubtitleDeclaration : CaptionsDeclaration
  {
    private string id;

    public string Name => this.xml.GetAttribute("name").Value;

    public bool Default => this.xml.GetBool("lang_default");

    public SubtitleDeclaration(XElement Xml, string ID)
      : base(Xml)
    {
      this.id = ID;
    }

    public override string GetLink(SubtitleFormat format) => new URLConstructor("https://www.youtube.com/api/timedtext")
    {
      ["lang"] = this.LanguageCode,
      ["v"] = this.id,
      ["fmt"] = Subtitle.FormatStrings[format],
      ["name"] = this.Name
    }.ToString();
  }
}
