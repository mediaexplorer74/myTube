// Decompiled with JetBrains decompiler
// Type: myTube.RoamingHistory`2
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace myTube
{
  public class RoamingHistory<TDataType, TClientType>
    where TDataType : ClientDataBase
    where TClientType : EntryClient<TDataType>, new()
  {
    public int MaxEntries = 200;
    public const string SavePath = "History.txt";
    public const string SplitBy = ",";
    private List<string> ids;
    private string openedPath = "History.txt";
    private TClientType client;

    public int Count => this.ids.Count;

    public RoamingHistory()
      : this(new List<string>())
    {
    }

    public RoamingHistory(List<string> ids) => this.ids = ids;

    public void Clear() => this.ids.Clear();

    public bool AddEntry(TDataType entry)
    {
      string id = entry.ID;
      return this.AddEntry(entry.ID);
    }

    public bool AddEntry(string id)
    {
      if (this.ids.FirstOrDefault<string>() == id)
        return false;
      if (this.ids.Contains(id))
        this.ids.Remove(id);
      while (this.ids.Count > this.MaxEntries)
        this.ids.RemoveAt(this.ids.Count - 1);
      this.ids.Insert(0, id);
      return true;
    }

    public bool HasEntry(string id) => this.ids.Contains(id);

    public bool RemoveEntry(TDataType entry) => (object) entry != null && this.ids.Contains(entry.ID) && this.ids.Remove(entry.ID);

    public async Task<TDataType[]> GetEntries(int page)
    {
      try
      {
        await this.Update();
      }
      catch
      {
      }
      int num1 = 20;
      int num2 = num1 * page;
      int length = 10;
      int num3 = this.ids.Count - num2;
      if (num3 <= 0)
        return new TDataType[0];
      if (num3 < num1)
        length = num3;
      string[] strArray = new string[length];
      for (int index = 0; index < strArray.Length; ++index)
        strArray[index] = this.ids[num2 + index];
      if ((object) this.client == null)
        this.client = new TClientType();
      return await this.client.GetBatchedInfo(strArray);
    }

    public string[] GetIds(int page, int howMany)
    {
      int num1 = howMany * page;
      int length = 10;
      int num2 = this.ids.Count - num1;
      if (num2 <= 0)
        return new string[0];
      if (num2 < howMany)
        length = num2;
      string[] ids = new string[length];
      for (int index = 0; index < ids.Length; ++index)
        ids[index] = this.ids[num1 + index];
      return ids;
    }

    public Task Save() => this.Save(this.openedPath);

    public async Task Save(string fileName)
    {
      StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
      if (roamingFolder == null)
        return;
      string s = string.Join(",", (IEnumerable<string>) this.ids);
      await FileIO.WriteTextAsync((IStorageFile) await roamingFolder.CreateFileAsync(fileName, (CreationCollisionOption) 1), s);
      s = (string) null;
    }

    public async Task Update()
    {
      StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
      if (roamingFolder == null)
        return;
      try
      {
        bool changed = false;
        List<string> list = ((IEnumerable<string>) (await FileIO.ReadTextAsync((IStorageFile) await roamingFolder.GetFileAsync(this.openedPath))).Split(",")).ToList<string>();
        if (list.Count == 0)
          return;
        if (this.ids.Count == 0 && list.Count > 0)
        {
          this.ids = list;
          changed = true;
        }
        if (this.ids.Count > 0)
        {
          string id = this.ids[0];
          int num1 = list.IndexOf(id);
          for (int index = 0; index < list.Count; ++index)
          {
            string str = list[index];
            int num2 = -1;
            int num3 = -1;
            if (!this.ids.Contains(str))
            {
              changed = true;
              if (index > 0)
                num2 = this.ids.IndexOf(list[index - 1]);
              if (index < list.Count - 1)
                num3 = this.ids.IndexOf(list[index + 1]);
              if (num2 != -1)
                this.ids.Insert(Math.Max(num2 + 1, this.ids.Count - 1), str);
              else if (num3 != -1)
                this.ids.Insert(Math.Min(num3 - 1, 0), str);
              else if (num1 != -1)
              {
                if (index < num1)
                  this.ids.Insert(Math.Min(0, this.ids.IndexOf(id) - (num1 - index)), str);
                else
                  this.ids.Insert(Math.Max(this.ids.Count - 1, this.ids.IndexOf(id) + (index - num1)), str);
              }
              else
                this.ids.Insert(Math.Min(this.ids.Count - 1, index), str);
            }
          }
        }
        if (!changed)
          return;
        await this.Save();
      }
      catch
      {
      }
    }

    public static Task<RoamingHistory<TDataType, TClientType>> Load() => RoamingHistory<TDataType, TClientType>.Load("History.txt");

    public static async Task<RoamingHistory<TDataType, TClientType>> Load(string filePath)
    {
      StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;
      if (roamingFolder != null)
      {
        try
        {
          return new RoamingHistory<TDataType, TClientType>(((IEnumerable<string>) (await FileIO.ReadTextAsync((IStorageFile) await roamingFolder.GetFileAsync(filePath))).Split(",")).ToList<string>())
          {
            openedPath = filePath
          };
        }
        catch
        {
          return new RoamingHistory<TDataType, TClientType>()
          {
            openedPath = filePath
          };
        }
      }
      else
        return new RoamingHistory<TDataType, TClientType>()
        {
          openedPath = filePath
        };
    }
  }
}
