// Decompiled with JetBrains decompiler
// Type: myTube.SettingsPages.WatchLaterSettings
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.SettingsPages
{
  public sealed class WatchLaterSettings : UserControl, IComponentConnector
  {
    private Popup p;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox where;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public WatchLaterSettings()
    {
      this.InitializeComponent();
      if (Settings.WatchLater.AddVideosTo == PlaylistPosition.End)
        ((Selector) this.where).put_SelectedIndex(1);
      else
        ((Selector) this.where).put_SelectedIndex(0);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.WatchLaterSettings_Loaded));
      ((FrameworkElement) this).put_RequestedTheme(Settings.Theme);
    }

    private void WatchLaterSettings_Loaded(object sender, RoutedEventArgs e)
    {
      this.p = Helper.FindParent<Popup>((FrameworkElement) this, 1000);
      Popup p = this.p;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(p.add_Closed), new Action<EventRegistrationToken>(p.remove_Closed), new EventHandler<object>(this.p_Closed));
    }

    private void p_Closed(object sender, object e)
    {
      WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>(new Action<EventRegistrationToken>(this.p.remove_Closed), new EventHandler<object>(this.p_Closed));
      if (((Selector) this.where).SelectedIndex == 0)
      {
        Settings.WatchLater.AddVideosTo = PlaylistPosition.Beginning;
      }
      else
      {
        if (((Selector) this.where).SelectedIndex != 1)
          return;
        Settings.WatchLater.AddVideosTo = PlaylistPosition.End;
      }
    }

    private async void ContentControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.p == null)
        return;
      DefaultPage.Current.ClosePopup();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///SettingsPages/WatchLaterSettings.xaml"), (ComponentResourceLocation) 0);
      this.where = (ComboBox) ((FrameworkElement) this).FindName("where");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.ContentControl_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
