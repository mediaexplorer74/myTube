// Decompiled with JetBrains decompiler
// Type: myTube.Vector3
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

namespace myTube
{
  public struct Vector3
  {
    public float X;
    public float Y;
    public float Z;
    public static readonly Vector3 Zero;

    public Vector3(float x, float y, float z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public static Vector3 operator +(Vector3 v, Vector3 v1) => new Vector3(v.X + v1.X, v.Y + v1.Y, v.Z + v1.Z);

    public static Vector3 operator -(Vector3 v, Vector3 v1) => new Vector3(v.X - v1.X, v.Y - v1.Y, v.Z - v1.Z);

    public static Vector3 operator %(Vector3 v, Vector3 v1) => new Vector3(v.X % v1.X, v.Y % v1.Y, v.Z % v1.Z);

    public static Vector3 operator /(Vector3 v, Vector3 v1) => new Vector3(v.X / v1.X, v.Y / v1.Y, v.Z / v1.Z);

    public static Vector3 operator /(Vector3 v, float f) => new Vector3(v.X / f, v.Y / f, v.Z / f);

    public override string ToString() => "{ " + (object) this.X + ", " + (object) this.Y + ", " + (object) this.Z + " }";
  }
}
