// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.NumericExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices;

namespace WinRTXamlToolkit.Controls
{
  internal static class NumericExtensions
  {
    public static bool IsZero(this double value) => Math.Abs(value) < 2.2204460492503131E-15;

    public static bool IsNaN(this double value)
    {
      NumericExtensions.NanUnion nanUnion = new NumericExtensions.NanUnion()
      {
        FloatingValue = value
      };
      switch (nanUnion.IntegerValue & 18442240474082181120UL)
      {
        case 9218868437227405312:
        case 18442240474082181120:
          return (nanUnion.IntegerValue & 4503599627370495UL) != 0UL;
        default:
          return false;
      }
    }

    public static bool IsGreaterThan(double left, double right) => left > right && !NumericExtensions.AreClose(left, right);

    public static bool AreClose(double left, double right)
    {
      if (left == right)
        return true;
      double num1 = (Math.Abs(left) + Math.Abs(right) + 10.0) * 2.2204460492503131E-16;
      double num2 = left - right;
      return -num1 < num2 && num1 > num2;
    }

    public static bool IsLessThanOrClose(double left, double right) => left < right || NumericExtensions.AreClose(left, right);

    [StructLayout(LayoutKind.Explicit)]
    private struct NanUnion
    {
      [FieldOffset(0)]
      internal double FloatingValue;
      [FieldOffset(0)]
      internal ulong IntegerValue;
    }
  }
}
