// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.CascadingTextBlock
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  public class CascadingTextBlock : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty AnimateOnLoadedProperty = DependencyProperty.Register(nameof (AnimateOnLoaded), (Type) typeof (bool), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) true));
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), (Type) typeof (string), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) "", new PropertyChangedCallback(CascadingTextBlock.OnTextChanged)));
    public static readonly DependencyProperty TextBlockTemplateProperty = DependencyProperty.Register(nameof (TextBlockTemplate), (Type) typeof (DataTemplate), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) null));
    public static readonly DependencyProperty StartDelayProperty = DependencyProperty.Register(nameof (StartDelay), (Type) typeof (int), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty CascadeInProperty = DependencyProperty.Register(nameof (CascadeIn), (Type) typeof (bool), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) true));
    public static readonly DependencyProperty CascadeOutProperty = DependencyProperty.Register(nameof (CascadeOut), (Type) typeof (bool), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) false));
    public static readonly DependencyProperty HoldDurationProperty = DependencyProperty.Register(nameof (HoldDuration), (Type) typeof (TimeSpan), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) TimeSpan.FromSeconds(3.0)));
    public static readonly DependencyProperty HoldDurationStringProperty = DependencyProperty.Register(nameof (HoldDurationString), (Type) typeof (string), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) "0:0:3", new PropertyChangedCallback(CascadingTextBlock.OnHoldDurationStringChanged)));
    public static readonly DependencyProperty CascadeInDurationProperty = DependencyProperty.Register(nameof (CascadeInDuration), (Type) typeof (TimeSpan), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) TimeSpan.FromSeconds(1.0)));
    public static readonly DependencyProperty CascadeInDurationStringProperty = DependencyProperty.Register(nameof (CascadeInDurationString), (Type) typeof (string), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) "0:0:1", new PropertyChangedCallback(CascadingTextBlock.OnCascadeInDurationStringChanged)));
    public static readonly DependencyProperty CascadeOutDurationProperty = DependencyProperty.Register(nameof (CascadeOutDuration), (Type) typeof (TimeSpan), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) TimeSpan.FromSeconds(1.0)));
    public static readonly DependencyProperty CascadeOutDurationStringProperty = DependencyProperty.Register(nameof (CascadeOutDurationString), (Type) typeof (string), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) "0:0:1", new PropertyChangedCallback(CascadingTextBlock.OnCascadeOutDurationStringChanged)));
    public static readonly DependencyProperty CascadeIntervalProperty = DependencyProperty.Register(nameof (CascadeInterval), (Type) typeof (TimeSpan), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) TimeSpan.FromMilliseconds(100.0)));
    public static readonly DependencyProperty CascadeIntervalStringProperty = DependencyProperty.Register(nameof (CascadeIntervalString), (Type) typeof (string), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) "0:0:0.1", new PropertyChangedCallback(CascadingTextBlock.OnCascadeIntervalStringChanged)));
    public static readonly DependencyProperty CascadeInEasingFunctionProperty;
    public static readonly DependencyProperty CascadeOutEasingFunctionProperty;
    public static readonly DependencyProperty FromVerticalOffsetProperty;
    public static readonly DependencyProperty ToVerticalOffsetProperty;
    public static readonly DependencyProperty FromRotationProperty;
    public static readonly DependencyProperty ToRotationProperty;
    public static readonly DependencyProperty UseFadeProperty;
    public static readonly DependencyProperty UseRotationProperty;
    public static readonly DependencyProperty FadeInEasingFunctionProperty;
    public static readonly DependencyProperty FadeOutEasingFunctionProperty;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid LayoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public bool AnimateOnLoaded
    {
      get => (bool) ((DependencyObject) this).GetValue(CascadingTextBlock.AnimateOnLoadedProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.AnimateOnLoadedProperty, (object) value);
    }

    public string Text
    {
      get => (string) ((DependencyObject) this).GetValue(CascadingTextBlock.TextProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.TextProperty, (object) value);
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CascadingTextBlock cascadingTextBlock = (CascadingTextBlock) d;
      string oldValue = (string) e.OldValue;
      string text = cascadingTextBlock.Text;
      cascadingTextBlock.OnTextChanged(oldValue, text);
    }

    private void OnTextChanged(string oldText, string newText)
    {
    }

    public DataTemplate TextBlockTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(CascadingTextBlock.TextBlockTemplateProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.TextBlockTemplateProperty, (object) value);
    }

    public int StartDelay
    {
      get => (int) ((DependencyObject) this).GetValue(CascadingTextBlock.StartDelayProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.StartDelayProperty, (object) value);
    }

    public bool CascadeIn
    {
      get => (bool) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeInProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeInProperty, (object) value);
    }

    public bool CascadeOut
    {
      get => (bool) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeOutProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeOutProperty, (object) value);
    }

    public TimeSpan HoldDuration
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingTextBlock.HoldDurationProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.HoldDurationProperty, (object) value);
    }

    public string HoldDurationString
    {
      get => (string) ((DependencyObject) this).GetValue(CascadingTextBlock.HoldDurationStringProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.HoldDurationStringProperty, (object) value);
    }

    private static void OnHoldDurationStringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingTextBlock cascadingTextBlock = (CascadingTextBlock) d;
      string oldValue = (string) e.OldValue;
      string holdDurationString = cascadingTextBlock.HoldDurationString;
      cascadingTextBlock.OnHoldDurationStringChanged(oldValue, holdDurationString);
    }

    protected virtual void OnHoldDurationStringChanged(
      string oldHoldDurationString,
      string newHoldDurationString)
    {
      this.HoldDuration = TimeSpan.Parse(newHoldDurationString);
    }

    public TimeSpan CascadeInDuration
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeInDurationProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeInDurationProperty, (object) value);
    }

    public string CascadeInDurationString
    {
      get => (string) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeInDurationStringProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeInDurationStringProperty, (object) value);
    }

    private static void OnCascadeInDurationStringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingTextBlock cascadingTextBlock = (CascadingTextBlock) d;
      string oldValue = (string) e.OldValue;
      string inDurationString = cascadingTextBlock.CascadeInDurationString;
      cascadingTextBlock.OnCascadeInDurationStringChanged(oldValue, inDurationString);
    }

    protected virtual void OnCascadeInDurationStringChanged(
      string oldCascadeInDurationString,
      string newCascadeInDurationString)
    {
      this.CascadeInDuration = TimeSpan.Parse(newCascadeInDurationString);
    }

    public TimeSpan CascadeOutDuration
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeOutDurationProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeOutDurationProperty, (object) value);
    }

    public string CascadeOutDurationString
    {
      get => (string) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeOutDurationStringProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeOutDurationStringProperty, (object) value);
    }

    private static void OnCascadeOutDurationStringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingTextBlock cascadingTextBlock = (CascadingTextBlock) d;
      string oldValue = (string) e.OldValue;
      string outDurationString = cascadingTextBlock.CascadeOutDurationString;
      cascadingTextBlock.OnCascadeOutDurationStringChanged(oldValue, outDurationString);
    }

    protected virtual void OnCascadeOutDurationStringChanged(
      string oldCascadeOutDurationString,
      string newCascadeOutDurationString)
    {
      this.CascadeOutDuration = TimeSpan.Parse(newCascadeOutDurationString);
    }

    public TimeSpan CascadeInterval
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeIntervalProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeIntervalProperty, (object) value);
    }

    public string CascadeIntervalString
    {
      get => (string) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeIntervalStringProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeIntervalStringProperty, (object) value);
    }

    private static void OnCascadeIntervalStringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingTextBlock cascadingTextBlock = (CascadingTextBlock) d;
      string oldValue = (string) e.OldValue;
      string cascadeIntervalString = cascadingTextBlock.CascadeIntervalString;
      cascadingTextBlock.OnCascadeIntervalStringChanged(oldValue, cascadeIntervalString);
    }

    protected virtual void OnCascadeIntervalStringChanged(
      string oldCascadeIntervalString,
      string newCascadeIntervalString)
    {
      this.CascadeInterval = TimeSpan.Parse(newCascadeIntervalString);
    }

    public EasingFunctionBase CascadeInEasingFunction
    {
      get => (EasingFunctionBase) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeInEasingFunctionProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeInEasingFunctionProperty, (object) value);
    }

    public EasingFunctionBase CascadeOutEasingFunction
    {
      get => (EasingFunctionBase) ((DependencyObject) this).GetValue(CascadingTextBlock.CascadeOutEasingFunctionProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.CascadeOutEasingFunctionProperty, (object) value);
    }

    public double FromVerticalOffset
    {
      get => (double) ((DependencyObject) this).GetValue(CascadingTextBlock.FromVerticalOffsetProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.FromVerticalOffsetProperty, (object) value);
    }

    public double ToVerticalOffset
    {
      get => (double) ((DependencyObject) this).GetValue(CascadingTextBlock.ToVerticalOffsetProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.ToVerticalOffsetProperty, (object) value);
    }

    public double FromRotation
    {
      get => (double) ((DependencyObject) this).GetValue(CascadingTextBlock.FromRotationProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.FromRotationProperty, (object) value);
    }

    public double ToRotation
    {
      get => (double) ((DependencyObject) this).GetValue(CascadingTextBlock.ToRotationProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.ToRotationProperty, (object) value);
    }

    public bool UseFade
    {
      get => (bool) ((DependencyObject) this).GetValue(CascadingTextBlock.UseFadeProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.UseFadeProperty, (object) value);
    }

    public bool UseRotation
    {
      get => (bool) ((DependencyObject) this).GetValue(CascadingTextBlock.UseRotationProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.UseRotationProperty, (object) value);
    }

    public EasingFunctionBase FadeInEasingFunction
    {
      get => (EasingFunctionBase) ((DependencyObject) this).GetValue(CascadingTextBlock.FadeInEasingFunctionProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.FadeInEasingFunctionProperty, (object) value);
    }

    public EasingFunctionBase FadeOutEasingFunction
    {
      get => (EasingFunctionBase) ((DependencyObject) this).GetValue(CascadingTextBlock.FadeOutEasingFunctionProperty);
      set => ((DependencyObject) this).SetValue(CascadingTextBlock.FadeOutEasingFunctionProperty, (object) value);
    }

    public event EventHandler CascadeCompleted;

    public CascadingTextBlock()
    {
      this.InitializeComponent();
      if (DesignMode.DesignModeEnabled)
        return;
      CascadingTextBlock cascadingTextBlock = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) cascadingTextBlock).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) cascadingTextBlock).remove_Loaded), new RoutedEventHandler(this.OnCascadingTextBlockLoaded));
    }

    private void OnCascadingTextBlockLoaded(object sender, RoutedEventArgs e)
    {
      if (!this.AnimateOnLoaded)
        return;
      this.BeginCascadingTransitionAsync();
    }

    public async Task BeginCascadingTransitionAsync()
    {
      SolidColorBrush transparentBrush = new SolidColorBrush(Colors.Transparent);
      ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Clear();
      TimeSpan totalDelay = TimeSpan.FromSeconds(0.0);
      Storyboard cascadeStoryboard = new Storyboard();
      Rect rect = new Rect(-100000.0, 0.0, 0.0, 0.0);
      int j;
      for (int i = 0; i < this.Text.Length; i += j)
      {
        j = 1;
        while (i + j < this.Text.Length && this.Text[i + j] == ' ')
          ++j;
        TranslateTransform tt = new TranslateTransform();
        if (this.CascadeIn)
          tt.put_Y(this.FromVerticalOffset);
        TextBlock tb = this.CreateTextBlock(tt);
        if (i > 0)
        {
          InlineCollection inlines = tb.Inlines;
          Run run1 = new Run();
          run1.put_Text(this.Text.Substring(0, i));
          ((TextElement) run1).put_Foreground((Brush) transparentBrush);
          Run run2 = run1;
          ((ICollection<Inline>) inlines).Add((Inline) run2);
        }
        Run run = new Run();
        run.put_Text(this.Text.Substring(i, j));
        Run singleLetterRun = run;
        ((ICollection<Inline>) tb.Inlines).Add((Inline) singleLetterRun);
        if (i + j < this.Text.Length)
        {
          InlineCollection inlines = tb.Inlines;
          Run run3 = new Run();
          run3.put_Text(this.Text.Substring(i + j));
          ((TextElement) run3).put_Foreground((Brush) transparentBrush);
          Run run4 = run3;
          ((ICollection<Inline>) inlines).Add((Inline) run4);
        }
        ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Add((UIElement) tb);
        DoubleAnimationUsingKeyFrames opacityAnimation = (DoubleAnimationUsingKeyFrames) null;
        if (this.UseFade)
        {
          opacityAnimation = new DoubleAnimationUsingKeyFrames();
          if (this.CascadeIn)
            ((UIElement) tb).put_Opacity(0.0);
          Storyboard.SetTarget((Timeline) opacityAnimation, (DependencyObject) tb);
          Storyboard.SetTargetProperty((Timeline) opacityAnimation, "UIElement.Opacity");
          ((ICollection<Timeline>) cascadeStoryboard.Children).Add((Timeline) opacityAnimation);
        }
        DoubleAnimationUsingKeyFrames yAnimation = (DoubleAnimationUsingKeyFrames) null;
        if (this.CascadeIn || this.CascadeOut)
        {
          yAnimation = new DoubleAnimationUsingKeyFrames();
          Storyboard.SetTarget((Timeline) yAnimation, (DependencyObject) tt);
          Storyboard.SetTargetProperty((Timeline) yAnimation, "TranslateTransform.Y");
          ((ICollection<Timeline>) cascadeStoryboard.Children).Add((Timeline) yAnimation);
        }
        DoubleAnimationUsingKeyFrames rotationAnimation = (DoubleAnimationUsingKeyFrames) null;
        PlaneProjection planeProjection = (PlaneProjection) null;
        if (this.UseRotation)
        {
          await ((FrameworkElement) tb).WaitForNonZeroSizeAsync();
          double aw = ((FrameworkElement) tb).ActualWidth;
          double actualHeight = ((FrameworkElement) tb).ActualHeight;
          Rect characterRect = tb.GetCharacterRect(i);
          ((UIElement) tb).put_Projection((Projection) (planeProjection = new PlaneProjection()));
          planeProjection.put_CenterOfRotationX((characterRect.X + characterRect.Width / 2.0) / aw);
          if (this.CascadeIn)
            planeProjection.put_RotationY(this.FromRotation);
          rotationAnimation = new DoubleAnimationUsingKeyFrames();
          Storyboard.SetTarget((Timeline) rotationAnimation, (DependencyObject) planeProjection);
          Storyboard.SetTargetProperty((Timeline) rotationAnimation, "PlaneProjection.RotationY");
          ((ICollection<Timeline>) cascadeStoryboard.Children).Add((Timeline) rotationAnimation);
          if (this.CascadeIn)
          {
            DoubleKeyFrameCollection keyFrames1 = rotationAnimation.KeyFrames;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame1 = new DiscreteDoubleKeyFrame();
            ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) totalDelay);
            ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_Value(this.FromRotation);
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame2 = discreteDoubleKeyFrame1;
            ((ICollection<DoubleKeyFrame>) keyFrames1).Add((DoubleKeyFrame) discreteDoubleKeyFrame2);
            DoubleKeyFrameCollection keyFrames2 = rotationAnimation.KeyFrames;
            EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
            ((DoubleKeyFrame) easingDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + this.CascadeInDuration));
            easingDoubleKeyFrame1.put_EasingFunction(this.CascadeInEasingFunction);
            ((DoubleKeyFrame) easingDoubleKeyFrame1).put_Value(0.0);
            EasingDoubleKeyFrame easingDoubleKeyFrame2 = easingDoubleKeyFrame1;
            ((ICollection<DoubleKeyFrame>) keyFrames2).Add((DoubleKeyFrame) easingDoubleKeyFrame2);
          }
          if (this.CascadeOut)
          {
            DoubleKeyFrameCollection keyFrames3 = rotationAnimation.KeyFrames;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame3 = new DiscreteDoubleKeyFrame();
            ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + (this.CascadeIn ? this.CascadeInDuration : TimeSpan.Zero) + this.HoldDuration));
            ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_Value(0.0);
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame4 = discreteDoubleKeyFrame3;
            ((ICollection<DoubleKeyFrame>) keyFrames3).Add((DoubleKeyFrame) discreteDoubleKeyFrame4);
            DoubleKeyFrameCollection keyFrames4 = rotationAnimation.KeyFrames;
            EasingDoubleKeyFrame easingDoubleKeyFrame3 = new EasingDoubleKeyFrame();
            ((DoubleKeyFrame) easingDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + (this.CascadeIn ? this.CascadeInDuration : TimeSpan.Zero) + this.HoldDuration + this.CascadeOutDuration));
            easingDoubleKeyFrame3.put_EasingFunction(this.CascadeOutEasingFunction);
            ((DoubleKeyFrame) easingDoubleKeyFrame3).put_Value(this.ToRotation);
            EasingDoubleKeyFrame easingDoubleKeyFrame4 = easingDoubleKeyFrame3;
            ((ICollection<DoubleKeyFrame>) keyFrames4).Add((DoubleKeyFrame) easingDoubleKeyFrame4);
          }
        }
        if (this.CascadeIn)
        {
          DoubleKeyFrameCollection keyFrames5 = yAnimation.KeyFrames;
          DiscreteDoubleKeyFrame discreteDoubleKeyFrame5 = new DiscreteDoubleKeyFrame();
          ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_KeyTime((KeyTime) (TimeSpan) totalDelay);
          ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_Value(this.FromVerticalOffset);
          DiscreteDoubleKeyFrame discreteDoubleKeyFrame6 = discreteDoubleKeyFrame5;
          ((ICollection<DoubleKeyFrame>) keyFrames5).Add((DoubleKeyFrame) discreteDoubleKeyFrame6);
          DoubleKeyFrameCollection keyFrames6 = yAnimation.KeyFrames;
          EasingDoubleKeyFrame easingDoubleKeyFrame5 = new EasingDoubleKeyFrame();
          ((DoubleKeyFrame) easingDoubleKeyFrame5).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + this.CascadeInDuration));
          easingDoubleKeyFrame5.put_EasingFunction(this.CascadeInEasingFunction);
          ((DoubleKeyFrame) easingDoubleKeyFrame5).put_Value(0.0);
          EasingDoubleKeyFrame easingDoubleKeyFrame6 = easingDoubleKeyFrame5;
          ((ICollection<DoubleKeyFrame>) keyFrames6).Add((DoubleKeyFrame) easingDoubleKeyFrame6);
          if (this.UseFade)
          {
            DoubleKeyFrameCollection keyFrames7 = opacityAnimation.KeyFrames;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame7 = new DiscreteDoubleKeyFrame();
            ((DoubleKeyFrame) discreteDoubleKeyFrame7).put_KeyTime((KeyTime) (TimeSpan) totalDelay);
            ((DoubleKeyFrame) discreteDoubleKeyFrame7).put_Value(0.0);
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame8 = discreteDoubleKeyFrame7;
            ((ICollection<DoubleKeyFrame>) keyFrames7).Add((DoubleKeyFrame) discreteDoubleKeyFrame8);
            DoubleKeyFrameCollection keyFrames8 = opacityAnimation.KeyFrames;
            EasingDoubleKeyFrame easingDoubleKeyFrame7 = new EasingDoubleKeyFrame();
            ((DoubleKeyFrame) easingDoubleKeyFrame7).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + this.CascadeInDuration));
            easingDoubleKeyFrame7.put_EasingFunction(this.FadeInEasingFunction);
            ((DoubleKeyFrame) easingDoubleKeyFrame7).put_Value(1.0);
            EasingDoubleKeyFrame easingDoubleKeyFrame8 = easingDoubleKeyFrame7;
            ((ICollection<DoubleKeyFrame>) keyFrames8).Add((DoubleKeyFrame) easingDoubleKeyFrame8);
          }
        }
        if (this.CascadeOut)
        {
          DoubleKeyFrameCollection keyFrames9 = yAnimation.KeyFrames;
          DiscreteDoubleKeyFrame discreteDoubleKeyFrame9 = new DiscreteDoubleKeyFrame();
          ((DoubleKeyFrame) discreteDoubleKeyFrame9).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + (this.CascadeIn ? this.CascadeInDuration : TimeSpan.Zero) + this.HoldDuration));
          ((DoubleKeyFrame) discreteDoubleKeyFrame9).put_Value(0.0);
          DiscreteDoubleKeyFrame discreteDoubleKeyFrame10 = discreteDoubleKeyFrame9;
          ((ICollection<DoubleKeyFrame>) keyFrames9).Add((DoubleKeyFrame) discreteDoubleKeyFrame10);
          DoubleKeyFrameCollection keyFrames10 = yAnimation.KeyFrames;
          EasingDoubleKeyFrame easingDoubleKeyFrame9 = new EasingDoubleKeyFrame();
          ((DoubleKeyFrame) easingDoubleKeyFrame9).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + (this.CascadeIn ? this.CascadeInDuration : TimeSpan.Zero) + this.HoldDuration + this.CascadeOutDuration));
          easingDoubleKeyFrame9.put_EasingFunction(this.CascadeOutEasingFunction);
          ((DoubleKeyFrame) easingDoubleKeyFrame9).put_Value(this.ToVerticalOffset);
          EasingDoubleKeyFrame easingDoubleKeyFrame10 = easingDoubleKeyFrame9;
          ((ICollection<DoubleKeyFrame>) keyFrames10).Add((DoubleKeyFrame) easingDoubleKeyFrame10);
          if (this.UseFade)
          {
            DoubleKeyFrameCollection keyFrames11 = opacityAnimation.KeyFrames;
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame11 = new DiscreteDoubleKeyFrame();
            ((DoubleKeyFrame) discreteDoubleKeyFrame11).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + (this.CascadeIn ? this.CascadeInDuration : TimeSpan.Zero) + this.HoldDuration));
            ((DoubleKeyFrame) discreteDoubleKeyFrame11).put_Value(1.0);
            DiscreteDoubleKeyFrame discreteDoubleKeyFrame12 = discreteDoubleKeyFrame11;
            ((ICollection<DoubleKeyFrame>) keyFrames11).Add((DoubleKeyFrame) discreteDoubleKeyFrame12);
            DoubleKeyFrameCollection keyFrames12 = opacityAnimation.KeyFrames;
            EasingDoubleKeyFrame easingDoubleKeyFrame11 = new EasingDoubleKeyFrame();
            ((DoubleKeyFrame) easingDoubleKeyFrame11).put_KeyTime((KeyTime) (TimeSpan) (totalDelay + (this.CascadeIn ? this.CascadeInDuration : TimeSpan.Zero) + this.HoldDuration + this.CascadeOutDuration));
            easingDoubleKeyFrame11.put_EasingFunction(this.FadeOutEasingFunction);
            ((DoubleKeyFrame) easingDoubleKeyFrame11).put_Value(0.0);
            EasingDoubleKeyFrame easingDoubleKeyFrame12 = easingDoubleKeyFrame11;
            ((ICollection<DoubleKeyFrame>) keyFrames12).Add((DoubleKeyFrame) easingDoubleKeyFrame12);
          }
        }
        totalDelay += this.CascadeInterval;
      }
      EventHandler<object> eh = (EventHandler<object>) null;
      eh = (EventHandler<object>) ((s, e) =>
      {
        WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) cascadeStoryboard).remove_Completed), eh);
        if (this.CascadeCompleted == null)
          return;
        this.CascadeCompleted((object) this, EventArgs.Empty);
      });
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) cascadeStoryboard).add_Completed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) cascadeStoryboard).remove_Completed), eh);
      await Task.Delay(this.StartDelay);
      await cascadeStoryboard.BeginAsync();
    }

    private TextBlock CreateTextBlock(TranslateTransform tt)
    {
      TextBlock textBlock1;
      if (this.TextBlockTemplate == null)
      {
        TextBlock textBlock2 = new TextBlock();
        textBlock2.put_Foreground(((Control) this).Foreground);
        ((FrameworkElement) textBlock2).put_HorizontalAlignment((HorizontalAlignment) 0);
        ((UIElement) textBlock2).put_RenderTransform((Transform) tt);
        textBlock1 = textBlock2;
      }
      else
      {
        textBlock1 = (TextBlock) this.TextBlockTemplate.LoadContent();
        ((FrameworkElement) textBlock1).put_HorizontalAlignment((HorizontalAlignment) 0);
        ((UIElement) textBlock1).put_RenderTransform((Transform) tt);
      }
      return textBlock1;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, (Uri) new Uri("ms-appx:///WinRTXamlToolkit/Controls/CascadingTextBlock/CascadingTextBlock.xaml"), (ComponentResourceLocation) 1);
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
    }

    [DebuggerNonUserCode]
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;

    static CascadingTextBlock()
    {
      Type type1 = typeof (EasingFunctionBase);
      Type type2 = typeof (CascadingTextBlock);
      PowerEase powerEase1 = new PowerEase();
      ((EasingFunctionBase) powerEase1).put_EasingMode((EasingMode) 0);
      powerEase1.put_Power(2.0);
      PropertyMetadata propertyMetadata1 = new PropertyMetadata((object) powerEase1);
      CascadingTextBlock.CascadeInEasingFunctionProperty = DependencyProperty.Register(nameof (CascadeInEasingFunction), (Type) type1, (Type) type2, propertyMetadata1);
      Type type3 = typeof (EasingFunctionBase);
      Type type4 = typeof (CascadingTextBlock);
      PowerEase powerEase2 = new PowerEase();
      ((EasingFunctionBase) powerEase2).put_EasingMode((EasingMode) 1);
      powerEase2.put_Power(2.0);
      PropertyMetadata propertyMetadata2 = new PropertyMetadata((object) powerEase2);
      CascadingTextBlock.CascadeOutEasingFunctionProperty = DependencyProperty.Register(nameof (CascadeOutEasingFunction), (Type) type3, (Type) type4, propertyMetadata2);
      CascadingTextBlock.FromVerticalOffsetProperty = DependencyProperty.Register(nameof (FromVerticalOffset), (Type) typeof (double), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) -200.0));
      CascadingTextBlock.ToVerticalOffsetProperty = DependencyProperty.Register(nameof (ToVerticalOffset), (Type) typeof (double), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) 200.0));
      CascadingTextBlock.FromRotationProperty = DependencyProperty.Register(nameof (FromRotation), (Type) typeof (double), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) -90.0));
      CascadingTextBlock.ToRotationProperty = DependencyProperty.Register(nameof (ToRotation), (Type) typeof (double), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) 90.0));
      CascadingTextBlock.UseFadeProperty = DependencyProperty.Register(nameof (UseFade), (Type) typeof (bool), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) true));
      CascadingTextBlock.UseRotationProperty = DependencyProperty.Register(nameof (UseRotation), (Type) typeof (bool), (Type) typeof (CascadingTextBlock), new PropertyMetadata((object) true));
      Type type5 = typeof (EasingFunctionBase);
      Type type6 = typeof (CascadingTextBlock);
      PowerEase powerEase3 = new PowerEase();
      ((EasingFunctionBase) powerEase3).put_EasingMode((EasingMode) 1);
      powerEase3.put_Power(1.5);
      PropertyMetadata propertyMetadata3 = new PropertyMetadata((object) powerEase3);
      CascadingTextBlock.FadeInEasingFunctionProperty = DependencyProperty.Register(nameof (FadeInEasingFunction), (Type) type5, (Type) type6, propertyMetadata3);
      Type type7 = typeof (EasingFunctionBase);
      Type type8 = typeof (CascadingTextBlock);
      PowerEase powerEase4 = new PowerEase();
      ((EasingFunctionBase) powerEase4).put_EasingMode((EasingMode) 0);
      powerEase4.put_Power(1.5);
      PropertyMetadata propertyMetadata4 = new PropertyMetadata((object) powerEase4);
      CascadingTextBlock.FadeOutEasingFunctionProperty = DependencyProperty.Register(nameof (FadeOutEasingFunction), (Type) type7, (Type) type8, propertyMetadata4);
    }
  }
}
