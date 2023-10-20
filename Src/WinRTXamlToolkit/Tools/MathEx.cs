// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Tools.MathEx
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;

namespace WinRTXamlToolkit.Tools
{
  public static class MathEx
  {
    public static double Min(this double lv, double rv) => lv >= rv ? rv : lv;

    public static double Max(this double lv, double rv) => lv <= rv ? rv : lv;

    public static double Distance(this double lv, double rv) => Math.Abs(lv - rv);

    public static double Lerp(double start, double end, double progress) => start * (1.0 - progress) + end * progress;

    public static double Clamp(this double value, double min, double max)
    {
      if (!double.IsNaN(min) && value < min)
        value = min;
      else if (!double.IsNaN(max) && value > max)
        value = max;
      return value;
    }
  }
}
