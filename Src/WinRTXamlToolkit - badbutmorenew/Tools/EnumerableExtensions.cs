// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.EnumerableExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace WinRTXamlToolkit.Tools
{
  public static class EnumerableExtensions
  {
    private static readonly Random _random = new Random();

    public static List<T> Shuffle<T>(this IEnumerable<T> list)
    {
      List<T> list1 = ((IEnumerable<T>) list).ToList<T>();
      List<T> objList = new List<T>(list1.Count);
      while (list1.Count > 0)
      {
        int index = EnumerableExtensions._random.Next(list1.Count);
        objList.Add(list1[index]);
        list1.RemoveAt(index);
      }
      return objList;
    }

    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
      foreach (T obj in list)
        action(obj);
    }
  }
}
