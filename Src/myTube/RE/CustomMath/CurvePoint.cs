// Decompiled with JetBrains decompiler
// Type: myTube.CustomMath.CurvePoint
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

namespace myTube.CustomMath
{
  public struct CurvePoint
  {
    public double Point;
    public double Value;
    public double EnteringPower;
    public double ExitingPower;

    public CurvePoint(double point, double value, double enteringPower, double exitingPower)
    {
      this.Point = point;
      this.Value = value;
      this.EnteringPower = enteringPower;
      this.ExitingPower = exitingPower;
    }
  }
}
