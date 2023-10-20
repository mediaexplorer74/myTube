// Decompiled with JetBrains decompiler
// Type: myTube.TimeBookmarksManager
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace myTube
{
  public class TimeBookmarksManager
  {
    private const string FileName = "TimeBookmarks.txt";
    private const int MaxVideos = 20;
    public readonly TimeSpan ShortestVideo = TimeSpan.FromMinutes(5.0);
    public readonly TimeSpan OnlyRememberAfter = TimeSpan.FromSeconds(30.0);
    private List<TimeBookmark> bookmarks;
    private bool changesMade;
    private StorageFolder folder = ApplicationData.Current.RoamingFolder;

    public event TypedEventHandler<TimeBookmarksManager, TimeBookmark> Changed
    {
      add
      {
        TypedEventHandler<TimeBookmarksManager, TimeBookmark> typedEventHandler1 = this.Changed;
        TypedEventHandler<TimeBookmarksManager, TimeBookmark> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<TimeBookmarksManager, TimeBookmark>>(ref this.Changed, (TypedEventHandler<TimeBookmarksManager, TimeBookmark>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<TimeBookmarksManager, TimeBookmark> typedEventHandler1 = this.Changed;
        TypedEventHandler<TimeBookmarksManager, TimeBookmark> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<TimeBookmarksManager, TimeBookmark>>(ref this.Changed, (TypedEventHandler<TimeBookmarksManager, TimeBookmark>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public TimeBookmarksManager() => this.bookmarks = new List<TimeBookmark>();

    public TimeBookmark Get(string videoID)
    {
      foreach (TimeBookmark bookmark in this.bookmarks)
      {
        if (bookmark.ID == videoID)
          return bookmark;
      }
      return (TimeBookmark) null;
    }

    public TimeBookmark SetBookmark(YouTubeEntry entry, TimeSpan time)
    {
      if (!(entry.Duration < this.ShortestVideo) && !(entry.Duration - time < TimeSpan.FromSeconds(5.0)))
        return this.SetBookmark(entry.ID, time);
      TimeBookmark timeBookmark = this.Get(entry.ID);
      if (timeBookmark != null)
      {
        this.changesMade = true;
        this.bookmarks.Remove(timeBookmark);
        // ISSUE: reference to a compiler-generated field
        this.Changed?.Invoke(this, new TimeBookmark(entry.ID, TimeSpan.Zero));
      }
      return (TimeBookmark) null;
    }

    private TimeBookmark SetBookmark(string videoID, TimeSpan time)
    {
      TimeBookmark timeBookmark1 = this.Get(videoID);
      if (timeBookmark1 != null)
      {
        if (time < this.OnlyRememberAfter)
          return (TimeBookmark) null;
        timeBookmark1.Time = time;
        if (this.bookmarks.IndexOf(timeBookmark1) != this.bookmarks.Count - 1)
        {
          this.bookmarks.Remove(timeBookmark1);
          this.bookmarks.Add(timeBookmark1);
        }
        this.changesMade = true;
        // ISSUE: reference to a compiler-generated field
        this.Changed?.Invoke(this, timeBookmark1);
        return timeBookmark1;
      }
      if (time < this.OnlyRememberAfter)
        return (TimeBookmark) null;
      TimeBookmark timeBookmark2 = new TimeBookmark(videoID, time);
      this.bookmarks.Add(timeBookmark2);
      if (this.bookmarks.Count > 20)
        this.bookmarks.RemoveAt(0);
      this.changesMade = true;
      // ISSUE: reference to a compiler-generated field
      this.Changed?.Invoke(this, timeBookmark2);
      return timeBookmark2;
    }

    public async Task Load(string fileName = "TimeBookmarks.txt")
    {
      try
      {
        foreach (string s in (IEnumerable<string>) await FileIO.ReadLinesAsync((IStorageFile) await this.folder.GetFileAsync(fileName)))
        {
          TimeBookmark timeBookmark = TimeBookmarksManager.bookmarkFromString(s);
          if (timeBookmark != null)
          {
            bool flag = true;
            foreach (TimeBookmark bookmark in this.bookmarks)
            {
              if (bookmark.ID == timeBookmark.ID)
              {
                flag = false;
                break;
              }
            }
            if (flag)
              this.bookmarks.Add(timeBookmark);
          }
        }
      }
      catch
      {
      }
    }

    public async Task Save(string fileName = "TimeBookmarks.txt")
    {
      if (!this.changesMade)
        return;
      try
      {
        StorageFile fileAsync = await this.folder.CreateFileAsync(fileName, (CreationCollisionOption) 1);
        List<string> values = new List<string>();
        foreach (TimeBookmark bookmark in this.bookmarks)
          values.Add(TimeBookmarksManager.bookmarkToString(bookmark));
        string str = string.Join("\n", (IEnumerable<string>) values);
        await FileIO.WriteTextAsync((IStorageFile) fileAsync, str);
      }
      catch
      {
      }
    }

    private static string bookmarkToString(TimeBookmark b) => b.ID + "," + ((int) b.Time.TotalSeconds).ToString();

    private static TimeBookmark bookmarkFromString(string s)
    {
      if (!s.Contains(","))
        return (TimeBookmark) null;
      string[] strArray = s.Split(',');
      int result = 0;
      int.TryParse(strArray[1], out result);
      return new TimeBookmark(strArray[0], TimeSpan.FromSeconds((double) result));
    }
  }
}
