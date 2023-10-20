// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.BitmapImageLoadExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.IO.Extensions;

namespace WinRTXamlToolkit.Imaging
{
  public static class BitmapImageLoadExtensions
  {
    public static async Task<BitmapImage> LoadAsync(StorageFile file)
    {
      BitmapImage bitmap = new BitmapImage();
      return await bitmap.SetSourceAsync(file);
    }

    public static async Task<BitmapImage> LoadAsync(StorageFolder folder, string fileName)
    {
      BitmapImage bitmap = new BitmapImage();
      if (!await folder.ContainsFileAsync(fileName))
        return (BitmapImage) null;
      StorageFile file = await folder.GetFileByPathAsync(fileName);
      return await bitmap.SetSourceAsync(file);
    }

    public static async Task<BitmapImage> SetSourceAsync(this BitmapImage bitmap, StorageFile file)
    {
      using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
        await ((BitmapSource) bitmap).SetSourceAsync((IRandomAccessStream) stream);
      return bitmap;
    }

    public static async Task<BitmapImage> LoadFromBase64String(this BitmapImage bitmap, string img)
    {
      byte[] imgBytes = Convert.FromBase64String(img);
      using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
      {
        using (DataWriter dw = new DataWriter((IOutputStream) ms))
        {
          dw.WriteBytes(imgBytes);
          int num = (int) await (IAsyncOperation<uint>) dw.StoreAsync();
          ms.Seek(0UL);
          await ((BitmapSource) bitmap).SetSourceAsync((IRandomAccessStream) ms);
        }
      }
      return bitmap;
    }

    public static async Task<BitmapImage> LoadFromBase64String(string img)
    {
      BitmapImage bm = new BitmapImage();
      BitmapImage bitmapImage = await bm.LoadFromBase64String(img);
      return bm;
    }
  }
}
