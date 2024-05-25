// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.IO.Extensions.StorageFolderExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Storage;

namespace WinRTXamlToolkit.IO.Extensions
{
  public static class StorageFolderExtensions
  {
    public static async Task<bool> ContainsFileAsync(this StorageFolder folder, string relativePath)
    {
      if (folder == Package.Current.InstalledLocation)
        return ((IReadOnlyDictionary<string, NamedResource>) ResourceManager.Current.MainResourceMap).ContainsKey(string.Format("Files/{0}", (object) relativePath));
      string[] parts = relativePath.Split('\\', '/');
      for (int i = 0; i < parts.Length - 1; ++i)
        folder = await folder.GetFolderAsync(parts[i]);
      string fileName = parts[parts.Length - 1];
      return ((IEnumerable<StorageFile>) await (IAsyncOperation<IReadOnlyList<StorageFile>>) folder.GetFilesAsync()).Any<StorageFile>((Func<StorageFile, bool>) (file => file.Name == fileName));
    }

    public static async Task<StorageFile> GetFileByPathAsync(
      this StorageFolder folder,
      string relativePath)
    {
      if (folder == Package.Current.InstalledLocation)
      {
        string resourceKey = string.Format("Files/{0}", (object) relativePath);
        ResourceMap mainResourceMap = ResourceManager.Current.MainResourceMap;
        if (((IReadOnlyDictionary<string, NamedResource>) mainResourceMap).ContainsKey(resourceKey))
          return await ((IReadOnlyDictionary<string, NamedResource>) mainResourceMap)[resourceKey].Resolve(ResourceContext.GetForCurrentView()).GetValueAsFileAsync();
      }
      string[] parts = relativePath.Split('\\', '/');
      for (int i = 0; i < parts.Length - 1; ++i)
        folder = await folder.GetFolderAsync(parts[i]);
      string fileName = parts[parts.Length - 1];
      return await folder.GetFileAsync(fileName);
    }

    public static async Task<string> CreateTempFileNameAsync(
      this StorageFolder folder,
      string extension = ".tmp",
      string prefix = "",
      string suffix = "")
    {
      if (folder == null)
        folder = ApplicationData.Current.TemporaryFolder;
      if (string.IsNullOrEmpty(extension))
        extension = ".tmp";
      else if (extension[0] != '.')
        extension = string.Format(".{0}", (object) extension);
      string fileName;
      if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(prefix))
      {
        fileName = string.Format("{0}{1}{2}", (object) prefix, (object) suffix, (object) extension);
        if (!await folder.ContainsFileAsync(fileName))
          return fileName;
      }
      do
      {
        fileName = string.Format("{0}{1}{2}{3}", (object) prefix, (object) Guid.NewGuid(), (object) suffix, (object) extension);
      }
      while (await folder.ContainsFileAsync(fileName));
      return fileName;
    }

    public static async Task<StorageFile> CreateTempFileAsync(
      this StorageFolder folder,
      string extension = ".tmp")
    {
      if (folder == null)
        folder = ApplicationData.Current.TemporaryFolder;
      string fileName = await folder.CreateTempFileNameAsync(extension, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.ffffff"));
      StorageFile file = await folder.CreateFileAsync(fileName, (CreationCollisionOption) 0);
      return file;
    }

    public static async Task DeleteFilesAsync(this StorageFolder folder, bool ignoreExceptions = false)
    {
      try
      {
        foreach (StorageFile file in (IEnumerable<StorageFile>) await (IAsyncOperation<IReadOnlyList<StorageFile>>) folder.GetFilesAsync())
        {
          try
          {
            await file.DeleteAsync((StorageDeleteOption) 1);
          }
          catch
          {
            if (!ignoreExceptions)
              throw;
          }
        }
      }
      catch
      {
        if (ignoreExceptions)
          return;
        throw;
      }
    }

    public static async Task<bool> ContainsFolderAsync(this StorageFolder folder, string name) => ((IEnumerable<StorageFolder>) await (IAsyncOperation<IReadOnlyList<StorageFolder>>) folder.GetFoldersAsync()).Any<StorageFolder>((Func<StorageFolder, bool>) (l => l.Name == name));

    public static async Task EnsureFolderExistsAsync(this StorageFolder folder, string name)
    {
      if (await folder.ContainsFolderAsync(name))
        return;
      StorageFolder folderAsync = await folder.CreateFolderAsync(name);
    }
  }
}
