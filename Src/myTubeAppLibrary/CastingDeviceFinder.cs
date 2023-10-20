// Decompiled with JetBrains decompiler
// Type: myTube.CastingDeviceFinder
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;

namespace myTube
{
  public class CastingDeviceFinder
  {
    private IVideoHandler[] handlers;
    private object listLock = new object();
    private CoreDispatcher dispatcher;
    private List<string> newIDs = new List<string>();

    public ObservableCollection<VideoCastingDevice> Items { get; } = new ObservableCollection<VideoCastingDevice>();

    public async Task FindDevices(CoreDispatcher dispatcher, params IVideoHandler[] handlers)
    {
      CastingDeviceFinder castingDeviceFinder = this;
      castingDeviceFinder.handlers = handlers;
      castingDeviceFinder.dispatcher = dispatcher;
      for (int index = 0; index < castingDeviceFinder.Items.Count; ++index)
      {
        if (DateTimeOffset.Now - castingDeviceFinder.Items[index].AddedAt > TimeSpan.FromMinutes(5.0))
        {
          castingDeviceFinder.Items.RemoveAt(index);
          --index;
        }
      }
      castingDeviceFinder.newIDs.Clear();
      List<Task> tasks = new List<Task>();
      foreach (IVideoHandler handler in handlers)
      {
        if (handler.SupportsCasting)
        {
          //RnD / TODO
          // ISSUE: method pointer
          //handler.FoundCastingDevice +=
          //              new TypedEventHandler<IVideoHandler,
          //              FoundCastingDeviceEventArgs>((object) castingDeviceFinder,
          //              H_FoundCastingDevice);//__methodptr(H_FoundCastingDevice));

          tasks.Add((Task) handler.FindCastingDevices());
        }
      }
      await Task.WhenAll(tasks.ToArray());
      foreach (IVideoHandler handler in handlers)
      {
        // RnD / TODO
        // ISSUE: method pointer
        //handler.FoundCastingDevice -= 
        //            new TypedEventHandler<IVideoHandler, 
        //            FoundCastingDeviceEventArgs>((object) castingDeviceFinder, 
        //            __methodptr(H_FoundCastingDevice));
        tasks.Add((Task) handler.FindCastingDevices());
      }
      for (int index = 0; index < castingDeviceFinder.Items.Count; ++index)
      {
        if (!castingDeviceFinder.newIDs.Contains(castingDeviceFinder.Items[index].Id))
        {
          castingDeviceFinder.Items.RemoveAt(index);
          --index;
        }
      }
    }

        private async void H_FoundCastingDevice(
            IVideoHandler sender,
            FoundCastingDeviceEventArgs args
        )
        {

            // RnD / TODO
            /* await this.dispatcher.RunAsync((CoreDispatcherPriority)1, 
                 new DispatchedHandler((object)new CastingDeviceFinder.\u003C\u003Ec__DisplayClass9_0()
            {
              \u003C\u003E4__this = this,
                  sender = sender,
                  args = args
            }
            }, __methodptr(\u003CH_FoundCastingDevice\u003Eb__0)));*/
        }
    }
}
