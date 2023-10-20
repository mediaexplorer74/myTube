// Decompiled with JetBrains decompiler
// Type: RykenTube.SearchUsersClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

namespace RykenTube
{
  public class SearchUsersClient : UserListClient
  {
    public string SearchTerm = "";

    public SearchUsersClient(int howMany)
      : base(howMany)
    {
      this.version = 3;
      if (this.version == 2)
      {
        this.BaseAddress.BaseAddress = "http://gdata.youtube.com/feeds/api/channels";
        this.BaseAddress.SetValue("v", (object) 2);
      }
      else
      {
        if (this.version != 3)
          return;
        this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/search";
        this.ClearParts(new Part[1]);
        this.BaseAddress["type"] = "channel";
        this.UseAccessToken = false;
        this.NeedsRefresh = true;
      }
    }

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.BaseAddress.SetValue("q", (object) this.SearchTerm);
    }
  }
}
