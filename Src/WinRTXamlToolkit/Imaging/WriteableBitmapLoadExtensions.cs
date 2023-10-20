// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapLoadExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.IO;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapLoadExtensions
  {
    public static async Task<WriteableBitmap> LoadAsync(string relativePath) => await new WriteableBitmap(1, 1).LoadAsync(relativePath);

    public static async Task<WriteableBitmap> LoadAsync(
      string relativePath,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      return await new WriteableBitmap(1, 1).LoadAsync(relativePath, decodePixelWidth, decodePixelHeight);
    }

    public static async Task<WriteableBitmap> LoadAsync(StorageFile storageFile) => await new WriteableBitmap(1, 1).LoadAsync(storageFile);

    public static async Task<WriteableBitmap> LoadAsync(
      StorageFile storageFile,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      return await new WriteableBitmap(1, 1).LoadAsync(storageFile, decodePixelWidth, decodePixelHeight);
    }

    public static async Task<WriteableBitmap> LoadAsync(
      this WriteableBitmap writeableBitmap,
      string relativePath)
    {
      StorageFile resolvedFile = await ScaledImageFile.Get(relativePath);
      if (resolvedFile == null)
        throw new FileNotFoundException("Could not load image.", relativePath);
      return await writeableBitmap.LoadAsync(resolvedFile);
    }

    public static async Task<WriteableBitmap> LoadAsync(
      this WriteableBitmap writeableBitmap,
      StorageFile storageFile)
    {
      WriteableBitmap wb = writeableBitmap;
      using (IRandomAccessStreamWithContentType stream = await storageFile.OpenReadAsync())
        await ((BitmapSource) wb).SetSourceAsync((IRandomAccessStream) stream);
      return wb;
    }

    public static async Task<WriteableBitmap> LoadAsync(
      this WriteableBitmap writeableBitmap,
      StorageFile storageFile,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      using (IRandomAccessStreamWithContentType stream = await storageFile.OpenReadAsync())
        await writeableBitmap.SetSourceAsync((IRandomAccessStream) stream, decodePixelWidth, decodePixelHeight);
      return writeableBitmap;
    }

    public static async Task<WriteableBitmap> LoadAsync(
      this WriteableBitmap writeableBitmap,
      string relativePath,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      StorageFile resolvedFile = await ScaledImageFile.Get(relativePath);
      return await writeableBitmap.LoadAsync(resolvedFile, decodePixelWidth, decodePixelHeight);
    }

    public static async Task SetSourceAsync(
      this WriteableBitmap writeableBitmap,
      IRandomAccessStream streamSource,
      uint decodePixelWidth,
      uint decodePixelHeight)
    {
      BitmapDecoder decoder = await BitmapDecoder.CreateAsync(streamSource);
      using (InMemoryRandomAccessStream inMemoryStream = new InMemoryRandomAccessStream())
      {
        BitmapEncoder encoder = await BitmapEncoder.CreateForTranscodingAsync((IRandomAccessStream) inMemoryStream, decoder);
        encoder.BitmapTransform.put_ScaledWidth(decodePixelWidth);
        encoder.BitmapTransform.put_ScaledHeight(decodePixelHeight);
        await encoder.FlushAsync();
        inMemoryStream.Seek(0UL);
        await ((BitmapSource) writeableBitmap).SetSourceAsync((IRandomAccessStream) inMemoryStream);
      }
    }
  }
}
