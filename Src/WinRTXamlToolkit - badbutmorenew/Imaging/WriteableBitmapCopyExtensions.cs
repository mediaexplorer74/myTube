// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapCopyExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapCopyExtensions
  {
    public static WriteableBitmap Copy(this WriteableBitmap source)
    {
      source.Invalidate();
      byte[] buffer = new byte[4 * ((BitmapSource) source).PixelWidth * ((BitmapSource) source).PixelHeight];
      Stream stream1 = WindowsRuntimeBufferExtensions.AsStream(source.PixelBuffer);
      stream1.Seek(0L, SeekOrigin.Begin);
      stream1.Read(buffer, 0, buffer.Length);
      WriteableBitmap writeableBitmap = new WriteableBitmap(((BitmapSource) source).PixelWidth, ((BitmapSource) source).PixelHeight);
      Stream stream2 = WindowsRuntimeBufferExtensions.AsStream(writeableBitmap.PixelBuffer);
      stream2.Seek(0L, SeekOrigin.Begin);
      stream2.Write(buffer, 0, buffer.Length);
      writeableBitmap.Invalidate();
      return writeableBitmap;
    }
  }
}
