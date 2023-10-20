// Decompiled with JetBrains decompiler
// Type: myTube.Cloud.CloudMath
// Assembly: CloudClasses, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F1E6CAED-50E5-43B9-9529-29F5F57F6BD5
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\CloudClasses.dll

using System;

namespace myTube.Cloud
{
  public static class CloudMath
  {
    private static Random rand = new Random();

    public static float BoundedAngle(float angle)
    {
      while ((double) angle < 0.0)
        angle += 360f;
      while ((double) angle > 360.0)
        angle -= 360f;
      return angle;
    }

    public static double BoundedAngle(double angle)
    {
      while (angle < 0.0)
        angle += 360.0;
      while (angle > 360.0)
        angle -= 360.0;
      return angle;
    }

    public static float AngleDistance(float angle, float distanceFrom) => Math.Min(Math.Min(Math.Abs(distanceFrom - angle), Math.Abs(distanceFrom - (angle + 360f))), Math.Abs(distanceFrom - (angle - 360f)));

    public static double AngleDistance(double angle, double distanceFrom) => Math.Min(Math.Min(Math.Abs(distanceFrom - angle), Math.Abs(distanceFrom - (angle + 360.0))), Math.Abs(distanceFrom - (angle - 360.0)));

    public static double Average(this double[] values)
    {
      double num1 = 0.0;
      foreach (double num2 in values)
        num1 += num2;
      return num1 / (double) values.Length;
    }

    public static int Clamp(int val, int min, int max)
    {
      if (val < min)
        return min;
      return val > max ? max : val;
    }

    public static float Clamp(float val, float min, float max)
    {
      if ((double) val < (double) min)
        return min;
      return (double) val > (double) max ? max : val;
    }

    public static double Clamp(double val, double min, double max)
    {
      if (val < min)
        return min;
      return val > max ? max : val;
    }

    public static double Matchsigns(double signNumber, double outputNumber) => signNumber > 0.0 && outputNumber < 0.0 || signNumber < 0.0 && outputNumber > 0.0 ? (outputNumber *= -1.0) : outputNumber;

    public static float RandomRange(float num1, float num2) => (float) CloudMath.rand.NextDouble() * (num2 - num1) + num1;

    public static float Between(float Val1, float Val2, float Dec) => Dec * (Val2 - Val1) + Val1;

    public static double Between(double Val1, double Val2, double Dec) => Dec * (Val2 - Val1) + Val1;

    public static float BetweenValue(float Val1, float Val2, float Between) => (float) (((double) Between - (double) Val1) / ((double) Val2 - (double) Val1));

    public static double BetweenValue(double Val1, double Val2, double Between) => (Between - Val1) / (Val2 - Val1);

    public static float Between(float Val1, float Val2, float Val11, float Val22, float between) => CloudMath.Between(Val1, Val2, CloudMath.BetweenValue(Val11, Val22, between));

    public static double Between(
      double Val1,
      double Val2,
      double Val11,
      double Val22,
      double between)
    {
      return CloudMath.Between(Val1, Val2, CloudMath.BetweenValue(Val11, Val22, between));
    }

    public static float LengthDirX(float Length, float Dir) => (float) Math.Cos((double) CloudMath.ToRadians(Dir)) * Length;

    public static double LengthDirX(double Length, double Dir) => Math.Cos(CloudMath.ToRadians(Dir)) * Length;

    public static float Distance(float x, float y) => (float) Math.Sqrt((double) x * (double) x + (double) y * (double) y);

    public static float Distance(double x, double y) => (float) Math.Sqrt(x * x + y * y);

    public static float LengthDirY(float Length, float Dir) => (float) -Math.Sin((double) CloudMath.ToRadians(Dir)) * Length;

    public static double LengthDirY(double Length, double Dir) => -Math.Sin(CloudMath.ToRadians(Dir)) * Length;

    public static float Direction(float X, float Y) => CloudMath.ToDegrees((float) Math.Atan2(-(double) Y, (double) X));

    public static double Direction(double X, double Y) => CloudMath.ToDegrees(Math.Atan2(-Y, X));

    public static float DirectionRadians(float X, float Y) => (float) Math.Atan2(-(double) Y, (double) X);

    public static float ToRadians(float Angle) => Angle * 0.0174532924f;

    public static double ToRadians(double Angle) => Angle * 0.017453292519943295;

    public static double ToDegrees(double Angle) => Angle / 0.017453292519943295;

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
