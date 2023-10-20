// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeEntryClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace RykenTube
{
  public class YouTubeEntryClient : EntryClient<YouTubeEntry>
  {
    private const string Tag = "YouTubeEntryClient";

    public YouTubeEntryClient()
      : this(3)
    {
    }

    public YouTubeEntryClient(int version)
      : base(version)
    {
      this.ClearParts(Part.Snippet, Part.ContentDetails, Part.Statistics);
      this.cache();
    }

    private void cache() => this.UseCache("videoEntries", new GroupCacheInfo()
    {
      MaxAge = TimeSpan.FromDays(3.0),
      MaxItems = 500
    });

        //TODO
    public async Task<LikedResult[]> GetRating(params string[] videoIds)
    {
            // ISSUE: unable to decompile the method.
            return default;
    }

    public override void SetBaseAddress(URLConstructor url) => url.BaseAddress = "https://www.googleapis.com/youtube/v3/videos";

    public override async Task<YouTubeEntry> ParseV3(JToken token) => new YouTubeEntry(token);

    public async Task Delete(string videoId)
    {
      try
      {
        HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
        httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
        ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
        HttpResponseMessage mess = await httpClient.DeleteAsync(new Uri("https://www.googleapis.com/youtube/v3/videos?id=" + videoId + "&key=" + YouTube.AccessToken));
        if (mess.Content != null)
        {
          string str = await mess.Content.ReadAsStringAsync();
        }
        mess.EnsureSuccessStatusCode();
        mess = (HttpResponseMessage) null;
      }
      catch
      {
        throw;
      }
    }
  }
}
