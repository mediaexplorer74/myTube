// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.TrianglePicker
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_SelectionCanvas", Type = typeof (Canvas))]
  [TemplatePart(Name = "PART_TouchTargetTriangle", Type = typeof (Path))]
  [TemplatePart(Name = "PART_Thumb", Type = typeof (Ellipse))]
  public class TrianglePicker : Control
  {
    private const string SelectionCanvasName = "PART_SelectionCanvas";
    private const string TouchTargetTriangleName = "PART_TouchTargetTriangle";
    private const string ThumbName = "PART_Thumb";
    public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof (X), (Type) typeof (double), (Type) typeof (TrianglePicker), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TrianglePicker.OnXChanged)));
    public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof (Y), (Type) typeof (double), (Type) typeof (TrianglePicker), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TrianglePicker.OnYChanged)));
    private Canvas _selectionCanvas;
    private Path _touchTargetTriangle;
    private Ellipse _thumb;

    public event EventHandler ValueChanged;

    public double X
    {
      get => (double) ((DependencyObject) this).GetValue(TrianglePicker.XProperty);
      set => ((DependencyObject) this).SetValue(TrianglePicker.XProperty, (object) value);
    }

    private static void OnXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      TrianglePicker trianglePicker = (TrianglePicker) d;
      double oldValue = (double) e.OldValue;
      double x = trianglePicker.X;
      trianglePicker.OnXChanged(oldValue, x);
    }

    private void OnXChanged(double oldX, double newX) => this.UpdateThumbPosition();

    public double Y
    {
      get => (double) ((DependencyObject) this).GetValue(TrianglePicker.YProperty);
      set => ((DependencyObject) this).SetValue(TrianglePicker.YProperty, (object) value);
    }

    private static void OnYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      TrianglePicker trianglePicker = (TrianglePicker) d;
      double oldValue = (double) e.OldValue;
      double y = trianglePicker.Y;
      trianglePicker.OnYChanged(oldValue, y);
    }

    private void OnYChanged(double oldY, double newY) => this.UpdateThumbPosition();

    public TrianglePicker() => this.put_DefaultStyleKey((object) typeof (TrianglePicker));

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._selectionCanvas = (Canvas) this.GetTemplateChild("PART_SelectionCanvas");
      this._touchTargetTriangle = (Path) this.GetTemplateChild("PART_TouchTargetTriangle");
      this._thumb = (Ellipse) this.GetTemplateChild("PART_Thumb");
      Canvas selectionCanvas = this._selectionCanvas;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) selectionCanvas).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) selectionCanvas).remove_SizeChanged), new SizeChangedEventHandler(this.OnSomeSizeChanged));
      Ellipse thumb = this._thumb;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) thumb).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) thumb).remove_SizeChanged), new SizeChangedEventHandler(this.OnSomeSizeChanged));
      Path touchTargetTriangle1 = this._touchTargetTriangle;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) touchTargetTriangle1).add_PointerPressed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) touchTargetTriangle1).remove_PointerPressed), new PointerEventHandler(this.OnTouchTargetPointerPressed));
      Path touchTargetTriangle2 = this._touchTargetTriangle;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) touchTargetTriangle2).add_PointerMoved), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) touchTargetTriangle2).remove_PointerMoved), new PointerEventHandler(this.OnTouchTargetPointerMoved));
      this.UpdateThumbPosition();
    }

    private void OnTouchTargetPointerPressed(object sender, PointerRoutedEventArgs e)
    {
      e.put_Handled(true);
      ((UIElement) this._touchTargetTriangle).CapturePointer(e.Pointer);
      this.UpdateSelection((Point) e.GetCurrentPoint((UIElement) this._touchTargetTriangle).Position);
    }

    private void OnTouchTargetPointerMoved(object sender, PointerRoutedEventArgs e)
    {
      if (!e.Pointer.IsInContact)
        return;
      e.put_Handled(true);
      this.UpdateSelection((Point) e.GetCurrentPoint((UIElement) this._touchTargetTriangle).Position);
    }

    private void UpdateSelection(Point position)
    {
      double actualWidth = ((FrameworkElement) this._touchTargetTriangle).ActualWidth;
      double actualHeight = ((FrameworkElement) this._touchTargetTriangle).ActualHeight;
      double num1 = position.X / actualWidth;
      double num2 = 1.0 - position.Y / actualHeight;
      double x = position.X;
      double num3 = actualHeight - position.Y;
      if (num2 > 0.0 && num2 < 2.0 * num1 && num2 < 2.0 - 2.0 * num1)
      {
        this.X = num1;
        this.Y = num2;
      }
      else if (x < 0.0 && num3 < -x * 0.5 / Math.Sqrt(0.75))
      {
        this.X = 0.0;
        this.Y = 0.0;
      }
      else if (x > actualWidth && num3 < (x - actualWidth) * 0.5 / Math.Sqrt(0.75))
      {
        this.X = 1.0;
        this.Y = 0.0;
      }
      else if (num3 - actualHeight > -(x - 0.5 * actualWidth) * 0.5 / Math.Sqrt(0.75) && num3 - actualHeight > (x - 0.5 * actualWidth) * 0.5 / Math.Sqrt(0.75))
      {
        this.X = 0.5;
        this.Y = 1.0;
      }
      else if (num2 < 0.0)
      {
        this.X = num1;
        this.Y = 0.0;
      }
      else if (num1 < 0.5)
      {
        double num4 = 2.0 * Math.Sqrt(0.75);
        double num5 = 0.5 / Math.Sqrt(0.75);
        this.X = Math.Min(0.5, Math.Max(0.0, (num3 + x * 0.5 / Math.Sqrt(0.75)) / (num4 + num5) / actualWidth));
        this.Y = this.X * num4 * actualWidth / actualHeight;
      }
      else
      {
        double num6 = 2.0 * Math.Sqrt(0.75);
        double num7 = 0.5 / Math.Sqrt(0.75);
        double num8 = Math.Min(0.5, Math.Max(0.0, (num3 - (x - actualWidth) * 0.5 / Math.Sqrt(0.75)) / (num6 + num7) / actualWidth));
        this.X = 1.0 - num8;
        this.Y = num8 * num6 * actualWidth / actualHeight;
      }
      EventHandler valueChanged = this.ValueChanged;
      if (valueChanged == null)
        return;
      valueChanged((object) this, EventArgs.Empty);
    }

    private void OnSomeSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => this.UpdateThumbPosition();

    private void UpdateThumbPosition()
    {
      Canvas.SetLeft((UIElement) this._thumb, -((FrameworkElement) this._thumb).ActualWidth * 0.5 + this.X * ((FrameworkElement) this._selectionCanvas).ActualWidth);
      Canvas.SetTop((UIElement) this._thumb, -((FrameworkElement) this._thumb).ActualHeight * 0.5 + (1.0 - this.Y) * ((FrameworkElement) this._selectionCanvas).ActualHeight);
    }
  }
}
