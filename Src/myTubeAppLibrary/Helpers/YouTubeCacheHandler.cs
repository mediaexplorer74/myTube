// Decompiled with JetBrains decompiler
// Type: myTube.Helpers.YouTubeCacheHandler
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using myTube.Cloud.Data;
using RykenTube;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.System.Threading;

namespace myTube.Helpers
{
  public class YouTubeCacheHandler : ICacheHandler
  {
    private Queue<Task<CacheInfo>> loadingTasks = new Queue<Task<CacheInfo>>();
    private const string Tag = "Cache";
    private const int MaxConcurrentSaves = 4;
    private const int MaxConcurrentLoads = 20;
    private Queue<CacheInfo> saveQueue = new Queue<CacheInfo>();
    private Queue<string> cleanGroups = new Queue<string>();
    private Queue<IAsyncAction> loadTasks = new Queue<IAsyncAction>();
    private object saveLock = new object();
    private const string InfoFileName = "groupCacheInfo.json";
    private const string CacheExtension = ".json";
    private StorageFolder folder;
    private StorageFolder local;
    private Dictionary<string, GroupCacheInfo> groupInfo = new Dictionary<string, GroupCacheInfo>();
    private Dictionary<string, Task> clearTasks = new Dictionary<string, Task>();
    private Dictionary<string, StorageFolder> folders = new Dictionary<string, StorageFolder>();
    private char[] pathChars;
    private char[] fileChars;
    private bool saving;

    public YouTubeCacheHandler() => this.local = ApplicationData.Current.LocalFolder;

    private async Task CreateCacheFolder()
    {
      try
      {
        if (this.folder != null)
          return;
        this.folder = await this.local.CreateFolderAsync("YouTubeCache", (CreationCollisionOption) 3);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    private async Task<StorageFolder> createGroupFolder(string group)
    {
      await this.CreateCacheFolder();
      string origGroup = group;
      if (this.folders.ContainsKey(origGroup))
        return this.folders[origGroup];
      group = this.toPathName(group);
      if (this.folder == null)
        return (StorageFolder) null;
      try
      {
        StorageFolder g = await this.folder.CreateFolderAsync(group, (CreationCollisionOption) 3);
        if (!this.groupInfo.ContainsKey(group))
        {
          try
          {
            GroupCacheInfo groupCacheInfo = DataObject.ToObject<GroupCacheInfo>(await FileIO.ReadTextAsync((IStorageFile) await g.GetFileAsync("groupCacheInfo.json")));
            this.groupInfo.Add(group, groupCacheInfo);
          }
          catch
          {
          }
        }
        try
        {
          this.folders.Add(origGroup, g);
        }
        catch
        {
        }
        return g;
      }
      catch
      {
        return (StorageFolder) null;
      }
    }

    private async Task<StorageFolder> getGroupFolder(string group)
    {
      await this.CreateCacheFolder();
      string origGroup = group;
      if (this.folders.ContainsKey(origGroup))
        return this.folders[origGroup];
      group = this.toPathName(group);
      if (this.folder == null)
        return (StorageFolder) null;
      try
      {
        StorageFolder g = await this.folder.GetFolderAsync(group);
        if (!this.groupInfo.ContainsKey(group))
        {
          try
          {
            GroupCacheInfo groupCacheInfo = DataObject.ToObject<GroupCacheInfo>(await FileIO.ReadTextAsync((IStorageFile) await g.GetFileAsync("groupCacheInfo.json")));
            this.groupInfo.Add(group, groupCacheInfo);
          }
          catch
          {
          }
        }
        try
        {
          this.folders.Add(origGroup, g);
        }
        catch
        {
        }
        return g;
      }
      catch
      {
        return (StorageFolder) null;
      }
    }

    private async Task saveGroupInfo(string group, GroupCacheInfo info)
    {
      StorageFolder groupFolder = await this.createGroupFolder(group);
      if (groupFolder == null)
        return;
      group = this.toPathName(group);
      try
      {
        await FileIO.WriteTextAsync((IStorageFile) await groupFolder.CreateFileAsync("groupCacheInfo.json", (CreationCollisionOption) 1), DataObject.ToJson((object) info));
      }
      catch
      {
      }
    }

    private async Task<GroupCacheInfo> getGroupInfo(string group)
    {
      StorageFolder groupFolder = await this.getGroupFolder(group);
      if (groupFolder != null)
      {
        group = this.toPathName(group);
        try
        {
          return DataObject.ToObject<GroupCacheInfo>(await FileIO.ReadTextAsync((IStorageFile) await groupFolder.GetFileAsync("groupCacheInfo.json")));
        }
        catch
        {
        }
      }
      return (GroupCacheInfo) null;
    }

    private async Task cleanGroup(string group)
    {
      //RnD / TODO

      /*
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_0 cDisplayClass230 = new YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_0();
      Stopwatch watch = new Stopwatch();
      watch.Start();
      string Tag = "CleanCache";
      Helper.Write((object) Tag, (object) ("Cleaning cache for " + group));
      StorageFolder g = await this.getGroupFolder(group);
      Queue<IAsyncAction> fileTasks = new Queue<IAsyncAction>();

      // ISSUE: reference to a compiler-generated field
      cDisplayClass230.deleted = 0;
      if (g != null)
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_1 cDisplayClass231 = new YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_1();
        // ISSUE: reference to a compiler-generated field
        cDisplayClass231.CS\u0024\u003C\u003E8__locals1 = cDisplayClass230;
        // ISSUE: reference to a compiler-generated field
        cDisplayClass231.info = await this.getGroupInfo(group);
        // ISSUE: reference to a compiler-generated field
        if (cDisplayClass231.info != (GroupCacheInfo) null)
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_2 cDisplayClass232 = new YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_2();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass232.CS\u0024\u003C\u003E8__locals2 = cDisplayClass231;
          IReadOnlyList<StorageFile> filesAsync = await g.GetFilesAsync();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass232.files2 = new List<StorageFile>();
          // ISSUE: reference to a compiler-generated field
          cDisplayClass232.count = filesAsync.Count - 1;
          foreach (StorageFile storageFile in (IEnumerable<StorageFile>) filesAsync)
          {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_3 cDisplayClass233 = new YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_3();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass233.CS\u0024\u003C\u003E8__locals3 = cDisplayClass232;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass233.f = storageFile;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!(cDisplayClass233.f.Name == "groupCacheInfo.json") && cDisplayClass233.f != null)
            {
              while (fileTasks.Count >= 4)
              {
                try
                {
                  await fileTasks.Dequeue();
                }
                catch
                {
                }
              }
              try
              {
                // ISSUE: method pointer
                fileTasks.Enqueue(ThreadPool.RunAsync(new WorkItemHandler((object) cDisplayClass233, __methodptr(\u003CcleanGroup\u003Eb__0))));
              }
              catch
              {
              }
              cDisplayClass233 = (YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_3) null;
            }
          }
          while (fileTasks.Count > 0)
          {
            try
            {
              await fileTasks.Dequeue();
            }
            catch
            {
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index = 1; index < cDisplayClass232.files2.Count; ++index)
          {
            if (index >= 1)
            {
              try
              {
                // ISSUE: reference to a compiler-generated field
                StorageFile storageFile1 = cDisplayClass232.files2[index];
                // ISSUE: reference to a compiler-generated field
                StorageFile storageFile2 = cDisplayClass232.files2[index - 1];
                if (storageFile1.DateCreated < storageFile2.DateCreated)
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass232.files2[index] = storageFile2;
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass232.files2[index - 1] = storageFile1;
                  index -= 2;
                }
              }
              catch
              {
              }
            }
          }
          List<IAsyncAction> iasyncActionList = new List<IAsyncAction>();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (cDisplayClass232.count > cDisplayClass232.CS\u0024\u003C\u003E8__locals2.info.MaxItems)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            for (; cDisplayClass232.count > cDisplayClass232.CS\u0024\u003C\u003E8__locals2.info.MaxItems; --cDisplayClass232.count)
            {
              // ISSUE: reference to a compiler-generated field
              if (cDisplayClass232.files2.Count > 0)
              {
                try
                {
                  // ISSUE: reference to a compiler-generated field
                  iasyncActionList.Add(cDisplayClass232.files2[0].DeleteAsync());
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  ++cDisplayClass232.CS\u0024\u003C\u003E8__locals2.CS\u0024\u003C\u003E8__locals1.deleted;
                }
                catch
                {
                  // ISSUE: reference to a compiler-generated field
                  Helper.Write((object) ("Error deleting cache file " + cDisplayClass232.files2[0].DisplayName + " in " + group));
                }
                try
                {
                  // ISSUE: reference to a compiler-generated field
                  cDisplayClass232.files2.RemoveAt(0);
                }
                catch
                {
                }
              }
              else
                break;
            }
          }
          foreach (IAsyncAction iasyncAction in iasyncActionList)
          {
            try
            {
              await iasyncAction;
            }
            catch
            {
            }
          }
          cDisplayClass232 = (YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_2) null;
        }
        cDisplayClass231 = (YouTubeCacheHandler.\u003C\u003Ec__DisplayClass23_1) null;
      }
      watch.Stop();

      // ISSUE: reference to a compiler-generated field
      Helper.Write((object) Tag, (object) ("Finished cleaning cache for " + 
          group + " in " + (object) watch.Elapsed.TotalSeconds +
          " seconds, deleted " + (object) cDisplayClass230.deleted + " files"));
            */
    }

    private string toFileName(string name)
    {
      if (this.fileChars == null)
        this.fileChars = Path.GetInvalidFileNameChars();
      foreach (char fileChar in this.fileChars)
        name = name.Replace(fileChar.ToString(), ".");
      return name;
    }

    private string toPathName(string name)
    {
      if (this.pathChars == null)
        this.pathChars = Path.GetInvalidPathChars();
      foreach (char pathChar in this.pathChars)
        name = name.Replace(pathChar.ToString(), "");
      return name;
    }

    public Task<CacheInfo> LoadCache(string group, string name)
    {
      Task<CacheInfo> task = this.loadCacheInternal(group, name);
      this.loadingTasks.Enqueue(task);
      return task;
    }

    public async Task<CacheInfo> loadCacheInternal(string group, string name)
    {
      name = this.toFileName(name);
      StorageFolder groupFolder = await this.getGroupFolder(group);
      if (groupFolder == null)
        return (CacheInfo) null;
      try
      {
        StorageFile file = await groupFolder.GetFileAsync(name + ".json");
        group = this.toPathName(group);
        GroupCacheInfo info = (GroupCacheInfo) null;
        if (this.groupInfo.ContainsKey(group))
        {
          info = this.groupInfo[group];
        }
        else
        {
          try
          {
            info = await this.getGroupInfo(group);
          }
          catch
          {
          }
        }
        if (!(info != (GroupCacheInfo) null) || !(DateTimeOffset.UtcNow - file.DateCreated > info.MaxAge) && !(file.DateCreated > DateTimeOffset.UtcNow))
          return DataObject.ToObject<CacheInfo>(await FileIO.ReadTextAsync((IStorageFile) file));
        try
        {
          await file.DeleteAsync((StorageDeleteOption) 1);
        }
        catch
        {
        }
        return (CacheInfo) null;
      }
      catch
      {
        return (CacheInfo) null;
      }
    }

    public async Task<bool> SaveCache(CacheInfo cache)
    {
      if (string.IsNullOrWhiteSpace(cache.Group))
        throw new InvalidOperationException("Group cannot be empty");
      if (string.IsNullOrWhiteSpace(cache.Name))
        throw new InvalidOperationException("Name cannot be empty");
      if (string.IsNullOrWhiteSpace(cache.Text))
        throw new InvalidOperationException("Text cannot be empty");
      this.saveQueue.Enqueue(cache);
      lock (this.saveLock)
      {
        if (!this.saving)
          this.startSaving();
      }
      return true;
    }

    private async Task startSaving()
    {
      YouTubeCacheHandler tubeCacheHandler = this;
      if (tubeCacheHandler.saving)
        return;
      tubeCacheHandler.saving = true;
      
      // RnD / TODO      
      // ISSUE: method pointer
      //await ThreadPool.RunAsync(new WorkItemHandler((object) tubeCacheHandler,
      //    __methodptr(\u003CstartSaving\u003Eb__30_0)));
    }

    private async Task startSavingInternal()
    {
      Queue<Task<bool>> saveTasks = new Queue<Task<bool>>();
      int count = 0;
      while (this.saveQueue.Count > 0)
      {
        CacheInfo cache = this.saveQueue.Dequeue();
        if (cache != null)
        {
          while (saveTasks.Count >= 4)
          {
            try
            {
              int num = await saveTasks.Dequeue() ? 1 : 0;
              ++count;
            }
            catch
            {
            }
          }
          while (this.loadingTasks.Count > 0)
          {
            try
            {
              CacheInfo cacheInfo = await this.loadingTasks.Dequeue();
            }
            catch
            {
            }
          }
          try
          {
            if (!this.cleanGroups.Contains(cache.Group))
              this.cleanGroups.Enqueue(cache.Group);
            saveTasks.Enqueue(this.saveCacheInternal(cache));
          }
          catch
          {
          }
          cache = (CacheInfo) null;
        }
      }
      while (saveTasks.Count > 0)
      {
        try
        {
          int num = await saveTasks.Dequeue() ? 1 : 0;
          ++count;
        }
        catch
        {
        }
      }
      if (this.saveQueue.Count > 0)
      {
        this.saving = true;
        try
        {
          await this.startSavingInternal();
        }
        catch
        {
        }
        this.saving = false;
      }
      else
      {
        this.saving = true;
        try
        {
          await this.runCleaningTasks();
        }
        catch
        {
        }
        if (this.saveQueue.Count > 0)
        {
          this.saving = true;
          try
          {
            await this.startSavingInternal();
          }
          catch
          {
          }
          this.saving = false;
        }
        else
          this.saving = false;
      }
    }

    private async Task runCleaningTasks()
    {
      YouTubeCacheHandler tubeCacheHandler = this;
      Queue<IAsyncAction> cleanTasks = new Queue<IAsyncAction>();
      while (tubeCacheHandler.cleanGroups.Count > 0)
      {
        while (cleanTasks.Count >= 4)
        {
          try
          {
            await cleanTasks.Dequeue();
          }
          catch (Exception ex)
          {
            Helper.Write((object) "Cache", (object) "Exception cleaning cache");
            Helper.Write((object) ex);
          }
        }
        try
        {
          if (tubeCacheHandler.cleanGroups.Count > 0)
          {
            //RnD
            /*
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: method pointer
            cleanTasks.Enqueue(ThreadPool.RunAsync(
                new WorkItemHandler((object) new YouTubeCacheHandler.\u003C\u003Ec__DisplayClass32_0()
            {
              \u003C\u003E4__this = tubeCacheHandler,
              g = tubeCacheHandler.cleanGroups.Dequeue()
            }, __methodptr(\u003CrunCleaningTasks\u003Eb__0))));
            */
          }
        }
        catch
        {
        }
      }
      while (cleanTasks.Count > 0)
      {
        try
        {
          await cleanTasks.Dequeue();
        }
        catch (Exception ex)
        {
          Helper.Write((object) "Cache", (object) "Exception cleaning cache");
          Helper.Write((object) ex);
        }
      }
    }

    private async Task<bool> saveCacheInternal(CacheInfo cache)
    {
      StorageFolder groupFolder = await this.getGroupFolder(cache.Group);
      if (groupFolder != null)
      {
        try
        {
          string fileName = this.toFileName(cache.Name);
          string json = DataObject.ToJson((object) cache);
          await FileIO.WriteTextAsync((IStorageFile) await groupFolder.CreateFileAsync(fileName + ".json", (CreationCollisionOption) 1), json);
          return true;
        }
        catch
        {
          Helper.Write((object) "Cache", (object) ("Error saving cache file " + cache.Name));
          return false;
        }
      }
      else
      {
        Helper.Write((object) "Cache", (object) ("Unable to save cache " + cache.Name + " as the group " + cache.Group + " is null"));
        return false;
      }
    }

    public Task RemoveCache(string group, string name) => throw new NotImplementedException();

    public Task RemoveGroup(string group) => throw new NotImplementedException();

    public async Task EstablishGroup(string group, GroupCacheInfo info)
    {
      string group2 = this.toPathName(group);
      if (this.groupInfo.ContainsKey(group2))
      {
        if (this.groupInfo[group2] != info)
        {
          await this.saveGroupInfo(group, info);
          this.groupInfo[group2] = info;
        }
      }
      else
      {
        GroupCacheInfo groupInfo = await this.getGroupInfo(group);
        if (groupInfo == (GroupCacheInfo) null || info != groupInfo)
          await this.saveGroupInfo(group, info);
        try
        {
          this.groupInfo.Add(group2, info);
        }
        catch
        {
          try
          {
            this.groupInfo[group2] = info;
          }
          catch
          {
          }
        }
      }
      group2 = (string) null;
    }
  }
}
