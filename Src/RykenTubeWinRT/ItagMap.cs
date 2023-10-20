// Decompiled with JetBrains decompiler
// Type: RykenTube.ItagMap
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;
using System.Linq;

namespace RykenTube
{
  public static class ItagMap
  {
    private const MediaEncoding DefaultVideoFormat = MediaEncoding.MP4;
    internal static int[] AudioQualities = new int[6]
    {
      140,
      141,
      171,
      249,
      250,
      251
    };
    internal static int[] QualitiesWhichNeedAudio = new int[39]
    {
      133,
      134,
      135,
      136,
      137,
      138,
      264,
      266,
      140,
      141,
      160,
      264,
      271,
      298,
      299,
      249,
      250,
      251,
      278,
      242,
      243,
      244,
      247,
      248,
      271,
      313,
      272,
      302,
      303,
      308,
      315,
      330,
      331,
      332,
      333,
      334,
      335,
      336,
      337
    };
    internal static Dictionary<YouTubeQuality, int> VerticalResolutions = new Dictionary<YouTubeQuality, int>()
    {
      {
        YouTubeQuality.LQ,
        240
      },
      {
        YouTubeQuality.HQ,
        360
      },
      {
        YouTubeQuality.SD,
        480
      },
      {
        YouTubeQuality.HD,
        720
      },
      {
        YouTubeQuality.HD60,
        720
      },
      {
        YouTubeQuality.HD60HDR,
        720
      },
      {
        YouTubeQuality.HD1080,
        1080
      },
      {
        YouTubeQuality.HD1080p60,
        1080
      },
      {
        YouTubeQuality.HD1080p60HDR,
        1080
      },
      {
        YouTubeQuality.HD1440,
        1440
      },
      {
        YouTubeQuality.HD1440p60,
        1440
      },
      {
        YouTubeQuality.HD1440p60HDR,
        1440
      },
      {
        YouTubeQuality.HD2160,
        2160
      },
      {
        YouTubeQuality.HD2160p60,
        2160
      },
      {
        YouTubeQuality.HD2160p60HDR,
        2160
      },
      {
        YouTubeQuality.HD4320,
        4320
      }
    };
    internal static Dictionary<int, YouTubeQuality> VideoItagMap = new Dictionary<int, YouTubeQuality>()
    {
      {
        140,
        YouTubeQuality.Audio
      },
      {
        36,
        YouTubeQuality.LQ
      },
      {
        133,
        YouTubeQuality.LQ
      },
      {
        18,
        YouTubeQuality.HQ
      },
      {
        134,
        YouTubeQuality.HQ
      },
      {
        135,
        YouTubeQuality.SD
      },
      {
        22,
        YouTubeQuality.HD
      },
      {
        136,
        YouTubeQuality.HD
      },
      {
        137,
        YouTubeQuality.HD1080
      },
      {
        298,
        YouTubeQuality.HD60
      },
      {
        299,
        YouTubeQuality.HD1080p60
      },
      {
        264,
        YouTubeQuality.HD1440
      },
      {
        266,
        YouTubeQuality.HD2160
      },
      {
        138,
        YouTubeQuality.HD4320
      },
      {
        251,
        YouTubeQuality.Audio
      },
      {
        242,
        YouTubeQuality.LQ
      },
      {
        243,
        YouTubeQuality.HQ
      },
      {
        244,
        YouTubeQuality.SD
      },
      {
        247,
        YouTubeQuality.HD
      },
      {
        248,
        YouTubeQuality.HD1080
      },
      {
        271,
        YouTubeQuality.HD1440
      },
      {
        313,
        YouTubeQuality.HD2160
      },
      {
        272,
        YouTubeQuality.HD4320
      },
      {
        302,
        YouTubeQuality.HD60
      },
      {
        303,
        YouTubeQuality.HD1080p60
      },
      {
        308,
        YouTubeQuality.HD1440p60
      },
      {
        315,
        YouTubeQuality.HD2160p60
      },
      {
        334,
        YouTubeQuality.HD60HDR
      },
      {
        335,
        YouTubeQuality.HD1080p60HDR
      },
      {
        336,
        YouTubeQuality.HD1440p60HDR
      },
      {
        337,
        YouTubeQuality.HD2160p60HDR
      }
    };
    private static Dictionary<MediaEncoding, int[]> VideoFormatMap = new Dictionary<MediaEncoding, int[]>()
    {
      {
        MediaEncoding.VP9,
        new int[24]
        {
          171,
          249,
          250,
          251,
          242,
          243,
          244,
          247,
          248,
          271,
          313,
          272,
          302,
          303,
          308,
          315,
          330,
          331,
          332,
          333,
          334,
          335,
          336,
          337
        }
      },
      {
        MediaEncoding.MP4,
        new int[16]
        {
          140,
          22,
          36,
          133,
          18,
          134,
          135,
          222,
          136,
          137,
          298,
          299,
          264,
          266,
          138,
          141
        }
      }
    };
    internal static Dictionary<int, YouTubeQuality> AudioItagMap = new Dictionary<int, YouTubeQuality>()
    {
      {
        140,
        YouTubeQuality.LQ
      },
      {
        141,
        YouTubeQuality.HD1080
      },
      {
        249,
        YouTubeQuality.LQ
      },
      {
        250,
        YouTubeQuality.HQ
      },
      {
        251,
        YouTubeQuality.HD
      }
    };
    internal static YouTubeQuality[] QualitiesWhichAre60FPS = new YouTubeQuality[8]
    {
      YouTubeQuality.HD60,
      YouTubeQuality.HD1080p60,
      YouTubeQuality.HD60HDR,
      YouTubeQuality.HD1080p60HDR,
      YouTubeQuality.HD1440p60,
      YouTubeQuality.HD1440p60HDR,
      YouTubeQuality.HD2160p60,
      YouTubeQuality.HD2160p60HDR
    };
    internal static YouTubeQuality[] QualitiesWhichAreHDR = new YouTubeQuality[4]
    {
      YouTubeQuality.HD60HDR,
      YouTubeQuality.HD1080p60HDR,
      YouTubeQuality.HD1440p60HDR,
      YouTubeQuality.HD2160p60HDR
    };
    internal static int[] ItagsWhichAre60FPS = new int[14]
    {
      298,
      299,
      302,
      303,
      308,
      315,
      330,
      331,
      332,
      333,
      334,
      335,
      336,
      337
    };
    internal static int[] ItagsWhichAreHDR = new int[8]
    {
      330,
      331,
      332,
      333,
      334,
      335,
      336,
      337
    };

    public static int? GetItagFromUrl(URLConstructor url)
    {
      if (url == null)
        return new int?();
      if (url.ContainsKey("itag"))
      {
        int result = 0;
        if (int.TryParse(url["itag"], out result))
          return new int?(result);
      }
      return new int?();
    }

    public static MediaEncoding? GetEncoding(VideoUrlInfo info) => ItagMap.GetEncoding(info.Url);

    public static MediaEncoding? GetEncoding(URLConstructor url) => ItagMap.GetEncoding(ItagMap.GetItagFromUrl(url));

    public static bool IsHDR(YouTubeQuality q) => ((IEnumerable<YouTubeQuality>) ItagMap.QualitiesWhichAreHDR).Contains<YouTubeQuality>(q);

    public static bool IsAdaptive(int? itag) => itag.HasValue && ((IEnumerable<int>) ItagMap.QualitiesWhichNeedAudio).Contains<int>(itag.Value);

    public static MediaEncoding? GetEncoding(int? itag)
    {
      if (!itag.HasValue)
        return new MediaEncoding?();
      foreach (KeyValuePair<MediaEncoding, int[]> videoFormat in ItagMap.VideoFormatMap)
      {
        if (((IEnumerable<int>) videoFormat.Value).Contains<int>(itag.Value))
          return new MediaEncoding?(videoFormat.Key);
      }
      return new MediaEncoding?();
    }

    public static YouTubeQuality? GetQuality(URLConstructor url, MediaType type)
    {
      if (url.ContainsKey("itag"))
      {
        int result = 0;
        if (int.TryParse(url["itag"], out result))
          return ItagMap.GetQuality(new int?(result), type);
      }
      return new YouTubeQuality?();
    }

    public static YouTubeQuality? GetQuality(int? itag, MediaType type)
    {
      if (!itag.HasValue)
        return new YouTubeQuality?();
      if (type != MediaType.Audio)
      {
        if (ItagMap.VideoItagMap.ContainsKey(itag.Value))
          return new YouTubeQuality?(ItagMap.VideoItagMap[itag.Value]);
      }
      else if (ItagMap.AudioItagMap.ContainsKey(itag.Value))
        return new YouTubeQuality?(ItagMap.AudioItagMap[itag.Value]);
      return new YouTubeQuality?();
    }

    public static int GetVerticalResolution(YouTubeQuality qual) => ItagMap.VerticalResolutions.ContainsKey(qual) ? ItagMap.VerticalResolutions[qual] : 1080;

    public static bool EqualResolution(YouTubeQuality qual1, YouTubeQuality qual2) => ItagMap.VerticalResolutions.ContainsKey(qual1) && ItagMap.VerticalResolutions.ContainsKey(qual2) && ItagMap.VerticalResolutions[qual1] == ItagMap.VerticalResolutions[qual2];

    public static bool IsAudio(int itag) => ((IEnumerable<int>) ItagMap.AudioQualities).Contains<int>(itag);
  }
}
