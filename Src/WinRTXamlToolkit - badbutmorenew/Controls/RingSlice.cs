// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.RingSlice
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.Common;

namespace WinRTXamlToolkit.Controls
{
  public class RingSlice : Path
  {
    private bool _isUpdating;
    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), (Type) typeof (double), (Type) typeof (RingSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(RingSlice.OnStartAngleChanged)));
    public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), (Type) typeof (double), (Type) typeof (RingSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(RingSlice.OnEndAngleChanged)));
    public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof (Radius), (Type) typeof (double), (Type) typeof (RingSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(RingSlice.OnRadiusChanged)));
    public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(nameof (InnerRadius), (Type) typeof (double), (Type) typeof (RingSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(RingSlice.OnInnerRadiusChanged)));
    public static readonly DependencyProperty CenterProperty = DependencyProperty.Register(nameof (Center), (Type) typeof (Point?), (Type) typeof (RingSlice), new PropertyMetadata((object) null, new PropertyChangedCallback(RingSlice.OnCenterChanged)));

    public double StartAngle
    {
      get => (double) ((DependencyObject) this).GetValue(RingSlice.StartAngleProperty);
      set => ((DependencyObject) this).SetValue(RingSlice.StartAngleProperty, (object) value);
    }

    private static void OnStartAngleChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((RingSlice) sender).OnStartAngleChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnStartAngleChanged(double oldStartAngle, double newStartAngle) => this.UpdatePath();

    public double EndAngle
    {
      get => (double) ((DependencyObject) this).GetValue(RingSlice.EndAngleProperty);
      set => ((DependencyObject) this).SetValue(RingSlice.EndAngleProperty, (object) value);
    }

    private static void OnEndAngleChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((RingSlice) sender).OnEndAngleChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnEndAngleChanged(double oldEndAngle, double newEndAngle) => this.UpdatePath();

    public double Radius
    {
      get => (double) ((DependencyObject) this).GetValue(RingSlice.RadiusProperty);
      set => ((DependencyObject) this).SetValue(RingSlice.RadiusProperty, (object) value);
    }

    private static void OnRadiusChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((RingSlice) sender).OnRadiusChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnRadiusChanged(double oldRadius, double newRadius)
    {
      double num;
      ((FrameworkElement) this).put_Height(num = 2.0 * this.Radius);
      ((FrameworkElement) this).put_Width(num);
      this.UpdatePath();
    }

    public double InnerRadius
    {
      get => (double) ((DependencyObject) this).GetValue(RingSlice.InnerRadiusProperty);
      set => ((DependencyObject) this).SetValue(RingSlice.InnerRadiusProperty, (object) value);
    }

    private static void OnInnerRadiusChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((RingSlice) sender).OnInnerRadiusChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnInnerRadiusChanged(double oldInnerRadius, double newInnerRadius)
    {
      if (newInnerRadius < 0.0)
        throw new ArgumentException("InnerRadius can't be a negative value.", "InnerRadius");
      this.UpdatePath();
    }

    public Point? Center
    {
      get => (Point?) ((DependencyObject) this).GetValue(RingSlice.CenterProperty);
      set => ((DependencyObject) this).SetValue(RingSlice.CenterProperty, (object) value);
    }

    private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      RingSlice ringSlice = (RingSlice) d;
      Point? oldValue = (Point?) e.OldValue;
      Point? center = ringSlice.Center;
      ringSlice.OnCenterChanged(oldValue, center);
    }

    private void OnCenterChanged(Point? oldCenter, Point? newCenter) => this.UpdatePath();

    public RingSlice()
    {
      RingSlice ringSlice = this;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) ringSlice).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) ringSlice).remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
      new PropertyChangeEventSource<double>((DependencyObject) this, "StrokeThickness", (BindingMode) 1).ValueChanged += new EventHandler<double>(this.OnStrokeThicknessChanged);
    }

    private void OnStrokeThicknessChanged(object sender, double e) => this.UpdatePath();

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => this.UpdatePath();

    public void BeginUpdate() => this._isUpdating = true;

    public void EndUpdate()
    {
      this._isUpdating = false;
      this.UpdatePath();
    }

    private void UpdatePath()
    {
      double num1 = this.InnerRadius + ((Shape) this).StrokeThickness / 2.0;
      double num2 = this.Radius - ((Shape) this).StrokeThickness / 2.0;
      if (this._isUpdating || ((FrameworkElement) this).ActualWidth == 0.0 || num1 <= 0.0 || num2 < num1)
        return;
      PathGeometry pathGeometry = new PathGeometry();
      PathFigure pathFigure = new PathFigure();
      pathFigure.put_IsClosed(true);
      Point point = this.Center ?? new Point(num2 + ((Shape) this).StrokeThickness / 2.0, num2 + ((Shape) this).StrokeThickness / 2.0);
      pathFigure.put_StartPoint((Point) new Point(point.X + Math.Sin(this.StartAngle * 3.1415926535897931 / 180.0) * num1, point.Y - Math.Cos(this.StartAngle * 3.1415926535897931 / 180.0) * num1));
      ArcSegment arcSegment1 = new ArcSegment();
      arcSegment1.put_IsLargeArc(this.EndAngle - this.StartAngle >= 180.0);
      arcSegment1.put_Point((Point) new Point(point.X + Math.Sin(this.EndAngle * 3.1415926535897931 / 180.0) * num1, point.Y - Math.Cos(this.EndAngle * 3.1415926535897931 / 180.0) * num1));
      arcSegment1.put_Size((Size) new Size(num1, num1));
      arcSegment1.put_SweepDirection((SweepDirection) 1);
      LineSegment lineSegment1 = new LineSegment();
      lineSegment1.put_Point((Point) new Point(point.X + Math.Sin(this.EndAngle * 3.1415926535897931 / 180.0) * num2, point.Y - Math.Cos(this.EndAngle * 3.1415926535897931 / 180.0) * num2));
      LineSegment lineSegment2 = lineSegment1;
      ArcSegment arcSegment2 = new ArcSegment();
      arcSegment2.put_IsLargeArc(this.EndAngle - this.StartAngle >= 180.0);
      arcSegment2.put_Point((Point) new Point(point.X + Math.Sin(this.StartAngle * 3.1415926535897931 / 180.0) * num2, point.Y - Math.Cos(this.StartAngle * 3.1415926535897931 / 180.0) * num2));
      arcSegment2.put_Size((Size) new Size(num2, num2));
      arcSegment2.put_SweepDirection((SweepDirection) 0);
      ((ICollection<PathSegment>) pathFigure.Segments).Add((PathSegment) arcSegment1);
      ((ICollection<PathSegment>) pathFigure.Segments).Add((PathSegment) lineSegment2);
      ((ICollection<PathSegment>) pathFigure.Segments).Add((PathSegment) arcSegment2);
      ((ICollection<PathFigure>) pathGeometry.Figures).Add(pathFigure);
      ((UIElement) this).InvalidateArrange();
      this.put_Data((Geometry) pathGeometry);
    }
  }
}
