// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.FocusVisualizer
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.Common;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Controls.Extensions.Forms;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "LayoutGrid", Type = typeof (Grid))]
  public class FocusVisualizer : Control
  {
    private const string LayoutGridName = "LayoutGrid";
    private Grid _layoutGrid;
    private Rectangle _leftRectangle;
    private Rectangle _topRectangle;
    private Rectangle _rightRectangle;
    private Rectangle _bottomRectangle;
    private Rectangle[] _rectangles;
    private CompositeTransform _leftTransform;
    private CompositeTransform _topTransform;
    private CompositeTransform _rightTransform;
    private CompositeTransform _bottomTransform;
    private bool _isLoaded;
    private UIElement _focusedElement;

    public FocusTracker FocusTracker { get; private set; }

    public FocusVisualizer()
    {
      this.put_DefaultStyleKey((object) typeof (FocusVisualizer));
      new PropertyChangeEventSource<Thickness>((DependencyObject) this, "BorderThickness").ValueChanged += new EventHandler<Thickness>(this.OnBorderThicknessChanged);
      new PropertyChangeEventSource<Brush>((DependencyObject) this, "BorderBrush").ValueChanged += new EventHandler<Brush>(this.OnBorderBrushChanged);
      FocusVisualizer focusVisualizer1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) focusVisualizer1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) focusVisualizer1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      FocusVisualizer focusVisualizer2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) focusVisualizer2).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) focusVisualizer2).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
      this.FocusTracker = new FocusTracker();
      this.FocusTracker.FocusChanged += new EventHandler<UIElement>(this.OnFocusChanged);
      this._focusedElement = FocusManager.GetFocusedElement() as UIElement;
    }

    private void OnFocusChanged(object sender, UIElement uiElement)
    {
      bool useAnimation = this._focusedElement != null && uiElement != null && this._focusedElement != uiElement;
      this._focusedElement = uiElement;
      this.UpdatePosition(useAnimation);
    }

    private void OnBorderThicknessChanged(object sender, Thickness e)
    {
      this.UpdateThickness();
      this.UpdatePosition(false);
    }

    private void OnBorderBrushChanged(object sender, Brush e) => this.UpdateBrush();

    private void OnLoaded(object sender, RoutedEventArgs args)
    {
      this._isLoaded = true;
      this.UpdateThickness();
      this.UpdatePosition(false);
    }

    private void OnUnloaded(object sender, RoutedEventArgs args) => this._isLoaded = false;

    private void CreateRectangles()
    {
      this._rectangles = new Rectangle[4]
      {
        this._leftRectangle = new Rectangle(),
        this._topRectangle = new Rectangle(),
        this._rightRectangle = new Rectangle(),
        this._bottomRectangle = new Rectangle()
      };
      ((UIElement) this._leftRectangle).put_RenderTransform((Transform) (this._leftTransform = new CompositeTransform()));
      ((UIElement) this._topRectangle).put_RenderTransform((Transform) (this._topTransform = new CompositeTransform()));
      ((UIElement) this._rightRectangle).put_RenderTransform((Transform) (this._rightTransform = new CompositeTransform()));
      ((UIElement) this._bottomRectangle).put_RenderTransform((Transform) (this._bottomTransform = new CompositeTransform()));
      foreach (Rectangle rectangle in this._rectangles)
      {
        ((FrameworkElement) rectangle).put_Width(1.0);
        ((FrameworkElement) rectangle).put_Height(1.0);
        ((FrameworkElement) rectangle).put_VerticalAlignment((VerticalAlignment) 0);
        ((FrameworkElement) rectangle).put_HorizontalAlignment((HorizontalAlignment) 0);
        ((UIElement) rectangle).put_IsHitTestVisible(false);
        ((UIElement) rectangle).put_Opacity(0.0);
        ((ICollection<UIElement>) ((Panel) this._layoutGrid).Children).Add((UIElement) rectangle);
      }
    }

    private void UpdateBrush()
    {
      if (this._leftRectangle == null)
        return;
      ((Shape) this._leftRectangle).put_Fill(this.BorderBrush);
      ((Shape) this._topRectangle).put_Fill(this.BorderBrush);
      ((Shape) this._rightRectangle).put_Fill(this.BorderBrush);
      ((Shape) this._bottomRectangle).put_Fill(this.BorderBrush);
    }

    private void UpdateThickness()
    {
      if (this._leftRectangle == null)
        return;
      ((FrameworkElement) this._leftRectangle).put_Width(this.BorderThickness.Left);
      ((FrameworkElement) this._topRectangle).put_Height(this.BorderThickness.Top);
      ((FrameworkElement) this._rightRectangle).put_Width(this.BorderThickness.Right);
      ((FrameworkElement) this._bottomRectangle).put_Height(this.BorderThickness.Bottom);
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._layoutGrid = (Grid) this.GetTemplateChild("LayoutGrid");
      ((ICollection<UIElement>) ((Panel) this._layoutGrid).Children).Clear();
      this.CreateRectangles();
      this.UpdateBrush();
      this.UpdateThickness();
      this.UpdatePosition(false);
    }

    private async void UpdatePosition(bool useAnimation)
    {
      if (this._leftTransform == null)
        return;
      if (!this._isLoaded || this._focusedElement == null)
      {
        foreach (UIElement rectangle in this._rectangles)
          rectangle.put_Opacity(0.0);
        bool flag;
        int num = flag ? 1 : 0;
      }
      else
      {
        await ((FrameworkElement) this._focusedElement).WaitForLoadedAsync();
        if (!this._isLoaded)
          await ((FrameworkElement) this).WaitForLoadedAsync();
        double scale = FocusVisualizer.GetResolutionScale();
        Rect boundingRect = this._focusedElement.GetBoundingRect((UIElement) this);
        double leftRectangleLeft = boundingRect.Left - this.BorderThickness.Left;
        double leftRectangleTop = boundingRect.Top;
        double leftRectangleHeight = boundingRect.Height * scale;
        double topRectangleLeft = boundingRect.Left - this.BorderThickness.Left;
        double topRectangleTop = boundingRect.Top - this.BorderThickness.Top;
        double topRectangleWidth = (boundingRect.Width + this.BorderThickness.Left + this.BorderThickness.Right) * scale;
        double rightRectangleLeft = boundingRect.Right;
        double rightRectangleTop = boundingRect.Top;
        double rightRectangleHeight = boundingRect.Height * scale;
        double bottomRectangleLeft = boundingRect.Left - this.BorderThickness.Left;
        double bottomRectangleTop = boundingRect.Bottom;
        double bottomRectangleWidth = (boundingRect.Width + this.BorderThickness.Left + this.BorderThickness.Right) * scale;
        if (!useAnimation)
        {
          this._leftTransform.put_TranslateX(leftRectangleLeft);
          this._leftTransform.put_TranslateY(leftRectangleTop);
          this._leftTransform.put_ScaleY(leftRectangleHeight);
          this._topTransform.put_TranslateX(topRectangleLeft);
          this._topTransform.put_TranslateY(topRectangleTop);
          this._topTransform.put_ScaleX(topRectangleWidth);
          this._rightTransform.put_TranslateX(rightRectangleLeft);
          this._rightTransform.put_TranslateY(rightRectangleTop);
          this._rightTransform.put_ScaleY(rightRectangleHeight);
          this._bottomTransform.put_TranslateX(bottomRectangleLeft);
          this._bottomTransform.put_TranslateY(bottomRectangleTop);
          this._bottomTransform.put_ScaleX(bottomRectangleWidth);
          ((UIElement) this._leftRectangle).put_Opacity(1.0);
          ((UIElement) this._topRectangle).put_Opacity(1.0);
          ((UIElement) this._rightRectangle).put_Opacity(1.0);
          ((UIElement) this._bottomRectangle).put_Opacity(1.0);
        }
        double duration = useAnimation ? 0.25 : 0.0;
        Storyboard sb = new Storyboard();
        this.AddAnimation(sb, (DependencyObject) this._leftTransform, "TranslateX", leftRectangleLeft, duration);
        this.AddAnimation(sb, (DependencyObject) this._leftTransform, "TranslateY", leftRectangleTop, duration);
        this.AddAnimation(sb, (DependencyObject) this._leftTransform, "ScaleY", leftRectangleHeight, duration);
        this.AddAnimation(sb, (DependencyObject) this._topTransform, "TranslateX", topRectangleLeft, duration);
        this.AddAnimation(sb, (DependencyObject) this._topTransform, "TranslateY", topRectangleTop, duration);
        this.AddAnimation(sb, (DependencyObject) this._topTransform, "ScaleX", topRectangleWidth, duration);
        this.AddAnimation(sb, (DependencyObject) this._rightTransform, "TranslateX", rightRectangleLeft, duration);
        this.AddAnimation(sb, (DependencyObject) this._rightTransform, "TranslateY", rightRectangleTop, duration);
        this.AddAnimation(sb, (DependencyObject) this._rightTransform, "ScaleY", rightRectangleHeight, duration);
        this.AddAnimation(sb, (DependencyObject) this._bottomTransform, "TranslateX", bottomRectangleLeft, duration);
        this.AddAnimation(sb, (DependencyObject) this._bottomTransform, "TranslateY", bottomRectangleTop, duration);
        this.AddAnimation(sb, (DependencyObject) this._bottomTransform, "ScaleX", bottomRectangleWidth, duration);
        this.AddAnimation(sb, (DependencyObject) this._leftRectangle, "Opacity", 1.0, duration);
        this.AddAnimation(sb, (DependencyObject) this._topRectangle, "Opacity", 1.0, duration);
        this.AddAnimation(sb, (DependencyObject) this._rightRectangle, "Opacity", 1.0, duration);
        this.AddAnimation(sb, (DependencyObject) this._bottomRectangle, "Opacity", 1.0, duration);
        sb.Begin();
      }
    }

    private static double GetResolutionScale() => DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

    private void AddAnimation(
      Storyboard sb,
      DependencyObject target,
      string propertyName,
      double toValue,
      double duration)
    {
      DoubleAnimation doubleAnimation1 = new DoubleAnimation();
      Storyboard.SetTarget((Timeline) doubleAnimation1, target);
      Storyboard.SetTargetProperty((Timeline) doubleAnimation1, propertyName);
      doubleAnimation1.put_To((double?) new double?(toValue));
      ((Timeline) doubleAnimation1).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(duration));
      DoubleAnimation doubleAnimation2 = doubleAnimation1;
      ExponentialEase exponentialEase1 = new ExponentialEase();
      exponentialEase1.put_Exponent(10.0);
      ((EasingFunctionBase) exponentialEase1).put_EasingMode((EasingMode) 0);
      ExponentialEase exponentialEase2 = exponentialEase1;
      doubleAnimation2.put_EasingFunction((EasingFunctionBase) exponentialEase2);
      ((ICollection<Timeline>) sb.Children).Add((Timeline) doubleAnimation1);
    }
  }
}
