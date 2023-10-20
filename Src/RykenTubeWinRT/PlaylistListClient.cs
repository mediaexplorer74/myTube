// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistListClient
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
  public class PlaylistListClient : YouTubeClient<PlaylistEntry>
  {
    protected PlaylistEntryClient internalClient = new PlaylistEntryClient();
    protected bool SignedInUser;

    public PlaylistListClient(int howMany)
      : base(howMany)
    {
      this.RefreshInternally = false;
      this.UseAccessToken = true;
      this.ClearParts(Part.Snippet, Part.Status);
    }

    protected override EntryClient<PlaylistEntry> GetRefreshClientOverride() => (EntryClient<PlaylistEntry>) this.internalClient;

    protected override async Task<PlaylistEntry[]> InternalRefresh(
      PlaylistEntry[] items,
      EntryClient<PlaylistEntry> refreshClient)
    {
      List<string> ids = new List<string>();
      List<PlaylistEntry> entries = new List<PlaylistEntry>();
      PlaylistEntry[] playlistEntryArray = items;
      for (int index = 0; index < playlistEntryArray.Length; ++index)
      {
        ids.Add(playlistEntryArray[index].ID);
        if (ids.Count >= 40)
        {
          PlaylistEntry[] batchedInfo = await refreshClient.GetBatchedInfo(ids.ToArray());
          ids.Clear();
          foreach (PlaylistEntry playlistEntry in batchedInfo)
            entries.Add(playlistEntry);
        }
      }
      playlistEntryArray = (PlaylistEntry[]) null;
      if (ids.Count > 0)
      {
        PlaylistEntry[] batchedInfo = await this.internalClient.GetBatchedInfo(ids.ToArray());
        ids.Clear();
        foreach (PlaylistEntry playlistEntry in batchedInfo)
          entries.Add(playlistEntry);
      }
      return entries.ToArray();
    }

    protected override async Task<PlaylistEntry[]> ParseV2(XDocument xdoc)
    {
      XElement[] array = xdoc.Element(YouTube.Atom("feed")).Elements(YouTube.Atom("entry")).ToArray<XElement>();
      PlaylistEntry[] v2 = new PlaylistEntry[array.Length];
      for (int index = 0; index < array.Length; ++index)
        v2[index] = new PlaylistEntry(array[index]);
      return v2;
    }

    protected override async Task<PlaylistEntry[]> ParseV3(JObject jo)
    {
      JArray jarray = (JArray) jo["items"];
      PlaylistEntry[] v3 = new PlaylistEntry[jarray.Count];
      for (int index = 0; index < v3.Length; ++index)
        v3[index] = new PlaylistEntry(jarray[index]);
      return v3;
    }

    protected override Task<PlaylistEntry[]> ParseOther(string s) => throw new NotImplementedException();

    protected override string GetNextPageTokenOther(string s) => throw new NotImplementedException();
  }
}
