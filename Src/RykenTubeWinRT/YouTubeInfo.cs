// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace RykenTube
{
  public class YouTubeInfo : YouTubeEntry
  {
    public Exception Exception;
    public List<string> Strings;
    public List<string> Urls;
    private bool fv;
    private int tries;
    public bool Enciphered;
    private bool allow60fps;
    public bool AllowAdaptiveVideo = true;
    public bool AllowStandardVideo = true;
    public URLConstructor AnnotationsLink;
    public URLConstructor CaptionsListLink;
    public URLConstructor CaptionsLinksBase;
    private YouTubeInfoUrlContainer standardLinks;
    private YouTubeInfoUrlContainer audioLinks;
    private YouTubeInfoUrlContainer adaptiveLinks;
    private string originalString;
    public bool WatchPage;
    public bool Decipher;
    private Stopwatch initStopWatch = new Stopwatch();
    private TimeSpan initTime;
    private Uri dashMpd;
    internal static Dictionary<string, Func<QualityDecisionArgs, bool>> globalEncodingPreferenceRules = new Dictionary<string, Func<QualityDecisionArgs, bool>>();
    internal Dictionary<string, Func<QualityDecisionArgs, bool>> encodingPreferenceRules = new Dictionary<string, Func<QualityDecisionArgs, bool>>();
    private static object encodingPrefLock = new object();
    internal static int globalEncodingPreferenceCommit = 0;
    internal int encodingPreferenceCommit;
    private string cipherAlgorithm;
    private long zval;

    public static void SetDefaultVideoEncoding(params MediaEncoding[] encoding) => YouTubeInfo.DefaultVideoEncoding = encoding;

    public static void SetDefaultAudioEncoding(params MediaEncoding[] encoding) => YouTubeInfo.DefaultAudioEncoding = encoding;

    public static MediaEncoding[] DefaultVideoEncoding { get; internal set; } = new MediaEncoding[1];

    public static MediaEncoding[] DefaultAudioEncoding { get; internal set; } = new MediaEncoding[1];

    public void SetVideoEncoding(params MediaEncoding[] encoding) => this.SupportedVideoEncoding = encoding;

    public void SetAudioEncoding(params MediaEncoding[] encoding) => this.SupportedAudioEncoding = encoding;

    public MediaEncoding[] SupportedVideoEncoding { get; internal set; } = YouTubeInfo.DefaultVideoEncoding;

    public MediaEncoding[] SupportedAudioEncoding { get; internal set; } = YouTubeInfo.DefaultAudioEncoding;

    public int Tries => this.tries;

    public bool FoundVideos => this.fv;

    public bool Allow60FPS
    {
      get => this.allow60fps && this.AllowAdaptiveVideo;
      set => this.allow60fps = value;
    }

    public bool Is60FPS { get; private set; }

    public bool QualityNeedsAudio(YouTubeQuality q) => this.QualityNeedsAudio(this.GetLink(q));

    public int GetItag(YouTubeQuality q) => int.Parse(this.GetLink(q)["itag"]);

    public uint GetAdaptiveBitrate(YouTubeQuality q)
    {
      VideoUrlInfo videoUrlInfo = this.GetVideoUrlInfo(q, MediaLinkType.Adaptive);
      return videoUrlInfo != null ? videoUrlInfo.Bitrate : 0U;
    }

    public bool QualityIs60FPS(YouTubeQuality q) => ((IEnumerable<YouTubeQuality>) ItagMap.QualitiesWhichAre60FPS).Contains<YouTubeQuality>(q);

    public bool QualityNeedsAudio(VideoUrlInfo info) => this.QualityNeedsAudio(info.Url);

    public bool QualityNeedsAudio(URLConstructor url)
    {
      if (url == null || !url.ContainsKey("itag"))
        return false;
      string s = url["itag"];
      int num = 0;
      ref int local = ref num;
      int.TryParse(s, out local);
      return !((IEnumerable<int>) ItagMap.AudioQualities).Contains<int>(num) && ((IEnumerable<int>) ItagMap.QualitiesWhichNeedAudio).Contains<int>(num);
    }

    public bool QualityIs60FPS(VideoUrlInfo info) => this.QualityIs60FPS(info.Url);

    public bool QualityIs60FPS(URLConstructor url)
    {
      if (!url.ContainsKey("itag"))
        return false;
      string s = url["itag"];
      int num = 0;
      ref int local = ref num;
      int.TryParse(s, out local);
      return ((IEnumerable<int>) ItagMap.ItagsWhichAre60FPS).Contains<int>(num);
    }

    public YouTubeQuality HighestQuality(YouTubeQuality q) => this.HighestQuality(q, MediaLinkType.AllVideo);

    private YouTubeQuality highestQuality(
      ref YouTubeQuality returnQual,
      YouTubeQuality highestQual,
      YouTubeInfoUrlContainer dict)
    {
      foreach (YouTubeQuality key in dict.Keys)
      {
        if (key > returnQual && key <= highestQual && (this.Allow60FPS || !this.QualityIs60FPS(key)) && (this.AllowAdaptiveVideo || !this.QualityNeedsAudio(dict[key])))
          returnQual = key;
      }
      return returnQual;
    }

    public YouTubeQuality HighestQuality(YouTubeQuality q, MediaLinkType type)
    {
      if (q == YouTubeQuality.Audio)
        return YouTubeQuality.Audio;
      YouTubeQuality returnQual = YouTubeQuality.LQ;
      switch (type)
      {
        case MediaLinkType.Standard:
          int num1 = (int) this.highestQuality(ref returnQual, q, this.standardLinks);
          break;
        case MediaLinkType.Adaptive:
          int num2 = (int) this.highestQuality(ref returnQual, q, this.adaptiveLinks);
          break;
        case MediaLinkType.Audio:
          int num3 = (int) this.highestQuality(ref returnQual, q, this.audioLinks);
          break;
        default:
          int num4 = (int) this.highestQuality(ref returnQual, q, this.standardLinks);
          int num5 = (int) this.highestQuality(ref returnQual, q, this.adaptiveLinks);
          break;
      }
      return returnQual;
    }

    public bool HasQuality(YouTubeQuality qual)
    {
      bool flag = this.standardLinks.Contains(qual) || this.adaptiveLinks.Contains(qual) && this.AllowAdaptiveVideo;
      if (this.adaptiveLinks.Contains(qual) && this.QualityIs60FPS(this.adaptiveLinks[qual]) && !this.Allow60FPS)
        flag = false;
      return flag;
    }

    public bool HasAudioQuality(YouTubeQuality qual) => this.audioLinks.Contains(qual) || this.adaptiveLinks.Contains(YouTubeQuality.Audio);

    public VideoUrlInfo GetVideoUrlInfo(YouTubeQuality qual) => this.GetVideoUrlInfo(qual, MediaLinkType.AllVideo);

    public VideoUrlInfo GetVideoUrlInfo(YouTubeQuality qual, MediaLinkType type)
    {
      if (qual == YouTubeQuality.Audio)
        return this.adaptiveLinks.Contains(YouTubeQuality.Audio) ? this.adaptiveLinks[YouTubeQuality.Audio] : (VideoUrlInfo) null;
      YouTubeQuality qual1 = this.HighestQuality(qual, type);
      switch (type)
      {
        case MediaLinkType.Standard:
          return this.getVideoUrlInfo(qual1, this.standardLinks);
        case MediaLinkType.Adaptive:
          return this.getVideoUrlInfo(qual1, this.adaptiveLinks);
        case MediaLinkType.Audio:
          return this.getVideoUrlInfo(qual1, this.audioLinks) ?? this.GetVideoUrlInfo(YouTubeQuality.Audio);
        default:
          return this.getVideoUrlInfo(qual1, this.standardLinks) ?? this.getVideoUrlInfo(qual1, this.adaptiveLinks);
      }
    }

    public URLConstructor GetLink(YouTubeQuality qual) => this.GetLink(qual, MediaLinkType.AllVideo);

    public URLConstructor GetLink(YouTubeQuality qual, MediaLinkType type) => this.GetVideoUrlInfo(qual, type)?.Url;

    private VideoUrlInfo getVideoUrlInfo(YouTubeQuality qual, YouTubeInfoUrlContainer dict) => !dict.Contains(qual) || !this.Allow60FPS && (this.QualityIs60FPS(qual) || !this.AllowAdaptiveVideo && this.QualityNeedsAudio(dict[qual])) ? (VideoUrlInfo) null : dict[qual];

    private URLConstructor GetAudioLink(YouTubeQuality qual) => this.adaptiveLinks.Contains(YouTubeQuality.Audio) ? this.adaptiveLinks[YouTubeQuality.Audio].Url : (URLConstructor) null;

    public new string OriginalString => this.originalString;

    public bool UseNavigatePage { get; private set; }

    public TimeSpan InitializationTime => this.initTime;

    public Uri DashMPD => this.dashMpd;

    public string CPN { get; private set; }

    public string OFValue { get; private set; }

    public string VideoPlaybackStatsBaseUrl { get; private set; }

    public void AddEncodingPreferenceRule(string id, Func<QualityDecisionArgs, bool> func)
    {
      lock (YouTubeInfo.encodingPrefLock)
      {
        ++this.encodingPreferenceCommit;
        if (!this.encodingPreferenceRules.ContainsKey(id))
          this.encodingPreferenceRules.Add(id, func);
        else
          this.encodingPreferenceRules[id] = func;
      }
    }

    public void RemoveEncodingPreferenceRule(string id)
    {
      lock (YouTubeInfo.encodingPrefLock)
      {
        ++this.encodingPreferenceCommit;
        if (!this.encodingPreferenceRules.ContainsKey(id))
          return;
        this.encodingPreferenceRules.Remove(id);
      }
    }

    public static void AddGlobalEncodingPreferenceRule(
      string id,
      Func<QualityDecisionArgs, bool> func)
    {
      lock (YouTubeInfo.encodingPrefLock)
      {
        ++YouTubeInfo.globalEncodingPreferenceCommit;
        if (!YouTubeInfo.globalEncodingPreferenceRules.ContainsKey(id))
          YouTubeInfo.globalEncodingPreferenceRules.Add(id, func);
        else
          YouTubeInfo.globalEncodingPreferenceRules[id] = func;
      }
    }

    private YouTubeInfo()
    {
    }

    public YouTubeInfo(
      string s,
      bool watchpage = false,
      bool decipher = false,
      bool useNavigatePage = false,
      string cipherAlgorithm = null)
    {
      this.audioLinks = new YouTubeInfoUrlContainer(this, MediaType.Audio);
      this.standardLinks = new YouTubeInfoUrlContainer(this, MediaType.Video);
      this.adaptiveLinks = new YouTubeInfoUrlContainer(this, MediaType.Video);
      this.UseNavigatePage = useNavigatePage;
      this.WatchPage = watchpage;
      this.Decipher = decipher;
      this.fv = false;
      this.cipherAlgorithm = cipherAlgorithm;
      this.fv = this.init(s, watchpage, decipher, !useNavigatePage, useNavigatePage);
    }

    public bool ReInit(string s)
    {
      this.fv = false;
      this.fv = this.init(s, false, false);
      return this.fv;
    }

    public static YouTubeInfo FromServer(string info, bool decipher)
    {
      YouTubeInfo youTubeInfo = new YouTubeInfo()
      {
        WatchPage = false,
        Decipher = false,
        fv = false
      };
      youTubeInfo.fv = youTubeInfo.init(info, false, youTubeInfo.Decipher);
      return youTubeInfo;
    }

    private bool init(string s, bool watch, bool decrypt, bool decode = true, bool useNavigatinPage = false)
    {
      this.audioLinks.Clear();
      this.standardLinks.Clear();
      this.adaptiveLinks.Clear();
      this.Strings = new List<string>();
      this.Urls = new List<string>();
      string str1 = this.originalString = s;
      if (!watch)
        return this.initFromGetVideoInfo(s, watch, decrypt);
      if (decode)
        s = WebUtility.UrlDecode(s);
      bool cipher = decrypt;
      if (!watch && ((s.Contains("use_cipher_signature=True") || s.Contains("use_cipher_signature=true") ? 1 : (s.Contains("use_cipher_signature=1") ? 1 : 0)) | (decrypt ? 1 : 0)) != 0)
      {
        cipher = true;
        this.Enciphered = true;
        if (!decrypt)
          return false;
      }
      try
      {
        if (watch)
        {
          JObject JSON = (JObject) null;
          string json;
          if (!useNavigatinPage)
          {
            bool flag1 = false;
            byte num = 0;
            int startIndex = str1.IndexOf("ytplayer.config");
            string str2;
            if (startIndex != -1)
            {
              str2 = str1.Substring(startIndex);
              startIndex = 0;
            }
            else
              str2 = str1;
            int length = str2.Length;
            bool flag2 = false;
            int index;
            for (index = startIndex; index < length; ++index)
            {
              switch (str2[index])
              {
                case '{':
                  if (!flag1)
                  {
                    startIndex = index;
                    flag1 = true;
                    ++num;
                    break;
                  }
                  ++num;
                  break;
                case '}':
                  if (flag1)
                  {
                    --num;
                    if (num == (byte) 0)
                    {
                      flag2 = true;
                      break;
                    }
                    break;
                  }
                  break;
              }
              if (flag1 && flag2)
                break;
            }
            json = str2.Substring(startIndex, index - startIndex + 1);
          }
          else
          {
            json = s;
            JArray jarray = JArray.Parse(s);
            for (int index = 0; index < jarray.Count; ++index)
            {
              try
              {
                JSON = (jarray[index][(object) "data"] as JObject)["swfcfg"] as JObject;
                break;
              }
              catch
              {
              }
            }
          }
          if (JSON == null)
            JSON = JObject.Parse(json);
          return this.initFromJson(JSON, watch, cipher, useNavigatinPage);
        }
      }
      catch
      {
      }
      if (s.Contains("url_encoded_fmt_stream_map="))
      {
        string[] strArray = s.Split("url_encoded_fmt_stream_map=");
        if (strArray.Length > 1)
          s = strArray[1];
      }
      else
        s.Contains("\"url_encoded_fmt_stream_map\": ");
      return this.GetVideosFromString(s, watch, cipher);
    }

    private bool initFromGetVideoInfo(string s, bool watch, bool cipher)
    {
      URLConstructor urlConstructor1 = new URLConstructor(s);
      string s1 = urlConstructor1["url_encoded_fmt_stream_map"];
      string url1 = urlConstructor1["rvs"];
      if (url1 != null)
      {
        URLConstructor urlConstructor2 = new URLConstructor(url1);
      }
      string url2 = urlConstructor1["ttsurl"];
      if (url2 != null)
      {
        URLConstructor urlConstructor3 = new URLConstructor(url2);
        URLConstructor urlConstructor4 = new URLConstructor(url2);
        urlConstructor4.SetValue("type", (object) "list");
        urlConstructor4.SetValue("tlangs", (object) "1");
        urlConstructor4.SetValue("asrs", (object) "1");
        this.CaptionsListLink = urlConstructor4;
      }
      string url3 = urlConstructor1["iv_invideo_url"];
      if (url3 != null)
        this.AnnotationsLink = new URLConstructor(url3);
      if (s1 != null)
        this.GetVideosFromString(s1, watch, cipher);
      string s2 = urlConstructor1["adaptive_fmts"];
      if (s2 != null)
        this.GetVideosFromString(s2, watch, cipher);
      string str1 = urlConstructor1["dashmpd"];
      if (str1 != null)
      {
        try
        {
          string[] strArray = str1.Split('/');
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (strArray[index] == nameof (s))
            {
              strArray[index] = "signature";
              string str2 = Cipher.NewDecipher(strArray[index + 1], YouTube.DecipherAlgorithm);
              strArray[index + 1] = str2;
            }
          }
          string uriString = "";
          for (int index = 0; index < strArray.Length; ++index)
            uriString = uriString + strArray[index] + (index < strArray.Length - 1 ? "/" : "");
          this.dashMpd = new Uri(uriString, UriKind.Absolute);
        }
        catch
        {
        }
      }
      string str3 = urlConstructor1["videostats_playback_base_url"];
      if (str3 != null)
        this.VideoPlaybackStatsBaseUrl = str3;
      string str4 = urlConstructor1["of"];
      if (str4 != null)
        this.OFValue = str4;
      return true;
    }

    private bool initFromJson(JObject JSON, bool watch, bool cipher, bool useNavigatePage)
    {
      JToken jtoken1 = JSON["args"];
      JSON.ToString();
      jtoken1.ToString();
      JToken jtoken2 = jtoken1[(object) "url_encoded_fmt_stream_map"];
      try
      {
        string str1 = (string) jtoken1[(object) "dashmpd"];
        if (str1 != null)
        {
          try
          {
            string[] strArray = str1.Split('/');
            for (int index = 0; index < strArray.Length; ++index)
            {
              if (strArray[index] == "s")
              {
                strArray[index] = "signature";
                string str2 = Cipher.NewDecipher(strArray[index + 1], YouTube.DecipherAlgorithm);
                strArray[index + 1] = str2;
              }
            }
            string uriString = "";
            for (int index = 0; index < strArray.Length; ++index)
              uriString = uriString + strArray[index] + (index < strArray.Length - 1 ? "/" : "");
            this.dashMpd = new Uri(uriString, UriKind.Absolute);
          }
          catch
          {
          }
        }
        try
        {
          string url = (string) jtoken1[(object) "ttsurl"];
          string str3 = (string) jtoken1[(object) "timestamp"];
          if (url != null)
          {
            URLConstructor urlConstructor1 = new URLConstructor(url);
            URLConstructor urlConstructor2 = new URLConstructor(url);
            urlConstructor2.SetValue("type", (object) "list");
            urlConstructor2.SetValue("tlangs", (object) "1");
            urlConstructor2.SetValue("asrs", (object) "1");
            this.CaptionsListLink = urlConstructor2;
          }
        }
        catch
        {
        }
        try
        {
          this.AnnotationsLink = new URLConstructor((string) jtoken1[(object) "iv_invideo_url"]);
        }
        catch
        {
        }
      }
      catch
      {
      }
      this.GetVideosFromString(jtoken2.Value<string>(), watch, cipher);
      JToken jtoken3 = jtoken1[(object) "adaptive_fmts"];
      if (jtoken3 != null)
        this.GetVideosFromString(jtoken3.Value<string>(), watch, cipher);
      string str = (string) jtoken1[(object) "videostats_playback_base_url"];
      if (str != null)
        this.VideoPlaybackStatsBaseUrl = str;
      return true;
    }

    private bool GetVideosFromString(string s, bool watch, bool cipher)
    {
      bool videosFromString = false;
      string[] strArray = s.Split(',');
      if (strArray.Length != 0)
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          string str;
          try
          {
            str = strArray[index];
          }
          catch
          {
            continue;
          }
          if (str.Contains("url=") && (str.Contains("sig") || str.Contains("s=")))
          {
            URLConstructor urlConstructor = new URLConstructor(str);
            VideoUrlInfo videoUrlInfo = new VideoUrlInfo(urlConstructor);
            string encodedValue;
            try
            {
              encodedValue = urlConstructor["url"] == null ? ((IEnumerable<string>) str.Split("url=")).Last<string>() : urlConstructor["url"];
            }
            catch
            {
              continue;
            }
            try
            {
              if (WebUtility.UrlDecode(encodedValue).Contains("itag="))
              {
                URLConstructor url = videoUrlInfo.Url;
                int result = 0;
                if (url.ContainsKey("itag"))
                {
                  if (int.TryParse(url["itag"], out result))
                  {
                    this.getsig(url, urlConstructor, watch);
                    bool flag = this.verifyLink(url);
                    if (ItagMap.VideoItagMap.ContainsKey(result))
                    {
                      YouTubeQuality videoItag = ItagMap.VideoItagMap[result];
                      YouTubeInfoUrlContainer infoUrlContainer = !((IEnumerable<int>) ItagMap.QualitiesWhichNeedAudio).Contains<int>(result) ? this.standardLinks : this.adaptiveLinks;
                      videosFromString = true;
                      if (url.ContainsKey("cpn"))
                        this.CPN = url["cpn"];
                      if (flag)
                        infoUrlContainer.Add(videoUrlInfo.Clone(), videoItag);
                    }
                    if (ItagMap.AudioItagMap.ContainsKey(result))
                    {
                      YouTubeQuality audioItag = ItagMap.AudioItagMap[result];
                      if (flag)
                        this.audioLinks.Add(videoUrlInfo.Clone(), audioItag);
                    }
                  }
                }
              }
            }
            catch (Exception ex)
            {
              this.Exception = ex;
            }
          }
        }
      }
      return videosFromString;
    }

    private bool verifyLink(URLConstructor con)
    {
      bool flag = true;
      if (con.ContainsKey("sparams"))
      {
        foreach (string key in con["sparams"].Split(","))
        {
          if (!con.ContainsKey(key))
            flag = false;
        }
      }
      else
        flag = false;
      return flag;
    }

    public void PerformDecipher(string decipherAlghorithm = null)
    {
      this.Decipher = true;
      foreach (MediaEncoding e in Enum.GetValues(typeof (MediaEncoding)))
      {
        this.performDecipher(this.adaptiveLinks[e], decipherAlghorithm);
        this.performDecipher(this.standardLinks[e], decipherAlghorithm);
        this.performDecipher(this.audioLinks[e], decipherAlghorithm);
      }
    }

    private void performDecipher(
      Dictionary<YouTubeQuality, VideoUrlInfo> dict,
      string decipherAlgorithm = null)
    {
      foreach (KeyValuePair<YouTubeQuality, VideoUrlInfo> keyValuePair in dict)
      {
        if (keyValuePair.Value.Url.ContainsKey("signature"))
        {
          string a = keyValuePair.Value.Url["signature"];
          keyValuePair.Value.Url["signature"] = Cipher.NewDecipher(a, decipherAlgorithm ?? this.cipherAlgorithm ?? YouTube.DecipherAlgorithm);
        }
      }
    }

    private void SetExtraProperties(URLConstructor l, string st)
    {
    }

    private string SetCPN()
    {
      byte[] numArray = this.JE();
      List<char> values = new List<char>();
      for (int index = 0; index < numArray.Length; ++index)
        values.Add("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_"[(int) numArray[index] % 63]);
      return string.Join<char>("", (IEnumerable<char>) values);
    }

    private byte[] JE()
    {
      Random random = new Random();
      byte[] numArray = new byte[16];
      for (int index1 = 0; index1 < 16; ++index1)
      {
        long num1 = this.z();
        for (int index2 = 0; (long) index2 < num1 % 23L; ++index2)
        {
          byte num2 = (byte) (256.0 * random.NextDouble());
          numArray[index1] = num2;
        }
      }
      return numArray;
    }

    private long z()
    {
      if (this.zval == 0L)
        this.zval = DateTime.Now.ToFileTimeUtc();
      return this.zval;
    }

    public string Slice(string s, int start = 0, int end = 0) => s.Substring(start, end - start);

    private string decipher(string s) => this.cipherAlgorithm != null || YouTube.DecipherAlgorithm != null ? Cipher.NewDecipher(s, this.cipherAlgorithm ?? YouTube.DecipherAlgorithm) : s;

    private string spliturls(string link, string cond, string cond2)
    {
      if (link.Contains(cond) && link.Contains(cond2) || !link.Contains("url="))
        return link;
      string str1 = link;
      string[] separator = new string[1]{ "url=" };
      foreach (string str2 in str1.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        if (str2.Contains(cond) && str2.Contains(cond2) && str2.Contains("http"))
          return str2;
      }
      return link;
    }

    private void getsig(URLConstructor url, URLConstructor urlParent, bool watch = false)
    {
      if (url.ContainsKey("signature") || !urlParent.ContainsKey("s"))
        return;
      url["signature"] = urlParent["s"];
    }

    private void getsig(string[] ss, URLConstructor l, int i, string cond, string cond2)
    {
      string str1 = "";
      bool flag1 = false;
      if (!flag1 && i < ss.Length - 1 && ss[i + 1].Contains(cond) && ss[i + 1].Contains(cond2))
      {
        str1 = ss[i + 1];
        flag1 = true;
      }
      if (!flag1 && i > 0 && ss[i - 1].Contains(cond) && ss[i - 1].Contains(cond2))
      {
        str1 = ss[i - 1];
        flag1 = true;
        this.tries = 2;
      }
      if (!flag1 && ss[i].Contains(cond) && ss[i].Contains(cond2))
      {
        str1 = ss[i];
        flag1 = true;
      }
      if (!flag1 || str1.Length <= 5)
        return;
      string str2 = "";
      bool flag2 = false;
      if (str1.Contains("signature"))
      {
        for (int index = str1.IndexOf("signature"); index < str1.Length; ++index)
        {
          char ch = str1[index];
          switch (ch)
          {
            case '%':
            case '&':
              goto label_16;
            case '=':
              if (!flag2)
              {
                flag2 = true;
                break;
              }
              goto default;
            default:
              if (flag2)
              {
                str2 += ch.ToString();
                break;
              }
              break;
          }
        }
      }
label_16:
      if (l.ContainsKey("signature"))
        return;
      l.SetValue("signature", (object) str2);
    }
  }
}
