// Decompiled with JetBrains decompiler
// Type: RykenTube.OfflinePlaylistClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class OfflinePlaylistClient : VideoListClient
  {
    private XElement xml;
    private List<YouTubeEntry> entries;
    private int page;

    public string EncodedXML => WebUtility.UrlEncode(this.xml.ToString());

    public List<YouTubeEntry> Entries => this.entries;

    public OfflinePlaylistClient(string UrlEncodedXML, int howMany)
      : base("", howMany)
    {
      this.xml = XElement.Parse(WebUtility.UrlDecode(UrlEncodedXML));
      XElement[] array = this.xml.Elements().ToArray<XElement>();
      this.entries = new List<YouTubeEntry>();
      for (int index = 0; index < array.Length; ++index)
        this.entries.Add(new YouTubeEntry(array[index].Value));
    }

    public override TypeConstructor GetTypeConstructor() => new TypeConstructor(this.GetType(), new object[2]
    {
      (object) this.EncodedXML,
      (object) this.howMany
    });

    protected override void SetBaseAddress(int page)
    {
      base.SetBaseAddress(page);
      this.page = page;
    }

    protected override async Task<string> GetResultString(int page) => "";

    protected override async Task<YouTubeEntry[]> StringDownloaded(string result)
    {
      OfflinePlaylistClient offlinePlaylistClient = this;
      int num1 = offlinePlaylistClient.page * offlinePlaylistClient.howMany;
      if (num1 >= offlinePlaylistClient.entries.Count)
        num1 = offlinePlaylistClient.entries.Count;
      int num2 = num1 + offlinePlaylistClient.howMany;
      if (num2 >= offlinePlaylistClient.entries.Count)
        num2 = offlinePlaylistClient.entries.Count;
      YouTubeEntry[] youTubeEntryArray = new YouTubeEntry[num2 - num1];
      int index1 = 0;
      for (int index2 = num1; index2 < num2; ++index2)
      {
        try
        {
          youTubeEntryArray[index1] = offlinePlaylistClient.entries[index2];
          ++index1;
        }
        catch
        {
        }
      }
      return youTubeEntryArray;
    }
  }
}
