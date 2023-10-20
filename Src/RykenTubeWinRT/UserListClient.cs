// Decompiled with JetBrains decompiler
// Type: RykenTube.UserListClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class UserListClient : YouTubeClient<UserInfo>
  {
    private UserInfoClient refreshClient = new UserInfoClient();

    public UserListClient(int howMany)
      : base(howMany)
    {
    }

    protected override EntryClient<UserInfo> GetRefreshClientOverride() => (EntryClient<UserInfo>) this.refreshClient;

    protected override async Task<UserInfo[]> ParseV2(XDocument xdoc)
    {
      XElement[] array = xdoc.Descendants(YouTube.Atom("entry")).ToArray<XElement>();
      UserInfo[] v2 = new UserInfo[array.Length];
      for (int index = 0; index < array.Length; ++index)
        v2[index] = new UserInfo(array[index]);
      return v2;
    }

    protected override async Task<UserInfo[]> ParseV3(JObject jo)
    {
      JArray jarray = (JArray) jo["items"];
      UserInfo[] v3 = new UserInfo[jarray.Count];
      for (int index = 0; index < v3.Length; ++index)
        v3[index] = new UserInfo(jarray[index]);
      return v3;
    }

    protected override Task<UserInfo[]> ParseOther(string s) => throw new NotImplementedException();

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();
  }
}
