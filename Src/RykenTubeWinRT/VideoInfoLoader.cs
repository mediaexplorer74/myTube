// Decompiled with JetBrains decompiler
// Type: RykenTube.VideoInfoLoader
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace RykenTube
{
  public class VideoInfoLoader
  {
    private HttpClient client;
    private string baseAddress = "";
    private bool watchPage;
    private bool decipher;
    private CipherLoader cipherLoader = new CipherLoader();
    private bool useNavigatePage;

    public string CipherAlgorithm { get; set; }

    public bool WatchPage => this.watchPage;

    public bool Decipher => this.decipher;

    public bool UseNavigatePage
    {
      get => this.useNavigatePage;
      set => this.useNavigatePage = value;
    }

    public VideoInfoLoader() => this.client = new HttpClient(YouTube.PrivateAPIHttpFilter);

    private HttpRequestMessage SetBaseAddress(string ID)
    {
      ((ICollection<HttpProductInfoHeaderValue>) this.client.DefaultRequestHeaders.UserAgent).Clear();
      this.client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      ((ICollection<HttpExpectationHeaderValue>) this.client.DefaultRequestHeaders.Expect).Clear();
      this.client.DefaultRequestHeaders.Referer = 
                new Uri("https://www.youtube.com/watch?v=" + ID);
      if (!this.watchPage)
      {
        this.baseAddress = "https://www.youtube.com/get_video_info?video_id=" + ID + "&ps=default&eurl=&gl=US&hl=en";
        if (YouTube.IsSignedIn && YouTube.UserInfo != null && !YouTube.UserInfo.Incomplete)
          this.baseAddress = this.baseAddress + "&access_token=" + YouTube.AccessToken;
      }
      else
      {
        this.baseAddress = "https://www.youtube.com/watch?v=" + ID + "&gl=US&hl=en&has_verified=1&bpctr=9999999999";
        if (YouTube.IsSignedIn && YouTube.UserInfo != null && !YouTube.UserInfo.Incomplete)
          this.baseAddress = this.baseAddress + "&access_token=" + YouTube.AccessToken;
      }
      return new HttpRequestMessage(HttpMethod.Get, new Uri(this.baseAddress));
    }

    public async Task<YouTubeInfo> LoadInfo(string ID, bool watchPage = true, bool decipher = false)
    {
      this.watchPage = watchPage;
      this.decipher = decipher;
      this.SetBaseAddress(ID);
      string stringAsync;
      try
      {
        stringAsync = await this.client.GetStringAsync(new Uri(this.baseAddress, UriKind.Absolute));
      }
      catch
      {
        throw;
      }
      YouTubeInfo info = (YouTubeInfo) null;
      YouTubeInfo youTubeInfo = new YouTubeInfo(stringAsync, watchPage, decipher, this.useNavigatePage & watchPage, this.CipherAlgorithm);
      youTubeInfo.ID = ID;
      info = youTubeInfo;
      if (decipher)
      {
        string cipher = (string) null;
        try
        {
          if (watchPage)
            cipher = await this.cipherLoader.GetCipherAlgorithmFromHttp(stringAsync);
          else
            cipher = await this.cipherLoader.GetCipherAlgorithmComplete(ID);
        }
        catch
        {
        }
        info.PerformDecipher(cipher);
        cipher = (string) null;
      }
      if (watchPage & decipher)
      {
        int num = info.DashMPD != (Uri) null ? 1 : 0;
      }
      if (!info.FoundVideos && !watchPage && info.Decipher)
        return await this.LoadInfo(ID, decipher: true);
      if (info != null)
        info.ID = ID;
      return info;
    }

    public Task<YouTubeInfo> LoadInfoAllMethods(string videoID) => this.LoadInfoAllMethods(videoID, false, false);

    public async Task<YouTubeInfo> LoadInfoAllMethods(
      string ID,
      bool startAtWatchPage,
      bool startWithDecipher)
    {
      bool success = false;
      bool keepTrying = true;
      bool watchPage = startAtWatchPage;
      bool decipher = startWithDecipher;
      YouTubeInfo info = (YouTubeInfo) null;
      int iterations = 0;
      Exception currentException = (Exception) null;
      while (!success & keepTrying)
      {
        try
        {
          info = await this.LoadInfo(ID, watchPage, decipher);
          if (info != null)
          {
            if (info.FoundVideos)
            {
              using (HttpRequestMessage mess = new HttpRequestMessage(HttpMethod.Head, info.GetLink(YouTubeQuality.HQ).ToUri(UriKind.Absolute)))
              {
                mess.Method = HttpMethod.Head;
                ((ICollection<HttpProductInfoHeaderValue>) this.client.DefaultRequestHeaders.UserAgent).Clear();
                this.client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
                using (HttpResponseMessage httpResponseMessage = await this.client.SendRequestAsync(mess, (HttpCompletionOption) 1))
                {
                  if (httpResponseMessage.IsSuccessStatusCode || (int)httpResponseMessage.StatusCode == 302)
                    return info;
                  httpResponseMessage.EnsureSuccessStatusCode();
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          currentException = ex;
        }
        if (watchPage & decipher)
        {
          watchPage = true;
          decipher = false;
        }
        else if (watchPage && !decipher)
        {
          watchPage = false;
          decipher = false;
        }
        else if (!watchPage)
        {
          watchPage = true;
          decipher = true;
        }
        ++iterations;
        if (iterations >= 3)
        {
          if (currentException != null)
            throw currentException;
          return (YouTubeInfo) null;
        }
      }
      return (YouTubeInfo) null;
    }
  }
}
