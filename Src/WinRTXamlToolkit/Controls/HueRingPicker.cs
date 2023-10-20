// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.HueRingPicker
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Async;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Imaging;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_HueRingImage", Type = typeof (Image))]
  [TemplatePart(Name = "PART_RingThumb", Type = typeof (RingSlice))]
  [TemplatePart(Name = "PART_ContainerGrid", Type = typeof (Grid))]
  public class HueRingPicker : RangeBase
  {
    private const string ContainerGridName = "PART_ContainerGrid";
    private const string HueRingImageName = "PART_HueRingImage";
    private const string RingThumbName = "PART_RingThumb";
    private Grid _containerGrid;
    private Image _hueRingImage;
    private RingSlice _ringThumb;
    private bool _isLoaded;
    private AsyncAutoResetEvent _bitmapUpdateRequired = new AsyncAutoResetEvent();
    public static readonly DependencyProperty RingThicknessProperty = DependencyProperty.Register(nameof (RingThickness), (Type) typeof (double), (Type) typeof (HueRingPicker), new PropertyMetadata((object) 40.0, new PropertyChangedCallback(HueRingPicker.OnRingThicknessChanged)));
    public static readonly DependencyProperty ThumbArcAngleProperty = DependencyProperty.Register(nameof (ThumbArcAngle), (Type) typeof (double), (Type) typeof (HueRingPicker), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(HueRingPicker.OnThumbArcAngleChanged)));
    public static readonly DependencyProperty ThumbBorderBrushProperty = DependencyProperty.Register(nameof (ThumbBorderBrush), (Type) typeof (Brush), (Type) typeof (HueRingPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(HueRingPicker.OnThumbBorderBrushChanged)));
    public static readonly DependencyProperty ThumbBorderThicknessProperty = DependencyProperty.Register(nameof (ThumbBorderThickness), (Type) typeof (double), (Type) typeof (HueRingPicker), new PropertyMetadata((object) 2.0, new PropertyChangedCallback(HueRingPicker.OnThumbBorderThicknessChanged)));
    public static readonly DependencyProperty ThumbBackgroundProperty = DependencyProperty.Register(nameof (ThumbBackground), (Type) typeof (Brush), (Type) typeof (HueRingPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(HueRingPicker.OnThumbBackgroundChanged)));

    public double RingThickness
    {
      get => (double) ((DependencyObject) this).GetValue(HueRingPicker.RingThicknessProperty);
      set => ((DependencyObject) this).SetValue(HueRingPicker.RingThicknessProperty, (object) value);
    }

    private static void OnRingThicknessChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HueRingPicker hueRingPicker = (HueRingPicker) d;
      double oldValue = (double) e.OldValue;
      double ringThickness = hueRingPicker.RingThickness;
      hueRingPicker.OnRingThicknessChanged(oldValue, ringThickness);
    }

    private void OnRingThicknessChanged(double oldRingThickness, double newRingThickness) => this.UpdateVisuals();

    public double ThumbArcAngle
    {
      get => (double) ((DependencyObject) this).GetValue(HueRingPicker.ThumbArcAngleProperty);
      set => ((DependencyObject) this).SetValue(HueRingPicker.ThumbArcAngleProperty, (object) value);
    }

    private static void OnThumbArcAngleChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HueRingPicker hueRingPicker = (HueRingPicker) d;
      double oldValue = (double) e.OldValue;
      double thumbArcAngle = hueRingPicker.ThumbArcAngle;
      hueRingPicker.OnThumbArcAngleChanged(oldValue, thumbArcAngle);
    }

    private void OnThumbArcAngleChanged(double oldThumbArcAngle, double newThumbArcAngle)
    {
      if (newThumbArcAngle < 0.0 || newThumbArcAngle > 180.0)
        throw new ArgumentException("ThumbArcAngle only supports values in the 0..180 range.");
      this.UpdateRingThumb();
    }

    public Brush ThumbBorderBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(HueRingPicker.ThumbBorderBrushProperty);
      set => ((DependencyObject) this).SetValue(HueRingPicker.ThumbBorderBrushProperty, (object) value);
    }

    private static void OnThumbBorderBrushChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HueRingPicker hueRingPicker = (HueRingPicker) d;
      Brush oldValue = (Brush) e.OldValue;
      Brush thumbBorderBrush = hueRingPicker.ThumbBorderBrush;
      hueRingPicker.OnThumbBorderBrushChanged(oldValue, thumbBorderBrush);
    }

    private void OnThumbBorderBrushChanged(Brush oldThumbBorderBrush, Brush newThumbBorderBrush)
    {
    }

    public double ThumbBorderThickness
    {
      get => (double) ((DependencyObject) this).GetValue(HueRingPicker.ThumbBorderThicknessProperty);
      set => ((DependencyObject) this).SetValue(HueRingPicker.ThumbBorderThicknessProperty, (object) value);
    }

    private static void OnThumbBorderThicknessChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HueRingPicker hueRingPicker = (HueRingPicker) d;
      double oldValue = (double) e.OldValue;
      double thumbBorderThickness = hueRingPicker.ThumbBorderThickness;
      hueRingPicker.OnThumbBorderThicknessChanged(oldValue, thumbBorderThickness);
    }

    private void OnThumbBorderThicknessChanged(
      double oldThumbBorderThickness,
      double newThumbBorderThickness)
    {
      this.UpdateRingThumb();
    }

    public Brush ThumbBackground
    {
      get => (Brush) ((DependencyObject) this).GetValue(HueRingPicker.ThumbBackgroundProperty);
      set => ((DependencyObject) this).SetValue(HueRingPicker.ThumbBackgroundProperty, (object) value);
    }

    private static void OnThumbBackgroundChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      HueRingPicker hueRingPicker = (HueRingPicker) d;
      Brush oldValue = (Brush) e.OldValue;
      Brush thumbBackground = hueRingPicker.ThumbBackground;
      hueRingPicker.OnThumbBackgroundChanged(oldValue, thumbBackground);
    }

    private void OnThumbBackgroundChanged(Brush oldThumbBackground, Brush newThumbBackground)
    {
    }

    public HueRingPicker()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (HueRingPicker));
      this.InitializeMinMaxCoercion();
      HueRingPicker hueRingPicker1 = this;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) hueRingPicker1).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) hueRingPicker1).remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
      this.DelayedUpdateWorkaround();
      HueRingPicker hueRingPicker2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) hueRingPicker2).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) hueRingPicker2).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      HueRingPicker hueRingPicker3 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) hueRingPicker3).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) hueRingPicker3).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this._isLoaded = true;
      this._bitmapUpdateRequired.Set();
      this.RunBitmapUpdaterAsync();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      this._isLoaded = false;
      this._bitmapUpdateRequired.Set();
    }

    private async void RunBitmapUpdaterAsync()
    {
      do
      {
        await this._bitmapUpdateRequired.WaitAsync();
        if (this._isLoaded && this._hueRingImage != null && ((FrameworkElement) this._containerGrid).ActualHeight > 2.0 * (this.RingThickness + this.ThumbBorderThickness) && ((FrameworkElement) this._containerGrid).ActualWidth > 2.0 * (this.RingThickness + this.ThumbBorderThickness))
        {
          int hueRingSize = (int) Math.Min(((FrameworkElement) this._containerGrid).ActualWidth - 2.0 * this.ThumbBorderThickness, ((FrameworkElement) this._containerGrid).ActualHeight - 2.0 * this.ThumbBorderThickness);
          WriteableBitmap wb = new WriteableBitmap(hueRingSize, hueRingSize);
          await wb.RenderColorPickerHueRingAsync(hueRingSize / 2 - (int) this.RingThickness);
          this._hueRingImage.put_Source((ImageSource) wb);
        }
      }
      while (this._isLoaded);
    }

    private async void DelayedUpdateWorkaround()
    {
      await Task.Delay(10);
      this.UpdateRingThumb();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => this.UpdateVisuals();

    protected virtual void OnValueChanged(double oldValue, double newValue)
    {
      base.OnValueChanged(oldValue, newValue);
      this.UpdateRingThumb();
    }

    protected virtual async void OnApplyTemplate()
    {
      // ISSUE: reference to a compiler-generated method
      this.\u003C\u003En__FabricatedMethoda();
      this._containerGrid = (Grid) ((Control) this).GetTemplateChild("PART_ContainerGrid");
      this._hueRingImage = (Image) ((Control) this).GetTemplateChild("PART_HueRingImage");
      this._ringThumb = (RingSlice) ((Control) this).GetTemplateChild("PART_RingThumb");
      Image hueRingImage1 = this._hueRingImage;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) hueRingImage1).add_PointerPressed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) hueRingImage1).remove_PointerPressed), new PointerEventHandler(this.OnPointerPressed));
      Image hueRingImage2 = this._hueRingImage;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) hueRingImage2).add_PointerMoved), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) hueRingImage2).remove_PointerMoved), new PointerEventHandler(this.OnPointerMoved));
      await ((FrameworkElement) this).WaitForLoadedAsync();
      this.UpdateVisuals();
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
    {
      if (!pointerRoutedEventArgs.Pointer.IsInContact)
        return;
      this.UpdateValueForPoint((Point) pointerRoutedEventArgs.GetCurrentPoint((UIElement) this._hueRingImage).Position);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
    {
      ((UIElement) this._hueRingImage).CapturePointer(pointerRoutedEventArgs.Pointer);
      this.UpdateValueForPoint((Point) pointerRoutedEventArgs.GetCurrentPoint((UIElement) this._hueRingImage).Position);
    }

    private void UpdateValueForPoint(Point point)
    {
      Point point1 = new Point(((FrameworkElement) this._hueRingImage).ActualWidth / 2.0, ((FrameworkElement) this._hueRingImage).ActualHeight / 2.0);
      this.put_Value((Math.Atan2(point.Y - point1.Y, point.X - point1.X) * 180.0 / 3.1415926535897931 + 360.0 + 90.0) % 360.0);
    }

    private void UpdateVisuals()
    {
      this.UpdateHueRingImage();
      this.UpdateRingThumb();
    }

    private void UpdateRingThumb()
    {
      if (this._ringThumb == null || ((FrameworkElement) this._containerGrid).ActualHeight <= 2.0 * (this.RingThickness + this.ThumbBorderThickness) || ((FrameworkElement) this._containerGrid).ActualWidth <= 2.0 * (this.RingThickness + this.ThumbBorderThickness))
        return;
      double num = Math.Min(((FrameworkElement) this._containerGrid).ActualWidth - 2.0 * this.ThumbBorderThickness, ((FrameworkElement) this._containerGrid).ActualHeight - 2.0 * this.ThumbBorderThickness);
      ((FrameworkElement) this._ringThumb).put_Width(num + 2.0 * this.ThumbBorderThickness);
      ((FrameworkElement) this._ringThumb).put_Height(num + 2.0 * this.ThumbBorderThickness);
      this._ringThumb.BeginUpdate();
      this._ringThumb.Center = new Point?(new Point(num / 2.0 + this.ThumbBorderThickness, num / 2.0 + this.ThumbBorderThickness));
      this._ringThumb.Radius = (num + this.ThumbBorderThickness) / 2.0 - 1.0;
      this._ringThumb.InnerRadius = this._ringThumb.Radius - this.RingThickness - this.ThumbBorderThickness + 2.0;
      this._ringThumb.StartAngle = this.Value - this.ThumbArcAngle / 2.0;
      this._ringThumb.EndAngle = this.Value + this.ThumbArcAngle / 2.0;
      this._ringThumb.EndUpdate();
      ((Shape) this._ringThumb).put_Stroke((Brush) new SolidColorBrush((Color) ColorExtensions.FromHsv(this.Value, 0.5, 1.0)));
    }

    private void UpdateHueRingImage() => this._bitmapUpdateRequired.Set();

    private void InitializeMinMaxCoercion()
    {
      this.put_Minimum(0.0);
      this.put_Maximum(360.0);
    }

    protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
    {
      base.OnMinimumChanged(oldMinimum, newMinimum);
      if (newMinimum >= 0.0)
        return;
      this.put_Minimum(0.0);
    }

    protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
    {
      base.OnMaximumChanged(oldMaximum, newMaximum);
      if (newMaximum <= 360.0)
        return;
      this.put_Maximum(360.0);
    }
  }
}
