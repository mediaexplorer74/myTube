// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.Forms.FocusTracker
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions.Forms
{
  public class FocusTracker
  {
    private const int DefaultFocusPollingInterval = 100;
    private UIElement focusedElement;
    private DispatcherTimer timer;
    private int focusPollingInterval;

    public int FocusPollingInterval
    {
      get => this.focusPollingInterval;
      set
      {
        if (this.focusPollingInterval == value)
          return;
        this.focusPollingInterval = value;
        if (this.timer == null)
          return;
        this.timer.put_Interval((TimeSpan) TimeSpan.FromMilliseconds((double) this.focusPollingInterval));
      }
    }

    public event EventHandler<UIElement> FocusChanged;

    private void RaiseFocusChanged(UIElement e)
    {
      EventHandler<UIElement> focusChanged = this.FocusChanged;
      if (focusChanged == null)
        return;
      focusChanged((object) this, e);
    }

    public FocusTracker()
    {
      this.FocusPollingInterval = 100;
      this.StartFocusTracking();
    }

    private void StartFocusTracking()
    {
      this.UpdateFocusedElement();
      this.timer = new DispatcherTimer();
      this.timer.put_Interval((TimeSpan) TimeSpan.FromMilliseconds((double) this.FocusPollingInterval));
      DispatcherTimer timer = this.timer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(timer.add_Tick), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(timer.remove_Tick), new EventHandler<object>(this.OnTick));
      this.timer.Start();
    }

    private void StopFocusTracking()
    {
      if (this.timer == null)
        return;
      this.timer.Stop();
    }

    private void OnLostFocus(object sender, RoutedEventArgs args) => this.UpdateFocusedElement();

    private void OnTick(object sender, object o) => this.UpdateFocusedElement();

    private void UpdateFocusedElement()
    {
      UIElement focusedElement1 = FocusManager.GetFocusedElement() as UIElement;
      if (this.focusedElement == focusedElement1)
        return;
      if (this.focusedElement != null)
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.focusedElement.remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
      this.focusedElement = focusedElement1;
      this.RaiseFocusChanged(this.focusedElement);
      if (this.focusedElement == null)
        return;
      UIElement focusedElement2 = this.focusedElement;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(focusedElement2.add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(focusedElement2.remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
    }
  }
}
