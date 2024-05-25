// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.UIElementExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class UIElementExtensions
  {
    public static async Task WaitForLostFocusAsync(this UIElement control)
    {
      RoutedEventArgs routedEventArgs = await EventAsync.FromRoutedEvent((Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(control.add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(control.remove_LostFocus), eh)), (Action<RoutedEventHandler>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(control.remove_LostFocus), eh)));
    }
  }
}
