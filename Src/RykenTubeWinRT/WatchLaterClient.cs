// Decompiled with JetBrains decompiler
// Type: RykenTube.WatchLaterClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube
{
  public class WatchLaterClient : PlaylistClient
  {
    public WatchLaterClient(int howMany)
      : base(howMany)
    {
      this.IsPlaylist = true;
      this.version = 3;
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      if (YouTube.UserInfo == null || this.version != 3)
        return;
      if (YouTube.UserInfo.WatchLaterPlaylist != null)
        this.BaseAddress["playlistId"] = this.id = YouTube.UserInfo.WatchLaterPlaylist;
      else
        this.BaseAddress["playlistId"] = this.id = "WL";
    }
  }
}
