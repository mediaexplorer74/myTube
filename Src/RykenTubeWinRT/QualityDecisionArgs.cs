// Decompiled with JetBrains decompiler
// Type: RykenTube.QualityDecisionArgs
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube
{
  public class QualityDecisionArgs
  {
    public YouTubeQuality Quality { get; internal set; }

    public int? Itag { get; internal set; }

    public MediaType Type { get; internal set; }

    public MediaEncoding Encoding { get; internal set; }
  }
}
