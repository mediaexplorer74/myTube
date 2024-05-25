// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.AwaitableUI.EventAsync
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.AwaitableUI
{
  public static class EventAsync
  {
    public static Task<object> FromEvent<T>(
      Action<EventHandler<T>> addEventHandler,
      Action<EventHandler<T>> removeEventHandler,
      Action beginAction = null)
    {
      return new EventAsync.EventHandlerTaskSource<T>(addEventHandler, removeEventHandler, beginAction).Task;
    }

    public static Task<RoutedEventArgs> FromRoutedEvent(
      Action<RoutedEventHandler> addEventHandler,
      Action<RoutedEventHandler> removeEventHandler,
      Action beginAction = null)
    {
      return new EventAsync.RoutedEventHandlerTaskSource(addEventHandler, removeEventHandler, beginAction).Task;
    }

    private sealed class EventHandlerTaskSource<TEventArgs>
    {
      private readonly TaskCompletionSource<object> tcs;
      private readonly Action<EventHandler<TEventArgs>> removeEventHandler;

      public EventHandlerTaskSource(
        Action<EventHandler<TEventArgs>> addEventHandler,
        Action<EventHandler<TEventArgs>> removeEventHandler,
        Action beginAction = null)
      {
        if (addEventHandler == null)
          throw new ArgumentNullException(nameof (addEventHandler));
        if (removeEventHandler == null)
          throw new ArgumentNullException(nameof (removeEventHandler));
        this.tcs = new TaskCompletionSource<object>();
        this.removeEventHandler = removeEventHandler;
        addEventHandler(new EventHandler<TEventArgs>(this.EventCompleted));
        if (beginAction == null)
          return;
        beginAction();
      }

      public Task<object> Task => this.tcs.Task;

      private void EventCompleted(object sender, TEventArgs args)
      {
        this.removeEventHandler(new EventHandler<TEventArgs>(this.EventCompleted));
        this.tcs.SetResult((object) args);
      }
    }

    private sealed class RoutedEventHandlerTaskSource
    {
      private readonly TaskCompletionSource<RoutedEventArgs> tcs;
      private readonly Action<RoutedEventHandler> removeEventHandler;

      public RoutedEventHandlerTaskSource(
        Action<RoutedEventHandler> addEventHandler,
        Action<RoutedEventHandler> removeEventHandler,
        Action beginAction = null)
      {
        if (addEventHandler == null)
          throw new ArgumentNullException(nameof (addEventHandler));
        if (removeEventHandler == null)
          throw new ArgumentNullException(nameof (removeEventHandler));
        this.tcs = new TaskCompletionSource<RoutedEventArgs>();
        this.removeEventHandler = removeEventHandler;
        addEventHandler(new RoutedEventHandler(this.EventCompleted));
        if (beginAction == null)
          return;
        beginAction();
      }

      public Task<RoutedEventArgs> Task => this.tcs.Task;

      private void EventCompleted(object sender, RoutedEventArgs args)
      {
        this.removeEventHandler(new RoutedEventHandler(this.EventCompleted));
        this.tcs.SetResult(args);
      }
    }
  }
}
