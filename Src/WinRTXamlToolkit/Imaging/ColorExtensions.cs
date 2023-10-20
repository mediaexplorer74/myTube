// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.ColorExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Imaging
{
  public static class ColorExtensions
  {
    public static int AsInt(this Color color)
    {
      int num = (int) color.A + 1;
      return (int) color.A << 24 | (int) (byte) ((int) color.R * num >> 8) << 16 | (int) (byte) ((int) color.G * num >> 8) << 8 | (int) (byte) ((int) color.B * num >> 8);
    }

    public static Color FromString(string c)
    {
      Color color;
      ColorExtensions.FromString(c, true, out color);
      return color;
    }

    public static bool TryFromString(string c, out Color color) => ColorExtensions.FromString(c, false, out color);

    private static bool FromString(string c, bool throwOnFail, out Color color)
    {
      if (string.IsNullOrEmpty(c))
      {
        if (throwOnFail)
          throw new ArgumentException("Invalid color string.", nameof (c));
        return false;
      }
      if (c[0] == '#')
      {
        switch (c.Length)
        {
          case 4:
            ushort uint16_1 = Convert.ToUInt16(c.Substring(1), 16);
            byte num1 = (byte) ((int) uint16_1 >> 8 & 15);
            byte num2 = (byte) ((int) uint16_1 >> 4 & 15);
            byte num3 = (byte) ((uint) uint16_1 & 15U);
            byte r1 = (byte) ((uint) num1 << 4 | (uint) num1);
            byte g1 = (byte) ((uint) num2 << 4 | (uint) num2);
            byte b1 = (byte) ((uint) num3 << 4 | (uint) num3);
            color = Color.FromArgb(byte.MaxValue, r1, g1, b1);
            return true;
          case 5:
            ushort uint16_2 = Convert.ToUInt16(c.Substring(1), 16);
            byte num4 = (byte) ((uint) uint16_2 >> 12);
            byte num5 = (byte) ((int) uint16_2 >> 8 & 15);
            byte num6 = (byte) ((int) uint16_2 >> 4 & 15);
            byte num7 = (byte) ((uint) uint16_2 & 15U);
            byte a1 = (byte) ((uint) num4 << 4 | (uint) num4);
            byte r2 = (byte) ((uint) num5 << 4 | (uint) num5);
            byte g2 = (byte) ((uint) num6 << 4 | (uint) num6);
            byte b2 = (byte) ((uint) num7 << 4 | (uint) num7);
            color = Color.FromArgb(a1, r2, g2, b2);
            return true;
          case 7:
            uint uint32_1 = Convert.ToUInt32(c.Substring(1), 16);
            byte r3 = (byte) (uint32_1 >> 16 & (uint) byte.MaxValue);
            byte g3 = (byte) (uint32_1 >> 8 & (uint) byte.MaxValue);
            byte b3 = (byte) (uint32_1 & (uint) byte.MaxValue);
            color = Color.FromArgb(byte.MaxValue, r3, g3, b3);
            return true;
          case 9:
            uint uint32_2 = Convert.ToUInt32(c.Substring(1), 16);
            byte a2 = (byte) (uint32_2 >> 24);
            byte r4 = (byte) (uint32_2 >> 16 & (uint) byte.MaxValue);
            byte g4 = (byte) (uint32_2 >> 8 & (uint) byte.MaxValue);
            byte b4 = (byte) (uint32_2 & (uint) byte.MaxValue);
            color = Color.FromArgb(a2, r4, g4, b4);
            return true;
          default:
            if (throwOnFail)
              throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color format.", (object) c));
            return false;
        }
      }
      else
      {
        if (c.Length > 3 && c[0] == 's' && c[1] == 'c' && c[2] == '#')
        {
          string[] strArray = c.Split(',');
          if (strArray.Length == 4)
          {
            double num8 = double.Parse(strArray[0].Substring(3));
            double num9 = double.Parse(strArray[1]);
            double num10 = double.Parse(strArray[2]);
            double num11 = double.Parse(strArray[3]);
            color = Color.FromArgb((byte) (num8 * (double) byte.MaxValue), (byte) (num9 * (double) byte.MaxValue), (byte) (num10 * (double) byte.MaxValue), (byte) (num11 * (double) byte.MaxValue));
            return true;
          }
          if (strArray.Length == 3)
          {
            double num12 = double.Parse(strArray[0].Substring(3));
            double num13 = double.Parse(strArray[1]);
            double num14 = double.Parse(strArray[2]);
            color = Color.FromArgb(byte.MaxValue, (byte) (num12 * (double) byte.MaxValue), (byte) (num13 * (double) byte.MaxValue), (byte) (num14 * (double) byte.MaxValue));
            return true;
          }
          if (throwOnFail)
            throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color format (sc#[scA,]scR,scG,scB).", (object) c));
          return false;
        }
        PropertyInfo declaredProperty = ((Type) typeof (Colors)).GetTypeInfo().GetDeclaredProperty(c);
        if ((object) declaredProperty != null)
        {
          color = (Color) declaredProperty.GetValue((object) null);
          return true;
        }
        if (throwOnFail)
          throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color.", (object) c));
        return false;
      }
    }

    public static Color FromStringUsingXamlReader(string c) => (Color) XamlReader.Load(string.Format("<Color xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">{0}</Color>", (object) c));

    public static Color FromRgb(double red, double green, double blue) => Color.FromArgb(byte.MaxValue, (byte) ((double) byte.MaxValue * red), (byte) ((double) byte.MaxValue * green), (byte) ((double) byte.MaxValue * blue));

    public static Color FromHsl(double hue, double saturation, double lightness, double alpha = 1.0)
    {
      double num1 = (1.0 - Math.Abs(2.0 * lightness - 1.0)) * saturation;
      double num2 = hue / 60.0;
      double num3 = num1 * (1.0 - Math.Abs(num2 % 2.0 - 1.0));
      double num4 = lightness - 0.5 * num1;
      double num5;
      double num6;
      double num7;
      if (num2 < 1.0)
      {
        num5 = num1;
        num6 = num3;
        num7 = 0.0;
      }
      else if (num2 < 2.0)
      {
        num5 = num3;
        num6 = num1;
        num7 = 0.0;
      }
      else if (num2 < 3.0)
      {
        num5 = 0.0;
        num6 = num1;
        num7 = num3;
      }
      else if (num2 < 4.0)
      {
        num5 = 0.0;
        num6 = num3;
        num7 = num1;
      }
      else if (num2 < 5.0)
      {
        num5 = num3;
        num6 = 0.0;
        num7 = num1;
      }
      else
      {
        num5 = num1;
        num6 = 0.0;
        num7 = num3;
      }
      byte r = (byte) ((double) byte.MaxValue * (num5 + num4));
      byte g = (byte) ((double) byte.MaxValue * (num6 + num4));
      byte b = (byte) ((double) byte.MaxValue * (num7 + num4));
      return Color.FromArgb((byte) ((double) byte.MaxValue * alpha), r, g, b);
    }

    public static Color FromHsv(double hue, double saturation, double value, double alpha = 1.0)
    {
      double num1 = value * saturation;
      double num2 = hue / 60.0;
      double num3 = num1 * (1.0 - Math.Abs(num2 % 2.0 - 1.0));
      double num4 = value - num1;
      double num5;
      double num6;
      double num7;
      if (num2 < 1.0)
      {
        num5 = num1;
        num6 = num3;
        num7 = 0.0;
      }
      else if (num2 < 2.0)
      {
        num5 = num3;
        num6 = num1;
        num7 = 0.0;
      }
      else if (num2 < 3.0)
      {
        num5 = 0.0;
        num6 = num1;
        num7 = num3;
      }
      else if (num2 < 4.0)
      {
        num5 = 0.0;
        num6 = num3;
        num7 = num1;
      }
      else if (num2 < 5.0)
      {
        num5 = num3;
        num6 = 0.0;
        num7 = num1;
      }
      else
      {
        num5 = num1;
        num6 = 0.0;
        num7 = num3;
      }
      byte r = (byte) ((double) byte.MaxValue * (num5 + num4));
      byte g = (byte) ((double) byte.MaxValue * (num6 + num4));
      byte b = (byte) ((double) byte.MaxValue * (num7 + num4));
      return Color.FromArgb((byte) ((double) byte.MaxValue * alpha), r, g, b);
    }

    public static HslColor ToHsl(this Color rgba)
    {
      double val1 = 1.0 / (double) byte.MaxValue * (double) rgba.R;
      double val2_1 = 1.0 / (double) byte.MaxValue * (double) rgba.G;
      double val2_2 = 1.0 / (double) byte.MaxValue * (double) rgba.B;
      double num1 = Math.Max(Math.Max(val1, val2_1), val2_2);
      double num2 = Math.Min(Math.Min(val1, val2_1), val2_2);
      double num3 = num1 - num2;
      double num4 = num3 != 0.0 ? (num1 != val1 ? (num1 != val2_1 ? 4.0 + (val1 - val2_1) / num3 : 2.0 + (val2_2 - val1) / num3) : (val2_1 - val2_2) / num3 % 6.0) : 0.0;
      double num5 = 0.5 * (num1 - num2);
      double num6 = num3 == 0.0 ? 0.0 : num3 / (1.0 - Math.Abs(2.0 * num5 - 1.0));
      HslColor hsl;
      hsl.H = 60.0 * num4;
      hsl.S = num6;
      hsl.L = num5;
      hsl.A = 1.0 / (double) byte.MaxValue * (double) rgba.A;
      return hsl;
    }

    public static HsvColor ToHsv(this Color rgba)
    {
      double val1 = 1.0 / (double) byte.MaxValue * (double) rgba.R;
      double val2_1 = 1.0 / (double) byte.MaxValue * (double) rgba.G;
      double val2_2 = 1.0 / (double) byte.MaxValue * (double) rgba.B;
      double num1 = Math.Max(Math.Max(val1, val2_1), val2_2);
      double num2 = Math.Min(Math.Min(val1, val2_1), val2_2);
      double num3 = num1 - num2;
      double num4 = num3 != 0.0 ? (num1 != val1 ? (num1 != val2_1 ? 4.0 + (val1 - val2_1) / num3 : 2.0 + (val2_2 - val1) / num3) : (val2_1 - val2_2) / num3 % 6.0) : 0.0;
      double num5 = 0.5 * (num1 - num2);
      double num6 = num3 == 0.0 ? 0.0 : num3 / (1.0 - Math.Abs(2.0 * num5 - 1.0));
      HsvColor hsv;
      hsv.H = 60.0 * num4;
      hsv.S = num6;
      hsv.V = num1;
      hsv.A = 1.0 / (double) byte.MaxValue * (double) rgba.A;
      return hsv;
    }

    public static int IntColorFromBytes(byte a, byte r, byte g, byte b) => (int) a << 24 | (int) r << 16 | (int) g << 8 | (int) b;

    public static int[] ToPixels(this byte[] bytes)
    {
      int[] pixels = new int[bytes.Length >> 2];
      int index1 = 0;
      for (int index2 = 0; index2 < bytes.Length; index2 += 4)
      {
        pixels[index1] = ColorExtensions.IntColorFromBytes(bytes[index2 + 3], bytes[index2 + 2], bytes[index2 + 1], bytes[index2]);
        ++index1;
      }
      return pixels;
    }

    public static byte[] ToBytes(this int[] pixels)
    {
      byte[] bytes = new byte[pixels.Length << 2];
      return pixels.ToBytes(bytes);
    }

    public static byte[] ToBytes(this int[] pixels, byte[] bytes)
    {
      int index1 = 0;
      for (int index2 = 0; index2 < bytes.Length; index2 += 4)
      {
        bytes[index2 + 3] = (byte) (pixels[index1] >> 24 & (int) byte.MaxValue);
        bytes[index2 + 2] = (byte) (pixels[index1] >> 16 & (int) byte.MaxValue);
        bytes[index2 + 1] = (byte) (pixels[index1] >> 8 & (int) byte.MaxValue);
        bytes[index2] = (byte) (pixels[index1] & (int) byte.MaxValue);
        ++index1;
      }
      return bytes;
    }

    public static byte MaxDiff(this int pixel, int color) => Math.Max(Math.Max(Math.Max((byte) Math.Abs((pixel >> 24 & (int) byte.MaxValue) - (color >> 24 & (int) byte.MaxValue)), (byte) Math.Abs((pixel >> 16 & (int) byte.MaxValue) - (color >> 16 & (int) byte.MaxValue))), (byte) Math.Abs((pixel >> 8 & (int) byte.MaxValue) - (color >> 8 & (int) byte.MaxValue))), (byte) Math.Abs((pixel & (int) byte.MaxValue) - (color & (int) byte.MaxValue)));

    public static List<Color> GetNamedColors() => ((Type) typeof (Colors)).GetTypeInfo().DeclaredProperties.Select<PropertyInfo, Color>((Func<PropertyInfo, Color>) (pi => (Color) pi.GetValue((object) null))).ToList<Color>();

    public static List<string> GetColorNames() => ((Type) typeof (Colors)).GetTypeInfo().DeclaredProperties.Select<PropertyInfo, string>((Func<PropertyInfo, string>) (pi => pi.Name)).ToList<string>();
  }
}
