// myTube.BookmarkableActionEventArgs

using RykenTube;
using System;

namespace myTube
{
  public class BookmarkableActionEventArgs : EventArgs
  {
    public YouTubeEntry CurrentEntry { get; set; }

    public TimeSpan Position { get; set; }

    internal BookmarkableActionEventArgs(YouTubeEntry ent, TimeSpan ts)
    {
      this.CurrentEntry = ent;
      this.Position = ts;
    }
  }
}
