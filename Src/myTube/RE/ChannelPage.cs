// Decompiled with JetBrains decompiler
// Type: myTube.ChannelPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Popups;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class ChannelPage : Page, IComponentConnector
  {
    private bool firstNavigation;
    private int scrollOnLoaded = -1;
    private IconButtonEvent unfaveIcon;
    private RepositionThemeTransition repositionTransition;
    private TranslateTransform scrollTrans;
    private double lastScrollOffset;
    private bool detailsVisible = true;
    private string lastID = "";
    private ChannelNotifications notifiedAbout;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Page page;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform detailsTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ImageBrush bannerBrush;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapImage bannerBitmap;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton pinButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton notifyMe;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapIcon notifyMeIcon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup NotifyMeStates;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState NoNotification;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState NotifyState;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scrollViewer;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PlaylistList playlists;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ChannelDetails details;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList uploads;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private UserInfo Channel => ((FrameworkElement) this).DataContext as UserInfo;

    public ChannelPage()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(ChannelPage_DataContextChanged)));
      this.put_NavigationCacheMode((NavigationCacheMode) 2);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.ChannelPage_Loaded));
      IconButtonEvent iconButtonEvent = new IconButtonEvent();
      iconButtonEvent.Symbol = (Symbol) 57749;
      iconButtonEvent.Text = App.Strings["common.unfavorite", "unfavorite"].ToLower();
      this.unfaveIcon = iconButtonEvent;
      this.unfaveIcon.Selected += new EventHandler<IconButtonEventArgs>(this.unfaveIcon_Selected);
      this.BottomAppBar.put_ClosedDisplayMode((AppBarClosedDisplayMode) 1);
      this.overCanvas.SetSearchParamFunction((Func<object>) (() => (object) (((FrameworkElement) this).DataContext as UserInfo)));
      ScrollViewer scrollViewer1 = this.uploads.ScrollViewer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>(new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scrollViewer1.add_ViewChanged), new Action<EventRegistrationToken>(scrollViewer1.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.UploadsScrollViewChanged));
      ScrollViewer scrollViewer2 = this.uploads.ScrollViewer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangingEventArgs>>(new Func<EventHandler<ScrollViewerViewChangingEventArgs>, EventRegistrationToken>(scrollViewer2.add_ViewChanging), new Action<EventRegistrationToken>(scrollViewer2.remove_ViewChanging), new EventHandler<ScrollViewerViewChangingEventArgs>(this.UploadsScrollViewChanging));
      ((UIElement) this.uploads).put_Transitions(new TransitionCollection());
      this.repositionTransition = new RepositionThemeTransition();
      this.scrollTrans = new TranslateTransform();
      ((UIElement) (((ContentControl) this.uploads.ScrollViewer).Content as FrameworkElement)).put_RenderTransform((Transform) this.scrollTrans);
    }

    private void UploadsScrollViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
    {
    }

    private void UploadsScrollViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
      double verticalOffset = this.uploads.ScrollViewer.VerticalOffset;
      if (verticalOffset > this.lastScrollOffset + 10.0 && verticalOffset > 200.0)
        this.setDetailsVisible(false);
      else if (verticalOffset < 200.0 || verticalOffset < this.lastScrollOffset - 10.0)
        this.setDetailsVisible(true);
      this.lastScrollOffset = verticalOffset;
    }

    private void ChannelPage_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.scrollOnLoaded == -1)
        return;
      this.overCanvas.ScrollToIndex(this.scrollOnLoaded, true);
      this.scrollOnLoaded = -1;
    }

    private void bannerBitmap_ImageOpened(object sender, RoutedEventArgs e) => this.overCanvas.BannerReady = true;

    private void setDetailsVisible(bool set)
    {
      if (set == this.detailsVisible)
        return;
      this.detailsVisible = set;
      if (set)
      {
        ((UIElement) this.details).put_Visibility((Visibility) 0);
        ((UIElement) this.uploads).put_RenderTransform((Transform) this.detailsTrans);
        Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.details, "Opacity", 1.0, 0.2), (Timeline) Ani.Begin((DependencyObject) this.detailsTrans, "Y", 0.0, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)));
        int num;
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => num = this.detailsVisible ? 1 : 0));
      }
      else
      {
        ((UIElement) this.uploads).put_RenderTransform((Transform) this.detailsTrans);
        Timeline[] timelineArray = new Timeline[2]
        {
          (Timeline) Ani.DoubleAni((DependencyObject) this.details, "Opacity", 0.0, 0.2),
          null
        };
        TranslateTransform detailsTrans = this.detailsTrans;
        double actualHeight = ((FrameworkElement) this.details).ActualHeight;
        Thickness margin = ((FrameworkElement) this.details).Margin;
        double top = margin.Top;
        double num = actualHeight + top;
        margin = ((FrameworkElement) this.details).Margin;
        double bottom = margin.Bottom;
        double To = -(num + bottom);
        ExponentialEase Ease = Ani.Ease((EasingMode) 1, 2.0);
        timelineArray[1] = (Timeline) Ani.Begin((DependencyObject) detailsTrans, "Y", To, 0.2, (EasingFunctionBase) Ease);
        Storyboard storyboard = Ani.Begin(timelineArray);
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
        {
          if (this.detailsVisible)
            return;
          ((UIElement) this.uploads).put_RenderTransform((Transform) null);
          ((UIElement) this.details).put_Visibility((Visibility) 1);
        }));
      }
    }

    private async void ChannelPage_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(args.NewValue is UserInfo info))
        return;
      if (UserInfo.RemoveUCFromID(this.lastID) != UserInfo.RemoveUCFromID(info.ID))
      {
        this.setNotifyMeIcon(true);
        try
        {
          if (SecondaryTile.Exists(TileHelper.CreateTileID(new TypeConstructor(typeof (UserFeed), new object[3]
          {
            (object) UserFeed.Uploads,
            (object) info.ID,
            (object) 5
          }))))
          {
            this.pinButton.put_Label(App.Strings["common.unpin", "unpin"].ToLower());
            this.pinButton.put_Icon((IconElement) new SymbolIcon((Symbol) 57750));
          }
          else
          {
            this.pinButton.put_Label(App.Strings["common.pin", "pin"].ToLower());
            this.pinButton.put_Icon((IconElement) new SymbolIcon((Symbol) 57665));
          }
        }
        catch
        {
        }
        this.lastID = UserInfo.RemoveUCFromID(info.ID);
        this.overCanvas.BannerReady = false;
        Task<Uri> bannerUriTask = (Task<Uri>) null;
        if (info.BannerUri == (Uri) null)
          bannerUriTask = new UserInfoClient().GetBannerUri(info.ID);
        else
          this.bannerBitmap.put_UriSource(info.BannerUri);
        this.uploads.Client = (VideoListClient) new UserClient(UserFeed.Uploads, info.ID, 20);
        this.uploads.Client.DontCache();
        this.uploads.Load();
        await Task.Delay(100);
        this.playlists.Client = (YouTubeClient<PlaylistEntry>) new UserPlaylistListClient(info.ID, 20);
        if (bannerUriTask != null)
        {
          try
          {
            Uri uri = await bannerUriTask;
            if (uri != (Uri) null)
            {
              this.bannerBitmap.put_UriSource(uri);
              Helper.Write((object) "Found channel banner URI");
            }
          }
          catch
          {
          }
        }
        bannerUriTask = (Task<Uri>) null;
      }
      else
      {
        await Task.Delay(200);
        this.overCanvas.BannerReady = true;
      }
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      TileArgs tile = App.GetLaunchTileArgs((object) this);
      if (tile == null)
      {
        if (e.NavigationMode != 1)
          this.overCanvas.ScrollToIndex(0, true);
      }
      else
        this.overCanvas.ScrollToIndex(tile.OverCanvasPage, true);
      if (e.Parameter is string && UserInfo.RemoveUCFromID(e.Parameter as string) != this.lastID)
      {
        this.overCanvas.BannerReady = false;
        this.lastID = "";
        if (tile == null)
          this.overCanvas.ScrollToPage(0, true);
        ((FrameworkElement) this).put_DataContext((object) null);
        this.bannerBitmap.put_UriSource((Uri) null);
        try
        {
          ((FrameworkElement) this).put_DataContext((object) await new UserInfoClient().GetInfo(e.Parameter.ToString()));
        }
        catch
        {
        }
      }
      else if (e.Parameter is UserInfo && UserInfo.RemoveUCFromID((e.Parameter as UserInfo).ID) != this.lastID)
      {
        this.overCanvas.BannerReady = false;
        if (tile == null)
          this.overCanvas.ScrollToPage(0, true);
        ((FrameworkElement) this).put_DataContext(e.Parameter);
      }
      else if (e.Parameter is YouTubeEntry && UserInfo.RemoveUCFromID((e.Parameter as YouTubeEntry).Author) != this.lastID)
      {
        this.overCanvas.BannerReady = false;
        this.overCanvas.ScrollToPage(0, true);
        ((FrameworkElement) this).put_DataContext((object) null);
        try
        {
          ((FrameworkElement) this).put_DataContext((object) await new UserInfoClient().GetInfo((e.Parameter as YouTubeEntry).Author));
        }
        catch
        {
        }
      }
      else if (e.Parameter is int && YouTube.IsSignedIn)
      {
        int parameter = (int) e.Parameter;
        this.overCanvas.BannerReady = false;
        if (e.NavigationMode != 1 || ((FrameworkElement) this).DataContext != YouTube.UserInfo)
        {
          this.overCanvas.ScrollToIndex(parameter, true);
          if (!this.firstNavigation)
            this.scrollOnLoaded = parameter;
        }
        ((FrameworkElement) this).put_DataContext((object) YouTube.UserInfo);
      }
      else if (e.Parameter == null && YouTube.IsSignedIn)
      {
        this.overCanvas.BannerReady = false;
        ((FrameworkElement) this).put_DataContext((object) YouTube.UserInfo);
      }
      this.firstNavigation = true;
    }

    private void bannerBitmap_ImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
    }

    private void overCanvas_SelectedPageChanged(object sender, OnSelectedPageChangedEventArgs e)
    {
      if (e.NewPage == OverCanvas.GetOverCanvasPage((DependencyObject) this.details))
        this.BottomAppBar.put_ClosedDisplayMode((AppBarClosedDisplayMode) 0);
      else
        this.BottomAppBar.put_ClosedDisplayMode((AppBarClosedDisplayMode) 1);
    }

    private void favorites_SetContextMenu(object sender, List<IconButtonEvent> e)
    {
      if (((FrameworkElement) this).DataContext != YouTube.UserInfo)
        return;
      e.Insert(0, this.unfaveIcon);
    }

    private async void unfaveIcon_Selected(object sender, IconButtonEventArgs e)
    {
      e.Close();
      if (e.OriginalSender is FrameworkElement originalSender1 && originalSender1.DataContext is YouTubeEntry dataContext)
      {
        string favoriteId1 = dataContext.FavoriteID;
      }
      if (!(e.OriginalSender is IEnumerable<YouTubeEntry> originalSender2))
        return;
      foreach (YouTubeEntry youTubeEntry in originalSender2)
      {
        string favoriteId2 = youTubeEntry.FavoriteID;
      }
    }

    private void overCanvas_ShownChanged(object sender, bool e)
    {
      if (e)
        ((UIElement) this.BottomAppBar).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.BottomAppBar).put_Visibility((Visibility) 1);
    }

    private async void pinButton_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is UserInfo info))
        return;
      TypeConstructor tc = new TypeConstructor(typeof (UserClient), new object[3]
      {
        (object) UserFeed.Uploads,
        (object) info.ID,
        (object) 5
      });
      if (!SecondaryTile.Exists(TileHelper.CreateTileID(tc)))
      {
        tc.Construct();
        SecondaryTile tile = await TileHelper.CreateTile(tc, info.UserDisplayName, new TileArgs(typeof (ChannelPage), info.ID, 0));
        Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>> renderTask = (Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>>) (async (el, rtb) => await DefaultPage.Current.RenderElementAsync(el, 0.5, rtb));
        Border border1 = new Border();
        ((FrameworkElement) border1).put_Width(Window.Current.Bounds.Width);
        ((FrameworkElement) border1).put_Height(Window.Current.Bounds.Height);
        Border border2 = border1;
        ProgressBar progressBar = new ProgressBar();
        ((Control) progressBar).put_Background((Brush) null);
        ((Control) progressBar).put_Foreground((Brush) App.Instance.GetThemeResource("AccentBrush"));
        progressBar.put_IsIndeterminate(true);
        border2.put_Child((UIElement) progressBar);
        Popup popup = new Popup();
        popup.put_Child((UIElement) border1);
        DefaultPage.Current.ShowPopup(popup, new Point(), new Point(), FadeType.Half, false);
        List<TileNotification> nots = await TileHelper.UpdateSecondaryTile(tc, renderTask, false);
        if (await tile.RequestCreateAsync())
        {
          TileUpdater forSecondaryTile = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId);
          forSecondaryTile.Clear();
          forSecondaryTile.EnableNotificationQueue(true);
          foreach (TileNotification tileNotification in nots)
            forSecondaryTile.Update(tileNotification);
          App.Instance.SetUpBackgroundTask();
        }
        else
          TileHelper.CleanUpFolders();
        DefaultPage.Current.ClosePopup();
        this.pinButton.put_Label(App.Strings["common.unpin", "unpin"].ToLower());
        this.pinButton.put_Icon((IconElement) new SymbolIcon((Symbol) 57750));
        tile = (SecondaryTile) null;
        nots = (List<TileNotification>) null;
      }
      else if (await (await TileHelper.CreateTile(tc, info.ID, new TileArgs())).RequestDeleteAsync())
      {
        this.pinButton.put_Label(App.Strings["common.pin", "pin"].ToLower());
        this.pinButton.put_Icon((IconElement) new SymbolIcon((Symbol) 57665));
      }
      tc = (TypeConstructor) null;
    }

    private void overCanvas_ScrollingStopped(object sender, OnScrollingStoppedEventArgs e)
    {
    }

    private async Task setNotifyMeIcon(bool checkChannels = false)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      if (this.Channel == null)
        return;
      bool hasChannel = false;
      if (App.GlobalObjects.ChannelNotifications.ContainsChannel(this.Channel.ID))
      {
        this.notifyMeIcon.put_UriSource(new Uri("ms-appx:///Icons/notification.reverse.png", UriKind.Absolute));
        hasChannel = true;
      }
      else
      {
        this.notifyMeIcon.put_UriSource(new Uri("ms-appx:///Icons/notification.png", UriKind.Absolute));
        hasChannel = false;
      }
      if (this.notifiedAbout == null)
      {
        ChannelPage channelPage = this;
        ChannelNotifications notifiedAbout = channelPage.notifiedAbout;
        ChannelNotifications channelNotifications = await ChannelNotifications.Load(ApplicationData.Current.LocalFolder, "NotifiedAbout.json");
        channelPage.notifiedAbout = channelNotifications;
        channelPage = (ChannelPage) null;
      }
      string[] ids = this.notifiedAbout.GetIds();
      bool changed = false;
      foreach (string channelId in ids)
      {
        if (!App.GlobalObjects.ChannelNotifications.ContainsChannel(channelId))
        {
          this.notifiedAbout.RemoveChannel(channelId);
          changed = true;
        }
      }
      if (checkChannels & hasChannel)
      {
        RSSUploadsClient rssUploadsClient = new RSSUploadsClient(this.Channel.ID);
        try
        {
          YouTubeEntry[] feed = await rssUploadsClient.GetFeed(0);
          if (feed.Length != 0)
          {
            if (this.notifiedAbout.AddOrModifyChannel(this.Channel.ID, feed[0].ID))
              changed = true;
          }
        }
        catch
        {
        }
      }
      if (!changed)
        return;
      await this.notifiedAbout.Save(ApplicationData.Current.LocalFolder, "NotifiedAbout.json");
    }

    private async void notifyMe_Click(object sender, RoutedEventArgs e)
    {
      if (DefaultPage.Current.PopupShown)
      {
        int num1 = await DefaultPage.Current.ClosePopup() ? 1 : 0;
      }
      else
      {
        int num2 = await App.GlobalObjects.InitializedTask ? 1 : 0;
        ChannelNotificationsPopup not = new ChannelNotificationsPopup();
        ((FrameworkElement) not).put_DataContext(((FrameworkElement) this).DataContext);
        Popup popup1 = new Popup();
        popup1.put_Child((UIElement) not);
        Popup popup2 = popup1;
        DefaultPage.SetPopupArrangeMethod((DependencyObject) popup2, (Func<Point>) (() =>
        {
          Rect visibleBounds = App.VisibleBounds;
          ((FrameworkElement) not).put_Height(Math.Min(visibleBounds.Height - 76.0, 700.0));
          ((FrameworkElement) not).put_Width(Math.Min(500.0, visibleBounds.Width - 38.0));
          double y = visibleBounds.Height - ((FrameworkElement) not).Height;
          return new Point((visibleBounds.Width - ((FrameworkElement) not).Width) / 2.0, y);
        }));
        string[] ids = App.GlobalObjects.ChannelNotifications.GetIds();
        int num3 = await DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, 80.0)) ? 1 : 0;
        string[] ids1 = App.GlobalObjects.ChannelNotifications.GetIds();
        bool flag = false;
        foreach (string str in ids)
        {
          if (!Enumerable.Contains<string>((IEnumerable<string>) ids1, str))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          foreach (string str in ids1)
          {
            if (!Enumerable.Contains<string>((IEnumerable<string>) ids, str))
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          return;
        await this.setNotifyMeIcon();
        try
        {
          await App.GlobalObjects.ChannelNotifications.Save();
        }
        catch
        {
        }
        if (!App.GlobalObjects.ChannelNotifications.ContainsChannel(this.Channel.ID) || ((Collection<YouTubeEntry>) this.uploads.Entries).Count <= 0)
          return;
        YouTubeEntry vid = ((Collection<YouTubeEntry>) this.uploads.Entries)[0];
        if (this.notifiedAbout == null)
        {
          ChannelPage channelPage = this;
          ChannelNotifications notifiedAbout = channelPage.notifiedAbout;
          ChannelNotifications channelNotifications = await ChannelNotifications.Load(ApplicationData.Current.LocalFolder, "NotifiedAbout.json");
          channelPage.notifiedAbout = channelNotifications;
          channelPage = (ChannelPage) null;
        }
        if (!this.notifiedAbout.ContainsChannel(this.Channel.ID) || this.notifiedAbout.GetName(this.Channel.ID) != vid.ID)
        {
          this.notifiedAbout.AddOrModifyChannel(this.Channel.ID, vid.ID);
          try
          {
            await this.notifiedAbout.Save(ApplicationData.Current.LocalFolder, "NotifiedAbout.json");
          }
          catch
          {
          }
        }
        vid = (YouTubeEntry) null;
      }
    }

    private async void uploads_VideosLoaded(object sender, YouTubeEntry[] e)
    {
    }

    private void favorites_SetMultiselectContextMenu(object sender, IconButtonEventCollection e)
    {
      if (((FrameworkElement) this).DataContext != YouTube.UserInfo)
        return;
      e.Insert(0, this.unfaveIcon);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ChannelPage.xaml"), (ComponentResourceLocation) 0);
      this.page = (Page) ((FrameworkElement) this).FindName("page");
      this.detailsTrans = (TranslateTransform) ((FrameworkElement) this).FindName("detailsTrans");
      this.bannerBrush = (ImageBrush) ((FrameworkElement) this).FindName("bannerBrush");
      this.bannerBitmap = (BitmapImage) ((FrameworkElement) this).FindName("bannerBitmap");
      this.pinButton = (AppBarButton) ((FrameworkElement) this).FindName("pinButton");
      this.notifyMe = (AppBarButton) ((FrameworkElement) this).FindName("notifyMe");
      this.notifyMeIcon = (BitmapIcon) ((FrameworkElement) this).FindName("notifyMeIcon");
      this.NotifyMeStates = (VisualStateGroup) ((FrameworkElement) this).FindName("NotifyMeStates");
      this.NoNotification = (VisualState) ((FrameworkElement) this).FindName("NoNotification");
      this.NotifyState = (VisualState) ((FrameworkElement) this).FindName("NotifyState");
      this.scrollViewer = (ScrollViewer) ((FrameworkElement) this).FindName("scrollViewer");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.playlists = (PlaylistList) ((FrameworkElement) this).FindName("playlists");
      this.details = (ChannelDetails) ((FrameworkElement) this).FindName("details");
      this.uploads = (VideoList) ((FrameworkElement) this).FindName("uploads");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          BitmapImage bitmapImage1 = (BitmapImage) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(bitmapImage1.add_ImageOpened), new Action<EventRegistrationToken>(bitmapImage1.remove_ImageOpened), new RoutedEventHandler(this.bannerBitmap_ImageOpened));
          BitmapImage bitmapImage2 = (BitmapImage) target;
          WindowsRuntimeMarshal.AddEventHandler<ExceptionRoutedEventHandler>(new Func<ExceptionRoutedEventHandler, EventRegistrationToken>(bitmapImage2.add_ImageFailed), new Action<EventRegistrationToken>(bitmapImage2.remove_ImageFailed), new ExceptionRoutedEventHandler(this.bannerBitmap_ImageFailed));
          break;
        case 2:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.pinButton_Click));
          break;
        case 3:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.notifyMe_Click));
          break;
        case 4:
          ((OverCanvas) target).SelectedPageChanged += new EventHandler<OnSelectedPageChangedEventArgs>(this.overCanvas_SelectedPageChanged);
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
          ((OverCanvas) target).ScrollingStopped += new EventHandler<OnScrollingStoppedEventArgs>(this.overCanvas_ScrollingStopped);
          break;
        case 5:
          ((VideoList) target).VideosLoaded += new EventHandler<YouTubeEntry[]>(this.uploads_VideosLoaded);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
