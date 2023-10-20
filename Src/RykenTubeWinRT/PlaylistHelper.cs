// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistHelper
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RykenTube
{
  public class PlaylistHelper : PlaylistHelperBase
  {
    private bool shuffled;
    private PlaylistRepeatMode repeatMode = PlaylistRepeatMode.All;
    private new string id;
    private string currentId;
    private const string Tag = "PlaylistHelper";
    private int[] shuffleIndices;
    private bool loaded;
    private int lastAddIndex = -1;
    private int indexWhenLastAdded = -1;

    public PlaylistHelper(YouTubeClient<YouTubeEntry> client)
      : base(client)
    {
    }

    public override IList<YouTubeEntry> CreateList(int capacity) => (IList<YouTubeEntry>) new ObservableCollection<YouTubeEntry>();

    public override void SetIndexBasedOnID(string ID)
    {
      for (int index = 0; index < this.Entries.Count; ++index)
      {
        if (this.Entries[index].ID == ID)
        {
          this.index = index;
          this.id = (string) null;
          this.currentId = this.Entries[index].ID;
          return;
        }
      }
      this.id = ID;
    }

    public override async Task Load()
    {
      PlaylistHelper playlistHelper = this;
      if (playlistHelper.loaded)
        return;
      playlistHelper.loaded = true;
      YouTube.Write((object) nameof (PlaylistHelper), (object) "Loading playlist");
      int page = 0;
      bool error = false;
      playlistHelper.Client?.SetHowMany(50);
      int iterations = 0;
      while (playlistHelper.Client != null && playlistHelper.Entries.Count < 300 && playlistHelper.Client.CanLoadPage(page) && iterations < 50)
      {
        ++iterations;
        try
        {
          YouTubeEntry[] feed = await playlistHelper.Client.GetFeed(page);
          ++page;
          if (feed.Length != 0)
          {
            foreach (YouTubeEntry youTubeEntry in feed)
            {
              if (playlistHelper.id != null && youTubeEntry.ID == playlistHelper.id)
              {
                playlistHelper.id = (string) null;
                playlistHelper.currentId = playlistHelper.id;
                playlistHelper.index = playlistHelper.Entries.Count;
              }
              playlistHelper.Entries.Add(youTubeEntry);
            }
          }
          else
            break;
        }
        catch (Exception ex)
        {
          error = true;
          YouTube.Write((object) nameof (PlaylistHelper), (object) "Error loading playlist");
          break;
        }
      }
      playlistHelper.shuffle();
      if (error)
        return;
      YouTube.Write((object) nameof (PlaylistHelper), (object) ("Loading playlist completed, loaded " + (object) playlistHelper.Entries.Count + " videos"));
    }

    private int[] shuffle()
    {
      if (this.Entries.Count <= 0)
      {
        this.shuffleIndices = new int[0];
        return this.shuffleIndices;
      }
      Random random = new Random(this.ShuffleSeed);
      this.shuffleIndices = new int[this.Entries.Count];
      List<int> intList = new List<int>();
      for (int index = 0; index < this.Entries.Count; ++index)
        intList.Add(index);
      int index1 = 0;
      while (intList.Count > 0)
      {
        int index2 = random.Next(0, intList.Count);
        int num = intList[index2];
        this.shuffleIndices[index1] = num;
        intList.RemoveAt(index2);
        ++index1;
      }
      return this.shuffleIndices;
    }

    public override YouTubeEntry GetEntry(int offset)
    {
      YouTube.Write((object) nameof (PlaylistHelper), (object) ("Getting playlist helper video at offset " + (object) offset));
      if (this.Entries.Count == 0)
        return (YouTubeEntry) null;
      if (this.Shuffle != this.shuffled && this.shuffleIndices != null)
      {
        if (this.Shuffle && this.shuffleIndices != null)
        {
          for (int index = 0; index < this.shuffleIndices.Length; ++index)
          {
            if (this.shuffleIndices[index] == this.index)
            {
              this.index = index;
              break;
            }
          }
        }
        else
          this.index = this.shuffleIndices[this.index];
        this.shuffled = this.Shuffle;
      }
      this.index += offset;
      if (this.index < 0)
        this.index = this.Entries.Count + this.index;
      if (this.index >= this.Entries.Count)
        this.index -= this.Entries.Count;
      YouTubeEntry entry = !this.Shuffle || this.shuffleIndices == null ? this.Entries[this.index] : this.Entries[this.shuffleIndices[this.index]];
      this.currentId = entry.ID;
      return entry;
    }

    public override IList<YouTubeEntry> GetTrueListOfEntries()
    {
      if (!this.Shuffle || this.shuffleIndices == null || this.shuffleIndices.Length != this.Entries.Count)
        return base.GetTrueListOfEntries();
      List<YouTubeEntry> trueListOfEntries = new List<YouTubeEntry>();
      foreach (int shuffleIndex in this.shuffleIndices)
        trueListOfEntries.Add(this.Entries[shuffleIndex]);
      return (IList<YouTubeEntry>) trueListOfEntries;
    }

    public override void AddEntry(YouTubeEntry entry)
    {
      if (this.HasEntryID(entry.ID))
        this.RemoveEntryID(entry.ID);
      this.SetIndexBasedOnID(this.currentId);
      if (this.index != this.indexWhenLastAdded)
        this.lastAddIndex = this.index;
      if (this.Entries.Count == 0)
      {
        this.Entries.Add(entry);
        this.lastAddIndex = 0;
      }
      else
      {
        int index = this.lastAddIndex + 1;
        if (index <= this.index)
          index = this.index + 1;
        if (index > this.Entries.Count)
          index = this.Entries.Count;
        if (index < 0)
          index = 0;
        this.Entries.Insert(index, entry);
        this.lastAddIndex = index;
      }
      this.indexWhenLastAdded = this.index;
      this.shuffle();
    }

    public override void RemoveEntryID(string ID)
    {
      base.RemoveEntryID(ID);
      --this.lastAddIndex;
      if (this.Entries.Count > 0)
        this.id = this.Entries[this.index].ID;
      this.shuffle();
    }

    public override YouTubeEntry GetEntryWithoutChangingIndex(int offset)
    {
      YouTube.Write((object) nameof (PlaylistHelper), (object) ("Getting playlist helper video at offset " + (object) offset));
      if (this.Entries.Count == 0)
        return (YouTubeEntry) null;
      int index1 = this.index;
      if (this.Shuffle != this.shuffled && this.shuffleIndices != null)
      {
        if (this.Shuffle && this.shuffleIndices != null)
        {
          for (int index2 = 0; index2 < this.shuffleIndices.Length; ++index2)
          {
            if (this.shuffleIndices[index2] == index1)
            {
              index1 = index2;
              break;
            }
          }
        }
        else
          index1 = this.shuffleIndices[index1];
        this.shuffled = this.Shuffle;
      }
      int index3 = index1 + offset;
      if (index3 < 0)
        index3 = this.Entries.Count + index3;
      if (index3 >= this.Entries.Count)
        index3 -= this.Entries.Count;
      return this.Shuffle && this.shuffleIndices != null ? this.Entries[this.shuffleIndices[index3]] : this.Entries[index3];
    }
  }
}
