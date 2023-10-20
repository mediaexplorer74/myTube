// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.CountdownControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
  public sealed class CountdownControl : UserControl, IComponentConnector
  {
    private bool _countingDown;
    public static readonly DependencyProperty SecondsProperty = DependencyProperty.Register(nameof (Seconds), (Type) typeof (int), (Type) typeof (CountdownControl), new PropertyMetadata((object) 0, new PropertyChangedCallback(CountdownControl.OnSecondsChanged)));
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid LayoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock PART_Label;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private RingSlice PART_RingSlice;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event RoutedEventHandler CountdownComplete;

    public int Seconds
    {
      get => (int) ((DependencyObject) this).GetValue(CountdownControl.SecondsProperty);
      set => ((DependencyObject) this).SetValue(CountdownControl.SecondsProperty, (object) value);
    }

    private static void OnSecondsChanged(
      DependencyObject sender,
      DependencyPropertyChangedEventArgs e)
    {
      ((CountdownControl) sender).OnSecondsChanged((int) e.OldValue, (int) e.NewValue);
    }

    private void OnSecondsChanged(int oldSeconds, int newSeconds)
    {
      if (this._countingDown || newSeconds <= 0)
        return;
      this.StartCountdownAsync(newSeconds);
    }

    public CountdownControl() => this.InitializeComponent();

    public async Task StartCountdownAsync(int seconds)
    {
      this._countingDown = true;
      this.Seconds = seconds;
      bool grow = true;
      for (; this.Seconds > 0; --this.Seconds)
      {
        Storyboard sb = new Storyboard();
        if (grow)
        {
          DoubleAnimation doubleAnimation1 = new DoubleAnimation();
          doubleAnimation1.put_From((double?) new double?(0.0));
          doubleAnimation1.put_To((double?) new double?(359.999));
          ((Timeline) doubleAnimation1).put_Duration(new Duration((TimeSpan) TimeSpan.FromSeconds(1.0)));
          doubleAnimation1.put_EnableDependentAnimation(true);
          DoubleAnimation doubleAnimation2 = doubleAnimation1;
          ((ICollection<Timeline>) sb.Children).Add((Timeline) doubleAnimation2);
          ((Timeline) sb).put_RepeatBehavior(new RepeatBehavior(1.0));
          Storyboard.SetTargetProperty((Timeline) doubleAnimation2, "EndAngle");
          Storyboard.SetTarget((Timeline) sb, (DependencyObject) this.PART_RingSlice);
        }
        else
        {
          DoubleAnimation doubleAnimation3 = new DoubleAnimation();
          doubleAnimation3.put_From((double?) new double?(0.0));
          doubleAnimation3.put_To((double?) new double?(359.999));
          ((Timeline) doubleAnimation3).put_Duration(new Duration((TimeSpan) TimeSpan.FromSeconds(1.0)));
          doubleAnimation3.put_EnableDependentAnimation(true);
          DoubleAnimation doubleAnimation4 = doubleAnimation3;
          ((ICollection<Timeline>) sb.Children).Add((Timeline) doubleAnimation4);
          ((Timeline) sb).put_RepeatBehavior(new RepeatBehavior(1.0));
          Storyboard.SetTargetProperty((Timeline) doubleAnimation4, "StartAngle");
          Storyboard.SetTarget((Timeline) sb, (DependencyObject) this.PART_RingSlice);
        }
        this.PART_Label.put_Text(this.Seconds.ToString());
        await sb.BeginAsync();
        if (grow)
        {
          this.PART_RingSlice.StartAngle = 0.0;
          this.PART_RingSlice.EndAngle = 359.999;
        }
        else
        {
          this.PART_RingSlice.StartAngle = 0.0;
          this.PART_RingSlice.EndAngle = 0.0;
        }
        grow = !grow;
      }
      this.PART_Label.put_Text(this.Seconds.ToString());
      if (this.CountdownComplete != null)
        this.CountdownComplete((object) this, new RoutedEventArgs());
      this._countingDown = false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, (Uri) new Uri("ms-appx:///WinRTXamlToolkit/Controls/CountdownControl/CountdownControl.xaml"), (ComponentResourceLocation) 1);
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.PART_Label = (TextBlock) ((FrameworkElement) this).FindName("PART_Label");
      this.PART_RingSlice = (RingSlice) ((FrameworkElement) this).FindName("PART_RingSlice");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
