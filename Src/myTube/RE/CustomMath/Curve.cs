// Decompiled with JetBrains decompiler
// Type: myTube.CustomMath.Curve
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.Collections.Generic;

namespace myTube.CustomMath
{
  public class Curve
  {
    private List<CurvePoint> points;

    public double MinPoint => this.points.Count == 0 ? double.NaN : this.points[0].Point;

    public double MaxPoint => this.points.Count == 0 ? double.NaN : this.points[this.points.Count - 1].Point;

    public double MaxValue
    {
      get
      {
        double maxValue = 0.0;
        using (List<CurvePoint>.Enumerator enumerator = this.points.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CurvePoint current = enumerator.Current;
            if (current.Value > maxValue)
              maxValue = current.Value;
          }
        }
        return maxValue;
      }
    }

    public double DefaultPower { get; set; }

    public Curve()
    {
      this.DefaultPower = 1.0;
      this.points = new List<CurvePoint>();
    }

    public void AddPoint(double point, double value) => this.AddPoint(point, value, double.NaN);

    public void AddPoint(double point, double value, double power) => this.AddPoint(point, value, power, power);

    public void AddPoint(double point, double value, double enteringPower, double exitingPower)
    {
      CurvePoint curvePoint = new CurvePoint(point, value, enteringPower, exitingPower);
      int num = this.points.Count - 1;
      for (int index = 0; index < this.points.Count; ++index)
      {
        if (this.points[index].Point > point)
        {
          num = index;
          break;
        }
      }
      if (num >= 0)
        this.points.Insert(num + 1, curvePoint);
      else
        this.points.Add(curvePoint);
    }

    public double GetValue(double point)
    {
      if (this.points.Count == 0)
        return double.NaN;
      int index1 = 0;
      int num1 = this.points.Count - 1;
      for (int index2 = 0; index2 < this.points.Count; ++index2)
      {
        if (this.points[index2].Point <= point)
          index1 = index2;
      }
      int index3 = index1 + 1;
      if (index3 >= this.points.Count)
        index3 = this.points.Count - 1;
      if (index1 == index3)
        return this.points[index1].Value;
      CurvePoint point1 = this.points[index1];
      CurvePoint point2 = this.points[index3];
      double num2 = 1.0 - Math.Pow(1.0 - Math.Pow(MyMath.Clamp(MyMath.BetweenValue(point1.Point, point2.Point, point), 0.0, 1.0), double.IsNaN(point2.EnteringPower) ? this.DefaultPower : point2.EnteringPower), double.IsNaN(point1.ExitingPower) ? this.DefaultPower : point1.ExitingPower);
      if (double.IsNaN(num2))
        num2 = MyMath.Clamp(MyMath.BetweenValue(point1.Point, point2.Point, point), 0.0, 1.0);
      return MyMath.Between(point1.Value, point2.Value, num2);
    }
  }
}
