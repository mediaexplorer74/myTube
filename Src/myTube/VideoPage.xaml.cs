﻿// myTube.VideoPage

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
using myTube.GlobalAppObjects;


namespace myTube
{
    public sealed partial class VideoPage : Page//, IDownloadPanelCallbackReceiver
    {
        private ObservableCollection<Comment> comments;
        private DataTransferManager dataTM;
        private bool textBoxFocused;
        private Dictionary<string, YouTubeEntry> loadedVideos = new Dictionary<string, YouTubeEntry>();
        private string lastID = "";
       

        
        private VisualState Default;
        
        private VisualState Large;
       
        private VisualState SmallTablet;
       
        private OverCanvas overCanvas;
       
        private ScrollViewer detailsScroll;

        //private CommentsList commentsList1;
        private IconButtonInfo saveButtonInfo;

        private IconButtonInfo cancelButtonInfo;

        private IconButtonInfo deletelButtonInfo;

        private IconButtonInfo shareButtonInfo;

        private IconButtonInfo manageButtonInfo;

        private VideoList related;
      
        //private VideoDetails details;
       
          /* 
        private AppBarButton saveButton;
      
        private AppBarButton addToButton;
       
        private AppBarButton shareButton;
       
        private AppBarButton settingsButton;
      
        private AppBarButton homeButton;
          */


        private string vsName = nameof(Default);

        private YouTubeEntry Entry
        {
            get
            {
                return ((FrameworkElement)this).DataContext as YouTubeEntry;
            }
        }

        public VideoPage()
        {
            this.InitializeComponent();
            ((FrameworkElement)this).Tag = (object)nameof(VideoPage);

           
            ((FrameworkElement)this).SizeChanged += this.OverCanvasTestPage_SizeChanged;

            this.NavigationCacheMode = (NavigationCacheMode)2;

          
            this.DataContextChanged += this.OverCanvasTestPage_DataContextChanged;

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

            // This assumes that VideoPage_Loaded and VideoPage_Unloaded are methods
            // in your code-behind that handle the Loaded and Unloaded events respectively.
            this.Loaded -= this.VideoPage_Loaded;
            this.Unloaded -= this.VideoPage_Unloaded;
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

            // TODO
            //((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
            //    new DispatchedHandler((object)cDisplayClass90,
            //    __methodptr(\u003CTransferManager_OnAction\u003Eb__0)));
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
            var cDisplayClass121 = new VideoPage.DisplayClass12_1();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass121.u003E4 = this;
            Helper.Write(((FrameworkElement)this).Tag, (object)"DataContext changed");
            // ISSUE: reference to a compiler-generated field
            cDisplayClass121.ent = args.NewValue as YouTubeEntry;
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass121.ent == null)
                return;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            var cDisplayClass120 = new VideoPage.DisplayClass12_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass120.u003E8_locals1 = cDisplayClass121;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (cDisplayClass120.u003E8_locals1.ent.ID == this.lastID)
      {
                Helper.Write(((FrameworkElement)this).Tag, (object)"Canceling DataContext changed", 1);
            }
      else
            {
                this.related.Client = (VideoListClient)null;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.lastID = cDisplayClass120.u003E8_locals1.ent.ID;
                int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
                // ISSUE: reference to a compiler-generated field
                cDisplayClass120.globalObjects = App.GlobalObjects;
                
                // TODO
                //ThreadPool.RunAsync(new WorkItemHandler((object)cDisplayClass120, 
                //    __methodptr(\u003COverCanvasTestPage_DataContextChanged\u003Eb__0)));
                
                this.overCanvas.ScrollToPage(0, true);
                
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.LoadComments(cDisplayClass120.u003E8_locals1.ent);

                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.overCanvas.Title = cDisplayClass120.u003E8_locals1.ent.Title;
                this.detailsScroll.ChangeView(new double?(0.0), new double?(0.0), new float?(), true);
                cDisplayClass120 = (VideoPage.DisplayClass12_0) null;
            }
        }

        private async void LoadComments(YouTubeEntry ent)
        {
            CommentClient commentClient = new CommentClient(ent.ID, 30);
            commentClient.Order = Settings.CommentsOrder;
            if (SharedSettings.CurrentAccount != null)
                commentClient.UseAccessToken = 
                    SharedSettings.CurrentAccount.Scope.Contains(
                        "https://www.googleapis.com/auth/youtube.force-ssl");
            //TODO
            //this.commentsList1.Client = commentClient;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //TODO
            DefaultPage.Current.ResetVideoPlayer();

            if (e.NavigationMode == null && e.SourcePageType == typeof(VideoPage))
                this.overCanvas.ScrollToPage(0, true);
            base.OnNavigatingFrom(e);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            //TODO
            //this.details.Thumb.ClientConstructor = (TypeConstructor)null;
            //this.details.Thumb.PlayAutomaticallyOnOpen = false;
            
            TileArgs launchTileArgs = App.GetLaunchTileArgs((object)this);
            
            if (e.NavigationMode == NavigationMode.Back)
            {
                //this.details.Thumb.PlayAutomatically = false;
                //this.details.Thumb.PlayAutomaticallyOnOpen = false;
            }
            else
            {
                //this.details.Thumb.PlayAutomaticallyOnOpen = launchTileArgs != null && launchTileArgs.Play;
                //this.detailsScroll.ChangeView(new double?(0.0), new double?(0.0), new float?());
                //this.details.Thumb.PlayAutomatically = true;
            }
            if (e.NavigationMode != NavigationMode.Back)
                App.CheckMessages(60.0);
            if (e.Parameter != null)
            {
                if (e.Parameter is YouTubeEntry)
                {
                    if ((e.Parameter as YouTubeEntry).NeedsRefresh)
                    {
                        try
                        {
                            ((FrameworkElement)this).DataContext = (object)null;
                            if (this.loadedVideos.ContainsKey((e.Parameter as YouTubeEntry).ID))
                            {
                                ((FrameworkElement)this).DataContext
                                    =(object)this.loadedVideos[(e.Parameter as YouTubeEntry).ID];
                            }
                            else
                            {
                                YouTubeEntry info = await new YouTubeEntryClient()
                                    .GetInfo((e.Parameter as YouTubeEntry).ID);

                                this.loadedVideos.Add(info.ID, info);
                                ((FrameworkElement)this).DataContext = (object)info;
                            }
                        }
                        catch
                        {
                            ((FrameworkElement)this).DataContext = e.Parameter;
                        }
                    }
                    else
                        ((FrameworkElement)this).DataContext = e.Parameter;
                }
                else if (e.Parameter is VideoPage.ClientConstructorAndEntry)
                {
                    VideoPage.ClientConstructorAndEntry c = e.Parameter as VideoPage.ClientConstructorAndEntry;
                    //this.details.Thumb.ClientConstructor = c.ClientConstructor;
                    if (c.Entry.NeedsRefresh)
                    {
                        try
                        {
                            ((FrameworkElement)this).DataContext = 
                                (object)await new YouTubeEntryClient().GetInfo(c.Entry.ID);
                        }
                        catch
                        {
                            ((FrameworkElement)this).DataContext = (object)c.Entry;
                        }
                    }
                    else
                        ((FrameworkElement)this).DataContext = (object)c.Entry;
                    c = (VideoPage.ClientConstructorAndEntry)null;
                }
                else if (e.Parameter is string)
                {
                    try
                    {
                        if (this.loadedVideos.ContainsKey(e.Parameter as string))
                        {
                            ((FrameworkElement)this).DataContext = (object)this.loadedVideos[e.Parameter as string];
                        }
                        else
                        {
                            YouTubeEntry info = await new YouTubeEntryClient().GetInfo(e.Parameter as string);
                            this.loadedVideos.Add(info.ID, info);
                            ((FrameworkElement)this).DataContext = (object)info;
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
            string str = e.NewSize.Width <= 1470.0 ? (e.NewSize.Height >= 700.0
                ? "Default" : "SmallTablet") : "Large";
            if (!(str != this.vsName))
                return;

            Helper.Write((object)nameof(VideoPage), (object)(
                "Changing VisualState to " + str + " with size " + (object)e.NewSize));

            VisualStateManager.GoToState((Control)this, str, true);
            this.vsName = str;
        }

        private void overCanvas_ShownChanged(object sender, bool e)
        {
            if (e)
                ((UIElement)this.appBar).Visibility = Visibility.Visible;
            else
                ((UIElement)this.appBar).Visibility = Visibility.Collapsed;
        }

        private void addToButton_Click(object sender, RoutedEventArgs e)
        {
            if (YouTube.IsSignedIn)
            {
                /*
                PlaylistSelectorControl playlistSelectorControl = new PlaylistSelectorControl();
                ((FrameworkElement)playlistSelectorControl).DataContext = (((FrameworkElement)this).DataContext);
                PlaylistSelectorControl psc = playlistSelectorControl;
                Popup popup1 = new Popup();
                popup1.Child = ((UIElement)psc);
                ((FrameworkElement)popup1).RequestedTheme = (App.Theme);
                Popup popup2 = popup1;
                psc.SelectionMade += (EventHandler)((_param1, _param2) => DefaultPage.Current.ClosePopup());
               
                DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
                {
                    Rect visibleBounds = App.VisibleBounds;
                    ((FrameworkElement)psc).put_Width(Math.Min(400.0, visibleBounds.Width - 38.0));
                    ((FrameworkElement)psc).put_Height(Math.Min(700.0, visibleBounds.Height - 95.0));
                    Point point = new Point(visibleBounds.Width / 2.0, visibleBounds.Height);
                    double num = 0.0;
                    return new Point(Math.Min(point.X - ((FrameworkElement)psc).Width / 2.0, 
                     visibleBounds.Width - ((FrameworkElement)psc).Width - 19.0), 
                      visibleBounds.Height - ((FrameworkElement)psc).Height - num);
                }));
                DefaultPage.Current.ShowPopup(popup2, new Point(), DefaultPage.DefaultPopupTransitionOffset);
                */
            }
            else
            {
                //DefaultPage.Current.OpenBrowser();
            }
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
                ((FrameworkElement)this.saveButton).DataContext = (object)this.deletelButtonInfo;
            //TODO
            //this.details.ProgressCallback(download);
        }

        private async void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry))
                return;
            //TODO
            /*int transferState = (int)await App.GlobalObjects.TransferManager.GetTransferState(((FrameworkElement)this).DataContext as YouTubeEntry);
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
            ((Control)this.saveButton).put_IsEnabled(true);*/
        }

        private async void SetDownloadButton()
        {
            if (!(((FrameworkElement)this).DataContext is YouTubeEntry dataContext))
                return;
            switch (await App.GlobalObjects.TransferManager.GetTransferState(dataContext))
            {
                case TransferManager.State.None:
                    //this.details.HideProgress();
                    ((FrameworkElement)this.saveButton).DataContext = ((object)this.saveButtonInfo);
                    break;
                case TransferManager.State.Downloading:
                    ((FrameworkElement)this.saveButton).DataContext = ((object)this.manageButtonInfo);
                    break;
                case TransferManager.State.Complete:
                    ((FrameworkElement)this.saveButton).DataContext = ((object)this.manageButtonInfo);
                    //this.details.HideProgress();
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
            //TODO
            //DefaultPage.Current.Frame.ClearBackStackAtNavigate();
            //DefaultPage.Current.Frame.Navigate(typeof(HomePage));
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

        private class DisplayClass12_1
        {
            internal VideoPage u003E4;
            internal YouTubeEntry ent;

            public DisplayClass12_1()
            {
            }
        }

        private class DisplayClass12_0
        {
            internal DisplayClass12_1 u003E8_locals1;
            internal GlobalObjects globalObjects;

            public DisplayClass12_0()
            {
            }
        }
    }
}
