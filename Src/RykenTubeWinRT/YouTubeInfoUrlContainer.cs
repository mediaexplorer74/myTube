// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeInfoUrlContainer
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;

namespace RykenTube
{
  public class YouTubeInfoUrlContainer
  {
    private Dictionary<MediaEncoding, Dictionary<YouTubeQuality, VideoUrlInfo>> links = new Dictionary<MediaEncoding, Dictionary<YouTubeQuality, VideoUrlInfo>>();
    internal YouTubeInfo info;
    private MediaType type;
    private List<YouTubeQuality> keys;
    private int commit = -1;
    private int globalCommit = -1;

    internal Dictionary<YouTubeQuality, VideoUrlInfo> this[MediaEncoding e] => this.links[e];

    public IEnumerable<YouTubeQuality> Keys
    {
      get
      {
        if (this.keys == null || this.commit != this.info.encodingPreferenceCommit || this.globalCommit != YouTubeInfo.globalEncodingPreferenceCommit)
        {
          this.commit = this.info.encodingPreferenceCommit;
          this.globalCommit = YouTubeInfo.globalEncodingPreferenceCommit;
          this.keys = new List<YouTubeQuality>();
          foreach (MediaEncoding mediaEncoding in this.Encoding)
          {
            foreach (KeyValuePair<YouTubeQuality, VideoUrlInfo> keyValuePair in this.links[mediaEncoding])
            {
              if (!this.keys.Contains(keyValuePair.Key) && this.supportsQuality(keyValuePair.Key, this.type, mediaEncoding, keyValuePair.Value.Url))
                this.keys.Add(keyValuePair.Key);
            }
          }
        }
        return (IEnumerable<YouTubeQuality>) this.keys;
      }
    }

    public VideoUrlInfo this[YouTubeQuality q]
    {
      get
      {
        foreach (MediaEncoding mediaEncoding in this.Encoding)
        {
          if (this.links[mediaEncoding].ContainsKey(q) && this.supportsQuality(q, this.type, mediaEncoding, this.links[mediaEncoding][q]))
            return this.links[mediaEncoding][q];
        }
        return (VideoUrlInfo) null;
      }
    }

    internal YouTubeInfoUrlContainer(YouTubeInfo info, MediaType type)
    {
      this.type = type;
      this.info = info;
      foreach (MediaEncoding key in Enum.GetValues(typeof (MediaEncoding)))
        this.links.Add(key, new Dictionary<YouTubeQuality, VideoUrlInfo>());
    }

    private MediaEncoding[] Encoding => this.type != MediaType.Audio ? this.info.SupportedVideoEncoding : this.info.SupportedAudioEncoding;

    private bool supportsQuality(
      YouTubeQuality qual,
      MediaType type,
      MediaEncoding enc,
      VideoUrlInfo info)
    {
      return this.supportsQuality(qual, type, enc, info.Url);
    }

    private bool supportsQuality(
      YouTubeQuality qual,
      MediaType type,
      MediaEncoding enc,
      URLConstructor url)
    {
      QualityDecisionArgs qualityDecisionArgs = new QualityDecisionArgs()
      {
        Quality = qual,
        Itag = ItagMap.GetItagFromUrl(url),
        Type = type,
        Encoding = enc
      };
      foreach (KeyValuePair<string, Func<QualityDecisionArgs, bool>> encodingPreferenceRule in this.info.encodingPreferenceRules)
      {
        try
        {
          if (!encodingPreferenceRule.Value(qualityDecisionArgs))
            return false;
        }
        catch
        {
        }
      }
      foreach (KeyValuePair<string, Func<QualityDecisionArgs, bool>> encodingPreferenceRule in YouTubeInfo.globalEncodingPreferenceRules)
      {
        try
        {
          if (!encodingPreferenceRule.Value(qualityDecisionArgs))
            return false;
        }
        catch
        {
        }
      }
      return true;
    }

    public void Clear()
    {
      foreach (KeyValuePair<MediaEncoding, Dictionary<YouTubeQuality, VideoUrlInfo>> link in this.links)
        link.Value.Clear();
    }

    public bool Contains(YouTubeQuality q)
    {
      foreach (MediaEncoding mediaEncoding in this.Encoding)
      {
        if (this.links[mediaEncoding].ContainsKey(q) && this.supportsQuality(q, this.type, mediaEncoding, this.links[mediaEncoding][q]))
          return true;
      }
      return false;
    }

    internal void Add(VideoUrlInfo info, YouTubeQuality qual)
    {
      MediaEncoding? encoding = ItagMap.GetEncoding(info);
      if (!encoding.HasValue)
        return;
      this.Add(info, qual, encoding.Value);
    }

    internal void Add(VideoUrlInfo info, YouTubeQuality qual, MediaEncoding encoding)
    {
      this.keys = (List<YouTubeQuality>) null;
      Dictionary<YouTubeQuality, VideoUrlInfo> link = this.links[encoding];
      if (link.ContainsKey(qual))
        link[qual] = info;
      else
        link.Add(qual, info);
    }
  }
}
