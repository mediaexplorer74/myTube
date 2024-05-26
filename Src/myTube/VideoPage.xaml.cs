// myTube.VideoPage

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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Networking.BackgroundTransfer;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Markup;
using RykenTube;


namespace myTube
{
    public sealed partial class VideoPage : Page//, IDownloadPanelCallbackReceiver
    {
        private VisualState Default;

        private ObservableCollection<Comment> comments;
        private DataTransferManager dataTM;
        private bool textBoxFocused;
        private Dictionary<string, YouTubeEntry> loadedVideos = new Dictionary<string, YouTubeEntry>();
        private string lastID = "";
        private string vsName = nameof(Default);

        /*
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private Page loadingCommentsString;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private IconButtonInfo saveButtonInfo;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private IconButtonInfo cancelButtonInfo;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private IconButtonInfo deletelButtonInfo;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private IconButtonInfo manageButtonInfo;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private ScrollViewer scrollViewer;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private VisualState Default;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private VisualState Large;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private VisualState SmallTablet;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private OverCanvas overCanvas;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private ScrollViewer detailsScroll;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private CommentsList commentsList1;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private VideoList related;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private VideoDetails details;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private CommandBar appBar;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private AppBarButton saveButton;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private AppBarButton addToButton;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private AppBarButton shareButton;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private AppBarButton settingsButton;
        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        private AppBarButton homeButton;
        */

        private YouTubeEntry Entry => ((FrameworkElement)this).DataContext as YouTubeEntry;

        public VideoPage()
        {
            this.InitializeComponent();
            ((FrameworkElement)this).Tag = (object)nameof(VideoPage);

            //WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(
            //    new Func<SizeChangedEventHandler, EventRegistrationToken>(
            //        ((FrameworkElement)this).add_SizeChanged), 
            //    new Action<EventRegistrationToken>(((FrameworkElement)this).remove_SizeChanged),
            //    new SizeChangedEventHandler(this.OverCanvasTestPage_SizeChanged));
            ((FrameworkElement)this).SizeChanged += this.OverCanvasTestPage_SizeChanged;

            this.NavigationCacheMode = (NavigationCacheMode)2;

          
            ((FrameworkElement)this).DataContextChanged += this.OverCanvasTestPage_DataContextChanged;

            this.BottomAppBar.ClosedDisplayMode = (AppBarClosedDisplayMode)1;
            this.dataTM = DataTransferManager.GetForCurrentView();
            DataTransferManager dataTm = this.dataTM;

            dataTm.DataRequested += this.dataTM_DataRequested;

            OverCanvas.SetScrolledToOrAdjacent((DependencyObject)this.related, (Action)(() =>
            {
                Helper.Write((object)nameof(VideoPage), (object)"Scrolled to related");
                if (this.Entry == null || this.related.Client != null)
                    return;
                VideoList related = this.related;
                related.Client = (VideoListClient)new RelatedClient(this.Entry.ID, 30)
                {
                    APIKey = App.GetAPIKey(1)
                };
            }));
            ((UIElement)this).UseLayoutRounding = false;

            WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(
                new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement)this).add_Loaded),
                new Action<EventRegistrationToken>(((FrameworkElement)this).remove_Loaded),
                new RoutedEventHandler(this.VideoPage_Loaded));

            WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(
                new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement)this).add_Unloaded),
                new Action<EventRegistrationToken>(((FrameworkElement)this).remove_Unloaded),
                new RoutedEventHandler(this.VideoPage_Unloaded));
        }

        private void VideoPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (App.GlobalObjects.TransferManager == null)
                return;
            App.GlobalObjects.TransferManager.OnAction -= new EventHandler<TransferManagerActionEventArgs>(
                this.TransferManager_OnAction);
        }

        private void VideoPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.GlobalObjects.TransferManager == null)
                return;
            App.GlobalObjects.TransferManager.OnAction += new EventHandler<TransferManagerActionEventArgs>(
                this.TransferManager_OnAction);
        }

        private void TransferManager_OnAction(object sender, TransferManagerActionEventArgs e)
        {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            var cDisplayClass90 = new VideoPage.DisplayClass9_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass90.u003E4 = this;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass90.e = e;
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass90.e.Action != TransferManagerAction.Removed)
                return;

            // ISSUE: method pointer
            ((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
                new DispatchedHandler((object)cDisplayClass90,
                __methodptr(\u003CTransferManager_OnAction\u003Eb__0)));
        }

        private void dataTM_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            YouTubeEntry dataContext = ((FrameworkElement)this).DataContext as YouTubeEntry;
            DataPackage data = args.Request.Data;
            data.Properties.Title = dataContext.Title + " #myTube";
            data.Properties.Description = dataContext.Description + "\n#myTube";
            data.SetWebLink(new Uri("http://www.youtube.com/watch?v=" + dataContext.ID));
        }

        private async void OverCanvasTestPage_DataContextChanged(
          FrameworkElement sender,
          DataContextChangedEventArgs args)
        {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            VideoPage.\u003C\u003Ec__DisplayClass12_1 cDisplayClass121 = new VideoPage.\u003C\u003Ec__DisplayClass12_1();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass121.\u003C\u003E4__this = this;
            Helper.Write(((FrameworkElement)this).Tag, (object)"DataContext changed");
            // ISSUE: reference to a compiler-generated field
            cDisplayClass121.ent = args.NewValue as YouTubeEntry;
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass121.ent == null)
                return;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            VideoPage.\u003C\u003Ec__DisplayClass12_0 cDisplayClass120 = new VideoPage.\u003C\u003Ec__DisplayClass12_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass120.CS\u0024\u003C\u003E8__locals1 = cDisplayClass121;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass120.CS\u0024\u003C\u003E8__locals1.ent.ID == this.lastID)
      {
                Helper.Write(((FrameworkElement)this).Tag, (object)"Canceling DataContext changed", 1);
            }
      else
            {
                this.related.Client = (VideoListClient)null;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.lastID = cDisplayClass120.CS\u0024\u003C\u003E8__locals1.ent.ID;
                int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
                // ISSUE: reference to a compiler-generated field
                cDisplayClass120.globalObjects = App.GlobalObjects;
                // ISSUE: method pointer
                ThreadPool.RunAsync(new WorkItemHandler((object)cDisplayClass120, __methodptr(\u003COverCanvasTestPage_DataContextChanged\u003Eb__0)));
                this.overCanvas.ScrollToPage(0, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.LoadComments(cDisplayClass120.CS\u0024\u003C\u003E8__locals1.ent);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.overCanvas.Title = cDisplayClass120.CS\u0024\u003C\u003E8__locals1.ent.Title;
                this.detailsScroll.ChangeView(new double?(0.0), new double?(0.0), new float?(), true);
                cDisplayClass120 = (VideoPage.\u003C\u003Ec__DisplayClass12_0) null;
            }
        }

        private async void LoadComments(YouTubeEntry ent)
        {
            CommentClient commentClient = new CommentClient(ent.ID, 30);
            commentClient.Order = Settings.CommentsOrder;
            if (SharedSettings.CurrentAccount != null)
                commentClient.UseAccessToken = SharedSettings.CurrentAccount.Scope.Contains("https://www.googleapis.com/auth/youtube.force-ssl");
            this.commentsList1.Client = commentClient;
        }

        protected virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            DefaultPage.Current.ResetVideoPlayer();
            if (e.NavigationMode == null && e.SourcePageType == typeof(VideoPage))
                this.overCanvas.ScrollToPage(0, true);
            base.OnNavigatingFrom(e);
        }

        protected virtual async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.details.Thumb.ClientConstructor = (TypeConstructor)null;
            this.details.Thumb.PlayAutomaticallyOnOpen = false;
            TileArgs launchTileArgs = App.GetLaunchTileArgs((object)this);
            if (e.NavigationMode == 1)
            {
                this.details.Thumb.PlayAutomatically = false;
                this.details.Thumb.PlayAutomaticallyOnOpen = false;
            }
            else
            {
                this.details.Thumb.PlayAutomaticallyOnOpen = launchTileArgs != null && launchTileArgs.Play;
                this.detailsScroll.ChangeView(new double?(0.0), new double?(0.0), new float?());
                this.details.Thumb.PlayAutomatically = true;
            }
            if (e.NavigationMode != 1)
                App.CheckMessages(60.0);
            if (e.Parameter != null)
            {
                if (e.Parameter is YouTubeEntry)
                {
                    if ((e.Parameter as YouTubeEntry).NeedsRefresh)
                    {
                        try
                        {
                            ((FrameworkElement)this).put_DataContext((object)null);
                            if (this.loadedVideos.ContainsKey((e.Parameter as YouTubeEntry).ID))
                            {
                                ((FrameworkElement)this).put_DataContext((object)this.loadedVideos[(e.Parameter as YouTubeEntry).ID]);
                            }
                            else
                            {
                                YouTubeEntry info = await new YouTubeEntryClient().GetInfo((e.Parameter as YouTubeEntry).ID);
                                this.loadedVideos.Add(info.ID, info);
                                ((FrameworkElement)this).put_DataContext((object)info);
                            }
                        }
                        catch
                        {
                            ((FrameworkElement)this).put_DataContext(e.Parameter);
                        }
                    }
                    else
                        ((FrameworkElement)this).put_DataContext(e.Parameter);
                }
                else if (e.Parameter is VideoPage.ClientConstructorAndEntry)
                {
                    VideoPage.ClientConstructorAndEntry c = e.Parameter as VideoPage.ClientConstructorAndEntry;
                    this.details.Thumb.ClientConstructor = c.ClientConstructor;
                    if (c.Entry.NeedsRefresh)
                    {
                        try
                        {
                            ((FrameworkElement)this).put_DataContext((object)await new YouTubeEntryClient().GetInfo(c.Entry.ID));
                        }
                        catch
                        {
                            ((FrameworkElement)this).put_DataContext((object)c.Entry);
                        }
                    }
                    else
                        ((FrameworkElement)this).put_DataContext((object)c.Entry);
                    c = (VideoPage.ClientConstructorAndEntry)null;
                }
                else if (e.Parameter is string)
                {
                    try
                    {
                        if (this.loadedVideos.ContainsKey(e.Parameter as string))
                        {
                            ((FrameworkElement)this).put_DataContext((object)this.loadedVideos[e.Parameter as string]);
                        }
                        else
                        {
                            YouTubeEntry info = await new YouTubeEntryClient().GetInfo(e.Parameter as string);
                            this.loadedVideos.Add(info.ID, info);
                            ((FrameworkElement)this).put_DataContext((object)info);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            while (!this.detailsScroll.ChangeView(new double?(0.0), new double?(), new float?(), true))
                await Task.Delay(100);
            await Task.Delay(1000);
            int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
            try
            {
                await App.GlobalObjects.History.Update();
            }
            catch
            {
            }
        }

        private void OverCanvasTestPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            string str = e.NewSize.Width <= 1470.0 ? (e.NewSize.Height >= 700.0 ? "Default" : "SmallTablet") : "Large";
            if (!(str != this.vsName))
                return;
            Helper.Write((object)nameof(VideoPage), (object)("Changing VisualState to " + str + " with size " + (object)e.NewSize));
            VisualStateManager.GoToState((Control)this, str, true);
            this.vsName = str;
        }

        private void overCanvas_ShownChanged(object sender, bool e)
        {
            if (e)
                ((UIElement)this.appBar).put_Visibility((Visibility)0);
            else
                ((UIElement)this.appBar).put_Visibility((Visibility)1);
        }

        private void addToButton_Click(object sender, RoutedEventArgs e)
        {
            if (YouTube.IsSignedIn)
            {
                PlaylistSelectorControl playlistSelectorControl = new PlaylistSelectorControl();
                ((FrameworkElement)playlistSelectorControl).put_DataContext(((FrameworkElement)this).DataContext);
                PlaylistSelectorControl psc = playlistSelectorControl;
                Popup popup1 = new Popup();
                popup1.put_Child((UIElement)psc);
                ((FrameworkElement)popup1).put_RequestedTheme(App.Theme);
                Popup popup2 = popup1;
                psc.SelectionMade += (EventHandler)((_param1, _param2) => DefaultPage.Current.ClosePopup());
                DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
                {
                    Rect visibleBounds = App.VisibleBounds;
                    ((FrameworkElement)psc).put_Width(Math.Min(400.0, visibleBounds.Width - 38.0));
                    ((FrameworkElement)psc).put_Height(Math.Min(700.0, visibleBounds.Height - 95.0));
                    Point point = new Point(visibleBounds.Width / 2.0, visibleBounds.Height);
                    double num = 0.0;
                    return new Point(Math.Min(point.X - ((FrameworkElement)psc).Width / 2.0, visibleBounds.Width - ((FrameworkElement)psc).Width - 19.0), visibleBounds.Height - ((FrameworkElement)psc).Height - num);
                }));
                DefaultPage.Current.ShowPopup(popup2, new Point(), DefaultPage.DefaultPopupTransitionOffset);
            }
            else
                DefaultPage.Current.OpenBrowser();
        }

        private void shareButton_Click(object sender, RoutedEventArgs e) => DataTransferManager.ShowShareUI();

        private void settingsButton_Click(object sender, RoutedEventArgs e) => ((App)Application.Current).OpenSettings();

        private async void commentBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void ProgressCallback(DownloadOperation download)
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            YouTubeEntry dataContext = ((FrameworkElement)this).DataContext as YouTubeEntry;
            TransferInfo transferInfo = App.GlobalObjects.TransferManager.GetTransferInfo(download);
            if (transferInfo == null || !(transferInfo.ID == dataContext.ID))
                return;
            if ((long)download.Progress.BytesReceived == (long)download.Progress.TotalBytesToReceive)
                ((FrameworkElement)this.saveButton).put_DataContext((object)this.deletelButtonInfo);
            this.details.ProgressCallback(download);
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            int transferState = (int)await App.GlobalObjects.TransferManager.GetTransferState(((FrameworkElement)this).DataContext as YouTubeEntry);
            DownloaderPanel downloaderPanel1 = new DownloaderPanel();
            ((Control)downloaderPanel1).put_VerticalContentAlignment((VerticalAlignment)2);
            DownloaderPanel downloaderPanel2 = downloaderPanel1;
            downloaderPanel2.VideoProgressCallbacks.Add(new Action<DownloadOperation>(this.ProgressCallback));
            downloaderPanel2.CallbackReceivers.Add((IDownloadPanelCallbackReceiver)this);
            Rect bounds = Window.Current.Bounds;
            ((FrameworkElement)downloaderPanel2).put_Width(Math.Min(500.0, bounds.Width));
            ((FrameworkElement)downloaderPanel2).put_Height(Math.Min(500.0, bounds.Height));
            double num = 24.0;
            Point position = new Point();
            position.Y = bounds.Height - ((FrameworkElement)downloaderPanel2).Height - num;
            position.X = (bounds.Width - ((FrameworkElement)downloaderPanel2).Width) / 2.0;
            Popup popup = new Popup();
            popup.put_Child((UIElement)downloaderPanel2);
            ((FrameworkElement)popup).put_RequestedTheme(App.Theme);
            ((FrameworkElement)popup).put_DataContext(((FrameworkElement)this).DataContext);
            DefaultPage.Current.ShowPopup(popup, position, new Point(0.0, 150.0));
            ((Control)this.saveButton).put_IsEnabled(true);
        }

        private async void SetDownloadButton()
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry dataContext))
                return;
            switch (await App.GlobalObjects.TransferManager.GetTransferState(dataContext))
            {
                case TransferManager.State.None:
                    this.details.HideProgress();
                    ((FrameworkElement)this.saveButton).put_DataContext((object)this.saveButtonInfo);
                    break;
                case TransferManager.State.Downloading:
                    ((FrameworkElement)this.saveButton).put_DataContext((object)this.manageButtonInfo);
                    break;
                case TransferManager.State.Complete:
                    ((FrameworkElement)this.saveButton).put_DataContext((object)this.manageButtonInfo);
                    this.details.HideProgress();
                    break;
            }
        }

        public async void DownloadPanelCallback(DownloaderPanel downloaderPanel)
        {
            await Task.Delay(200);
            this.SetDownloadButton();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultPage.Current.Frame.ClearBackStackAtNavigate();
            DefaultPage.Current.Frame.Navigate(typeof(HomePage));
        }

        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("ms-appx:///VideoPage.xaml"), (ComponentResourceLocation)0);
            this.loadingCommentsString = (Page)((FrameworkElement)this).FindName("loadingCommentsString");
            this.saveButtonInfo = (IconButtonInfo)((FrameworkElement)this).FindName("saveButtonInfo");
            this.cancelButtonInfo = (IconButtonInfo)((FrameworkElement)this).FindName("cancelButtonInfo");
            this.deletelButtonInfo = (IconButtonInfo)((FrameworkElement)this).FindName("deletelButtonInfo");
            this.manageButtonInfo = (IconButtonInfo)((FrameworkElement)this).FindName("manageButtonInfo");
            this.scrollViewer = (ScrollViewer)((FrameworkElement)this).FindName("scrollViewer");
            this.Default = (VisualState)((FrameworkElement)this).FindName("Default");
            this.Large = (VisualState)((FrameworkElement)this).FindName("Large");
            this.SmallTablet = (VisualState)((FrameworkElement)this).FindName("SmallTablet");
            this.overCanvas = (OverCanvas)((FrameworkElement)this).FindName("overCanvas");
            this.detailsScroll = (ScrollViewer)((FrameworkElement)this).FindName("detailsScroll");
            this.commentsList1 = (CommentsList)((FrameworkElement)this).FindName("commentsList1");
            this.related = (VideoList)((FrameworkElement)this).FindName("related");
            this.details = (VideoDetails)((FrameworkElement)this).FindName("details");
            this.appBar = (CommandBar)((FrameworkElement)this).FindName("appBar");
            this.saveButton = (AppBarButton)((FrameworkElement)this).FindName("saveButton");
            this.addToButton = (AppBarButton)((FrameworkElement)this).FindName("addToButton");
            this.shareButton = (AppBarButton)((FrameworkElement)this).FindName("shareButton");
            this.settingsButton = (AppBarButton)((FrameworkElement)this).FindName("settingsButton");
            this.homeButton = (AppBarButton)((FrameworkElement)this).FindName("homeButton");
        }

        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [DebuggerNonUserCode]
        public void Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((OverCanvas)target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
                    break;
                case 2:
                    ButtonBase buttonBase1 = (ButtonBase)target;
                    WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.saveButton_Click));
                    break;
                case 3:
                    ButtonBase buttonBase2 = (ButtonBase)target;
                    WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.addToButton_Click));
                    break;
                case 4:
                    ButtonBase buttonBase3 = (ButtonBase)target;
                    WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase3.add_Click), new Action<EventRegistrationToken>(buttonBase3.remove_Click), new RoutedEventHandler(this.shareButton_Click));
                    break;
                case 5:
                    ButtonBase buttonBase4 = (ButtonBase)target;
                    WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase4.add_Click), new Action<EventRegistrationToken>(buttonBase4.remove_Click), new RoutedEventHandler(this.settingsButton_Click));
                    break;
                case 6:
                    ButtonBase buttonBase5 = (ButtonBase)target;
                    WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase5.add_Click), new Action<EventRegistrationToken>(buttonBase5.remove_Click), new RoutedEventHandler(this.homeButton_Click));
                    break;
            }
            this._contentLoaded = true;
        }

        public class ClientConstructorAndEntry
        {
            public YouTubeEntry Entry;
            public TypeConstructor ClientConstructor;
        }

        private class DisplayClass9_0
        {
            internal VideoPage u003E4;
            internal TransferManagerActionEventArgs e;

            public DisplayClass9_0()
            {
            }
        }
    }
}
