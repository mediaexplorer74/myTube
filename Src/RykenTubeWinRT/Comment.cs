// Decompiled with JetBrains decompiler
// Type: RykenTube.Comment
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class Comment : ClientData<Comment>
  {
    private string content = "There was an error loading this comment. Maybe tell the developer?";
    private string author = "nobody...";
    private int likes = -1;
    private int replyCount = -1;
    private bool topLevelComment;
    public string CommentThreadId;
    public bool IsReply;
    public Uri ReplyLink;
    public string Channel = "";
    private DateTime time;
    public string VideoID;
    private Uri channelThumb;
    private string thumbLink;
    private bool repliesLoaded;

    public List<Comment> Replies { get; set; }

    public string Content
    {
      get => this.content;
      set
      {
        this.content = value;
        this.OnPropertyChanged(nameof (Content));
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

    public int Likes
    {
      get => this.likes < 0 ? 0 : this.likes;
      set
      {
        if (this.likes == value)
          return;
        this.likes = value;
        this.OnPropertyChanged(nameof (Likes));
        this.OnPropertyChanged("TrueLikes");
      }
    }

    public int TrueLikes
    {
      get => this.likes;
      set
      {
        if (this.likes == value)
          return;
        this.likes = value;
        this.OnPropertyChanged("Likes");
        this.OnPropertyChanged(nameof (TrueLikes));
      }
    }

    public int ReplyCount => this.Replies.Count > this.replyCount ? this.Replies.Count : this.replyCount;

    public bool TopLevelComment => this.topLevelComment;

    public override string ID { get; set; }

    public DateTime Time
    {
      get => this.time;
      set
      {
        this.time = value;
        this.OnPropertyChanged(nameof (Time));
      }
    }

    public override bool NeedsRefresh { get; set; }

    public string ThumbLink
    {
      get => this.thumbLink;
      set => this.thumbLink = value;
    }

    public Rating ViewerRating { get; set; } = Rating.None;

    public bool CanRate { get; set; } = true;

    public Uri ChannelThumb
    {
      get => this.thumbLink != null ? new Uri(this.thumbLink) : this.channelThumb;
      set
      {
        this.channelThumb = value;
        this.OnPropertyChanged(nameof (ChannelThumb));
      }
    }

    public Comment() => this.Replies = new List<Comment>();

    public Comment(XElement xml)
    {
    }

    public Comment(JToken json) => this.SetValuesFromJson(json);

    public override void SetValuesFromJson(JToken json)
    {
      this.Replies = new List<Comment>();
      JToken jtoken = json[(object) "snippet"];
      if ((string) json[(object) "kind"] == "youtube#commentThread")
      {
        string str = (string) json[(object) "id"];
        if (str != null)
          this.CommentThreadId = str;
        if (jtoken != null && jtoken[(object) "totalReplyCount"] != null)
          this.replyCount = (int) jtoken[(object) "totalReplyCount"];
      }
      if (jtoken != null)
      {
        if (jtoken[(object) "topLevelComment"] != null)
        {
          this.topLevelComment = true;
          this.IsReply = false;
          this.InitFromJson(jtoken[(object) "topLevelComment"]);
        }
        else
        {
          this.IsReply = true;
          this.InitFromJson(json);
        }
      }
      JObject jobject = (JObject) json[(object) "replies"];
      if (jobject == null)
        return;
      JArray jarray = (JArray) jobject["comments"];
      if (jarray != null)
      {
        for (int index = 0; index < jarray.Count; ++index)
          this.Replies.Add(new Comment(jarray[index])
          {
            IsReply = true,
            CommentThreadId = this.CommentThreadId
          });
      }
      this.Replies.Sort((Comparison<Comment>) ((c1, c2) => c1.Time.CompareTo(c2.Time)));
    }

    public void InitFromJson(JToken json)
    {
      JToken json1 = json[(object) "snippet"];
      if (json[(object) "id"] is JValue)
        this.ID = (string) json[(object) "id"];
      if (json1 == null)
        return;
      if (json1[(object) "textDisplay"] is JValue)
      {
        this.content = (string) json1[(object) "textDisplay"];
        this.content = this.content.Replace(" \n", " ");
      }
      if (json1[(object) "videoId"] != null)
        this.VideoID = (string) json1[(object) "videoId"];
      if (json1[(object) "authorDisplayName"] is JValue)
        this.author = (string) json1[(object) "authorDisplayName"];
      if (json1[(object) "authorChannelId"] is JObject && json1[(object) "authorChannelId"][(object) "value"] is JValue)
        this.Channel = (string) json1[(object) "authorChannelId"][(object) "value"];
      if (json1[(object) "publishedAt"] is JValue)
      {
        try
        {
          this.time = (DateTime) json1[(object) "publishedAt"];
        }
        catch
        {
        }
      }
      if (json1[(object) "likeCount"] is JValue)
      {
        try
        {
          this.likes = (int) json1[(object) "likeCount"];
        }
        catch
        {
        }
      }
      if (json1[(object) "authorProfileImageUrl"] is JValue)
      {
        try
        {
          this.channelThumb = new Uri((string) json1[(object) "authorProfileImageUrl"], UriKind.Absolute);
        }
        catch
        {
        }
      }
      this.CanRate = json1.GetValue<bool>(true, "canRate");
      switch (json1.GetValue<string>("none", "viewerRating"))
      {
        case "none":
          this.ViewerRating = Rating.None;
          break;
        case "dislike":
          this.ViewerRating = Rating.Dislike;
          break;
        case "like":
          this.ViewerRating = Rating.Like;
          break;
      }
    }

    public override void SetValuesFromXML(XElement xml)
    {
      this.Replies = new List<Comment>();
      try
      {
        this.ID = ((IEnumerable<string>) xml.Element(YouTube.Atom("id")).Value.Split(':')).Last<string>();
      }
      catch
      {
      }
      try
      {
        this.Content = xml.Element(YouTube.Atom("content")).Value.Replace("\n", "");
      }
      catch
      {
      }
      try
      {
        this.Author = xml.Element(YouTube.Atom("author")).Element(YouTube.Atom("name")).Value;
        this.Channel = xml.Element(YouTube.Atom("channelId", "yt")).Value;
      }
      catch
      {
      }
      try
      {
        foreach (XElement element in xml.Elements(YouTube.Atom("link")))
        {
          try
          {
            if (element.Attribute((XName) "rel").Value.Contains("reply"))
            {
              this.ReplyLink = new Uri(element.Attribute((XName) "href").Value, UriKind.Absolute);
              this.IsReply = true;
              break;
            }
          }
          catch
          {
          }
        }
      }
      catch
      {
      }
      string[] strArray1 = xml.Element(YouTube.Atom("published")).Value.Split('T');
      string[] strArray2 = strArray1[0].Split('-');
      string[] strArray3 = strArray1[1].Split(':');
      strArray3[2] = strArray3[2].Replace("Z", "0");
      string s = "0";
      if (strArray3[2].Contains("."))
        s = strArray3[2].Split('.')[0];
      try
      {
        this.time = new DateTime(int.Parse(strArray2[0]), int.Parse(strArray2[1]), int.Parse(strArray2[2]), int.Parse(strArray3[0]), int.Parse(strArray3[1]), (int) double.Parse(s));
        this.time += TimeZoneInfo.Local.BaseUtcOffset;
        if (!TimeZoneInfo.Local.SupportsDaylightSavingTime)
          return;
        this.time += TimeSpan.FromHours(1.0);
      }
      catch
      {
      }
    }

    protected override EntryClient<Comment> GetRefreshClientOverride() => throw new NotImplementedException();

    public async Task LoadReplies()
    {
      Comment comment1 = this;
      if (comment1.Replies.Count >= comment1.replyCount || comment1.Replies.Count >= 20 || comment1.repliesLoaded || !comment1.TopLevelComment)
        return;
      comment1.repliesLoaded = true;
      CommentClient repliesClient = CommentClient.GetRepliesClient(comment1.ID, 50);
      try
      {
        Comment[] feed = await repliesClient.GetFeed(0);
        comment1.Replies.Clear();
        foreach (Comment comment2 in feed)
        {
          comment2.CommentThreadId = comment1.CommentThreadId;
          comment1.Replies.Add(comment2);
        }
        comment1.Replies.Sort((Comparison<Comment>) ((x, y) => x.Time.CompareTo(y.Time)));
      }
      catch
      {
      }
    }
  }
}
