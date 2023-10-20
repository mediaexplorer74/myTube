// Decompiled with JetBrains decompiler
// Type: RykenTube.UserInfo
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Xml.Linq;

namespace RykenTube
{
  public class UserInfo : ClientData<UserInfo>
  {
    private string username = "";
    private string userdisplayname = "";
    private string title = "";
    private string googlePlusId;
    private string subscriptionId;
    private string thumblink = "";
    private Uri thumbUri = new Uri("https://s.ytimg.com/yts/img/avatar_48-vfllY0UTT.png");
    private Uri bannerUri;
    private Uri bannerUriHD;
    private int upl;
    private int views;
    private int newItemCount;
    private int totalItemCount;
    private string favoritesPlaylist;
    private string uploadsPlaylist;
    private string watchLaterPlaylist;
    private string description = "";
    public string Location = "";
    private int subscribers;
    public int Playlists;
    public bool Suspended;
    public bool Incomplete;
    private XElement xml;
    private YouTubeActivity newActivityType = YouTubeActivity.All;

    public override bool NeedsRefresh { get; set; }

    public string UserDisplayName
    {
      get => this.userdisplayname;
      set => this.userdisplayname = value;
    }

    public string UserName
    {
      get => this.username;
      set => this.username = value;
    }

    public UserInfo Instance => this;

    public string Title
    {
      get => this.title;
      set => this.title = value;
    }

    public string GooglePlusId
    {
      get => this.googlePlusId;
      set => this.googlePlusId = value;
    }

    public string SubscriptionID
    {
      get => this.subscriptionId;
      set => this.subscriptionId = value;
    }

    public string ThumbLink
    {
      get => this.thumblink;
      set => this.thumblink = value;
    }

    public Uri ThumbUri
    {
      get => this.thumbUri;
      set
      {
        this.thumbUri = value;
        this.OnPropertyChanged(nameof (ThumbUri));
      }
    }

    public Uri BannerUri
    {
      get => this.bannerUri;
      set
      {
        this.bannerUri = value;
        this.OnPropertyChanged(nameof (BannerUri));
      }
    }

    public string BannerUriHDString
    {
      get => this.BannerUriHD != (Uri) null ? this.BannerUriHD.ToString() : (string) null;
      set => this.BannerUriHD = new Uri(value);
    }

    public Uri BannerUriHD
    {
      get => this.bannerUriHD;
      set
      {
        this.bannerUriHD = value;
        this.OnPropertyChanged(nameof (BannerUriHD));
      }
    }

    public int Uploads
    {
      get => this.upl;
      set => this.upl = value;
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

    public int Subscribers
    {
      get => this.subscribers;
      set
      {
        this.subscribers = value;
        this.OnPropertyChanged(nameof (Subscribers));
      }
    }

    public int Views
    {
      get => this.views;
      set
      {
        this.views = value;
        this.OnPropertyChanged(nameof (Views));
      }
    }

    public int NewItemCount
    {
      get => this.newItemCount;
      set
      {
        this.newItemCount = value;
        this.OnPropertyChanged(nameof (NewItemCount));
      }
    }

    public int TotalItemCount
    {
      get => this.totalItemCount;
      set
      {
        this.totalItemCount = value;
        this.OnPropertyChanged(nameof (TotalItemCount));
      }
    }

    public string FavoritesPlaylist => this.favoritesPlaylist ?? "FL" + UserInfo.RemoveUCFromID(this.ID);

    public string UploadsPlaylist => this.uploadsPlaylist ?? "UL" + UserInfo.RemoveUCFromID(this.ID);

    public string WatchLaterPlaylist
    {
      get => this.watchLaterPlaylist ?? "WL" + UserInfo.RemoveUCFromID(this.ID);
      set => this.watchLaterPlaylist = value;
    }

    public override string ID { get; set; }

    public YouTubeActivity NewActivityType => this.newActivityType;

    public XElement XML => this.xml;

    public UserInfo(XElement entry) => this.SetValuesFromXML(entry);

    public UserInfo()
    {
    }

    public UserInfo(JToken json) => this.SetValuesFromJson(json);

    public override void SetValuesFromXML(XElement entry)
    {
      this.xml = entry;
      XElement xelement1 = entry.Element(YouTube.Atom("author"));
      try
      {
        XElement xelement2 = entry.Element(YouTube.Atom("userId", "yt"));
        if (xelement2 != null)
        {
          this.UserName = xelement2.Value;
        }
        else
        {
          XElement xelement3 = xelement1.Element(YouTube.Atom("userId", "yt"));
          if (xelement3 != null)
            this.UserName = xelement3.Value;
        }
      }
      catch
      {
      }
      try
      {
        this.UserDisplayName = xelement1.Element(YouTube.Atom("name")).Value;
      }
      catch
      {
        this.UserDisplayName = this.UserName;
      }
      try
      {
        this.Title = entry.Element(YouTube.Atom("title")).Value;
        this.Description = entry.Element(YouTube.Atom("summary")).Value;
      }
      catch
      {
      }
      if (entry.Element(YouTube.Atom("incomplete", "yt")) != null)
        this.Incomplete = true;
      try
      {
        this.ThumbLink = entry.Element(YouTube.Atom("thumbnail", "media")).Attribute((XName) "url").Value;
      }
      catch
      {
      }
      try
      {
        this.ThumbUri = new Uri(this.ThumbLink, UriKind.Absolute);
      }
      catch
      {
      }
      try
      {
        XElement xelement4 = entry.Element(YouTube.Atom("location", "yt"));
        if (xelement4 != null)
          this.Location = xelement4.Value ?? "US";
      }
      catch
      {
        this.Location = "NA";
      }
      XElement xelement5 = entry.Element(YouTube.Atom("statistics", "yt"));
      try
      {
        if (xelement5 != null)
        {
          try
          {
            this.Subscribers = int.Parse(xelement5.Attribute((XName) "subscriberCount").Value);
          }
          catch
          {
          }
        }
        else
        {
          XElement xelement6 = entry.Element(YouTube.Atom("channelStatistics", "yt"));
          if (xelement6 != null)
            this.Subscribers = int.Parse(xelement6.Attribute((XName) "subscriberCount").Value);
        }
      }
      catch
      {
      }
      XElement[] xelementArray = (XElement[]) null;
      try
      {
        xelementArray = entry.Elements(YouTube.Atom("feedLink", "gd")).ToArray<XElement>();
      }
      catch
      {
      }
      try
      {
        this.ID = entry.Element(YouTube.Atom("channelId", "yt")).Value;
      }
      catch
      {
        try
        {
          string str = entry.Element(YouTube.Atom("id")).Value;
          string[] strArray;
          if (str.Contains("tag"))
            strArray = str.Split(':');
          else
            strArray = str.Split('/');
          this.ID = strArray[strArray.Length - 1];
        }
        catch
        {
          this.ID = this.UserName;
        }
      }
      try
      {
        if (xelementArray != null)
        {
          foreach (XElement xelement7 in xelementArray)
          {
            if (xelement7.Attribute((XName) "href").Value.Contains("uplo"))
              this.Uploads = int.Parse(xelement7.Attribute((XName) "countHint").Value);
          }
        }
      }
      catch
      {
      }
      if (this.Uploads != 0)
        return;
      try
      {
        this.Uploads = int.Parse(entry.Element(YouTube.Atom("countHint", "yt")).Value);
      }
      catch
      {
      }
    }

    public override void SetValuesFromJson(JToken json)
    {
      JToken jtoken1 = json[(object) "snippet"];
      JToken jtoken2 = json[(object) "statistics"];
      JToken jtoken3 = json[(object) "brandingSettings"];
      JToken jtoken4 = json[(object) "contentDetails"];
      try
      {
        if (json[(object) "id"] != null)
        {
          if (json[(object) "id"] is JValue)
            this.ID = (string) json[(object) "id"];
          else if (json[(object) "id"] is JObject)
          {
            if (json[(object) "id"][(object) "channelId"] is JValue)
              this.ID = (string) json[(object) "id"][(object) "channelId"];
          }
        }
      }
      catch
      {
      }
      if (jtoken1 != null)
      {
        this.userdisplayname = this.username = this.title = (string) jtoken1[(object) "title"];
        this.Description = (string) jtoken1[(object) "description"];
        if (jtoken1[(object) "channelId"] is JValue)
          this.ID = (string) jtoken1[(object) "channelId"];
        JToken thumbs = jtoken1[(object) "thumbnails"];
        if (thumbs != null)
        {
          try
          {
            this.tryGetThumb(thumbs, "default");
            this.tryGetThumb(thumbs, "medium");
            this.tryGetThumb(thumbs, "high");
          }
          catch
          {
          }
        }
      }
      if (jtoken2 != null)
      {
        try
        {
          this.Subscribers = (int) jtoken2[(object) "subscriberCount"];
        }
        catch
        {
        }
        try
        {
          this.Views = (int) jtoken2[(object) "viewCount"];
        }
        catch
        {
        }
        try
        {
          this.Uploads = (int) jtoken2[(object) "videoCount"];
        }
        catch
        {
        }
      }
      if (jtoken3 != null)
      {
        try
        {
          this.BannerUri = new Uri((string) jtoken3[(object) "image"][(object) "bannerImageUrl"], UriKind.Absolute);
        }
        catch
        {
        }
        try
        {
          this.BannerUriHD = new Uri((string) jtoken3[(object) "image"][(object) "bannerTabletHdImageUrl"], UriKind.Absolute);
        }
        catch
        {
          this.BannerUriHD = this.bannerUri;
        }
      }
      if (jtoken4 == null)
        return;
      this.googlePlusId = (string) jtoken4[(object) "googlePlusUserId"];
    }

    private void tryGetThumb(JToken thumbs, string name)
    {
      try
      {
        if (thumbs[(object) name] == null)
          return;
        JToken thumb = thumbs[(object) name];
        if (thumb[(object) "url"] == null)
          return;
        string uriString = (string) thumb[(object) "url"];
        if (uriString == null)
          return;
        if (uriString.StartsWith("//"))
          uriString = "https" + uriString;
        this.thumbUri = new Uri(uriString, UriKind.Absolute);
      }
      catch
      {
      }
    }

    public static string AddUCToID(string id) => "UC" + UserInfo.RemoveUCFromID(id);

    public static string RemoveUCFromID(string id)
    {
      if (id == null && YouTube.UserInfo != null)
        id = YouTube.UserInfo.ID;
      return id.StartsWith("UC") ? id.Substring(2) : id;
    }

    public static UserInfo FromSubscriptionData(JToken json)
    {
      UserInfo userInfo = new UserInfo();
      JToken jtoken1 = json[(object) "id"];
      if (jtoken1 != null)
        userInfo.subscriptionId = (string) jtoken1;
      JToken jtoken2 = json[(object) "snippet"];
      JToken jtoken3 = json[(object) "contentDetails"];
      if (jtoken2 != null)
      {
        userInfo.title = (string) jtoken2[(object) "title"];
        userInfo.description = (string) jtoken2[(object) "description"];
        userInfo.userdisplayname = (string) jtoken2[(object) "title"];
        if (jtoken2[(object) "resourceId"] != null)
          userInfo.ID = (string) jtoken2[(object) "resourceId"][(object) "channelId"];
        JToken thumbs = jtoken2[(object) "thumbnails"];
        if (thumbs != null)
        {
          userInfo.tryGetThumb(thumbs, "default");
          userInfo.tryGetThumb(thumbs, "high");
        }
      }
      if (jtoken3 != null)
      {
        if (jtoken3[(object) "newItemCount"] != null)
          userInfo.newItemCount = (int) jtoken3[(object) "newItemCount"];
        if (jtoken3[(object) "totalItemCount"] != null)
          userInfo.totalItemCount = (int) jtoken3[(object) "totalItemCount"];
        if (jtoken3[(object) "activityType"] != null && (string) jtoken3[(object) "activityType"] == "uploads")
          userInfo.newActivityType = YouTubeActivity.Upload;
      }
      return userInfo;
    }

    public override string ToString() => "User: " + this.UserDisplayName;

    protected override EntryClient<UserInfo> GetRefreshClientOverride() => (EntryClient<UserInfo>) new UserInfoClient();
  }
}
