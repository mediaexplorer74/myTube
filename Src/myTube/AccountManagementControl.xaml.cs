// myTube.AccountManagementControl

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml.Markup;
using RykenTube;


namespace myTube
{
    public sealed partial class AccountManagementControl : UserControl
    {
        public AccountManagementControl()
        {
            this.InitializeComponent();

            WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(
                new Func<RoutedEventHandler,
                EventRegistrationToken>(((FrameworkElement)this).add_Loaded),
                new Action<EventRegistrationToken>(((FrameworkElement)this).remove_Loaded),
                new RoutedEventHandler(this.AccountManagementControl_Loaded));

            ((Control)this).FontFamily = (((Control)DefaultPage.Current).FontFamily);
        }

        private void AccountManagementControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (List<SignInInfo>.Enumerator enumerator = SharedSettings.Accounts.Accounts.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    SignInInfo current = enumerator.Current;
                    if (SharedSettings.CurrentAccount == null || current.UserID != SharedSettings.CurrentAccount.UserID)
                        ((ICollection<object>)((ItemsControl)this.accountsList).Items).Add((object)current);
                }
            }
            if (((ICollection<object>)((ItemsControl)this.accountsList).Items).Count > 0)
                ((UIElement)this.noOtherAccounts).put_Visibility((Visibility)1);
            if (YouTube.IsSignedIn)
                return;
            ((UIElement)this.signOutButton).put_Visibility((Visibility)1);
        }

        private void HidePopup()
        {
            if (!(((FrameworkElement)this).Parent is Popup))
                return;
            (((FrameworkElement)this).Parent as Popup).put_IsOpen(false);
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
            SignInInfo dataContext = (SignInInfo)(sender as FrameworkElement).DataContext;
            YouTube.RefreshSignIn(dataContext.RefreshToken, dataContext.UserID);
        }

        private async void deleteIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(sender is FrameworkElement) || !((sender as FrameworkElement).DataContext is SignInInfo))
                return;
            SignInInfo acc = (SignInInfo)(sender as FrameworkElement).DataContext;
            MessageDialog messageDialog = new MessageDialog("Are you sure you want to remove the account " + acc.UserName + "?");
            IUICommand yes;
            messageDialog.Commands.Add(yes = (IUICommand)new UICommand("yes"));
            IUICommand iuiCommand;
            messageDialog.Commands.Add(iuiCommand = (IUICommand)new UICommand("no"));
            if (await messageDialog.ShowAsync() == yes)
            {
                SharedSettings.Accounts.RemoveAccount(acc);
                Settings.Save();
                if (((ICollection<object>)((ItemsControl)this.accountsList).Items).Contains((object)acc))
                    ((ICollection<object>)((ItemsControl)this.accountsList).Items).Remove((object)acc);
            }
            acc = (SignInInfo)null;
            yes = (IUICommand)null;
        }

        private void TextBlock_DataContextChanged(
          FrameworkElement sender,
          DataContextChangedEventArgs args)
        {
            if (!(sender is TextBlock textBlock) 
                || !(((FrameworkElement)textBlock).DataContext is SignInInfo dataContext))
                return;

            textBlock.Text = string.IsNullOrWhiteSpace(dataContext.UserName) 
                ? App.Strings["channels.google", "Google Account"]
                : dataContext.UserName;
        }           
       
    }
}
