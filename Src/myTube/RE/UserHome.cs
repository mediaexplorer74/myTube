// Decompiled with JetBrains decompiler
// Type: myTube.UserHome
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class UserHome : UserControl, IComponentConnector
  {
    private BitmapImage bitmap;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ImageBrush thumbBrush;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid signInPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView listView;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl uploadControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl accountNamePanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Ellipse thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock userText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public UserHome()
    {
      this.InitializeComponent();
      this.bitmap = new BitmapImage();
      this.thumbBrush.put_ImageSource((ImageSource) this.bitmap);
      BitmapImage bitmap = this.bitmap;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(bitmap.add_ImageOpened), new Action<EventRegistrationToken>(bitmap.remove_ImageOpened), new RoutedEventHandler(this.Bitmap_ImageOpened));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.UserHome_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.UserHome_Unloaded));
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

    private void YouTube_SigningIn(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SigningIn\u003Eb__4_0)));

    private void UserHome_Loaded(object sender, RoutedEventArgs e)
    {
      if (YouTube.CurrentlySigningIn)
        this.userText.put_Text(App.Strings["home.accounts.signingin", "signing in"].ToLower() + "...");
      else if (YouTube.IsSignedIn)
      {
        if (YouTube.UserInfo != null)
        {
          this.userText.put_Text(string.IsNullOrWhiteSpace(YouTube.UserInfo.UserDisplayName) ? App.Strings["channels.google", "Google Account"] : YouTube.UserInfo.UserDisplayName);
          this.bitmap.put_UriSource(YouTube.UserInfo.ThumbUri);
        }
        else
        {
          this.userText.put_Text(App.Strings["common.signedin", "signed in"].ToLower());
          this.bitmap.put_UriSource(new Uri("ms-appx:/Images/UserNoThumb.png"));
        }
      }
      else
      {
        this.userText.put_Text(App.Strings["common.signin", "sign in"].ToLower());
        this.bitmap.put_UriSource(new Uri("ms-appx:/Images/UserNoThumb.png"));
      }
      YouTube.SigningIn += new EventHandler(this.YouTube_SigningIn);
      YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
      YouTube.SignedOut += new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
    }

    private async void YouTube_SignedOut(object sender, EventArgs e) => await ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedOut\u003Eb__6_0)));

    private async void YouTube_SignedIn(object sender, EventArgs e) => await ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedIn\u003Eb__7_0)));

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
        popup1.put_Child((UIElement) control);
        ((FrameworkElement) popup1).put_RequestedTheme(App.Theme);
        Popup popup2 = popup1;
        DefaultPage.SetPopupArrangeMethod((DependencyObject) popup2, (Func<Point>) (() =>
        {
          Rect bounds1 = ((FrameworkElement) this.signInPanel).GetBounds(Window.Current.Content);
          Rect visibleBounds = App.VisibleBounds;
          Rect bounds2 = Window.Current.Bounds;
          ((UIElement) control).UpdateLayout();
          ((FrameworkElement) control).put_Width(Math.Min(bounds1.Width, bounds2.Width - 38.0));
          ((FrameworkElement) control).put_MaxHeight(Math.Min(400.0, bounds2.Height));
          return new Point()
          {
            X = Math.Max(bounds1.Left, 19.0),
            Y = Math.Min(bounds1.Bottom, bounds2.Height - ((FrameworkElement) control).ActualHeight)
          };
        }));
        DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, -80.0), FadeType.Half, hideAppBar: false);
      }
    }

    private void addAccount_Click(object sender, RoutedEventArgs e) => (Window.Current.Content as DefaultPage).OpenBrowser();

    private void signIn_Click(object sender, RoutedEventArgs e)
    {
      if (YouTube.IsSignedIn)
        YouTube.SignOut();
      else
        (Window.Current.Content as DefaultPage).OpenBrowser();
    }

    public async void SignInChanged(bool signIn)
    {
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (((Selector) this.listView).SelectedItem is IconButtonInfo selectedItem)
      {
        if (selectedItem.Name == "channel")
        {
          if (YouTube.IsSignedIn)
            (Application.Current as App).RootFrame.Navigate(typeof (ChannelPage), (object) 0);
          else
            (Window.Current.Content as DefaultPage).OpenBrowser();
        }
        else if (selectedItem.Name == "uploads")
        {
          if (YouTube.IsSignedIn)
            (Application.Current as App).RootFrame.Navigate(typeof (ChannelPage), (object) 0);
          else
            (Window.Current.Content as DefaultPage).OpenBrowser();
        }
        else if (selectedItem.Name == "favorites")
        {
          if (YouTube.IsSignedIn)
          {
            if (YouTube.UserInfo != null)
              DefaultPage.Current.Frame.Navigate(typeof (PlaylistPage), (object) YouTube.UserInfo.FavoritesPlaylist);
            else
              DefaultPage.Current.Frame.Navigate(typeof (PlaylistPage), (object) ("FL" + UserInfo.RemoveUCFromID(SharedSettings.CurrentAccount.UserID)));
          }
          else
            (Window.Current.Content as DefaultPage).OpenBrowser();
        }
        else if (selectedItem.Name == "watch later")
        {
          if (YouTube.IsSignedIn)
          {
            VideoListClient client = WatchLaterPage.Client;
            (Application.Current as App).RootFrame.Navigate(typeof (WatchLaterPage), (object) new ClientAndFirstLoadTask()
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
            (Application.Current as App).RootFrame.Navigate(typeof (PlaylistListPage));
          else
            (Window.Current.Content as DefaultPage).OpenBrowser();
        }
        else if (selectedItem.Name == "saved")
          (Application.Current as App).RootFrame.Navigate(typeof (SavedVideosPage));
        else if (selectedItem.Name == "subscribed to")
        {
          if (YouTube.IsSignedIn)
            (Application.Current as App).RootFrame.Navigate(typeof (SubscribedToPage));
          else
            (Window.Current.Content as DefaultPage).OpenBrowser();
        }
        else if (selectedItem.Name == "history")
          App.Instance.RootFrame.Navigate(typeof (HistoryPage));
        else if (selectedItem.Name == "liked")
          App.Instance.RootFrame.Navigate(typeof (LikesPage));
        else if (selectedItem.Name == "upload")
        {
          if (YouTube.IsSignedIn)
            App.Instance.RootFrame.Navigate(typeof (UploaderPage));
          else
            this.signIn();
        }
      }
      ((Selector) this.listView).put_SelectedItem((object) null);
    }

    private void signIn()
    {
      if (SharedSettings.Accounts.Count == 0)
        (Window.Current.Content as DefaultPage).OpenBrowser();
      else
        this.signInPanel_Tapped((object) null, (TappedRoutedEventArgs) null);
    }

    private void accountNamePanel_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (YouTube.IsSignedIn)
        (Application.Current as App).RootFrame.Navigate(typeof (ChannelPage), (object) 0);
      else if (SharedSettings.Accounts.Count == 0)
        (Window.Current.Content as DefaultPage).OpenBrowser();
      else
        this.signInPanel_Tapped(sender, e);
    }

    private void uploadControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (YouTube.IsSignedIn)
        App.Instance.RootFrame.Navigate(typeof (UploaderPage));
      else
        this.signIn();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///UserHome.xaml"), (ComponentResourceLocation) 0);
      this.thumbBrush = (ImageBrush) ((FrameworkElement) this).FindName("thumbBrush");
      this.signInPanel = (Grid) ((FrameworkElement) this).FindName("signInPanel");
      this.listView = (ListView) ((FrameworkElement) this).FindName("listView");
      this.uploadControl = (ContentControl) ((FrameworkElement) this).FindName("uploadControl");
      this.accountNamePanel = (ContentControl) ((FrameworkElement) this).FindName("accountNamePanel");
      this.thumb = (Ellipse) ((FrameworkElement) this).FindName("thumb");
      this.userText = (TextBlock) ((FrameworkElement) this).FindName("userText");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.ListView_SelectionChanged));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.uploadControl_Tapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.accountNamePanel_Tapped));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.signInPanel_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
