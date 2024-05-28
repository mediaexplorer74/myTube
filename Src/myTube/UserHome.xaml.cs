// myTube.UserHome

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
using myTube.Helpers;
using RykenTube;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;

namespace myTube
{
    public sealed partial class UserHome : UserControl
    {
        private BitmapImage bitmap;

        //TEMP
        //private ImageBrush thumbBrush;
        //private TextBlock userText;
        //private FrameworkElement signInPanel;
        //private Selector listView;

        public UserHome()
        {
            this.InitializeComponent();
            this.bitmap = new BitmapImage();
            this.thumbBrush.ImageSource = ((ImageSource)this.bitmap);
            BitmapImage bitmap = this.bitmap;



            bitmap.ImageOpened += this.Bitmap_ImageOpened;
            ((FrameworkElement)this).Loaded += this.UserHome_Loaded;
            ((FrameworkElement)this).Unloaded += this.UserHome_Unloaded;
        }

        private void Bitmap_ImageOpened(object sender, RoutedEventArgs e)
        {
        }

        private void UserHome_Unloaded(object sender, RoutedEventArgs e)
        {
            YouTube.SigningIn -= new EventHandler(this.YouTube_SigningIn);
            YouTube.SignedIn -= new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
            YouTube.SignedOut -= new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
        }

        private async void YouTube_SigningIn(object sender, EventArgs e)
        {
            
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await Task.Yield(); // RnD
                this.YouTube_SigningIn(this, EventArgs.Empty);
            });
        }

        private void UserHome_Loaded(object sender, RoutedEventArgs e)
        {
            if (YouTube.CurrentlySigningIn)
                this.userText.Text = (App.Strings["home.accounts.signingin", "signing in"].ToLower() + "...");

            else if (YouTube.IsSignedIn)
            {
                if (YouTube.UserInfo != null)
                {
                    this.userText.Text = (string.IsNullOrWhiteSpace(YouTube.UserInfo.UserDisplayName)
                                  ? App.Strings["channels.google", "Google Account"]
                                  : YouTube.UserInfo.UserDisplayName);
                    this.bitmap.UriSource = (YouTube.UserInfo.ThumbUri);
                }
                else
                {
                    this.userText.Text = (App.Strings["common.signedin", "signed in"].ToLower());
                    this.bitmap.UriSource = (new Uri("ms-appx:/Images/UserNoThumb.png"));
                }
            }
            else
            {
                this.userText.Text = (App.Strings["common.signin", "sign in"].ToLower());
                this.bitmap.UriSource = (new Uri("ms-appx:/Images/UserNoThumb.png"));
            }
            YouTube.SigningIn += new EventHandler(this.YouTube_SigningIn);
            YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
            YouTube.SignedOut += new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
        }

        private async void YouTube_SignedOut(object sender, EventArgs e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.YouTube_SignedOut(this, EventArgs.Empty);
            });
        }

        private async void YouTube_SignedIn(object sender, EventArgs e)
        {
            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.YouTube_SignedIn(this, EventArgs.Empty);
            });
        }

        private void signInPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!YouTube.IsSignedIn && SharedSettings.Accounts.Count == 0)
            {
                (Window.Current.Content as DefaultPage).OpenBrowser();
            }
            else
            {
                if (!YouTube.IsSignedIn && SharedSettings.Accounts.Count < 1)
                    return;
                AccountManagementControl control = new AccountManagementControl();
                Popup popup1 = new Popup();

                popup1.Child = control;
                ((FrameworkElement)popup1).RequestedTheme = (App.Theme);

                Popup popup2 = popup1;
                DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
                {
                    Rect bounds1 = ((FrameworkElement)this.signInPanel).GetBounds(Window.Current.Content);
                    Rect visibleBounds = App.VisibleBounds;
                    Rect bounds2 = Window.Current.Bounds;
                    control.UpdateLayout();
                    control.Width = (Math.Min(bounds1.Width, bounds2.Width - 38.0));
                    control.MaxHeight = (Math.Min(400.0, bounds2.Height));
                    return new Point()
                    {
                        X = Math.Max(bounds1.Left, 19.0),
                        Y = Math.Min(bounds1.Bottom, bounds2.Height - control.ActualHeight)
                    };
                }));
                DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, -80.0),
                      FadeType.Half, hideAppBar: false);
            }
        }

        private void addAccount_Click(object sender, RoutedEventArgs e)
                => (Window.Current.Content as DefaultPage).OpenBrowser();

        private void signIn_Click(object sender, RoutedEventArgs e)
        {
            if (YouTube.IsSignedIn)
            {
                YouTube.SignOut();
            }
            else
            {
                (Window.Current.Content as DefaultPage).OpenBrowser();
            }
        }

        public async void SignInChanged(bool signIn)
        {
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Selector)this.listView).SelectedItem is IconButtonInfo selectedItem)
            {
                if (selectedItem.Name == "channel")
                {
                    if (YouTube.IsSignedIn)
                    {
                        (Application.Current as App).RootFrame.Navigate(typeof(ChannelPage), (object)0);
                    }
                    else
                    {
                        (Window.Current.Content as DefaultPage).OpenBrowser();
                    }
                }
                else if (selectedItem.Name == "uploads")
                {
                    if (YouTube.IsSignedIn)
                    {
                        (Application.Current as App).RootFrame.Navigate(typeof(ChannelPage), (object)0);
                    }
                    else
                    {
                        (Window.Current.Content as DefaultPage).OpenBrowser();
                    }
                }
                else if (selectedItem.Name == "favorites")
                {
                    if (YouTube.IsSignedIn)
                    {
                        if (YouTube.UserInfo != null)
                        {
                            DefaultPage.Current.Frame.Navigate(typeof(PlaylistPage),
                                (object)YouTube.UserInfo.FavoritesPlaylist);
                        }
                        else
                        {
                            DefaultPage.Current.Frame.Navigate(typeof(PlaylistPage),
                                (object)("FL" + UserInfo.RemoveUCFromID(SharedSettings.CurrentAccount.UserID)));
                        }
                    }
                    else
                        (Window.Current.Content as DefaultPage).OpenBrowser();
                }
                else if (selectedItem.Name == "watch later")
                {
                    if (YouTube.IsSignedIn)
                    {
                        VideoListClient client = WatchLaterPage.Client;
                        (Application.Current as App).RootFrame.Navigate(typeof(WatchLaterPage),
                            (object)new ClientAndFirstLoadTask()
                            {
                                Client = client,
                                LoadTask = client.GetFeed(0)
                            });
                    }
                    else
                        (Window.Current.Content as DefaultPage).OpenBrowser();
                }
                else if (selectedItem.Name == "playlists")
                {
                    if (SharedSettings.CurrentAccount != null || App.GlobalObjects.OfflinePlaylistsCollection.Playlists.Length != 0)
                        (Application.Current as App).RootFrame.Navigate(typeof(PlaylistListPage));
                    else
                        (Window.Current.Content as DefaultPage).OpenBrowser();
                }
                else if (selectedItem.Name == "saved")
                    (Application.Current as App).RootFrame.Navigate(typeof(SavedVideosPage));
                else if (selectedItem.Name == "subscribed to")
                {
                    if (YouTube.IsSignedIn)
                        (Application.Current as App).RootFrame.Navigate(typeof(SubscribedToPage));
                    else
                        (Window.Current.Content as DefaultPage).OpenBrowser();
                }
                else if (selectedItem.Name == "history")
                    App.Instance.RootFrame.Navigate(typeof(HistoryPage));
                else if (selectedItem.Name == "liked")
                    App.Instance.RootFrame.Navigate(typeof(LikesPage));
                else if (selectedItem.Name == "upload")
                {
                    if (YouTube.IsSignedIn)
                        App.Instance.RootFrame.Navigate(typeof(UploaderPage));
                    else
                        this.signIn();
                }
            }
          ((Selector)this.listView).SelectedItem = (object)null;
        }

        private void signIn()
        {
            if (SharedSettings.Accounts.Count == 0)
                (Window.Current.Content as DefaultPage).OpenBrowser();
            else
                this.signInPanel_Tapped((object)null, (TappedRoutedEventArgs)null);
        }

        private void accountNamePanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (YouTube.IsSignedIn)
            {
                //(Application.Current as App).RootFrame.Navigate(typeof(ChannelPage), (object)0);
            }
            else if (SharedSettings.Accounts.Count == 0)
            {
                (Window.Current.Content as DefaultPage).OpenBrowser();
            }
            else
                this.signInPanel_Tapped(sender, e);
        }

        private void uploadControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (YouTube.IsSignedIn)
                App.Instance.RootFrame.Navigate(typeof(UploaderPage));
            else
                this.signIn();
        }

    }
}

