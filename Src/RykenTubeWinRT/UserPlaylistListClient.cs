// Decompiled with JetBrains decompiler
// Type: RykenTube.UserPlaylistListClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;

namespace RykenTube
{
  public class UserPlaylistListClient : PlaylistListClient
  {
    private string channelId;

    public UserPlaylistListClient(int howMany)
      : base(howMany)
    {
      this.DefaultCacheGroupName = "userPlaylists";
      this.CacheRequiresSameAccount = true;
      this.SignedInUser = true;
      this.version = 3;
      if (this.version == 2)
      {
        this.BaseAddress.BaseAddress = "https://gdata.youtube.com/feeds/api/users/default/playlists";
      }
      else
      {
        if (this.version != 3)
          return;
        this.Mine = true;
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/playlists";
      }
    }

    public UserPlaylistListClient(string channel, int howMany)
      : base(howMany)
    {
      this.DefaultCacheGroupName = "userPlaylists";
      this.UseCache("userPlaylists", new GroupCacheInfo()
      {
        MaxAge = TimeSpan.FromDays(4.0),
        MaxItems = 100
      });
      this.SignedInUser = false;
      this.version = 3;
      this.channelId = channel;
      if (this.version == 2)
      {
        this.BaseAddress.BaseAddress = "https://gdata.youtube.com/feeds/api/users/" + channel + "/playlists";
        this.BaseAddress.SetValue("v", (object) "2");
      }
      else
      {
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/playlists";
        this.BaseAddress[nameof (channelId)] = channel;
        this.Mine = false;
      }
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.BaseAddress.SetValue("randtime", (object) DateTime.Now);
    }

    protected override string GetCacheName() => this.SignedInUser ? "user" : this.channelId;
  }
}
