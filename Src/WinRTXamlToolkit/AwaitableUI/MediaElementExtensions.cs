// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.MediaElementExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class MediaElementExtensions
  {
    public static async Task<MediaElement> WaitForStateAsync(
      this MediaElement mediaElement,
      MediaElementState? newState = null)
    {
      if (newState.HasValue && mediaElement.CurrentState == newState.Value)
        return (MediaElement) null;
      TaskCompletionSource<MediaElement> tcs = new TaskCompletionSource<MediaElement>();
      RoutedEventHandler reh = (RoutedEventHandler) null;
      reh = (RoutedEventHandler) ((s, e) =>
      {
        if (newState.HasValue && mediaElement.CurrentState != newState.Value)
          return;
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(mediaElement.remove_CurrentStateChanged), reh);
        tcs.SetResult((MediaElement) s);
      });
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(mediaElement.add_CurrentStateChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(mediaElement.remove_CurrentStateChanged), reh);
      return await tcs.Task;
    }

    public static async Task<MediaElement> PlayToEndAsync(
      this MediaElement mediaElement,
      Uri source)
    {
      mediaElement.put_Source((Uri) source);
      return await mediaElement.WaitToCompleteAsync();
    }

    public static async Task<MediaElement> WaitToCompleteAsync(this MediaElement mediaElement)
    {
      TaskCompletionSource<MediaElement> tcs = new TaskCompletionSource<MediaElement>();
      RoutedEventHandler reh = (RoutedEventHandler) null;
      reh = (RoutedEventHandler) ((s, e) =>
      {
        if (mediaElement.CurrentState == 2 || mediaElement.CurrentState == 1 || mediaElement.CurrentState == 3)
          return;
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(mediaElement.remove_CurrentStateChanged), reh);
        tcs.SetResult((MediaElement) s);
      });
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(mediaElement.add_CurrentStateChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(mediaElement.remove_CurrentStateChanged), reh);
      return await tcs.Task;
    }
  }
}
