// Decompiled with JetBrains decompiler
// Type: RykenTube.UserInfoClient
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace RykenTube
{
  public class UserInfoClient : EntryClient<UserInfo>
  {
    public UserInfoClient()
    {
      this.ClearParts(Part.Snippet, Part.Statistics, Part.BrandingSettings);
      this.UseCache("userInfo", new GroupCacheInfo()
      {
        MaxAge = TimeSpan.FromDays(5.0),
        MaxItems = 500
      });
    }

    public async Task<Uri> GetThumbUri(string userId)
    {
      URLConstructor urlConstructor = new URLConstructor("https://www.youtube.com/channel/" + userId);
      try
      {
        using (HttpClient client = new HttpClient())
        {
          string page = await client.GetStringAsync(urlConstructor.ToUri());
          int startIndex = page.IndexOf("class=\"style-scope yt-img-shadow");
          if (startIndex != -1)
            page = page.Substring(startIndex);
          return new Uri(YouTubeHelpers.GetStringFromQuotesHTML(page, "src=\""));
        }
      }
      catch
      {
        return (Uri) null;
      }
    }

    public async Task<Uri> GetBannerUri(string userID = null)
    {
      URLConstructor urlConstructor = new URLConstructor("https://www.googleapis.com/youtube/v3/channels");
      if (userID != null)
        urlConstructor.SetValue("id", (object) userID);
      urlConstructor.SetValue("key", (object) YouTube.APIKey);
      urlConstructor.SetValue("part", (object) "brandingSettings");
      try
      {
        return new Uri((string) (JObject.Parse(await new HttpClient(YouTube.HttpFilter).GetStringAsync(new Uri(urlConstructor.ToString(), UriKind.Absolute)))["items"] as JArray)[0][(object) "brandingSettings"][(object) "image"][(object) "bannerImageUrl"], UriKind.Absolute);
      }
      catch
      {
        return (Uri) null;
      }
    }

    public override void SetBaseAddress(URLConstructor url) => url.BaseAddress = "https://www.googleapis.com/youtube/v3/channels";

    public async Task<UserInfo> GetInfo()
    {
      UserInfoClient userInfoClient = this;
      URLConstructor urlConstructor = new URLConstructor("https://www.googleapis.com/youtube/v3/channels");
      urlConstructor["mine"] = "true";
      urlConstructor["key"] = YouTube.APIKey;
      urlConstructor["part"] = userInfoClient.PartsToString();
      HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
      httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      ((ICollection<HttpExpectationHeaderValue>) httpClient.DefaultRequestHeaders.Expect).Clear();
      ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
      UserInfo info;
      try
      {
        HttpResponseMessage response = await httpClient.GetAsync(urlConstructor.ToUri(UriKind.Absolute));
        if (response.IsSuccessStatusCode)
        {
          YouTube.Write((object) nameof (UserInfoClient), (object) "Parsing signed in user JSON");
          JObject jobject = JObject.Parse(await response.Content.ReadAsStringAsync());
          YouTube.Write((object) nameof (UserInfoClient), (object) "Getting items from signed in user info JSON");
          info = new UserInfo(((JArray) jobject["items"])[0]);
        }
        else
        {
          string str = await response.Content.ReadAsStringAsync();
          throw new Exception("Request failed with error code " + (object) response.StatusCode + " and error:\n" + str);
        }
      }
      catch
      {
        throw;
      }
      return info;
    }

    public override async Task<UserInfo> ParseV3(JToken token) => new UserInfo(token);
  }
}
