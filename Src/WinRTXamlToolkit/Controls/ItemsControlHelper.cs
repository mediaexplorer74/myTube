// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ItemsControlHelper
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls
{
  internal sealed class ItemsControlHelper
  {
    private Panel _itemsHost;
    private ScrollViewer _scrollHost;

    private ItemsControl ItemsControl { get; set; }

    internal Panel ItemsHost
    {
      get
      {
        if (this._itemsHost == null && this.ItemsControl != null && this.ItemsControl.ItemContainerGenerator != null)
        {
          DependencyObject dependencyObject = this.ItemsControl.ContainerFromIndex(0);
          if (dependencyObject != null)
            this._itemsHost = VisualTreeHelper.GetParent(dependencyObject) as Panel;
        }
        return this._itemsHost;
      }
    }

    internal ScrollViewer ScrollHost
    {
      get
      {
        if (this._scrollHost == null)
        {
          Panel itemsHost = this.ItemsHost;
          if (itemsHost != null)
          {
            for (DependencyObject dependencyObject = (DependencyObject) itemsHost; dependencyObject != this.ItemsControl && dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
            {
              if (dependencyObject is ScrollViewer scrollViewer)
              {
                this._scrollHost = scrollViewer;
                break;
              }
            }
          }
        }
        return this._scrollHost;
      }
    }

    internal ItemsControlHelper(ItemsControl control) => this.ItemsControl = control;

    internal void OnApplyTemplate()
    {
      this._itemsHost = (Panel) null;
      this._scrollHost = (ScrollViewer) null;
    }

    internal static void PrepareContainerForItemOverride(
      DependencyObject element,
      Style parentItemContainerStyle)
    {
      Control control = element as Control;
      if (parentItemContainerStyle == null || control == null || ((FrameworkElement) control).Style != null)
        return;
      ((DependencyObject) control).SetValue(FrameworkElement.StyleProperty, (object) parentItemContainerStyle);
    }

    internal void UpdateItemContainerStyle(Style itemContainerStyle)
    {
      if (itemContainerStyle == null)
        return;
      Panel itemsHost = this.ItemsHost;
      if (itemsHost == null || itemsHost.Children == null)
        return;
      foreach (UIElement child in (IEnumerable<UIElement>) itemsHost.Children)
      {
        FrameworkElement frameworkElement = child as FrameworkElement;
        if (frameworkElement.Style == null)
          frameworkElement.put_Style(itemContainerStyle);
      }
    }

    internal void ScrollIntoView(FrameworkElement element, Thickness? margin = null)
    {
      ScrollViewer scrollHost = this.ScrollHost;
      if (scrollHost == null)
        return;
      GeneralTransform visual;
      try
      {
        visual = ((UIElement) element).TransformToVisual((UIElement) scrollHost);
      }
      catch (ArgumentException ex)
      {
        return;
      }
      Rect rect = !margin.HasValue ? new Rect((Point) visual.TransformPoint((Point) new Point()), (Point) visual.TransformPoint((Point) new Point(element.ActualWidth, element.ActualHeight))) : new Rect((Point) visual.TransformPoint((Point) new Point(-margin.Value.Left, -margin.Value.Top)), (Point) visual.TransformPoint((Point) new Point(element.ActualWidth + margin.Value.Left + margin.Value.Right, element.ActualHeight + margin.Value.Top + margin.Value.Bottom)));
      double verticalOffset = scrollHost.VerticalOffset;
      double num1 = 0.0;
      double viewportHeight = scrollHost.ViewportHeight;
      double bottom = rect.Bottom;
      if (viewportHeight < bottom)
      {
        num1 = bottom - viewportHeight;
        verticalOffset += num1;
      }
      double top = rect.Top;
      if (top - num1 < 0.0)
        verticalOffset -= num1 - top;
      scrollHost.ScrollToVerticalOffsetWithAnimationAsync(verticalOffset);
      double horizontalOffset = scrollHost.HorizontalOffset;
      double num2 = 0.0;
      double viewportWidth = scrollHost.ViewportWidth;
      double right = rect.Right;
      if (viewportWidth < right)
      {
        num2 = right - viewportWidth;
        horizontalOffset += num2;
      }
      double left = rect.Left;
      if (left - num2 < 0.0)
        horizontalOffset -= num2 - left;
      scrollHost.ScrollToHorizontalOffsetWithAnimationAsync(horizontalOffset);
    }
  }
}
