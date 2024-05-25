// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Imaging.PixelMath
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

namespace WinRTXamlToolkit.Imaging
{
  public static class PixelMath
  {
    public static double Max(double v1, double v2, double v3) => v1 <= v2 ? (v2 <= v3 ? v3 : v2) : (v1 <= v3 ? v3 : v1);

    public static double Max(double v1, double v2, double v3, double v4) => v1 <= v2 ? (v2 <= v3 ? (v3 <= v4 ? v4 : v3) : (v2 <= v4 ? v4 : v2)) : (v1 <= v3 ? (v3 <= v4 ? v4 : v3) : (v1 <= v4 ? v4 : v1));

    public static double Min(double v1, double v2, double v3) => v1 >= v2 ? (v2 >= v3 ? v3 : v2) : (v1 >= v3 ? v3 : v1);

    public static double Min(double v1, double v2, double v3, double v4) => v1 >= v2 ? (v2 >= v3 ? (v3 >= v4 ? v4 : v3) : (v2 >= v4 ? v4 : v2)) : (v1 >= v3 ? (v3 >= v4 ? v4 : v3) : (v1 >= v4 ? v4 : v1));

    public static byte Max(byte v1, byte v2, byte v3) => (int) v1 <= (int) v2 ? ((int) v2 <= (int) v3 ? v3 : v2) : ((int) v1 <= (int) v3 ? v3 : v1);

    public static byte Max(byte v1, byte v2, byte v3, byte v4) => (int) v1 <= (int) v2 ? ((int) v2 <= (int) v3 ? ((int) v3 <= (int) v4 ? v4 : v3) : ((int) v2 <= (int) v4 ? v4 : v2)) : ((int) v1 <= (int) v3 ? ((int) v3 <= (int) v4 ? v4 : v3) : ((int) v1 <= (int) v4 ? v4 : v1));

    public static byte Min(byte v1, byte v2, byte v3) => (int) v1 >= (int) v2 ? ((int) v2 >= (int) v3 ? v3 : v2) : ((int) v1 >= (int) v3 ? v3 : v1);

    public static byte Min(byte v1, byte v2, byte v3, byte v4) => (int) v1 >= (int) v2 ? ((int) v2 >= (int) v3 ? ((int) v3 >= (int) v4 ? v4 : v3) : ((int) v2 >= (int) v4 ? v4 : v2)) : ((int) v1 >= (int) v3 ? ((int) v3 >= (int) v4 ? v4 : v3) : ((int) v1 >= (int) v4 ? v4 : v1));

    public static double Clamp(this double value, double min, double max)
    {
      if (value < min)
        return min;
      return value <= max ? value : max;
    }

    public static byte Clamp(this byte value, byte min, byte max)
    {
      if ((int) value < (int) min)
        return min;
      return (int) value <= (int) max ? value : max;
    }
  }
}
