// Decompiled with JetBrains decompiler
// Type: RykenTube.ChannelSection
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;

namespace RykenTube
{
  public class ChannelSection : ClientData<ChannelSection>
  {
    private string id;
    private ChannelSectionType type;
    private string title;

    public override bool NeedsRefresh { get; set; }

    public PlaylistEntry Playlist { get; set; }

    public ChannelSectionType Type => this.type;

    public override string ID
    {
      get => this.id;
      set => this.id = value;
    }

    public string ChannelID { get; set; }

    public string Title
    {
      get => this.title;
      set
      {
        if (!(this.title != value))
          return;
        this.title = value;
        this.opc(nameof (Title));
      }
    }

    public string[] PlaylistIds { get; set; } = new string[0];

    public string[] ChannelIds { get; set; } = new string[0];

    public ChannelSection(JToken json) => this.SetValuesFromJson(json);

    public override void SetValuesFromJson(JToken json)
    {
      JToken json1 = json[(object) "snippet"];
      JToken jtoken1 = json[(object) "contentDetails"];
      JToken jtoken2 = json[(object) "id"];
      if (jtoken2 != null)
        this.id = (string) jtoken2;
      if (json1 != null)
      {
        this.Title = json1.GetValue<string>("", "title");
        this.ChannelID = json1.GetValue<string>((string) null, "channelId");
        switch ((string) json1[(object) "type"])
        {
          case null:
            break;
          case "singlePlaylist":
            this.type = ChannelSectionType.SinglePlaylist;
            break;
          case "multiplePlaylists":
            this.type = ChannelSectionType.MultiplePlaylists;
            break;
          case "multipleChannels":
            this.type = ChannelSectionType.MultipleChannels;
            break;
          case "recentUploads":
            this.type = ChannelSectionType.RecentUploads;
            break;
          default:
            this.type = ChannelSectionType.Unknown;
            break;
        }
      }
      if (jtoken1 == null)
        return;
      if (jtoken1[(object) "playlists"] is JArray)
        this.PlaylistIds = jtoken1[(object) "playlists"].ToObject<string[]>();
      if (!(jtoken1[(object) "channels"] is JArray))
        return;
      this.ChannelIds = jtoken1[(object) "channels"].ToObject<string[]>();
    }

    public override void SetValuesFromXML(XElement xml) => throw new NotImplementedException();

    protected override EntryClient<ChannelSection> GetRefreshClientOverride() => throw new NotImplementedException();
  }
}
