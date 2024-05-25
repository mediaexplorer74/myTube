// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ItemsControlExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ItemsControlExtensions
  {
    public static ScrollViewer GetScrollViewer(this ItemsControl itemsControl) => ((DependencyObject) itemsControl).GetFirstDescendantOfType<ScrollViewer>();

    public static object GetFirstVisibleItem(this ItemsControl itemsControl)
    {
      int firstVisibleIndex = itemsControl.GetFirstVisibleIndex();
      if (firstVisibleIndex == -1)
        return (object) null;
      IList itemsSource = itemsControl.ItemsSource as IList;
      if (itemsControl.ItemsSource != null && itemsSource != null && itemsSource.Count > firstVisibleIndex)
        return itemsSource[firstVisibleIndex];
      if (itemsControl.Items != null && ((ICollection<object>) itemsControl.Items).Count > firstVisibleIndex)
        return ((IList<object>) itemsControl.Items)[firstVisibleIndex];
      throw new InvalidOperationException();
    }

    public static int GetFirstVisibleIndex(this ItemsControl itemsControl)
    {
      if (itemsControl.ItemsSource == null || itemsControl.ItemsSource is IEnumerable itemsSource && !itemsSource.GetEnumerator().MoveNext())
        return -1;
      Panel itemsPanelRoot = itemsControl.ItemsPanelRoot;
      if (itemsPanelRoot == null)
        throw new InvalidOperationException("Can't get first visible index from an ItemsControl with no ItemsPanel.");
      if (itemsPanelRoot is ItemsStackPanel itemsStackPanel)
        return itemsStackPanel.FirstVisibleIndex;
      if (itemsPanelRoot is ItemsWrapGrid itemsWrapGrid)
        return itemsWrapGrid.FirstVisibleIndex;
      if (((ICollection<UIElement>) itemsPanelRoot.Children).Count == 0)
        return -1;
      if (((FrameworkElement) itemsControl).ActualWidth == 0.0)
        throw new InvalidOperationException("Can't get first visible index from an ItemsControl that is not loaded or has zero size.");
      for (int index = 0; index < ((ICollection<UIElement>) itemsPanelRoot.Children).Count; ++index)
      {
        FrameworkElement child = (FrameworkElement) ((IList<UIElement>) itemsPanelRoot.Children)[index];
        Rect rect = (Rect) ((UIElement) child).TransformToVisual((UIElement) itemsControl).TransformBounds((Rect) new Rect(0.0, 0.0, child.ActualWidth, child.ActualHeight));
        if (rect.Left < ((FrameworkElement) itemsControl).ActualWidth && rect.Top < ((FrameworkElement) itemsControl).ActualHeight && rect.Right > 0.0 && rect.Bottom > 0.0)
          return itemsControl.IndexFromContainer((DependencyObject) child);
      }
      throw new InvalidOperationException();
    }

    public static void SynchronizeScrollOffset(
      this ItemsControl targetItemsControl,
      ItemsControl sourceItemsControl,
      bool throwOnFail = false)
    {
      int firstVisibleIndex = sourceItemsControl.GetFirstVisibleIndex();
      if (firstVisibleIndex == -1)
      {
        if (throwOnFail)
          throw new InvalidOperationException();
      }
      else
      {
        switch (targetItemsControl)
        {
          case ListBox listBox:
            listBox.ScrollIntoView((object) sourceItemsControl.IndexFromContainer(sourceItemsControl.ContainerFromIndex(firstVisibleIndex)));
            break;
          case ListViewBase listViewBase:
            listViewBase.ScrollIntoView((object) sourceItemsControl.IndexFromContainer(sourceItemsControl.ContainerFromIndex(firstVisibleIndex)), (ScrollIntoViewAlignment) 1);
            break;
          default:
            ScrollViewer scrollViewer = targetItemsControl.GetScrollViewer();
            if (scrollViewer == null)
              break;
            Point point = (Point) ((UIElement) targetItemsControl.ContainerFromIndex(firstVisibleIndex)).TransformToVisual((UIElement) scrollViewer).TransformPoint((Point) new Point());
            scrollViewer.ChangeView((double?) new double?(scrollViewer.HorizontalOffset + point.X), (double?) new double?(scrollViewer.VerticalOffset + point.Y), (float?) new float?());
            break;
        }
      }
    }
  }
}
