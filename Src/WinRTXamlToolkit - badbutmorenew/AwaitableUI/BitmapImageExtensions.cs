// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.BitmapImageExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class BitmapImageExtensions
  {
    public static async Task<ExceptionRoutedEventArgs> WaitForLoadedAsync(
      this BitmapImage bitmapImage,
      int timeoutInMs = 0)
    {
      TaskCompletionSource<ExceptionRoutedEventArgs> tcs = new TaskCompletionSource<ExceptionRoutedEventArgs>();
      if (((BitmapSource) bitmapImage).PixelWidth > 0 || ((BitmapSource) bitmapImage).PixelHeight > 0)
      {
        tcs.SetResult((ExceptionRoutedEventArgs) null);
        return await tcs.Task;
      }
      RoutedEventHandler reh = (RoutedEventHandler) null;
      ExceptionRoutedEventHandler ereh = (ExceptionRoutedEventHandler) null;
      EventHandler<object> progressCheckTimerTickHandler = (EventHandler<object>) null;
      DispatcherTimer progressCheckTimer = new DispatcherTimer();
      Action dismissWatchmen = (Action) (() =>
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(bitmapImage.remove_ImageOpened), reh);
        WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(bitmapImage.remove_ImageFailed), ereh);
        WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(progressCheckTimer.remove_Tick), progressCheckTimerTickHandler);
        progressCheckTimer.Stop();
      });
      int totalWait = 0;
      progressCheckTimerTickHandler = (EventHandler<object>) ((sender, o) =>
      {
        totalWait += 10;
        if (((BitmapSource) bitmapImage).PixelWidth > 0)
        {
          dismissWatchmen();
          tcs.SetResult((ExceptionRoutedEventArgs) null);
        }
        else
        {
          if (timeoutInMs <= 0 || totalWait < timeoutInMs)
            return;
          dismissWatchmen();
          tcs.SetResult((ExceptionRoutedEventArgs) null);
        }
      });
      progressCheckTimer.put_Interval((TimeSpan) TimeSpan.FromMilliseconds(10.0));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(progressCheckTimer.add_Tick), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(progressCheckTimer.remove_Tick), progressCheckTimerTickHandler);
      progressCheckTimer.Start();
      reh = (RoutedEventHandler) ((s, e) =>
      {
        dismissWatchmen();
        tcs.SetResult((ExceptionRoutedEventArgs) null);
      });
      ereh = (ExceptionRoutedEventHandler) ((s, e) =>
      {
        dismissWatchmen();
        tcs.SetResult(e);
      });
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(bitmapImage.add_ImageOpened), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(bitmapImage.remove_ImageOpened), reh);
      WindowsRuntimeMarshal.AddEventHandler<ExceptionRoutedEventHandler>((Func<ExceptionRoutedEventHandler, EventRegistrationToken>) new Func<ExceptionRoutedEventHandler, EventRegistrationToken>(bitmapImage.add_ImageFailed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(bitmapImage.remove_ImageFailed), ereh);
      return await tcs.Task;
    }
  }
}
