// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ScrollExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
  internal static class ScrollExtensions
  {
    private const double LineChange = 16.0;

    private static void ScrollByVerticalOffset(ScrollViewer viewer, double offset)
    {
      offset += viewer.VerticalOffset;
      offset = Math.Max(Math.Min(offset, viewer.ExtentHeight), 0.0);
      viewer.ChangeView((double?) new double?(), (double?) new double?(offset), (float?) new float?());
    }

    private static void ScrollByHorizontalOffset(ScrollViewer viewer, double offset)
    {
      offset += viewer.HorizontalOffset;
      offset = Math.Max(Math.Min(offset, viewer.ExtentWidth), 0.0);
      viewer.ChangeView((double?) new double?(offset), (double?) new double?(), (float?) new float?());
    }

    public static void LineUp(this ScrollViewer viewer) => ScrollExtensions.ScrollByVerticalOffset(viewer, -16.0);

    public static void LineDown(this ScrollViewer viewer) => ScrollExtensions.ScrollByVerticalOffset(viewer, 16.0);

    public static void LineLeft(this ScrollViewer viewer) => ScrollExtensions.ScrollByHorizontalOffset(viewer, -16.0);

    public static void LineRight(this ScrollViewer viewer) => ScrollExtensions.ScrollByHorizontalOffset(viewer, 16.0);

    public static void PageUp(this ScrollViewer viewer) => ScrollExtensions.ScrollByVerticalOffset(viewer, -viewer.ViewportHeight);

    public static void PageDown(this ScrollViewer viewer) => ScrollExtensions.ScrollByVerticalOffset(viewer, viewer.ViewportHeight);

    public static void PageLeft(this ScrollViewer viewer) => ScrollExtensions.ScrollByHorizontalOffset(viewer, -viewer.ViewportWidth);

    public static void PageRight(this ScrollViewer viewer) => ScrollExtensions.ScrollByHorizontalOffset(viewer, viewer.ViewportWidth);

    public static void ScrollToTop(this ScrollViewer viewer) => viewer.ChangeView((double?) new double?(), (double?) new double?(0.0), (float?) new float?());

    public static void ScrollToBottom(this ScrollViewer viewer) => viewer.ChangeView((double?) new double?(), (double?) new double?(viewer.ExtentHeight), (float?) new float?());

    public static void GetTopAndBottom(
      this FrameworkElement element,
      FrameworkElement parent,
      out double top,
      out double bottom)
    {
      GeneralTransform visual = ((UIElement) element).TransformToVisual((UIElement) parent);
      top = ((Point) visual.TransformPoint((Point) new Point(0.0, 0.0))).Y;
      bottom = ((Point) visual.TransformPoint((Point) new Point(0.0, element.ActualHeight))).Y;
    }
  }
}
