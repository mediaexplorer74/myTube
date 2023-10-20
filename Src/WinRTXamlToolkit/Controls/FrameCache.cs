// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.FrameCache
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;

namespace WinRTXamlToolkit.Controls
{
  internal class FrameCache
  {
    private readonly Dictionary<Type, List<AlternativePage>> _typeToPageListMap = new Dictionary<Type, List<AlternativePage>>();
    private readonly List<AlternativePage> _limitedCache = new List<AlternativePage>();
    private int _cacheSize;

    internal FrameCache(int cacheSize) => this._cacheSize = cacheSize;

    internal int CacheSize
    {
      get => this._cacheSize;
      set
      {
        this._cacheSize = value;
        this.TrimLimitedCache();
      }
    }

    internal AlternativePage Get(Type type)
    {
      List<AlternativePage> alternativePageList;
      if (!this._typeToPageListMap.TryGetValue(type, out alternativePageList) || alternativePageList.Count <= 0)
        return Activator.CreateInstance(type) as AlternativePage;
      AlternativePage alternativePage = alternativePageList[alternativePageList.Count - 1];
      alternativePageList.RemoveAt(alternativePageList.Count - 1);
      if (alternativePage.NavigationCacheMode == NavigationCacheMode.Enabled)
        this._limitedCache.Remove(alternativePage);
      return alternativePage;
    }

    public void Store(AlternativePage page)
    {
      if (page.NavigationCacheMode == NavigationCacheMode.Disabled)
        return;
      Type type = ((object) page).GetType();
      List<AlternativePage> alternativePageList;
      if (!this._typeToPageListMap.TryGetValue(type, out alternativePageList))
        this._typeToPageListMap.Add(type, alternativePageList = new List<AlternativePage>());
      alternativePageList.Add(page);
      this._limitedCache.Add(page);
      this.TrimLimitedCache();
    }

    private void TrimLimitedCache()
    {
      while (this._limitedCache.Count > this._cacheSize)
      {
        AlternativePage alternativePage = this._limitedCache[0];
        this._typeToPageListMap[((object) alternativePage).GetType()].Remove(alternativePage);
        this._limitedCache.RemoveAt(0);
      }
    }
  }
}
