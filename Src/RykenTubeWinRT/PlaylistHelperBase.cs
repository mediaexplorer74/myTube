// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistHelperBase
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RykenTube
{
  public abstract class PlaylistHelperBase
  {
    public const int MaxVideos = 300;
    private PlaylistRepeatMode repeatMode = PlaylistRepeatMode.All;
    protected int index;
    protected string id;
    private const string Tag = "PlaylistHelper";

    public bool Shuffle { get; set; }

    public int ShuffleSeed { get; set; } = 120;

    public PlaylistRepeatMode RepeatMode
    {
      get => this.repeatMode;
      set => this.repeatMode = value;
    }

    public IList<YouTubeEntry> Entries { get; private set; }

    public YouTubeClient<YouTubeEntry> Client { get; set; }

    public PlaylistHelperBase(YouTubeClient<YouTubeEntry> client)
    {
      if (client != null)
        this.Client = client.GetDuplicate();
      this.Entries = this.CreateList(400);
    }

    public virtual IList<YouTubeEntry> CreateList(int capacity) => (IList<YouTubeEntry>) new List<YouTubeEntry>(capacity);

    public virtual IList<YouTubeEntry> GetTrueListOfEntries() => this.Entries;

    public virtual bool HasEntry(YouTubeEntry entry) => this.Entries.Contains(entry);

    public virtual bool HasEntryID(string ID) => this.Entries.Where<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == ID)).Count<YouTubeEntry>() > 0;

    public virtual void RemoveEntryID(string ID)
    {
      YouTubeEntry youTubeEntry = this.Entries.Where<YouTubeEntry>((Func<YouTubeEntry, bool>) (e => e.ID == ID)).FirstOrDefault<YouTubeEntry>();
      if (youTubeEntry != null)
      {
        int index = this.Entries.IndexOf(youTubeEntry);
        this.Entries.RemoveAt(index);
        if (index <= this.index)
          --this.index;
      }
      if (this.index >= 0)
        return;
      this.index = 0;
    }

    public abstract void SetIndexBasedOnID(string ID);

    public abstract Task Load();

    public abstract YouTubeEntry GetEntry(int offset);

    public abstract YouTubeEntry GetEntryWithoutChangingIndex(int offset);

    public abstract void AddEntry(YouTubeEntry entry);
  }
}
