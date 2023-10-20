// Decompiled with JetBrains decompiler
// Type: myTube.CustomSlider
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class CustomSlider : UserControl, IComponentConnector
  {
    public DependencyProperty SliderBackgroundProperty = DependencyProperty.Register(nameof (SliderBackground), typeof (Brush), typeof (CustomSlider), new PropertyMetadata((object) new SolidColorBrush(Colors.White)));
    public DependencyProperty SliderForegroundProperty = DependencyProperty.Register(nameof (SliderForeground), typeof (Brush), typeof (CustomSlider), new PropertyMetadata((object) null));
    public DependencyProperty ThumbForegroundProperty = DependencyProperty.Register(nameof (ThumbForeground), typeof (Brush), typeof (CustomSlider), new PropertyMetadata((object) null));
    public DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double), typeof (CustomSlider), new PropertyMetadata((object) 0));
    public DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double), typeof (CustomSlider), new PropertyMetadata((object) 100));
    public DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (CustomSlider), new PropertyMetadata((object) 50.0, new PropertyChangedCallback(CustomSlider.OnValueChanged)));
    private bool isChanging;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl customSlider;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform thumbTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform recTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle thumbRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler<SliderValueChangedEventArgs> ValueChanged;

    public Brush ThumbForeground
    {
      get => (Brush) ((DependencyObject) this).GetValue(this.ThumbForegroundProperty);
      set => ((DependencyObject) this).SetValue(this.ThumbForegroundProperty, (object) value);
    }

    public double Minimum
    {
      get => (double) ((DependencyObject) this).GetValue(this.MinimumProperty);
      set => ((DependencyObject) this).SetValue(this.MinimumProperty, (object) value);
    }

    public double Maximum
    {
      get => (double) ((DependencyObject) this).GetValue(this.MaximumProperty);
      set => ((DependencyObject) this).SetValue(this.MaximumProperty, (object) value);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CustomSlider sender = d as CustomSlider;
      try
      {
        sender.setPos((double) e.NewValue);
      }
      catch
      {
      }
      if (sender.ValueChanged == null)
        return;
      try
      {
        sender.ValueChanged((object) sender, new SliderValueChangedEventArgs()
        {
          OldValue = (double) e.OldValue,
          NewValue = (double) e.NewValue
        });
      }
      catch
      {
      }
    }

    private void setPos(double val)
    {
      double Dec = MyMath.BetweenValue(this.Minimum, this.Maximum, val);
      this.recTrans.put_ScaleX(Dec);
      this.thumbTrans.put_X(MyMath.Between(0.0, ((FrameworkElement) this).ActualWidth - ((FrameworkElement) this.thumbRec).Width, Dec));
    }

    public double Value
    {
      get => (double) ((DependencyObject) this).GetValue(this.ValueProperty);
      set => ((DependencyObject) this).SetValue(this.ValueProperty, (object) value);
    }

    public Brush SliderBackground
    {
      get => (Brush) ((DependencyObject) this).GetValue(this.SliderBackgroundProperty);
      set => ((DependencyObject) this).SetValue(this.SliderBackgroundProperty, (object) value);
    }

    public Brush SliderForeground
    {
      get => (Brush) ((DependencyObject) this).GetValue(this.SliderBackgroundProperty);
      set => ((DependencyObject) this).SetValue(this.SliderBackgroundProperty, (object) value);
    }

    public bool IsChanging => this.isChanging;

    public CustomSlider()
    {
      this.InitializeComponent();
      ((UIElement) this).put_ManipulationMode((ManipulationModes) 1);
      WindowsRuntimeMarshal.AddEventHandler<ManipulationStartedEventHandler>(new Func<ManipulationStartedEventHandler, EventRegistrationToken>(((UIElement) this).add_ManipulationStarted), new Action<EventRegistrationToken>(((UIElement) this).remove_ManipulationStarted), new ManipulationStartedEventHandler(this.CustomSlider_ManipulationStarted));
      WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>(new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(((UIElement) this).add_ManipulationDelta), new Action<EventRegistrationToken>(((UIElement) this).remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.CustomSlider_ManipulationDelta));
      WindowsRuntimeMarshal.AddEventHandler<ManipulationCompletedEventHandler>(new Func<ManipulationCompletedEventHandler, EventRegistrationToken>(((UIElement) this).add_ManipulationCompleted), new Action<EventRegistrationToken>(((UIElement) this).remove_ManipulationCompleted), new ManipulationCompletedEventHandler(this.CustomSlider_ManipulationCompleted));
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.CustomSlider_SizeChanged));
    }

    private void CustomSlider_ManipulationCompleted(
      object sender,
      ManipulationCompletedRoutedEventArgs e)
    {
      this.isChanging = false;
    }

    private void CustomSlider_SizeChanged(object sender, SizeChangedEventArgs e) => this.setPos(this.Value);

    public void SetRelativeValue(double rel)
    {
      this.Value = MyMath.Between(this.Minimum, this.Maximum, rel);
      this.setPos(this.Value);
    }

    private void CustomSlider_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
      this.Value = MyMath.Clamp(this.Value + e.Delta.Translation.X / ((FrameworkElement) this).ActualWidth * (this.Maximum - this.Minimum), this.Minimum, this.Maximum);
      e.put_Handled(true);
      this.isChanging = true;
    }

    private void CustomSlider_ManipulationStarted(
      object sender,
      ManipulationStartedRoutedEventArgs e)
    {
      double x = e.Position.X;
      e.put_Handled(true);
      this.Value = MyMath.Between(this.Minimum, this.Maximum, MyMath.BetweenValue(0.0, ((FrameworkElement) this).ActualWidth, x));
      this.isChanging = true;
    }

    protected virtual void OnTapped(TappedRoutedEventArgs e)
    {
      this.Value = MyMath.Between(this.Minimum, this.Maximum, MyMath.BetweenValue(0.0, ((FrameworkElement) this).ActualWidth, e.GetPosition((UIElement) this).X));
      e.put_Handled(true);
      ((Control) this).OnTapped(e);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///CustomSlider.xaml"), (ComponentResourceLocation) 0);
      this.customSlider = (UserControl) ((FrameworkElement) this).FindName("customSlider");
      this.thumbTrans = (TranslateTransform) ((FrameworkElement) this).FindName("thumbTrans");
      this.recTrans = (ScaleTransform) ((FrameworkElement) this).FindName("recTrans");
      this.thumbRec = (Rectangle) ((FrameworkElement) this).FindName("thumbRec");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
