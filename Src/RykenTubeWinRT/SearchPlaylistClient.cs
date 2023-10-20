// Decompiled with JetBrains decompiler
// Type: RykenTube.SearchPlaylistClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube
{
  public class SearchPlaylistClient : PlaylistListClient
  {
    public string SearchTerm = "";
    public string ChannelID;

    public SearchPlaylistClient(int howMany)
      : base(howMany)
    {
      this.version = 3;
      if (this.version == 2)
      {
        this.BaseAddress.BaseAddress = "https://gdata.youtube.com/feeds/api/playlists/snippets";
        this.BaseAddress.SetValue("v", (object) "2");
      }
      else if (this.version == 3)
      {
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/search";
        this.BaseAddress["type"] = "playlist";
        this.UseAccessToken = false;
      }
      this.ClearParts(new Part[1]);
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.BaseAddress.SetValue("q", (object) this.SearchTerm);
      if (this.version != 3)
        return;
      if (this.ChannelID != null)
        this.BaseAddress.SetValue("channelId", (object) this.ChannelID);
      else
        this.BaseAddress.RemoveValue("channelId");
    }
  }
}
