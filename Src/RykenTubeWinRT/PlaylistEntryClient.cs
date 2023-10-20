// Decompiled with JetBrains decompiler
// Type: RykenTube.PlaylistEntryClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace RykenTube
{
  public class PlaylistEntryClient : EntryClient<PlaylistEntry>
  {
    public PlaylistEntryClient()
    {
      this.ClearParts(Part.Snippet, Part.ContentDetails);
      this.UseCache("playlistEntries", new GroupCacheInfo()
      {
        MaxAge = TimeSpan.FromDays(3.0),
        MaxItems = 300
      });
    }

    public override void SetBaseAddress(URLConstructor url) => url.BaseAddress = "https://www.googleapis.com/youtube/v3/playlists";

    public override async Task<PlaylistEntry> ParseV3(JToken token) => new PlaylistEntry(token);
  }
}
