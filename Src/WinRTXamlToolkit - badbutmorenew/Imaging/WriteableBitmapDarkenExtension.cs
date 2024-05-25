// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapDarkenExtension
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapDarkenExtension
  {
    public static WriteableBitmap Darken(this WriteableBitmap target, double amount)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      for (int index = 0; index < pixels.Bytes.Length; index += 4)
      {
        if (pixels.Bytes[index + 3] > (byte) 0)
        {
          double num1 = (double) pixels.Bytes[index + 2];
          double num2 = (double) pixels.Bytes[index + 1];
          double num3 = (double) pixels.Bytes[index];
          double num4 = num1 * (1.0 - amount);
          double num5 = num2 * (1.0 - amount);
          double num6 = num3 * (1.0 - amount);
          pixels.Bytes[index] = (byte) num6;
          pixels.Bytes[index + 1] = (byte) num5;
          pixels.Bytes[index + 2] = (byte) num4;
        }
      }
      pixels.UpdateFromBytes();
      target.Invalidate();
      return target;
    }
  }
}
