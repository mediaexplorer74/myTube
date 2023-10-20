// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapFromBitmapImageExtension
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Net;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapFromBitmapImageExtension
  {
    public static async Task<WriteableBitmap> FromBitmapImage(BitmapImage source)
    {
      WriteableBitmap ret = new WriteableBitmap(1, 1);
      return await ret.FromBitmapImage(source);
    }

    public static async Task<WriteableBitmap> FromBitmapImage(
      this WriteableBitmap target,
      BitmapImage source)
    {
      if ((Uri) source.UriSource == (Uri) null || ((Uri) source.UriSource).OriginalString == null)
        return target;
      string originalString = ((Uri) source.UriSource).OriginalString;
      if (originalString.StartsWith("ms-appx:/"))
      {
        string installedFolderImageSourceUri = originalString.Replace("ms-appx:/", "").TrimStart('/');
        WriteableBitmap writeableBitmap = await target.LoadAsync(installedFolderImageSourceUri);
      }
      else
      {
        StorageFile file = await WebFile.SaveAsync((Uri) source.UriSource, ApplicationData.Current.TemporaryFolder);
        WriteableBitmap writeableBitmap = await target.LoadAsync(file);
        await file.DeleteAsync((StorageDeleteOption) 1);
      }
      return target;
    }

    public static async Task<WriteableBitmap> FromBitmapImage(
      BitmapImage source,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      WriteableBitmap ret = new WriteableBitmap(1, 1);
      return await ret.FromBitmapImage(source, decodePixelWidth, decodePixelHeight);
    }

    public static async Task<WriteableBitmap> FromBitmapImage(
      this WriteableBitmap target,
      BitmapImage source,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      string installedFolderImageSourceUri = ((Uri) source.UriSource).OriginalString.Replace("ms-appx:/", "");
      WriteableBitmap writeableBitmap = await target.LoadAsync(installedFolderImageSourceUri, decodePixelWidth, decodePixelHeight);
      return target;
    }
  }
}
