// Decompiled with JetBrains decompiler
// Type: myTube.Helpers.Accel
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Core;

namespace myTube.Helpers
{
  public static class Accel
  {
    private const string Tag = "Accel";
    private static SimpleOrientation orient;
    private static bool locked = false;
    private static Accelerometer accel;
    public static CoreDispatcher Dispatcher;
    private static Vector3 v = new Vector3();
    private const int howMany = 4;
    private static Vector3[] vs = new Vector3[4];
    private static Vector3[] vs2 = new Vector3[4];
    private static int ind = 0;
    private static int ind2 = 0;
    private static float landscapeThresh = 0.95f;
    private static float portraitThresh = 0.3f;
    private static bool running = false;
    private static SimpleOrientation lockedOrientation;

    public static event OrientChangedEventHandler OrientChanged;

    public static SimpleOrientation Orient
    {
      get => Accel.locked ? Accel.lockedOrientation : Accel.orient;
      private set => Accel.orient = value;
    }

    public static bool Locked => Accel.locked;

    static Accel() => Accel.Orient = (SimpleOrientation) 0;

    public static void Start()
    {
      if (Accel.running)
        return;
      Helper.Write((object) nameof (Accel), (object) "Starting");
      Accel.running = true;
      if (Accel.accel == null)
      {
        try
        {
          Accel.accel = Accelerometer.GetDefault();
        }
        catch
        {
        }
        if (Accel.accel != null)
        {
          for (int index = 0; index < Accel.vs.Length; ++index)
            Accel.vs[index] = Vector3.Zero;
        }
      }
      if (Accel.accel == null)
        return;
      Accelerometer accel = Accel.accel;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>>(new Func<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>, EventRegistrationToken>(accel.add_ReadingChanged), new Action<EventRegistrationToken>(accel.remove_ReadingChanged), new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>((object) null, __methodptr(accel_ReadingChanged)));
      Accel.accel.put_ReportInterval(Math.Max(33U, Accel.accel.MinimumReportInterval));
    }

    public static void Stop()
    {
      if (!Accel.running)
        return;
      Helper.Write((object) nameof (Accel), (object) "Stopping");
      Accel.running = false;
      if (Accel.accel == null)
        return;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>>(new Action<EventRegistrationToken>(Accel.accel.remove_ReadingChanged), new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>((object) null, __methodptr(accel_ReadingChanged)));
      Accel.accel.put_ReportInterval(0U);
      for (int index = 0; index < Accel.vs.Length; ++index)
      {
        Accel.vs[index] = new Vector3();
        Accel.vs2[index] = new Vector3();
      }
    }

    private static double Distance(this Vector3 v) => Math.Sqrt((double) v.X * (double) v.X + (double) v.Y * (double) v.Y + (double) v.Z * (double) v.Z);

    public static void Lock()
    {
      Accel.lockedOrientation = Accel.orient;
      Accel.locked = true;
    }

    public static void Unlock()
    {
      Accel.locked = false;
      if (Accel.lockedOrientation == Accel.orient)
        return;
      Accel.changed(Accel.orient);
    }

    private static void accel_ReadingChanged(
      Accelerometer sender,
      AccelerometerReadingChangedEventArgs args)
    {
      Vector3 vector3_1 = new Vector3((float) args.Reading.AccelerationX, (float) args.Reading.AccelerationY, (float) args.Reading.AccelerationZ);
      Accel.vs2[Accel.ind2 % Accel.vs2.Length] = vector3_1;
      ++Accel.ind2;
      if (Accel.ind2 >= Accel.vs2.Length)
        Accel.ind2 = 0;
      Vector3 zero = Vector3.Zero;
      foreach (Vector3 vector3_2 in Accel.vs2)
        zero += vector3_2;
      if ((zero / (float) Accel.vs.Length - vector3_1).Distance() < 0.3)
      {
        Accel.vs[Accel.ind % Accel.vs.Length] = vector3_1;
        ++Accel.ind;
      }
      if (Accel.ind >= Accel.vs.Length)
        Accel.ind = 0;
      Accel.v = Vector3.Zero;
      foreach (Vector3 v in Accel.vs)
        Accel.v += v;
      Accel.v /= (float) Accel.vs.Length;
      if ((double) Math.Abs(Accel.v.Z) >= 0.85000002384185791)
        return;
      float num = MyMath.Between(Accel.landscapeThresh * 0.8f, Accel.landscapeThresh, Math.Abs(Accel.v.Z / 0.85f));
      if ((double) Accel.v.X < -(double) num && Accel.Orient != 1)
        Accel.Orient = Accel.changed((SimpleOrientation) 1);
      else if ((double) Accel.v.X > (double) num && Accel.Orient != 3)
      {
        Accel.Orient = Accel.changed((SimpleOrientation) 3);
      }
      else
      {
        if (Accel.Orient == null || (double) Accel.v.X <= -(double) Accel.portraitThresh || (double) Accel.v.X >= (double) Accel.portraitThresh || (double) Accel.v.Y >= -0.4)
          return;
        Accel.Orient = Accel.changed((SimpleOrientation) 0);
      }
    }

    private static SimpleOrientation changed(SimpleOrientation o)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      Accel.\u003C\u003Ec__DisplayClass30_0 cDisplayClass300 = new Accel.\u003C\u003Ec__DisplayClass30_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass300.o = o;
      if (!Accel.locked && Accel.running && Accel.Dispatcher != null && Accel.OrientChanged != null)
      {
        // ISSUE: reference to a compiler-generated field
        Helper.Write((object) nameof (Accel), (object) ("Orientation changed to " + (object) cDisplayClass300.o));
        // ISSUE: method pointer
        Accel.Dispatcher.RunAsync((CoreDispatcherPriority) 1, new DispatchedHandler((object) cDisplayClass300, __methodptr(\u003Cchanged\u003Eb__0)));
      }
      // ISSUE: reference to a compiler-generated field
      return cDisplayClass300.o;
    }
  }
}
