// Decompiled with JetBrains decompiler
// Type: myTube.OfflinePlaylistsCollection
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace myTube
{
  public class OfflinePlaylistsCollection
  {
    private static StorageFolder folder = ApplicationData.Current.LocalFolder;
    private const string FilePath = "OfflinePlaylists.xml";
    private XElement xml;
    private bool changed;
    private OfflinePlaylist[] playlists;

    public OfflinePlaylist[] Playlists
    {
      get
      {
        if (this.playlists == null || this.changed)
        {
          XElement[] array = this.xml.Elements((XName) "OfflinePlaylist").ToArray<XElement>();
          this.playlists = new OfflinePlaylist[array.Length];
          for (int index = 0; index < this.playlists.Length; ++index)
            this.playlists[index] = new OfflinePlaylist(array[index]);
        }
        return this.playlists;
      }
    }

    public OfflinePlaylistsCollection() => this.xml = new XElement((XName) nameof (OfflinePlaylistsCollection));

    public OfflinePlaylistsCollection(XElement Xml) => this.xml = Xml;

    public void RemoveOfflinePlaylist(string playlistID)
    {
      OfflinePlaylist offlinePlaylist = this.GetOfflinePlaylist(playlistID);
      if (offlinePlaylist == null)
        return;
      offlinePlaylist.XML.Remove();
      this.changed = true;
    }

    public string[] SetOfflinePlaylistVideos(
      PlaylistEntry playlist,
      bool clearVideos,
      params string[] videos)
    {
      OfflinePlaylist offlinePlaylist = (OfflinePlaylist) null;
      foreach (OfflinePlaylist playlist1 in this.Playlists)
      {
        if (playlist1.Playlist != null && playlist1.Playlist.ID == playlist.ID)
        {
          offlinePlaylist = playlist1;
          break;
        }
      }
      if (offlinePlaylist == null)
      {
        offlinePlaylist = new OfflinePlaylist(playlist, videos);
        this.xml.Add((object) offlinePlaylist.XML);
        this.changed = true;
      }
      if (clearVideos)
        offlinePlaylist.ClearVideos();
      return offlinePlaylist.AddVideos(videos);
    }

    public OfflinePlaylist GetOfflinePlaylist(string playlistID)
    {
      foreach (OfflinePlaylist playlist in this.Playlists)
      {
        if (playlist.Playlist.ID == playlistID)
          return playlist;
      }
      return (OfflinePlaylist) null;
    }

    public async Task Save(string filePath = "OfflinePlaylists.xml")
    {
      try
      {
        await FileIO.WriteTextAsync((IStorageFile) await OfflinePlaylistsCollection.folder.CreateFileAsync(filePath, (CreationCollisionOption) 1), this.xml.ToString());
      }
      catch
      {
      }
    }

    public static async Task<OfflinePlaylistsCollection> Load(string filePath = "OfflinePlaylists.xml")
    {
      try
      {
        return new OfflinePlaylistsCollection(XElement.Parse(await FileIO.ReadTextAsync((IStorageFile) await OfflinePlaylistsCollection.folder.GetFileAsync(filePath))));
      }
      catch
      {
        return new OfflinePlaylistsCollection();
      }
    }
  }
}
