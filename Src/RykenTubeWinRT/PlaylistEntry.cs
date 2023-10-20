// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistEntry
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
  public class PlaylistEntry : ClientData<PlaylistEntry>
  {
    public bool Bookmarked;
    private string title = "Playlist";
    private bool exists = true;
    public int Count;
    private string thumblink = "http://s.ytimg.com/yts/img/no_thumbnail-vfl4t3-4R.jpg";
    private Uri thumbnail;
    private string author = "author unknown";
    public string AuthorID = "";
    private PrivacyStatus privacy;
    private string description = "";
    private JToken json;
    private XElement xml;

    public override bool NeedsRefresh { get; set; }

    public bool PlaylistOfSignedInUser => YouTube.UserInfo != null && this.AuthorID == YouTube.UserInfo.ID;

    public string Title
    {
      get => this.title;
      set
      {
        if (!(value != this.title))
          return;
        this.title = value;
        this.opc(nameof (Title));
      }
    }

    public bool Exists
    {
      get => this.exists;
      set
      {
        if (value == this.exists)
          return;
        this.exists = value;
        this.opc(nameof (Exists));
      }
    }

    public override string ID { get; set; }

    public string ThumbLink
    {
      get => this.thumblink;
      set
      {
        if (!(value != this.thumblink))
          return;
        this.thumblink = value;
        this.opc(nameof (ThumbLink));
      }
    }

    public Uri Thumbnail
    {
      get
      {
        if (this.thumbnail == (Uri) null && this.thumblink != null)
          this.thumbnail = new Uri(this.thumblink, UriKind.Absolute);
        return this.thumbnail;
      }
    }

    public string Author
    {
      get => this.author;
      set
      {
        if (!(value != this.author))
          return;
        this.author = value;
        this.opc(nameof (Author));
      }
    }

    public bool Private => this.Privacy == PrivacyStatus.Private;

    public PrivacyStatus Privacy
    {
      get => this.privacy;
      set => this.privacy = value;
    }

    public string Description
    {
      get => this.description;
      set
      {
        if (!(value != this.description))
          return;
        this.description = value;
        this.opc(nameof (Description));
      }
    }

    public string OriginalString
    {
      get
      {
        if (this.json != null)
          return this.json.ToString();
        return this.xml != null ? this.xml.ToString() : (string) null;
      }
    }

    public PlaylistEntry()
    {
    }

    public PlaylistEntry(string origString)
    {
      try
      {
        this.SetValuesFromJson(JToken.Parse(origString));
        return;
      }
      catch
      {
      }
      try
      {
        this.SetValuesFromXML(XElement.Parse(origString));
      }
      catch
      {
      }
    }

    public PlaylistEntry(JToken json) => this.SetValuesFromJson(json);

    public override void SetValuesFromJson(JToken json)
    {
      this.json = json;
      JToken jtoken1 = json[(object) "snippet"];
      JToken jtoken2 = json[(object) "status"];
      if (json[(object) "id"] is JValue)
        this.ID = (string) json[(object) "id"];
      else if (json[(object) "id"] is JObject && json[(object) "id"][(object) "playlistId"] is JValue)
        this.ID = (string) json[(object) "id"][(object) "playlistId"];
      if (jtoken1 != null)
      {
        if (jtoken1[(object) "title"] is JValue)
          this.title = (string) jtoken1[(object) "title"];
        if (jtoken1[(object) "channelId"] is JValue)
          this.AuthorID = (string) jtoken1[(object) "channelId"];
        if (jtoken1[(object) "channelTitle"] is JValue)
          this.author = (string) jtoken1[(object) "channelTitle"];
        if (jtoken1[(object) "description"] is JValue)
          this.description = (string) jtoken1[(object) "description"];
        JToken thumbs = jtoken1[(object) "thumbnails"];
        if (thumbs != null)
        {
          this.tryGetThumb(thumbs, "default");
          this.tryGetThumb(thumbs, "medium");
          this.tryGetThumb(thumbs, "high");
        }
      }
      if (jtoken2 == null || !(jtoken2[(object) "privacyStatus"] is JValue))
        return;
      switch ((string) jtoken2[(object) "privacyStatus"])
      {
        case "private":
          this.privacy = PrivacyStatus.Private;
          break;
        case "public":
          this.privacy = PrivacyStatus.Public;
          break;
        case "unlisted":
          this.privacy = PrivacyStatus.Unlisted;
          break;
      }
    }

    public PlaylistEntry(XElement xml) => this.SetValuesFromXML(xml);

    public override void SetValuesFromXML(XElement xml)
    {
      this.xml = xml;
      try
      {
        this.Title = xml.Element(YouTube.Atom("title")).Value;
      }
      catch
      {
      }
      try
      {
        this.ID = xml.Element(YouTube.Atom("playlistId", "yt")).Value;
      }
      catch
      {
      }
      try
      {
        XElement xelement = xml.Element(YouTube.Atom("feedLink", "gd"));
        if (xelement != null)
        {
          XAttribute xattribute = xelement.Attribute((XName) "countHint");
          if (xattribute != null)
            this.Count = int.Parse(xattribute.Value);
        }
      }
      catch
      {
      }
      try
      {
        XElement xelement = xml.Element(YouTube.Atom("content"));
        if (xelement != null)
          this.Description = xelement.Value;
      }
      catch
      {
      }
      try
      {
        this.Author = xml.Element(YouTube.Atom("author")).Element(YouTube.Atom("name")).Value;
        this.AuthorID = ((IEnumerable<string>) xml.Element(YouTube.Atom("author")).Element(YouTube.Atom("uri")).Value.Split('/')).Last<string>();
      }
      catch
      {
      }
      try
      {
        foreach (XElement element in xml.Element(YouTube.Atom("group", "media")).Elements())
        {
          try
          {
            this.thumblink = element.Attribute((XName) "url").Value;
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
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
        string str = (string) thumb[(object) "url"];
        if (str == null)
          return;
        this.thumblink = str;
      }
      catch
      {
      }
    }

    protected override EntryClient<PlaylistEntry> GetRefreshClientOverride() => (EntryClient<PlaylistEntry>) new PlaylistEntryClient();
  }
}
