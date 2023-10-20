// Decompiled with JetBrains decompiler
// Type: myTube.MyMath
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;

namespace myTube
{
  public static class MyMath
  {
    private static Random rand = new Random();

    public static double Average(this double[] values)
    {
      double num1 = 0.0;
      foreach (double num2 in values)
        num1 += num2;
      return num1 / (double) values.Length;
    }

    public static double Clamp(double val, double min, double max)
    {
      if (val < min)
        return min;
      return val > max ? max : val;
    }

    public static double Matchsigns(double signNumber, double outputNumber) => signNumber > 0.0 && outputNumber < 0.0 || signNumber < 0.0 && outputNumber > 0.0 ? (outputNumber *= -1.0) : outputNumber;

    public static float RandomRange(float num1, float num2) => (float) MyMath.rand.NextDouble() * (num2 - num1) + num1;

    public static float Between(float Val1, float Val2, float Dec) => Dec * (Val2 - Val1) + Val1;

    public static double Between(double Val1, double Val2, double Dec) => Dec * (Val2 - Val1) + Val1;

    public static float BetweenValue(float Val1, float Val2, float Between) => (float) (((double) Between - (double) Val1) / ((double) Val2 - (double) Val1));

    public static double BetweenValue(double Val1, double Val2, double Between) => (Between - Val1) / (Val2 - Val1);

    public static float Between(float Val1, float Val2, float Val11, float Val22, float between) => MyMath.Between(Val1, Val2, MyMath.BetweenValue(Val11, Val22, between));

    public static float LengthDirX(float Length, float Dir) => (float) Math.Cos((double) MyMath.ToRadians(Dir)) * Length;

    public static float Distance(float x, float y) => (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);

    public static float Distance(double x, double y) => (float) Math.Sqrt(x * x + y * y);

    public static float LengthDirY(float Length, float Dir) => (float) -Math.Sin((double) MyMath.ToRadians(Dir)) * Length;

    public static float Direction(float X, float Y) => MyMath.ToDegrees((float) Math.Atan2(-(double) Y, (double) X));

    public static float DirectionRadians(float X, float Y) => (float) Math.Atan2(-(double) Y, (double) X);

    public static float ToRadians(float Angle) => Angle * 0.0174532924f;

    public static float ToDegrees(float Angle) => Angle / 0.0174532924f;

    public static float NormalizeRadians(float val)
    {
      while ((double) val > 6.2831853071795862)
        val -= 6.28318548f;
      return val;
    }

    public static double Round(this double d) => d.Round(1.0, RoundDirection.Both);

    public static double Round(this double d, RoundDirection dir) => d.Round(1.0, dir);

    public static double Round(this double d, double roundTo) => d.Round(roundTo, RoundDirection.Both);

    public static double Round(this double d, double roundTo, RoundDirection dir)
    {
      double num = d % roundTo;
      switch (dir)
      {
        case RoundDirection.Up:
          d += roundTo - num;
          break;
        case RoundDirection.Down:
          d -= num;
          break;
        default:
          if (num > roundTo * 0.5)
          {
            d += roundTo - num;
            break;
          }
          d -= num;
          break;
      }
      return d;
    }
  }
}
