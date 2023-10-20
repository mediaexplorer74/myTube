// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTube
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;
using Windows.Web.Http.Headers;
using HttpClient = Windows.Web.Http.HttpClient;
using HttpRequestMessage = Windows.Web.Http.HttpRequestMessage;
using HttpResponseMessage = Windows.Web.Http.HttpResponseMessage;
using HttpMethod = Windows.Web.Http.HttpMethod;
using UnicodeEncoding = System.Text.UnicodeEncoding;

namespace RykenTube
{
  public static class YouTube
  {
    public static string Scope = "https://gdata.youtube.com https://www.googleapis.com/auth/youtube https://www.googleapis.com/auth/youtube.force-ssl https://www.googleapis.com/auth/youtube.readonly https://www.googleapis.com/auth/youtubepartner";
    public const string APIBaseUrlV3 = "https://www.googleapis.com/youtube/v3";
    public static string DecipherAlgorithm = "d:32; reverse; slice:2; d:65; d:26; d:45; d:24; d:40; slice:2";
    public static bool OnlineSignIn = false;
    public static EventHandler<SignedInEventArgs> SignedIn;
    private static HttpWebRequest accessTokenRequest;
    private static HttpWebRequest refreshRequest;
    private static bool wasRefreshSignIn = false;
    private static string address = "";
    public static string ClientID;
    public static string DeveloperKey;
    public static string RedirectUri;
    public static string ClientSecret;
    public static string APIKey;
    private static string userCode;
    private static string accessToken;
    private static string refreshToken;
    private static UserInfo userInfo;
    public static List<string> Subscriptions;
    public static Dictionary<string, string> SubscriptionIDs;
    public static SubscriptionData SubscriptionData;
    private static string lastID;
    private static string _lastID = "";
    private static int tries = 0;
    private static bool initialized = false;
    private static RegionInfo region = RegionInfo.Global;
    public static string UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
    private static bool watchPage = true;
    private static bool decrypt = false;
    public static bool HTTPS = true;
    private static HttpBaseProtocolFilter httpFilter;
    public static Func<IHttpFilter> GetPrivateAPIFilterMethod;
    public static Func<IHttpFilter> GetFilterMethod;
    public static bool Debug;
    public static int DefaultVersion;
    private static YouTubeInfo lastInfo;
    private static TaskCompletionSource<bool> subsLoadedTcs;
    private static bool signedIn;
    private static string res;
    private static readonly Dictionary<int, string> standardPageTokens;
    private static string tokenChars;
    private static string secondTokenCharsUnder128;
    private static string secondTokenChars;
    private static string middleTokenChars;
    private static SemaphoreSlim signInSemaphore;

    public static XName Atom(string name) => XNamespace.Get("http://www.w3.org/2005/Atom") + name;

    public static XName Atom(string name, string ns)
    {
      switch (ns)
      {
        case "yt":
          return XNamespace.Get("http://gdata.youtube.com/schemas/2007") + name;
        case "media":
          return XNamespace.Get("http://search.yahoo.com/mrss/") + name;
        case "gd":
          return XNamespace.Get("http://schemas.google.com/g/2005") + name;
        case "app":
          return XNamespace.Get("http://www.w3.org/2007/app") + name;
        case "openSearch":
          return XNamespace.Get("http://a9.com/-/spec/opensearch/1.1/") + name;
        default:
          return XNamespace.Get(ns) + name;
      }
    }

    public static Task<UserInfo> SignInTask => YouTube.signInSemaphoreTask();

    private static async Task<UserInfo> signInSemaphoreTask()
    {
      await YouTube.signInSemaphore.WaitAsync();
      YouTube.signInSemaphore.Release();
      return YouTube.UserInfo;
    }

    public static event EventHandler<string> Logged;

    public static ICacheHandler CacheHandler { get; set; }

    public static bool CurrentlySigningIn { get; private set; }

    public static event YouTubeErrorEventHandler ErrorReported;

    public static event YouTubeInfoEventHandler InfoLoaded;

    public static event YouTubeSearchEventHandler SearchCompleted;

    public static event YouTubeCommentsEventHandler CommentsLoaded;

    public static event EventHandler SigningIn;

    public static event EventHandler<SignedOutEventArgs> SignedOut;

    public static event EventHandler<SignedInFailedEventArgs> SignInFailed;

    public static event YouTubeInfoFailedEventHandler InfoFailed;

    public static event EventHandler SubscriptionsLoaded;

    public static event EventHandler SubscriptionsUnloaded;

    public static bool WasRefreshSignIn => YouTube.wasRefreshSignIn;

    public static bool LoadUserInfoOnLogin { get; set; } = true;

    public static bool LoadSubscriptionsOnLogin { get; set; } = true;

    public static RegionInfo Region
    {
      get => YouTube.region;
      set
      {
        if (!(YouTube.region != value))
          return;
        EventHandler regionChanged = YouTube.RegionChanged;
        if (regionChanged != null)
          regionChanged((object) null, new EventArgs());
        YouTube.region = value;
      }
    }

    public static event EventHandler RegionChanged;

    internal static IHttpFilter PrivateAPIHttpFilter
    {
      get
      {
        if (YouTube.GetPrivateAPIFilterMethod != null)
          return YouTube.GetPrivateAPIFilterMethod();
        if (YouTube.GetFilterMethod != null)
          return YouTube.GetFilterMethod();
        HttpBaseProtocolFilter privateApiHttpFilter = new HttpBaseProtocolFilter();
        privateApiHttpFilter.MaxConnectionsPerServer = 128U;
        privateApiHttpFilter.AutomaticDecompression = true;
        privateApiHttpFilter.UseProxy = false;
        return (IHttpFilter) privateApiHttpFilter;
      }
    }

    internal static IHttpFilter HttpFilter
    {
      get
      {
        if (YouTube.GetFilterMethod != null)
          return YouTube.GetFilterMethod();
        HttpBaseProtocolFilter httpFilter = new HttpBaseProtocolFilter();
        httpFilter.MaxConnectionsPerServer = 128U;
        httpFilter.AutomaticDecompression = true;
        httpFilter.UseProxy = false;
        return (IHttpFilter) httpFilter;
      }
    }

    public static bool Initialized
    {
      get => YouTube.initialized;
      private set => YouTube.initialized = value;
    }

    public static int Tries => YouTube.tries;

    public static string LastID => YouTube.lastID;

    public static Task<bool> SubscriptionsLoadedTask => YouTube.subsLoadedTcs.Task;

    public static UserInfo UserInfo => YouTube.userInfo;

    public static string RefreshToken => YouTube.refreshToken;

    public static string AccessToken => YouTube.accessToken;

    public static string UserCode
    {
      get => YouTube.userCode;
      set => YouTube.userCode = value;
    }

    public static bool IsSignedIn => YouTube.signedIn;

    public static string[] Split(this string s, string splitBy)
    {
      if (splitBy.Length == 0)
      {
        string[] strArray = new string[s.Length];
        for (int index = 0; index < strArray.Length; ++index)
          strArray[index] = s[index].ToString();
        return strArray;
      }
      return s.Split(new string[1]{ splitBy }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static bool SubscriptionsAreLoaded { get; private set; }

    public static string GetStandardPageToken(int page, int howMany)
    {
      int num = howMany * page;
      if (num > 500)
        return (string) null;
      if (num == 0)
        return (string) null;
      string str1 = YouTube.middleTokenChars[num % YouTube.middleTokenChars.Length].ToString();
      string str2 = num > (int) sbyte.MaxValue ? YouTube.secondTokenChars : YouTube.secondTokenCharsUnder128;
      string str3 = str2[num / YouTube.middleTokenChars.Length % str2.Length].ToString();
      string str4 = "QAA";
      if (num > (int) sbyte.MaxValue)
        str4 = YouTube.tokenChars[num / 128].ToString() + "EAA";
      return "C" + str3 + str1 + str4;
    }

    public static void init()
    {
      if (YouTube.Initialized)
        return;
      YouTube.Write((object) "Initializing");
      YouTube.Initialized = true;
      YouTube.SubscriptionsAreLoaded = false;
      YouTube.subsLoadedTcs = new TaskCompletionSource<bool>();
      YouTube.Subscriptions = new List<string>();
      YouTube.SubscriptionIDs = new Dictionary<string, string>();
      YouTube.SubscriptionData = new SubscriptionData();
      YouTube.Write((object) "Initialized");
    }

    public static async Task<string> GetGSessionId() => (string) JArray.Parse(await new HttpClient(YouTube.HttpFilter).GetStringAsync(new Uri("https://2.client-channel.google.com/client-channel/gsid")))[1];

    public static async Task<string> GetChatSID(string gsessionid) => (string) JArray.Parse(await new HttpClient(YouTube.HttpFilter).GetStringAsync(new Uri("https://2.client-channel.google.com/client-channel/channel/bind?ctype=yt-live-comments&gsessionid=" + gsessionid + "&VER=8&RID=43546&CVER=5&zx=3uqwl1xmrhiw&t=1")))[1];

    public static async Task PollForChat(string gsessionid, string SID) => JArray.Parse(await new HttpClient(YouTube.HttpFilter).GetStringAsync(new Uri("https://2.client-channel.google.com/client-channel/channel/bind?ctype=yt-live-comments&gsessionid=" + gsessionid + "&VER=8&RID=rpc&SID=" + SID + "&CI=0&AID=0&TYPE=xmlhttp&zx=2tvk6ce1szqq&t=1")));

    public static async Task<int> GetNumberOfLiveViewers(string videoId) => int.Parse(await new HttpClient(YouTube.HttpFilter).GetStringAsync(new Uri("http://www.youtube.com/live_stats?v=" + videoId)));

    public static void ReportError(YouTubeError e)
    {
      if (YouTube.ErrorReported == null)
        return;
      YouTube.ErrorReported(e);
    }

    internal static void Write(object tag, object output)
    {
      string e = (tag != null ? "[" + tag + "] " : "[RykenTube] ") + output.ToString();
      if (YouTube.Debug)
      {
        int num = Debugger.IsAttached ? 1 : 0;
      }
      if (YouTube.Logged == null)
        return;
      YouTube.Logged((object) null, e);
    }

    internal static void Write(object output) => YouTube.Write((object) null, output);

    public static void SignOut() => YouTube.SignOut((SignedOutEventArgs) null);

    private static void SignOut(SignedOutEventArgs args)
    {
      YouTube.Write((object) "Signing out");
      YouTube.signedIn = false;
      string refreshToken = YouTube.refreshToken;
      string accessToken = YouTube.accessToken;
      YouTube.accessToken = YouTube.refreshToken = (string) null;
      YouTube.userInfo = (UserInfo) null;
      int num = YouTube.SubscriptionData.Count > 0 ? 1 : 0;
      if (num != 0)
      {
        YouTube.SubscriptionData.Clear();
        YouTube.SubscriptionIDs.Clear();
        YouTube.Subscriptions.Clear();
      }
      if (YouTube.SignedOut != null)
      {
        if (args == null)
          args = new SignedOutEventArgs();
        args.PreviousAccessToken = accessToken;
        args.PreviousRefreshToken = refreshToken;
        YouTube.SignedOut((object) null, args);
      }
      if (num == 0)
        return;
      EventHandler subscriptionsUnloaded = YouTube.SubscriptionsUnloaded;
      if (subscriptionsUnloaded == null)
        return;
      subscriptionsUnloaded((object) null, new EventArgs());
    }

    public static async Task<UserInfo> SignIn(string code)
    {
      YouTube.Write((object) "Signing in");
      await YouTube.signInSemaphore.WaitAsync();
      UserInfo userInfo1;
      try
      {
        SignedInEventArgs args = new SignedInEventArgs()
        {
          PreviousRefreshToken = YouTube.refreshToken,
          PreviousAccessToken = YouTube.accessToken,
          IsRefreshSignIn = false
        };
        if (YouTube.signedIn)
          YouTube.SignOut();
        YouTube.signedIn = false;
        YouTube.CurrentlySigningIn = true;
        EventHandler signingIn = YouTube.SigningIn;
        if (signingIn != null)
          signingIn((object) null, new EventArgs());
        YouTube.userCode = code;
        URLConstructor urlConstructor = new URLConstructor("https://accounts.google.com/o/oauth2/token");
        HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
        httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
        HttpStringContent httpStringContent = new HttpStringContent("" + "code=" + YouTube.userCode
            + "&" + "client_id=" + YouTube.ClientID + "&" +
            "client_secret=" + YouTube.ClientSecret + "&"
            + "redirect_uri=" + YouTube.RedirectUri + "&" + "grant_type=authorization_code",
            /*(UnicodeEncoding) 0*/default, "application/x-www-form-urlencoded");
        try
        {
          YouTube.Write((object) "Posting data to Google auth server for sign in");
          HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(urlConstructor.ToUri(UriKind.Absolute), (IHttpContent) httpStringContent);
          httpResponseMessage.EnsureSuccessStatusCode();
          string json = await httpResponseMessage.Content.ReadAsStringAsync();
          YouTube.Write((object) "Parsing JSON with access token and refresh token");
          JObject jobject = JObject.Parse(json);
          string str1 = (string) jobject["access_token"];
          args.NewAccessToken = YouTube.accessToken = str1;
          YouTube.Write((object) ("New access token: " + YouTube.accessToken));
          string str2 = (string) jobject["refresh_token"];
          args.NewRefreshToken = YouTube.refreshToken = str2;
          YouTube.Write((object) ("New refresh roken: " + YouTube.refreshToken));
          YouTube.signedIn = true;
          YouTube.Write((object) "Loading user account info");
          UserInfoClient userInfoClient = new UserInfoClient();
          YouTube.wasRefreshSignIn = false;
          UserInfo info = (UserInfo) null;
          if (YouTube.LoadUserInfoOnLogin)
          {
            try
            {
              info = await userInfoClient.GetInfo();
            }
            catch (Exception ex)
            {
              YouTube.Write((object) ("Error getting UserInfo during sign in: " + (object) ex));
            }
          }
          if (info == null)
          {
            YouTube.Write((object) "Falling back to basic channel with no ID, perhaps the user has no channel");
            UserInfo userInfo2 = new UserInfo();
            userInfo2.ID = "null";
            userInfo2.UserName = "Google Account";
            userInfo2.WatchLaterPlaylist = "WL";
            info = userInfo2;
          }
          YouTube.userInfoCompleted(info, args);
          userInfo1 = info;
        }
        catch (Exception ex)
        {
          YouTube.Write((object) ("Sign in exception: " + (object) ex));
          YouTube.signedIn = false;
          throw;
        }
      }
      finally
      {
        YouTube.signInSemaphore.Release();
      }
      return userInfo1;
    }

    public static async Task<UserInfo> RefreshSignIn(string token, string fallBackUserId = null)
    {
      SignedInEventArgs args = new SignedInEventArgs()
      {
        PreviousRefreshToken = YouTube.refreshToken,
        PreviousAccessToken = YouTube.accessToken,
        NewRefreshToken = token,
        IsRefreshSignIn = true
      };
      YouTube.Write((object) "Refresh signing in");
      YouTube.CurrentlySigningIn = true;
      if (YouTube.signedIn)
        YouTube.SignOut(new SignedOutEventArgs()
        {
          NewRefreshToken = token
        });
      YouTube.signedIn = false;
      UserInfo userInfo1;
      try
      {
        UserInfo info = (UserInfo) null;
        YouTube.Write((object) "Waiting for sign-in semaphore");
        await YouTube.signInSemaphore.WaitAsync();
        if (YouTube.SigningIn != null)
          YouTube.SigningIn((object) null, new EventArgs());
        YouTube.signedIn = false;
        YouTube.refreshToken = token;
        HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
        HttpStringContent httpStringContent = new HttpStringContent(new URLConstructor()
        {
          ["client_id"] = YouTube.ClientID,
          ["client_secret"] = YouTube.ClientSecret,
          ["refresh_token"] = YouTube.refreshToken,
          ["grant_type"] = "refresh_token"
        }.ToString(), /*(UnicodeEncoding) 0*/default, "application/x-www-form-urlencoded");
        YouTube.Write((object) "Sending refresh token for sign-in");
        HttpResponseMessage httpResponseMessage;
        try
        {
          httpResponseMessage = await httpClient.PostAsync(new Uri("https://accounts.google.com/o/oauth2/token"), (IHttpContent) httpStringContent);
        }
        catch (Exception ex)
        {
          YouTube.Write((object) string.Format("Exception getting access token: {0}", (object) ex));
          EventHandler<SignedInFailedEventArgs> signInFailed = YouTube.SignInFailed;
          if (signInFailed != null)
          {
            signInFailed((object) null, new SignedInFailedEventArgs()
            {
              Critical = true,
              Reason = "Failed web request"
            });
            goto label_26;
          }
          else
            goto label_26;
        }
        string str = (string) JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync())["access_token"];
        args.NewAccessToken = YouTube.accessToken = str;
        if (string.IsNullOrWhiteSpace(YouTube.accessToken))
        {
          YouTube.Write((object) "No access token");
          EventHandler<SignedInFailedEventArgs> signInFailed = YouTube.SignInFailed;
          if (signInFailed != null)
            signInFailed((object) null, new SignedInFailedEventArgs()
            {
              Critical = true,
              Reason = "No access token"
            });
          info = (UserInfo) null;
        }
        else
        {
          YouTube.Write((object) "Successfully received access token");
          UserInfoClient userInfoClient = new UserInfoClient();
          YouTube.wasRefreshSignIn = true;
          if (YouTube.LoadUserInfoOnLogin)
          {
            try
            {
              info = await userInfoClient.GetInfo();
            }
            catch (Exception ex)
            {
              YouTube.Write((object) ("Error getting UserInfo during refresh sign in: " + (object) ex));
            }
            if (info == null && fallBackUserId != null)
            {
              try
              {
                info = await userInfoClient.GetInfo(fallBackUserId);
              }
              catch (Exception ex)
              {
                YouTube.Write((object) ("Error getting fallback UserInfo during refresh sign in for ID " + fallBackUserId + ": " + (object) ex));
              }
            }
          }
          if (info == null)
          {
            YouTube.Write((object) "Falling back to basic channel with no ID, perhaps the user has no channel");
            UserInfo userInfo2 = new UserInfo();
            userInfo2.ID = "null";
            userInfo2.UserName = "Google Account";
            userInfo2.WatchLaterPlaylist = "WL";
            info = userInfo2;
          }
          YouTube.userInfoCompleted(info, args);
          YouTube.OnlineSignIn = true;
        }
label_26:
        userInfo1 = info;
      }
      finally
      {
        YouTube.CurrentlySigningIn = false;
        YouTube.signInSemaphore.Release();
      }
      return userInfo1;
    }

    public static async Task<UserInfo> LoadUserInfo()
    {
      if (!YouTube.IsSignedIn)
        return (UserInfo) null;
      UserInfo info = (UserInfo) null;
      try
      {
        info = await new UserInfoClient().GetInfo();
        YouTube.userInfo = info;
      }
      catch (Exception ex)
      {
        YouTube.Write((object) ("Error getting UserInfo during refresh sign in: " + (object) ex));
      }
      return info;
    }

    public static void OfflineSignIn(UserInfo info, string refresh, string access)
    {
      SignedInEventArgs e = new SignedInEventArgs()
      {
        PreviousAccessToken = YouTube.accessToken,
        PreviousRefreshToken = YouTube.refreshToken,
        NewAccessToken = access,
        NewRefreshToken = refresh,
        IsRefreshSignIn = true
      };
      YouTube.userInfo = info;
      YouTube.refreshToken = refresh;
      YouTube.accessToken = access;
      YouTube.signedIn = true;
      YouTube.GetSubscriptions();
      YouTube.OnlineSignIn = false;
      if (YouTube.SignedIn == null)
        return;
      YouTube.SignedIn((object) null, e);
    }

    private static void userInfoCompleted(UserInfo info, SignedInEventArgs args)
    {
      YouTube.CurrentlySigningIn = false;
      YouTube.userInfo = info;
      YouTube.signedIn = true;
      YouTube.OnlineSignIn = true;
      YouTube.signedIn = true;
      if (YouTube.SignedIn == null)
        return;
      YouTube.SignedIn((object) null, args);
    }

    public static bool IsSubscribedTo(UserInfo inf) => YouTube.IsSubscribedTo(inf.ID);

    public static bool IsSubscribedTo(string channelId)
    {
      if (YouTube.SubscriptionData == null)
        return false;
      foreach (ClientDataBase clientDataBase in (Collection<UserInfo>) YouTube.SubscriptionData)
      {
        if (UserInfo.RemoveUCFromID(clientDataBase.ID) == UserInfo.RemoveUCFromID(channelId))
          return true;
      }
      return false;
    }

    public static string GetSubscriptionID(UserInfo inf)
    {
      if (YouTube.SubscriptionIDs.ContainsKey(inf.ID))
        return YouTube.SubscriptionIDs[inf.ID];
      return YouTube.SubscriptionIDs.ContainsKey(inf.UserName) ? YouTube.SubscriptionIDs[inf.UserName] : (string) null;
    }

    public static string GetSubscriptionID(string channelId)
    {
      if (YouTube.SubscriptionIDs == null)
        return (string) null;
      return YouTube.SubscriptionIDs.ContainsKey(channelId) ? YouTube.SubscriptionIDs[channelId] : (string) null;
    }

    public static void RemoveSubscribedUser(UserInfo inf)
    {
      if (YouTube.SubscriptionIDs.ContainsKey(inf.ID))
        YouTube.SubscriptionIDs.Remove(inf.ID);
      else if (YouTube.SubscriptionIDs.ContainsKey(inf.UserName))
        YouTube.SubscriptionIDs.Remove(inf.UserName);
      foreach (UserInfo userInfo in (Collection<UserInfo>) YouTube.SubscriptionData)
      {
        if (userInfo.ID == inf.ID || userInfo.UserName == inf.UserName)
        {
          YouTube.SubscriptionData.Remove(userInfo);
          break;
        }
      }
    }

    public static void RemoveSubscribedUser(string channelId)
    {
      if (YouTube.SubscriptionIDs.ContainsKey(channelId))
        YouTube.SubscriptionIDs.Remove(channelId);
      foreach (UserInfo userInfo in (Collection<UserInfo>) YouTube.SubscriptionData)
      {
        if (userInfo.ID == channelId)
        {
          YouTube.SubscriptionData.Remove(userInfo);
          break;
        }
      }
    }

    public static void CallSubscriptionsLoaded()
    {
      if (YouTube.SubscriptionsLoaded == null)
        return;
      YouTube.SubscriptionsLoaded((object) null, new EventArgs());
    }

    public static Task<UserInfo> Subscribe(string username)
    {
      if (YouTube.DefaultVersion == 2)
      {
        TaskCompletionSource<UserInfo> tcs = new TaskCompletionSource<UserInfo>();
        XDocument x = new XDocument();
        XNamespace xnamespace1 = (XNamespace) "http://www.w3.org/2005/Atom";
        XNamespace xnamespace2 = (XNamespace) "http://gdata.youtube.com/schemas/2007";
        XElement content1 = new XElement(xnamespace1 + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) xnamespace2.NamespaceName));
        XElement content2 = new XElement(xnamespace1 + "category");
        content2.Add((object) new XAttribute((XName) "scheme", (object) "http://gdata.youtube.com/schemas/2007/subscriptiontypes.cat"));
        content2.Add((object) new XAttribute((XName) "term", (object) "channel"));
        XElement content3 = new XElement(xnamespace2 + nameof (username));
        content3.Value = username;
        content1.Add((object) content2);
        content1.Add((object) content3);
        x.Add((object) content1);
        HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/subscriptions");
        req.Method = "POST";
        req.ContentType = "application/atom+xml";
        req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
        req.Headers["GData-Version"] = "2";
        req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
        req.BeginGetRequestStream((AsyncCallback) (a =>
        {
          Stream requestStream;
          try
          {
            requestStream = req.EndGetRequestStream(a);
          }
          catch
          {
            return;
          }
          using (StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.UTF8))
          {
            streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            streamWriter.WriteLine(x.ToString());
          }
          req.BeginGetResponse((AsyncCallback) (a2 =>
          {
            try
            {
              tcs.SetResult((UserInfo) null);
            }
            catch (Exception ex)
            {
              tcs.SetException(ex);
            }
          }), (object) 0);
        }), (object) 0);
        return tcs.Task;
      }
      return YouTube.DefaultVersion == 3 ? Task.Run<UserInfo>((Func<Task<UserInfo>>) (async () =>
      {
        JObject jobject1 = new JObject();
        JObject jobject2 = new JObject();
        JObject jobject3 = new JObject();
        jobject3.Add("kind", (JToken) new JValue("youtube#channel"));
        jobject3.Add("channelId", (JToken) new JValue(username));
        jobject1.Add("snippet", (JToken) jobject2);
        jobject2.Add("resourceId", (JToken) jobject3);
        string str = jobject1.ToString(Formatting.None);
        HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
        HttpRequestMessage content = new HttpRequestMessage(HttpMethod.Post, new Uri("https://www.googleapis.com/youtube/v3/subscriptions?part=snippet&key=" + YouTube.APIKey));
        ((IDictionary<string, string>) content.Headers).Add("Authorization", "Bearer " + YouTube.accessToken);
        content.Headers.UserAgent.TryParseAdd(YouTube.UserAgent);
        content.Content = (IHttpContent) new HttpStringContent(str, /*(UnicodeEncoding) 0*/default, "application/json");
        UserInfo userInfo;
        try
        {
          JToken json = JToken.Parse(await (await httpClient.SendRequestAsync(content)).Content.ReadAsStringAsync());
          content.Dispose();
          userInfo = UserInfo.FromSubscriptionData(json);
        }
        catch (Exception ex)
        {
          content.Dispose();
          throw;
        }
        return userInfo;
      })) : new TaskCompletionSource<UserInfo>().Task;
    }

    public static Task<bool> Unsubscribe(string subscriptionId)
    {
      if (YouTube.DefaultVersion != 2)
        return Task.Run<bool>((Func<Task<bool>>) (async () =>
        {
          string uriString = "https://www.googleapis.com/youtube/v3/subscriptions?id=" + subscriptionId + "&key=" + YouTube.APIKey;
          HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
          httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
          ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.accessToken);
          bool flag;
          try
          {
            HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(new Uri(uriString));
            flag = true;
          }
          catch
          {
            throw;
          }
          return flag;
        }));
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/subscriptions/" + subscriptionId);
      req.Method = "DELETE";
      req.ContentType = "application/atom+xml";
      req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
      req.Headers["GData-Version"] = "2";
      req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
      req.BeginGetResponse((AsyncCallback) (a2 =>
      {
        try
        {
          req.EndGetResponse(a2);
          tcs.TrySetResult(true);
        }
        catch (WebException ex)
        {
          tcs.SetException((Exception) ex);
        }
      }), (object) 0);
      return tcs.Task;
    }

    public static Task<bool> Rate(string ID, bool? Like)
    {
      switch (YouTube.DefaultVersion)
      {
        case 2:
          TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
          XDocument x = new XDocument();
          XNamespace xnamespace1 = (XNamespace) "http://www.w3.org/2005/Atom";
          XNamespace xnamespace2 = (XNamespace) "http://gdata.youtube.com/schemas/2007";
          XElement content1 = new XElement(xnamespace1 + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) xnamespace2.NamespaceName));
          XElement content2 = new XElement(xnamespace2 + "rating");
          if (Like.Value)
            content2.Add((object) new XAttribute((XName) "value", (object) "like"));
          else
            content2.Add((object) new XAttribute((XName) "value", (object) "dislike"));
          content1.Add((object) content2);
          x.Add((object) content1);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/videos/" + ID + "/ratings");
          req.Method = "POST";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a =>
          {
            try
            {
              using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
              {
                streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                streamWriter.WriteLine(x.ToString());
              }
            }
            catch (Exception ex)
            {
              tcs.SetException(ex);
            }
            req.BeginGetResponse((AsyncCallback) (a2 =>
            {
              try
              {
                tcs.SetResult(req.EndGetResponse(a2) != null);
              }
              catch (Exception ex)
              {
                tcs.SetException(ex);
              }
            }), (object) 0);
          }), (object) 0);
          return tcs.Task;
        case 3:
          return Task.Run<bool>((Func<Task<bool>>) (async () =>
          {
            URLConstructor urlConstructor = new URLConstructor("https://www.googleapis.com/youtube/v3/videos/rate?key=" + YouTube.APIKey);
            urlConstructor["id"] = ID;
            urlConstructor["rating"] = !Like.HasValue || !Like.HasValue ? "none" : (Like.Value ? "like" : "dislike");
            HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
            ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
            try
            {
              HttpResponseMessage response = await httpClient.PostAsync(urlConstructor.ToUri(UriKind.Absolute), (IHttpContent) null);
              if (response.IsSuccessStatusCode)
                return true;
              if (response.Content == null)
                throw new Exception("Like request failed with error code " + (object) response.StatusCode);
              try
              {
                string str = await response.Content.ReadAsStringAsync();
                throw new Exception("Like request failed with code " + (object) response.StatusCode + " and message:\n" + str);
              }
              catch
              {
                throw;
              }
            }
            catch
            {
              return false;
            }
          }));
        default:
          TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
          completionSource.SetResult(false);
          return completionSource.Task;
      }
    }

    public static Task<bool> Favorite(string ID)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          XDocument x = new XDocument();
          XNamespace xnamespace = (XNamespace) "http://www.w3.org/2005/Atom";
          XElement content = new XElement(xnamespace + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) ((XNamespace) "http://gdata.youtube.com/schemas/2007").NamespaceName));
          content.Add((object) new XElement(xnamespace + "id")
          {
            Value = ID
          });
          x.Add((object) content);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/favorites");
          req.Method = "POST";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a =>
          {
            using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
            {
              streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
              streamWriter.WriteLine(x.ToString());
            }
            req.BeginGetResponse((AsyncCallback) (a2 =>
            {
              try
              {
                req.EndGetResponse(a2);
                tcs.SetResult(true);
              }
              catch (Exception ex)
              {
                tcs.SetException(ex);
              }
            }), (object) 0);
          }), (object) 0);
          break;
        case 3:
          return YouTube.AddToPlaylist(YouTube.UserInfo.FavoritesPlaylist, ID);
      }
      return tcs.Task;
    }

    public static Task<bool> FavoriteRemove(string ID)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          XDocument xdocument = new XDocument();
          XNamespace xnamespace = (XNamespace) "http://www.w3.org/2005/Atom";
          XElement xelement = new XElement(xnamespace + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) ((XNamespace) "http://gdata.youtube.com/schemas/2007").NamespaceName));
          xelement.Add((object) new XElement(xnamespace + "id")
          {
            Value = ID
          });
          XElement content = xelement;
          xdocument.Add((object) content);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/favorites/" + ID);
          req.Method = "DELETE";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetResponse((AsyncCallback) (a2 =>
          {
            try
            {
              req.EndGetResponse(a2);
              tcs.SetResult(true);
            }
            catch (Exception ex)
            {
              tcs.SetException(ex);
            }
          }), (object) 0);
          break;
        case 3:
          return YouTube.RemoveFromPlaylist(YouTube.UserInfo.FavoritesPlaylist, ID);
      }
      return tcs.Task;
    }

    public static Task<bool> CreatePlaylist(
      string title,
      string description,
      bool privat,
      string videoID = null)
    {
      return YouTube.CreatePlaylist(title, description, privat ? PrivacyStatus.Private : PrivacyStatus.Public, videoID);
    }

    public static Task<bool> CreatePlaylist(
      string title,
      string description,
      PrivacyStatus privat,
      string videoID = null)
    {
      TaskCompletionSource<bool> tsc = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          XDocument x = new XDocument();
          XNamespace xnamespace1 = (XNamespace) "http://www.w3.org/2005/Atom";
          XNamespace xnamespace2 = (XNamespace) "http://gdata.youtube.com/schemas/2007";
          XElement content1 = new XElement(xnamespace1 + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) xnamespace2.NamespaceName));
          XElement content2 = new XElement(xnamespace1 + nameof (title));
          content2.Add((object) new XAttribute((XName) "type", (object) "text"));
          XElement content3 = new XElement(xnamespace1 + "summary")
          {
            Value = description
          };
          content2.Value = title;
          content1.Add((object) content2);
          content1.Add((object) content3);
          if (privat == PrivacyStatus.Private)
            content1.Add((object) new XElement(xnamespace2 + "private"));
          x.Add((object) content1);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/playlists/");
          req.Method = "POST";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a =>
          {
            using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
            {
              streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
              streamWriter.WriteLine(x.ToString());
            }
            req.BeginGetResponse((AsyncCallback) (async a2 =>
            {
              try
              {
                Stream responseStream = req.EndGetResponse(a2).GetResponseStream();
                string text = "";
                using (StreamReader streamReader = new StreamReader(responseStream))
                  text = streamReader.ReadToEnd();
                if (videoID != null)
                {
                  int num = await YouTube.AddToPlaylist(new PlaylistEntry(XElement.Parse(text)).ID, videoID, 1) ? 1 : 0;
                }
                tsc.SetResult(true);
              }
              catch (WebException ex)
              {
                tsc.SetException((Exception) ex);
                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                  streamReader.ReadToEnd();
              }
            }), (object) 0);
          }), (object) 0);
          break;
        case 3:
          return Task.Run<bool>((Func<Task<bool>>) (async () =>
          {
            string uriString = "https://www.googleapis.com/youtube/v3/playlists?part=snippet,status,contentDetails&key=" + YouTube.APIKey;
            JObject jobject1 = new JObject();
            JObject jobject2 = new JObject();
            JObject jobject3 = new JObject();
            switch (privat)
            {
              case PrivacyStatus.Public:
                jobject3.Add("privacyStatus", (JToken) "public");
                break;
              case PrivacyStatus.Private:
                jobject3.Add("privacyStatus", (JToken) "private");
                break;
              case PrivacyStatus.Unlisted:
                jobject3.Add("privacyStatus", (JToken) "unlisted");
                break;
            }
            jobject2.Add(nameof (title), (JToken) title);
            jobject2.Add(nameof (description), (JToken) description);
            jobject1.Add("snippet", (JToken) jobject2);
            jobject1.Add("status", (JToken) jobject3);
            HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
            ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
            try
            {
              HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(new Uri(uriString), 
                  (IHttpContent) new HttpStringContent(jobject1.ToString(), /*(UnicodeEncoding) 0*/default, "application/json"));
              if (!string.IsNullOrWhiteSpace(videoID))
              {
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                  if (httpResponseMessage.Content != null)
                  {
                    try
                    {
                      PlaylistEntry playlistEntry = new PlaylistEntry(JToken.Parse(await httpResponseMessage.Content.ReadAsStringAsync()));
                      if (!string.IsNullOrWhiteSpace(playlistEntry.ID))
                      {
                        int num = await YouTube.AddToPlaylist(playlistEntry.ID, videoID) ? 1 : 0;
                      }
                    }
                    catch
                    {
                    }
                  }
                }
              }
            }
            catch
            {
              throw;
            }
            return true;
          }));
      }
      return tsc.Task;
    }

    public static Task<bool> DeletePlaylist(string playlistID)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/playlists/" + playlistID);
          req.Method = "DELETE";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a => req.BeginGetResponse((AsyncCallback) (a2 =>
          {
            try
            {
              WebResponse response = req.EndGetResponse(a2);
              tcs.SetResult(true);
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                streamReader.ReadToEnd();
            }
            catch (WebException ex)
            {
              tcs.SetException((Exception) ex);
              using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                streamReader.ReadToEnd();
            }
          }), (object) 0)), (object) 0);
          break;
        case 3:
          return Task.Run<bool>((Func<Task<bool>>) (async () =>
          {
            string uriString = "https://www.googleapis.com/youtube/v3/playlists?id=" + playlistID + "&key=" + YouTube.APIKey;
            HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
            ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
            try
            {
              HttpResponseMessage httpResponseMessage = await httpClient.DeleteAsync(new Uri(uriString));
            }
            catch
            {
              throw;
            }
            return true;
          }));
      }
      return tcs.Task;
    }

    public static Task<bool> EditPlaylist(
      string playlistID,
      string title,
      string description,
      bool privat)
    {
      return YouTube.EditPlaylist(playlistID, title, description, privat ? PrivacyStatus.Private : PrivacyStatus.Public);
    }

    public static Task<bool> EditPlaylist(
      string playlistID,
      string title,
      string description,
      PrivacyStatus privat)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          XDocument x = new XDocument();
          XNamespace xnamespace1 = (XNamespace) "http://www.w3.org/2005/Atom";
          XNamespace xnamespace2 = (XNamespace) "http://gdata.youtube.com/schemas/2007";
          XElement content1 = new XElement(xnamespace1 + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) xnamespace2.NamespaceName));
          XElement content2 = new XElement(xnamespace1 + nameof (title));
          content2.Add((object) new XAttribute((XName) "type", (object) "text"));
          XElement content3 = new XElement(xnamespace1 + "summary")
          {
            Value = description
          };
          content2.Value = title;
          content1.Add((object) content2);
          content1.Add((object) content3);
          if (privat == PrivacyStatus.Private)
            content1.Add((object) new XElement(xnamespace2 + "private"));
          x.Add((object) content1);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/playlists/" + playlistID);
          req.Method = "PUT";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a =>
          {
            using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
            {
              streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
              streamWriter.WriteLine(x.ToString());
            }
            req.BeginGetResponse((AsyncCallback) (a2 =>
            {
              try
              {
                WebResponse response = req.EndGetResponse(a2);
                tcs.SetResult(true);
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                  streamReader.ReadToEnd();
              }
              catch (WebException ex)
              {
                tcs.SetException((Exception) ex);
                using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                  streamReader.ReadToEnd();
              }
            }), (object) 0);
          }), (object) 0);
          break;
        case 3:
          return Task.Run<bool>((Func<Task<bool>>) (async () =>
          {
            string uriString = "https://www.googleapis.com/youtube/v3/playlists?part=id,snippet,status&key=" + YouTube.APIKey;
            JObject jobject1 = new JObject();
            jobject1.Add("id", (JToken) playlistID);
            JObject jobject2 = new JObject();
            JObject jobject3 = new JObject();
            jobject2.Add(nameof (title), (JToken) title);
            jobject2.Add(nameof (description), (JToken) description);
            switch (privat)
            {
              case PrivacyStatus.Public:
                jobject3.Add("privacyStatus", (JToken) "public");
                break;
              case PrivacyStatus.Private:
                jobject3.Add("privacyStatus", (JToken) "private");
                break;
              case PrivacyStatus.Unlisted:
                jobject3.Add("privacyStatus", (JToken) "unlisted");
                break;
            }
            jobject1.Add("snippet", (JToken) jobject2);
            jobject1.Add("status", (JToken) jobject3);
            HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
            ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
            try
            {
              string str = await (await httpClient.PutAsync(new Uri(uriString), 
                  (IHttpContent) new HttpStringContent(jobject1.ToString(), /*(UnicodeEncoding) 0*/default, "application/json"))).Content.ReadAsStringAsync();
            }
            catch
            {
              throw;
            }
            return true;
          }));
      }
      return tcs.Task;
    }

    public static Task<bool> RateComment(Comment comment, bool like, bool undo = false) => YouTube.RateComment(comment.ID, comment.VideoID, like, undo, comment.IsReply);

    public static Task<bool> RateComment(
      string commentId,
      string videoId,
      bool like,
      bool undo = false,
      bool isReply = false)
    {
      return Task.Run<bool>((Func<Task<bool>>) (async () =>
      {
        HttpClient client = new HttpClient(YouTube.HttpFilter);
        client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
        ((ICollection<HttpExpectationHeaderValue>) client.DefaultRequestHeaders.Expect).Clear();
        string stringAsync = await client.GetStringAsync(new Uri("https://m.youtube.com/watch?v=" + videoId + "&access_token=" + YouTube.AccessToken));
        string str1 = "'XSRF_TOKEN':";
        int index1 = 0;
        bool flag1 = false;
        bool flag2 = false;
        int startIndex = 0;
        string str2 = (string) null;
        for (int index2 = 0; index2 < stringAsync.Length; ++index2)
        {
          char ch = stringAsync[index2];
          if (!flag1)
          {
            if ((int) ch == (int) str1[index1])
            {
              ++index1;
              if (index1 >= str1.Length)
              {
                flag1 = true;
                index1 = 0;
              }
            }
            else
              index1 = 0;
          }
          else if (!flag2)
          {
            if (ch == '"')
            {
              flag2 = true;
              startIndex = index2 + 1;
            }
          }
          else if (ch == '"')
          {
            str2 = stringAsync.Substring(startIndex, index2 - startIndex);
            break;
          }
        }
        if (str2 == null)
          throw new Exception("Unable to find session token");
        URLConstructor urlConstructor = new URLConstructor();
        if (isReply)
          urlConstructor["is_reply"] = "1";
        urlConstructor["comment_id"] = commentId;
        urlConstructor["session_token"] = str2;
        HttpStringContent httpStringContent = new HttpStringContent(urlConstructor.ToString(),
            /*(UnicodeEncoding) 0*/default, "application/x-www-form-urlencoded");
        string[] strArray = new string[6]
        {
          "https://www.youtube.com/comment_ajax?action_",
          like ? nameof (like) : "dislike",
          "=1",
          undo ? "&undo=1" : "",
          "&access_token=",
          YouTube.AccessToken
        };
        HttpResponseMessage response = await client.PostAsync(new Uri(string.Concat(strArray)), (IHttpContent) httpStringContent);
        if (!response.IsSuccessStatusCode && response.Content != null)
        {
          string str3 = await response.Content.ReadAsStringAsync();
        }
        response.EnsureSuccessStatusCode();
        return true;
      }));
    }

    public static async Task ClearWatchHistory()
    {
      try
      {
        if (!YouTube.IsSignedIn)
          return;
        HttpClient client = new HttpClient(YouTube.HttpFilter);
        string sessionTokenFromHtml = YouTubeHelpers.GetSessionTokenFromHTML(await client.GetStringAsync(new Uri("https://www.youtube.com/feed/history?access_token=" + YouTube.accessToken)), "'XSRF_TOKEN':");
        if (sessionTokenFromHtml != null)
        {
          URLConstructor urlConstructor1 = new URLConstructor("https://www.youtube.com/feed_ajax?action_clear_watch_history=1&clear_dialog_shown=0");
          urlConstructor1["access_token"] = YouTube.accessToken;
          URLConstructor urlConstructor2 = new URLConstructor();
          urlConstructor2.SetValue("session_token", (object) sessionTokenFromHtml);
          HttpStringContent httpStringContent = new HttpStringContent(urlConstructor2.ToString(), /*(UnicodeEncoding) 0*/default, "application/x-www-form-urlencoded");
          HttpResponseMessage httpResponseMessage = await client.PostAsync(urlConstructor1.ToUri(UriKind.Absolute), (IHttpContent) httpStringContent);
        }
        client = (HttpClient) null;
      }
      catch
      {
      }
    }

    public static async Task AddToWatchHistory(string videoID)
    {
      if (!YouTube.IsSignedIn)
        return;
      try
      {
        await YouTube.AddToWatchHistory(await new VideoInfoLoader().LoadInfoAllMethods(videoID));
      }
      catch (Exception ex)
      {
      }
    }

    public static async Task AddToWatchHistory(YouTubeInfo info)
    {
      if (!YouTube.IsSignedIn || info.VideoPlaybackStatsBaseUrl == null)
        return;
      URLConstructor urlConstructor = new URLConstructor(info.VideoPlaybackStatsBaseUrl);
      urlConstructor["ns"] = "yt";
      urlConstructor["lact"] = "2125";
      urlConstructor["mos"] = "0";
      urlConstructor.SetValue("ver", (object) 2);
      urlConstructor.SetValue("volume", (object) 100);
      urlConstructor.SetValue("cr", (object) "US");
      urlConstructor.SetValue("c", (object) "WEB");
      urlConstructor.SetValue("afmt", (object) 251);
      urlConstructor.SetValue("cver", (object) "1.20170518");
      urlConstructor.SetValue("feature", (object) "atom-subs");
      urlConstructor.SetValue("fmt", (object) "135");
      urlConstructor.SetValue("ns", (object) "yt");
      urlConstructor.SetValue("fs", (object) "0");
      urlConstructor.SetValue("hl", (object) "en_US");
      urlConstructor.SetValue("cbr", (object) "Edge");
      urlConstructor.SetValue("cbrver", (object) "15.15063");
      urlConstructor.SetValue("cl", (object) "156517915");
      urlConstructor.SetValue("cplayer", (object) "UNIPLAYER");
      urlConstructor.SetValue("cosver", (object) "10.0");
      urlConstructor.SetValue("rt", (object) "1.138");
      urlConstructor.SetValue("rtn", (object) "7");
      if (info.OFValue != null)
        urlConstructor.SetValue("of", (object) info.OFValue);
      if (info.CPN != null)
        urlConstructor.SetValue("cpn", (object) info.CPN);
      else if (!urlConstructor.ContainsKey("cpn"))
      {
        string str = Guid.NewGuid().ToString().Replace("-", "");
        urlConstructor.SetValue("cpn", (object) str);
      }
      using (HttpClient client = new HttpClient(YouTube.HttpFilter))
      {
        ((IDictionary<string, string>) client.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36 Edge/15.15063");
        try
        {
          (await client.GetAsync(urlConstructor.ToUri(UriKind.Absolute))).EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
          YouTube.Write((object) nameof (AddToWatchHistory), (object) string.Format("Http error: {0}", (object) ex));
        }
      }
    }

    public static Task<bool> BookmarkPlaylist(string playlistID, bool add) => Task.Run<bool>((Func<Task<bool>>) (async () =>
    {
      if (YouTube.AccessToken == null)
        return false;
      HttpClient client = new HttpClient(YouTube.PrivateAPIHttpFilter);
      client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      string stringAsync = await client.GetStringAsync(new Uri("https://www.youtube.com/playlist?list=" + playlistID + "&access_token=" + YouTube.AccessToken));
      string str1 = YouTubeHelpers.GetSessionTokenFromHTML(stringAsync, "'XSRF_TOKEN':") ?? YouTubeHelpers.GetSessionTokenFromHTML(stringAsync, "\"XSRF_TOKEN\":");
      if (str1 == null)
        return false;
      HttpStringContent httpStringContent = new HttpStringContent(new URLConstructor()
      {
        ["session_token"] = str1,
        ["list"] = playlistID,
        ["vote"] = (add ? "like" : "remove_like")
      }.ToString(), /*(UnicodeEncoding) 0*/default, "application/x-www-form-urlencoded");
      try
      {
        HttpResponseMessage response = await client.PostAsync(new Uri("https://www.youtube.com/playlist_ajax?action_playlist_vote=1&access_token=" + YouTube.AccessToken), (IHttpContent) httpStringContent);
        if (response.IsSuccessStatusCode)
        {
          string str2 = await response.Content.ReadAsStringAsync();
          return true;
        }
        string str3 = await response.Content.ReadAsStringAsync();
        throw new Exception("Bookmark playlist failed with error code " + (object) response.StatusCode);
      }
      catch
      {
        throw;
      }
    }));

    public static Task<bool> AddToPlaylist(string playlistID, string videoID, int index = -1)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          if (index < 1)
            index = 1;
          XDocument x = new XDocument();
          XNamespace xnamespace1 = (XNamespace) "http://www.w3.org/2005/Atom";
          XNamespace xnamespace2 = (XNamespace) "http://gdata.youtube.com/schemas/2007";
          XElement content1 = new XElement(xnamespace1 + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) xnamespace2.NamespaceName));
          XElement content2 = new XElement(xnamespace1 + "id");
          XElement content3 = new XElement(xnamespace2 + "position")
          {
            Value = index.ToString()
          };
          content2.Value = videoID;
          content1.Add((object) content2);
          content1.Add((object) content3);
          x.Add((object) content1);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/playlists/" + playlistID);
          req.Method = "POST";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a =>
          {
            using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
            {
              streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
              streamWriter.WriteLine(x.ToString());
            }
            req.BeginGetResponse((AsyncCallback) (a2 =>
            {
              try
              {
                req.EndGetResponse(a2);
                tcs.SetResult(true);
              }
              catch (WebException ex)
              {
                tcs.SetException((Exception) ex);
              }
            }), (object) 0);
          }), (object) 0);
          break;
        case 3:
          return Task.Run<bool>((Func<Task<bool>>) (async () =>
          {
            string uriString = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&key=" + YouTube.APIKey;
            HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
            JObject jobject1 = new JObject();
            JValue jvalue = new JValue(playlistID);
            JObject jobject2 = new JObject();
            JObject jobject3 = new JObject();
            jobject3.Add("kind", (JToken) "youtube#video");
            jobject3.Add("videoId", (JToken) videoID);
            jobject2.Add("playlistId", (JToken) jvalue);
            jobject2.Add("resourceId", (JToken) jobject3);
            if (index != -1)
              jobject2.Add("position", (JToken) index);
            jobject1.Add("snippet", (JToken) jobject2);
            HttpStringContent httpStringContent = new HttpStringContent(jobject1.ToString(), /*(UnicodeEncoding) 0*/default, "application/json");
            ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
            try
            {
              HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(new Uri(uriString), (IHttpContent) httpStringContent);
              try
              {
                string str = await httpResponseMessage.Content.ReadAsStringAsync();
              }
              catch
              {
              }
            }
            catch
            {
              throw;
            }
            return true;
          }));
      }
      return tcs.Task;
    }

    public static Task<YouTubeEntry> RearrangePlaylist(
      string playlistID,
      string playlistVideoID,
      string videoID,
      int index)
    {
      if (index < 0)
        index = 0;
      if (YouTube.DefaultVersion == 2)
        return (Task<YouTubeEntry>) null;
      return YouTube.DefaultVersion == 3 ? Task.Run<YouTubeEntry>((Func<Task<YouTubeEntry>>) (async () =>
      {
        URLConstructor urlConstructor = new URLConstructor("https://www.googleapis.com/youtube/v3/playlistItems");
        urlConstructor["key"] = YouTube.APIKey;
        urlConstructor["part"] = "snippet,contentDetails";
        JObject jobject1 = new JObject();
        JObject jobject2 = new JObject();
        jobject1.Add("snippet", (JToken) jobject2);
        jobject1.Add("id", (JToken) playlistVideoID);
        jobject2.Add("playlistId", (JToken) playlistID);
        jobject2.Add("position", (JToken) index);
        JObject jobject3 = new JObject();
        jobject2.Add("resourceId", (JToken) jobject3);
        jobject3.Add("videoId", (JToken) videoID);
        jobject3.Add("kind", (JToken) "youtube#video");
        string str1 = jobject1.ToString();
        HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
        httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
        ((IDictionary<string, string>) httpClient.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
        HttpStringContent httpStringContent = new HttpStringContent(str1, /*(UnicodeEncoding) 0*/default, "application/json");
        YouTubeEntry youTubeEntry;
        try
        {
          HttpResponseMessage response = await httpClient.PutAsync(urlConstructor.ToUri(UriKind.Absolute), (IHttpContent) httpStringContent);
          if (response.IsSuccessStatusCode)
          {
            youTubeEntry = new YouTubeEntry(JToken.Parse(await response.Content.ReadAsStringAsync()));
          }
          else
          {
            string str2 = await response.Content.ReadAsStringAsync();
            throw new Exception("Playlist rearrange failed with status code " + (object) response.StatusCode + " and error:\n" + str2);
          }
        }
        catch
        {
          throw;
        }
        return youTubeEntry;
      })) : (Task<YouTubeEntry>) null;
    }

    public static async Task<bool> RemoveFromWatchLaterPage(string videoID)
    {
      string Tag = nameof (RemoveFromWatchLaterPage);
      if (YouTube.AccessToken == null)
        throw new InvalidOperationException("Must be signed into YouTube to remove videos from watch later");
      HttpClient client = new HttpClient(YouTube.PrivateAPIHttpFilter);
      client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
      client.DefaultRequestHeaders.AcceptEncoding.TryParseAdd("gzip, deflate, br");
      URLConstructor urlConstructor1 = new URLConstructor("https://www.youtube.com/watch?v=" + videoID + "&list=WL");
      ((IDictionary<string, string>) client.DefaultRequestHeaders).Add("Authorization", "Bearer " + YouTube.AccessToken);
      string stringAsync = await client.GetStringAsync(urlConstructor1.ToUri(UriKind.Absolute));
      string str1 = YouTubeHelpers.GetStringFromQuotesHTML(stringAsync, "\"plid\":") ?? YouTubeHelpers.GetStringFromQuotesHTML(stringAsync, "'plid':");
      string str2 = YouTubeHelpers.GetSessionTokenFromHTML(stringAsync, "'XSRF_TOKEN':") ?? YouTubeHelpers.GetSessionTokenFromHTML(stringAsync, "\"XSRF_TOKEN\":");
      if (str1 == null)
      {
        YouTube.Write((object) Tag, (object) "Could not get plid");
        throw new Exception("Could not get plid");
      }
      if (str2 != null)
      {
        if (YouTube.AccessToken == null)
          throw new InvalidOperationException("Must be signed into YouTube to remove videos from watch later");
        URLConstructor urlConstructor2 = new URLConstructor("https://www.youtube.com/playlist_video_ajax?action_delete_from_playlist=1");
        HttpStringContent httpStringContent = new HttpStringContent(new URLConstructor()
        {
          ["video_ids"] = videoID,
          ["full_list_id"] = "WL",
          ["plid"] = str1,
          ["session_token"] = WebUtility.UrlEncode(str2)
        }.ToString(URLDisplayMode.ExcludeNullValues, false), /*(UnicodeEncoding) 0*/default, "application/x-www-form-urlencoded");
        HttpResponseMessage result = await client.PostAsync(urlConstructor2.ToUri(UriKind.Absolute), (IHttpContent) httpStringContent);
        if (!result.IsSuccessStatusCode)
        {
          string str3 = await result.Content.ReadAsStringAsync();
          YouTube.Write((object) nameof (RemoveFromWatchLaterPage), (object) string.Format("Remove failed with status code {0} {1}: {2}", (object) (int) result.StatusCode, (object) result.StatusCode, (object) str3));
        }
        result.EnsureSuccessStatusCode();
        return true;
      }
      YouTube.Write((object) nameof (RemoveFromWatchLaterPage), (object) "Could not get session token");
      throw new Exception("Could not get session token");
    }

    public static Task<bool> RemoveFromWatchLater(string playlistVideoID)
    {
      if (YouTube.DefaultVersion == 2)
      {
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/watch_later/" + playlistVideoID);
        req.Method = "DELETE";
        req.ContentType = "application/atom+xml";
        req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
        req.Headers["GData-Version"] = "2";
        req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
        req.BeginGetRequestStream((AsyncCallback) (a => req.BeginGetResponse((AsyncCallback) (a2 =>
        {
          try
          {
            tcs.SetResult(req.EndGetResponse(a2) != null);
          }
          catch (WebException ex)
          {
            tcs.SetException((Exception) ex);
            using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
              streamReader.ReadToEnd();
          }
        }), (object) 0)), (object) 0);
        return tcs.Task;
      }
      return YouTube.DefaultVersion == 3 ? YouTube.RemoveFromPlaylist(YouTube.UserInfo.WatchLaterPlaylist, playlistVideoID) : Task.Run<bool>((Func<bool>) (() => false));
    }

    public static Task<bool> RemoveFromPlaylist(string playlistID, string playlistVideoID)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/playlists/" + playlistID + "/" + playlistVideoID);
          req.Method = "DELETE";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a => req.BeginGetResponse((AsyncCallback) (a2 =>
          {
            try
            {
              req.EndGetResponse(a2);
              YouTube.Write((object) ("Removed playlist ID " + playlistVideoID + " from playlist " + playlistID));
              tcs.SetResult(true);
            }
            catch (WebException ex)
            {
              tcs.SetException((Exception) ex);
              using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                streamReader.ReadToEnd();
            }
          }), (object) 0)), (object) 0);
          break;
        case 3:
          return Task.Run<bool>((Func<Task<bool>>) (async () =>
          {
            YouTube.address = "https://www.googleapis.com/youtube/v3/playlistItems?key=" + YouTube.APIKey + "&id=" + playlistVideoID + "&access_token=" + YouTube.AccessToken;
            HttpClient httpClient = new HttpClient(YouTube.HttpFilter);
            httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
            try
            {
              HttpResponseMessage mess = await httpClient.DeleteAsync(new Uri(YouTube.address));
              if (!mess.IsSuccessStatusCode)
              {
                string str = await mess.Content.ReadAsStringAsync();
                YouTube.Write((object) nameof (RemoveFromPlaylist), (object) ("Remove from playlist failed failed with status code " + (object) mess.StatusCode + ": " + str));
                throw new InvalidOperationException("Remove from playlist failed failed with status code " + (object) mess.StatusCode + ": " + str);
              }
              mess = (HttpResponseMessage) null;
            }
            catch
            {
              throw;
            }
            return true;
          }));
      }
      return tcs.Task;
    }

    public static Task<bool> WatchLater(string videoID, int index = -1)
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      switch (YouTube.DefaultVersion)
      {
        case 2:
          XDocument x = new XDocument();
          XNamespace xnamespace1 = (XNamespace) "http://www.w3.org/2005/Atom";
          XNamespace xnamespace2 = (XNamespace) "http://gdata.youtube.com/schemas/2007";
          XElement content1 = new XElement(xnamespace1 + "entry", (object) new XAttribute(XNamespace.Xmlns + "yt", (object) xnamespace2.NamespaceName));
          XElement content2 = new XElement(xnamespace1 + "id");
          XElement content3 = new XElement(xnamespace2 + "position")
          {
            Value = "1"
          };
          content2.Value = videoID;
          content1.Add((object) content2);
          content1.Add((object) content3);
          x.Add((object) content1);
          HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://gdata.youtube.com/feeds/api/users/default/watch_later");
          req.Method = "POST";
          req.ContentType = "application/atom+xml";
          req.Headers["Authorization"] = "Bearer " + YouTube.accessToken;
          req.Headers["GData-Version"] = "2.1";
          req.Headers["X-GData-Key"] = "key=" + YouTube.DeveloperKey;
          req.BeginGetRequestStream((AsyncCallback) (a =>
          {
            using (StreamWriter streamWriter = new StreamWriter(req.EndGetRequestStream(a)))
            {
              streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
              streamWriter.WriteLine(x.ToString());
            }
            req.BeginGetResponse((AsyncCallback) (a2 =>
            {
              try
              {
                req.EndGetResponse(a2);
                tcs.SetResult(true);
              }
              catch (WebException ex)
              {
                tcs.SetException((Exception) ex);
              }
            }), (object) 0);
          }), (object) 0);
          break;
        case 3:
          return YouTube.AddToPlaylist(YouTube.UserInfo.WatchLaterPlaylist, videoID, index);
      }
      return tcs.Task;
    }

    public static void InsertSubscription(UserInfo info)
    {
      if (info.SubscriptionID == null)
        throw new NullReferenceException("UserInfo has no subscription ID");
      YouTube.SubscriptionIDs.Add(info.ID, info.SubscriptionID);
      YouTube.Subscriptions.Add(info.UserDisplayName);
      YouTube.SubscriptionData.Add(info);
      YouTube.SubscriptionData.Sort();
      YouTube.CallSubscriptionsLoaded();
    }

    public static Task<bool> GetSubscriptions()
    {
      YouTube.Subscriptions.Clear();
      YouTube.SubscriptionIDs.Clear();
      YouTube.SubscriptionData.Clear();
      if (YouTube.subsLoadedTcs.Task.IsCompleted || YouTube.subsLoadedTcs.Task.IsCanceled || YouTube.subsLoadedTcs.Task.IsFaulted)
        YouTube.subsLoadedTcs = new TaskCompletionSource<bool>();
      YouTube.SubscriptionsAreLoaded = false;
      if (YouTube.SubscriptionsUnloaded != null)
        YouTube.SubscriptionsUnloaded((object) null, new EventArgs());
      switch (YouTube.DefaultVersion)
      {
        case 2:
          string str1 = "https://gdata.youtube.com/feeds/api/users/default/subscriptions?v=2.1&access_token=" + YouTube.accessToken;
          int page1 = 0;
          HttpClient wc = new HttpClient(YouTube.HttpFilter);
          Task.Run((Func<Task>) (async () =>
          {
            string uriString = "https://gdata.youtube.com/feeds/api/users/default/subscriptions?v=2.1&start-index=" + (object) (25 * page1 + 1) + "&how-many=25&access_token=" + YouTube.accessToken;
            string text = await wc.GetStringAsync(new Uri(uriString, UriKind.Absolute));
            while (text != null)
            {
              try
              {
                XElement[] array = XDocument.Parse(text).Element(YouTube.Atom("feed")).Descendants(YouTube.Atom("entry")).ToArray<XElement>();
                YouTube.Write((object) nameof (GetSubscriptions), (object) ("Found " + (object) array.Length + " subscriptions"));
                if (array != null && array.Length != 0)
                {
                  for (int index1 = 0; index1 < array.Length; ++index1)
                  {
                    XElement entry = array[index1];
                    try
                    {
                      string key1 = entry.Element(YouTube.Atom("username", "yt")).Value;
                      YouTube.Subscriptions.Add(key1);
                      try
                      {
                        string key2 = entry.Element(YouTube.Atom("channelId", "yt")).Value;
                        string str2 = ((IEnumerable<string>) entry.Element(YouTube.Atom("id")).Value.Split(':')).Last<string>();
                        string str3;
                        try
                        {
                          str3 = entry.Element(YouTube.Atom("username", "yt")).Attribute((XName) "display").Value;
                        }
                        catch
                        {
                          str3 = key1;
                        }
                        YouTube.SubscriptionIDs.Add(key2, str2);
                        UserInfo userInfo;
                        if (Debugger.IsAttached)
                          userInfo = new UserInfo()
                          {
                            ID = key2,
                            UserName = key1,
                            UserDisplayName = str3
                          };
                        else
                          userInfo = new UserInfo(entry)
                          {
                            ID = key2,
                            UserName = key1,
                            UserDisplayName = str3
                          };
                        YouTube.SubscriptionData.Add(userInfo);
                      }
                      catch (Exception ex)
                      {
                        YouTube.Write((object) nameof (GetSubscriptions), (object) ("Exception getting subscription info: " + (object) ex));
                        YouTube.SubscriptionIDs.Add(key1, key1);
                      }
                    }
                    catch
                    {
                    }
                  }
                  page1++;
                  text = await wc.GetStringAsync(new Uri("https://gdata.youtube.com/feeds/api/users/default/subscriptions?v=2.1&start-index=" + (object) (25 * page1 + 1) + "&how-many=25&access_token=" + YouTube.accessToken, UriKind.Absolute));
                }
                else
                {
                  YouTube.Write((object) nameof (GetSubscriptions), (object) "Completed, sorting subscriptions list");
                  YouTube.Subscriptions.Sort();
                  YouTube.SubscriptionData.Sort();
                  if (YouTube.SubscriptionsLoaded != null)
                    YouTube.SubscriptionsLoaded((object) null, new EventArgs());
                  text = (string) null;
                  YouTube.subsLoadedTcs.TrySetResult(true);
                }
              }
              catch
              {
                text = (string) null;
                YouTube.subsLoadedTcs.TrySetResult(false);
              }
            }
          }));
          break;
        case 3:
          HttpClient client = new HttpClient(YouTube.HttpFilter);
          client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
          int maxResults = 50;
          URLConstructor url = new URLConstructor("https://www.googleapis.com/youtube/v3/subscriptions");
          url["mine"] = "true";
          url["maxResults"] = maxResults.ToString();
          url["order"] = "alphabetical";
          url["access_token"] = YouTube.AccessToken;
          url["part"] = "snippet,contentDetails";
          url["fields"] = "items(id,contentDetails,snippet(resourceId,title, thumbnails)),nextPageToken";
          int page2 = 0;
          int index = 0;
          Task.Run((Func<Task>) (async () =>
          {
            try
            {
              HttpResponseMessage mess = await client.GetAsync(url.ToUri(UriKind.Absolute));
              if (!mess.IsSuccessStatusCode)
              {
                if (mess.Content != null)
                {
                  string str4 = await mess.Content.ReadAsStringAsync();
                  YouTube.ReportError(new YouTubeError()
                  {
                    Method = "GetSubscriptions()",
                    Action = "Getting list of subscribed channels",
                    OtherInfo = new Dictionary<string, object>()
                    {
                      {
                        "Uri",
                        (object) url.ToString()
                      }
                    }
                  });
                  throw new Exception("Request failed with error code " + (object) mess.StatusCode + ", and error message: \n" + str4);
                }
                throw new Exception("Request failed with error code " + (object) mess.StatusCode);
              }
              string json1 = await mess.Content.ReadAsStringAsync();
              while (json1 != null)
              {
                JObject jobject = JObject.Parse(json1);
                JArray jarray = (JArray) jobject["items"];
                for (int index2 = 0; index2 < jarray.Count; ++index2)
                {
                  JToken json2 = jarray[index2];
                  JToken jtoken = json2[(object) "snippet"];
                  bool flag = false;
                  if (jtoken != null)
                  {
                    string str5 = (string) json2[(object) "id"];
                    string key = (string) null;
                    if (jtoken[(object) "resourceId"] != null)
                      key = (string) jtoken[(object) "resourceId"][(object) "channelId"];
                    if (str5 != null && key != null && !YouTube.SubscriptionIDs.ContainsKey(key))
                    {
                      flag = true;
                      YouTube.SubscriptionIDs.Add(key, str5);
                    }
                  }
                  if (flag)
                  {
                    UserInfo userInfo = UserInfo.FromSubscriptionData(json2);
                    userInfo.NeedsRefresh = true;
                    YouTube.SubscriptionData.Add(userInfo);
                    YouTube.Subscriptions.Add(userInfo.UserDisplayName);
                  }
                }
                page2++;
                index += jarray.Count;
                string str6 = (string) jobject["nextPageToken"] ?? YouTube.GetStandardPageToken(page2, maxResults);
                if (str6 != null && jarray.Count > 0)
                {
                  url["pageToken"] = str6;
                  json1 = await client.GetStringAsync(url.ToUri(UriKind.Absolute));
                }
                else
                {
                  YouTube.Write((object) "Finished getting subscriptions");
                  YouTube.SubscriptionsAreLoaded = true;
                  YouTube.subsLoadedTcs.TrySetResult(true);
                  json1 = (string) null;
                  if (YouTube.SubscriptionsLoaded != null)
                    YouTube.SubscriptionsLoaded((object) null, new EventArgs());
                  client.Dispose();
                }
              }
              mess = (HttpResponseMessage) null;
            }
            catch (Exception ex)
            {
              HttpRequestException requestException = ex as HttpRequestException;
              YouTube.subsLoadedTcs.TrySetResult(false);
              client.Dispose();
            }
          }));
          break;
      }
      return YouTube.subsLoadedTcs.Task;
    }

    public static string GetLinkFromTag(string s)
    {
      string linkFromTag = "";
      bool flag1 = false;
      bool flag2 = false;
      if (!s.Contains("src"))
        throw new NullReferenceException("Does not contain src element");
      for (int index = s.IndexOf("src"); index < s.Length && !flag2; ++index)
      {
        if (s[index] == '"')
        {
          if (flag1)
            flag2 = true;
          flag1 = !flag1;
        }
        else if (flag1)
          linkFromTag += s[index].ToString();
      }
      return linkFromTag;
    }

    public static string Decode2(string info)
    {
      info = info.Replace("&3A", ":");
      info = info.Replace("%3A", ":");
      info = info.Replace("%2F", "/");
      info = info.Replace("&2F", "/");
      info = info.Replace("sig=", "signature=");
      info = info.Replace("&3B", ";");
      return info;
    }

    public static string Decode(string info)
    {
      info = info.Replace("%2F", "/");
      info = info.Replace("&2F", "/");
      info = info.Replace("%2B", "_");
      info = info.Replace("%25", "&");
      info = info.Replace("%26", "&");
      info = info.Replace("&26", "&");
      info = info.Replace("%3D", "=");
      info = info.Replace("&3D", "=");
      info = info.Replace("%3F", "?");
      info = info.Replace("&3F", "?");
      info = info.Replace("&252C", "%2C");
      info = info.Replace("&22", "\"");
      info = info.Replace("&2C", ",");
      info = info.Replace("&3B", ";");
      return info;
    }

    public static string FullDecode(string info)
    {
      info = info.Replace("&3A", ":");
      info = info.Replace("%3A", ":");
      info = info.Replace("%2F", "/");
      info = info.Replace("&2F", "/");
      info = info.Replace("%2B", "_");
      info = info.Replace("%25", "&");
      info = info.Replace("%26", "&");
      info = info.Replace("&26", "&");
      info = info.Replace("%3D", "=");
      info = info.Replace("&3D", "=");
      info = info.Replace("%3F", "?");
      info = info.Replace("&3F", "?");
      info = info.Replace("&252C", "%2C");
      info = info.Replace("&22", "\"");
      info = info.Replace("&2C", ",");
      info = info.Replace("&3B", ";");
      info = info.Replace("sig=", "signature=");
      info = info.Replace("sig=", "signature=");
      return info;
    }

    public static string LightDecode(string info)
    {
      info = info.Replace("%25", "&");
      info = info.Replace("%26", "&");
      info = info.Replace("&26", "&");
      info = info.Replace("%3D", "=");
      info = info.Replace("&3D", "=");
      info = info.Replace("%3F", "?");
      info = info.Replace("&3F", "?");
      info = info.Replace("%2C", "&");
      info = info.Replace("&252C", "%2C");
      info = info.Replace("&22", "\"");
      info = info.Replace("&2C", ",");
      info = info.Replace("&3B", ";");
      return info;
    }

     //TODO
    public static async Task<Uri> GetUploadUri(
      YouTubeEntry entry,
      string contentType,
      ulong contentLength)
    {
            // ISSUE: unable to decompile the method.
            return default;
    }

    static YouTube()
    {
      HttpBaseProtocolFilter baseProtocolFilter = new HttpBaseProtocolFilter();
      baseProtocolFilter.MaxConnectionsPerServer = 128U;
      baseProtocolFilter.AutomaticDecompression = true;
      YouTube.httpFilter = baseProtocolFilter;
      YouTube.Debug = true;
      YouTube.DefaultVersion = 3;
      YouTube.signedIn = false;
      YouTube.res = "";
      YouTube.standardPageTokens = new Dictionary<int, string>()
      {
        {
          1,
          "CAUQAA"
        },
        {
          2,
          "CAoQAA"
        },
        {
          3,
          "CA8QAA"
        },
        {
          4,
          "CBQQAA"
        },
        {
          5,
          "CBkQAA"
        },
        {
          6,
          "CB4QAA"
        },
        {
          7,
          "CCMQAA"
        },
        {
          8,
          "CCgQAA"
        },
        {
          9,
          "CC0QAA"
        },
        {
          10,
          "CDIQAA"
        },
        {
          11,
          "CDcQAA"
        },
        {
          12,
          "CDwQAA"
        },
        {
          13,
          "CEEQAA"
        },
        {
          14,
          "CEYQAA"
        },
        {
          15,
          "CEsQAA"
        },
        {
          16,
          "CFAQAA"
        },
        {
          17,
          "CFUQAA"
        },
        {
          18,
          "CFoQAA"
        },
        {
          19,
          "CF8QAA"
        },
        {
          20,
          "CGQQAA"
        }
      };
      YouTube.tokenChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
      YouTube.secondTokenCharsUnder128 = "ABCDEFGH";
      YouTube.secondTokenChars = "IJKLMNOP";
      YouTube.middleTokenChars = "AEIMQUYcgkosw048";
      YouTube.signInSemaphore = new SemaphoreSlim(1, 1);
    }
  }
}
