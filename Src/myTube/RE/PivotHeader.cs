// Decompiled with JetBrains decompiler
// Type: myTube.PivotHeader
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.Controls.Extensions;

namespace myTube
{
  public sealed class PivotHeader : UserControl, IComponentConnector
  {
    private const double InactiveOpacity = 0.3;
    private const double rightSideOffset = 76.0;
    private const double rightSideOffsetClassic = 700.0;
    public static DependencyProperty StringsProperty = DependencyProperty.Register("Strings", typeof (string[]), typeof (PivotHeader), new PropertyMetadata((object) new string[0], new PropertyChangedCallback(PivotHeader.OnStringsPropertyChanged)));
    public static DependencyProperty IndexProperty = DependencyProperty.Register(nameof (Index), typeof (int), typeof (PivotHeader), new PropertyMetadata((object) 0, new PropertyChangedCallback(PivotHeader.OnIndexPropertyChanged)));
    private bool tapped = true;
    public OverCanvas OverCanvas;
    private TranslateTransform trans;
    private TranslateTransform trans2;
    public bool NextMoveInstant = true;
    private bool largeScreen;
    private double width;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel stackPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private double RightSideOffset => this.OverCanvas != null && this.OverCanvas.FlipStyle != FlipStyle.Pivot ? 700.0 : 76.0;

    private static void OnIndexPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PivotHeader pivot = d as PivotHeader;
      bool tapped = pivot.tapped;
      pivot.tapped = false;
      if (pivot.NextMoveInstant)
        return;
      int index = (int) e.NewValue;
      int lastIndex = (int) e.OldValue;
      bool flag1 = false;
      if (!pivot.largeScreen && (index < lastIndex || index > lastIndex && !tapped) && Math.Abs(index - lastIndex) > 1)
        flag1 = true;
      pivot.calculateWidth();
      Storyboard sb1 = new Storyboard();
      for (int index1 = 0; index1 < ((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Count; ++index1)
      {
        UIElement child = ((IList<UIElement>) ((Panel) pivot.stackPanel).Children)[index1];
        double To = 0.3;
        if (index1 == index)
          To = 1.0;
        if (index1 == index - 1 && !pivot.largeScreen && child.RenderTransform != pivot.trans2)
          To = flag1 ? 0.3 : 0.0;
        DoubleAnimation dAni = Ani.DoubleAni((DependencyObject) child, "Opacity", To, 0.3);
        sb1.Add((Timeline) dAni);
        if (index1 < index && !pivot.largeScreen)
        {
          int iCopy = index1;
          DoubleAnimation doubleAnimation = dAni;
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) doubleAnimation).add_Completed), new Action<EventRegistrationToken>(((Timeline) doubleAnimation).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
          {
            if (iCopy >= pivot.Index || pivot.largeScreen)
              return;
            child.put_RenderTransform((Transform) pivot.trans2);
            if (child.Opacity == 0.3)
              return;
            Ani.Begin((DependencyObject) child, "Opacity", 0.3, 0.3);
          }));
        }
        else if (!flag1)
          child.put_RenderTransform((Transform) pivot.trans);
        if (index1 == index && !pivot.largeScreen)
        {
          if (!flag1)
          {
            Point point;
            if (child.TransformToVisual((UIElement) pivot).TryTransform(new Point(-(child.RenderTransform as TranslateTransform).X, 0.0), ref point))
            {
              sb1.Add((Timeline) Ani.DoubleAni((DependencyObject) pivot.trans, "X", -point.X, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
              sb1.Add((Timeline) Ani.DoubleAni((DependencyObject) pivot.trans2, "X", pivot.width + pivot.RightSideOffset - point.X, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
            }
          }
          else
          {
            GeneralTransform visual = child.TransformToVisual((UIElement) pivot);
            double x = -(child.RenderTransform as TranslateTransform).X;
            if (index > lastIndex)
              x = -pivot.trans.X;
            Point point1 = new Point(x, 0.0);
            Point point2;
            ref Point local = ref point2;
            if (visual.TryTransform(point1, ref local))
            {
              DoubleAnimation doubleAnimation1;
              DoubleAnimation doubleAnimation2;
              if (index < lastIndex)
              {
                doubleAnimation1 = Ani.DoubleAni((DependencyObject) pivot.trans, "X", -point2.X - (pivot.width + pivot.RightSideOffset), 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0));
                doubleAnimation2 = Ani.DoubleAni((DependencyObject) pivot.trans2, "X", -point2.X, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0));
              }
              else
              {
                for (int index2 = 0; index2 < ((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Count; ++index2)
                {
                  UIElement child1 = ((IList<UIElement>) ((Panel) pivot.stackPanel).Children)[index2];
                  if (index2 < index)
                    child1.put_RenderTransform((Transform) pivot.trans2);
                }
                pivot.trans2.put_X(pivot.trans.X);
                pivot.trans.put_X(pivot.trans2.X - (pivot.width + pivot.RightSideOffset));
                doubleAnimation1 = Ani.DoubleAni((DependencyObject) pivot.trans, "X", -point2.X, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0));
                doubleAnimation2 = Ani.DoubleAni((DependencyObject) pivot.trans2, "X", pivot.width + pivot.RightSideOffset - point2.X, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0));
              }
              sb1.Add((Timeline) doubleAnimation1, (Timeline) doubleAnimation2);
              DoubleAnimation doubleAnimation3 = doubleAnimation2;
              WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) doubleAnimation3).add_Completed), new Action<EventRegistrationToken>(((Timeline) doubleAnimation3).remove_Completed), closure_1 ?? (closure_1 = (EventHandler<object>) ((_param1, _param2) =>
              {
                Storyboard sb2 = Ani.Animation();
                if (index < lastIndex)
                {
                  pivot.trans.put_X(pivot.trans2.X);
                  pivot.trans2.put_X(pivot.trans.X + (pivot.width + pivot.RightSideOffset));
                  for (int index3 = 0; index3 < ((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Count; ++index3)
                  {
                    UIElement child2 = ((IList<UIElement>) ((Panel) pivot.stackPanel).Children)[index3];
                    TranslateTransform translateTransform = pivot.trans;
                    bool flag2 = false;
                    if (index3 < index)
                    {
                      flag2 = true;
                      translateTransform = pivot.trans2;
                    }
                    else if (index3 > index && index3 >= lastIndex)
                      flag2 = true;
                    if (flag2)
                    {
                      child2.put_Opacity(0.0);
                      sb2.Add((Timeline) Ani.DoubleAni((DependencyObject) child2, "Opacity", 0.3, 0.3));
                    }
                    child2.put_RenderTransform((Transform) translateTransform);
                  }
                }
                sb2.Begin();
              })));
            }
          }
        }
      }
      if (index == -1)
        sb1.Add((Timeline) Ani.DoubleAni((DependencyObject) pivot.trans, "X", 100.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
      else if (index == ((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Count)
      {
        Point point;
        if (((IList<UIElement>) ((Panel) pivot.stackPanel).Children)[((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Count - 1].TransformToVisual((UIElement) pivot).TryTransform(new Point(-pivot.trans.X, 0.0), ref point))
          sb1.Add((Timeline) Ani.DoubleAni((DependencyObject) pivot.trans, "X", -point.X - 100.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
      }
      else if (pivot.largeScreen)
        sb1.Add((Timeline) Ani.DoubleAni((DependencyObject) pivot.trans, "X", 0.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
      sb1.Begin();
    }

    private static void OnStringsPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (e.NewValue == null)
        return;
      string[] newValue = e.NewValue as string[];
      PivotHeader pivot = d as PivotHeader;
      ((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Clear();
      pivot.trans.put_X(0.0);
      pivot.NextMoveInstant = true;
      for (int index = 0; index < newValue.Length; ++index)
      {
        TextBlock textBlock1 = new TextBlock();
        textBlock1.put_Text(newValue[index]);
        ((FrameworkElement) textBlock1).put_Margin(new Thickness(0.0, 0.0, index == newValue.Length - 1 ? 19.0 : 47.5, 0.0));
        ((UIElement) textBlock1).put_Opacity(index < pivot.Index ? 0.0 : (index == pivot.Index ? 1.0 : 0.3));
        ((UIElement) textBlock1).put_RenderTransform((Transform) pivot.trans);
        TextBlock d1 = textBlock1;
        ((FrameworkElement) d1).put_DataContext((object) pivot);
        TextBlock textBlock2 = d1;
        DependencyProperty foregroundProperty = TextBlock.ForegroundProperty;
        Binding binding = new Binding();
        binding.put_Path(new PropertyPath("Foreground"));
        ((FrameworkElement) textBlock2).SetBinding(foregroundProperty, (BindingBase) binding);
        int ii = index;
        TextBlock textBlock3 = d1;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) textBlock3).add_Tapped), new Action<EventRegistrationToken>(((UIElement) textBlock3).remove_Tapped), (TappedEventHandler) ((_param1, _param2) =>
        {
          if (pivot.OverCanvas == null)
            return;
          pivot.tapped = true;
          pivot.OverCanvas.ScrollToPage(ii, false, true);
        }));
        FrameworkElementExtensions.SetSystemCursor((DependencyObject) d1, (CoreCursorType) 3);
        ((ICollection<UIElement>) ((Panel) pivot.stackPanel).Children).Add((UIElement) d1);
      }
    }

    public int Index
    {
      get => (int) ((DependencyObject) this).GetValue(PivotHeader.IndexProperty);
      set => ((DependencyObject) this).SetValue(PivotHeader.IndexProperty, (object) value);
    }

    public PivotHeader()
    {
      this.InitializeComponent();
      this.trans = new TranslateTransform();
      this.trans2 = new TranslateTransform();
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((FrameworkElement) this).add_LayoutUpdated), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_LayoutUpdated), new EventHandler<object>(this.PivotHeader_LayoutUpdated));
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      Size size = ((FrameworkElement) this).MeasureOverride(availableSize);
      this.setLargeScreen(size.Width < availableSize.Width - 2.0);
      return size;
    }

    private void calculateWidth()
    {
      this.width = 0.0;
      foreach (UIElement child in (IEnumerable<UIElement>) ((Panel) this.stackPanel).Children)
      {
        FrameworkElement frameworkElement = child as FrameworkElement;
        double width = this.width;
        double actualWidth = frameworkElement.ActualWidth;
        Thickness margin = frameworkElement.Margin;
        double left = margin.Left;
        double num1 = actualWidth + left;
        margin = frameworkElement.Margin;
        double right = margin.Right;
        double num2 = num1 + right;
        this.width = width + num2;
      }
    }

    private void setLargeScreen(bool largeScreen)
    {
      if (this.largeScreen == largeScreen)
        return;
      this.largeScreen = largeScreen;
      int num = largeScreen ? 1 : 0;
    }

    private void PivotHeader_LayoutUpdated(object sender, object e)
    {
      if (!this.NextMoveInstant)
        return;
      this.calculateWidth();
      for (int index = 0; index < ((ICollection<UIElement>) ((Panel) this.stackPanel).Children).Count; ++index)
      {
        UIElement child = ((IList<UIElement>) ((Panel) this.stackPanel).Children)[index];
        child.put_Opacity(index < this.Index ? (this.largeScreen ? 0.3 : 0.3) : (index == this.Index ? 1.0 : 0.3));
        if (index < this.Index && !this.largeScreen)
          child.put_RenderTransform((Transform) this.trans2);
        else
          child.put_RenderTransform((Transform) this.trans);
        Point point;
        if (index == this.Index && !this.largeScreen && child.TransformToVisual((UIElement) this).TryTransform(new Point(-this.trans.X, 0.0), ref point))
        {
          this.trans.put_X(-point.X);
          this.trans2.put_X(this.width + this.RightSideOffset - point.X);
        }
      }
      if (this.largeScreen)
        this.trans.put_X(0.0);
      this.NextMoveInstant = false;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PivotHeader.xaml"), (ComponentResourceLocation) 0);
      this.stackPanel = (StackPanel) ((FrameworkElement) this).FindName("stackPanel");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
