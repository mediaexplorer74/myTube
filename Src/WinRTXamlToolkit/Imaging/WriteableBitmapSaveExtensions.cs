// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapSaveExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapSaveExtensions
  {
    public static async Task<StorageFile> SaveToFile(this WriteableBitmap writeableBitmap) => await writeableBitmap.SaveToFile(KnownFolders.PicturesLibrary, string.Format("{0}_{1}.png", (object) DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"), (object) Guid.NewGuid()), (CreationCollisionOption) 0);

    public static async Task<StorageFile> SaveToFile(
      this WriteableBitmap writeableBitmap,
      StorageFolder storageFolder)
    {
      return await writeableBitmap.SaveToFile(storageFolder, string.Format("{0}_{1}.png", (object) DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"), (object) Guid.NewGuid()), (CreationCollisionOption) 0);
    }

    public static async Task<StorageFile> SaveToFile(
      this WriteableBitmap writeableBitmap,
      StorageFolder storageFolder,
      string fileName,
      CreationCollisionOption options = 1)
    {
      StorageFile outputFile = await storageFolder.CreateFileAsync(fileName, options);
      string ext = Path.GetExtension(fileName);
      Guid encoderId;
      if (((IEnumerable<string>) new string[2]
      {
        ".bmp",
        ".dib"
      }).Contains<string>(ext))
        encoderId = BitmapEncoder.BmpEncoderId;
      else if (((IEnumerable<string>) new string[2]
      {
        ".tiff",
        ".tif"
      }).Contains<string>(ext))
        encoderId = BitmapEncoder.TiffEncoderId;
      else if (((IEnumerable<string>) new string[1]
      {
        ".gif"
      }).Contains<string>(ext))
        encoderId = BitmapEncoder.GifEncoderId;
      else if (((IEnumerable<string>) new string[5]
      {
        ".jpg",
        ".jpeg",
        ".jpe",
        ".jfif",
        ".jif"
      }).Contains<string>(ext))
        encoderId = BitmapEncoder.JpegEncoderId;
      else
        encoderId = !((IEnumerable<string>) new string[3]
        {
          ".hdp",
          ".jxr",
          ".wdp"
        }).Contains<string>(ext) ? BitmapEncoder.PngEncoderId : BitmapEncoder.JpegXREncoderId;
      await writeableBitmap.SaveToFile(outputFile, encoderId);
      return outputFile;
    }

    public static async Task SaveToFile(
      this WriteableBitmap writeableBitmap,
      StorageFile outputFile,
      Guid encoderId)
    {
      Stream stream = WindowsRuntimeBufferExtensions.AsStream(writeableBitmap.PixelBuffer);
      byte[] pixels = new byte[(IntPtr) (uint) stream.Length];
      int num1 = await stream.ReadAsync(pixels, 0, pixels.Length);
      using (IRandomAccessStream writeStream = await outputFile.OpenAsync((FileAccessMode) 1))
      {
        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(encoderId, writeStream);
        encoder.SetPixelData((BitmapPixelFormat) 87, (BitmapAlphaMode) 0, (uint) ((BitmapSource) writeableBitmap).PixelWidth, (uint) ((BitmapSource) writeableBitmap).PixelHeight, 96.0, 96.0, pixels);
        await encoder.FlushAsync();
        using (IOutputStream outputStream = writeStream.GetOutputStreamAt(0UL))
        {
          int num2 = await outputStream.FlushAsync() ? 1 : 0;
        }
      }
    }
  }
}
