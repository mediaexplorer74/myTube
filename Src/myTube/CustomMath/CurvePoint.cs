// myTube.CustomMath.CurvePoint

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
