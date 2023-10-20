// Decompiled with JetBrains decompiler
// Type: RykenTube.VideoListClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RykenTube
{
  public class VideoListClient : YouTubeClient<YouTubeEntry>
  {
    private YouTubeEntryClient entryClient = new YouTubeEntryClient(3);

    [DefaultValue(YouTubeFeedType.Videos)]
    public YouTubeFeedType FeedType { get; protected set; }

    public VideoListClient(string baseAddress, int howMany)
      : base(howMany)
    {
      this.RefreshInternally = true;
      this.AddParts(Part.Snippet, Part.Statistics, Part.ContentDetails);
      this.Fields = "entry(id,title,yt:statistics(@viewCount),yt:rating,yt:accessControl,media:group(yt:videoid,media:thumbnail(@yt:name,@url),media:credit,media:description,yt:duration,yt:uploaded))";
      this.BaseAddress.BaseAddress = baseAddress;
    }

    protected override EntryClient<YouTubeEntry> GetRefreshClientOverride() => (EntryClient<YouTubeEntry>) this.entryClient;

    protected override async Task<YouTubeEntry[]> ParseV2(XDocument xdoc)
    {
      VideoListClient videoListClient = this;
      xdoc.Element(YouTube.Atom("feed"));
      List<XElement> list = xdoc.Descendants(YouTube.Atom("entry")).ToList<XElement>();
      YouTubeEntry[] v2 = new YouTubeEntry[list.Count];
      for (int index1 = 0; index1 < list.Count; ++index1)
      {
        YouTubeEntry[] youTubeEntryArray = v2;
        int index2 = index1;
        YouTubeEntry youTubeEntry = new YouTubeEntry(list[index1]);
        youTubeEntry.NeedsRefresh = videoListClient.NeedsRefresh;
        youTubeEntryArray[index2] = youTubeEntry;
      }
      return v2;
    }

    protected override async Task<YouTubeEntry[]> ParseV3(JObject jo)
    {
      VideoListClient videoListClient = this;
      JArray jarray = (JArray) jo["items"];
      List<YouTubeEntry> youTubeEntryList = new List<YouTubeEntry>();
      for (int index = 0; index < jarray.Count; ++index)
      {
        JToken json = jarray[index];
        if (videoListClient.JsonFilter == null || videoListClient.JsonFilter(json))
        {
          YouTubeEntry youTubeEntry1 = new YouTubeEntry(json);
          youTubeEntry1.NeedsRefresh = videoListClient.NeedsRefresh;
          YouTubeEntry youTubeEntry2 = youTubeEntry1;
          youTubeEntryList.Add(youTubeEntry2);
        }
      }
      return youTubeEntryList.ToArray();
    }

    protected override async Task<YouTubeEntry[]> InternalRefresh(
      YouTubeEntry[] items,
      EntryClient<YouTubeEntry> refreshClient)
    {
      List<List<string>> refreshIds = new List<List<string>>();
      foreach (YouTubeEntry youTubeEntry in items)
      {
        if (refreshIds.Count == 0 || refreshIds[refreshIds.Count - 1].Count >= 40)
          refreshIds.Add(new List<string>());
        refreshIds[refreshIds.Count - 1].Add(youTubeEntry.ID);
      }
      if (refreshIds.Count > 0)
      {
        List<Task<JObject>> tasks = new List<Task<JObject>>();
        foreach (List<string> stringList in refreshIds)
          tasks.Add(refreshClient.GetBatchedJson(stringList.ToArray()));
        try
        {
          int lasti = 0;
          int lasto = 0;
          while (refreshIds.Count > 0)
          {
            List<string> stringList = refreshIds[0];
            refreshIds.RemoveAt(0);
            Task<JObject> task = tasks[0];
            tasks.RemoveAt(0);
            JArray jarray = (JArray) (await task)[nameof (items)];
            for (int index1 = 0; index1 < jarray.Count; ++index1)
            {
              string str = jarray[index1].GetValue<string>((string) null, "id");
              for (int index2 = lasto; index2 < items.Length; ++index2)
              {
                if (str == items[index2].ID)
                {
                  items[index2].SetValuesFromJson(jarray[index1]);
                  items[index2].NeedsRefresh = false;
                  lasto = index2 + 1;
                  break;
                }
              }
              lasti = index1 + 1;
            }
          }
          return items;
        }
        catch
        {
        }
        tasks = (List<Task<JObject>>) null;
      }
      return items;
    }

    protected override Task<YouTubeEntry[]> ParseOther(string s) => throw new NotImplementedException();

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();
  }
}
