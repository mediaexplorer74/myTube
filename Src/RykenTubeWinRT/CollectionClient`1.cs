// Decompiled with JetBrains decompiler
// Type: RykenTube.CollectionClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class CollectionClient<T> : YouTubeClient<T> where T : ClientData<T>
  {
    private List<T> collection;

    public CollectionClient(IEnumerable<T> items, int howMany)
      : base(howMany)
    {
      this.collection = items.ToList<T>();
      this.RefreshInternally = false;
      this.version = -1;
    }

    public override bool CanLoadPage(int page) => page * this.howMany < this.collection.Count;

    protected override async Task<T[]> GetFeedOverride(int page)
    {
      CollectionClient<T> collectionClient = this;
      int num1 = page * collectionClient.howMany;
      int num2 = num1 + collectionClient.howMany;
      if (num2 > collectionClient.collection.Count)
        num2 = collectionClient.collection.Count;
      int length = num2 - num1;
      T[] feedOverride = new T[length];
      for (int index = 0; index < length; ++index)
        feedOverride[index] = collectionClient.collection[num1 + index];
      return feedOverride;
    }

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();

    protected override EntryClient<T> GetRefreshClientOverride() => throw new NotImplementedException();

    protected override Task<T[]> ParseOther(string s) => throw new NotImplementedException();

    protected override Task<T[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override Task<T[]> ParseV3(JObject jo) => throw new NotImplementedException();
  }
}
