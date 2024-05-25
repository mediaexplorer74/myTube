// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapCropExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapCropExtensions
  {
    public static WriteableBitmap Crop(
      this WriteableBitmap source,
      int x1,
      int y1,
      int x2,
      int y2)
    {
      if (x1 >= x2 || y1 >= y2 || x1 < 0 || y1 < 0 || x2 < 0 || y2 < 0 || x1 > ((BitmapSource) source).PixelWidth || y1 > ((BitmapSource) source).PixelHeight || x2 > ((BitmapSource) source).PixelWidth || y2 > ((BitmapSource) source).PixelHeight)
        throw new ArgumentException();
      int num1 = x2 - x1;
      int num2 = y2 - y1;
      WriteableBitmap writeableBitmap = new WriteableBitmap(num1, num2);
      byte[] buffer = new byte[4 * num1 * num2];
      Stream stream1 = WindowsRuntimeBufferExtensions.AsStream(source.PixelBuffer);
      stream1.Seek((long) (4 * (((BitmapSource) source).PixelWidth * y1 + x1)), SeekOrigin.Current);
      for (int index = 0; index < num2; ++index)
      {
        stream1.Read(buffer, 4 * num1 * index, 4 * num1);
        stream1.Seek((long) (4 * (((BitmapSource) source).PixelWidth - num1)), SeekOrigin.Current);
      }
      Stream stream2 = WindowsRuntimeBufferExtensions.AsStream(writeableBitmap.PixelBuffer);
      stream2.Seek(0L, SeekOrigin.Begin);
      stream2.Write(buffer, 0, buffer.Length);
      writeableBitmap.Invalidate();
      return writeableBitmap;
    }
  }
}
