// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.AnimatingContainer
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.Controls
{
  [ContentProperty(Name = "RotatingContent")]
  public sealed class AnimatingContainer : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty RotatingContentProperty = DependencyProperty.Register(nameof (RotatingContent), (Type) typeof (object), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) null, new PropertyChangedCallback(AnimatingContainer.OnRotatingContentChanged)));
    public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(nameof (RadiusX), (Type) typeof (double), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(AnimatingContainer.OnRadiusXChanged)));
    public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(nameof (RadiusY), (Type) typeof (double), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(AnimatingContainer.OnRadiusYChanged)));
    public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(nameof (Duration), (Type) typeof (string), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) "0:0:4", new PropertyChangedCallback(AnimatingContainer.OnDurationChanged)));
    public static readonly DependencyProperty PulseScaleProperty = DependencyProperty.Register(nameof (PulseScale), (Type) typeof (double), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(AnimatingContainer.OnPulseScaleChanged)));
    public static readonly DependencyProperty AutoPlayProperty = DependencyProperty.Register(nameof (AutoPlay), (Type) typeof (bool), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) true));
    public static readonly DependencyProperty IsAnimatingProperty = DependencyProperty.Register(nameof (IsAnimating), (Type) typeof (bool), (Type) typeof (AnimatingContainer), new PropertyMetadata((object) false, new PropertyChangedCallback(AnimatingContainer.OnIsAnimatingChanged)));
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid LayoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl ContentContainer;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Storyboard RotationStoryboard;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Storyboard PulsingStoryboard;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame PulseKeyY;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame PulseKeyX;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyRightY;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyBottomY;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyLeftY;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyTopY;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyRightX;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyBottomX;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyLeftX;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private EasingDoubleKeyFrame KeyTopX;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform RotatingTransform;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform PulseTransform;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public object RotatingContent
    {
      get => ((DependencyObject) this).GetValue(AnimatingContainer.RotatingContentProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.RotatingContentProperty, value);
    }

    private static void OnRotatingContentChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AnimatingContainer animatingContainer = (AnimatingContainer) d;
      object oldValue = e.OldValue;
      object rotatingContent = animatingContainer.RotatingContent;
      animatingContainer.OnRotatingContentChanged(oldValue, rotatingContent);
    }

    private void OnRotatingContentChanged(object oldRotatingContent, object newRotatingContent) => this.ContentContainer.put_Content(newRotatingContent);

    public double RadiusX
    {
      get => (double) ((DependencyObject) this).GetValue(AnimatingContainer.RadiusXProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.RadiusXProperty, (object) value);
    }

    private static void OnRadiusXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      AnimatingContainer animatingContainer = (AnimatingContainer) d;
      double oldValue = (double) e.OldValue;
      double radiusX = animatingContainer.RadiusX;
      animatingContainer.OnRadiusXChanged(oldValue, radiusX);
    }

    private void OnRadiusXChanged(double oldRadiusX, double newRadiusX) => this.UpdateRadius();

    public double RadiusY
    {
      get => (double) ((DependencyObject) this).GetValue(AnimatingContainer.RadiusYProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.RadiusYProperty, (object) value);
    }

    private static void OnRadiusYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      AnimatingContainer animatingContainer = (AnimatingContainer) d;
      double oldValue = (double) e.OldValue;
      double radiusY = animatingContainer.RadiusY;
      animatingContainer.OnRadiusYChanged(oldValue, radiusY);
    }

    private void OnRadiusYChanged(double oldRadiusY, double newRadiusY) => this.UpdateRadius();

    public string Duration
    {
      get => (string) ((DependencyObject) this).GetValue(AnimatingContainer.DurationProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.DurationProperty, (object) value);
    }

    private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      AnimatingContainer animatingContainer = (AnimatingContainer) d;
      string oldValue = (string) e.OldValue;
      string duration = animatingContainer.Duration;
      animatingContainer.OnDurationChanged(oldValue, duration);
    }

    private void OnDurationChanged(string oldDuration, string newDuration) => this.UpdateDuration();

    public double PulseScale
    {
      get => (double) ((DependencyObject) this).GetValue(AnimatingContainer.PulseScaleProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.PulseScaleProperty, (object) value);
    }

    private static void OnPulseScaleChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AnimatingContainer animatingContainer = (AnimatingContainer) d;
      double oldValue = (double) e.OldValue;
      double pulseScale = animatingContainer.PulseScale;
      animatingContainer.OnPulseScaleChanged(oldValue, pulseScale);
    }

    private void OnPulseScaleChanged(double oldPulseScale, double newPulseScale) => this.UpdateRadius();

    public bool AutoPlay
    {
      get => (bool) ((DependencyObject) this).GetValue(AnimatingContainer.AutoPlayProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.AutoPlayProperty, (object) value);
    }

    public bool IsAnimating
    {
      get => (bool) ((DependencyObject) this).GetValue(AnimatingContainer.IsAnimatingProperty);
      set => ((DependencyObject) this).SetValue(AnimatingContainer.IsAnimatingProperty, (object) value);
    }

    private static void OnIsAnimatingChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AnimatingContainer animatingContainer = (AnimatingContainer) d;
      bool oldValue = (bool) e.OldValue;
      bool isAnimating = animatingContainer.IsAnimating;
      animatingContainer.OnIsAnimatingChanged(oldValue, isAnimating);
    }

    private void OnIsAnimatingChanged(bool oldIsAnimating, bool newIsAnimating)
    {
      if (newIsAnimating)
        this.BeginAnimations();
      else
        this.StopAnimations();
    }

    public AnimatingContainer()
    {
      this.InitializeComponent();
      AnimatingContainer animatingContainer1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) animatingContainer1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) animatingContainer1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      AnimatingContainer animatingContainer2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) animatingContainer2).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) animatingContainer2).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    public void Animate() => this.IsAnimating = true;

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (!this.AutoPlay)
        return;
      this.BeginAnimations();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e) => this.StopAnimations();

    private void BeginAnimations()
    {
      this.UpdateDuration();
      this.UpdateRadius();
      this.RotationStoryboard.Begin();
      this.PulsingStoryboard.Begin();
    }

    private void StopAnimations()
    {
      this.RotationStoryboard.Stop();
      this.PulsingStoryboard.Stop();
    }

    private void UpdateDuration()
    {
      TimeSpan timeSpan = TimeSpan.Parse(this.Duration);
      ((DoubleKeyFrame) this.KeyRightX).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds / 4.0)));
      ((DoubleKeyFrame) this.KeyRightY).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds / 4.0)));
      ((DoubleKeyFrame) this.KeyBottomX).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds / 2.0)));
      ((DoubleKeyFrame) this.KeyBottomY).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds / 2.0)));
      ((DoubleKeyFrame) this.KeyLeftX).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds * 3.0 / 4.0)));
      ((DoubleKeyFrame) this.KeyLeftY).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds * 3.0 / 4.0)));
      ((DoubleKeyFrame) this.KeyTopX).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) timeSpan));
      ((DoubleKeyFrame) this.KeyTopY).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) timeSpan));
      ((DoubleKeyFrame) this.PulseKeyX).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds / 2.0)));
      ((DoubleKeyFrame) this.PulseKeyY).put_KeyTime(KeyTime.FromTimeSpan((TimeSpan) TimeSpan.FromSeconds(timeSpan.TotalSeconds / 2.0)));
    }

    private void UpdateRadius()
    {
      ((DoubleKeyFrame) this.KeyRightX).put_Value(this.RadiusX);
      ((DoubleKeyFrame) this.KeyRightY).put_Value(this.RadiusY);
      ((DoubleKeyFrame) this.KeyBottomY).put_Value(2.0 * this.RadiusY);
      ((DoubleKeyFrame) this.KeyLeftX).put_Value(-this.RadiusX);
      ((DoubleKeyFrame) this.KeyLeftY).put_Value(this.RadiusY);
      ((DoubleKeyFrame) this.PulseKeyX).put_Value(this.PulseScale);
      ((DoubleKeyFrame) this.PulseKeyY).put_Value(this.PulseScale);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, (Uri) new Uri("ms-appx:///WinRTXamlToolkit/Controls/AnimatingContainer/AnimatingContainer.xaml"), (ComponentResourceLocation) 1);
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.ContentContainer = (ContentControl) ((FrameworkElement) this).FindName("ContentContainer");
      this.RotationStoryboard = (Storyboard) ((FrameworkElement) this).FindName("RotationStoryboard");
      this.PulsingStoryboard = (Storyboard) ((FrameworkElement) this).FindName("PulsingStoryboard");
      this.PulseKeyY = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("PulseKeyY");
      this.PulseKeyX = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("PulseKeyX");
      this.KeyRightY = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyRightY");
      this.KeyBottomY = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyBottomY");
      this.KeyLeftY = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyLeftY");
      this.KeyTopY = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyTopY");
      this.KeyRightX = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyRightX");
      this.KeyBottomX = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyBottomX");
      this.KeyLeftX = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyLeftX");
      this.KeyTopX = (EasingDoubleKeyFrame) ((FrameworkElement) this).FindName("KeyTopX");
      this.RotatingTransform = (TranslateTransform) ((FrameworkElement) this).FindName("RotatingTransform");
      this.PulseTransform = (ScaleTransform) ((FrameworkElement) this).FindName("PulseTransform");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
