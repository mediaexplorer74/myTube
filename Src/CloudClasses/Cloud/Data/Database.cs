// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.Data.Database
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace myTube.Cloud.Data
{
  public static class Database
  {
    private static object newTableLock = new object();
    private static object savingLock = new object();
    private static Dictionary<string, ITable> tables;
    private static Dictionary<string, DateTime> tableTimes;
    private static Timer timer;
    private static bool saving = false;
    private static bool saveScheduled = false;
    private static Queue<string> saveKeys = new Queue<string>();
    private static bool enteredLock = false;

    static Database()
    {
      Database.tables = new Dictionary<string, ITable>();
      Database.tableTimes = new Dictionary<string, DateTime>();
      Database.timer = new Timer(new TimerCallback(Database.timerCallback), (object) 0, 60000, -1);
    }

    private static void timerCallback(object t)
    {
      try
      {
        while (Database.saveKeysCount() > 0)
        {
          Database.Log("Save timer");
          Database.saveTable(Database.saveKeys.Dequeue());
        }
      }
      catch
      {
      }
      Database.timer.Change(60000, -1);
    }

    private static int saveKeysCount()
    {
      lock (Database.savingLock)
        return Database.saveKeys.Count;
    }

    public static Table<T> GetTable<T>(string key, uint maxValues = 0, Func<T, T, bool> sortingMethod = null) where T : DataObject
    {
      lock (Database.newTableLock)
      {
        if (Database.tables.ContainsKey(key))
          return Database.tables[key] as Table<T>;
        if (FileManager.Exists == null || FileManager.LoadFile == null)
          return (Table<T>) null;
        if (FileManager.Exists(key + ".tbl"))
        {
          using (Stream stream1 = FileManager.LoadFile(key + ".tbl"))
          {
            using (StreamReader streamReader1 = new StreamReader(stream1))
            {
              try
              {
                Table<T> table = JObject.Parse(streamReader1.ReadToEnd()).ToObject<Table<T>>();
                lock (Database.savingLock)
                {
                  if (Database.tables.ContainsKey(key))
                    return Database.tables[key] as Table<T>;
                  Database.tables.Add(key, (ITable) table);
                }
                if (maxValues > 0U)
                  table.MaxValues = maxValues;
                if (sortingMethod != null)
                  table.SortMethod = sortingMethod;
                return table;
              }
              catch
              {
                using (Stream stream2 = FileManager.LoadFile(key + ".Backup.tbl"))
                {
                  using (StreamReader streamReader2 = new StreamReader(stream2))
                  {
                    Table<T> table = JObject.Parse(streamReader2.ReadToEnd()).ToObject<Table<T>>();
                    lock (Database.savingLock)
                    {
                      if (Database.tables.ContainsKey(key))
                        return Database.tables[key] as Table<T>;
                      Database.tables.Add(key, (ITable) table);
                    }
                    if (maxValues > 0U)
                      table.MaxValues = maxValues;
                    if (sortingMethod != null)
                      table.SortMethod = sortingMethod;
                    return table;
                  }
                }
              }
            }
          }
        }
        else
        {
          Table<T> table = new Table<T>();
          lock (Database.savingLock)
          {
            if (Database.tables.ContainsKey(key))
              return Database.tables[key] as Table<T>;
            Database.tables.Add(key, (ITable) table);
          }
          if (maxValues > 0U)
            table.MaxValues = maxValues;
          if (sortingMethod != null)
            table.SortMethod = sortingMethod;
          return table;
        }
      }
    }

    public static void SaveTables(ITable table)
    {
      if (!Database.tables.ContainsValue(table))
        return;
      lock (Database.savingLock)
      {
        foreach (KeyValuePair<string, ITable> table1 in Database.tables)
        {
          if (table1.Value == table)
            Database.SaveTables(table1.Key);
        }
      }
    }

    public static void SaveTables(string tableKey = "all")
    {
      lock (Database.savingLock)
      {
        if (!Database.saveKeys.Contains(tableKey))
        {
          Database.Log("Enqueing database save");
          Database.saveKeys.Enqueue(tableKey);
        }
        Database.saveScheduled = true;
      }
    }

    private static void saveTable(string tableKey)
    {
      Database.saving = true;
      if (FileManager.SaveFile != null)
      {
        bool flag = true;
        lock (Database.newTableLock)
        {
          Database.enteredLock = true;
          if (flag)
          {
            foreach (KeyValuePair<string, ITable> table in Database.tables)
            {
              if (tableKey == "all" || table.Key == tableKey)
              {
                table.Value.Count.ToString();
                string json = DataObject.ToJson((object) table.Value);
                JObject.FromObject((object) table.Value);
                lock (Database.savingLock)
                {
                  using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                  {
                    string str1 = table.Key + ".tbl";
                    string str2 = table.Key + ".Backup.tbl";
                    if (FileManager.Copy != null)
                    {
                      try
                      {
                        int num = FileManager.Copy(str1, str2) ? 1 : 0;
                      }
                      catch
                      {
                      }
                    }
                    FileManager.SaveFile(table.Key + ".tbl", (Stream) memoryStream);
                  }
                }
              }
            }
            if (!Database.tableTimes.ContainsKey(tableKey))
              Database.tableTimes.Add(tableKey, DateTime.UtcNow);
            else
              Database.tableTimes[tableKey] = DateTime.UtcNow;
            Database.Log("Saved " + tableKey);
          }
          Database.enteredLock = false;
        }
      }
      Database.saving = false;
    }

    public static void Log(string log)
    {
    }
  }
}
