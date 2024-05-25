// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.FrameworkElementExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class FrameworkElementExtensions
  {
    public static async Task WaitForLoadedAsync(this FrameworkElement frameworkElement)
    {
      if (((DependencyObject) frameworkElement).IsInVisualTree())
        return;
      RoutedEventArgs routedEventArgs = await EventAsync.FromRoutedEvent((Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement.add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_Loaded), eh)), (Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_Loaded), eh)));
    }

    public static async Task WaitForUnloadedAsync(this FrameworkElement frameworkElement)
    {
      RoutedEventArgs routedEventArgs = await EventAsync.FromRoutedEvent((Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement.add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_Unloaded), eh)), (Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_Unloaded), eh)));
    }

    public static async Task WaitForLayoutUpdateAsync(this FrameworkElement frameworkElement)
    {
      object obj = await EventAsync.FromEvent<object>((Action<EventHandler<object>>) (eh => WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(frameworkElement.add_LayoutUpdated), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_LayoutUpdated), eh)), (Action<EventHandler<object>>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_LayoutUpdated), eh)));
    }

    public static async Task WaitForNonZeroSizeAsync(this FrameworkElement frameworkElement)
    {
      while (frameworkElement.ActualWidth == 0.0 && frameworkElement.ActualHeight == 0.0)
      {
        TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
        SizeChangedEventHandler sceh = (SizeChangedEventHandler) null;
        sceh = (SizeChangedEventHandler) ((s, e) =>
        {
          WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_SizeChanged), sceh);
          tcs.SetResult((object) e);
        });
        WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(frameworkElement.add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(frameworkElement.remove_SizeChanged), sceh);
        object task = await tcs.Task;
      }
    }

    public static async Task WaitForImagesToLoad(
      this FrameworkElement frameworkElement,
      int millisecondsTimeout = 0)
    {
      foreach (Image image in ((DependencyObject) frameworkElement).GetDescendantsOfType<Image>())
      {
        if (image.Source != null)
        {
          if (image.Source is BitmapImage bi)
          {
            ExceptionRoutedEventArgs exceptionRoutedEventArgs = await bi.WaitForLoadedAsync(millisecondsTimeout);
          }
          else if (image.Source is WriteableBitmap wb)
            await wb.WaitForLoadedAsync(millisecondsTimeout);
        }
      }
    }
  }
}
