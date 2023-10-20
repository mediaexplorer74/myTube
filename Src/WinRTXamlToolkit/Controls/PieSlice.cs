// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.PieSlice
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
  public class PieSlice : Path
  {
    private bool _isUpdating;
    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register(nameof (StartAngle), (Type) typeof (double), (Type) typeof (PieSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(PieSlice.OnStartAngleChanged)));
    public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register(nameof (EndAngle), (Type) typeof (double), (Type) typeof (PieSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(PieSlice.OnEndAngleChanged)));
    public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof (Radius), (Type) typeof (double), (Type) typeof (PieSlice), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(PieSlice.OnRadiusChanged)));

    public double StartAngle
    {
      get => (double) ((DependencyObject) this).GetValue(PieSlice.StartAngleProperty);
      set => ((DependencyObject) this).SetValue(PieSlice.StartAngleProperty, (object) value);
    }

    private static void OnStartAngleChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((PieSlice) sender).OnStartAngleChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnStartAngleChanged(double oldStartAngle, double newStartAngle) => this.UpdatePath();

    public double EndAngle
    {
      get => (double) ((DependencyObject) this).GetValue(PieSlice.EndAngleProperty);
      set => ((DependencyObject) this).SetValue(PieSlice.EndAngleProperty, (object) value);
    }

    private static void OnEndAngleChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((PieSlice) sender).OnEndAngleChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnEndAngleChanged(double oldEndAngle, double newEndAngle) => this.UpdatePath();

    public double Radius
    {
      get => (double) ((DependencyObject) this).GetValue(PieSlice.RadiusProperty);
      set => ((DependencyObject) this).SetValue(PieSlice.RadiusProperty, (object) value);
    }

    private static void OnRadiusChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((PieSlice) sender).OnRadiusChanged((double) e.OldValue, (double) e.NewValue);
    }

    private void OnRadiusChanged(double oldRadius, double newRadius)
    {
      double num;
      ((FrameworkElement) this).put_Height(num = 2.0 * this.Radius);
      ((FrameworkElement) this).put_Width(num);
      this.UpdatePath();
    }

    public PieSlice()
    {
      PieSlice pieSlice = this;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) pieSlice).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) pieSlice).remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
      new PropertyChangeEventSource<double>((DependencyObject) this, "StrokeThickness", (BindingMode) 1).ValueChanged += new EventHandler<double>(this.OnStrokeThicknessChanged);
    }

    private void OnStrokeThicknessChanged(object sender, double e) => this.UpdatePath();

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) => this.UpdatePath();

    public void BeginUpdate() => this._isUpdating = true;

    public void EndUpdate()
    {
      this._isUpdating = false;
      this.UpdatePath();
    }

    private void UpdatePath()
    {
      double num = this.Radius - ((Shape) this).StrokeThickness / 2.0;
      if (this._isUpdating || ((FrameworkElement) this).ActualWidth == 0.0 || num <= 0.0)
        return;
      PathGeometry pathGeometry = new PathGeometry();
      PathFigure pathFigure = new PathFigure();
      pathFigure.put_StartPoint((Point) new Point(num, num));
      pathFigure.put_IsClosed(true);
      LineSegment lineSegment1 = new LineSegment();
      lineSegment1.put_Point((Point) new Point(num + Math.Sin(this.StartAngle * 3.1415926535897931 / 180.0) * num, num - Math.Cos(this.StartAngle * 3.1415926535897931 / 180.0) * num));
      LineSegment lineSegment2 = lineSegment1;
      ArcSegment arcSegment = new ArcSegment();
      arcSegment.put_IsLargeArc(this.EndAngle - this.StartAngle >= 180.0);
      arcSegment.put_Point((Point) new Point(num + Math.Sin(this.EndAngle * 3.1415926535897931 / 180.0) * num, num - Math.Cos(this.EndAngle * 3.1415926535897931 / 180.0) * num));
      arcSegment.put_Size((Size) new Size(num, num));
      arcSegment.put_SweepDirection((SweepDirection) 1);
      ((ICollection<PathSegment>) pathFigure.Segments).Add((PathSegment) lineSegment2);
      ((ICollection<PathSegment>) pathFigure.Segments).Add((PathSegment) arcSegment);
      ((ICollection<PathFigure>) pathGeometry.Figures).Add(pathFigure);
      this.put_Data((Geometry) pathGeometry);
      ((UIElement) this).InvalidateArrange();
    }
  }
}
