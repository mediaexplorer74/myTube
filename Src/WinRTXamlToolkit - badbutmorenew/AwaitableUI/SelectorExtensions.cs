// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.SelectorExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class SelectorExtensions
  {
    public static async Task<SelectionChangedEventArgs> WaitForSelectionChangedAsync(
      this Selector selector)
    {
      TaskCompletionSource<SelectionChangedEventArgs> tcs = new TaskCompletionSource<SelectionChangedEventArgs>();
      SelectionChangedEventHandler sceh = (SelectionChangedEventHandler) null;
      sceh = (SelectionChangedEventHandler) ((s, e) =>
      {
        WindowsRuntimeMarshal.RemoveEventHandler<SelectionChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(selector.remove_SelectionChanged), sceh);
        tcs.SetResult(e);
      });
      WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>((Func<SelectionChangedEventHandler, EventRegistrationToken>) new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(selector.remove_SelectionChanged), sceh);
      return await tcs.Task;
    }
  }
}
