// Decompiled with JetBrains decompiler
// Type: RykenTube.FeedPageClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public abstract class FeedPageClient<T> : YouTubeClient<T> where T : ClientData<T>
  {
    private List<T> items;
    private int currentPage;
    private string currentString;

    protected FeedPageClient(int howMany)
      : base(howMany)
    {
      this.items = new List<T>();
      this.version = -1;
      this.UseAPIKey = false;
    }

    public override bool CanLoadPage(int page) => page == 0 || this.items.Count >= page * this.howMany;

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.currentPage = page;
    }

    protected override async Task<string> GetResultString(int page)
    {
      if (page == 0 || this.currentString == null)
      {
        this.items.Clear();
        this.currentString = await base.GetResultString(page);
      }
      return this.currentString;
    }

    protected virtual bool ShouldIncludeURLInfo(YouTubeURLInfo info) => true;

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();

    protected override async Task<T[]> ParseOther(string s)
    {
      FeedPageClient<T> feedPageClient = this;
      if (feedPageClient.items.Count == 0)
      {
        foreach (T obj in feedPageClient.GetItems(s))
          feedPageClient.items.Add(obj);
      }
      int num = feedPageClient.currentPage * feedPageClient.howMany;
      if (num >= feedPageClient.items.Count)
        return new T[0];
      int length = Math.Min(feedPageClient.items.Count - num, feedPageClient.howMany);
      T[] other = new T[length];
      for (int index = 0; index < length; ++index)
        other[index] = feedPageClient.items[num + index];
      return other;
    }

    protected override Task<T[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override async Task<T[]> ParseV3(JObject jo) => throw new NotImplementedException();

    protected abstract IEnumerable<T> GetItems(string s);
  }
}
