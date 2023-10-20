// Decompiled with JetBrains decompiler
// Type: myTube.Popups.DevicePickerFlyout
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed class DevicePickerFlyout : UserControl, IComponentConnector
  {
    private DispatcherTimer timer;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate itemTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView listView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar progress;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl dontCastControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler<DeviceInformation> ItemSelected;

    public object ItemsSource
    {
      set => ((ItemsControl) this.listView).put_ItemsSource(value);
    }

    public DevicePickerFlyout()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.DevicePickerFlyout_Unloaded));
      DispatcherTimer dispatcherTimer = new DispatcherTimer();
      dispatcherTimer.put_Interval(TimeSpan.FromSeconds(12.0));
      this.timer = dispatcherTimer;
      DispatcherTimer timer = this.timer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(timer.add_Tick), new Action<EventRegistrationToken>(timer.remove_Tick), new EventHandler<object>(this.Timer_Tick));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.DevicePickerFlyout_Loaded));
    }

    private void DevicePickerFlyout_Loaded(object sender, RoutedEventArgs e) => this.timer.Start();

    private void Timer_Tick(object sender, object e)
    {
      this.timer.Stop();
      ((UIElement) this.progress).put_Visibility((Visibility) 1);
    }

    private void DevicePickerFlyout_Unloaded(object sender, RoutedEventArgs e)
    {
    }

    private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.ItemSelected == null || !(((Selector) this.listView).SelectedItem is DeviceViewModel selectedItem))
        return;
      this.ItemSelected((object) this, selectedItem.Device);
    }

    private void dontCastControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.ItemSelected == null)
        return;
      this.ItemSelected((object) this, (DeviceInformation) null);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Popups/DevicePickerFlyout.xaml"), (ComponentResourceLocation) 0);
      this.itemTemplate = (DataTemplate) ((FrameworkElement) this).FindName("itemTemplate");
      this.listView = (ListView) ((FrameworkElement) this).FindName("listView");
      this.progress = (ProgressBar) ((FrameworkElement) this).FindName("progress");
      this.dontCastControl = (ContentControl) ((FrameworkElement) this).FindName("dontCastControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.listView_SelectionChanged));
          break;
        case 2:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.dontCastControl_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
