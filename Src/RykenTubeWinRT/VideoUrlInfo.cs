// Decompiled with JetBrains decompiler
// Type: RykenTube.VideoUrlInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Net;

namespace RykenTube
{
  public class VideoUrlInfo
  {
    private URLConstructor originalCon;

    public URLConstructor Url { get; set; }

    public int Itag { get; set; } = -1;

    public VideoSize? Size { get; set; }

    public VideoByteRange? InitRange { get; set; }

    public VideoByteRange? IndexRange { get; set; }

    public MediaEncoding? Encoding => ItagMap.GetEncoding(new int?(this.Itag));

    public bool IsAdaptive => ItagMap.IsAdaptive(new int?(this.Itag));

    public string Mime { get; set; }

    public string Codecs { get; set; }

    public uint Bitrate { get; set; }

    public VideoUrlInfo()
    {
    }

    public VideoUrlInfo(URLConstructor con)
    {
      if (con == null)
        return;
      this.originalCon = con;
      if (con.ContainsKey("type"))
      {
        string[] strArray = con["type"].Split(new char[1]
        {
          ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        if (strArray.Length != 0)
          this.Mime = strArray[0].Replace("\"", "").Trim();
        if (strArray.Length > 1)
          this.Codecs = strArray[1].Replace("\"", "").Replace("codecs=", "").Trim();
      }
      int result1;
      if (con.ContainsKey("itag") && int.TryParse(con["itag"], out result1))
        this.Itag = result1;
      if (con.ContainsKey("url"))
        this.Url = new URLConstructor(WebUtility.UrlDecode(con["url"]));
      VideoSize size;
      if (con.ContainsKey("size") && VideoSize.TryParse(con["size"], out size))
        this.Size = new VideoSize?(size);
      VideoByteRange range1;
      if (con.ContainsKey("init") && VideoByteRange.TryParse(con["init"], out range1))
        this.InitRange = new VideoByteRange?(range1);
      VideoByteRange range2;
      if (con.ContainsKey("index") && VideoByteRange.TryParse(con["index"], out range2))
        this.IndexRange = new VideoByteRange?(range2);
      uint result2;
      if (!con.ContainsKey("bitrate") || !uint.TryParse(con["bitrate"], out result2))
        return;
      this.Bitrate = result2;
    }

    public VideoUrlInfo Clone() => new VideoUrlInfo(this.originalCon)
    {
      Url = this.Url?.Clone(),
      Size = this.Size,
      Itag = this.Itag,
      Mime = this.Mime,
      Codecs = this.Codecs,
      Bitrate = this.Bitrate,
      InitRange = this.InitRange,
      IndexRange = this.IndexRange
    };
  }
}
