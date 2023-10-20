// Decompiled with JetBrains decompiler
// Type: RykenTube.FunctionalClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class FunctionalClient<T> : YouTubeClient<T> where T : ClientData<T>
  {
    private Func<int, Task<T[]>> func;

    public FunctionalClient(Func<int, Task<T[]>> func)
      : base(10)
    {
      this.func = func;
    }

    protected override async Task<T[]> GetFeedOverride(int page) => await this.func(page);

    public override bool CanLoadPage(int page) => true;

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();

    protected override EntryClient<T> GetRefreshClientOverride() => throw new NotImplementedException();

    protected override Task<T[]> InternalRefresh(T[] items, EntryClient<T> refreshClient) => throw new NotImplementedException();

    protected override Task<T[]> ParseOther(string s) => throw new NotImplementedException();

    protected override Task<T[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override Task<T[]> ParseV3(JObject jo) => throw new NotImplementedException();
  }
}
