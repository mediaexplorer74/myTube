// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.ScrollViewerExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class ScrollViewerExtensions
  {
    public static async Task ScrollToVerticalOffsetAsync(
      this ScrollViewer scrollViewer,
      double offset)
    {
      if (offset < 0.0)
        offset = 0.0;
      if (offset > scrollViewer.ScrollableHeight)
        offset = scrollViewer.ScrollableHeight;
      double currentOffset = scrollViewer.VerticalOffset;
      if (offset == currentOffset || !scrollViewer.ChangeView((double?) new double?(), (double?) new double?(offset), (float?) new float?()) || scrollViewer.VerticalOffset == offset || scrollViewer.VerticalOffset != currentOffset)
        return;
      object obj = await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>((Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>) new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)), (Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)));
    }

    public static async Task ScrollToHorizontalOffsetAsync(
      this ScrollViewer scrollViewer,
      double offset)
    {
      if (offset < 0.0)
        offset = 0.0;
      if (offset > scrollViewer.ScrollableWidth)
        offset = scrollViewer.ScrollableWidth;
      double currentOffset = scrollViewer.HorizontalOffset;
      if (offset == currentOffset || !scrollViewer.ChangeView((double?) new double?(offset), (double?) new double?(), (float?) new float?(), true) || scrollViewer.HorizontalOffset == offset || scrollViewer.HorizontalOffset != currentOffset)
        return;
      object obj = await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>((Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>) new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)), (Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)));
    }

    public static async Task ScrollToVerticalOffsetWithAnimationAsync(
      this ScrollViewer scrollViewer,
      double offset)
    {
      if (offset < 0.0)
        offset = 0.0;
      if (offset > scrollViewer.ScrollableHeight)
        offset = scrollViewer.ScrollableHeight;
      double currentOffset = scrollViewer.VerticalOffset;
      if (offset == currentOffset)
        return;
      await scrollViewer.ScrollToVerticalOffsetWithAnimation(offset);
      if (scrollViewer.VerticalOffset == offset || scrollViewer.VerticalOffset != currentOffset)
        return;
      object obj = await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>((Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>) new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)), (Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)));
    }

    public static async Task ScrollToHorizontalOffsetWithAnimationAsync(
      this ScrollViewer scrollViewer,
      double offset)
    {
      if (offset < 0.0)
        offset = 0.0;
      if (offset > scrollViewer.ScrollableWidth)
        offset = scrollViewer.ScrollableWidth;
      double currentOffset = scrollViewer.HorizontalOffset;
      if (offset == currentOffset)
        return;
      await scrollViewer.ScrollToHorizontalOffsetWithAnimation(offset);
      if (scrollViewer.HorizontalOffset == offset || scrollViewer.HorizontalOffset != currentOffset)
        return;
      object obj = await EventAsync.FromEvent<ScrollViewerViewChangedEventArgs>((Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>) new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)), (Action<EventHandler<ScrollViewerViewChangedEventArgs>>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), eh)));
    }
  }
}
