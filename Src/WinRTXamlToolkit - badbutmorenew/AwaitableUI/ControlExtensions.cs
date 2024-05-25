// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.ControlExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class ControlExtensions
  {
    public static async Task GoToVisualStateAsync(
      this Control control,
      FrameworkElement visualStatesHost,
      string stateGroupName,
      string stateName)
    {
      TaskCompletionSource<Storyboard> tcs = new TaskCompletionSource<Storyboard>();
      Storyboard storyboard = ControlExtensions.GetStoryboardForVisualState(visualStatesHost, stateGroupName, stateName);
      if (storyboard != null)
      {
        EventHandler<object> eh = (EventHandler<object>) null;
        eh = (EventHandler<object>) ((s, e) =>
        {
          WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), eh);
          tcs.SetResult(storyboard);
        });
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), eh);
      }
      VisualStateManager.GoToState(control, stateName, true);
      if (storyboard == null)
        return;
      Storyboard task = await tcs.Task;
    }

    private static Storyboard GetStoryboardForVisualState(
      FrameworkElement visualStatesHost,
      string stateGroupName,
      string stateName)
    {
      Storyboard storyboardForVisualState = (Storyboard) null;
      IList<VisualStateGroup> visualStateGroups = (IList<VisualStateGroup>) VisualStateManager.GetVisualStateGroups(visualStatesHost);
      VisualStateGroup visualStateGroup1 = (VisualStateGroup) null;
      if (!string.IsNullOrEmpty(stateGroupName))
        visualStateGroup1 = ((IEnumerable<VisualStateGroup>) visualStateGroups).FirstOrDefault<VisualStateGroup>((Func<VisualStateGroup, bool>) (g => g.Name == stateGroupName));
      VisualState visualState = (VisualState) null;
      if (visualStateGroup1 != null)
        visualState = visualStateGroup1.States.FirstOrDefault<VisualState>((Func<VisualState, bool>) (s => s.Name == stateName));
      if (visualState == null)
      {
        foreach (VisualStateGroup visualStateGroup2 in (IEnumerable<VisualStateGroup>) visualStateGroups)
        {
          visualState = visualStateGroup2.States.FirstOrDefault<VisualState>((Func<VisualState, bool>) (s => s.Name == stateName));
          if (visualState != null)
            break;
        }
      }
      if (visualState != null)
        storyboardForVisualState = visualState.Storyboard;
      return storyboardForVisualState;
    }
  }
}
