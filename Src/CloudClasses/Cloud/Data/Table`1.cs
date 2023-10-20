// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Data.Table`1
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace myTube.Cloud.Data
{
  public class Table<T> : ITable where T : DataObject
  {
    private uint maxValues = uint.MaxValue;
    public TableOverflowHandling OverflowHandling;
    [JsonIgnore]
    public Func<T, T, bool> SortMethod;
    public const string FileExtension = ".tbl";
    private List<T> items;
    private object itemsLock = new object();
    private string key = "";

    public uint MaxValues
    {
      get => this.maxValues;
      set
      {
        if (value < 0U)
          return;
        this.maxValues = value;
      }
    }

    public int Count => this.Items.Count;

    public List<T> Items
    {
      get => this.items;
      internal set => this.items = value;
    }

    public string Key
    {
      get => this.key;
      set => this.key = value;
    }

    public Table() => this.items = new List<T>();

    public T GetItem(string id)
    {
      IEnumerable<T> source = this.items.Where<T>((Func<T, bool>) (t => t.Id == id));
      return source.Count<T>() != 0 ? source.First<T>() : default (T);
    }

    public bool DeleteItem(string id)
    {
      T obj = this.GetItem(id);
      if ((object) obj == null)
        return false;
      this.items.Remove(obj);
      return true;
    }

    public bool DeleteItems(Func<T, bool> deleteWhere)
    {
      bool flag = false;
      lock (this.itemsLock)
      {
        for (int index = 0; index < this.items.Count; ++index)
        {
          if (deleteWhere(this.items[index]))
          {
            this.items.Remove(this.items[index]);
            --index;
            flag = true;
          }
        }
      }
      return flag;
    }

    public void AddItem(T item)
    {
      this.throwIfTooMany();
      lock (this.itemsLock)
      {
        if (string.IsNullOrWhiteSpace(item.Id))
          item.Id = this.createUniqueGuid();
        else if (this.Exists(item.Id))
          throw new InvalidOperationException("An item (" + (object) typeof (T) + ") with the given GUID already exists");
        item.DateCreated = item.DateModified = DateTime.UtcNow;
        this.items.Add(item);
        this.RemoveExtraItems();
      }
    }

    public void AddOrReplaceItem(T item)
    {
      int index = -1;
      if (string.IsNullOrWhiteSpace(item.Id))
      {
        item.Id = this.createUniqueGuid();
        item.DateCreated = DateTime.UtcNow;
        this.throwIfTooMany();
      }
      else
      {
        T obj;
        if ((object) (obj = this.GetItem(item.Id)) != null)
        {
          index = this.items.IndexOf(obj);
          item.DateCreated = obj.DateCreated;
          lock (this.itemsLock)
            this.items.Remove(obj);
        }
        else
        {
          item.DateCreated = DateTime.UtcNow;
          this.throwIfTooMany();
        }
      }
      item.DateModified = DateTime.UtcNow;
      lock (this.itemsLock)
      {
        if (index == -1)
          this.items.Add(item);
        else
          this.items.Insert(index, item);
      }
      this.RemoveExtraItems();
    }

    public void Save() => Database.SaveTables((ITable) this);

    public void Sort()
    {
      lock (this.itemsLock)
      {
        int num1 = this.items.Count * this.items.Count;
        int num2 = 0;
        if (this.SortMethod == null)
          return;
        for (int index = 0; index < this.items.Count && num2 <= num1; ++index)
        {
          ++num2;
          if (index > 0)
          {
            T obj1 = this.items[index];
            T obj2 = this.items[index - 1];
            if (this.SortMethod(obj2, obj1))
            {
              this.items[index] = obj2;
              this.items[index - 1] = obj1;
              index -= 2;
            }
          }
        }
      }
    }

    private void throwIfTooMany()
    {
      if ((long) this.items.Count >= (long) this.MaxValues && this.OverflowHandling == TableOverflowHandling.ThrowException)
        throw new OverflowException("There are too many items in the table");
    }

    internal void RemoveExtraItems()
    {
      this.Sort();
      if ((long) this.items.Count <= (long) this.MaxValues || this.OverflowHandling != TableOverflowHandling.DeleteOldest)
        return;
      lock (this.itemsLock)
      {
        while ((long) this.items.Count > (long) this.MaxValues)
          this.items.RemoveAt(0);
      }
    }

    public T GetLast()
    {
      lock (this.itemsLock)
        return this.items.LastOrDefault<T>();
    }

    public T[] GetLatest(int number)
    {
      T[] latest = new T[Math.Min(this.items.Count, number)];
      for (int index = 0; index < latest.Length; ++index)
        latest[index] = this.items[this.items.Count - 1 - index];
      return latest;
    }

    public T[] GetOldest(int number)
    {
      T[] oldest = new T[Math.Min(this.items.Count, number)];
      for (int index = 0; index < oldest.Length; ++index)
        oldest[index] = this.items[index];
      return oldest;
    }

    public T[] GetLatest(int number, int page)
    {
      int num = page * number;
      T[] latest = new T[Math.Min(this.items.Count - num, number)];
      for (int index = 0; index < latest.Length; ++index)
        latest[index] = this.items[this.items.Count - 1 - index - num];
      return latest;
    }

    public T[] GetOldest(int number, int page)
    {
      int num = page * number;
      T[] oldest = new T[Math.Min(this.items.Count - num, number)];
      for (int index = 0; index < oldest.Length; ++index)
        oldest[index] = this.items[index + num];
      return oldest;
    }

    public T[] GetLatestWhere(int number, int page, Func<T, bool> whereFunction)
    {
      lock (this.itemsLock)
      {
        T[] array = this.items.Where<T>(whereFunction).ToArray<T>();
        int num = page * number;
        T[] latestWhere = new T[Math.Max(Math.Min(array.Length - num, number), 0)];
        for (int index = 0; index < latestWhere.Length; ++index)
          latestWhere[index] = array[array.Length - 1 - index - num];
        return latestWhere;
      }
    }

    public T[] GetOldestWhere(int number, int page, Func<T, bool> whereFunction)
    {
      lock (this.itemsLock)
      {
        T[] array = this.items.Where<T>(whereFunction).ToArray<T>();
        int num = page * number;
        T[] oldestWhere = new T[Math.Max(Math.Min(array.Length - num, number), 0)];
        for (int index = 0; index < oldestWhere.Length; ++index)
          oldestWhere[index] = array[index + num];
        return oldestWhere;
      }
    }

    public bool Contains(Func<T, bool> containsFunc)
    {
      lock (this.itemsLock)
      {
        foreach (T obj in this.items)
        {
          if (containsFunc(obj))
            return true;
        }
      }
      return false;
    }

    public bool Exists(string ID)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index].Id == ID)
          return true;
      }
      return false;
    }

    public int CountWhere(Func<T, bool> countFunction)
    {
      lock (this.itemsLock)
        return this.items.Count<T>(countFunction);
    }

    public T FirstOrDefault(Func<T, bool> whereFunction)
    {
      lock (this.itemsLock)
        return this.items.FirstOrDefault<T>(whereFunction);
    }

    public IEnumerable<T> Where(Func<T, bool> whereFunction)
    {
      lock (this.itemsLock)
        return this.items.Where<T>(whereFunction);
    }

    private string createUniqueGuid()
    {
      int num = 600;
      while (num < 600)
      {
        string uniqueGuid = Guid.NewGuid().ToString();
        bool flag = true;
        lock (this.itemsLock)
        {
          lock (this.itemsLock)
          {
            foreach (T obj in this.items)
            {
              if (obj.Id == uniqueGuid)
              {
                flag = false;
                break;
              }
            }
          }
        }
        if (flag)
          return uniqueGuid;
      }
      return Guid.NewGuid().ToString();
    }

    public string CreateUniqueGuid(Func<T, string> func)
    {
      int num = 600;
      while (num < 600)
      {
        string guid = Guid.NewGuid().ToString();
        if (this.items.Where<T>((Func<T, bool>) (i => func(i) == guid)).Count<T>() == 0)
          return guid;
      }
      return Guid.NewGuid().ToString();
    }
  }
}
