// Decompiled with JetBrains decompiler
// Type: myTube.AccountManagementControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed class AccountManagementControl : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock noOtherAccounts;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView accountsList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button signOutButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public AccountManagementControl()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.AccountManagementControl_Loaded));
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
    }

    private void AccountManagementControl_Loaded(object sender, RoutedEventArgs e)
    {
      using (List<SignInInfo>.Enumerator enumerator = SharedSettings.Accounts.Accounts.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          SignInInfo current = enumerator.Current;
          if (SharedSettings.CurrentAccount == null || current.UserID != SharedSettings.CurrentAccount.UserID)
            ((ICollection<object>) ((ItemsControl) this.accountsList).Items).Add((object) current);
        }
      }
      if (((ICollection<object>) ((ItemsControl) this.accountsList).Items).Count > 0)
        ((UIElement) this.noOtherAccounts).put_Visibility((Visibility) 1);
      if (YouTube.IsSignedIn)
        return;
      ((UIElement) this.signOutButton).put_Visibility((Visibility) 1);
    }

    private void HidePopup()
    {
      if (!(((FrameworkElement) this).Parent is Popup))
        return;
      (((FrameworkElement) this).Parent as Popup).put_IsOpen(false);
    }

    private void Button_Tapped(object sender, TappedRoutedEventArgs e)
    {
      this.HidePopup();
      DefaultPage.Current.OpenBrowser();
    }

    private void Button_Tapped_1(object sender, TappedRoutedEventArgs e)
    {
      this.HidePopup();
      YouTube.SignOut();
    }

    private void accountBorder_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement) || !((sender as FrameworkElement).DataContext is SignInInfo))
        return;
      this.HidePopup();
      SignInInfo dataContext = (SignInInfo) (sender as FrameworkElement).DataContext;
      YouTube.RefreshSignIn(dataContext.RefreshToken, dataContext.UserID);
    }

    private async void deleteIcon_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement) || !((sender as FrameworkElement).DataContext is SignInInfo))
        return;
      SignInInfo acc = (SignInInfo) (sender as FrameworkElement).DataContext;
      MessageDialog messageDialog = new MessageDialog("Are you sure you want to remove the account " + acc.UserName + "?");
      IUICommand yes;
      messageDialog.Commands.Add(yes = (IUICommand) new UICommand("yes"));
      IUICommand iuiCommand;
      messageDialog.Commands.Add(iuiCommand = (IUICommand) new UICommand("no"));
      if (await messageDialog.ShowAsync() == yes)
      {
        SharedSettings.Accounts.RemoveAccount(acc);
        Settings.Save();
        if (((ICollection<object>) ((ItemsControl) this.accountsList).Items).Contains((object) acc))
          ((ICollection<object>) ((ItemsControl) this.accountsList).Items).Remove((object) acc);
      }
      acc = (SignInInfo) null;
      yes = (IUICommand) null;
    }

    private void TextBlock_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(sender is TextBlock textBlock) || !(((FrameworkElement) textBlock).DataContext is SignInInfo dataContext))
        return;
      textBlock.put_Text(string.IsNullOrWhiteSpace(dataContext.UserName) ? App.Strings["channels.google", "Google Account"] : dataContext.UserName);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///AccountManagementControl.xaml"), (ComponentResourceLocation) 0);
      this.noOtherAccounts = (TextBlock) ((FrameworkElement) this).FindName("noOtherAccounts");
      this.accountsList = (ListView) ((FrameworkElement) this).FindName("accountsList");
      this.signOutButton = (Button) ((FrameworkElement) this).FindName("signOutButton");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.deleteIcon_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.accountBorder_Tapped));
          break;
        case 3:
          FrameworkElement frameworkElement = (FrameworkElement) target;
          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(frameworkElement.add_DataContextChanged), new Action<EventRegistrationToken>(frameworkElement.remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(TextBlock_DataContextChanged)));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.Button_Tapped));
          break;
        case 5:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.Button_Tapped_1));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
