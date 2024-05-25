// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapLightenExtension
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapLightenExtension
  {
    public static WriteableBitmap Lighten(this WriteableBitmap target, double amount)
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
          double num6 = ((double) byte.MaxValue - num3) * amount * num2;
          double num7 = ((double) byte.MaxValue - num4) * amount * num2;
          double num8 = ((double) byte.MaxValue - num5) * amount * num2;
          pixels.Bytes[index] += (byte) num8;
          pixels.Bytes[index + 1] += (byte) num7;
          pixels.Bytes[index + 2] += (byte) num6;
        }
      }
      pixels.UpdateFromBytes();
      target.Invalidate();
      return target;
    }
  }
}
