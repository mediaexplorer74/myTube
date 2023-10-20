// Decompiled with JetBrains decompiler
// Type: RykenTube.EntryClient`1
// Assembly: RykenTubeWinRT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 90ADCE83-9478-4D5C-B501-F15F53E219D5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\RykenTubeWinRT.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace RykenTube
{
  public abstract class EntryClient<T> : PartsHandler where T : ClientDataBase
  {
    private static HttpClient client = new HttpClient(YouTube.HttpFilter);
    private GroupCacheInfo cacheInfo = new GroupCacheInfo()
    {
      MaxItems = 500,
      MaxAge = TimeSpan.FromDays(3.0)
    };
    private string cacheGroupName;
    private bool useCache;
    private int version = 3;

    public string Fields { get; set; }

    public bool UseFields { get; set; }

    public bool UseAccessToken { get; set; }

    public bool UseAPIKey { get; set; }

    public string APIKey { get; set; }

    public int MaxItems { get; set; } = 40;

    public EntryClient()
    {
      this.UseAccessToken = true;
      this.UseFields = false;
      this.UseAPIKey = true;
    }

    public EntryClient(int version)
      : this()
    {
      this.version = version;
    }

    public void UseCache(string groupName, GroupCacheInfo cacheInfo)
    {
      if (string.IsNullOrWhiteSpace(groupName))
        throw new ArgumentException("Cache group name cannot be null or empty");
      this.useCache = true;
      this.cacheGroupName = groupName;
      this.cacheInfo = cacheInfo;
    }

    private URLConstructor setBaseAddress(params string[] ids)
    {
      URLConstructor url = new URLConstructor();
      this.SetBaseAddress(url);
      url.SetValue("part", (object) this.PartsToString());
      if (this.UseFields && !string.IsNullOrEmpty(this.Fields))
        url["fields"] = this.Fields;
      else
        url.RemoveValue("fields");
      if (this.UseAccessToken && YouTube.AccessToken != null)
        url["access_token"] = YouTube.AccessToken;
      else
        url.RemoveValue("access_token");
      if (this.UseAPIKey)
        url["key"] = this.APIKey ?? YouTube.APIKey;
      else
        url.RemoveValue("key");
      url.SetValue("id", (object) string.Join(",", ids));
      return url;
    }

    public abstract void SetBaseAddress(URLConstructor url);

    public async Task<T> GetInfo(string ID)
    {
      EntryClient<T> entryClient = this;
      try
      {
        T[] batchedInfo = await entryClient.GetBatchedInfo(ID);
        return batchedInfo.Length == 0 ? default (T) : batchedInfo[0];
      }
      catch
      {
        return default (T);
      }
    }

    private string toCacheId(string id)
    {
      string cacheId = id + "pt";
      foreach (Part part in this.Parts)
        cacheId += ((int) part).ToString();
      return cacheId;
    }

    public async Task<JToken> GetJson(string id)
    {
      JArray jarray = (JArray) (await this.GetBatchedJson(id))["items"];
      return jarray.Count < 0 ? (JToken) null : jarray[0];
    }

    public async Task<JObject> GetBatchedJson(params string[] ids)
    {
      try
      {
        List<string> origList = ((IEnumerable<string>) ids).ToList<string>();
        List<string> list = ((IEnumerable<string>) ids).ToList<string>();
        Dictionary<int, JToken> jsonDict = new Dictionary<int, JToken>();
        if (this.useCache && YouTube.CacheHandler != null)
        {
          try
          {
            await YouTube.CacheHandler.EstablishGroup(this.cacheGroupName, this.cacheInfo);
          }
          catch
          {
          }
          List<Task<CacheInfo>> cacheTask = new List<Task<CacheInfo>>();
          for (int index = 0; index < list.Count; ++index)
          {
            string id = list[index];
            cacheTask.Add(YouTube.CacheHandler.LoadCache(this.cacheGroupName, this.toCacheId(id)));
          }
          for (int i = 0; i < list.Count; ++i)
          {
            string id = list[i];
            try
            {
              CacheInfo cacheInfo = await cacheTask[i];
              if (cacheInfo != null)
              {
                if (cacheInfo.Version == this.version)
                {
                  try
                  {
                    JToken jtoken = JToken.Parse(cacheInfo.Text);
                    jsonDict.Add(origList.IndexOf(id), jtoken);
                    list.RemoveAt(i);
                    cacheTask.RemoveAt(i);
                    --i;
                  }
                  catch
                  {
                  }
                }
              }
            }
            catch
            {
            }
            id = (string) null;
          }
          cacheTask = (List<Task<CacheInfo>>) null;
        }
        if (list.Count > 0)
        {
          string.Join(",", (IEnumerable<string>) list);
          URLConstructor url = this.setBaseAddress(list.ToArray());
          string json1 = (string) null;
          ((ICollection<HttpProductInfoHeaderValue>) EntryClient<T>.client.DefaultRequestHeaders.UserAgent).Clear();
          EntryClient<T>.client.DefaultRequestHeaders.UserAgent.TryParseAdd(YouTube.UserAgent);
          HttpResponseMessage response = (HttpResponseMessage) null;
          try
          {
            response = await EntryClient<T>.client.GetAsync(url.ToUri(UriKind.Absolute));
            if (response.IsSuccessStatusCode)
            {
              json1 = await response.Content.ReadAsStringAsync();
            }
            else
            {
              string str = await response.Content.ReadAsStringAsync();
              throw new Exception("Request failed with status code " + (object) response.StatusCode + " and error message:\n" + str + "\nAddress: " + url.ToString());
            }
          }
          catch
          {
            throw;
          }
          finally
          {
            ((IDisposable) response?.Content)?.Dispose();
            response?.Dispose();
          }
          JObject json = JObject.Parse(json1);
          try
          {
            JArray items = (JArray) json["items"];
            if (this.useCache && YouTube.CacheHandler != null)
            {
              foreach (JToken jtoken in items)
              {
                try
                {
                  string id = (string) jtoken[(object) "id"];
                  if (jtoken != null)
                  {
                    int num = await YouTube.CacheHandler.SaveCache(new CacheInfo()
                    {
                      Version = this.version,
                      Text = jtoken.ToString(),
                      Group = this.cacheGroupName,
                      Name = this.toCacheId(id)
                    }) ? 1 : 0;
                  }
                }
                catch
                {
                }
              }
            }
            foreach (KeyValuePair<int, JToken> keyValuePair in jsonDict)
            {
              try
              {
                int index = keyValuePair.Key;
                if (index < 0)
                  index = 0;
                if (index > items.Count)
                  index = items.Count;
                items.Insert(index, keyValuePair.Value);
              }
              catch
              {
              }
            }
            items = (JArray) null;
          }
          catch
          {
          }
          return json;
        }
        JObject batchedJson = new JObject();
        JArray jarray = new JArray();
        foreach (KeyValuePair<int, JToken> keyValuePair in jsonDict)
          jarray.Add(keyValuePair.Value);
        batchedJson.Add("items", (JToken) jarray);
        return batchedJson;
      }
      catch (Exception ex)
      {
        YouTube.Write((object) nameof (EntryClient<T>), (object) string.Format("GetBatchedJson failed with exception:\n {0}", (object) ex));
        throw;
      }
    }

    public async Task<T[]> GetBatchedInfo(params string[] ids)
    {
      if (this.version != 3)
        return (T[]) null;
      if (ids.Length <= this.MaxItems)
        return await this.getBatchedInfo(ids);
      List<Task<T[]>> tasks = new List<Task<T[]>>();
      int sourceIndex = 0;
      for (; sourceIndex < ids.Length; sourceIndex += this.MaxItems)
      {
        int num = sourceIndex + this.MaxItems;
        if (num > ids.Length)
          num = ids.Length;
        int length = num - sourceIndex;
        string[] destinationArray = new string[length];
        Array.Copy((Array) ids, sourceIndex, (Array) destinationArray, 0, length);
        tasks.Add(this.getBatchedInfo(destinationArray));
      }
      T[][] objArray1 = await Task.WhenAll<T[]>((IEnumerable<Task<T[]>>) tasks);
      List<T> items = new List<T>();
      foreach (Task<T[]> task in tasks)
      {
        T[] objArray2 = await task;
        if (objArray2 != null)
        {
          foreach (T obj in objArray2)
            items.Add(obj);
        }
      }
      return items.ToArray();
    }

    private async Task<T[]> getBatchedInfo(params string[] ids)
    {
      try
      {
        JArray array = (JArray) (await this.GetBatchedJson(ids))["items"];
        T[] items = new T[array.Count];
        for (int i = 0; i < items.Length; ++i)
        {
          T v3 = await this.ParseV3(array[i]);
          items[i] = v3;
          v3.NeedsRefresh = false;
        }
        return items;
      }
      catch (Exception ex)
      {
        YouTube.Write((object) nameof (EntryClient<T>), (object) string.Format("GetBatchedInfo failed with exception:\n {0}", (object) ex));
        return (T[]) null;
      }
    }

    public abstract Task<T> ParseV3(JToken token);
  }
}
