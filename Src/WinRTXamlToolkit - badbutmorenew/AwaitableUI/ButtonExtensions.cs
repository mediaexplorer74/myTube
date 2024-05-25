// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.ButtonExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class ButtonExtensions
  {
    public static async Task<RoutedEventArgs> WaitForClickAsync(this ButtonBase button) => await EventAsync.FromRoutedEvent((Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(button.add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(button.remove_Click), eh)), (Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(button.remove_Click), eh)));

    public static async Task<ButtonBase> WaitForClickAsync(this IEnumerable<ButtonBase> buttons)
    {
      TaskCompletionSource<ButtonBase> tcs = new TaskCompletionSource<ButtonBase>();
      RoutedEventHandler reh = (RoutedEventHandler) null;
      reh = (RoutedEventHandler) ((s, e) =>
      {
        foreach (ButtonBase button in buttons)
          WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(button.remove_Click), reh);
        tcs.SetResult((ButtonBase) s);
      });
      foreach (ButtonBase button in buttons)
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(button.add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(button.remove_Click), reh);
      return await tcs.Task;
    }

    public static async Task<ButtonBase> WaitForClickAsync(params ButtonBase[] buttons) => await ((IEnumerable<ButtonBase>) buttons).WaitForClickAsync();
  }
}
