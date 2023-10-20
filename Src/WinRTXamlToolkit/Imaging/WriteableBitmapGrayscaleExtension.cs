// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapGrayscaleExtension
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapGrayscaleExtension
  {
    public static WriteableBitmap Grayscale(this WriteableBitmap target)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      for (int index = 0; index < pixels.Bytes.Length; index += 4)
      {
        byte num1 = pixels.Bytes[index + 3];
        if (num1 > (byte) 0)
        {
          double num2 = (double) num1 / (double) byte.MaxValue;
          double num3 = (0.2126 * ((double) pixels.Bytes[index + 2] / num2) + 447.0 / 625.0 * ((double) pixels.Bytes[index + 1] / num2) + 0.0722 * ((double) pixels.Bytes[index] / num2)) * num2;
          double num4 = num3;
          double num5 = num3;
          pixels.Bytes[index] = (byte) num5;
          pixels.Bytes[index + 1] = (byte) num4;
          pixels.Bytes[index + 2] = (byte) num3;
        }
      }
      pixels.UpdateFromBytes();
      target.Invalidate();
      return target;
    }

    public static WriteableBitmap Grayscale(this WriteableBitmap target, double amount)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      for (int index = 0; index < pixels.Bytes.Length; index += 4)
      {
        byte num1 = pixels.Bytes[index + 3];
        if (num1 > (byte) 0)
        {
          double num2 = (double) num1 / (double) byte.MaxValue;
          double num3 = (double) pixels.Bytes[index + 2] / num2;
          double num4 = (double) pixels.Bytes[index + 1] / num2;
          double num5 = (double) pixels.Bytes[index] / num2;
          double num6 = 0.2126 * num3 + 447.0 / 625.0 * num4 + 0.0722 * num5;
          double num7 = ((1.0 - amount) * num3 + amount * num6) * num2;
          double num8 = ((1.0 - amount) * num4 + amount * num6) * num2;
          double num9 = ((1.0 - amount) * num5 + amount * num6) * num2;
          pixels.Bytes[index] = (byte) num9;
          pixels.Bytes[index + 1] = (byte) num8;
          pixels.Bytes[index + 2] = (byte) num7;
        }
      }
      pixels.UpdateFromBytes();
      target.Invalidate();
      return target;
    }
  }
}
