// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Behaviors.ParallaxBackgroundBehavior
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Interactivity;

namespace WinRTXamlToolkit.Controls.Behaviors
{
  public class ParallaxBackgroundBehavior : Behavior<FrameworkElement>
  {
    private const double MaxLength = 100000.0;
    public static readonly DependencyProperty BackgroundElementTemplateProperty = DependencyProperty.Register(nameof (BackgroundElementTemplate), (Type) typeof (DataTemplate), (Type) typeof (ParallaxBackgroundBehavior), new PropertyMetadata((object) null));
    private ScrollViewer _associatedScrollViewer;
    private Grid _scrollViewerRootGrid;
    private Canvas _parallaxCanvas;
    private FrameworkElement _backgroundElement;
    private bool _inUpdateBackgroundElementPosition;
    private double _backgroundElementWidth;
    private double _backgroundElementHeight;

    public DataTemplate BackgroundElementTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ParallaxBackgroundBehavior.BackgroundElementTemplateProperty);
      set => ((DependencyObject) this).SetValue(ParallaxBackgroundBehavior.BackgroundElementTemplateProperty, (object) value);
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      FrameworkElement associatedObject = this.AssociatedObject;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(associatedObject.add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(associatedObject.remove_Loaded), new RoutedEventHandler(this.OnAssociatedObjectLoaded));
    }

    protected override void OnDetaching()
    {
      base.OnDetaching();
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.AssociatedObject.remove_Loaded), new RoutedEventHandler(this.OnAssociatedObjectLoaded));
    }

    private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      this.AttachScrollViewer();
      this.AttachRootGrid();
      this.CreateBackgroundElement();
      this.CreateParallaxCanvas();
    }

    private void CreateParallaxCanvas()
    {
      this._parallaxCanvas = new Canvas();
      Grid.SetColumnSpan((FrameworkElement) this._parallaxCanvas, ((ICollection<ColumnDefinition>) this._scrollViewerRootGrid.ColumnDefinitions).Count > 0 ? ((ICollection<ColumnDefinition>) this._scrollViewerRootGrid.ColumnDefinitions).Count : 1);
      Grid.SetRowSpan((FrameworkElement) this._parallaxCanvas, ((ICollection<RowDefinition>) this._scrollViewerRootGrid.RowDefinitions).Count > 0 ? ((ICollection<RowDefinition>) this._scrollViewerRootGrid.RowDefinitions).Count : 1);
      ((ICollection<UIElement>) ((Panel) this._parallaxCanvas).Children).Add((UIElement) this._backgroundElement);
      ((IList<UIElement>) ((Panel) this._scrollViewerRootGrid).Children).Insert(0, (UIElement) this._parallaxCanvas);
      this.UpdateBackgroundElementPosition();
      Canvas parallaxCanvas = this._parallaxCanvas;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) parallaxCanvas).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) parallaxCanvas).remove_SizeChanged), new SizeChangedEventHandler(this.OnParallaxCanvasSizeChanged));
    }

    private void UpdateBackgroundElementPosition()
    {
      if (this._inUpdateBackgroundElementPosition || ((FrameworkElement) this._parallaxCanvas).ActualHeight == 0.0 || ((FrameworkElement) this._parallaxCanvas).ActualWidth == 0.0 || ((FrameworkElement) this._associatedScrollViewer).ActualHeight == 0.0 || ((FrameworkElement) this._associatedScrollViewer).ActualWidth == 0.0)
        return;
      this._inUpdateBackgroundElementPosition = true;
      try
      {
        bool flag1 = this._associatedScrollViewer.VerticalScrollMode != 0;
        bool flag2 = this._associatedScrollViewer.HorizontalScrollMode != 0;
        double num = this._associatedScrollViewer.ViewportHeight / ((FrameworkElement) this._associatedScrollViewer).ActualHeight;
        if (!flag2 || flag1)
          return;
        this._backgroundElement.put_Height(((FrameworkElement) this._parallaxCanvas).ActualHeight);
        if (this._backgroundElementWidth == 100000.0)
          this._backgroundElement.put_Width(0.5 * (this._associatedScrollViewer.ExtentWidth + this._associatedScrollViewer.ViewportWidth));
        else
          this._backgroundElement.put_Width(this._backgroundElement.Height * this._backgroundElementWidth / this._backgroundElementHeight);
        this._backgroundElement.put_Margin(new Thickness(0.0));
        Canvas.SetTop((UIElement) this._backgroundElement, 0.0);
        Canvas.SetLeft((UIElement) this._backgroundElement, this._backgroundElement.Width >= ((FrameworkElement) this._parallaxCanvas).ActualWidth ? -(this._associatedScrollViewer.HorizontalOffset / (this._associatedScrollViewer.ExtentWidth - this._associatedScrollViewer.ViewportWidth)) * (this._backgroundElement.Width - ((FrameworkElement) this._associatedScrollViewer).ActualWidth) : 0.0);
      }
      finally
      {
        this._inUpdateBackgroundElementPosition = false;
      }
    }

    private void CreateBackgroundElement()
    {
      this._backgroundElement = this.BackgroundElementTemplate != null ? this.BackgroundElementTemplate.LoadContent() as FrameworkElement : throw new InvalidOperationException("BackgroundElementTemplate needs to be defined.");
      ((UIElement) this._backgroundElement).Measure((Size) new Size(100000.0, 100000.0));
      ((UIElement) this._backgroundElement).Arrange((Rect) new Rect(0.0, 0.0, 100000.0, 100000.0));
      this._backgroundElementWidth = this._backgroundElement.ActualWidth;
      this._backgroundElementHeight = this._backgroundElement.ActualHeight;
      if (this._backgroundElement == null)
        throw new InvalidOperationException("BackgroundElementTemplate needs to be defined as a FrameworkElement.");
    }

    private void OnParallaxCanvasSizeChanged(
      object sender,
      SizeChangedEventArgs sizeChangedEventArgs)
    {
      Canvas parallaxCanvas = this._parallaxCanvas;
      RectangleGeometry rectangleGeometry1 = new RectangleGeometry();
      rectangleGeometry1.put_Rect((Rect) new Rect(0.0, 0.0, ((FrameworkElement) this._parallaxCanvas).ActualWidth, ((FrameworkElement) this._parallaxCanvas).ActualHeight));
      RectangleGeometry rectangleGeometry2 = rectangleGeometry1;
      ((UIElement) parallaxCanvas).put_Clip(rectangleGeometry2);
      this.UpdateBackgroundElementPosition();
    }

    private void AttachRootGrid()
    {
      this._scrollViewerRootGrid = ((DependencyObject) this.AssociatedObject).GetFirstDescendantOfType<Grid>();
      if (this._scrollViewerRootGrid == null)
        throw new InvalidOperationException("The ScrollViewer associated with the ParallaxBackgroundBehavior does not contain a root Grid required for the parallax effect.");
    }

    private void AttachScrollViewer()
    {
      this._associatedScrollViewer = this.AssociatedObject as ScrollViewer;
      if (this._associatedScrollViewer == null)
        this._associatedScrollViewer = ((DependencyObject) this.AssociatedObject).GetFirstDescendantOfType<ScrollViewer>();
      ScrollViewer scrollViewer = this._associatedScrollViewer != null ? this._associatedScrollViewer : throw new InvalidOperationException("ParallaxBackgroundBehavior can only be attached to ScrollViewers or elements that have a ScrollViewer in their visual tree.");
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>) new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.OnAssociatedScrollViewerViewChanged));
    }

    private void OnAssociatedScrollViewerViewChanged(
      object sender,
      ScrollViewerViewChangedEventArgs scrollViewerViewChangedEventArgs)
    {
      this.UpdateBackgroundElementPosition();
    }
  }
}
