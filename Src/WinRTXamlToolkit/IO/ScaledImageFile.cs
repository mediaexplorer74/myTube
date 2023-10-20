// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.ScaledImageFile
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Storage;

namespace WinRTXamlToolkit.IO
{
  public static class ScaledImageFile
  {
    public static async Task<StorageFile> Get(string relativePath)
    {
      string resourceKey = string.Format("Files/{0}", (object) relativePath);
      ResourceMap mainResourceMap = ResourceManager.Current.MainResourceMap;
      return !((IReadOnlyDictionary<string, NamedResource>) mainResourceMap).ContainsKey(resourceKey) ? (StorageFile) null : await ((IReadOnlyDictionary<string, NamedResource>) mainResourceMap)[resourceKey].Resolve(ResourceContext.GetForCurrentView()).GetValueAsFileAsync();
    }
  }
}
