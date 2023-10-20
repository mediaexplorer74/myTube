// Decompiled with JetBrains decompiler
// Type: myTube.Popups.ChannelNotificationsPopup
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube.Popups
{
  public sealed class ChannelNotificationsPopup : UserControl, IComponentConnector
  {
    public const int MaxChannels = 15;
    private ObservableCollection<ChannelNotificationViewModel> viewModels;
    private ChannelNotificationViewModel mainViewModel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style countStyle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style countStyleMax;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock countText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl items;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl currentUser;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run currentNumber;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run maxNumber;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private UserInfo Channel => ((FrameworkElement) this).DataContext as UserInfo;

    public ChannelNotificationsPopup()
    {
      this.InitializeComponent();
      this.viewModels = new ObservableCollection<ChannelNotificationViewModel>();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(ChannelNotificationsPopup_DataContextChanged)));
      ((FrameworkElement) this).put_RequestedTheme(Settings.Theme);
      this.maxNumber.put_Text(15.ToString());
    }

    private async void populateViewModel()
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      this.setCurrentNumber();
      foreach (UserInfo userInfo in (Collection<UserInfo>) YouTube.SubscriptionData)
      {
        bool flag = false;
        foreach (ChannelNotificationViewModel viewModel in (Collection<ChannelNotificationViewModel>) this.viewModels)
        {
          if (UserInfo.RemoveUCFromID(viewModel.Id) == UserInfo.RemoveUCFromID(userInfo.ID))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          ((Collection<ChannelNotificationViewModel>) this.viewModels).Add(new ChannelNotificationViewModel(userInfo.ID, userInfo.Title));
      }
      foreach (string id in App.GlobalObjects.ChannelNotifications.GetIds())
      {
        bool flag = false;
        foreach (ChannelNotificationViewModel viewModel in (Collection<ChannelNotificationViewModel>) this.viewModels)
        {
          if (UserInfo.RemoveUCFromID(viewModel.Id) == UserInfo.RemoveUCFromID(id))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          string name = App.GlobalObjects.ChannelNotifications.GetName(id);
          int count = ((Collection<ChannelNotificationViewModel>) this.viewModels).Count;
          ((Collection<ChannelNotificationViewModel>) this.viewModels).Add(new ChannelNotificationViewModel(id, name));
        }
      }
      for (int index = 1; index < ((Collection<ChannelNotificationViewModel>) this.viewModels).Count; ++index)
      {
        if (index >= 1)
        {
          ChannelNotificationViewModel viewModel1 = ((Collection<ChannelNotificationViewModel>) this.viewModels)[index];
          ChannelNotificationViewModel viewModel2 = ((Collection<ChannelNotificationViewModel>) this.viewModels)[index - 1];
          if (viewModel1.Name.CompareTo(viewModel2.Name) < 0 && viewModel1.Notify == viewModel2.Notify || viewModel1.Notify && !viewModel2.Notify)
          {
            ((Collection<ChannelNotificationViewModel>) this.viewModels)[index - 1] = viewModel1;
            ((Collection<ChannelNotificationViewModel>) this.viewModels)[index] = viewModel2;
            index -= 2;
          }
        }
      }
      this.items.put_ItemsSource((object) this.viewModels);
    }

    private void setCurrentNumber()
    {
      int count = App.GlobalObjects.ChannelNotifications.Count;
      this.currentNumber.put_Text(count.ToString());
      if (count >= 15)
        ((FrameworkElement) this.countText).put_Style(this.countStyleMax);
      else
        ((FrameworkElement) this.countText).put_Style(this.countStyle);
    }

    private void ChannelNotificationsPopup_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      this.populateViewModel();
      if (this.Channel != null)
      {
        ((FrameworkElement) this.currentUser).put_DataContext((object) (this.mainViewModel = new ChannelNotificationViewModel(this.Channel.ID, this.Channel.UserName)));
        for (int index = 0; index < ((Collection<ChannelNotificationViewModel>) this.viewModels).Count; ++index)
        {
          if (UserInfo.RemoveUCFromID(((Collection<ChannelNotificationViewModel>) this.viewModels)[index].Id) == UserInfo.RemoveUCFromID(this.Channel.ID))
          {
            ((Collection<ChannelNotificationViewModel>) this.viewModels).RemoveAt(index);
            break;
          }
        }
      }
      else
        this.mainViewModel = (ChannelNotificationViewModel) null;
    }

    private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement parent) || !(parent.DataContext is ChannelNotificationViewModel dataContext))
        return;
      int currentIteration = 0;
      if (Helper.FindChild<ToggleSwitch>((DependencyObject) parent, 100, ref currentIteration) == null)
        return;
      if (!dataContext.Notify && App.GlobalObjects.ChannelNotifications.Count >= 15)
      {
        MessageDialog messageDialog = new MessageDialog("You can only receive notifications from up to " + (object) 15 + " channels. Disable notifications from another channel first.", "Too many");
        messageDialog.Commands.Add((IUICommand) new UICommand("OK"));
        messageDialog.ShowAsync();
      }
      else
      {
        dataContext.Notify = !dataContext.Notify;
        this.setCurrentNumber();
      }
    }

    private void ToggleSwitch_Tapped(object sender, TappedRoutedEventArgs e) => e.put_Handled(true);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///Popups/ChannelNotificationsPopup.xaml"), (ComponentResourceLocation) 0);
      this.countStyle = (Style) ((FrameworkElement) this).FindName("countStyle");
      this.countStyleMax = (Style) ((FrameworkElement) this).FindName("countStyleMax");
      this.countText = (TextBlock) ((FrameworkElement) this).FindName("countText");
      this.items = (ItemsControl) ((FrameworkElement) this).FindName("items");
      this.currentUser = (ContentControl) ((FrameworkElement) this).FindName("currentUser");
      this.currentNumber = (Run) ((FrameworkElement) this).FindName("currentNumber");
      this.maxNumber = (Run) ((FrameworkElement) this).FindName("maxNumber");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.Grid_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.ToggleSwitch_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
