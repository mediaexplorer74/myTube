// Decompiled with JetBrains decompiler
// Type: RykenTube.ActivityClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class ActivityClient : YouTubeClient<Activity>
  {
    public ActivityClient(int num)
      : base(num)
    {
      this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/activities";
      this.NeedsRefresh = false;
      this.RefreshInternally = false;
    }

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();

    protected override EntryClient<Activity> GetRefreshClientOverride() => (EntryClient<Activity>) null;

    protected override Task<Activity[]> InternalRefresh(
      Activity[] items,
      EntryClient<Activity> refreshClient)
    {
      throw new NotImplementedException();
    }

    protected override Task<Activity[]> ParseOther(string s) => throw new NotImplementedException();

    protected override Task<Activity[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override Task<Activity[]> ParseV3(JObject jo) => throw new NotImplementedException();
  }
}
