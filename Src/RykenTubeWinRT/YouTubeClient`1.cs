// Decompiled with JetBrains decompiler
// Type: RykenTube.YouTubeClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

//RnD
using HttpClient = Windows.Web.Http.HttpClient;
using HttpResponseMessage = Windows.Web.Http.HttpResponseMessage;

namespace RykenTube
{
  public abstract class YouTubeClient<T> : PartsHandler, IDisposable where T : ClientData<T>
  {
    public string RefreshClientFields;
    private int lastPage;
    private bool busy;
    private HttpClient client;
    private HttpClient netClient;
    public string Fields;
    public string FieldsV3;
    private Dictionary<int, string> pageTokens = new Dictionary<int, string>();
    private bool useCache;
    private GroupCacheInfo groupCache;
    private string groupName;
    public bool UseFields = true;
    public bool UseRandomQuery = true;
    protected int howMany;
    public string AccessToken;
    public string APIKey;
    public bool UseAccessToken = true;
    public bool NeedsRefresh;
    public bool UseAPIKey;
    public bool ForceAPIKeyInV3 = true;
    public URLConstructor BaseAddress;
    protected int version = 3;
    private bool useWindowsWebClient = true;
    public EntryClient<T> RefreshClientOverride;

    public bool UseFieldsOnRefreshClient { get; set; }

    public bool IsBusy => this.busy;

    public Func<JToken, bool> JsonFilter { get; set; }

    public Func<T, bool> ItemFilter { get; set; }

    public bool Mine { get; protected set; }

    public bool SetPart { get; set; }

    public string UserAgent { get; set; }

    public bool UsePublishedAfter { get; set; }

    public bool RequiresSignIn { get; set; }

    public bool RefreshInternally { get; set; }

    public DateTime PublishedAfter { get; set; }

    public TimeSpan Timeout { get; set; }

    public bool IsPlaylist { get; set; }

    public bool CacheRequiresSameAccount { get; set; }

    public string DefaultCacheGroupName { get; set; }

    public void SetHowMany(int howMany)
    {
      this.howMany = howMany;
      this.pageTokens.Clear();
    }

    public YouTubeClient<T> GetDuplicate()
    {
      YouTubeClient<T> duplicateOverride = this.GetDuplicateOverride();
      if (duplicateOverride == null)
        return this;
      duplicateOverride.JsonFilter = this.JsonFilter;
      duplicateOverride.ItemFilter = this.ItemFilter;
      duplicateOverride.Mine = this.Mine;
      duplicateOverride.ClearParts(this.Parts.ToArray());
      duplicateOverride.IsPlaylist = true;
      duplicateOverride.CacheRequiresSameAccount = this.CacheRequiresSameAccount;
      duplicateOverride.Fields = this.Fields;
      duplicateOverride.FieldsV3 = this.FieldsV3;
      duplicateOverride.ForceAPIKeyInV3 = this.ForceAPIKeyInV3;
      duplicateOverride.AccessToken = this.AccessToken;
      duplicateOverride.APIKey = this.APIKey;
      duplicateOverride.RefreshInternally = this.RefreshInternally;
      duplicateOverride.UseFieldsOnRefreshClient = this.UseFieldsOnRefreshClient;
      duplicateOverride.UsePublishedAfter = this.UsePublishedAfter;
      duplicateOverride.UserAgent = this.UserAgent;
      duplicateOverride.UseRandomQuery = this.UseRandomQuery;
      duplicateOverride.PublishedAfter = this.PublishedAfter;
      return duplicateOverride;
    }

    protected virtual YouTubeClient<T> GetDuplicateOverride() => (YouTubeClient<T>) null;

    public void UseWindowsWebClient() => this.useWindowsWebClient = true;

    public YouTubeClient(int num)
    {
      this.DefaultCacheGroupName = this.GetType().FullName;
      this.SetPart = true;
      this.Timeout = TimeSpan.MaxValue;
      this.howMany = num;
      this.BaseAddress = new URLConstructor();
    }

    public EntryClient<T> GetRefreshClient() => this.RefreshClientOverride ?? this.GetRefreshClientOverride();

    protected abstract EntryClient<T> GetRefreshClientOverride();

    protected virtual string GetCacheName() => (string) null;

    public void UseCache(string groupName, GroupCacheInfo info)
    {
      this.groupCache = info;
      this.groupName = groupName;
      this.useCache = true;
      if (YouTube.CacheHandler == null)
        return;
      YouTube.CacheHandler.EstablishGroup(groupName, info);
    }

    public void UseCache(GroupCacheInfo info) => this.UseCache(this.groupName ?? this.DefaultCacheGroupName, info);

    public void DontCache() => this.useCache = false;

    protected virtual IDictionary<string, string> GetHeaders() => (IDictionary<string, string>) null;

    protected virtual Task<T[]> GetFeedOverride(int page) => (Task<T[]>) null;

    public async Task<T[]> GetFeed(int page)
    {
      YouTubeClient<T> youTubeClient = this;
      if (youTubeClient.RequiresSignIn && string.IsNullOrWhiteSpace(YouTube.AccessToken) && string.IsNullOrWhiteSpace(YouTube.AccessToken))
        throw new InvalidOperationException("Must be signed in to use this client");
      youTubeClient.busy = !youTubeClient.busy ? true : throw new InvalidOperationException("Cannot get feed while the client is busy");
      T[] vals = (T[]) null;
      bool useInternal = true;
      try
      {
        Task<T[]> feedOverride = youTubeClient.GetFeedOverride(page);
        if (feedOverride != null)
        {
          vals = await feedOverride;
          useInternal = false;
        }
      }
      catch
      {
      }
      if (useInternal)
      {
        try
        {
          vals = await youTubeClient.getFeedInternal(page);
        }
        catch
        {
          youTubeClient.busy = false;
          throw;
        }
      }
      youTubeClient.busy = false;
      foreach (T obj in vals)
        obj.Client = youTubeClient;
      return vals;
    }

    public async Task<T[]> getFeedInternal(int page)
    {
      YouTubeClient<T> youTubeClient = this;
      youTubeClient.lastPage = page;
      youTubeClient.SetBaseAddress(page);
      string s = (string) null;
      string cacheName = (string) null;
      if (youTubeClient.useCache && YouTube.CacheHandler != null && (cacheName = youTubeClient.GetCacheName()) != null)
      {
        cacheName = cacheName + "pg" + (object) page + "hm" + (object) youTubeClient.howMany;
        if (youTubeClient.Mine)
          cacheName += "mine";
        cacheName += "pt";
        foreach (int part in youTubeClient.Parts)
          cacheName += part.ToString();
        if (YouTube.IsSignedIn && !string.IsNullOrWhiteSpace(YouTube.RefreshToken) && youTubeClient.UseAccessToken && youTubeClient.CacheRequiresSameAccount)
        {
          cacheName += "rt";
          if (YouTube.RefreshToken.Length > 3)
          {
            cacheName += YouTube.RefreshToken.Substring(0, 3);
            cacheName += YouTube.RefreshToken.Substring(YouTube.RefreshToken.Length - 4, 3);
          }
        }
        try
        {
          CacheInfo cacheInfo = await YouTube.CacheHandler.LoadCache(youTubeClient.groupName, cacheName);
          if (cacheInfo != null)
          {
            if (cacheInfo.Version == youTubeClient.version)
              s = cacheInfo.Text;
          }
        }
        catch
        {
        }
      }
      bool noCache = s == null;
      if (noCache)
        s = await youTubeClient.GetResultString(page);
      if (noCache && youTubeClient.useCache && YouTube.CacheHandler != null && cacheName != null)
      {
        try
        {
          int num = await YouTube.CacheHandler.SaveCache(new CacheInfo()
          {
            Group = youTubeClient.groupName,
            Text = s,
            Name = cacheName,
            Version = youTubeClient.version
          }) ? 1 : 0;
        }
        catch
        {
        }
      }
      return await youTubeClient.StringDownloaded(s);
    }

    protected virtual void SetPagination(int page)
    {
      if (this.howMany != 0 && this.version == 2)
      {
        this.BaseAddress.SetValue("start-index", (object) (page * this.howMany + 1));
        this.BaseAddress.SetValue("max-results", (object) this.howMany);
      }
      else
      {
        this.BaseAddress.RemoveValue("start-index");
        this.BaseAddress.RemoveValue("max-results");
      }
      if (this.howMany != 0 && this.version == 3)
        this.BaseAddress.SetValue("maxResults", (object) this.howMany);
      else
        this.BaseAddress.RemoveValue("maxResults");
      if (this.version != 3)
        return;
      this.setPageToken(page);
    }

    protected virtual void SetBaseAddress(int page)
    {
      this.SetPagination(page);
      if (this.UseAccessToken)
      {
        if (this.AccessToken != null)
          this.BaseAddress.SetValue("access_token", (object) this.AccessToken);
        else if (YouTube.IsSignedIn && YouTube.AccessToken != null)
          this.BaseAddress.SetValue("access_token", (object) YouTube.AccessToken);
        else
          this.BaseAddress.RemoveValue("access_token");
      }
      else
        this.BaseAddress.RemoveValue("access_token");
      if (this.UseFields)
      {
        if (this.version == 2)
        {
          if (this.Fields != null)
            this.BaseAddress.SetValue("fields", (object) this.Fields);
          else
            this.BaseAddress.RemoveValue("fields");
        }
        else if (this.version == 3)
        {
          if (this.FieldsV3 != null)
            this.BaseAddress.SetValue("fields", (object) this.FieldsV3);
          else
            this.BaseAddress.RemoveValue("fields");
        }
        else
          this.BaseAddress.RemoveValue("fields");
      }
      else
        this.BaseAddress.RemoveValue("fields");
      if (this.UseRandomQuery)
        this.BaseAddress.SetValue("rand", (object) DateTime.Now.Ticks);
      else
        this.BaseAddress.RemoveValue("rand");
      if (this.version == 3)
      {
        if (this.SetPart)
          this.BaseAddress.SetValue("part", (object) this.PartsToString());
        if (this.UsePublishedAfter)
          this.BaseAddress.SetValue("publishedAfter", (object) this.dateToString(this.PublishedAfter));
        else
          this.BaseAddress.RemoveValue("publishedAfter");
        if (this.Mine)
          this.BaseAddress.SetValue("mine", (object) "true");
        else
          this.BaseAddress.RemoveValue("mine");
      }
      else
        this.BaseAddress.RemoveValue("part");
      if (this.UseAPIKey || this.ForceAPIKeyInV3 && this.version == 3)
      {
        if (this.APIKey != null)
          this.BaseAddress.SetValue("key", (object) this.APIKey);
        else if (YouTube.APIKey != null)
          this.BaseAddress.SetValue("key", (object) YouTube.APIKey);
        else
          this.BaseAddress.RemoveValue("key");
      }
      else
        this.BaseAddress.RemoveValue("key");
    }

    protected virtual async Task<string> GetResultString(int page)
    {
      CancellationTokenSource cancellationTokenSource = (CancellationTokenSource) null;
      if (this.Timeout < TimeSpan.FromMinutes(30.0))
        cancellationTokenSource = new CancellationTokenSource(this.Timeout);
      if (this.useWindowsWebClient)
      {
        if (this.client == null)
          this.client = new HttpClient(YouTube.HttpFilter);
        ((ICollection<HttpExpectationHeaderValue>) this.client.DefaultRequestHeaders.Expect).Clear();
        IDictionary<string, string> headers = this.GetHeaders();
        if (headers != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) headers)
          {
            if (!((IDictionary<string, string>) this.client.DefaultRequestHeaders).ContainsKey(keyValuePair.Key))
              ((ICollection<KeyValuePair<string, string>>) this.client.DefaultRequestHeaders).Add(keyValuePair);
            else
              ((IDictionary<string, string>) this.client.DefaultRequestHeaders)[keyValuePair.Key] = keyValuePair.Value;
          }
        }
        ((ICollection<HttpProductInfoHeaderValue>) this.client.DefaultRequestHeaders.UserAgent).Clear();
        this.client.DefaultRequestHeaders.UserAgent.TryParseAdd(this.UserAgent ?? YouTube.UserAgent);
        IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> async = this.client.GetAsync(new Uri(this.BaseAddress.ToString(), UriKind.Absolute));
        HttpResponseMessage mess;
        if (cancellationTokenSource != null)
          mess = await async.AsTask<HttpResponseMessage, HttpProgress>(cancellationTokenSource.Token);
        else
          mess = await async;
        if (mess.IsSuccessStatusCode)
        {
          string resultString = await mess.Content.ReadAsStringAsync();
          mess.Dispose();
          return resultString;
        }
        string str = "";
        if (mess.Content != null)
          str = await mess.Content.ReadAsStringAsync();
        mess.Dispose();
        throw new Exception(string.Format("Request failed with error code {0}, Error: {1}", (object) mess.StatusCode, (object) str));
      }
      if (this.netClient == null)
      {
        this.netClient = new HttpClient();
        this.netClient.DefaultRequestHeaders.UserAgent.TryParseAdd(this.UserAgent ?? YouTube.UserAgent);
        this.netClient.DefaultRequestHeaders.Expect.Clear();
        
                //RnD
        //this.netClient.DefaultRequestHeaders.ExpectContinue = new bool?(false);
        
                IDictionary<string, string> headers = this.GetHeaders();
        if (headers != null)
        {
          foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) headers)
            this.netClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
    
    HttpResponseMessage mess1;

    if (cancellationTokenSource != null)
    {
        mess1 = await this.netClient.GetAsync(
            new Uri(this.BaseAddress.ToString(), UriKind.Absolute),
            default);//, cancellationTokenSource.Token);
    }
    else
    {
        mess1 = await this.netClient.GetAsync(
            new Uri(this.BaseAddress.ToString(), 
            UriKind.Absolute));
    }
      if (mess1.IsSuccessStatusCode)
      {
        string resultString = await mess1.Content.ReadAsStringAsync();
        mess1.Dispose();
        return resultString;
      }
      string str1 = "";
      if (mess1.Content != null)
        str1 = await mess1.Content.ReadAsStringAsync();
      mess1.Dispose();
      throw new Exception(string.Format("Request failed with error code {0}, Error: {1}", (object) mess1.StatusCode, (object) str1));
    }

    protected virtual async Task<T[]> StringDownloaded(string result)
    {
      EntryClient<T> refreshClient = this.GetRefreshClient();
      if (refreshClient != null)
      {
        refreshClient.Fields = this.RefreshClientFields;
        refreshClient.UseFields = this.UseFieldsOnRefreshClient;
      }
      if (this.version == 2)
        return await this.ParseV2(XDocument.Parse(result));
      if (this.version == 3)
      {
        JObject jo = JObject.Parse(result);
        try
        {
          this.addTokenForNextPage(this.lastPage, (string) jo["nextPageToken"]);
        }
        catch
        {
        }
        if (this.JsonFilter != null)
        {
          JArray jarray = (JArray) jo["items"];
          if (jarray != null)
          {
            for (int index = 0; index < jarray.Count; ++index)
            {
              if (!this.JsonFilter(jarray[index]))
              {
                jarray.RemoveAt(index);
                --index;
              }
            }
          }
        }
        T[] objArray = await this.ParseV3Base(jo);
        if (this.ItemFilter != null)
          objArray = ((IEnumerable<T>) objArray).Where<T>(this.ItemFilter).ToArray<T>();
        return this.RefreshInternally ? await this.InternalRefresh(objArray, refreshClient) : objArray;
      }
      try
      {
        string nextPageTokenOther = this.GetNextPageTokenOther(result);
        if (!string.IsNullOrWhiteSpace(nextPageTokenOther))
          this.addTokenForNextPage(this.lastPage, nextPageTokenOther);
      }
      catch
      {
      }
      T[] objArray1 = await this.ParseOther(result);
      if (this.ItemFilter != null)
        objArray1 = ((IEnumerable<T>) objArray1).Where<T>(this.ItemFilter).ToArray<T>();
      return this.RefreshInternally ? await this.InternalRefresh(objArray1, refreshClient) : objArray1;
    }

    public virtual string GetPageToken(int page) => this.pageTokens.ContainsKey(page) ? this.pageTokens[page] : (string) null;

    protected virtual async Task<T[]> InternalRefresh(T[] items, EntryClient<T> refreshClient)
    {
      List<List<string>> refreshIds = new List<List<string>>();
      foreach (T obj in items)
      {
        if (refreshIds.Count == 0 || refreshIds[refreshIds.Count - 1].Count >= 50)
          refreshIds.Add(new List<string>());
        refreshIds[refreshIds.Count - 1].Add(obj.ID);
      }
      if (refreshIds.Count > 0)
      {
        List<Task<JObject>> tasks = new List<Task<JObject>>();
        foreach (List<string> stringList in refreshIds)
          tasks.Add(refreshClient.GetBatchedJson(stringList.ToArray()));
        try
        {
          int lasti = 0;
          while (refreshIds.Count > 0)
          {
            List<string> stringList = refreshIds[0];
            refreshIds.RemoveAt(0);
            Task<JObject> task = tasks[0];
            tasks.RemoveAt(0);
            JArray jarray = (JArray) (await task)[nameof (items)];
            for (int index = 0; index < jarray.Count; ++index)
            {
              string id = jarray[index].GetValue<string>((string) null, "id");
              T obj = ((IEnumerable<T>) items).FirstOrDefault<T>((Func<T, bool>) (t => t.ID == id));
              if ((object) obj != null)
              {
                obj.SetValuesFromJson(jarray[index]);
                obj.NeedsRefresh = false;
              }
              lasti = index + 1;
            }
          }
          return items;
        }
        catch
        {
        }
        tasks = (List<Task<JObject>>) null;
      }
      return items;
    }

    protected abstract Task<T[]> ParseV2(XDocument xdoc);

    private async Task<T[]> ParseV3Base(JObject jo)
    {
      T[] v3 = await this.ParseV3(jo);
      for (int index = 0; index < v3.Length; ++index)
        v3[index].NeedsRefresh = this.NeedsRefresh;
      return v3;
    }

    protected abstract Task<T[]> ParseV3(JObject jo);

    protected abstract Task<T[]> ParseOther(string s);

    protected abstract string GetNextPageTokenOther(string s);

    private void addTokenForNextPage(int page, string token)
    {
      if (token == null)
        return;
      int key = page + 1;
      if (!this.pageTokens.ContainsKey(key))
        this.pageTokens.Add(key, token);
      else
        this.pageTokens[key] = token;
    }

    public virtual bool CanLoadPage(int page) => page == 0 || this.version == 2 || this.pageTokens.ContainsKey(page);

    private void setPageToken(int page)
    {
      if (this.version == 3)
      {
        if (page == 0)
          this.BaseAddress.RemoveValue("pageToken");
        else if (this.pageTokens.ContainsKey(page))
          this.BaseAddress.SetValue("pageToken", (object) this.pageTokens[page]);
        else
          this.BaseAddress.RemoveValue("pageToken");
      }
      else
        this.BaseAddress.RemoveValue("pageToken");
    }

    private string dateToString(DateTime time)
    {
      object[] objArray = new object[5]
      {
        (object) time.Year,
        (object) "-",
        null,
        null,
        null
      };
      int num;
      string str1;
      if (time.Month >= 10)
      {
        num = time.Month;
        str1 = num.ToString();
      }
      else
      {
        num = time.Month;
        str1 = "0" + num.ToString();
      }
      objArray[2] = (object) str1;
      objArray[3] = (object) "-";
      string str2;
      if (time.Day >= 10)
      {
        num = time.Day;
        str2 = num.ToString();
      }
      else
      {
        num = time.Day;
        str2 = "0" + num.ToString();
      }
      objArray[4] = (object) str2;
      return string.Concat(objArray) + nameof (T) + this.intToTwo(time.Hour) + ":" + this.intToTwo(time.Minute) + ":" + this.intToTwo(time.Second) + "Z";
    }

    private string intToTwo(int i) => i >= 10 ? i.ToString() : "0" + i.ToString();

    public virtual TypeConstructor GetTypeConstructor() => (TypeConstructor) null;

    public bool IsEqualTo(YouTubeClient<T> client)
    {
      if (client == null)
        return false;
      if (client == this)
        return true;
      TypeConstructor typeConstructor1 = this.GetTypeConstructor();
      TypeConstructor typeConstructor2 = client.GetTypeConstructor();
      return typeConstructor1 == null || typeConstructor2 == null || typeConstructor1.ToString() == typeConstructor2.ToString();
    }

    public void Dispose()
    {
      this.client?.Dispose();
      this.netClient?.Dispose();
      this.DisposeOverride();
    }

    public virtual void DisposeOverride()
    {
    }
  }
}
