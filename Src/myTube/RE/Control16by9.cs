// Decompiled with JetBrains decompiler
// Type: myTube.Control16by9
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed class Control16by9 : UserControl, IComponentConnector
  {
    public static DependencyProperty AspectWidthProperty = DependencyProperty.Register(nameof (AspectWidth), typeof (double), typeof (Control16by9), new PropertyMetadata((object) 16.0, new PropertyChangedCallback(Control16by9.OnAspectWidthPropertyChanged)));
    public static DependencyProperty AspectHeightProperty = DependencyProperty.Register(nameof (AspectHeight), typeof (double), typeof (Control16by9), new PropertyMetadata((object) 9.0, new PropertyChangedCallback(Control16by9.OnAspectWidthPropertyChanged)));
    public static DependencyProperty AspectRatioProperty = DependencyProperty.Register(nameof (AspectRatio), typeof (double), typeof (Control16by9), new PropertyMetadata((object) (16.0 / 9.0), new PropertyChangedCallback(Control16by9.OnAspectRatioPropertyChanged)));
    private bool surpressLayoutUpdate;
    private bool updateOnAllowLayout;
    private double ratio;
    private Size measureSize = new Size(200.0, 200.0);
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private static void OnAspectWidthPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Control16by9 control16by9 = d as Control16by9;
      control16by9.AspectRatio = control16by9.AspectWidth / control16by9.AspectHeight;
    }

    private static void OnAspectRatioPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      Control16by9 control16by9 = d as Control16by9;
      if (!control16by9.surpressLayoutUpdate)
        ((UIElement) control16by9).InvalidateMeasure();
      else
        control16by9.updateOnAllowLayout = true;
    }

    public double AspectRatio
    {
      get => (double) ((DependencyObject) this).GetValue(Control16by9.AspectRatioProperty);
      private set => ((DependencyObject) this).SetValue(Control16by9.AspectRatioProperty, (object) value);
    }

    public double AspectWidth
    {
      get => (double) ((DependencyObject) this).GetValue(Control16by9.AspectWidthProperty);
      set => ((DependencyObject) this).SetValue(Control16by9.AspectWidthProperty, (object) value);
    }

    public double AspectHeight
    {
      get => (double) ((DependencyObject) this).GetValue(Control16by9.AspectHeightProperty);
      set => ((DependencyObject) this).SetValue(Control16by9.AspectHeightProperty, (object) value);
    }

    public Control16by9() => this.InitializeComponent();

    public void SurpressLayoutUpdate()
    {
      if (this.surpressLayoutUpdate)
        return;
      this.ratio = this.AspectRatio;
      this.surpressLayoutUpdate = true;
    }

    public void AllowLayoutUpdate()
    {
      this.surpressLayoutUpdate = false;
      if (this.updateOnAllowLayout && this.AspectRatio != this.ratio)
        ((UIElement) this).InvalidateMeasure();
      this.updateOnAllowLayout = false;
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      UIElement content = this.Content;
      return ((FrameworkElement) this).ArrangeOverride(this.measureSize);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(200.0, 200.0);
      double num = availableSize.Width / availableSize.Height;
      if (num < this.AspectRatio || double.IsInfinity(availableSize.Height))
      {
        size.Width = availableSize.Width;
        size.Height = availableSize.Width * (1.0 / this.AspectRatio);
      }
      else if (num >= this.AspectRatio || double.IsInfinity(availableSize.Width))
      {
        size.Height = availableSize.Height;
        size.Width = availableSize.Height * this.AspectRatio;
      }
      if (size.Height > availableSize.Height)
        size.Height = availableSize.Height;
      if (size.Width > availableSize.Width)
        size.Width = availableSize.Width;
      if (double.IsInfinity(size.Width) || double.IsInfinity(size.Height))
      {
        size.Width = 300.0;
        size.Height = 200.0;
      }
      if (double.IsNaN(size.Width))
        size.Width = 100.0;
      if (double.IsNaN(size.Height))
        size.Height = 100.0;
      if (this.Content != null)
        this.Content.Measure(size);
      this.measureSize = size;
      return size;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Control16by9.xaml"), (ComponentResourceLocation) 0);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
