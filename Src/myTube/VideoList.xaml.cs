// myTube.VideoList

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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.AwaitableUI;
using myTube.Helpers;
using myTube.Tiles;
using RykenTube;
using Windows.Devices.Input;

namespace myTube
{
    public sealed partial class VideoList : UserControl
    {
        public DependencyProperty ContextMenuProperty 
            = DependencyProperty.Register(nameof(ContextMenu), typeof(IconButtonEventCollection), 
                typeof(VideoList), new PropertyMetadata((object)null));

        public static DependencyProperty ListPaddingProperty 
            = DependencyProperty.Register(nameof(ListPadding), typeof(Thickness), typeof(VideoList),
                new PropertyMetadata((object)new Thickness(0.0, 0.0, -19.0, 0.0)));

        public static DependencyProperty ThumbPaddingProperty 
            = DependencyProperty.Register(nameof(ThumbPadding), typeof(Thickness), typeof(VideoList),
                new PropertyMetadata((object)new Thickness(0.0, 0.0, 19.0, 19.0)));

        public static DependencyProperty EntriesProperty 
            = DependencyProperty.Register(nameof(Entries), typeof(ObservableCollection<YouTubeEntry>), 
                typeof(VideoList), new PropertyMetadata((object)null,
                    new PropertyChangedCallback(VideoList.OnEntriesChanged)));

        public static DependencyProperty ClientProperty 
            = DependencyProperty.Register(nameof(Client), typeof(VideoListClient), typeof(VideoList), 
                new PropertyMetadata((object)null, 
                    new PropertyChangedCallback(VideoList.OnClientChanged)));

        public static DependencyProperty LoadOnScrollProperty 
            = DependencyProperty.Register(nameof(LoadOnScroll), typeof(bool), typeof(VideoList), 
                new PropertyMetadata((object)true));

        public static DependencyProperty AutomaticallyLoadDataProperty 
            = DependencyProperty.Register(nameof(AutomaticallyLoadData), typeof(bool), typeof(VideoList), 
                new PropertyMetadata((object)true));

        public static DependencyProperty AllowPlaylistModeProperty 
            = DependencyProperty.Register(nameof(AllowPlaylistMode), typeof(bool), typeof(VideoList), 
                new PropertyMetadata((object)true));

        public static DependencyProperty LoadVideosFuncProperty 
            = DependencyProperty.Register(nameof(LoadVideosFunc), typeof(Func<int, Task<YouTubeEntry[]>>), 
                typeof(VideoList), new PropertyMetadata((object)null,
                    new PropertyChangedCallback(VideoList.OnLoadVideosFuncChanged)));

        public static DependencyProperty SortVideosFuncProperty 
            = DependencyProperty.Register(nameof(SortVideosFunc), typeof(Func<YouTubeEntry, YouTubeEntry, bool>), 
                typeof(VideoList), new PropertyMetadata((object)null,
                    new PropertyChangedCallback(VideoList.OnSortVideosFuncChanged)));

        public static DependencyProperty DependsOnSignInProperty
            = DependencyProperty.Register(nameof(DependsOnSignIn), typeof(bool), typeof(VideoList), 
                new PropertyMetadata((object)false, 
                    new PropertyChangedCallback(VideoList.DependsOnSignInChanged)));

        public static DependencyProperty IsSignedInProperty 
            = DependencyProperty.Register(nameof(IsSignedIn), typeof(bool), typeof(VideoList), 
                new PropertyMetadata((object)false, 
                    new PropertyChangedCallback(VideoList.IsSignedInChanged)));

        private int page;
        private bool loaded;
        private bool isSorting;
        private Dictionary<YouTubeEntry, int> sortingDictonary;
        private ISortableThumbnail<YouTubeEntry> currentSortingThumb;
        private YouTubeEntry currentSortingEntry;
        private TaskCompletionSource<bool> clearingTcs;
        private bool busy;
        private bool isSelecting;
        private Dictionary<YouTubeEntry, bool> selected;

        // TEMP
        private ListStrings listStrings;
        private ListView ItemList;
        private ScrollViewer scroll;
        private List<MenuFlyoutItem> contextMenuCollection;
        private MenuFlyoutItem playIcon;
        private MenuFlyoutItem deleteButton;
        private MenuFlyoutItem watchLaterIcon;
        private MenuFlyoutItem pinIcon;
        private MenuFlyoutItem saveIcon;

        private static void OnEntriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoList videoList = d as VideoList;
            ObservableCollection<YouTubeEntry> oldValue = e.OldValue as ObservableCollection<YouTubeEntry>;
            ObservableCollection<YouTubeEntry> newValue = e.NewValue as ObservableCollection<YouTubeEntry>;
            if (oldValue != null)
                oldValue.CollectionChanged -= new NotifyCollectionChangedEventHandler(
                    videoList.newEntries_CollectionChanged);
            if (newValue == null)
                return;
            newValue.CollectionChanged += new NotifyCollectionChangedEventHandler(
                videoList.newEntries_CollectionChanged);
        }

        private async void newEntries_CollectionChanged(
          object sender,
          NotifyCollectionChangedEventArgs e)
        {
            await Task.Delay(100);

            //if (((Collection<YouTubeEntry>)this.Entries).Count > 0)
            //    ((UIElement)this.loadingText).Visibility = (Visibility)1;
            //else
            //    ((UIElement)this.loadingText).Visibility = (Visibility)0;
        }

        private static void OnSortVideosFuncChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
        }

        private static void IsSignedInChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VideoList videoList))
                return;
            bool newValue = (bool)e.NewValue;
            if (!videoList.DependsOnSignIn)
                return;
            if (newValue)
            {
                videoList.Clear(false);
                if (!videoList.AutomaticallyLoadData)
                    return;
                videoList.Load();
            }
            else
            {
                videoList.Clear(true);
                videoList.listStrings.State = ListState.SignIn;
            }
        }

        private static void DependsOnSignInChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            if (!(d is VideoList videoList))
                return;
            bool newValue = (bool)e.NewValue;
            if (newValue && !videoList.IsSignedIn)
            {
                videoList.Clear(false);
                videoList.listStrings.State = ListState.SignIn;
            }
            else
            {
                if (newValue || videoList.IsSignedIn)
                    return;
                videoList.listStrings.State = ListState.Default;
            }
        }

        private static void OnLoadVideosFuncChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            VideoList videoList = d as VideoList;
            videoList.Clear(false);
            videoList.page = 0;
            if (!videoList.AutomaticallyLoadData || !videoList.AuthorizedToLoad)
                return;
            videoList.Load();
        }

        private static void OnClientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoList videoList = d as VideoList;
            videoList.Clear(false);
            videoList.page = 0;
            if (e.NewValue == null || !videoList.AutomaticallyLoadData || !videoList.AuthorizedToLoad)
                return;
            videoList.Load();
        }

        public ObservableCollection<YouTubeEntry> Entries
        {
            get => (ObservableCollection<YouTubeEntry>)((DependencyObject)this).GetValue(VideoList.EntriesProperty);
            set => ((DependencyObject)this).SetValue(VideoList.EntriesProperty, (object)value);
        }

        public event EventHandler<YouTubeEntry[]> VideosLoaded;

        public event EventHandler<Exception> LoadFailed;

        public Thickness ListPadding
        {
            get => (Thickness)((DependencyObject)this).GetValue(VideoList.ListPaddingProperty);
            set => ((DependencyObject)this).SetValue(VideoList.ListPaddingProperty, (object)value);
        }

        public Thickness ThumbPadding
        {
            get => (Thickness)((DependencyObject)this).GetValue(VideoList.ThumbPaddingProperty);
            set => ((DependencyObject)this).SetValue(VideoList.ThumbPaddingProperty, (object)value);
        }

        public VideoListClient Client
        {
            get => (VideoListClient)((DependencyObject)this).GetValue(VideoList.ClientProperty);
            set => ((DependencyObject)this).SetValue(VideoList.ClientProperty, (object)value);
        }

        public bool IsSorting => this.isSorting;

        public bool AllowPlaylistMode
        {
            get => (bool)((DependencyObject)this).GetValue(VideoList.AllowPlaylistModeProperty);
            set => ((DependencyObject)this).SetValue(VideoList.AllowPlaylistModeProperty, (object)value);
        }

        public ListStrings ListStrings
        {
            get
            {
                return this.listStrings;
            }
        }

        public event EventHandler<IconButtonEventCollection> SetContextMenu;

        public event EventHandler<IconButtonEventCollection> SetMultiselectContextMenu;

        public IconButtonEventCollection ContextMenu
        {
            get
            {
                return (IconButtonEventCollection)((DependencyObject)this).GetValue(this.ContextMenuProperty);
            }
        }

        public bool DependsOnSignIn
        {
            get => (bool)((DependencyObject)this).GetValue(VideoList.DependsOnSignInProperty);
            set => ((DependencyObject)this).SetValue(VideoList.DependsOnSignInProperty, (object)value);
        }

        public bool IsSignedIn
        {
            get => (bool)((DependencyObject)this).GetValue(VideoList.IsSignedInProperty);
            set => ((DependencyObject)this).SetValue(VideoList.IsSignedInProperty, (object)value);
        }

        public Func<int, Task<YouTubeEntry[]>> LoadVideosFunc
        {
            get => (Func<int, Task<YouTubeEntry[]>>)((DependencyObject)this).GetValue(VideoList.LoadVideosFuncProperty);
            set => ((DependencyObject)this).SetValue(VideoList.LoadVideosFuncProperty, (object)value);
        }

        public Func<YouTubeEntry, YouTubeEntry, bool> SortVideosFunc
        {
            get => (Func<YouTubeEntry, YouTubeEntry, bool>)((DependencyObject)this).GetValue(VideoList.SortVideosFuncProperty);
            set => ((DependencyObject)this).SetValue(VideoList.SortVideosFuncProperty, (object)value);
        }

        public bool AutomaticallyLoadData
        {
            get => (bool)((DependencyObject)this).GetValue(VideoList.AutomaticallyLoadDataProperty);
            set => ((DependencyObject)this).SetValue(VideoList.AutomaticallyLoadDataProperty, (object)value);
        }

        public bool LoadOnScroll
        {
            get => (bool)((DependencyObject)this).GetValue(VideoList.LoadOnScrollProperty);
            set => ((DependencyObject)this).SetValue(VideoList.LoadOnScrollProperty, (object)value);
        }

        public ThumbnailDispatcher ThumbnailDispatcher { get; private set; }

        public ScrollViewer ScrollViewer => this.scroll;

        public VideoList()
        {
            this.Entries = new ObservableCollection<YouTubeEntry>();
            this.InitializeComponent();

            // ISSUE: method pointer
            //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(
            //    new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(
            //        ((FrameworkElement)this).add_DataContextChanged), 
            //    new Action<EventRegistrationToken>(((FrameworkElement)this).remove_DataContextChanged), 
            //    new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object)this, 
            //    __methodptr(VideoList_DataContextChanged)));
            this.DataContextChanged += (sender, args) 
                => this.VideoList_DataContextChanged(sender as FrameworkElement, args as DataContextChangedEventArgs);

            ((FrameworkElement)this.ItemList).DataContext = (object)this;

            //WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, 
            //    EventRegistrationToken>(((FrameworkElement)this).add_SizeChanged), 
            //    new Action<EventRegistrationToken>(((FrameworkElement)this).remove_SizeChanged), 
            //    new SizeChangedEventHandler(this.VideoList_SizeChanged));
            this.SizeChanged += this.VideoList_SizeChanged;

            ScrollViewer scroll = this.scroll;

            //WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>(new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scroll.add_ViewChanged), new Action<EventRegistrationToken>(scroll.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.scroll_ViewChanged));
            //WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement)this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement)this).remove_Loaded), new RoutedEventHandler(this.VideoList_Loaded));
            //WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement)this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement)this).remove_Unloaded), new RoutedEventHandler(this.VideoList_Unloaded));
            scroll.ViewChanged += this.scroll_ViewChanged;
            this.Loaded += this.VideoList_Loaded;
            this.Unloaded += this.VideoList_Unloaded;

            this.ThumbnailDispatcher = new ThumbnailDispatcher();
        }

        private void VideoList_Unloaded(object sender, RoutedEventArgs e)
        {
            this.loaded = false;
            if (!this.isSorting)
                return;
            this.EndSort();
        }

        public bool AuthorizedToLoad => !this.DependsOnSignIn || this.IsSignedIn;

        private void VideoList_Loaded(object sender, RoutedEventArgs e)
        {
            this.loaded = true;
            ((DependencyObject)this).SetValue(this.ContextMenuProperty, (object)this.contextMenuCollection);
            this.playIcon = this.contextMenuCollection[0];
            this.deleteButton = this.contextMenuCollection[1];
            this.watchLaterIcon = this.contextMenuCollection[2];
            this.pinIcon = this.contextMenuCollection[3];
            this.saveIcon = this.contextMenuCollection[4];
        }

        public void BeginSort()
        {
            this.isSorting = !this.isSorting ? true : throw new InvalidOperationException(
                "Please complete the current sorting operation before beginning a new one");
            this.sortingDictonary = new Dictionary<YouTubeEntry, int>();
            if (this.ItemList.ItemsPanelRoot == null)
                return;
            Panel itemsPanelRoot;
            Panel panel = itemsPanelRoot = this.ItemList.ItemsPanelRoot;

            //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>,
            //    EventRegistrationToken>(((FrameworkElement)panel).add_LayoutUpdated), 
            //    new Action<EventRegistrationToken>(((FrameworkElement)panel).remove_LayoutUpdated), 
            //    new EventHandler<object>(this.panel_LayoutUpdated));
            panel.LayoutUpdated += (sender, args) => this.panel_LayoutUpdated(sender as FrameworkElement, args as object);

            foreach (UIElement child1 in (IEnumerable<UIElement>)itemsPanelRoot.Children)
            {
                foreach (FrameworkElement child2 
                    in Helper.FindChildren<FrameworkElement>((DependencyObject)(child1 as FrameworkElement), 10))
                {
                    if (child2 is ISortableThumbnail<YouTubeEntry>)
                    {
                        CompositeTransform compositeTransform = new CompositeTransform();
                        ((UIElement)child2).RenderTransformOrigin = (new Point(0.5, 0.5));
                        ((UIElement)child2).RenderTransform = ((Transform)compositeTransform);
                        this.setSortTrans((UIElement)child2, false);
                        break;
                    }
                }
            }
        }

        private void panel_LayoutUpdated(object sender, object e)
        {
            Panel itemsPanelRoot = this.ItemList.ItemsPanelRoot;
            if (itemsPanelRoot == null)
                return;
            foreach (UIElement child in (IEnumerable<UIElement>)itemsPanelRoot.Children)
            {
                FrameworkElement sortingElement = this.getSortingElement(child);
                if (sortingElement.DataContext != this.currentSortingEntry)
                {
                    this.setSortTrans((UIElement)sortingElement, false);
                    (sortingElement as ISortableThumbnail<YouTubeEntry>).EndSorting();
                }
                else
                {
                    this.setSortTrans((UIElement)sortingElement, true);
                    (sortingElement as ISortableThumbnail<YouTubeEntry>).BeginSorting();
                }
            }
        }

        public Dictionary<YouTubeEntry, int> EndSort()
        {
            this.isSorting = false;
            if (this.currentSortingThumb != null)
                this.currentSortingThumb.EndSorting();
            if (this.ItemList.ItemsPanelRoot != null)
            {
                Panel itemsPanelRoot = this.ItemList.ItemsPanelRoot;
                                
                itemsPanelRoot.LayoutUpdated -= this.panel_LayoutUpdated;
                itemsPanelRoot.LayoutUpdated += this.panel_LayoutUpdated;



                foreach (UIElement child1 in (IEnumerable<UIElement>)itemsPanelRoot.Children)
                {
                    foreach (FrameworkElement child2 in Helper.FindChildren<FrameworkElement>((DependencyObject)(child1 as FrameworkElement), 10))
                    {
                        if (child2 is ISortableThumbnail<YouTubeEntry>)
                        {
                            CompositeTransform compositeTransform = new CompositeTransform();
                            ((UIElement)child2).RenderTransformOrigin = new Point(0.5, 0.5);
                            ((UIElement)child2).RenderTransform = (Transform)compositeTransform;
                            this.setSortTrans((UIElement)child2, true);
                            break;
                        }
                    }
                }
            }
            return this.sortingDictonary;
        }

        private void setSortTrans(UIElement el, bool isSort)
        {
            if (el == null)
                return;
            if (!(el.RenderTransform is CompositeTransform Element))
            {
                Element = new CompositeTransform();
                el.RenderTransform = (Transform)Element;
            }
            el.RenderTransformOrigin = new Point(0.5, 0.5);

            if (isSort)
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)Element, "ScaleX",
                    1.0, 0.2, (EasingFunctionBase)Ani.Ease((EasingMode)0, 3.0)), 
                    (Timeline)Ani.DoubleAni((DependencyObject)Element, "ScaleY", 1.0, 0.2,
                    (EasingFunctionBase)Ani.Ease((EasingMode)0, 3.0)), 
                    (Timeline)Ani.DoubleAni((DependencyObject)el, "Opacity", 1.0, 0.1));
            else
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)Element, "ScaleX", 
                    0.8, 0.2, (EasingFunctionBase)Ani.Ease((EasingMode)0, 3.0)), 
                    (Timeline)Ani.DoubleAni((DependencyObject)Element, "ScaleY", 0.8, 0.2,
                    (EasingFunctionBase)Ani.Ease((EasingMode)0, 3.0)), 
                    (Timeline)Ani.DoubleAni((DependencyObject)el, "Opacity", 0.6, 0.1));
        }

        public void setCurrentSortingThumb(ISortableThumbnail<YouTubeEntry> thumb)
        {
            FrameworkElement el = thumb as FrameworkElement;
            if (thumb == null)
                return;
            if (this.currentSortingThumb != null)
            {
                this.currentSortingThumb.EndSorting();
                this.currentSortingThumb.SortTapped -= new EventHandler<SortTappedEventArgs<YouTubeEntry>>(this.thumb_SortTapped);
                this.setSortTrans(this.currentSortingThumb as UIElement, false);
                this.currentSortingEntry = (YouTubeEntry)null;
            }
            if (thumb != null && thumb != this.currentSortingThumb)
            {
                thumb.BeginSorting();
                this.currentSortingThumb = thumb;
                if (el.DataContext is YouTubeEntry)
                    this.currentSortingEntry = el.DataContext as YouTubeEntry;
                this.setSortTrans((UIElement)el, true);
                thumb.SortTapped += new EventHandler<SortTappedEventArgs<YouTubeEntry>>(this.thumb_SortTapped);
            }
            else
            {
                this.currentSortingThumb = (ISortableThumbnail<YouTubeEntry>)null;
                this.currentSortingEntry = (YouTubeEntry)null;
            }
        }

        private async void thumb_SortTapped(object sender, SortTappedEventArgs<YouTubeEntry> e)
        {
            if (e.Object == null || !((Collection<YouTubeEntry>)this.Entries).Contains(e.Object))
                return;
            int index1 = ((Collection<YouTubeEntry>)this.Entries).IndexOf(e.Object);
            if (index1 == -1)
                return;
            int index2 = -1;
            
            //switch (e.Direction)
            //{
            //    case ControlDirection.Up:
            //        index2 = index1 - 1;
            //        break;
            //    case ControlDirection.Down:
            //        index2 = index1 + 1;
            //        break;
            //}

            if (index2 < 0 || index2 >= ((Collection<YouTubeEntry>)this.Entries).Count)
                return;
            if (this.sortingDictonary.ContainsKey(e.Object))
                this.sortingDictonary[e.Object] = index2;
            else
                this.sortingDictonary.Add(e.Object, index2);
            Panel itemsPanelRoot = this.ItemList.ItemsPanelRoot;
            FrameworkElement sortingElement = this.getSortingElement(((IList<UIElement>)itemsPanelRoot.Children)[index2]);
            double scrollTo = -1.0;
            if (sortingElement != null)
                scrollTo = sortingElement.GetBounds((UIElement)itemsPanelRoot).Top - ((FrameworkElement)this.scroll).ActualHeight / 2.0 + sortingElement.ActualHeight / 2.0;
            ((Collection<YouTubeEntry>)this.Entries)[index1] = ((Collection<YouTubeEntry>)this.Entries)[index2];
            ((Collection<YouTubeEntry>)this.Entries)[index2] = e.Object;
            await Task.Delay(200);
            this.scroll.ChangeView(new double?(), new double?(scrollTo), new float?());
        }

        private FrameworkElement getSortingElement(UIElement c)
        {
            foreach (FrameworkElement child in Helper.FindChildren<FrameworkElement>((DependencyObject)(c as FrameworkElement), 10))
            {
                if (child is ISortableThumbnail<YouTubeEntry>)
                    return child;
            }
            return (FrameworkElement)null;
        }

        private async void scroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if ((this.Client == null || this.Client.IsBusy) && this.LoadVideosFunc == null || !this.LoadOnScroll || this.scroll.ScrollableHeight - this.scroll.VerticalOffset >= 3000.0 || this.busy || this.isSorting || this.isSelecting)
                return;
            await this.Load();
        }

        private void VideoList_DataContextChanged(
          FrameworkElement sender,
          DataContextChangedEventArgs args)
        {
            if (!(args.NewValue is VideoListClient newValue))
                return;
            this.Client = newValue;
        }

        private void VideoList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;//((FrameworkElement)this).ArrangeOverride(finalSize);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height))
                this.AdjustPanelSize(availableSize);
            return availableSize;//((FrameworkElement)this).MeasureOverride(availableSize);
        }

        private void AdjustPanelSize(Size size)
        {
            if (!(this.ItemList.ItemsPanelRoot is ItemsWrapGrid itemsPanelRoot))
                return;
            double num = 18.9;
            if (size.Width > 1760.0)
            {
                this.ThumbPadding = new Thickness(0.0, 0.0, 19.0, 19.0);
                itemsPanelRoot.ItemWidth = (size.Width + num) / 4.0;
            }
            else if (size.Width > 1160.0)
            {
                this.ThumbPadding = new Thickness(0.0, 0.0, 19.0, 19.0);
                itemsPanelRoot.ItemWidth = (size.Width + num) / 3.0;
            }
            else if (size.Width > 665.0 || size.Width > size.Height && size.Width > 400.0 && size.Height < 480.0)
            {
                this.ThumbPadding = new Thickness(0.0, 0.0, 19.0, 19.0);
                itemsPanelRoot.ItemWidth = ((size.Width + num) / 2.0);
            }
            else
            {
                this.ThumbPadding = new Thickness(0.0, 0.0, 0.0, 19.0);
                itemsPanelRoot.ItemWidth = (size.Width);
            }
        }

        public Task<bool> Clear(bool animate)
        {
            this.ThumbnailDispatcher.Reset();
            if (this.clearingTcs != null)
            {
                if (!this.clearingTcs.Task.IsCompleted && !this.clearingTcs.Task.IsCanceled && !this.clearingTcs.Task.IsFaulted)
                    return this.clearingTcs.Task;
                if (animate)
                {
                    this.clearingTcs.TrySetResult(false);
                    this.clearingTcs = (TaskCompletionSource<bool>)null;
                }
            }
            this.clearingTcs = new TaskCompletionSource<bool>();
            this.listStrings.State = !this.DependsOnSignIn || this.IsSignedIn ? ListState.Default : ListState.SignIn;
            if (animate)
            {
                this.page = 0;
                Storyboard sb = new Storyboard();
                double num = 0.03;
                double startTime = 0.0;
                if (this.ItemList.ItemsPanelRoot != null)
                {
                    try
                    {
                        foreach (UIElement child in (IEnumerable<UIElement>)this.ItemList.ItemsPanelRoot.Children)
                        {
                            if (child is FrameworkElement frameworkElement)
                            {
                                Rect bounds = frameworkElement.GetBounds((UIElement)this.scroll);
                                if (bounds.Top > 0.0 && bounds.Top < this.scroll.ViewportHeight 
                                    || bounds.Bottom > 0.0 && bounds.Bottom < this.scroll.ViewportHeight)
                                {
                                    TranslateTransform Element = new TranslateTransform();
                                    ((UIElement)frameworkElement).RenderTransform = (Transform)Element;

                                    sb.Add((Timeline)Ani.DoubleAni((DependencyObject)frameworkElement, 
                                        "Opacity", 0.0, 0.15, (EasingFunctionBase)Ani.Ease((EasingMode)2, 1.0), startTime),
                                        (Timeline)Ani.DoubleAni((DependencyObject)Element, 
                                        "Y", -50.0, 0.15, (EasingFunctionBase)Ani.Ease((EasingMode)1, 5.0), 
                                        startTime));
                                    startTime += num;
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                Storyboard storyboard = sb;

                storyboard.Completed += (s, e) =>
                {
                    ((Collection<YouTubeEntry>)this.Entries).Clear();
                    foreach (UIElement child in (IEnumerable<UIElement>)this.ItemList.ItemsPanelRoot.Children)
                    {
                        child.Opacity = 1.0;
                        child.RenderTransform = null;
                    }
                    this.clearingTcs.TrySetResult(true);
                };


                sb.Begin();
            }
            else
            {
                this.page = 0;
                ((Collection<YouTubeEntry>)this.Entries).Clear();
                this.clearingTcs.TrySetResult(true);
            }
            this.page = 0;
            return this.clearingTcs.Task;
        }

        public async Task Load()
        {
            this.busy = true;
            try
            {
                await this.loadInternal();
            }
            catch
            {
                this.busy = false;
                throw;
            }
            this.busy = false;
        }

        public async Task Load(Task<YouTubeEntry[]> task)
        {
            this.busy = true;
            try
            {
                await this.loadInternal(task);
            }
            catch
            {
                this.busy = false;
                throw;
            }
            this.busy = false;
        }

        private async Task loadInternal(Task<YouTubeEntry[]> task = null)
        {
            if (this.DependsOnSignIn && !this.IsSignedIn && !YouTube.IsSignedIn)
            {
                this.listStrings.State = ListState.SignIn;
            }
            else
            {
                try
                {
                    this.listStrings.State = ListState.Loading;
                    int origPage = this.page;
                    ++this.page;
                    int origPageAdded = this.page;
                    YouTubeEntry[] vids;
                    if (task == null)
                    {
                        if (this.LoadVideosFunc == null)
                        {
                            if (!this.Client.CanLoadPage(origPage))
                                return;
                            vids = await this.Client.GetFeed(origPage);
                        }
                        else
                            vids = await this.LoadVideosFunc(origPage);
                    }
                    else
                        vids = await task;
                    if (this.clearingTcs != null)
                    {
                        try
                        {
                            int num = await this.clearingTcs.Task ? 1 : 0;
                        }
                        catch
                        {
                        }
                    }
                    int num1 = await App.Instance.WindowActivatedTask ? 1 : 0;
                    foreach (YouTubeEntry youTubeEntry in vids)
                    {
                        if (((Collection<YouTubeEntry>)this.Entries).Count == 0 || this.SortVideosFunc == null)
                            ((Collection<YouTubeEntry>)this.Entries).Add(youTubeEntry);
                        else if (this.SortVideosFunc != null)
                        {
                            bool flag = false;
                            for (int index = 0; index < ((Collection<YouTubeEntry>)this.Entries).Count; ++index)
                            {
                                if (this.SortVideosFunc(youTubeEntry, ((Collection<YouTubeEntry>)this.Entries)[index]))
                                {
                                    ((Collection<YouTubeEntry>)this.Entries).Insert(index, youTubeEntry);
                                    flag = true;
                                    break;
                                }
                            }
                            if (!flag)
                                ((Collection<YouTubeEntry>)this.Entries).Add(youTubeEntry);
                        }
                        else
                            ((Collection<YouTubeEntry>)this.Entries).Add(youTubeEntry);
                    }
                    this.page = origPageAdded;
                    if (vids.Length >= 0 && this.VideosLoaded != null)
                        this.VideosLoaded((object)this, vids);
                    if (((Collection<YouTubeEntry>)this.Entries).Count == 0)
                        this.listStrings.State = ListState.NoItems;
                    else if (!this.isSorting && !this.isSelecting)
                    {
                        await ((FrameworkElement)this).WaitForLayoutUpdateAsync();
                        await Task.Delay(500);
                        if (this.loaded && vids.Length != 0 && this.scroll.ScrollableHeight < this.scroll.ViewportHeight)
                            this.Load();
                    }
                    vids = (YouTubeEntry[])null;
                }
                catch (Exception ex)
                {
                    this.listStrings.State = ((Collection<YouTubeEntry>)this.Entries).Count != 0 ? ListState.NoItems : ListState.NoItems;
                    if (this.LoadFailed == null)
                        return;
                    this.LoadFailed((object)this, ex);
                }
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
                return;
            ((App)Application.Current).RootFrame.Navigate(typeof(VideoPage), (object)(e.AddedItems[0] as YouTubeEntry));
        }

        public static void TappedThumb(FrameworkElement thumb)
        {
            if (!(thumb.DataContext is YouTubeEntry dataContext))
                return;
            VideoList parentFromTree = Helper.FindParentFromTree<VideoList>(thumb, 20);
            if (parentFromTree != null)
            {
                if (parentFromTree.isSelecting)
                    return;
                if (parentFromTree.isSorting && thumb is ISortableThumbnail<YouTubeEntry> thumb1)
                    parentFromTree.setCurrentSortingThumb(thumb1);
                else if (parentFromTree.Client != null)
                    (Application.Current as App).RootFrame.Navigate(typeof(VideoPage),
                        (object)new VideoPage.ClientConstructorAndEntry()
                    {
                        ClientConstructor = parentFromTree.Client.GetTypeConstructor(),
                        Entry = dataContext
                    });
                else
                    (Application.Current as App).RootFrame.Navigate(typeof(VideoPage), (object)dataContext);
            }
            else
                (Application.Current as App).RootFrame.Navigate(typeof(VideoPage), (object)dataContext);
        }

        public static void RightTappedThumb(FrameworkElement thumb, RightTappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Mouse)
                return;
            e.Handled = true;
            VideoList parentFromTree = Helper.FindParentFromTree<VideoList>(thumb, 50);
            if (parentFromTree == null || parentFromTree.isSorting)
                return;
            VideoList.showContextMenu(e.GetPosition((UIElement)DefaultPage.Current), thumb, parentFromTree);
        }

        private async Task<TransferManager.State> getState(IEnumerable<YouTubeEntry> entries)
        {
            TransferManager.State state = TransferManager.State.Complete;
            foreach (YouTubeEntry e in entries)
            {
                TransferManager.State s = await App.GlobalObjects.TransferManager.GetTransferState(e, TransferType.Video);
                if (state == TransferManager.State.Complete)
                {
                    switch (s)
                    {
                        case TransferManager.State.None:
                            if (await App.GlobalObjects.TransferManager.GetTransferState(e, TransferType.Audio) == TransferManager.State.None)
                            {
                                state = TransferManager.State.None;
                                return state;
                            }
                            break;
                        case TransferManager.State.Downloading:
                            state = TransferManager.State.Downloading;
                            break;
                    }
                }
                else if (state == TransferManager.State.Downloading && s == TransferManager.State.None)
                {
                    if (await App.GlobalObjects.TransferManager.GetTransferState(e, TransferType.Audio) == TransferManager.State.None)
                    {
                        state = TransferManager.State.None;
                        return state;
                    }
                }
            }
            return state;
        }

        private async Task<IconButtonEventCollection> getMultiselectCollection(
          IEnumerable<YouTubeEntry> entries)
        {
            IconButtonEventCollection coll = new IconButtonEventCollection();
            List<IconButtonEvent>.Enumerator enumerator = this.contextMenuCollection.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    IconButtonEvent c = enumerator.Current;
                    if ((YouTube.IsSignedIn || c != this.watchLaterIcon) && c != this.pinIcon)
                    {
                        if (c == this.deleteButton)
                        {
                            if (YouTube.IsSignedIn)
                            {
                                bool flag = false;
                                foreach (YouTubeEntry entry in entries)
                                {
                                    if (entry.Author != YouTube.UserInfo.ID)
                                    {
                                        flag = true;
                                        break;
                                    }
                                }
                                if (flag)
                                    continue;
                            }
                            else
                                continue;
                        }
                        if (c == this.saveIcon)
                        {
                            switch (await this.getState(entries))
                            {
                                case TransferManager.State.None:
                                    c.Symbol = (Symbol)57605;
                                    c.Text = (App.Strings["common.save", "save"] + " (" + App.Strings["common.highestquality",
                                        "highest quality"] + ")").ToLower();
                                    break;
                                case TransferManager.State.Downloading:
                                    c.Symbol = (Symbol)57610;
                                    c.Text = App.Strings["common.cancel", "cancel"].ToLower();
                                    break;
                                default:
                                    c.Symbol = (Symbol)57607;
                                    c.Text = App.Strings["common.delete", "delete"].ToLower();
                                    break;
                            }
                        }
                        coll.Add(c);
                        c = (IconButtonEvent)null;
                    }
                }
            }
            finally
            {
                enumerator.Dispose();
            }
            enumerator = new List<IconButtonEvent>.Enumerator();
            if (this.SetMultiselectContextMenu != null)
                this.SetMultiselectContextMenu((object)this, coll);
            return coll;
        }

        private async Task<IconButtonEventCollection> getCollection(YouTubeEntry entry)
        {
            IconButtonEventCollection coll = new IconButtonEventCollection();
            if (SecondaryTile.Exists(TileHelper.CreateTileID(entry)))
            {
                this.pinIcon.Symbol = (Symbol)57750;
                this.pinIcon.Text = App.Strings["common.unpin", "unpin"].ToLower();
            }
            else
            {
                this.pinIcon.Symbol = (Symbol)57665;
                this.pinIcon.Text = App.Strings["common.pin", "pin"].ToLower();
            }
            for (int i = 0; i < this.contextMenuCollection.Count; ++i)
            {
                IconButtonEvent c = this.contextMenuCollection[i];
                if (c != this.playIcon && (YouTube.IsSignedIn || c != this.watchLaterIcon))
                {
                    if (c == this.saveIcon)
                    {
                        if (entry != null)
                        {
                            switch (await App.GlobalObjects.TransferManager.GetTransferState(entry))
                            {
                                case TransferManager.State.None:
                                    c.Symbol = (Symbol)57605;
                                    c.Text = (App.Strings["common.save", "save"] + " (" + App.Strings["common.highestquality", "highest quality"] + ")").ToLower();
                                    break;
                                case TransferManager.State.Downloading:
                                    c.Symbol = (Symbol)57610;
                                    c.Text = App.Strings["common.cancel", "cancel"].ToLower();
                                    break;
                                default:
                                    c.Symbol = (Symbol)57607;
                                    c.Text = App.Strings["common.delete", "delete"].ToLower();
                                    break;
                            }
                        }
                        else
                            continue;
                    }
                    if (c != this.deleteButton || YouTube.IsSignedIn && entry.Author == YouTube.UserInfo.ID)
                    {
                        coll.Add(c);
                        c = (IconButtonEvent)null;
                    }
                }
            }
            if (this.SetContextMenu != null)
                this.SetContextMenu((object)this, coll);
            return coll;
        }

        public static void HoldingThumb(FrameworkElement thumb, HoldingRoutedEventArgs e)
        {
            e.put_Handled(true);
            if (e.HoldingState != null || e.PointerDeviceType == 2)
                return;
            VideoList parentFromTree = Helper.FindParentFromTree<VideoList>(thumb, 50);
            if (parentFromTree == null)
                return;
            VideoList.showContextMenu(e.GetPosition((UIElement)DefaultPage.Current), thumb, parentFromTree);
        }

        private static async void showContextMenu(
          Point tappedAt,
          FrameworkElement thumb,
          VideoList list)
        {
            VideoContextMenu videoContextMenu1 = new VideoContextMenu();
            ((FrameworkElement)videoContextMenu1).put_RequestedTheme(App.Theme);
            videoContextMenu1.SelectButtonEnabled = thumb.DataContext is YouTubeEntry;
            VideoContextMenu menu = videoContextMenu1;
            Rect bounds1 = Window.Current.Bounds;
            Rect bounds2 = thumb.GetBounds((UIElement)DefaultPage.Current);
            ((FrameworkElement)menu).put_Width(bounds2.Width);
            ((FrameworkElement)menu).put_Height(Math.Min(bounds2.Height - list.ThumbPadding.Bottom, bounds1.Height - bounds2.Y));
            Popup popup = new Popup();
            popup.put_Child((UIElement)menu);
            ((FrameworkElement)popup).put_RequestedTheme(App.Theme);
            Popup p = popup;
            Point showAt = new Point();
            showAt.X = bounds2.X;
            menu.SetTransitionOffset(0.0, -40.0);
            showAt.Y = bounds2.Y;
            ((Control)menu).put_HorizontalContentAlignment((HorizontalAlignment)3);
            ((Control)menu).put_VerticalContentAlignment((VerticalAlignment)3);
            VideoContextMenu videoContextMenu = menu;
            IconButtonEventCollection collection = await list.getCollection(thumb.DataContext as YouTubeEntry);
            videoContextMenu.ItemsSource = (List<IconButtonEvent>)collection;
            videoContextMenu = (VideoContextMenu)null;
            menu.SelectedElement = thumb;
            menu.SelectTapped += (EventHandler)((s, e) =>
            {
                DefaultPage.Current.ClosePopup();
                list.beginSelect(thumb.DataContext as YouTubeEntry);
            });
            if (menu.ItemsSource.Count > 0)
            {
                ((UIElement)thumb).CancelDirectManipulations();
                DefaultPage.Current.ShowPopup(p, showAt, new Point(0.0, -10.0), FadeType.Half, hideAppBar: false);
            }
            p.put_IsLightDismissEnabled(false);
        }

        private void beginSelect(YouTubeEntry ent)
        {
            Panel itemsPanelRoot = this.ItemList.ItemsPanelRoot;
            if (itemsPanelRoot == null)
                return;
            if (this.selected == null)
                this.selected = new Dictionary<YouTubeEntry, bool>();
            this.selected.Add(ent, true);
            foreach (DependencyObject child1 in (IEnumerable<UIElement>)itemsPanelRoot.Children)
            {
                foreach (FrameworkElement child2 in Helper.FindChildren<FrameworkElement>(child1, 10))
                {
                    if (child2 is ISelectableThumbnail)
                    {
                        ISelectableThumbnail selectableThumbnail = child2 as ISelectableThumbnail;
                        selectableThumbnail.Selected = ent != null && child2.DataContext == ent;
                        selectableThumbnail.BeginSelecting();
                        selectableThumbnail.SelectChanged += new EventHandler<bool>(this.S_SelectChanged);
                        this.isSelecting = true;
                        break;
                    }
                }
            }
            if (!this.isSelecting)
                return;
            ((UIElement)this.cancelSelectedButton).put_Visibility((Visibility)0);
            ((UIElement)this.acceptSelectedButton).put_Visibility((Visibility)0);
        }

        private void S_SelectChanged(object sender, bool e)
        {
            YouTubeEntry key = (YouTubeEntry)null;
            if (sender is YouTubeEntry)
                key = sender as YouTubeEntry;
            if (sender is FrameworkElement)
                key = (sender as FrameworkElement).DataContext as YouTubeEntry;
            if (key == null)
                return;
            if (this.selected.ContainsKey(key))
                this.selected[key] = e;
            else
                this.selected.Add(key, e);
        }

        private List<YouTubeEntry> endSelect()
        {
            Panel itemsPanelRoot = this.ItemList.ItemsPanelRoot;
            List<YouTubeEntry> youTubeEntryList = new List<YouTubeEntry>();
            if (itemsPanelRoot != null)
            {
                foreach (DependencyObject child1 in (IEnumerable<UIElement>)itemsPanelRoot.Children)
                {
                    foreach (FrameworkElement child2 in Helper.FindChildren<FrameworkElement>(child1, 10))
                    {
                        if (child2 is ISelectableThumbnail)
                        {
                            (child2 as ISelectableThumbnail).EndSelecting();
                            break;
                        }
                    }
                }
            }
            using (Dictionary<YouTubeEntry, bool>.Enumerator enumerator = this.selected.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<YouTubeEntry, bool> current = enumerator.Current;
                    if (current.Value)
                        youTubeEntryList.Add(current.Key);
                }
            }
            this.isSelecting = false;
            ((UIElement)this.acceptSelectedButton).put_Visibility((Visibility)1);
            this.selected.Clear();
            return youTubeEntryList;
        }

        private async void watchLaterSelected(object sender, IconButtonEventArgs e)
        {
            e.Close();
            if (e.OriginalSender is FrameworkElement originalSender1 && originalSender1.DataContext is YouTubeEntry dataContext)
            {
                try
                {
                    int num = await YouTube.WatchLater(dataContext.ID, Settings.WatchLater.AddVideosTo == PlaylistPosition.End ? -1 : 0) ? 1 : 0;
                }
                catch
                {
                }
            }
            if (!(e.OriginalSender is IEnumerable<YouTubeEntry> originalSender2))
                return;
            foreach (YouTubeEntry youTubeEntry in originalSender2)
            {
                try
                {
                    int num = await YouTube.WatchLater(youTubeEntry.ID, Settings.WatchLater.AddVideosTo == PlaylistPosition.End ? -1 : 0) ? 1 : 0;
                }
                catch
                {
                }
            }
        }

        private async void saveLaterSelected(object sender, IconButtonEventArgs e)
        {
            e.Close();
            if (e.OriginalSender is FrameworkElement originalSender)
            {
                if (originalSender.DataContext is YouTubeEntry ent)
                {
                    if (await App.GlobalObjects.TransferManager.GetTransferState(ent) != TransferManager.State.None)
                    {
                        int num1 = await App.GlobalObjects.TransferManager.DeleteTransfer(ent, TransferType.Video) ? 1 : 0;
                        int num2 = await App.GlobalObjects.TransferManager.DeleteTransfer(ent, TransferType.Audio) ? 1 : 0;
                    }
                    else
                    {
                        VideoInfoLoader videoInfoLoader = new VideoInfoLoader();
                        try
                        {
                            YouTubeInfo info = await videoInfoLoader.LoadInfoAllMethods(ent.ID);
                            if (info != null)
                            {
                                if (info.FoundVideos)
                                {
                                    DownloadOperation downloadOperation = await App.GlobalObjects.TransferManager.StartTransfer(ent, info, info.HighestQuality(YouTubeQuality.HD), (Progress<DownloadOperation>)null);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                ent = (YouTubeEntry)null;
            }
            if (!(e.OriginalSender is IEnumerable<YouTubeEntry> coll))
                return;
            TransferManager.State status = await this.getState(coll);
            foreach (YouTubeEntry youTubeEntry in coll)
            {
                foreach (YouTubeEntry ent in coll)
                {
                    TransferManager.State st = await App.GlobalObjects.TransferManager.GetTransferState(ent, TransferType.Video);
                    if (status != TransferManager.State.None)
                    {
                        if (st != TransferManager.State.None)
                        {
                            try
                            {
                                int num = await App.GlobalObjects.TransferManager.DeleteTransfer(ent, TransferType.Video) ? 1 : 0;
                            }
                            catch
                            {
                            }
                        }
                    }
                    else if (st == TransferManager.State.None)
                    {
                        VideoInfoLoader videoInfoLoader = new VideoInfoLoader();
                        try
                        {
                            YouTubeInfo info = await videoInfoLoader.LoadInfoAllMethods(ent.ID);
                            if (info != null)
                            {
                                if (info.FoundVideos)
                                {
                                    DownloadOperation downloadOperation = await App.GlobalObjects.TransferManager.StartTransfer(ent, info, info.HighestQuality(YouTubeQuality.HD), (Progress<DownloadOperation>)null);
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private async void pinIcon_Selected(object sender, IconButtonEventArgs e)
        {
            e.Close();
            await Task.Delay(100);
            if (!(e.OriginalSender is FrameworkElement el))
                return;
            if (el.DataContext is YouTubeEntry ent)
            {
                if (!SecondaryTile.Exists(TileHelper.CreateTileID(ent)))
                {
                    VideoTile videoTile = new VideoTile();
                    ((FrameworkElement)videoTile).put_DataContext((object)ent);
                    ((FrameworkElement)videoTile).put_Width(336.0);
                    ((FrameworkElement)videoTile).put_Height(336.0);
                    VideoTile tileElement = videoTile;
                    await Task.Delay(100);
                    RenderTargetBitmap wb = await DefaultPage.Current.RenderElementAsync((FrameworkElement)tileElement, 0.40000000596046448);
                    ((FrameworkElement)tileElement).put_Width(691.0);
                    RenderTargetBitmap wb2 = await DefaultPage.Current.RenderElementAsync((FrameworkElement)tileElement, 0.40000000596046448);
                    ((FrameworkElement)tileElement).put_Width(336.0);
                    Image image1 = new Image();
                    image1.put_Source((ImageSource)wb);
                    Image image2 = image1;
                    Image image3 = new Image();
                    image3.put_Source((ImageSource)wb2);
                    Image image4 = image3;
                    StackPanel stackPanel = new StackPanel();
                    ((FrameworkElement)DefaultPage.Current).GetBounds((UIElement)DefaultPage.Current);
                    ((ICollection<UIElement>)((Panel)stackPanel).Children).Add((UIElement)tileElement);
                    ((ICollection<UIElement>)((Panel)stackPanel).Children).Add((UIElement)image2);
                    ((ICollection<UIElement>)((Panel)stackPanel).Children).Add((UIElement)image4);
                    SecondaryTile tile = await TileHelper.CreateTile(ent, new TileArgs(typeof(VideoPage), ent.ID));
                    el.GetBounds((UIElement)DefaultPage.Current);
                    StorageFile tileImageFile1 = await TileHelper.CreateTileImageFile(ent, wb, (TileSize)3);
                    StorageFile tileImageFile2 = await TileHelper.CreateTileImageFile(ent, wb2, (TileSize)4);
                    await Task.Delay(300);
                    if (await tile.RequestCreateAsync())
                    {
                        SecondaryTile secondaryTile = tile;
                        // ISSUE: method pointer
                        WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<SecondaryTile, VisualElementsRequestedEventArgs>>(new Func<TypedEventHandler<SecondaryTile, VisualElementsRequestedEventArgs>, EventRegistrationToken>(secondaryTile.add_VisualElementsRequested), new Action<EventRegistrationToken>(secondaryTile.remove_VisualElementsRequested), new TypedEventHandler<SecondaryTile, VisualElementsRequestedEventArgs>((object)this, __methodptr(tile_VisualElementsRequested)));
                        await TileHelper.UpdateSecondaryTile(ent);
                    }
                    tileElement = (VideoTile)null;
                    wb = (RenderTargetBitmap)null;
                    wb2 = (RenderTargetBitmap)null;
                    tile = (SecondaryTile)null;
                }
                else
                    await (await TileHelper.CreateTile(ent, new TileArgs(typeof(VideoPage), ent.ID))).DeleteAndCleanUpImages();
            }
            ent = (YouTubeEntry)null;
        }

        private void tile_VisualElementsRequested(
          SecondaryTile sender,
          VisualElementsRequestedEventArgs args)
        {
        }

        private async void deleteButton_Selected(object sender, IconButtonEventArgs e)
        {
            if (e.OriginalSender is FrameworkElement originalSender)
            {
                if (originalSender.DataContext is YouTubeEntry ent)
                {
                    int num = await new MessageDialog(App.Strings["dialogs.videos.deleteupload", "Are you sure you want to delete this upload?"], App.Strings["dialogs.titles.areyousure", "Are you sure?"]).ShowAsync(App.Strings["common.yes", "yes"].ToLower(), App.Strings["common.no", "no"].ToLower());
                    e.Close();
                    if (num == 0)
                    {
                        int index = ((Collection<YouTubeEntry>)this.Entries).IndexOf(ent);
                        if (index != -1)
                            ((Collection<YouTubeEntry>)this.Entries).Remove(ent);
                        try
                        {
                            await new YouTubeEntryClient().Delete(ent.ID);
                        }
                        catch
                        {
                            ((Collection<YouTubeEntry>)this.Entries).Insert(index, ent);
                        }
                    }
                }
                ent = (YouTubeEntry)null;
            }
            if (!(e.OriginalSender is IEnumerable<YouTubeEntry> ents))
                return;
            if (await new MessageDialog(App.Strings["dialogs.videos.deleteuploadmultiple", "Are you sure you want to delete these upload?"], App.Strings["dialogs.titles.areyousure", "Are you sure?"]).ShowAsync(App.Strings["common.yes", "yes"].ToLower(), App.Strings["common.no", "no"].ToLower()) != 0)
                return;
            e.Close();
            foreach (YouTubeEntry ent in ents)
            {
                if (ent.Author == YouTube.UserInfo.ID)
                {
                    int index = ((Collection<YouTubeEntry>)this.Entries).IndexOf(ent);
                    if (index != -1)
                        ((Collection<YouTubeEntry>)this.Entries).Remove(ent);
                    try
                    {
                        await new YouTubeEntryClient().Delete(ent.ID);
                    }
                    catch
                    {
                        if (index != 1)
                            ((Collection<YouTubeEntry>)this.Entries).Insert(index, ent);
                    }
                }
            }
        }

        private async void acceptSortButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.isSelecting)
                return;
            List<YouTubeEntry> entries = this.endSelect();
            ((UIElement)this.acceptSelectedButton).put_Visibility((Visibility)1);
            ((UIElement)this.cancelSelectedButton).put_Visibility((Visibility)1);
            if (entries.Count <= 0)
                return;
            VideoContextMenu videoContextMenu1 = new VideoContextMenu();
            ((FrameworkElement)videoContextMenu1).put_DataContext((object)entries);
            ((FrameworkElement)videoContextMenu1).put_RequestedTheme(App.Theme);
            videoContextMenu1.CancelButtonEnabled = true;
            VideoContextMenu menu = videoContextMenu1;
            menu.SelectedElement = (FrameworkElement)this;
            VideoContextMenu videoContextMenu = menu;
            IconButtonEventCollection multiselectCollection = await this.getMultiselectCollection((IEnumerable<YouTubeEntry>)entries);
            videoContextMenu.ItemsSource = (List<IconButtonEvent>)multiselectCollection;
            videoContextMenu = (VideoContextMenu)null;
            menu.CancelTapped += (EventHandler)((s, args) => DefaultPage.Current.ClosePopup());
            Popup popup1 = new Popup();
            popup1.put_Child((UIElement)menu);
            Popup popup2 = popup1;
            DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
            {
                ((UIElement)this).UpdateLayout();
                Rect bounds = ((FrameworkElement)this).GetBounds(Window.Current.Content);
                ((FrameworkElement)menu).put_Width(Math.Min(bounds.Width, 380.0));
                ((FrameworkElement)menu).put_Height(Math.Min(bounds.Height, 380.0));
                return new Point(bounds.Right - ((FrameworkElement)menu).Width, bounds.Bottom - ((FrameworkElement)menu).Height);
            }));
            int num = await DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, -40.0)) ? 1 : 0;
            ((UIElement)this.cancelSelectedButton).put_Visibility((Visibility)1);
        }

        private void cancelSortButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.endSelect();
            DefaultPage.Current.ClosePopup();
            ((UIElement)this.cancelSelectedButton).put_Visibility((Visibility)1);
            ((UIElement)this.acceptSelectedButton).put_Visibility((Visibility)1);
        }

        private void playIcon_Selected(object sender, IconButtonEventArgs e)
        {
            if (!(e.OriginalSender is IEnumerable<YouTubeEntry>))
                return;
            YouTubeEntry[] array = Enumerable.ToArray<YouTubeEntry>(e.OriginalSender as IEnumerable<YouTubeEntry>);
            YouTubeEntry entry = Enumerable.FirstOrDefault<YouTubeEntry>((IEnumerable<YouTubeEntry>)array);
            if (entry == null)
                return;
            XElement xelement = new XElement((XName)"OfflineVideos");
            foreach (YouTubeEntry youTubeEntry in array)
                xelement.Add((object)new XElement((XName)"video")
                {
                    Value = youTubeEntry.OriginalString
                });
            DefaultPage.Current.VideoPlayer.SetTypeConstructor(new OfflinePlaylistClient(WebUtility.UrlEncode(((object)xelement).ToString()), 15).GetTypeConstructor());
            DefaultPage.Current.VideoPlayer.OpenVideo(entry, Settings.Quality);
            e.Close();
        }

        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [DebuggerNonUserCode]
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("ms-appx:///VideoList.xaml"), (ComponentResourceLocation)0);
            this.userControl = (UserControl)((FrameworkElement)this).FindName("userControl");
            this.listStrings = (ListStrings)((FrameworkElement)this).FindName("listStrings");
            this.contextMenuCollection = (IconButtonEventCollection)((FrameworkElement)this).FindName("contextMenuCollection");
            this.playIcon = (IconButtonEvent)((FrameworkElement)this).FindName("playIcon");
            this.deleteButton = (IconButtonEvent)((FrameworkElement)this).FindName("deleteButton");
            this.watchLaterIcon = (IconButtonEvent)((FrameworkElement)this).FindName("watchLaterIcon");
            this.pinIcon = (IconButtonEvent)((FrameworkElement)this).FindName("pinIcon");
            this.saveIcon = (IconButtonEvent)((FrameworkElement)this).FindName("saveIcon");
            this.scroll = (ScrollViewer)((FrameworkElement)this).FindName("scroll");
            this.cancelSelectedButton = (ContentControl)((FrameworkElement)this).FindName("cancelSelectedButton");
            this.acceptSelectedButton = (ContentControl)((FrameworkElement)this).FindName("acceptSelectedButton");
            this.multiSelectSymbol = (SymbolIcon)((FrameworkElement)this).FindName("multiSelectSymbol");
            this.loadingText = (TextBlock)((FrameworkElement)this).FindName("loadingText");
            this.ItemList = (ItemsControl)((FrameworkElement)this).FindName("ItemList");
        }

        [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
        [DebuggerNonUserCode]
        public void Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((IconButtonEvent)target).Selected += new EventHandler<IconButtonEventArgs>(this.playIcon_Selected);
                    break;
                case 2:
                    ((IconButtonEvent)target).Selected += new EventHandler<IconButtonEventArgs>(this.deleteButton_Selected);
                    break;
                case 3:
                    ((IconButtonEvent)target).Selected += new EventHandler<IconButtonEventArgs>(this.watchLaterSelected);
                    break;
                case 4:
                    ((IconButtonEvent)target).Selected += new EventHandler<IconButtonEventArgs>(this.pinIcon_Selected);
                    break;
                case 5:
                    ((IconButtonEvent)target).Selected += new EventHandler<IconButtonEventArgs>(this.saveLaterSelected);
                    break;
                case 6:
                    UIElement uiElement1 = (UIElement)target;
                    WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.cancelSortButton_Tapped));
                    break;
                case 7:
                    UIElement uiElement2 = (UIElement)target;
                    WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.acceptSortButton_Tapped));
                    break;
            }
            this._contentLoaded = true;
        }
    }
}

