// Decompiled with JetBrains decompiler
// Type: myTube.OfflinePlaylist
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using UriTester;

namespace myTube
{
  public class OfflinePlaylist
  {
    private static TransferManager transferManager;
    private XElement xml;
    private PlaylistEntry playlist;
    private List<string> videos;

    public XElement XML
    {
      get
      {
        if (this.playlist != null)
          this.xml.GetElement("Playlist").Value = this.playlist.OriginalString;
        this.xml.GetElement("Videos").Value = string.Join(",", (IEnumerable<string>) this.videos);
        return this.xml;
      }
    }

    public PlaylistEntry Playlist
    {
      get
      {
        if (this.playlist == null)
        {
          XElement element;
          if ((element = this.xml.GetElement(nameof (Playlist))) != null)
            this.playlist = new PlaylistEntry(element.Value);
        }
        return this.playlist;
      }
      set
      {
        this.playlist = value;
        this.xml.GetElement(nameof (Playlist)).Value = this.playlist.OriginalString;
      }
    }

    public string[] Videos
    {
      get
      {
        this.createVideosList();
        return this.videos.ToArray();
      }
    }

    private void createVideosList()
    {
      if (this.videos != null)
        return;
      this.videos = ((IEnumerable<string>) this.xml.GetElement("Videos").Value.Split(',')).ToList<string>();
    }

    public OfflinePlaylist() => this.xml = new XElement((XName) nameof (OfflinePlaylist));

    public OfflinePlaylist(PlaylistEntry playlist, params string[] videos)
      : this()
    {
      this.playlist = playlist;
      this.videos = ((IEnumerable<string>) videos).ToList<string>();
    }

    public OfflinePlaylist(XElement Xml) => this.xml = Xml;

    public void ClearVideos()
    {
      this.videos.Clear();
      this.xml.GetElement("Videos").Value = "";
    }

    public string[] AddVideos(params string[] videoIDs)
    {
      this.createVideosList();
      List<string> stringList = new List<string>();
      foreach (string videoId in videoIDs)
      {
        if (!((IEnumerable<string>) this.Videos).Contains<string>(videoId))
        {
          this.videos.Add(videoId);
          stringList.Add(videoId);
        }
      }
      this.xml.GetElement("Videos").Value = this.videos.Count <= 0 ? "" : string.Join(",", (IEnumerable<string>) this.videos);
      return stringList.ToArray();
    }

    public async Task<YouTubeEntry[]> GetAllEntries()
    {
      if (OfflinePlaylist.transferManager == null)
        OfflinePlaylist.transferManager = await TransferManager.Load();
      List<YouTubeEntry> entries = new List<YouTubeEntry>();
      string[] strArray = this.Videos;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string id = strArray[index];
        if (await OfflinePlaylist.transferManager.GetTransferState(id) != TransferManager.State.None)
        {
          List<YouTubeEntry> youTubeEntryList = entries;
          youTubeEntryList.Add(new YouTubeEntry(await OfflinePlaylist.transferManager.GetVideoString(id)));
          youTubeEntryList = (List<YouTubeEntry>) null;
        }
        id = (string) null;
      }
      strArray = (string[]) null;
      return entries.ToArray();
    }

    public async Task<YouTubeEntry[]> GetEntries(int page)
    {
      if (OfflinePlaylist.transferManager == null)
        OfflinePlaylist.transferManager = await TransferManager.Load();
      int maxCount = 10;
      int totalCount = 0;
      List<YouTubeEntry> entries = new List<YouTubeEntry>();
      string[] strArray = this.Videos;
      for (int index = 0; index < strArray.Length; ++index)
      {
        string id = strArray[index];
        if (await OfflinePlaylist.transferManager.GetTransferState(id) != TransferManager.State.None)
        {
          if (totalCount >= page * maxCount)
          {
            List<YouTubeEntry> youTubeEntryList = entries;
            youTubeEntryList.Add(new YouTubeEntry(await OfflinePlaylist.transferManager.GetVideoString(id)));
            youTubeEntryList = (List<YouTubeEntry>) null;
            if (entries.Count >= maxCount)
              break;
          }
          ++totalCount;
        }
        id = (string) null;
      }
      strArray = (string[]) null;
      return entries.ToArray();
    }
  }
}
