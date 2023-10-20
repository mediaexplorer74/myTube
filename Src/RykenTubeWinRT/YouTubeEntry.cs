// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeEntry
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace RykenTube
{
  public class YouTubeEntry : ClientData<YouTubeEntry>
  {
    public static ThumbnailQuality DefaultThumbnailQuality = ThumbnailQuality.Med;
    private ThumbnailQuality thumbQual = YouTubeEntry.DefaultThumbnailQuality;
    private YouTubeActivity activityType;
    private LiveStatus liveStatus;
    private string title = "A Title";
    public string authorDisplayName = "uploader";
    private Uri thumbnail;
    private string description = "No description.";
    private string author = "uploader not found";
    private string id = "xB-RyQq-Egg";
    private DateTimeOffset? publishAt;
    private Category category;
    private List<string> tags = new List<string>();
    public string PlaylistID = "";
    public string ThumbLink = "http://s.ytimg.com/yts/img/no_thumbnail-vfl4t3-4R.jpg";
    private Dictionary<string, string> ThumbLinks;
    private int likes;
    private int dislikes;
    private ulong views;
    public bool IsHD;
    public bool Restricted;
    private TimeSpan duration = new TimeSpan(0, 13, 37);
    private DateTime time = new DateTime(2014, 6, 14);
    public bool Comment = true;
    private static string[] thumbs = new string[5]
    {
      "default",
      "mqdefault",
      "hqdefault",
      "sddefault",
      "maxres"
    };
    private XElement xml;
    private JToken json;
    public string FavoriteID;
    private bool embeddable = true;
    private bool publicStatsVisible = true;
    private License license;
    private PrivacyStatus privacyStatus;
    private VideoProjection projection;

    public override bool NeedsRefresh { get; set; }

    public YouTubeActivity ActivityType
    {
      get => this.activityType;
      set => this.activityType = value;
    }

    public LiveStatus LiveStatus
    {
      get => this.liveStatus;
      set
      {
        this.liveStatus = value;
        this.OnPropertyChanged(nameof (LiveStatus));
      }
    }

    public ThumbnailQuality ThumbnailQuality
    {
      get => this.thumbQual;
      set
      {
        if (this.thumbQual == value)
          return;
        this.thumbQual = value;
        this.OnPropertyChanged("Thumbnail");
      }
    }

    public string Title
    {
      get => this.title;
      set
      {
        this.title = value;
        this.OnPropertyChanged(nameof (Title));
      }
    }

    public string AuthorDisplayName
    {
      get => this.authorDisplayName;
      set
      {
        this.authorDisplayName = value;
        this.OnPropertyChanged(nameof (AuthorDisplayName));
      }
    }

    public Uri this[ThumbnailQuality qual] => this.GetThumb(qual);

    public Uri HighDefThumbnail => this.GetThumb(ThumbnailQuality.MaxRes);

    public Uri Thumbnail
    {
      set
      {
        this.thumbnail = value;
        this.OnPropertyChanged(nameof (Thumbnail));
        this.OnPropertyChanged("ThumbnailString");
      }
      get => this.thumbnail == (Uri) null ? this.GetThumb(this.ThumbnailQuality) : this.thumbnail;
    }

    public string ThumbnailString
    {
      set
      {
        this.thumbnail = new Uri(value);
        this.OnPropertyChanged("Thumbnail");
        this.OnPropertyChanged(nameof (ThumbnailString));
      }
      get => this.thumbnail == (Uri) null ? this.GetThumb(this.ThumbnailQuality).OriginalString : this.thumbnail.OriginalString;
    }

    public string Description
    {
      get => this.description;
      set
      {
        this.description = value;
        this.OnPropertyChanged(nameof (Description));
      }
    }

    public string Author
    {
      get => this.author;
      set
      {
        this.author = value;
        this.OnPropertyChanged(nameof (Author));
      }
    }

    public override string ID
    {
      get => this.id;
      set
      {
        this.id = value;
        this.OnPropertyChanged(nameof (ID));
      }
    }

    public int Likes
    {
      get => this.likes;
      set
      {
        this.likes = value;
        this.OnPropertyChanged(nameof (Likes));
        this.OnPropertyChanged("LikesPercentage");
      }
    }

    public Category Category
    {
      get => this.category;
      set => this.category = value;
    }

    public DateTimeOffset? PublishAt
    {
      get => this.publishAt;
      set
      {
        if (this.publishAt.HasValue && !(this.publishAt.Value != value.Value))
          return;
        this.publishAt = value;
        this.OnPropertyChanged(nameof (PublishAt));
      }
    }

    public List<string> Tags => this.tags;

    public int Dislikes
    {
      get => this.dislikes;
      set
      {
        this.dislikes = value;
        this.OnPropertyChanged(nameof (Dislikes));
        this.OnPropertyChanged("LikesPercentage");
      }
    }

    public ulong Views
    {
      get => this.views;
      set
      {
        this.views = value;
        this.OnPropertyChanged(nameof (Views));
      }
    }

    public double LikesPercentage => (double) this.likes / (double) (this.likes + this.dislikes);

    public TimeSpan Duration
    {
      get => this.duration;
      set
      {
        this.duration = value;
        this.OnPropertyChanged(nameof (Duration));
      }
    }

    public DateTime Time
    {
      get => this.time;
      set
      {
        this.time = value;
        this.OnPropertyChanged(nameof (Time));
      }
    }

    public bool PublicStatsVisible
    {
      get => this.publicStatsVisible;
      set
      {
        if (this.publicStatsVisible == value)
          return;
        this.publicStatsVisible = value;
        this.OnPropertyChanged(nameof (PublicStatsVisible));
      }
    }

    public bool Embeddable
    {
      get => this.embeddable;
      set
      {
        if (value == this.embeddable)
          return;
        this.embeddable = value;
        this.OnPropertyChanged(nameof (Embeddable));
      }
    }

    public License License
    {
      get => this.license;
      set
      {
        if (this.license == value)
          return;
        this.license = value;
        this.OnPropertyChanged(nameof (License));
      }
    }

    public PrivacyStatus PrivacyStatus
    {
      get => this.privacyStatus;
      set
      {
        if (this.privacyStatus == value)
          return;
        this.privacyStatus = value;
        this.OnPropertyChanged(nameof (PrivacyStatus));
      }
    }

    public VideoProjection Projection
    {
      get => this.projection;
      set => this.projection = value;
    }

    private XElement XML => this.xml;

    private JToken Json => this.json;

    public string OriginalString
    {
      get
      {
        if (this.xml != null)
          return this.xml.ToString();
        return this.json != null ? this.json.ToString() : (string) null;
      }
    }

    public YouTubeEntry(string originalString)
      : this()
    {
      JToken json = (JToken) null;
      try
      {
        json = JToken.Parse(originalString);
      }
      catch
      {
      }
      if (json != null)
      {
        this.SetValuesFromJson(json);
      }
      else
      {
        try
        {
          this.SetValuesFromXML(XElement.Parse(originalString));
        }
        catch
        {
        }
      }
    }

    public YouTubeEntry()
    {
      this.ThumbnailQuality = YouTubeEntry.DefaultThumbnailQuality;
      this.ThumbLinks = new Dictionary<string, string>();
    }

    public YouTubeEntry(JToken json)
      : this()
    {
      this.SetValuesFromJson(json);
    }

    public override void SetValuesFromJson(JToken json)
    {
      if (json == null)
        return;
      JToken json1 = json[(object) "snippet"];
      JToken json2 = json[(object) "contentDetails"];
      JToken json3 = json[(object) "statistics"];
      string str1 = (string) json[(object) "kind"];
      switch (str1)
      {
        case "youtube#video":
          if (this.json != null)
          {
            this.json[(object) "kind"] = (JToken) "youtube#video";
            break;
          }
          break;
      }
      JToken jtoken1 = json[(object) "id"];
      if (jtoken1 is JValue)
      {
        if (str1 == "youtube#playlistItem")
        {
          this.PlaylistID = this.FavoriteID = this.id = (string) jtoken1;
        }
        else
        {
          if (this.json != null)
            this.json[(object) "id"] = jtoken1;
          this.ID = (string) jtoken1;
        }
      }
      else if (jtoken1 is JObject)
      {
        JToken jtoken2 = jtoken1[(object) "videoId"];
        if (jtoken2 != null)
          this.ID = (string) jtoken2;
      }
      if (json1 != null)
      {
        if (this.json != null)
          this.json[(object) "snippet"] = json1;
        JSONResourceCreator.GetValue<string>(json1, out this.title, this.title, "title");
        JSONResourceCreator.GetValue<string>(json1, out this.description, this.description, "description");
        JSONResourceCreator.GetValue<string>(json1, out this.author, this.Author, "channelId");
        JSONResourceCreator.GetValue<string>(json1, out this.authorDisplayName, this.authorDisplayName, "channelTitle");
        JToken jtoken3 = json1[(object) "thumbnails"];
        if (jtoken3 != null)
        {
          if (jtoken3[(object) "default"] != null && jtoken3[(object) "default"][(object) "url"] != null)
            this.ThumbLinks["default"] = (string) jtoken3[(object) "default"][(object) "url"];
          if (jtoken3[(object) "medium"] != null && jtoken3[(object) "medium"][(object) "url"] != null)
            this.ThumbLinks["mqdefault"] = (string) jtoken3[(object) "medium"][(object) "url"];
          if (jtoken3[(object) "high"] != null && jtoken3[(object) "high"][(object) "url"] != null)
            this.ThumbLinks["hqdefault"] = (string) jtoken3[(object) "high"][(object) "url"];
          if (jtoken3[(object) "standard"] != null && jtoken3[(object) "standard"][(object) "url"] != null)
            this.ThumbLinks["sddefault"] = (string) jtoken3[(object) "standard"][(object) "url"];
          if (jtoken3[(object) "maxres"] != null && jtoken3[(object) "maxres"][(object) "url"] != null)
            this.ThumbLinks["maxres"] = (string) jtoken3[(object) "maxres"][(object) "url"];
        }
        json1.GetValue<DateTime>(out this.time, this.time, "publishedAt");
        try
        {
          if (json1[(object) "type"] != null)
          {
            switch ((string) json1[(object) "type"])
            {
              case "upload":
                this.activityType = YouTubeActivity.Upload;
                break;
              case "recommendation":
                this.activityType = YouTubeActivity.Recommended;
                break;
              case "like":
                this.activityType = YouTubeActivity.Like;
                break;
            }
          }
        }
        catch
        {
        }
        try
        {
          if (json1[(object) "resourceId"] is JObject)
          {
            if (json1[(object) "resourceId"][(object) "videoId"] is JValue)
              this.id = (string) json1[(object) "resourceId"][(object) "videoId"];
          }
        }
        catch
        {
        }
        string outValue = "none";
        if (JSONResourceCreator.GetValue<string>(json1, out outValue, "none", "liveBroadcastContent"))
        {
          switch (outValue)
          {
            case "none":
              this.liveStatus = LiveStatus.None;
              break;
            case "live":
              this.liveStatus = LiveStatus.Live;
              break;
            case "upcoming":
              this.liveStatus = LiveStatus.Upcoming;
              break;
          }
        }
      }
      if (json3 != null)
      {
        if (this.json != null)
          this.json[(object) "statistics"] = json3;
        json3.GetValue<ulong>(out this.views, this.views, "viewCount");
        this.Likes = json3.GetValue<int>(0, "likeCount");
        this.Dislikes = json3.GetValue<int>(0, "dislikeCount");
      }
      if (json2 != null)
      {
        if (this.json != null)
          this.json[(object) "contentDetails"] = json2;
        if (json2[(object) "duration"] != null && !TimeSpan.TryParse((string) json2[(object) "duration"], out this.duration))
        {
          string source = "0123456789";
          string s = "";
          int hours = 0;
          int minutes = 0;
          int seconds = 0;
          foreach (char ch in (string) json2[(object) "duration"])
          {
            if (source.Contains<char>(ch))
              s += ch.ToString();
            switch (ch)
            {
              case 'H':
                hours = int.Parse(s);
                s = "";
                break;
              case 'M':
                minutes = int.Parse(s);
                s = "";
                break;
              case 'S':
                seconds = int.Parse(s);
                s = "";
                break;
            }
          }
          this.duration = new TimeSpan(hours, minutes, seconds);
        }
        if (json2[(object) "upload"] is JObject)
        {
          string str2 = (string) json2[(object) "upload"][(object) "videoId"];
          if (str2 != null)
            this.id = str2;
        }
        if (json2[(object) "like"] is JObject)
        {
          string str3 = json2.GetValue<string>((string) null, "like", "resourceId", "videoId");
          if (str3 != null)
            this.id = str3;
        }
        if (json2[(object) "recommendation"] is JObject)
        {
          string str4 = json2.GetValue<string>((string) null, "recommendation", "resourceId", "videoId");
          if (str4 != null)
            this.id = str4;
        }
        if (json2[(object) "videoId"] is JValue)
          this.id = (string) json2[(object) "videoId"];
        if (json2.GetValue<string>("rectangular", "projection").Contains("360"))
          this.projection = VideoProjection.Spherical;
      }
      if (this.json != null)
        return;
      this.json = json;
    }

    internal YouTubeEntry(XElement xml) => this.SetValuesFromXML(xml);

    public static YouTubeEntry FromRSS(XElement xml)
    {
      YouTubeEntry youTubeEntry = new YouTubeEntry();
      XElement xelement1 = xml.Element(YouTube.Atom("videoId", "http://www.youtube.com/xml/schemas/2015"));
      if (xelement1 != null)
        youTubeEntry.ID = xelement1.Value;
      XElement xelement2 = xml.Element(YouTube.Atom("published"));
      if (xelement2 != null)
        DateTime.TryParse(xelement2.Value, out youTubeEntry.time);
      XElement xelement3 = xml.Element(YouTube.Atom("title"));
      if (xelement3 != null)
        youTubeEntry.title = xelement3.Value;
      XElement xelement4 = xml.Element(YouTube.Atom("author"));
      if (xelement4 != null)
      {
        XElement xelement5 = xelement4.Element(YouTube.Atom("name"));
        if (xelement5 != null)
          youTubeEntry.authorDisplayName = xelement5.Value;
      }
      XElement xelement6 = xml.Element(YouTube.Atom("group", "media"));
      if (xelement6 != null)
      {
        XElement xelement7 = xelement6.Element(YouTube.Atom("thumbnail", "media"));
        if (xelement7 != null)
        {
          XAttribute xattribute = xelement7.Attribute((XName) "url");
          if (xattribute != null)
            youTubeEntry.ThumbLinks.Add("default", xattribute.Value);
        }
      }
      return youTubeEntry;
    }

    public override void SetValuesFromXML(XElement xml)
    {
      this.ThumbnailQuality = YouTubeEntry.DefaultThumbnailQuality;
      this.xml = xml;
      XElement group = xml.Element(YouTube.Atom("group", "media"));
      this.init2(group);
      this.Title = xml.Element(YouTube.Atom("title")).Value;
      XElement xelement1 = group.Element(YouTube.Atom("description", "media"));
      if (xelement1 == null)
        this.Restricted = true;
      else
        this.Description = xelement1.Value;
      XElement xelement2 = xml.Element(YouTube.Atom("statistics", "yt"));
      if (xelement2 != null)
      {
        XAttribute xattribute = xelement2.Attribute((XName) "viewCount");
        if (xattribute != null)
          this.Views = ulong.Parse(xattribute.Value);
      }
      XElement xelement3 = group.Element(YouTube.Atom("videoid", "yt"));
      if (xelement3 != null)
        this.ID = xelement3.Value;
      XElement xelement4 = xml.Element(YouTube.Atom("videoId", "http://www.youtube.com/xml/schemas/2015"));
      if (xelement4 != null)
        this.ID = xelement4.Value;
      XElement xelement5 = xml.Element(YouTube.Atom("published"));
      if (xelement5 != null)
        DateTime.TryParse(xelement5.Value, out this.time);
      this.IsHD = xml.Element(YouTube.Atom("hd", "yt")) != null;
      XElement xelement6 = xml.Element(YouTube.Atom("control", "app"));
      if (xelement6 == null)
      {
        this.Restricted = false;
      }
      else
      {
        XElement xelement7 = xelement6.Element(YouTube.Atom("state", "yt"));
        if (xelement7 != null && xelement7.Value == "Account Suspended")
          this.Restricted = true;
      }
      XElement xelement8 = group.Element(YouTube.Atom("credit", "media"));
      if (xelement8 != null)
      {
        this.Author = xelement8.Value;
        this.AuthorDisplayName = xelement8.Attribute(YouTube.Atom("display", "yt")).Value;
      }
      XElement xelement9 = group.Element(YouTube.Atom("uploaderId", "yt"));
      if (xelement9 != null)
        this.Author = xelement9.Value;
      try
      {
        XAttribute xattribute = xml.Element(YouTube.Atom("group", "media")).Element(YouTube.Atom("duration", "yt")).Attribute((XName) "seconds");
        this.Duration = xattribute == null ? TimeSpan.FromSeconds(0.0) : TimeSpan.FromSeconds((double) int.Parse(xattribute.Value));
      }
      catch
      {
      }
      XElement xelement10 = xml.Element(YouTube.Atom("favoriteId", "yt"));
      if (xelement10 != null)
        this.FavoriteID = xelement10.Value;
      try
      {
        string[] strArray1 = group.Element(YouTube.Atom("uploaded", "yt")).Value.Split('T');
        string[] strArray2 = strArray1[0].Split('-');
        string[] strArray3 = strArray1[1].Split(':');
        strArray3[2] = strArray3[2].Replace("Z", "0");
        string s = "0";
        if (strArray3[2].Contains("."))
          s = strArray3[2].Split('.')[0];
        try
        {
          this.time = new DateTime(int.Parse(strArray2[0]), int.Parse(strArray2[1]), int.Parse(strArray2[2]), int.Parse(strArray3[0]), int.Parse(strArray3[1]), (int) double.Parse(s));
        }
        catch
        {
        }
      }
      catch
      {
      }
      XElement xelement11 = xml.Element(YouTube.Atom("rating", "yt"));
      if (xelement11 != null)
      {
        this.Likes = int.Parse(xelement11.Attribute((XName) "numLikes").Value);
        this.Dislikes = int.Parse(xelement11.Attribute((XName) "numDislikes").Value);
      }
      try
      {
        foreach (XElement xelement12 in xml.Elements(YouTube.Atom("accessControl", "yt")).ToArray<XElement>())
        {
          if (xelement12.Attribute((XName) "action").Value == "comment" && xelement12.Attribute((XName) "permission").Value == "denied")
            this.Comment = false;
        }
      }
      catch
      {
      }
      XElement xelement13 = xml.Element(YouTube.Atom("id"));
      if (xelement13 == null)
        return;
      this.PlaylistID = ((IEnumerable<string>) xelement13.Value.Split(':')).Last<string>();
    }

    private void init2(XElement group) => this.getThumb(group);

    private void getThumb(XElement group)
    {
      this.ThumbLinks = new Dictionary<string, string>();
      foreach (XElement xelement in group.Elements(YouTube.Atom("thumbnail", "media")).ToArray<XElement>())
      {
        string key = "default";
        XAttribute xattribute1 = xelement.Attribute((XName) "width");
        if (xattribute1 != null)
        {
          string str = xattribute1.Value;
          key = "default";
          if (str == "480")
            key = "hqdefault";
        }
        else
        {
          XAttribute xattribute2 = xelement.Attribute(YouTube.Atom("name", "yt"));
          if (xattribute2 != null)
            key = xattribute2.Value;
        }
        XAttribute xattribute3 = xelement.Attribute((XName) "url");
        string str1 = xattribute3 == null ? this.ThumbLink : xattribute3.Value;
        if (!this.ThumbLinks.ContainsKey(key))
          this.ThumbLinks.Add(key, str1);
      }
      if (this.ThumbLinks.ContainsKey("hqdefault"))
        this.ThumbLink = this.ThumbLinks["hqdefault"];
      else if (this.ThumbLinks.ContainsKey("mqdefault"))
      {
        this.ThumbLink = this.ThumbLinks["mqdefault"];
      }
      else
      {
        if (!this.ThumbLinks.ContainsKey("default"))
          return;
        this.ThumbLink = this.ThumbLinks["default"];
      }
    }

    public Uri GetThumb(ThumbnailQuality q)
    {
      string thumbLink = this.ThumbLink;
      int num;
      switch (q)
      {
        case ThumbnailQuality.Low:
          num = 0;
          break;
        case ThumbnailQuality.Med:
          num = 1;
          break;
        case ThumbnailQuality.High:
          num = 2;
          break;
        case ThumbnailQuality.SD:
          num = 3;
          break;
        case ThumbnailQuality.MaxRes:
          num = 4;
          break;
        default:
          num = 2;
          break;
      }
      for (int index = num; index >= 0 && index < YouTubeEntry.thumbs.Length; --index)
      {
        if (this.ThumbLinks.ContainsKey(YouTubeEntry.thumbs[index]))
          return new Uri(this.ThumbLinks[YouTubeEntry.thumbs[index]], UriKind.Absolute);
      }
      return new Uri(thumbLink, UriKind.Absolute);
    }

    public override string ToString() => this.ID + ", " + this.Title;

    protected override EntryClient<YouTubeEntry> GetRefreshClientOverride() => (EntryClient<YouTubeEntry>) new YouTubeEntryClient();
  }
}
