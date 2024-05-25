// myTube.ChannelNotifications

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RykenTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace myTube
{
  public class ChannelNotifications
  {
    [JsonIgnore]
    public const string FileName = "ChannelNotifications.json";
    [JsonIgnore]
    public const string FileNameForFoundVideos = "NotifiedAbout.json";
    private Dictionary<string, string> channelIds;
    private Dictionary<string, string> lastSaveIds;
    private StorageFolder loadedFromFolder;
    private string loadedFromFileName;

    [JsonIgnore]
    public int Count => this.channelIds.Count;

    public bool Changed
    {
      get
      {
        if (this.lastSaveIds.Count != this.channelIds.Count)
          return true;
        foreach (KeyValuePair<string, string> channelId in this.channelIds)
        {
          if (!this.lastSaveIds.ContainsKey(channelId.Key) || this.lastSaveIds[channelId.Key] != channelId.Value)
            return true;
        }
        foreach (KeyValuePair<string, string> lastSaveId in this.lastSaveIds)
        {
          if (!this.channelIds.ContainsKey(lastSaveId.Key) || this.channelIds[lastSaveId.Key] != lastSaveId.Value)
            return true;
        }
        return false;
      }
    }

    private ChannelNotifications()
    {
      this.channelIds = new Dictionary<string, string>();
      this.lastSaveIds = new Dictionary<string, string>();
    }

    public bool RemoveItemsNotIncludedInOtherCollection(ChannelNotifications n)
    {
      bool flag = false;
      string[] array = new string[this.channelIds.Count];
      this.channelIds.Keys.CopyTo(array, 0);
      for (int index = 0; index < array.Length; ++index)
      {
        if (!n.channelIds.ContainsKey(array[index]))
        {
          this.RemoveChannel(array[index]);
          flag = true;
        }
      }
      return flag;
    }

    public string[] GetIds()
    {
      string[] array = this.channelIds.Keys.ToArray<string>();
      string[] ids = new string[this.channelIds.Count];
      for (int index = 0; index < ids.Length; ++index)
        ids[index] = "UC" + array[index];
      return ids;
    }

    public string GetName(string channelId)
    {
      string key = UserInfo.RemoveUCFromID(channelId);
      return this.channelIds.ContainsKey(key) ? this.channelIds[key] : "";
    }

    public void AddChannel(string channelId, string title)
    {
      string key = UserInfo.RemoveUCFromID(channelId);
      if (this.channelIds.ContainsKey(key))
        return;
      this.channelIds.Add(key, title);
    }

    public bool AddOrModifyChannel(string channelId, string title)
    {
      string key = UserInfo.RemoveUCFromID(channelId);
      if (!this.channelIds.ContainsKey(key))
      {
        this.channelIds.Add(key, title);
        return true;
      }
      if (!(this.channelIds[key] != title))
        return false;
      this.channelIds[key] = title;
      return true;
    }

    public void RemoveChannel(string channelId)
    {
      string key = UserInfo.RemoveUCFromID(channelId);
      if (!this.channelIds.ContainsKey(key))
        return;
      this.channelIds.Remove(key);
    }

    public bool ContainsChannel(string channelId) => this.channelIds.ContainsKey(UserInfo.RemoveUCFromID(channelId));

    private void setLastSaveIds()
    {
      this.lastSaveIds.Clear();
      foreach (KeyValuePair<string, string> channelId in this.channelIds)
        this.lastSaveIds.Add(channelId.Key, channelId.Value);
    }

    public Task Save(string fileName = "ChannelNotifications.json") => this.Save(ApplicationData.Current.RoamingFolder, fileName);

    public async Task Save(StorageFolder folder, string fileName = "ChannelNotifications.json")
    {
      try
      {
        await FileIO.WriteTextAsync((IStorageFile) 
            await folder.CreateFileAsync(fileName, (CreationCollisionOption) 1), 
            JObject.FromObject((object) this.channelIds).ToString());
      }
      catch
      {
      }
    }

    public static Task<ChannelNotifications> Load(string fileName = "ChannelNotifications.json") => ChannelNotifications.Load(ApplicationData.Current.RoamingFolder, fileName);

    public static async Task<ChannelNotifications> Load(StorageFolder folder, string fileName = "ChannelNotifications.json")
    {
      ChannelNotifications not = new ChannelNotifications();
      not.channelIds = await ChannelNotifications.loadItems(folder, fileName);
      not.setLastSaveIds();
      not.loadedFromFileName = fileName;
      not.loadedFromFolder = folder;
      return not;
    }

    private static async Task<Dictionary<string, string>> loadItems(
      StorageFolder folder,
      string fileName)
    {
      try
      {
        return JObject.Parse(await FileIO.ReadTextAsync((IStorageFile) await folder.GetFileAsync(fileName))).ToObject<Dictionary<string, string>>();
      }
      catch
      {
        return new Dictionary<string, string>();
      }
    }

    public async Task Reload()
    {
      if (this.loadedFromFolder == null || string.IsNullOrWhiteSpace(this.loadedFromFileName))
        return;
      this.channelIds = await ChannelNotifications.loadItems(this.loadedFromFolder, this.loadedFromFileName);
      this.setLastSaveIds();
    }
  }
}
