// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.StoryboardExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class StoryboardExtensions
  {
    public static async Task BeginAsync(this Storyboard storyboard)
    {
      object obj = await EventAsync.FromEvent<object>((Action<EventHandler<object>>) (eh => WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), eh)), (Action<EventHandler<object>>) (eh => WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), eh)), new Action(storyboard.Begin));
    }
  }
}
