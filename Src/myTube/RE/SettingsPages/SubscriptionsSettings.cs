// Decompiled with JetBrains decompiler
// Type: myTube.SettingsPages.SubscriptionsSettings
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.SettingsPages
{
  public sealed class SubscriptionsSettings : UserControl, IComponentConnector
  {
    private Popup p;
    private Point selectPos;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel selectPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl done;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl all;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl none;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel stackPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox allChannels;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox filterChannels;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView subscriptionItems;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public SubscriptionsSettings()
    {
      this.InitializeComponent();
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.SubscriptionsSettings_Loaded));
      ((FrameworkElement) this).put_RequestedTheme(Settings.Theme);
    }

    private async void SubscriptionsSettings_Loaded(object sender, RoutedEventArgs e)
    {
      this.p = Helper.FindParent<Popup>((FrameworkElement) this, 1000);
      if (this.p != null)
      {
        Popup p = this.p;
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(p.add_Closed), new Action<EventRegistrationToken>(p.remove_Closed), new EventHandler<object>(this.p_Closed));
      }
      ((ToggleButton) this.allChannels).put_IsChecked(new bool?(Settings.Subscriptions.AllChannels));
      ((ToggleButton) this.filterChannels).put_IsChecked(new bool?(Settings.Subscriptions.FilterChannels));
      this.setVisibilities();
      int num = await YouTube.SubscriptionsLoadedTask ? 1 : 0;
      ((ItemsControl) this.subscriptionItems).put_ItemsSource((object) YouTube.SubscriptionData);
      List<string> filteredItems = await Settings.Subscriptions.GetFilteredItems(YouTube.UserInfo.ID);
      foreach (UserInfo userInfo in (Collection<UserInfo>) YouTube.SubscriptionData)
      {
        if (!filteredItems.Contains(userInfo.ID))
          ((ListViewBase) this.subscriptionItems).SelectedItems.Add((object) userInfo);
      }
    }

    private async void p_Closed(object sender, object e)
    {
      WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>(new Action<EventRegistrationToken>(this.p.remove_Closed), new EventHandler<object>(this.p_Closed));
      if (await YouTube.SubscriptionsLoadedTask && YouTube.IsSignedIn && Settings.Subscriptions.FilterChannels && ((ListViewBase) this.subscriptionItems).SelectedItems.Count > 0)
      {
        string id = YouTube.UserInfo.ID;
        List<string> subscriptionIds = new List<string>();
        foreach (UserInfo userInfo in (Collection<UserInfo>) YouTube.SubscriptionData)
        {
          if (!((ListViewBase) this.subscriptionItems).SelectedItems.Contains((object) userInfo))
            subscriptionIds.Add(userInfo.ID);
        }
        await Settings.Subscriptions.SetFilter(id, subscriptionIds);
      }
      await Settings.Subscriptions.Save();
    }

    private void setVisibilities()
    {
      ((Control) this.filterChannels).put_IsEnabled(((ToggleButton) this.allChannels).IsChecked.Value);
      ListView subscriptionItems = this.subscriptionItems;
      StackPanel selectPanel = this.selectPanel;
      Visibility visibility1;
      ((UIElement) this.filterChannels).put_Visibility((Visibility) (int) (visibility1 = (Visibility) 1));
      Visibility visibility2;
      Visibility visibility3 = visibility2 = visibility1;
      ((UIElement) selectPanel).put_Visibility(visibility2);
      Visibility visibility4 = visibility3;
      ((UIElement) subscriptionItems).put_Visibility(visibility4);
    }

    private void allChannels_Checked(object sender, RoutedEventArgs e)
    {
      Settings.Subscriptions.AllChannels = true;
      this.setVisibilities();
    }

    private void allChannels_Unchecked(object sender, RoutedEventArgs e)
    {
      Settings.Subscriptions.AllChannels = false;
      this.setVisibilities();
    }

    private void filterChannels_Checked(object sender, RoutedEventArgs e)
    {
      Settings.Subscriptions.FilterChannels = true;
      this.setVisibilities();
    }

    private void filterChannels_Unchecked(object sender, RoutedEventArgs e)
    {
      Settings.Subscriptions.FilterChannels = false;
      this.setVisibilities();
    }

    private void scroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
    }

    private async void done_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.p == null)
        return;
      DefaultPage.Current.ClosePopup();
    }

    private void all_Tapped(object sender, TappedRoutedEventArgs e)
    {
      ((ListViewBase) this.subscriptionItems).SelectedItems.Clear();
      foreach (object obj in (Collection<UserInfo>) YouTube.SubscriptionData)
        ((ListViewBase) this.subscriptionItems).SelectedItems.Add(obj);
    }

    private void none_Tapped(object sender, TappedRoutedEventArgs e) => ((ListViewBase) this.subscriptionItems).SelectedItems.Clear();

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///SettingsPages/SubscriptionsSettings.xaml"), (ComponentResourceLocation) 0);
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.selectPanel = (StackPanel) ((FrameworkElement) this).FindName("selectPanel");
      this.done = (ContentControl) ((FrameworkElement) this).FindName("done");
      this.all = (ContentControl) ((FrameworkElement) this).FindName("all");
      this.none = (ContentControl) ((FrameworkElement) this).FindName("none");
      this.stackPanel = (StackPanel) ((FrameworkElement) this).FindName("stackPanel");
      this.allChannels = (CheckBox) ((FrameworkElement) this).FindName("allChannels");
      this.filterChannels = (CheckBox) ((FrameworkElement) this).FindName("filterChannels");
      this.subscriptionItems = (ListView) ((FrameworkElement) this).FindName("subscriptionItems");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ScrollViewer scrollViewer = (ScrollViewer) target;
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>(new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer.add_ViewChanged), new Action<EventRegistrationToken>(scrollViewer.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.scroll_ViewChanged));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.done_Tapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.all_Tapped));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.none_Tapped));
          break;
        case 5:
          ToggleButton toggleButton1 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton1.add_Checked), new Action<EventRegistrationToken>(toggleButton1.remove_Checked), new RoutedEventHandler(this.allChannels_Checked));
          ToggleButton toggleButton2 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton2.add_Unchecked), new Action<EventRegistrationToken>(toggleButton2.remove_Unchecked), new RoutedEventHandler(this.allChannels_Unchecked));
          break;
        case 6:
          ToggleButton toggleButton3 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton3.add_Checked), new Action<EventRegistrationToken>(toggleButton3.remove_Checked), new RoutedEventHandler(this.filterChannels_Checked));
          ToggleButton toggleButton4 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton4.add_Unchecked), new Action<EventRegistrationToken>(toggleButton4.remove_Unchecked), new RoutedEventHandler(this.filterChannels_Unchecked));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
