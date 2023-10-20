// Decompiled with JetBrains decompiler
// Type: myTube.VideoDetails
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public sealed class VideoDetails : UserControl, IComponentConnector
  {
    private Dictionary<string, bool> likeStatuses = new Dictionary<string, bool>();
    private bool progressShown;
    private OverCanvas overCanvas;
    private UserInfoClient userClient;
    private UserInfo currentUserInfo;
    private string lastId = "";
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapImage userThumbBitmap;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ImageBrush userThumbBrush;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoPageThumb thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid progressGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock bookmarkText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private RichTextBlock description;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton saveButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton addToButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton shareButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock linkText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl dislikeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl likeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform likesBarScale;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid userThumbGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid subscribeBorder;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton subscribeIcon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock progressText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform progressTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public VideoPageThumb Thumb => this.thumb;

    public IconTextButton SaveButton => this.saveButton;

    public IconTextButton ShareButton => this.shareButton;

    public IconTextButton AddToButton => this.addToButton;

    public YouTubeEntry Entry => ((FrameworkElement) this).DataContext as YouTubeEntry;

    public VideoDetails()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.VideoDetails_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.VideoDetails_Unloaded));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(VideoDetails_DataContextChanged)));
      ((UIElement) this.progressGrid).put_Visibility((Visibility) 1);
      UserInfoClient userInfoClient = new UserInfoClient();
      userInfoClient.APIKey = App.GetAPIKey(2);
      userInfoClient.UseAccessToken = false;
      this.userClient = userInfoClient;
      ImageBrush userThumbBrush = this.userThumbBrush;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(userThumbBrush.add_ImageOpened), new Action<EventRegistrationToken>(userThumbBrush.remove_ImageOpened), new RoutedEventHandler(this.UserThumbBrush_ImageOpened));
    }

    private void UserThumbBrush_ImageOpened(object sender, RoutedEventArgs e)
    {
    }

    private async void VideoDetails_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      YouTubeEntry ent = ((FrameworkElement) this).DataContext as YouTubeEntry;
      if (ent.ID == this.lastId)
        return;
      this.lastId = ent.ID;
      int num1 = await App.GlobalObjects.InitializedTask ? 1 : 0;
      this.setSubscribeButton();
      ((FrameworkElement) this.bookmarkText).put_DataContext((object) App.GlobalObjects.TimeBookmarksManager.Get(ent.ID));
      ContentControl likeButton1 = this.likeButton;
      bool flag;
      ((UIElement) this.dislikeButton).put_IsHitTestVisible(flag = true);
      int num2 = flag ? 1 : 0;
      ((UIElement) likeButton1).put_IsHitTestVisible(num2 != 0);
      ContentControl likeButton2 = this.likeButton;
      double num3;
      ((UIElement) this.dislikeButton).put_Opacity(num3 = 1.0);
      double num4 = num3;
      ((UIElement) likeButton2).put_Opacity(num4);
      this.setLikeButtonsInDataContext(ent);
      if (this.description != null)
      {
        RichTextBlock description = this.description;
        ((ICollection<Block>) description.Blocks).Clear();
        ((ICollection<Block>) description.Blocks).Add((Block) new TextToHyperlinkBlocksConverter().Convert(ent.Description));
      }
      if (await App.TaskDispatcher.AddTask<TransferManager.State>((Func<Task<TransferManager.State>>) (() => App.GlobalObjects.TransferManager.GetTransferState(ent))) != TransferManager.State.None)
      {
        this.saveButton.Text = App.Strings["common.manage", "manage"].ToLower();
        this.saveButton.Symbol = (Symbol) 57605;
      }
      else
      {
        this.saveButton.Text = App.Strings["common.save", "save"].ToLower();
        this.saveButton.Symbol = (Symbol) 57624;
      }
      try
      {
        VideoDetails videoDetails = this;
        UserInfo currentUserInfo = videoDetails.currentUserInfo;
        UserInfo info = await this.userClient.GetInfo(ent.Author);
        videoDetails.currentUserInfo = info;
        videoDetails = (VideoDetails) null;
      }
      catch
      {
      }
      if (this.currentUserInfo != null && this.currentUserInfo.ID == ent.Author)
        this.userThumbBitmap.put_UriSource(this.currentUserInfo.ThumbUri);
    }

    private async Task setLikeButtonsInDataContext(YouTubeEntry ent)
    {
      if (this.likeStatuses.ContainsKey(ent.ID))
      {
        this.setLikeButtons(this.likeStatuses[ent.ID]);
      }
      else
      {
        this.resetLikeButtons();
        try
        {
          LikedResult[] likedResultArray = await App.TaskDispatcher.AddTask<LikedResult[]>((Func<Task<LikedResult[]>>) (() => new YouTubeEntryClient().GetRating(ent.ID)));
          if (!likedResultArray[0].Liked.HasValue)
            return;
          this.likeStatuses.Add(ent.ID, likedResultArray[0].Liked.Value);
          this.setLikeButtons(likedResultArray[0].Liked.Value);
        }
        catch
        {
        }
      }
    }

    public void ProgressCallback(DownloadOperation download)
    {
      double num = (double) download.Progress.BytesReceived / (double) download.Progress.TotalBytesToReceive;
      if (num == 1.0)
      {
        this.HideProgress();
      }
      else
      {
        this.ShowProgress();
        if (double.IsNaN(num) || double.IsInfinity(num))
          return;
        Ani.Begin((DependencyObject) this.progressTrans, "ScaleX", num, 0.4, 4.0);
      }
    }

    public void ShowProgress()
    {
      if (this.progressShown)
        return;
      ((UIElement) this.progressGrid).put_Opacity(0.0);
      this.progressTrans.put_ScaleX(0.0);
      ((UIElement) this.progressGrid).put_Visibility((Visibility) 0);
      Ani.Begin((DependencyObject) this.progressGrid, "Opacity", 1.0, 0.2);
      this.progressShown = true;
    }

    public void HideProgress()
    {
      if (!this.progressShown)
        return;
      this.progressShown = false;
      Storyboard storyboard = Ani.Begin((DependencyObject) this.progressGrid, "Opacity", 0.0, 0.2);
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
      {
        ((UIElement) this.progressGrid).put_Visibility((Visibility) 1);
        this.progressTrans.put_ScaleX(0.0);
      }));
    }

    private void resetLikeButtons()
    {
      try
      {
        ContentControl likeButton1 = this.likeButton;
        Brush themeResource;
        ((Control) this.dislikeButton).put_Background(themeResource = (Brush) App.Instance.GetThemeResource("AccentBrush"));
        Brush brush1 = themeResource;
        ((Control) likeButton1).put_Background(brush1);
        ContentControl likeButton2 = this.likeButton;
        Brush brush2;
        ((Control) this.dislikeButton).put_Foreground(brush2 = (Brush) new SolidColorBrush(Colors.White));
        Brush brush3 = brush2;
        ((Control) likeButton2).put_Foreground(brush3);
      }
      catch
      {
      }
    }

    private void setSubscribeButton()
    {
      if (this.Entry == null)
        return;
      if (YouTube.IsSignedIn)
      {
        ((UIElement) this.subscribeBorder).put_Visibility((Visibility) 0);
        if (YouTube.IsSubscribedTo(this.Entry.Author))
          this.subscribeIcon.Symbol = (Symbol) 57608;
        else
          this.subscribeIcon.Symbol = (Symbol) 57609;
      }
      else
        ((UIElement) this.subscribeBorder).put_Visibility((Visibility) 1);
    }

    private void setLikeButtons(bool like)
    {
      if (like)
      {
        ((Control) this.likeButton).put_Foreground((Brush) App.Instance.GetThemeResource("AccentBrush"));
        ((Control) this.likeButton).put_Background((Brush) new SolidColorBrush(Colors.White));
        ((Control) this.dislikeButton).put_Background((Brush) App.Instance.GetThemeResource("AccentBrush"));
        ((Control) this.dislikeButton).put_Foreground((Brush) new SolidColorBrush(Colors.White));
      }
      else
      {
        ((Control) this.dislikeButton).put_Foreground((Brush) App.Instance.GetThemeResource("AccentBrush"));
        ((Control) this.dislikeButton).put_Background((Brush) new SolidColorBrush(Colors.White));
        ((Control) this.likeButton).put_Background((Brush) App.Instance.GetThemeResource("AccentBrush"));
        ((Control) this.likeButton).put_Foreground((Brush) new SolidColorBrush(Colors.White));
      }
    }

    private void VideoDetails_Loaded(object sender, RoutedEventArgs e)
    {
      this.CheckForComments();
      if (this.overCanvas == null)
      {
        this.overCanvas = Helper.FindParent<OverCanvas>((FrameworkElement) this, 100);
        this.overCanvas.ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
      }
      this.setSubscribeButton();
      YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
      YouTube.SignedOut += new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
      YouTube.SubscriptionsLoaded += new EventHandler(this.YouTube_SubscriptionsLoaded);
    }

    private void YouTube_SubscriptionsLoaded(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SubscriptionsLoaded\u003Eb__27_0)));

    private void YouTube_SignedOut(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedOut\u003Eb__28_0)));

    private void YouTube_SignedIn(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedIn\u003Eb__29_0)));

    private void VideoDetails_Unloaded(object sender, RoutedEventArgs e)
    {
      YouTube.SignedIn -= new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
      YouTube.SignedOut -= new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
      YouTube.SubscriptionsLoaded -= new EventHandler(this.YouTube_SubscriptionsLoaded);
    }

    private async void overCanvas_ShownChanged(object sender, bool e)
    {
      if (!e || !(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      ((FrameworkElement) this.bookmarkText).put_DataContext((object) App.GlobalObjects.TimeBookmarksManager.Get((((FrameworkElement) this).DataContext as YouTubeEntry).ID));
    }

    private async void CheckForComments()
    {
    }

    private void author_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (((FrameworkElement) this).DataContext == null || !(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      (Application.Current as App).RootFrame.Navigate(typeof (ChannelPage), (object) (((FrameworkElement) this).DataContext as YouTubeEntry).Author);
    }

    private async void Border_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private async void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement) || !(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      YouTubeEntry ent = ((FrameworkElement) this).DataContext as YouTubeEntry;
      FrameworkElement fe = sender as FrameworkElement;
      ((UIElement) fe).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) fe, "Opacity", 0.5, 0.2);
      bool like = fe != this.dislikeButton;
      try
      {
        bool likeStatus = false;
        bool hasLikeStatus = this.likeStatuses.ContainsKey(ent.ID);
        likeStatus = hasLikeStatus && this.likeStatuses[ent.ID];
        if (!hasLikeStatus || likeStatus != like)
        {
          int num = await YouTube.Rate((((FrameworkElement) this).DataContext as YouTubeEntry).ID, new bool?(like)) ? 1 : 0;
          if (like)
          {
            if (!hasLikeStatus || !likeStatus)
              ++ent.Likes;
            if (hasLikeStatus && !likeStatus)
              --ent.Dislikes;
          }
          else
          {
            if (!hasLikeStatus | likeStatus)
              ++ent.Dislikes;
            if (hasLikeStatus & likeStatus)
              --ent.Likes;
          }
          if (!hasLikeStatus)
            this.likeStatuses.Add(ent.ID, like);
          else
            this.likeStatuses[ent.ID] = like;
          this.setLikeButtons(like);
        }
        else if (hasLikeStatus)
        {
          if (likeStatus == like)
          {
            if (like)
              --ent.Likes;
            else
              --ent.Dislikes;
            int num = await YouTube.Rate(ent.ID, new bool?()) ? 1 : 0;
            if (this.likeStatuses.ContainsKey(ent.ID))
              this.likeStatuses.Remove(ent.ID);
            this.resetLikeButtons();
          }
        }
      }
      catch (WebException ex)
      {
        this.resetLikeButtons();
        MessageDialog messageDialog = new MessageDialog("We're having a bit of trouble rating this video", "Oops");
        messageDialog.Commands.Add((IUICommand) new UICommand("okay :("));
        messageDialog.ShowAsync();
      }
      Ani.Begin((DependencyObject) fe, "Opacity", 1.0, 0.2);
      ((UIElement) fe).put_IsHitTestVisible(true);
      ent = (YouTubeEntry) null;
      fe = (FrameworkElement) null;
    }

    private async void saveButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry ent))
        return;
      DownloaderPanel downloaderPanel = new DownloaderPanel();
      ((FrameworkElement) downloaderPanel).put_DataContext((object) ent);
      ((Control) downloaderPanel).put_VerticalContentAlignment((VerticalAlignment) 2);
      ((FrameworkElement) downloaderPanel).put_RequestedTheme(Settings.Theme);
      DownloaderPanel panel = downloaderPanel;
      panel.VideoProgressCallbacks.Add(new Action<DownloadOperation>(this.ProgressCallback));
      Popup popup1 = new Popup();
      popup1.put_Child((UIElement) panel);
      Popup popup2 = popup1;
      DefaultPage.SetPopupArrangeMethod((DependencyObject) popup2, (Func<Point>) (() =>
      {
        Rect bounds1 = ((FrameworkElement) this).GetBounds(Window.Current.Content);
        Rect bounds2 = ((FrameworkElement) this.saveButton).GetBounds(Window.Current.Content);
        ((FrameworkElement) panel).put_Width(bounds1.Width);
        ((FrameworkElement) panel).put_Height(360.0);
        return new Point()
        {
          X = bounds1.X,
          Y = Math.Max(bounds2.Bottom - ((FrameworkElement) panel).Height, 0.0)
        };
      }));
      int num = await DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, 80.0)) ? 1 : 0;
      if (await App.GlobalObjects.TransferManager.GetTransferState(ent) != TransferManager.State.None)
      {
        this.saveButton.Text = App.Strings["common.manage", "manage"].ToLower();
        this.saveButton.Symbol = (Symbol) 57605;
      }
      else
      {
        this.saveButton.Text = App.Strings["common.save", "save"].ToLower();
        this.saveButton.Symbol = (Symbol) 57624;
      }
    }

    private async void addToButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry dataContext))
        return;
      PlaylistSelectorControl playlistSelectorControl = new PlaylistSelectorControl();
      ((FrameworkElement) playlistSelectorControl).put_DataContext((object) dataContext);
      ((FrameworkElement) playlistSelectorControl).put_Margin(new Thickness(0.0, 19.0, 0.0, 0.0));
      PlaylistSelectorControl control = playlistSelectorControl;
      ((FrameworkElement) control).put_RequestedTheme(Settings.Theme);
      control.SelectionMade += (EventHandler) ((_param1, _param2) => DefaultPage.Current.ClosePopup());
      Popup popup1 = new Popup();
      popup1.put_Child((UIElement) control);
      Popup popup2 = popup1;
      DefaultPage.SetPopupArrangeMethod((DependencyObject) popup2, (Func<Point>) (() =>
      {
        Rect bounds1 = ((FrameworkElement) this).GetBounds(Window.Current.Content);
        Rect bounds2 = ((FrameworkElement) this.addToButton).GetBounds(Window.Current.Content);
        ((FrameworkElement) control).put_Width(bounds1.Width);
        ((FrameworkElement) control).put_Height(bounds2.Bottom);
        if (((FrameworkElement) control).Height < 380.0)
          ((FrameworkElement) control).put_Height(Math.Min(500.0, App.VisibleBounds.Height));
        return new Point() { X = bounds1.X, Y = 0.0 };
      }));
      int num = await DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, 80.0)) ? 1 : 0;
    }

    private async void shareButton_Tapped(object sender, TappedRoutedEventArgs e) => DataTransferManager.ShowShareUI();

    private void Share_Click(object sender, RoutedEventArgs e) => DataTransferManager.ShowShareUI();

    private void linkText_SelectionChanged(object sender, RoutedEventArgs e)
    {
    }

    private void linkText_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
    }

    private async void linkText_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private void ImageBrush_ImageOpened(object sender, RoutedEventArgs e)
    {
    }

    private async void subscribeBorder_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!YouTube.IsSignedIn || this.Entry == null)
        return;
      ((UIElement) this.subscribeBorder).put_IsHitTestVisible(false);
      Ani.Begin((DependencyObject) this.subscribeBorder, "Opacity", 0.5, 0.2);
      if (YouTube.IsSubscribedTo(this.Entry.Author))
      {
        try
        {
          string subscriptionId = YouTube.GetSubscriptionID(this.Entry.Author);
          if (subscriptionId != null)
          {
            int num = await YouTube.Unsubscribe(subscriptionId) ? 1 : 0;
            YouTube.RemoveSubscribedUser(this.Entry.Author);
            YouTube.CallSubscriptionsLoaded();
          }
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          YouTube.InsertSubscription(await YouTube.Subscribe(this.Entry.Author));
        }
        catch
        {
        }
      }
      this.setSubscribeButton();
      ((UIElement) this.subscribeBorder).put_IsHitTestVisible(true);
      Ani.Begin((DependencyObject) this.subscribeBorder, "Opacity", 1.0, 0.2);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///VideoDetails.xaml"), (ComponentResourceLocation) 0);
      this.userThumbBitmap = (BitmapImage) ((FrameworkElement) this).FindName("userThumbBitmap");
      this.userThumbBrush = (ImageBrush) ((FrameworkElement) this).FindName("userThumbBrush");
      this.thumb = (VideoPageThumb) ((FrameworkElement) this).FindName("thumb");
      this.progressGrid = (Grid) ((FrameworkElement) this).FindName("progressGrid");
      this.bookmarkText = (TextBlock) ((FrameworkElement) this).FindName("bookmarkText");
      this.description = (RichTextBlock) ((FrameworkElement) this).FindName("description");
      this.saveButton = (IconTextButton) ((FrameworkElement) this).FindName("saveButton");
      this.addToButton = (IconTextButton) ((FrameworkElement) this).FindName("addToButton");
      this.shareButton = (IconTextButton) ((FrameworkElement) this).FindName("shareButton");
      this.linkText = (TextBlock) ((FrameworkElement) this).FindName("linkText");
      this.dislikeButton = (ContentControl) ((FrameworkElement) this).FindName("dislikeButton");
      this.likeButton = (ContentControl) ((FrameworkElement) this).FindName("likeButton");
      this.likesBarScale = (ScaleTransform) ((FrameworkElement) this).FindName("likesBarScale");
      this.userThumbGrid = (Grid) ((FrameworkElement) this).FindName("userThumbGrid");
      this.subscribeBorder = (Grid) ((FrameworkElement) this).FindName("subscribeBorder");
      this.subscribeIcon = (IconTextButton) ((FrameworkElement) this).FindName("subscribeIcon");
      this.progressText = (TextBlock) ((FrameworkElement) this).FindName("progressText");
      this.progressTrans = (ScaleTransform) ((FrameworkElement) this).FindName("progressTrans");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ImageBrush imageBrush = (ImageBrush) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(imageBrush.add_ImageOpened), new Action<EventRegistrationToken>(imageBrush.remove_ImageOpened), new RoutedEventHandler(this.ImageBrush_ImageOpened));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.Border_Tapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.saveButton_Tapped));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.addToButton_Tapped));
          break;
        case 5:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.shareButton_Tapped));
          break;
        case 6:
          TextBlock textBlock = (TextBlock) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(textBlock.add_SelectionChanged), new Action<EventRegistrationToken>(textBlock.remove_SelectionChanged), new RoutedEventHandler(this.linkText_SelectionChanged));
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement5.add_RightTapped), new Action<EventRegistrationToken>(uiElement5.remove_RightTapped), new RightTappedEventHandler(this.linkText_RightTapped));
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.linkText_Tapped));
          break;
        case 7:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement7.add_Tapped), new Action<EventRegistrationToken>(uiElement7.remove_Tapped), new TappedEventHandler(this.IconTextButton_Tapped));
          break;
        case 8:
          UIElement uiElement8 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement8.add_Tapped), new Action<EventRegistrationToken>(uiElement8.remove_Tapped), new TappedEventHandler(this.IconTextButton_Tapped));
          break;
        case 9:
          UIElement uiElement9 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement9.add_Tapped), new Action<EventRegistrationToken>(uiElement9.remove_Tapped), new TappedEventHandler(this.author_Tapped));
          break;
        case 10:
          UIElement uiElement10 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement10.add_Tapped), new Action<EventRegistrationToken>(uiElement10.remove_Tapped), new TappedEventHandler(this.subscribeBorder_Tapped));
          break;
        case 11:
          UIElement uiElement11 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement11.add_Tapped), new Action<EventRegistrationToken>(uiElement11.remove_Tapped), new TappedEventHandler(this.author_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
