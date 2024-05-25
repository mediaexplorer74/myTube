// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapBlitBlockExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapBlitBlockExtensions
  {
    public static void BlitBlock(
      this WriteableBitmap target,
      int targetVerticalOffset,
      WriteableBitmap source,
      int sourceVerticalOffset,
      int verticalBlockHeight,
      bool autoInvalidate = false)
    {
      if (((BitmapSource) source).PixelWidth != ((BitmapSource) target).PixelWidth)
        throw new ArgumentException("BlitBlock only supports copying blocks of pixels between bitmaps of equal size.", nameof (source));
      if (sourceVerticalOffset + verticalBlockHeight > ((BitmapSource) source).PixelHeight || targetVerticalOffset + verticalBlockHeight > ((BitmapSource) target).PixelHeight || verticalBlockHeight <= 0)
        throw new ArgumentException();
      byte[] buffer = new byte[4 * ((BitmapSource) source).PixelWidth * verticalBlockHeight];
      Stream stream1 = WindowsRuntimeBufferExtensions.AsStream(source.PixelBuffer);
      stream1.Seek((long) (4 * ((BitmapSource) source).PixelWidth * sourceVerticalOffset), SeekOrigin.Begin);
      stream1.Read(buffer, 0, buffer.Length);
      Stream stream2 = WindowsRuntimeBufferExtensions.AsStream(target.PixelBuffer);
      stream2.Seek((long) (4 * ((BitmapSource) target).PixelWidth * targetVerticalOffset), SeekOrigin.Begin);
      stream2.Write(buffer, 0, buffer.Length);
      if (!autoInvalidate)
        return;
      target.Invalidate();
    }
  }
}
