// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.WriteableBitmapColorPickerExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Imaging
{
  public static class WriteableBitmapColorPickerExtensions
  {
    public static void RenderColorPickerHueLightness(this WriteableBitmap target, double saturation)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHueLightnessCore(saturation, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHueLightnessAsync(
      this WriteableBitmap target,
      double saturation)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHueLightnessCore(saturation, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHueLightnessCore(
      double saturation,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw - 1;
      int num2 = ph - 1;
      for (int index1 = 0; index1 < ph; ++index1)
      {
        double lightness = 1.0 * (double) (ph - 1 - index1) / (double) num2;
        for (int index2 = 0; index2 < pw; ++index2)
        {
          Color color = ColorExtensions.FromHsl(360.0 * (double) index2 / (double) num1, saturation, lightness);
          pixels[pw * index1 + index2] = color.AsInt();
        }
      }
    }

    public static void RenderColorPickerSaturationLightnessRect(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationLightnessRectCore(hue, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerSaturationLightnessRectAsync(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationLightnessRectCore(hue, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerSaturationLightnessRectCore(
      double hue,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw - 1;
      int num2 = ph - 1;
      for (int index1 = 0; index1 < ph; ++index1)
      {
        double lightness = 1.0 * (double) (ph - 1 - index1) / (double) num2;
        for (int index2 = 0; index2 < pw; ++index2)
        {
          double saturation = (double) index2 / (double) num1;
          Color color = ColorExtensions.FromHsl(hue, saturation, lightness);
          pixels[pw * index1 + index2] = color.AsInt();
        }
      }
    }

    public static void RenderColorPickerSaturationLightnessTriangle(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationLightnessTriangleCore(hue, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerSaturationLightnessTriangleAsync(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationLightnessTriangleCore(hue, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerSaturationLightnessTriangleCore(
      double hue,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw / 2;
      double num2 = 1.0 / (double) ph;
      for (int index1 = 0; index1 < ph; ++index1)
      {
        double lightness = 1.0 - 1.0 * (double) (ph - 1 - index1) / (double) ph;
        int num3 = (int) ((double) num1 * (1.0 - num2 * (double) index1));
        int num4 = pw - num3;
        for (int index2 = num3; index2 < num4; ++index2)
        {
          double saturation = 1.0 - (double) (index2 + num4 - pw) / (double) pw;
          Color color = ColorExtensions.FromHsl(hue, saturation, lightness);
          pixels[pw * index1 + index2] = color.AsInt();
        }
      }
    }

    public static void RenderColorPickerHueValue(this WriteableBitmap target, double saturation)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHueValueCore(saturation, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHueValueAsync(
      this WriteableBitmap target,
      double saturation)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHueValueCore(saturation, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHueValueCore(
      double saturation,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw - 1;
      int num2 = ph - 1;
      for (int index1 = 0; index1 < ph; ++index1)
      {
        double num3 = 1.0 * (double) (ph - 1 - index1) / (double) num2;
        for (int index2 = 0; index2 < pw; ++index2)
        {
          Color color = ColorExtensions.FromHsv(360.0 * (double) index2 / (double) num1, saturation, num3);
          pixels[pw * index1 + index2] = color.AsInt();
        }
      }
    }

    public static void RenderColorPickerSaturationValueRect(this WriteableBitmap target, double hue = 0.0)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationValueRectCore(hue, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerSaturationValueRectAsync(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationValueRectCore(hue, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerSaturationValueRectCore(
      double hue,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw - 1;
      int num2 = ph - 1;
      for (int index1 = 0; index1 < ph; ++index1)
      {
        double num3 = 1.0 * (double) (ph - 1 - index1) / (double) num2;
        for (int index2 = 0; index2 < pw; ++index2)
        {
          double saturation = (double) index2 / (double) num1;
          Color color = ColorExtensions.FromHsv(hue, saturation, num3);
          pixels[pw * index1 + index2] = color.AsInt();
        }
      }
    }

    public static void RenderColorPickerSaturationValueTriangle(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int hw = pixelWidth / 2;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      double invPh = 1.0 / (double) pixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationValueTriangleCore(hue, pixelHeight, hw, invPh, pixelWidth, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerSaturationValueTriangleAsync(
      this WriteableBitmap target,
      double hue = 0.0)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int hw = pw / 2;
      int ph = ((BitmapSource) target).PixelHeight;
      double invPh = 1.0 / (double) ph;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerSaturationValueTriangleCore(hue, ph, hw, invPh, pw, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerSaturationValueTriangleCore(
      double hue,
      int ph,
      int hw,
      double invPh,
      int pw,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      for (int index1 = 0; index1 < ph; ++index1)
      {
        double num1 = 1.0 - 1.0 * (double) (ph - 1 - index1) / (double) ph;
        int num2 = (int) ((double) hw * (1.0 - invPh * (double) index1));
        int num3 = pw - num2;
        for (int index2 = num2; index2 < num3; ++index2)
        {
          double saturation = 1.0 - (double) (index2 + num3 - pw) / (double) pw;
          Color color = ColorExtensions.FromHsv(hue, saturation, num1);
          pixels[pw * index1 + index2] = color.AsInt();
        }
      }
    }

    public static void RenderColorPickerHueRing(
      this WriteableBitmap target,
      int innerRingRadius = 0,
      int outerRingRadius = 0)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHueRingCore(innerRingRadius, outerRingRadius, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHueRingAsync(
      this WriteableBitmap target,
      int innerRingRadius = 0,
      int outerRingRadius = 0)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHueRingCore(innerRingRadius, outerRingRadius, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHueRingCore(
      int innerRingRadius,
      int outerRingRadius,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw / 2;
      int num2 = ph / 2;
      if (outerRingRadius == 0)
        outerRingRadius = Math.Min(pw, ph) / 2;
      if (innerRingRadius == 0)
        innerRingRadius = outerRingRadius * 2 / 3;
      int num3 = outerRingRadius * outerRingRadius;
      int num4 = innerRingRadius * innerRingRadius;
      for (int index1 = 0; index1 < ph; ++index1)
      {
        for (int index2 = 0; index2 < pw; ++index2)
        {
          int num5 = (index2 - num1) * (index2 - num1) + (index1 - num2) * (index1 - num2);
          if (num5 >= num4 && num5 <= num3)
          {
            Color color = ColorExtensions.FromHsl((Math.Atan2((double) (index1 - num2), (double) (index2 - num1)) * 180.0 * 0.31830988618379069 + 90.0 + 360.0) % 360.0, 1.0, 0.5);
            pixels[pw * index1 + index2] = color.AsInt();
          }
        }
      }
    }

    public static void RenderColorPickerHSVHueBar(
      this WriteableBitmap target,
      double saturation,
      double value)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHSVHueBarCore(saturation, value, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHSVHueBarAsync(
      this WriteableBitmap target,
      double saturation,
      double value)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHSVHueBarCore(saturation, value, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHSVHueBarCore(
      double saturation,
      double value,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        Color color = ColorExtensions.FromHsv(360.0 * (double) index1 / (double) num, saturation, value);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerHSVSaturationBar(
      this WriteableBitmap target,
      double hue,
      double value)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHSVSaturationBarCore(hue, value, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHSVSaturationBarAsync(
      this WriteableBitmap target,
      double hue,
      double value)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHSVSaturationBarCore(hue, value, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHSVSaturationBarCore(
      double hue,
      double value,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        double saturation = (double) index1 / (double) num;
        Color color = ColorExtensions.FromHsv(hue, saturation, value);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerHSVValueBar(
      this WriteableBitmap target,
      double hue,
      double saturation)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHSVValueBarCore(hue, saturation, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHSVValueBarAsync(
      this WriteableBitmap target,
      double hue,
      double saturation)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHSVValueBarCore(hue, saturation, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHSVValueBarCore(
      double hue,
      double saturation,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num1 = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        double num2 = (double) index1 / (double) num1;
        Color color = ColorExtensions.FromHsv(hue, saturation, num2);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerHSLHueBar(
      this WriteableBitmap target,
      double saturation,
      double lightness)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHSLHueBarCore(saturation, lightness, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHSLHueBarAsync(
      this WriteableBitmap target,
      double saturation,
      double lightness)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHSLHueBarCore(saturation, lightness, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHSLHueBarCore(
      double saturation,
      double lightness,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        Color color = ColorExtensions.FromHsl(360.0 * (double) index1 / (double) num, saturation, lightness);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerHSLSaturationBar(
      this WriteableBitmap target,
      double hue,
      double lightness)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      WriteableBitmapColorPickerExtensions.RenderColorPickerHSLSaturationBarCore(hue, lightness, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHSLSaturationBarAsync(
      this WriteableBitmap target,
      double hue,
      double lightness)
    {
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHSLSaturationBarCore(hue, lightness, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHSLSaturationBarCore(
      double hue,
      double lightness,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        double saturation = (double) index1 / (double) num;
        Color color = ColorExtensions.FromHsl(hue, saturation, lightness);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerHSLLightnessBar(
      this WriteableBitmap target,
      double hue,
      double saturation)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerHSLLightnessBarCore(hue, saturation, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerHSLLightnessBarAsync(
      this WriteableBitmap target,
      double hue,
      double saturation)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerHSLLightnessBarCore(hue, saturation, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerHSLLightnessBarCore(
      double hue,
      double saturation,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        double lightness = (double) index1 / (double) num;
        Color color = ColorExtensions.FromHsl(hue, saturation, lightness);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerRGBRedBar(
      this WriteableBitmap target,
      double green,
      double blue)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerRGBRedBarCore(green, blue, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerRGBRedBarAsync(
      this WriteableBitmap target,
      double green,
      double blue)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerRGBRedBarCore(green, blue, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerRGBRedBarCore(
      double green,
      double blue,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        Color color = ColorExtensions.FromRgb((double) index1 / (double) num, green, blue);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerRGBGreenBar(
      this WriteableBitmap target,
      double red,
      double blue)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerRGBGreenBarCore(red, blue, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerRGBGreenBarAsync(
      this WriteableBitmap target,
      double red,
      double blue)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerRGBGreenBarCore(red, blue, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerRGBGreenBarCore(
      double red,
      double blue,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        double green = (double) index1 / (double) num;
        Color color = ColorExtensions.FromRgb(red, green, blue);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }

    public static void RenderColorPickerRGBBlueBar(
      this WriteableBitmap target,
      double red,
      double green)
    {
      int pixelWidth = ((BitmapSource) target).PixelWidth;
      int pixelHeight = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      WriteableBitmapColorPickerExtensions.RenderColorPickerRGBBlueBarCore(red, green, pixelWidth, pixelHeight, pixels);
      target.Invalidate();
    }

    public static async Task RenderColorPickerRGBBlueBarAsync(
      this WriteableBitmap target,
      double red,
      double green)
    {
      int pw = ((BitmapSource) target).PixelWidth;
      int ph = ((BitmapSource) target).PixelHeight;
      IBufferExtensions.PixelBufferInfo pixels = target.PixelBuffer.GetPixels();
      await Task.Run((Action) (() => WriteableBitmapColorPickerExtensions.RenderColorPickerRGBBlueBarCore(red, green, pw, ph, pixels)));
      target.Invalidate();
    }

    private static void RenderColorPickerRGBBlueBarCore(
      double red,
      double green,
      int pw,
      int ph,
      IBufferExtensions.PixelBufferInfo pixels)
    {
      int num = pw - 1;
      for (int index1 = 0; index1 < pw; ++index1)
      {
        double blue = (double) index1 / (double) num;
        Color color = ColorExtensions.FromRgb(red, green, blue);
        for (int index2 = 0; index2 < ph; ++index2)
          pixels[pw * index2 + index1] = color.AsInt();
      }
    }
  }
}
