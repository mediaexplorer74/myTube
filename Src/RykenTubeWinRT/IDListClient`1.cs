// Decompiled with JetBrains decompiler
// Type: RykenTube.IDListClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class IDListClient<T> : YouTubeClient<T> where T : ClientData<T>
  {
    private EntryClient<T> client;
    private string[] ids;

    public IDListClient(EntryClient<T> client, int howMany, params string[] ids)
      : base(howMany)
    {
      this.client = client;
      this.ids = ids;
      this.version = -1;
    }

    protected override string GetNextPageTokenOther(string s) => "";

    protected override async Task<T[]> GetFeedOverride(int page)
    {
      IDListClient<T> idListClient = this;
      int num1 = page * idListClient.howMany;
      int num2 = num1 + idListClient.howMany;
      if (num2 > idListClient.ids.Length)
        num2 = idListClient.ids.Length;
      int length = num2 - num1;
      if (length <= 0)
        return new T[0];
      string[] strArray = new string[length];
      for (int index = 0; index < length; ++index)
        strArray[index] = idListClient.ids[num1 + index];
      return await idListClient.client.GetBatchedInfo(strArray);
    }

    public override bool CanLoadPage(int page) => page * this.howMany < this.ids.Length;

    protected override EntryClient<T> GetRefreshClientOverride() => this.client;

    protected override Task<T[]> ParseOther(string s) => throw new NotImplementedException();

    protected override Task<T[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override Task<T[]> ParseV3(JObject jo) => throw new NotImplementedException();
  }
}
