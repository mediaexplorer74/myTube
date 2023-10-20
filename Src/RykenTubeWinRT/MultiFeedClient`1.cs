// Decompiled with JetBrains decompiler
// Type: RykenTube.MultiFeedClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class MultiFeedClient<T> : YouTubeClient<T> where T : ClientData<T>
  {
    private YouTubeClient<T>[] clients;

    public MultiFeedClient(params YouTubeClient<T>[] clients)
      : base(15)
    {
      this.clients = clients;
      if (clients.Length == 0)
        throw new InvalidOperationException("Provide at least one client");
      this.version = -1;
    }

    private ClientIndexPageIndex getIndexFromPage(int page)
    {
      ClientIndexPageIndex indexFromPage = new ClientIndexPageIndex()
      {
        ClientIndex = 0,
        PageIndex = 0
      };
      if (page == 0)
        return indexFromPage;
      int page1 = -1;
      int index1 = 0;
      for (int index2 = -1; index2 < page; ++index2)
      {
        ++page1;
        if (!this.clients[index1].CanLoadPage(page1) && index1 < this.clients.Length - 1)
        {
          page1 = 0;
          ++index1;
        }
      }
      indexFromPage.PageIndex = page1;
      indexFromPage.ClientIndex = index1;
      return indexFromPage;
    }

    public override bool CanLoadPage(int page)
    {
      ClientIndexPageIndex indexFromPage = this.getIndexFromPage(page);
      return this.clients[indexFromPage.ClientIndex].CanLoadPage(indexFromPage.PageIndex);
    }

    protected override Task<T[]> GetFeedOverride(int page)
    {
      ClientIndexPageIndex indexFromPage = this.getIndexFromPage(page);
      return this.clients[indexFromPage.ClientIndex].CanLoadPage(indexFromPage.PageIndex) ? this.clients[indexFromPage.ClientIndex].GetFeed(indexFromPage.PageIndex) : base.GetFeedOverride(page);
    }

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();

    protected override EntryClient<T> GetRefreshClientOverride() => throw new NotImplementedException();

    protected override async Task<T[]> ParseOther(string s) => new T[0];

    protected override Task<T[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override Task<T[]> ParseV3(JObject jo) => throw new NotImplementedException();
  }
}
