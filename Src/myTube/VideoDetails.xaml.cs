// myTube.VideoDetails

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
using RykenTube;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Networking.BackgroundTransfer;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
    public sealed partial class VideoDetails : UserControl
    {
        private Dictionary<string, bool> likeStatuses = new Dictionary<string, bool>();
        private bool progressShown;
        private OverCanvas overCanvas;
        private UserInfoClient userClient;
        private UserInfo currentUserInfo;
        private string lastId = "";
        /*
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
        */

        public VideoPageThumb Thumb
        {
            get
            {
                //TODO
                return default;// this.thumb;
            }
        }

        public IconTextButton SaveButton => this.saveButton;

        public IconTextButton ShareButton => this.shareButton;

        public IconTextButton AddToButton => this.addToButton;

        public YouTubeEntry Entry => ((FrameworkElement)this).DataContext as YouTubeEntry;

        public VideoDetails()
        {
            this.InitializeComponent();
            this.Loaded += this.VideoDetails_Loaded;
            this.Unloaded += this.VideoDetails_Unloaded;
            this.DataContextChanged += this.VideoDetails_DataContextChanged;
            this.progressGrid.Visibility = Visibility.Collapsed;
            
            UserInfoClient userInfoClient = new UserInfoClient();
            userInfoClient.APIKey = App.GetAPIKey(2);
            userInfoClient.UseAccessToken = false;
            this.userClient = userInfoClient;
            ImageBrush userThumbBrush = this.userThumbBrush;

            userThumbBrush.ImageOpened += this.UserThumbBrush_ImageOpened;
        }

        private void UserThumbBrush_ImageOpened(object sender, RoutedEventArgs e)
        {
        }

        private async void VideoDetails_DataContextChanged(
          FrameworkElement sender,
          DataContextChangedEventArgs args)
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            YouTubeEntry ent = ((FrameworkElement)this).DataContext as YouTubeEntry;
            if (ent.ID == this.lastId)
                return;
            this.lastId = ent.ID;
            int num1 = await App.GlobalObjects.InitializedTask ? 1 : 0;
            this.setSubscribeButton();
            this.bookmarkText.DataContext = (object)App.GlobalObjects.TimeBookmarksManager.Get(ent.ID);
            ContentControl likeButton1 = this.likeButton;
            bool flag;
            this.dislikeButton.IsHitTestVisible = flag = true;
            int num2 = flag ? 1 : 0;
            likeButton1.IsHitTestVisible = num2 != 0;
            ContentControl likeButton2 = this.likeButton;
            double num3;
            this.dislikeButton.Opacity = num3 = 1.0;
            double num4 = num3;
            likeButton2.Opacity = num4;
            this.setLikeButtonsInDataContext(ent);
            if (this.description != null)
            {
                RichTextBlock description = this.description;
                description.Blocks.Clear();
                description.Blocks.Add((Block)new TextToHyperlinkBlocksConverter().Convert(ent.Description));
            }
            if (await App.TaskDispatcher.AddTask<TransferManager.State>((Func<Task<TransferManager.State>>)(() => App.GlobalObjects.TransferManager.GetTransferState(ent))) != TransferManager.State.None)
            {
                //TODO
                //this.saveButton.Text = App.Strings["common.manage", "manage"].ToLower();
                //this.saveButton.Symbol = (Symbol)57605;
            }
            else
            {
                //this.saveButton.Text = App.Strings["common.save", "save"].ToLower();
                //this.saveButton.Symbol = (Symbol)57624;
            }
            try
            {
                VideoDetails videoDetails = this;
                UserInfo currentUserInfo = videoDetails.currentUserInfo;
                UserInfo info = await this.userClient.GetInfo(ent.Author);
                videoDetails.currentUserInfo = info;
                videoDetails = (VideoDetails)null;
            }
            catch
            {
            }
            if (this.currentUserInfo != null && this.currentUserInfo.ID == ent.Author)
                this.userThumbBitmap.UriSource = this.currentUserInfo.ThumbUri;
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
                    LikedResult[] likedResultArray = 
                        await App.TaskDispatcher.AddTask<LikedResult[]>((Func<Task<LikedResult[]>>)(
                        () => new YouTubeEntryClient().GetRating(ent.ID)));

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
            double num = (double)download.Progress.BytesReceived / (double)download.Progress.TotalBytesToReceive;
            if (num == 1.0)
            {
                this.HideProgress();
            }
            else
            {
                this.ShowProgress();
                if (double.IsNaN(num) || double.IsInfinity(num))
                    return;
                Ani.Begin((DependencyObject)this.progressTrans, "ScaleX", num, 0.4, 4.0);
            }
        }

        public void ShowProgress()
        {
            if (this.progressShown)
                return;
            this.progressGrid.Opacity = 0.0;
            this.progressTrans.ScaleX = 0.0;
            this.progressGrid.Visibility = Visibility.Visible;
            Ani.Begin((DependencyObject)this.progressGrid, "Opacity", 1.0, 0.2);
            this.progressShown = true;
        }

               
        public async Task HideProgress()
        {
            TaskCompletionSource<object> _progressCompletionSource = new TaskCompletionSource<object>();
            if (!this.progressShown)
                return;
            this.progressShown = false;
            Storyboard storyboard = Ani.Begin((DependencyObject)this.progressGrid, "Opacity", 0.0, 0.2);
            ((Timeline)storyboard).Completed += (sender, args) =>
            {
                this.progressGrid.Visibility = Visibility.Collapsed;
                this.progressTrans.ScaleX = 0.0;
                _progressCompletionSource.SetResult(null);
            };
            await _progressCompletionSource.Task;
        }

        private void resetLikeButtons()
        {
            try
            {
                ContentControl likeButton1 = this.likeButton;
                Brush themeResource;
                this.dislikeButton.Background = 
                    themeResource = (Brush)App.Instance.GetThemeResource("AccentBrush");
                Brush brush1 = themeResource;
                likeButton1.Background = brush1;
                ContentControl likeButton2 = this.likeButton;
                Brush brush2;
                this.dislikeButton.Foreground = brush2 = (Brush)new SolidColorBrush(Colors.White);
                Brush brush3 = brush2;
                likeButton2.Foreground = brush3;
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
                this.subscribeBorder.Visibility = Visibility.Visible;
                if (YouTube.IsSubscribedTo(this.Entry.Author))
                {
                    //TODO
                    //this.subscribeIcon.Symbol = (Symbol)57608;
                }
                else
                {
                    //this.subscribeIcon.Symbol = (Symbol)57609;
                }
            }
            else
                this.subscribeBorder.Visibility = Visibility.Collapsed;
        }

        private void setLikeButtons(bool like)
        {
            if (like)
            {
                this.likeButton.Foreground = ((Brush)App.Instance.GetThemeResource("AccentBrush"));
                this.likeButton.Background = ((Brush)new SolidColorBrush(Colors.White));
                this.dislikeButton.Background = ((Brush)App.Instance.GetThemeResource("AccentBrush"));
                this.dislikeButton.Foreground = ((Brush)new SolidColorBrush(Colors.White));
            }
            else
            {
                this.dislikeButton.Foreground = ((Brush)App.Instance.GetThemeResource("AccentBrush"));
                this.dislikeButton.Background = ((Brush)new SolidColorBrush(Colors.White));
                this.likeButton.Background = ((Brush)App.Instance.GetThemeResource("AccentBrush"));
                this.likeButton.Foreground = ((Brush)new SolidColorBrush(Colors.White));
            }
        }

        private void VideoDetails_Loaded(object sender, RoutedEventArgs e)
        {
            this.CheckForComments();
            if (this.overCanvas == null)
            {
                this.overCanvas = Helper.FindParent<OverCanvas>((FrameworkElement)this, 100);
                this.overCanvas.ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
            }
            this.setSubscribeButton();
            YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
            YouTube.SignedOut += new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);

            //TODO
            //YouTube.SubscriptionsLoaded += new EventHandler(this.YouTube_SubscriptionsLoaded);
        }

        private async Task YouTube_SubscriptionsLoaded(object sender, EventArgs e)
        {
            TaskCompletionSource<object> _subscriptionsLoadedCompletionSource
              = new TaskCompletionSource<object>();

              await((DependencyObject)this).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.YouTube_SubscriptionsLoaded(null, EventArgs.Empty);
                _subscriptionsLoadedCompletionSource.SetResult(null);
            });
            await _subscriptionsLoadedCompletionSource.Task;
        }



        private void YouTube_SignedOut(object sender, EventArgs e)
        {
            //((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
            //    new DispatchedHandler((object)this, __methodptr(\u003CYouTube_SignedOut\u003Eb__28_0)));
        }

        private void YouTube_SignedIn(object sender, EventArgs e)
        {
            //((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
            //    new DispatchedHandler((object)this, __methodptr(\u003CYouTube_SignedIn\u003Eb__29_0)));
        }

        private void VideoDetails_Unloaded(object sender, RoutedEventArgs e)
        {
            YouTube.SignedIn -= new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
            YouTube.SignedOut -= new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
            //YouTube.SubscriptionsLoaded -= new EventHandler(this.YouTube_SubscriptionsLoaded);
        }

        private async void overCanvas_ShownChanged(object sender, bool e)
        {
            if (!e || !(this.DataContext is YouTubeEntry))
                return;

            this.bookmarkText.DataContext = (object)App.GlobalObjects.TimeBookmarksManager.Get
                ((this.DataContext as YouTubeEntry).ID);
        }

        private async void CheckForComments()
        {
        }

        private void author_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (((FrameworkElement)this).DataContext == null 
                || !(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            (Application.Current as App).RootFrame.Navigate(typeof(ChannelPage),
                (object)(((FrameworkElement)this).DataContext as YouTubeEntry).Author);
        }

        private async void Border_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private async void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(sender is FrameworkElement) || !(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            YouTubeEntry ent = ((FrameworkElement)this).DataContext as YouTubeEntry;
            FrameworkElement fe = sender as FrameworkElement;
            ((UIElement)fe).IsHitTestVisible = false;
            Ani.Begin((DependencyObject)fe, "Opacity", 0.5, 0.2);
            bool like = fe != this.dislikeButton;
            try
            {
                bool likeStatus = false;
                bool hasLikeStatus = this.likeStatuses.ContainsKey(ent.ID);
                likeStatus = hasLikeStatus && this.likeStatuses[ent.ID];
                if (!hasLikeStatus || likeStatus != like)
                {
                    int num = await YouTube.Rate((((FrameworkElement)this).DataContext as YouTubeEntry).ID, new bool?(like)) ? 1 : 0;
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
                messageDialog.Commands.Add((IUICommand)new UICommand("okay :("));
                messageDialog.ShowAsync();
            }
            Ani.Begin((DependencyObject)fe, "Opacity", 1.0, 0.2);
            ((UIElement)fe).IsHitTestVisible = true;
            ent = (YouTubeEntry)null;
            fe = (FrameworkElement)null;
        }

        private async void saveButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry ent))
                return;
            DownloaderPanel downloaderPanel = new DownloaderPanel();
            ((FrameworkElement)downloaderPanel).DataContext = (object)ent;
            ((Control)downloaderPanel).VerticalContentAlignment = (VerticalAlignment)2;
            ((FrameworkElement)downloaderPanel).RequestedTheme = Settings.Theme;
            DownloaderPanel panel = downloaderPanel;
            panel.VideoProgressCallbacks.Add(new Action<DownloadOperation>(this.ProgressCallback));
            Popup popup1 = new Popup();
            popup1.Child = panel;
            Popup popup2 = popup1;
            DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
            {
                Rect bounds1 = ((FrameworkElement)this).GetBounds(Window.Current.Content);
                Rect bounds2 = ((FrameworkElement)this.saveButton).GetBounds(Window.Current.Content);
                ((FrameworkElement)panel).Width = (bounds1.Width);
                ((FrameworkElement)panel).Height = (360.0);
                return new Point()
                {
                    X = bounds1.X,
                    Y = Math.Max(bounds2.Bottom - ((FrameworkElement)panel).Height, 0.0)
                };
            }));
            int num = await DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, 80.0)) ? 1 : 0;
            if (await App.GlobalObjects.TransferManager.GetTransferState(ent) != TransferManager.State.None)
            {
                //this.saveButton.Text = App.Strings["common.manage", "manage"].ToLower();
                //this.saveButton.Symbol = (Symbol)57605;
            }
            else
            {
                //this.saveButton.Text = App.Strings["common.save", "save"].ToLower();
                //this.saveButton.Symbol = (Symbol)57624;
            }
        }

        private async void addToButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!(this.DataContext is YouTubeEntry dataContext))
                return;
            //TODO
           
            PlaylistSelectorControl playlistSelectorControl = new PlaylistSelectorControl();
            playlistSelectorControl.DataContext = (object)dataContext;
            playlistSelectorControl.Margin = new Thickness(0.0, 19.0, 0.0, 0.0);
            PlaylistSelectorControl control = playlistSelectorControl;
            control.RequestedTheme = Settings.Theme;
            //TODO
            //control.SelectionMade += (EventHandler)((_param1, _param2) => DefaultPage.Current.ClosePopup());
            Popup popup1 = new Popup();
            popup1.Child = control;
            Popup popup2 = popup1;
            DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
            {
                Rect bounds1 = this.GetBounds(Window.Current.Content);
                Rect bounds2 = this.addToButton.GetBounds(Window.Current.Content);
                control.Width = bounds1.Width;
                control.Height = bounds2.Bottom;
                if (control.Height < 380.0)
                    control.Height = Math.Min(500.0, App.VisibleBounds.Height);
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
            this.subscribeBorder.IsHitTestVisible = false;
            Ani.Begin((DependencyObject)this.subscribeBorder, "Opacity", 0.5, 0.2);
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
            this.subscribeBorder.IsHitTestVisible = true;
            Ani.Begin((DependencyObject)this.subscribeBorder, "Opacity", 1.0, 0.2);
        }
      
    }
}
