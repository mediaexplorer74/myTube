// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Net.WebFile
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.Net
{
  public static class WebFile
  {
    public static async Task<StorageFile> SaveAsync(
      Uri fileUri,
      StorageFolder folder = null,
      string fileName = null,
      NameCollisionOption option = 0)
    {
      if (folder == null)
        folder = ApplicationData.Current.LocalFolder;
      StorageFile file = await folder.CreateTempFileAsync();
      BackgroundDownloader downloader = new BackgroundDownloader();
      DownloadOperation download = downloader.CreateDownload((Uri) fileUri, (IStorageFile) file);
      DownloadOperation res = await download.StartAsync();
      if (string.IsNullOrEmpty(fileName))
      {
        fileName = file.Name;
        ResponseInformation info = res.GetResponseInformation();
        if (((IReadOnlyDictionary<string, string>) info.Headers).ContainsKey("Content-Disposition"))
        {
          string cd = ((IReadOnlyDictionary<string, string>) info.Headers)["Content-Disposition"];
          Regex regEx = new Regex("filename=\"(?<fileNameGroup>.+?)\"");
          Match match = regEx.Match(cd);
          if (match.Success)
          {
            fileName = match.Groups["fileNameGroup"].Value;
            await file.RenameAsync(fileName, option);
            return file;
          }
        }
      }
      await file.RenameAsync(fileName, option);
      return file;
    }
  }
}
