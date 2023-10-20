// Decompiled with JetBrains decompiler
// Type: RykenTube.ChannelSectionListClient
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
  public class ChannelSectionListClient : YouTubeClient<ChannelSection>
  {
    private string channelId;
    private ChannelSection[] currentSections;
    private string resultString;
    private int currentPage;

    public ChannelSectionListClient(string channelId, int howMany)
      : base(howMany)
    {
      this.version = 3;
      this.BaseAddress.BaseAddress = "https://www.googleapis.com/youtube/v3/channelSections";
      this.channelId = channelId;
      this.ClearParts(Part.Snippet, Part.ContentDetails, Part.Targeting);
      this.UseCache("channelSections", new GroupCacheInfo()
      {
        MaxAge = TimeSpan.FromDays(5.0),
        MaxItems = 100
      });
      this.JsonFilter = (Func<JToken, bool>) (json =>
      {
        if (YouTube.Region == (RegionInfo) null || string.IsNullOrWhiteSpace(YouTube.Region.CountryCode))
          return false;
        string lowerInvariant = YouTube.Region.CountryCode.Trim().ToLowerInvariant();
        JToken jtoken = json[(object) "targeting"];
        if (jtoken == null)
          return true;
        if (jtoken[(object) "languages"] is JArray jarray3 && jarray3.Count != 0 || !(jtoken[(object) "regions"] is JArray jarray4))
          return false;
        foreach (object obj in jarray4)
        {
          if (obj.ToString().Trim().ToLowerInvariant() == lowerInvariant)
            return true;
        }
        return false;
      });
    }

    protected override string GetCacheName() => this.channelId;

    public override bool CanLoadPage(int page) => page == 0 || this.currentSections == null || page * this.howMany < this.currentSections.Length;

    protected override EntryClient<ChannelSection> GetRefreshClientOverride() => (EntryClient<ChannelSection>) null;

    protected override async Task<string> GetResultString(int page)
    {
      if (page == 0)
        this.resultString = await base.GetResultString(page);
      return this.resultString;
    }

    protected override void SetBaseAddress(int page)
    {
      this.currentPage = page;
      if (this.channelId != null)
        this.BaseAddress.SetValue("channelId", (object) this.channelId);
      else
        this.BaseAddress.RemoveValue("channelId");
      if (YouTube.Region != RegionInfo.Global)
        this.BaseAddress["regionCode"] = YouTube.Region.CountryCode;
      else
        this.BaseAddress.RemoveValue("regionCode");
      base.SetBaseAddress(page);
    }

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();

    protected override Task<ChannelSection[]> InternalRefresh(
      ChannelSection[] items,
      EntryClient<ChannelSection> refreshClient)
    {
      throw new NotImplementedException();
    }

    protected override Task<ChannelSection[]> ParseOther(string s) => throw new NotImplementedException();

    protected override Task<ChannelSection[]> ParseV2(XDocument xdoc) => throw new NotImplementedException();

    protected override async Task<ChannelSection[]> ParseV3(JObject jo)
    {
      ChannelSectionListClient sectionListClient = this;
      if (sectionListClient.currentPage == 0 || sectionListClient.currentSections == null)
      {
        JArray jarray = jo["items"] as JArray;
        List<ChannelSection> channelSectionList = new List<ChannelSection>();
        for (int index = 0; index < jarray.Count; ++index)
        {
          ChannelSection channelSection = new ChannelSection(jarray[index]);
          channelSectionList.Add(channelSection);
        }
        sectionListClient.currentSections = channelSectionList.ToArray();
      }
      int sourceIndex = sectionListClient.currentPage * sectionListClient.howMany;
      int num1 = sourceIndex + sectionListClient.howMany;
      if (sourceIndex > sectionListClient.currentSections.Length)
        sourceIndex = sectionListClient.currentSections.Length;
      int num2 = sourceIndex;
      int length = num1 - num2;
      if (length <= 0)
        return new ChannelSection[0];
      ChannelSection[] array = new ChannelSection[length];
      Array.Copy((Array) sectionListClient.currentSections, sourceIndex, (Array) array, 0, length);
      List<string> stringList = new List<string>();
      ChannelSection[] playlistSections = ((IEnumerable<ChannelSection>) sectionListClient.currentSections).Where<ChannelSection>((Func<ChannelSection, bool>) (ss => ss.Type == ChannelSectionType.SinglePlaylist && ss.PlaylistIds != null && ss.PlaylistIds.Length != 0)).ToArray<ChannelSection>();
      foreach (ChannelSection channelSection in playlistSections)
        stringList.Add(channelSection.PlaylistIds[0]);
      try
      {
        PlaylistEntry[] batchedInfo = await new PlaylistEntryClient().GetBatchedInfo(stringList.ToArray());
        for (int index = 0; index < playlistSections.Length; ++index)
        {
          ChannelSection channelSection = playlistSections[index];
          string plId = channelSection.PlaylistIds[0];
          PlaylistEntry playlistEntry = ((IEnumerable<PlaylistEntry>) batchedInfo).Where<PlaylistEntry>((Func<PlaylistEntry, bool>) (p => p.ID == plId)).FirstOrDefault<PlaylistEntry>();
          if (playlistEntry != null)
          {
            channelSection.Playlist = playlistEntry;
            if (string.IsNullOrWhiteSpace(channelSection.Title))
              channelSection.Title = playlistEntry.Title;
          }
        }
      }
      catch
      {
      }
      return array;
    }
  }
}
