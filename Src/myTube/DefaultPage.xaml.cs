// myTube.DefaultPage

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
using myTube.Debug;
using myTube.Helpers;
using RykenTube;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.Media.Transcoding;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.AwaitableUI;

namespace myTube
{
    public sealed partial class DefaultPage : UserControl, IVideoContainer
    {
        private const double TitleOffset = 45.0;
        private const double PivotOffset = 70.0;
        private const string Tag = "DefaultPage";
        private TaskCompletionSource<bool> popupTcs;
        private WebView webView;
        private bool goingBack;
        private TranslateTransform rootTrans;
        public const int MaxVisualTreeIterations = 100;
        private int iterations;
        private OverCanvas overCanvas = new OverCanvas(); //!
        private TranslateTransform stackPanelTrans;
        private TranslateTransform titleTrans;
        private TranslateTransform pivotTrans;

        public static readonly DependencyProperty ShownProperty 
            = DependencyProperty.Register(nameof(Shown), typeof(bool), 
                typeof(DefaultPage), 
                new PropertyMetadata((object)true,
                    new PropertyChangedCallback(DefaultPage.ShownPropertyChanged)));

        public static readonly DependencyProperty BannerReadyProperty 
            = DependencyProperty.Register(nameof(BannerReady), typeof(bool),
                typeof(DefaultPage), 
                new PropertyMetadata((object)false, 
                    new PropertyChangedCallback(DefaultPage.BannerReadyPropertyChanged)));

        public static readonly DependencyProperty PopupCloseAnimationProperty 
            = DependencyProperty.RegisterAttached("PopupCloseAnimation", typeof(Storyboard), 
                typeof(Popup), 
                new PropertyMetadata((object)null));

        public static readonly DependencyProperty PopupArrangeMethodProperty 
            = DependencyProperty.RegisterAttached("PopupArrangeMethod", typeof(Func<Point>),
                typeof(Popup), 
                new PropertyMetadata((object)null));
       
        public bool KeyboardControls = true;
        private LeftRightControl leftRight;
        private Popup currentPopup;
        private Page page;
        private SimpleOrientation lastSimpleOrientation;
        private RotationType rotationType = RotationType.Custom;
        private CompositeTransform trans = new CompositeTransform();
        private static DefaultPage lastDefaultPage = (DefaultPage)null;
        private const double AppBarMinimalHeight = 24.0;
        private const double AppBarCompactHeight = 57.5;
        private bool easyPopupDismissal;
        private ApplicationView appView;
        private AppBarButton appBarSearch;
        private uint videoPlayerIndex;
        private bool setMarginsOnStopScrolling;
        private bool playerShown;
        private Visibility appBarVis = (Visibility)1;
        private bool rotationLocked;
        private SimpleOrientation lockRotation;
        public bool SurpressPopupClosing;
        private bool movingWithDpad;
        private bool movingOvercanvas;
        private Storyboard leftRightFadeIn = new Storyboard();
        private Storyboard leftRightFadeOut = new Storyboard();
        private DispatcherTimer leftRightTimer;
        private DispatcherTimer mouseShownTimer;
        private bool leftRightShown;
        private DisplayOrientations or;
        private bool firstSetOrient;
        private Storyboard frameNavigatingAnimation;
        private string lastStateName = "";
        private Type lastType;
        private bool firstMiniPlayerChanged;
        private MiniPlayerType lastMiniPlayerType;
        private IVideoContainer currentPlayerElement;
        private DispatcherTimer playerArrangeTimer;
        private TranslateTransform playerTrans;
        private bool busyPlacingVideo;
        private IVideoContainer waitingVideoElement;
        private IVideoContainer reservedVideoElement;
        private bool waitingBind;
        private TaskCompletionSource<bool> waitingVideoTcs;
        private TaskCompletionSource<bool> currentVideoTcs;
        private bool lastVideoBind = true;
        private bool videoButtonShown;
        private DebugInfoPanel debugPanel;
        private IVideoContainer fullscreenReturnElement;
        public static readonly Point DefaultPopupTransitionOffset = new Point(0.0, 80.0);
        private Point popupTrans;
        private Storyboard popupAni;
        private object BackPressed;

      

        public VideoPlayer Player
        {
            get
            {
                return this.player;
            }
        }

        public OverCanvas OverCanvas
        {
            get
            {
                return this.overCanvas;
            }
        }

        public TimeSpan PlayerArrangeActiveTick
        {
            get
            {
                return App.DeviceFamily == DeviceFamily.Desktop
                   ? TimeSpan.FromSeconds(1.0 / 120.0) 
                   : TimeSpan.FromSeconds(1.0 / 30.0);
            }
        }

        public TimeSpan PlayerArrangeInactiveTick
        {
            get
            {
                return App.DeviceFamily == DeviceFamily.Desktop
                   ? TimeSpan.FromSeconds(0.5) 
                   : TimeSpan.FromSeconds(1.0);
            }
        }

        public static Storyboard GetPopupCloseAnimation(DependencyObject obj)
        {
            return (Storyboard)obj.GetValue(DefaultPage.PopupCloseAnimationProperty);
        }

        public static void SetPopupCloseAnimation(DependencyObject obj, Storyboard value)
        {
            obj.SetValue(DefaultPage.PopupCloseAnimationProperty, (object)value);
        }

        public static Func<Point> GetPopupArrangeMethod(DependencyObject obj)
        {
            return (Func<Point>)obj.GetValue(DefaultPage.PopupArrangeMethodProperty);
        }

        public static void SetPopupArrangeMethod(DependencyObject obj, Func<Point> value)
        {
            obj.SetValue(DefaultPage.PopupArrangeMethodProperty, (object)value);
        }

        private static void BannerReadyPropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            DefaultPage defaultPage = d as DefaultPage;
            if ((bool)e.NewValue)
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)defaultPage.backgroundRec, 
                    "Opacity", (double)((IDictionary<object, object>)((FrameworkElement)defaultPage).Resources)
                    [(object)"BackgroundRecOpacity"], 0.2));
            else
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)defaultPage.backgroundRec, 
                    "Opacity", 0.0, 0.2));
        }

        private static async void ShownPropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            DefaultPage page = d as DefaultPage;
            bool newValue = (bool)e.NewValue;
            if (page.playerShown)
                page.player.Hidden = false;
            else if (page.lastVideoBind)
                page.player.Hidden = newValue;
            page.player.ControlsShown = false;
            if (!newValue)
            {
                ((UIElement)page.blackRec).Visibility = Visibility.Visible;
                Storyboard storyboard = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)page.titleGrid, 
                    "Opacity", 0.0, 0.25), page.player.MediaRunning
                    ? (Timeline)Ani.DoubleAni((DependencyObject)page.blackRec, "Opacity", 1.0, 0.25, 
                                     (EasingFunctionBase)Ani.Ease((EasingMode)2, 1.0), 0.25)
                    : (Timeline)null);
                
                int num;

                storyboard.Completed += (s, e1) =>
                {
                    num = page.Shown ? 1 : 0;
                };

                ((UIElement)page.titleGrid).IsHitTestVisible = false;
            }
            else
            {
                ((UIElement)page.titleGrid).Visibility = Visibility.Visible;

                Storyboard storyboard = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)page.titleGrid, 
                    "Opacity", 1.0, 0.2), page.player.MediaRunning 
                    ? (Timeline)Ani.DoubleAni((DependencyObject)page.blackRec, "Opacity", 0.0, 0.2) 
                    : (Timeline)null);

                storyboard.Completed += (s, e2) =>
                {
                    if (!page.Shown)
                        return;
                    ((UIElement)page.blackRec).Visibility = Visibility.Visible;
                };

                ((UIElement)page.titleGrid).IsHitTestVisible = true;
            }
        }

        public bool Shown
        {
            get => (bool)((DependencyObject)this).GetValue(DefaultPage.ShownProperty);
            set => ((DependencyObject)this).SetValue(DefaultPage.ShownProperty, (object)value);
        }

        public bool BannerReady
        {
            get => (bool)((DependencyObject)this).GetValue(DefaultPage.BannerReadyProperty);
            set => ((DependencyObject)this).SetValue(DefaultPage.BannerReadyProperty, 
                (object)DefaultPage.BannerReadyProperty);
        }

        public VideoPlayer VideoPlayer => this.player;

        public Popup CurrentPopup => this.currentPopup;

        public bool PopupShown => this.currentPopup != null;

        public RotationType RotationType
        {
            get => this.rotationType;
            set => this.rotationType = value;
        }

        public static DefaultPage Current => Window.Current != null 
            && Window.Current.Content is DefaultPage ? Window.Current.Content as DefaultPage
            : DefaultPage.lastDefaultPage;

        //public event EventHandler<BackPressedEventArgs> BackPressed;

        public CustomFrame Frame => this.RootFrame;

        public DefaultPage()
        {
            DefaultPage.lastDefaultPage = this;

            Helper.Write((object)nameof(DefaultPage), (object)"Constructor");
            this.InitializeComponent();
            
            Helper.Write((object)nameof(DefaultPage), (object)"InitializedComponent");
            DependencyProperty shownProperty = DefaultPage.ShownProperty;
            
            Binding binding1 = new Binding();

            binding1.Path = new PropertyPath(nameof(Shown));

            this.SetBinding(shownProperty, (BindingBase)binding1);
            AppBarButton appBarButton = new AppBarButton();
            SymbolIcon symbolIcon = new SymbolIcon();

            symbolIcon.Symbol = (Symbol)57626;
            appBarButton.Icon = (IconElement)symbolIcon;
            appBarButton.Label="search";

            this.appBarSearch = appBarButton;
            AppBarButton appBarSearch = this.appBarSearch;

            appBarSearch.Click += this.searchButton_Click;

            this.appView = ApplicationView.GetForCurrentView();
            this.FontFamily = new FontFamily("Segoe WP");
            ApplicationView appView = this.appView;

            appView.VisibleBoundsChanged += (s, e) => view_VisibleBoundsChanged(this, e);

            this.appView.SetDesiredBoundsMode((ApplicationViewBoundsMode)1);

            this.Loaded += this.DefaultPage_Loaded;


            this.rootTrans = new TranslateTransform();
            this.titleTrans = new TranslateTransform();
            this.pivotTrans = new TranslateTransform();
            this.RootFrame.RenderTransform = (Transform)this.rootTrans;
            Helper.Write((object)nameof(DefaultPage), (object)"Create TranslateTransforms");
            CustomFrame rootFrame1 = this.RootFrame;

            rootFrame1.Navigated += this.RootFrame_Navigated;


            CustomFrame rootFrame2 = this.RootFrame;
           
            rootFrame2.Navigating += (s, e) => RootFrame_Navigating(s, e);

            this.RootFrame.NavigationCalled += new EventHandler<NavigationMode>(this.RootFrame_NavigationCalled);
            Button backButton = this.backButton;
           
           
            backButton.Click += this.backButton_Click;

            this.SizeChanged += this.DefaultPage_SizeChanged;

            HardwareButtons.BackPressed += this.HardwareButtons_BackPressed;

            this.ManipulationMode = (ManipulationModes)3;

            Helper.Write((object)nameof(DefaultPage), 
                (object)("Set ManipulatonMode to " + (object)((UIElement)this).ManipulationMode));

            YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
            YouTube.SignedOut += new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);

            this.ManipulationDelta += this.DefaultPage_ManipulationDelta;
            this.ManipulationStarted += this.DefaultPage_ManipulationStarted;
            this.Tapped += this.DefaultPage_Tapped;
            this.ManipulationCompleted += this.DefaultPage_ManipulationCompleted;

            CoreWindow coreWindow1 = Window.Current.CoreWindow;

            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;

            CoreWindow coreWindow2 = Window.Current.CoreWindow;

            Window.Current.CoreWindow.KeyUp += this.CoreWindow_KeyUp;

            CoreWindow coreWindow3 = Window.Current.CoreWindow;
            
            Window.Current.CoreWindow.PointerPressed += this.CoreWindow_PointerPressed;

            CoreWindow coreWindow4 = Window.Current.CoreWindow;

            Window.Current.CoreWindow.PointerEntered += this.CoreWindow_PointerEntered;


            Window current = Window.Current;
            
            current.SizeChanged += this.Current_SizeChanged;

            Helper.Write((object)nameof(DefaultPage), (object)"Created CoreWindow key and pointer events");
            
            this.SetTheme(Settings.Theme);

            try
            {
                if (App.GlobalObjects != null)
                App.GlobalObjects.VideoThumbTemplate = default;//Settings.Thunbnail != ThumbnailStyle.Classic
                 //   ? (DataTemplate)((IDictionary<object, object>)Application.Current.Resources)[(object)"VideoThumbs2"]
                 //   : (DataTemplate)((IDictionary<object, object>)Application.Current.Resources)[(object)"VideoThumbs"];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "[ex] App.GlobalObjects.VideoThumbTemplate init error: " + ex.Message);
            }

            DependencyProperty bannerReadyProperty = DefaultPage.BannerReadyProperty;
            Binding binding2 = new Binding();
            binding2.Path = (new PropertyPath(nameof(BannerReady)));
            binding2.FallbackValue = ((object)false);
            binding2.TargetNullValue = ((object)false);
            this.SetBinding(bannerReadyProperty, (BindingBase)binding2);
            DisplayInformation forCurrentView = DisplayInformation.GetForCurrentView();
            
         
            forCurrentView.OrientationChanged += this.DefaultPage_OrientationChanged;

            CustomFrame rootFrame3 = this.RootFrame;

            rootFrame3.Navigated += this.FirstNavi;


            this.RenderTransform = ((Transform)this.trans);
            this.RenderTransformOrigin = (new Point(0.5, 0.5));
            Helper.Write((object)nameof(DefaultPage), (object)"Created");
            
            CoreDispatcher dispatcher = ((DependencyObject)this).Dispatcher;
            dispatcher.AcceleratorKeyActivated += this.Dispatcher_AcceleratorKeyActivated;

            this.videoPlayerIndex = (uint)this.LayoutRoot.Children.IndexOf(this.player);
        }

        private void view_VisibleBoundsChanged(DefaultPage defaultPage, object e)
        {
            //Not Implemented
        }

        private void Dispatcher_AcceleratorKeyActivated(
          CoreDispatcher sender,
          AcceleratorKeyEventArgs args)
        {
            if (args.EventType != null && args.EventType != CoreAcceleratorKeyEventType.SystemKeyDown 
                || args.VirtualKey != VirtualKey.MiddleButton || args.VirtualKey != VirtualKey.Enter
                || !args.KeyStatus.IsMenuKeyDown || !this.player.MediaRunning || this.overCanvas == null)
                return;
            this.ToggleFullscreen();
            this.player.Controls.UpdateFullscreenButton();
        }

        private void view_VisibleBoundsChanged(ApplicationView sender, object args)
        {
            this.setMargins();
            if (this.overCanvas == null)
                return;
            ((UIElement)this.overCanvas).InvalidateMeasure();
        }

        private void setMargins()
        {
            Thickness thickness1 = new Thickness();
            Thickness thickness2 = new Thickness();
            Rect visibleBounds = this.appView.VisibleBounds;
            Rect bounds = Window.Current.Bounds;
            double num = 0.0;
            thickness1.Bottom = bounds.Bottom - visibleBounds.Bottom + num;
            thickness1.Left = thickness2.Left = visibleBounds.Left - bounds.Left;
            thickness1.Right = thickness2.Right = bounds.Right - visibleBounds.Right;
            if (!this.Shown || this.playerShown)
                return;
            this.RootFrame.Margin = thickness1;
            this.titleGrid.Margin = thickness2;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            //RnD
            return base.MeasureOverride(availableSize);
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            if (this.overCanvas == null)
                return;
            this.overCanvas.Invalidate();
        }

        private void CoreWindow_PointerEntered(CoreWindow sender, PointerEventArgs args)
        {
        }

        private void FirstNavi(object sender, NavigationEventArgs e)
        {
            ((Windows.UI.Xaml.Controls.Frame)this.RootFrame).Navigated -= this.FirstNavi;
            this.SetOrientationType();
        }

        public void SetOrientationType() => this.SetOrientationType(Settings.RotationType);

        public void SetOrientationType(RotationType type)
        {
            if (type == RotationType.Custom)
            {
                DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
                this.player.Controls.ControlsState = PlayerControlsState.Compact;
            }
            else
            {
                if (type != RotationType.System)
                    return;
                DisplayInformation.AutoRotationPreferences = (DisplayOrientations)7;
                this.player.Controls.ControlsState = PlayerControlsState.Default;
            }
        }

        private void DefaultPage_OrientationChanged(DisplayInformation sender, object args) 
            => this.SetOrientation(sender.CurrentOrientation);

        private void DefaultPage_OrientationChanged(
          SimpleOrientationSensor sender,
          SimpleOrientationSensorOrientationChangedEventArgs args)
        {
            this.Rotate(args.Orientation);
        }

        public void LockRotation(SimpleOrientation or)
        {
            Helper.Write((object)nameof(DefaultPage), (object)("Locking rotation to " + (object)or));
            Accel.Stop();
            this.lockRotation = or;
            this.rotationLocked = true;
            this.Rotate(or);
        }

        public void UnlockRotation()
        {
            Accel.Start();
            this.rotationLocked = false;
            Helper.Write((object)nameof(DefaultPage), (object)"Unlocking rotation");
            SimpleOrientation orient = Accel.Orient;
            if (orient == this.lockRotation)
                return;
            this.Rotate(orient);
        }

        public async void Rotate(SimpleOrientation orientation, bool actuallyRotate = true)
        {
            if (currentPopup != null) return;

            if (rotationLocked)
            {
                orientation = lockRotation;
            }

            if (Settings.RotationType != RotationType.Custom &&
                orientation != SimpleOrientation.Rotated270DegreesCounterclockwise &&
                orientation != SimpleOrientation.Rotated90DegreesCounterclockwise &&
                orientation != lastSimpleOrientation)
            {
                lastSimpleOrientation = orientation;
                Helper.Write(nameof(DefaultPage), $"Rotating to {orientation}");

                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, 
                    RotateHandler(orientation));
            }
        }

        private DispatchedHandler RotateHandler(object state)
        {
            var displayClass = (DefaultPage.DisplayClass86_0)state;
            
            // Bad idea (iterative cycling!)
            //displayClass.u003E4.Rotate(displayClass.or);
            return default;
        }

        private void setPlayerShown(bool res)
        {
            this.playerShown = res;
            if (this.Shown && this.page != null && this.page.BottomAppBar != null)
            {
                if (this.lastSimpleOrientation == null && !res)
                    ((UIElement)this.page.BottomAppBar).Visibility = (Visibility)0;
                else if (((UIElement)this.page.BottomAppBar).Visibility != (Visibility)1 & res)
                {
                    this.appBarVis = ((UIElement)this.page.BottomAppBar).Visibility;
                    ((UIElement)this.page.BottomAppBar).Visibility = (Visibility)1;
                }
                else
                    this.appBarVis = (Visibility)0;
            }
          ((UIElement)this.RootFrame).IsHitTestVisible = !res;
            ((UIElement)this.titleGrid).IsHitTestVisible = !res && this.Shown;
            if (this.Shown)
            {
                double opacity;
                if (res)
                {
                    opacity = 0.0;
                }
                else
                {
                    opacity = 1.0;
                    this.Player.ControlsShown = false;
                }
                if (res)
                {
                    Storyboard storyboard = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.RootFrame,
                        "Opacity", opacity, 0.4), 
                        (Timeline)Ani.DoubleAni((DependencyObject)this.titleGrid, "Opacity", opacity, 0.4));

                    
                    ((Timeline)storyboard).Completed += (s, e) =>
                    {
                        ((UIElement)this.blackRec).Visibility = Visibility.Collapsed;
                        Ani.Begin((DependencyObject)this.blackRec, "Opacity", 1.0 - opacity, 0.2);
                        ((UIElement)this.RootFrame).Visibility = Visibility.Visible;
                        ((UIElement)this.titleGrid).Visibility = Visibility.Visible;
                    };
                }
                else
                {
                    Grid titleGrid = this.titleGrid;

                    ((UIElement)this.RootFrame).Visibility = Visibility.Collapsed;
                    ((UIElement)this.titleGrid).Visibility = Visibility.Collapsed;
                    Storyboard storyboard = Ani.Begin(
                        Ani.DoubleAni((DependencyObject)this.RootFrame, "Opacity", opacity, 0.4),
                        Ani.DoubleAni((DependencyObject)this.titleGrid, "Opacity", opacity, 0.4),
                        Ani.DoubleAni((DependencyObject)this.blackRec, "Opacity", 1.0 - opacity, 0.1));
                    storyboard.Completed += (s, e) =>
                    {
                        if (!this.Shown)
                            return;
                        ((UIElement)this.blackRec).Visibility = Visibility.Visible;
                    };
                }
            }
            else if (res)
            {
                Grid titleGrid = this.titleGrid;

                Visibility visibility5;
                ((UIElement)this.RootFrame).Visibility = (Visibility)(int)(visibility5 = (Visibility.Collapsed));
                Visibility visibility6 = visibility5;
                ((UIElement)titleGrid).Visibility = visibility6;
            }
            else
            {
                Grid titleGrid = this.titleGrid;
                Visibility visibility7;
                ((UIElement)this.RootFrame).Visibility = ((Visibility)(int)(visibility7 = (Visibility)0));
                Visibility visibility8 = visibility7;
                ((UIElement)titleGrid).Visibility = (visibility8);
            }
        }

        public async Task<RenderTargetBitmap> RenderElementAsync(
          FrameworkElement el,
          double scale,
          RenderTargetBitmap ren = null)
        {
            ((UIElement)this.renderingCanvas).Visibility = Visibility.Visible;

            Helper.Write((object)"Rendering RenderTargetBitmap in DefaultPage");
            Helper.WriteMemory("Memory before rendering element");

            ((UIElement)el).Opacity = 0.1;
            ((ICollection<UIElement>)((Panel)this.renderingCanvas).Children).Add((UIElement)el);
            await el.WaitForImagesToLoad(5000);
            if (ren == null)
                ren = new RenderTargetBitmap();
            ((UIElement)el).Opacity = 1.0;
            await ren.RenderAsync((UIElement)el, (int)(el.ActualWidth * scale), (int)(el.ActualHeight * scale));
            ((ICollection<UIElement>)((Panel)this.renderingCanvas).Children).Remove((UIElement)el);
            Helper.WriteMemory("Memory after rendering element");

            ((UIElement)this.renderingCanvas).Visibility = Visibility.Collapsed;
            return ren;
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            if (this.currentPopup != null && this.currentPopup.Child != null && this.easyPopupDismissal)
            {
                Point rawPosition = args.CurrentPoint.RawPosition;
                FrameworkElement el = this.currentPopup.Child as FrameworkElement;
                if (el is UserControl)
                    el = (el as UserControl).Content as FrameworkElement;
                if (!this.SurpressPopupClosing && !el.ContainsPoint(Window.Current.Content as FrameworkElement, args.CurrentPoint.RawPosition))
                    this.ClosePopup();
            }
            if (!args.CurrentPoint.Properties.IsXButton1Pressed)
                return;
            args.Handled = true;
            this.GoBack();
        }

        private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
        {
            if (args.Handled || FocusManager.GetFocusedElement() is TextBox)
                return;
            if ((args.VirtualKey == (VirtualKey)32 || args.VirtualKey == (VirtualKey)75) && !this.VideoPlayer.Hidden)
            {
                if (this.VideoPlayer.MediaElement.CurrentState == MediaElementState.Playing)
                    this.VideoPlayer.MediaElement.Pause();
                else
                    this.VideoPlayer.MediaElement.Play();
            }
            
            if (!this.VideoPlayer.MediaRunning || this.VideoPlayer.Hidden 
                || this.VideoPlayer.MediaElement.CurrentState != MediaElementState.Playing 
                && this.VideoPlayer.MediaElement.CurrentState != MediaElementState.Paused 
                && this.VideoPlayer.MediaElement.CurrentState != MediaElementState.Buffering)
                return;

            TimeSpan timeSpan1 = this.VideoPlayer.MediaElement.Position;
            TimeSpan timeSpan2 = this.VideoPlayer.MediaElement.NaturalDuration.TimeSpan;
            bool flag = false;
            VirtualKey virtualKey = args.VirtualKey;
            switch (virtualKey - 48)
            {
                case (VirtualKey)0:
                    timeSpan1 = TimeSpan.Zero;
                    flag = true;
                    break;
                case (VirtualKey)1:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.1);
                    flag = true;
                    break;
                case (VirtualKey)2:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.2);
                    flag = true;
                    break;
                case (VirtualKey)3:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.3);
                    flag = true;
                    break;
                case (VirtualKey)4:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.4);
                    flag = true;
                    break;
                case (VirtualKey)5:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.5);
                    flag = true;
                    break;
                case (VirtualKey)6:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.6);
                    flag = true;
                    break;
                case (VirtualKey)7:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.7);
                    flag = true;
                    break;
                case (VirtualKey)8:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.8);
                    flag = true;
                    break;
                case (VirtualKey)9:
                    timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.9);
                    flag = true;
                    break;
                default:
                    if (virtualKey != (VirtualKey)74)
                    {
                        if (virtualKey == (VirtualKey)76)
                        {
                            timeSpan1 += TimeSpan.FromSeconds(10.0);
                            flag = true;
                            break;
                        }
                        break;
                    }
                    timeSpan1 -= TimeSpan.FromSeconds(10.0);
                    flag = true;
                    break;
            }
            if (!flag)
                return;
            this.VideoPlayer.ControlsShown = true;
            this.VideoPlayer.ControlsWatch.Restart();
            if (VideoPlayer.AudioElement.HasMedia())
            {
                MediaElement audioElement = VideoPlayer.AudioElement;
                TimeSpan timeSpan3;
                this.VideoPlayer.MediaElement.Position = timeSpan3 = timeSpan1;
                TimeSpan timeSpan4 = timeSpan3;
                audioElement.Position =(timeSpan4);
            }
            else
                this.VideoPlayer.MediaElement.Position = timeSpan1;
        }

        private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.KeyStatus.IsKeyReleased || args.Handled || FocusManager.GetFocusedElement() is TextBox)
                return;
            if (args.VirtualKey == (VirtualKey)27 || args.VirtualKey == (VirtualKey)166)
                args.Handled = this.GoBack();
            if (args.VirtualKey == (VirtualKey)36 || args.VirtualKey == (VirtualKey)172)
            {
                //TODO
                //this.RootFrame.Navigate(typeof(HomePage));
                this.RootFrame.ClearBackStackAtNavigate();
            }
            if (this.overCanvas == null || this.movingWithDpad || App.IsFullScreen)
                return;
            if (args.VirtualKey == (VirtualKey)37)
            {
                this.movingWithDpad = true;
                this.overCanvas.ScrollToPage(this.overCanvas.SelectedPage - 1, false);
            }
            else if (args.VirtualKey == (VirtualKey)39)
            {
                this.movingWithDpad = true;
                this.overCanvas.ScrollToPage(this.overCanvas.SelectedPage + 1, false);
            }
            if (!this.movingWithDpad)
                return;
            await Task.Delay(250);
            this.movingWithDpad = false;
        }

        private void makeLeftRightTimer()
        {
            if (this.leftRightTimer != null)
                return;
            this.leftRightTimer = new DispatcherTimer();
            this.leftRightTimer.Interval = (TimeSpan.FromSeconds(0.5));
            DispatcherTimer leftRightTimer = this.leftRightTimer;

            leftRightTimer.Tick += leftRightTimer_Tick;

            this.leftRightFadeIn.Add((Timeline)Ani.DoubleAni((DependencyObject)this.leftRight, 
                "Opacity", 1.0, 0.1));
            this.leftRightFadeOut.Add((Timeline)Ani.DoubleAni((DependencyObject)this.leftRight, 
                "Opacity", 0.0, 0.1));
            Storyboard leftRightFadeOut = this.leftRightFadeOut;
            leftRightFadeOut.Completed += (s, e) =>
            {
                this.leftRight.Visibility = Visibility.Visible;
            };
        }

        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            if (this.overCanvas != null && !this.Shown && App.DeviceFamily == DeviceFamily.Desktop)
            {
                if (this.player.MediaRunning)
                    this.ToggleFullscreen();
                else
                    this.ExitFullscreenMode();
            }         
            base.OnDoubleTapped(e);
        }

        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if ((e.Pointer.PointerDeviceType == PointerDeviceType.Mouse 
                || e.Pointer.PointerDeviceType == PointerDeviceType.Pen) && this.leftRight != null)
            {
                if (this.leftRight == null)
                    return;
                this.makeLeftRightTimer();
                PointerPoint currentPoint = e.GetCurrentPoint((UIElement)this.leftRight);
                if (currentPoint.Position.X < 380.0 
                    || currentPoint.Position.X > ((FrameworkElement)this).ActualWidth - 380.0)
                {
                    this.leftRightTimer.Stop();
                    if (!this.Shown)
                        this.leftRightTimer.Start();
                    this.showLeftRight();
                }
                else if (!this.Shown)
                {
                    this.leftRightTimer.Stop();
                    this.leftRightTimer.Start();
                    this.showLeftRight();
                }
                else if (this.leftRightShown)
                {
                    if (!this.leftRightTimer.IsEnabled)
                    {
                        this.leftRightTimer.Start();
                    }
                    else
                    {
                        this.leftRightTimer.Stop();
                        this.leftRightTimer.Start();
                    }
                }
            }
            else
                this.hideLeftRight();
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                if (!this.player.Hidden && ((FrameworkElement)this.player).ContainsPoint(e))
                {
                    if (this.player.MediaRunning)
                        this.player.ControlsShown = true;
                    if (this.player.ControlsWatch.IsRunning)
                        this.player.ControlsWatch.Restart();
                    if (this.player.MediaElement.CurrentState == MediaElementState.Playing 
                        || this.player.MediaElement.CurrentState == MediaElementState.Paused)
                        this.createMouseShownTimer();
                }
                else if (!this.player.Controls.IsSeeking && this.player.ControlsShown)
                {
                    Helper.Write((object)nameof(DefaultPage), (object)"Hiding player controls on OnPointerMoved event");
                    this.player.ControlsShown = false;
                }
            }
          base.OnPointerMoved(e);
        }

        private void createMouseShownTimer()
        {
            if (this.mouseShownTimer != null)
                return;
            this.mouseShownTimer = new DispatcherTimer();
            this.mouseShownTimer.Interval = TimeSpan.FromSeconds(3.0);
        }

        private void mouseShownTimer_Tick(object sender, object e) => this.mouseShownTimer.Stop();

        private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
        }

        private void showLeftRight()
        {
            if (this.leftRight == null || this.leftRightShown)
                return;
            ((UIElement)this.leftRight).Visibility = ((Visibility)0);
            this.leftRightFadeIn.Begin();
            this.leftRightShown = true;
        }

        private void hideLeftRight()
        {
            if (this.leftRight == null || !this.leftRightShown || this.leftRightFadeOut == null)
                return;
            this.leftRightShown = false;
            this.leftRightFadeOut.Begin();
        }

        private void leftRightTimer_Tick(object sender, object e)
        {
            this.leftRightTimer.Stop();
            this.hideLeftRight();
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);
            if (this.currentPlayerElement == null 
                || !this.player.ControlsShown || ((FrameworkElement)this.player).ContainsPoint(e))
                return;
            this.player.ControlsShown = false;
        }

        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            if (this.currentPlayerElement != null && this.currentPlayerElement != this.LayoutRoot)
                this.player.ControlsShown = false;
            base.OnPointerWheelChanged(e);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (((RoutedEventArgs)e).OriginalSource is TextBlock originalSource && originalSource.IsTextSelectionEnabled || ((RoutedEventArgs)e).OriginalSource is TextBox)
                return;
            int num1 = this.player.ControlsShown ? 1 : 0;
            PointerPoint currentPoint = e.GetCurrentPoint(this);
            if (!this.movingOvercanvas && this.overCanvas != null 
                && 
                !this.player.ControlsShown 
                && e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                int num2 = currentPoint.Properties.IsLeftButtonPressed ? 1 : 0;
            }
            this.movingOvercanvas = false;
        }

        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            base.OnManipulationDelta(e);
            if (!this.movingOvercanvas)
                return;
            this.overCanvas.Move(e.Delta.Translation.X);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            base.OnManipulationCompleted(e);
            if (!this.movingOvercanvas)
                return;
            ((UIElement)this).ManipulationMode = (ManipulationModes)65536;
            if (this.overCanvas != null)
                this.overCanvas.EndMove();
            this.movingOvercanvas = false;
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            base.OnPointerReleased(e);
            MediaTranscoder mediaTranscoder = new MediaTranscoder();
            if (!this.movingOvercanvas)
                return;
            if (this.overCanvas != null)
                this.overCanvas.EndMove();
            ((UIElement)this).ManipulationMode = (ManipulationModes)65536;
            this.movingOvercanvas = false;
        }

        private async void SetOrientation(DisplayOrientations o)
        {
            if (this.page == null || this.firstSetOrient)
                return;
            this.firstSetOrient = true;
            int num = await App.Instance.WindowActivatedTask ? 1 : 0;
        }

        private void DefaultPage_ManipulationCompleted(
          object sender,
          ManipulationCompletedRoutedEventArgs e)
        {
            this.player.Controls.EndManipulation();
        }

        private void DefaultPage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.player.Hidden && !this.playerShown)
                return;
            e.GetPosition((UIElement)this.player.Controls.MainGrid);
            if ((!this.player.ControlsShown || !this.player.Controls.IsTapping(e)) 
                && (this.playerShown || ((FrameworkElement)this.player).ContainsPoint(e)))
            {
                this.player.ControlsShown = !this.player.ControlsShown;
            }
            else
            {
                if (!this.player.ControlsShown || ((FrameworkElement)this.player).ContainsPoint(e))
                    return;
                this.player.ControlsShown = false;
            }
        }

        private void DefaultPage_ManipulationStarted(
          object sender,
          ManipulationStartedRoutedEventArgs e)
        {
        }

        private void DefaultPage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!this.player.ControlsShown && !this.playerShown)
                return;
            if (Settings.MiniPlayerType == MiniPlayerType.MiniPlayer
                && this.player.ControlsShown && !this.player.Controls.IsSeeking 
                && !((FrameworkElement)this.player).ContainsPoint((FrameworkElement)this, e.Position))
            {
                Helper.Write((object)nameof(DefaultPage),
                    (object)"Hiding player controls in ManipulationDelta event");
                this.player.ControlsShown = false;
            }
            else
            {
                e.Handled = true;
                if (!this.player.ControlsShown)
                    this.player.ControlsShown = true;
                double wid;
                Point delta;
                Point total;

                if (this.lastSimpleOrientation == SimpleOrientation.Rotated90DegreesCounterclockwise)
                {
                    wid = Window.Current.Bounds.Height * 0.9;
                    ref Point local1 = ref delta;
                    double y1 = e.Delta.Translation.Y;
                    ManipulationDelta manipulationDelta = e.Delta;
                    double y2 = -manipulationDelta.Translation.X;
                    local1 = new Point(y1, y2);
                    ref Point local2 = ref total;
                    manipulationDelta = e.Cumulative;
                    double y3 = manipulationDelta.Translation.Y;
                    manipulationDelta = e.Cumulative;
                    double y4 = -manipulationDelta.Translation.X;
                    local2 = new Point(y3, y4);
                }
                else if (this.lastSimpleOrientation == SimpleOrientation.Rotated270DegreesCounterclockwise)
                {
                    wid = Window.Current.Bounds.Height * 0.9;
                    delta = new Point(-e.Delta.Translation.Y, e.Delta.Translation.X);
                    total = new Point(-e.Cumulative.Translation.Y, e.Cumulative.Translation.X);
                }
                else
                {
                    delta = e.Delta.Translation;
                    total = e.Cumulative.Translation;
                    wid = double.NaN;
                }
                this.player.Controls.PerformManipulation(delta, total, wid);
            }
        }

        private async void YouTube_SignedOut(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                this.YouTube_SignedOut();
            });
        }


        private async void YouTube_SignedIn(object sender, EventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                this.YouTube_SignedIn();
            });
        }

        //TODO
        private void YouTube_SignedIn()
        {
            // your code here
        }

        private void YouTube_SignedOut()
        {
            // your code here
        }

        private void HardwareButtons_BackPressed(object sender, EventArgs e)
        {
            //TODO
            //if (this.BackPressed != null)
            //    this.BackPressed(sender, e);

            //if (e.Handled)
            //    return;
            //e.Handled= this.GoBack();
        }

        
        public bool GoBack()
        {
            if (App.IsFullScreen)
            {
                this.ExitFullscreenMode();
                this.player.Controls.UpdateFullscreenButton();
                return true;
            }
            if (this.currentPopup != null)
            {
                this.ClosePopup();
                return true;
            }
            if (this.webView != null && ((ICollection<UIElement>)(
                (Panel)this.LayoutRoot).Children).Contains((UIElement)this.webView))
            {
                this.CloseBrowser();
                return true;
            }
            if (this.RootFrame != null)
            {
                if ((!this.VideoPlayer.Hidden || this.playerShown) && this.VideoPlayer.MediaElement != null 
                    && Settings.MiniPlayerType == MiniPlayerType.Background)
                {
                    if ((this.VideoPlayer.MediaElement.CurrentState == (MediaElementState)3 
                        || this.VideoPlayer.MediaElement.CurrentState == (MediaElementState)4 
                        || this.VideoPlayer.MediaElement.CurrentState == (MediaElementState)2) 
                        && Settings.MiniPlayerType == MiniPlayerType.Background)
                    {
                        this.VideoPlayer.SetBookmark(save: true);
                        if (VideoPlayer.AudioElement.HasMedia())
                            VideoPlayer.AudioElement.Stop();
                        this.VideoPlayer.MediaElement.Stop();
                    }
                    else if (this.OverCanvas != null && !this.OverCanvas.Shown)
                    {
                        if (this.OverCanvas.SelectedPage == -1)
                            this.OverCanvas.ScrollToIndex(0, false, true);
                        else
                            this.OverCanvas.ScrollToIndex(((ICollection<UIElement>)this.OverCanvas.Children).Count - 1, false, true);
                    }
                    return true;
                }
                if (this.RootFrame.CanGoBack)
                {
                    this.RootFrame.GoBack();
                    return true;
                }
            }
            return false;
        }

        private void RootFrame_NavigationCalled(object sender, NavigationMode e)
        {
            if (this.currentPopup != null)
                this.ClosePopup();
            this.goingBack = (e == NavigationMode.Back);

            if (this.RootFrame.Animate)
            {
                Storyboard storyboard = Ani.Animation(
                    Ani.DoubleAni((DependencyObject)this.titleTrans, "Y", 
                    this.goingBack ? 45.0 : -45.0, 0.1, (EasingFunctionBase)Ani.Ease((EasingMode)1, 3.0)),
                    Ani.DoubleAni((DependencyObject)this.pivotTrans, "Y",
                    this.goingBack ? 70.0 : -70.0, 0.075, (EasingFunctionBase)Ani.Ease((EasingMode)1, 3.0)),
                    Ani.DoubleAni((DependencyObject)this.pivot, "Opacity", 0.0, 0.075),
                    Ani.DoubleAni((DependencyObject)this.title, "Opacity", 0.0, 0.1),
                    Ani.DoubleAni((DependencyObject)this.backgroundRec, "Opacity", 0.0, 0.1));
                this.frameNavigatingAnimation = storyboard;
                storyboard.Begin();
            }
            else
            {
                TextBlock title = this.title;
                double num1;
                this.pivot.Opacity = num1 = 0.0;
                double num2 = num1;
                title.Opacity = num2;
                this.titleTrans.Y = this.goingBack ? 15.0 : -15.0;
                this.pivotTrans.Y = this.goingBack ? 20.0 : -20.0;
            }
        }

        private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
        }

        private void DefaultPage_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.Write((object)nameof(DefaultPage), (object)"Loaded");
            this.titleGrid.RenderTransform = (Transform)(this.stackPanelTrans = new TranslateTransform());
            this.pivot.RenderTransform = (Transform)this.pivotTrans;
            this.title.RenderTransform = (Transform)this.titleTrans;
            Helper.Write((object)nameof(DefaultPage), (object)"Getting orientation sensor");
            Accel.Dispatcher = ((DependencyObject)this).Dispatcher;
            Accel.OrientChanged += new OrientChangedEventHandler(this.Accel_OrientChanged);
            Helper.Write((object)"Got orientation sensor");
        }

        private void Accel_OrientChanged(OrientChangedEventArgs e)
        {
            this.Rotate(e.Orientation);
        }

        private void DefaultPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.MiniPlayerTypeChanged();
            
            if (this.currentPopup != null)
            {
                Func<Point> popupArrangeMethod 
                    = DefaultPage.GetPopupArrangeMethod((DependencyObject)this.currentPopup);
                if (popupArrangeMethod != null)
                {
                    Point point = popupArrangeMethod();
                    this.currentPopup.HorizontalOffset = point.X;
                    this.currentPopup.VerticalOffset = point.Y;
                }
            }
            string str = e.NewSize.Height > 450.0 ? (e.NewSize.Width <= 500.0 
                || e.NewSize.Height <= 500.0 
                ? "TinyPhone" 
                : (e.NewSize.Width > 800.0 ? "DefaultPhone" : "NarrowPhone")) : "TinyLandscapePhone";
            if (!(str != this.lastStateName))
                return;
            VisualStateManager.GoToState((Control)this, str, false);
            

            //TEMP
            //string str = "DefaultPhone";
            this.lastStateName = str;
        }

        private void backButton_Click(object sender, RoutedEventArgs e) => ((App)Application.Current).GoBack();

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            App.CheckSignIn(45.0);
            if (((ContentControl)this.RootFrame).Content == null || !(((ContentControl)this.RootFrame).Content is FrameworkElement content))
                return;
            if (content is Page)
                this.SetPage(content as Page);
            if (content.GetType() == this.lastType)
            {
                this.fe_Loaded((object)content, (RoutedEventArgs)null);
            }
            else
            {
                this.lastType = content.GetType();
                FrameworkElement frameworkElement = content;
                frameworkElement.Loaded += this.fe_Loaded;
            }
            if (!(content is OverCanvas))
                return;
            this.SetOverCanvas(content as OverCanvas);
        }

        private void SetPage(Page page)
        {
            if (this.page != null && this.page.BottomAppBar 
                is CommandBar && this.page.BottomAppBar is CommandBar bottomAppBar 
                && ((ICollection<ICommandBarElement>)bottomAppBar.PrimaryCommands).Contains((ICommandBarElement)this.appBarSearch))
                ((ICollection<ICommandBarElement>)bottomAppBar
                    .PrimaryCommands).Remove((ICommandBarElement)this.appBarSearch);
            this.page = page;
            this.SetOrientation(DisplayInformation.GetForCurrentView().CurrentOrientation);
            this.SetSearchInAppBar(Settings.SearchInAppBar);
        }

        public void SetSearchInAppBar(bool set)
        {
            if (this.page == null | this.page.BottomAppBar == null 
                || !(this.page.BottomAppBar is CommandBar bottomAppBar))
                return;
            if (set)
            {
                ((UIElement)this.searchButton).Opacity = 0.0;
                ((UIElement)this.searchButton).IsHitTestVisible = false;
                ((UIElement)this.searchButton).Visibility = Visibility.Collapsed;
                if (bottomAppBar == null || ((ICollection<ICommandBarElement>)bottomAppBar.PrimaryCommands).Contains((ICommandBarElement)this.appBarSearch))
                    return;
                this.appBarSearch.Label = App.Strings["search.search", "search"];
                ((ICollection<ICommandBarElement>)bottomAppBar.PrimaryCommands)
                    .Add((ICommandBarElement)this.appBarSearch);
                ((Control)this.appBarSearch).IsEnabled = true;
            }
            else
            {
                ((UIElement)this.searchButton).Opacity = 1.0;
                ((UIElement)this.searchButton).IsHitTestVisible = true;
                ((UIElement)this.searchButton).Visibility = Visibility.Visible;
                if (bottomAppBar == null || 
                  !((ICollection<ICommandBarElement>)bottomAppBar.PrimaryCommands).Contains((ICommandBarElement)this.appBarSearch))
                    return;
                ((ICollection<ICommandBarElement>)bottomAppBar.PrimaryCommands).Remove((ICommandBarElement)this.appBarSearch);
            }
        }

        public void SetTheme(ElementTheme theme) => ((FrameworkElement)this).RequestedTheme = theme;

        private void setAppBarPlaceholder(bool open)
        {
            if (open)
            {
                this.appBarTrans.Y = this.appBarPlaceHolder.ActualHeight;
                this.appBarPlaceHolder.Visibility = Visibility.Visible;
                Ani.Begin((DependencyObject)this.appBarTrans, "Y", 0.0, 0.4, 
                    (EasingFunctionBase)Ani.Ease((EasingMode)0, 4.0));
            }
            else
            {
                if (((UIElement)this.appBarPlaceHolder).Visibility != null)
                    return;
                Storyboard storyboard = Ani.Begin((DependencyObject)this.appBarTrans, "Y", 
                    ((FrameworkElement)this.appBarPlaceHolder).ActualHeight, 0.2, 
                    (EasingFunctionBase)Ani.Ease((EasingMode)1, 4.0));

                storyboard.Completed += (s, e) 
                    => this.appBarPlaceHolder.Visibility = Visibility.Visible;
            }
        }

        private async void fe_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = default;
            if (sender is FrameworkElement)
            {
                this.iterations = 0;
                OverCanvas oc = await this.SearchForOverCanvas((DependencyObject)fe);
                fe.Loaded -= this.fe_Loaded;
                this.SetOverCanvas(oc);
            }
            Page page = fe as Page;
        }

        private async Task<OverCanvas> SearchForOverCanvas(DependencyObject obj)
        {
            if (obj is OverCanvas)
                return obj as OverCanvas;
            int numChildren = VisualTreeHelper.GetChildrenCount(obj);
            if (numChildren == 0)
                return (OverCanvas)null;
            for (int i = 0; i < numChildren; ++i)
            {
                ++this.iterations;
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is OverCanvas)
                    return child as OverCanvas;
                if (this.iterations > 100)
                    return (OverCanvas)null;
                OverCanvas overCanvas = await this.SearchForOverCanvas(child);
                if (overCanvas != null)
                    return overCanvas;
            }
            return (OverCanvas)null;
        }

        private void setOverCanvasFlipStyle(OverCanvas oc)
        {
            if (Settings.MiniPlayerType == MiniPlayerType.MiniPlayer)
                oc.FlipStyle = FlipStyle.Pivot;
            else
                oc.FlipStyle = FlipStyle.Classic;
        }

        private void SetOverCanvas(OverCanvas oc)
        {
            if (this.overCanvas != null)
                this.overCanvas.ScrollingStopped -= new EventHandler<OnScrollingStoppedEventArgs>(this.oc_ScrollingStopped);
            if (oc != null)
                oc.ScrollingStopped += new EventHandler<OnScrollingStoppedEventArgs>(this.oc_ScrollingStopped);
            this.overCanvas = oc;
            if (oc != null)
            {
                oc.SignedIn = YouTube.IsSignedIn;
                this.setOverCanvasFlipStyle(oc);
            }
            bool bannerReady = this.BannerReady;

            this.DataContext = (object)oc;
            if (oc != null)
            {
                PivotHeader pivot1 = this.pivot;
                DependencyProperty stringsProperty = PivotHeader.StringsProperty;
                Binding binding1 = new Binding();
                binding1.Path = new PropertyPath("PageTitles");
                ((FrameworkElement)pivot1).SetBinding(stringsProperty, (BindingBase)binding1);
                PivotHeader pivot2 = this.pivot;
                DependencyProperty indexProperty = PivotHeader.IndexProperty;
                Binding binding2 = new Binding();
                binding2.Path = new PropertyPath("SelectedPage");
                ((FrameworkElement)pivot2).SetBinding(indexProperty, (BindingBase)binding2);
                this.pivot.OverCanvas = oc;
            }
            else
                this.title.Text = "";

            double num = 60.0;
            double Duration = 1.1;
            if (this.frameNavigatingAnimation != null)
                this.frameNavigatingAnimation.Stop();
            this.pivotTrans.Y = this.goingBack ? -num : num;
            this.titleTrans.Y = this.goingBack ? -num * 0.677 : num * 0.677;

            Storyboard sb = Ani.Animation(
                Ani.DoubleAni((DependencyObject)this.pivotTrans, "Y", 0.0, 
                Duration, (EasingFunctionBase)Ani.Ease((EasingMode)0, 7.0)), 
                Ani.DoubleAni((DependencyObject)this.titleTrans, "Y", 0.0, 
                Duration * 0.667, (EasingFunctionBase)Ani.Ease((EasingMode)0, 5.0)), 
                Ani.DoubleAni((DependencyObject)this.title, "Opacity", 1.0, 0.2),
                Ani.DoubleAni((DependencyObject)this.pivot, "Opacity", 1.0, 0.2));

            if (((oc == null ? 0 : (oc.BannerReady ? 1 : 0)) & (bannerReady ? 1 : 0)) != 0)
                sb.Add((Timeline)Ani.DoubleAni((DependencyObject)this.backgroundRec, 
                    "Opacity", (double)((IDictionary<object, object>)(
                    (FrameworkElement)this).Resources)[(object)"BackgroundRecOpacity"], 0.2));
            if (!this.RootFrame.Animate)
                ((Timeline)sb).SpeedRatio = 2.0;
            sb.Begin();
        }

        public async void MiniPlayerTypeChanged()
        {
            
            if (!this.firstMiniPlayerChanged)
                this.firstMiniPlayerChanged = true;
            else if (this.lastMiniPlayerType == Settings.MiniPlayerType)
                return;

            this.lastMiniPlayerType = Settings.MiniPlayerType;
            switch (Settings.MiniPlayerType)
            {
                case MiniPlayerType.Background:
                    this.SetOrientationType();
                    IVideoContainer temp = this.currentPlayerElement;
                    int num1 = 0;

                    try
                    {
                        num1 = await this.requestVideoPlayerInternal((IVideoContainer)null, true) ? 1 : 0;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("[ex] requestVideoPlayerInternal error:" + ex.Message);
                    }


                    this.bindVideoPlayer(true);
                    this.reservedVideoElement = temp;
                    temp = (IVideoContainer)null;
                    break;
                case MiniPlayerType.MiniPlayer:
                    this.SetOrientationType(RotationType.System);
                    if (this.reservedVideoElement != null)
                    {
                        int num2 = await this.requestVideoPlayerInternal(this.reservedVideoElement, this.reservedVideoElement.GetBindVideoPlayerShown()) ? 1 : 0;
                        break;
                    }
                    break;
            }
            
            if (this.overCanvas == null)
                return;
            this.setOverCanvasFlipStyle(this.overCanvas);
        }

        private void removePlayerFromParent(bool callEvent = true)
        {
            if (this.currentPlayerElement == null || !callEvent || this.currentPlayerElement == null)
                return;
            this.currentPlayerElement.VideoUnset();
        }

        private async void addPlayerToParent(IVideoContainer parent, bool callEvent = true)
        {
            if (parent == this.LayoutRoot || parent == null)
            {
                if (this.currentPlayerElement == null && this.fullscreenReturnElement == null)
                    return;
                ((Panel)this.LayoutRoot).Children.Move((uint)((IList<UIElement>)((Panel)this.LayoutRoot).Children).IndexOf((UIElement)this.player), this.videoPlayerIndex);
                if (this.playerArrangeTimer != null)
                    this.playerArrangeTimer.Stop();
                this.currentPlayerElement = (IVideoContainer)null;
                if (this.fullscreenReturnElement != null)
                {
                    if (callEvent && this.fullscreenReturnElement != null)
                        this.fullscreenReturnElement.VideoUnset();
                    this.fullscreenReturnElement = (IVideoContainer)null;
                }
                VideoPlayer player = this.player;
                double num1;
                this.player.Height = num1 = double.NaN;
                double num2 = num1;
                player.Width = num2;
                this.player.HorizontalAlignment = HorizontalAlignment.Stretch;
                this.player.VerticalAlignment = VerticalAlignment.Stretch;
                if (this.playerTrans != null)
                {
                    TranslateTransform playerTrans = this.playerTrans;
                    double num3;
                    this.playerTrans.Y = num3 = 0.0;
                    double num4 = num3;
                    playerTrans.X = num4;
                }
              ((Control)this.player).Background = (Brush)null;
            }
            else
            {
                if (this.playerArrangeTimer == null)
                {
                    this.playerArrangeTimer = new DispatcherTimer();
                    this.playerArrangeTimer.Interval = TimeSpan.FromSeconds(1.0 / 120.0);
                    DispatcherTimer playerArrangeTimer = this.playerArrangeTimer;
                    playerArrangeTimer.Tick += this.PlayerArrangeTimer_Tick;
                }
                if (this.playerTrans == null)
                {
                    this.playerTrans = new TranslateTransform();
                    this.player.RenderTransform = (Transform)this.playerTrans;
                }
                this.player.Background = (Brush)new SolidColorBrush(Colors.Black);
                this.currentPlayerElement = parent;
                if (this.fullscreenReturnElement != null)
                    this.fullscreenReturnElement = parent;
                this.player.HorizontalAlignment = HorizontalAlignment.Left;
                this.player.VerticalAlignment = VerticalAlignment.Top;
                this.PlayerArrangeTimer_Tick((object)null, (object)null);
                this.playerArrangeTimer.Start();
                if (!callEvent || parent == null)
                    return;
                parent.VideoSet();
            }
        }

        private void PlayerArrangeTimer_Tick(object sender, object e)
        {
            if (this.currentPlayerElement == null)
                return;
            try
            {
                if (this.currentPlayerElement.GetVideoDepth() == VideoDepth.Above)
                {
                    uint num1 = (uint)(((IList<UIElement>)((Panel)this.LayoutRoot).Children).IndexOf((UIElement)this.RootFrame) + 1);
                    uint num2 = (uint)((IList<UIElement>)((Panel)this.LayoutRoot).Children).IndexOf((UIElement)this.player);
                    if ((int)num1 != (int)num2)
                        ((Panel)this.LayoutRoot).Children.Move(num2, num1);
                }
                else
                {
                    uint num = (uint)((IList<UIElement>)((Panel)this.LayoutRoot).Children)
                        .IndexOf((UIElement)this.player);
                    if ((int)num != (int)this.videoPlayerIndex)
                        ((Panel)this.LayoutRoot).Children.Move(num, this.videoPlayerIndex);
                }

                Point position = ((UIElement)this.currentPlayerElement.GetElement()).GetPosition((UIElement)this);
                
                this.playerTrans.X = position.X;
                this.playerTrans.Y = position.Y;

                if (this.player.Width != this.currentPlayerElement.GetElement().ActualWidth)
                    this.player.Width = this.currentPlayerElement.GetElement().ActualWidth;
                if (this.player.Height != this.currentPlayerElement.GetElement().ActualHeight)
                    this.player.Height = this.currentPlayerElement.GetElement().ActualHeight;
                this.bindVideoPlayer(this.currentPlayerElement.GetBindVideoPlayerShown());

                if (this.currentPlayerElement.HasBackground())
                {
                    if (this.player.Background == null)
                        this.player.Background = new SolidColorBrush(Colors.Black);
                }
                else if (this.player.Background != null)
                    this.player.Background = null;

                if (this.currentPlayerElement.IsArrangeActive())
                    this.playerArrangeTimer.Interval = this.PlayerArrangeActiveTick;
                else
                    this.playerArrangeTimer.Interval = this.PlayerArrangeInactiveTick;
            }
            catch
            {
            }
        }

        public async Task<bool> RequestVideoPlayer(IVideoContainer element)
        {
            if (this.currentVideoTcs != null)
            {
                int num = await this.currentVideoTcs.Task ? 1 : 0;
            }
            return await this.requestVideoPlayerInternal(element, false);
        }

        public async Task<bool> ResetVideoPlayer()
        {
            if (this.currentVideoTcs != null)
            {
                int num = await this.currentVideoTcs.Task ? 1 : 0;
            }

            Helper.Write((object)nameof(DefaultPage), 
                (object)"Resetting video player");

            return await this.requestVideoPlayerInternal((IVideoContainer)this, true);
        }




        private Task<bool> requestVideoPlayerInternal(
          IVideoContainer element,
          bool bindShown,
          bool callEvent = true)
        {
            Helper.Write((object)nameof(DefaultPage),
                (object)("Requesting video player control on " + (object)element ?? "NULL"));
            System.Diagnostics.Debug.WriteLine("DefaultPage",
                   "Requesting video player control on " + (object)element ?? "NULL");

                                            //RnD
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>(); 

            /*
            if (Settings.MiniPlayerType != MiniPlayerType.MiniPlayer 
                && this.currentPlayerElement == null)
            {
                this.reservedVideoElement = element;
                tcs = new TaskCompletionSource<bool>();
                tcs.SetResult(false);
                return tcs.Task;
            }
            if (App.IsFullScreen && element != null && element != this)
            {
                Helper.Write((object)nameof(DefaultPage),
                    (object)"VideoPlayer currently in fullscreen, exiting method");
                System.Diagnostics.Debug.WriteLine("DefaultPage",
                   "VideoPlayer currently in fullscreen, exiting method");

                tcs = new TaskCompletionSource<bool>();
                this.fullscreenReturnElement = element;
                tcs.SetResult(true);
                this.bindVideoPlayer(bindShown);
                this.showOrHideVideoButton();
                return tcs.Task;
            }
            if (this.waitingVideoTcs != null)
            {
                tcs = this.waitingVideoTcs;
                this.waitingVideoTcs = (TaskCompletionSource<bool>)null;
            }
            else
                tcs = new TaskCompletionSource<bool>();
            if (this.currentPlayerElement == null && element == null 
                || this.currentPlayerElement == this && element == this)
            {
                Helper.Write((object)nameof(DefaultPage), 
                    (object)"Video player has already been reset, exiting method");
                System.Diagnostics.Debug.WriteLine("DefaultPage",
                   "Video player has already been reset, exiting method");

                tcs.TrySetResult(true);
                return tcs.Task;
            }
            if (element == this.currentPlayerElement && (!App.IsFullScreen 
                || this.fullscreenReturnElement == element))
            {
                Helper.Write((object)nameof(DefaultPage), 
                    (object)"Element already set as video player control, exiting method");
                System.Diagnostics.Debug.WriteLine("DefaultPage",
                  "Element already set as video player control, exiting method");

                tcs.SetResult(true);
                this.showOrHideVideoButton();
                return tcs.Task;
            }
            if (this.busyPlacingVideo)
            {
                Helper.Write((object)nameof(DefaultPage), 
                    (object)"Already setting other element as video player control, this element will be placed afterwords. Exiting method.");
                System.Diagnostics.Debug.WriteLine("DefaultPage",
                  "Already setting other element as video player control, this element will be placed afterwords. Exiting method.");

                this.waitingBind = bindShown;
                this.waitingVideoElement = element;
                if (this.waitingVideoTcs != null)
                    this.waitingVideoTcs.TrySetResult(false);
                this.waitingVideoTcs = this.waitingVideoTcs != tcs 
                    ? tcs : new TaskCompletionSource<bool>();
                this.showOrHideVideoButton();
                return tcs.Task;
            }
            this.busyPlacingVideo = true;
            this.currentVideoTcs = tcs;
            if (this.playerArrangeTimer != null)
                this.playerArrangeTimer.Stop();
            this.removePlayerFromParent(callEvent);

            this.player.Opacity = 0.0;

            this.addPlayerToParent(element, callEvent);

            Storyboard storyboard = Ani.Begin((DependencyObject)this.player, "Opacity", 1.0, 0.5);

            storyboard.Completed += (s, e) =>
            {
                this.busyPlacingVideo = false;
                if (this.waitingVideoElement != null)
                {
                    this.requestVideoPlayerInternal(this.waitingVideoElement, this.waitingBind);
                    this.waitingVideoElement = null;
                }
                tcs.TrySetResult(this.player.Parent == element);
                this.showOrHideVideoButton();
            };
            */
            return tcs.Task;
        }//



        private void bindVideoPlayer(bool bindShown)
        {
            if (this.lastVideoBind == bindShown)
                return;
            this.lastVideoBind = bindShown;
            this.player.Hidden = bindShown && this.Shown;
            this.showOrHideVideoButton();
        }

        private bool showOrHideVideoButton()
        {
            bool flag = false;
            if (App.IsFullScreen)
                flag = false;
            else if (Settings.MiniPlayerType != MiniPlayerType.MiniPlayer)
                flag = false;
            else if (!this.playerShown && this.Shown && this.player.Hidden && this.player.MediaRunning)
                flag = true;
            if (flag)
                this.showVideoButton();
            else
                this.hideVideoButton();
            return flag;
        }

        private void showVideoButton()
        {
            if (Settings.MiniPlayerType != MiniPlayerType.MiniPlayer)
            {
                this.hideVideoButton();
            }
            else
            {
                if (this.videoButtonShown)
                    return;
                bool flag;
                ((UIElement)this.openVideoButton).IsHitTestVisible = flag = true;
                this.videoButtonShown = flag;
                ((UIElement)this.openVideoButton).Visibility = Visibility.Visible;
                Ani.Begin((DependencyObject)this.openVideoButton, "Opacity", 1.0, 0.2);
            }
        }

        private void hideVideoButton()
        {
            if (!this.videoButtonShown)
                return;
            bool flag;
            ((UIElement)this.openVideoButton).IsHitTestVisible = flag = false;
            this.videoButtonShown = flag;
            Ani.Begin((DependencyObject)this.openVideoButton, "Opacity", 0.0, 0.2);
        }

        private void oc_ScrollingStopped(object sender, OnScrollingStoppedEventArgs e)
        {
            if (!this.setMarginsOnStopScrolling)
                return;
            this.setMarginsOnStopScrolling = false;
            this.setMargins();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.RootFrame.Content != null && this.RootFrame.Content is SearchPage)
                return;
            object parameter = (object)null;
            if (this.overCanvas != null)
                parameter = this.overCanvas.GetSearchParam();
            this.RootFrame.Navigate(typeof(SearchPage), parameter);
        }

        public void CloseBrowser()
        {
            if (this.webView == null 
                || !((ICollection<UIElement>)((Panel)this.LayoutRoot).Children).Contains((UIElement)this.webView))
                return;

            this.webView.NavigationCompleted -= this.webView_NavigationCompleted;

            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = (0.0);
            TranslateTransform Element = translateTransform;
            ((UIElement)this.webView).RenderTransform = ((Transform)Element);
            Storyboard storyboard = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.webView, 
                "Opacity", 0.0, 0.3), (Timeline)Ani.DoubleAni((DependencyObject)Element, "X", -100.0, 0.3, (EasingFunctionBase)Ani.Ease((EasingMode)1, 6.0)));

            storyboard.Completed += (s, e) =>
            {
                ((Panel)this.LayoutRoot).Children.Remove((UIElement)this.webView);
                this.webView = null;
            };

        }

        public async void OpenBrowser(string message = null)
        {
            if (message != null)
            {
                if (await new MessageDialog(message, "Message")
                    .ShowAsync(App.Strings["common.okay", "okay"].ToLower(), 
                    App.Strings["common.cancel", "cancel"].ToLower()) == 1)
                    return;
            }

            if (this.webView == null)
            {
                this.webView = new WebView();
                Grid.SetRowSpan((FrameworkElement)this.webView, 10);
                Grid.SetColumnSpan((FrameworkElement)this.webView, 10);
                WebView webView = this.webView;

                this.webView.NavigationCompleted += this.webView_NavigationCompleted;
            }

            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = 100.0;
            TranslateTransform Element = translateTransform;
            ((UIElement)this.webView).RenderTransform=(Transform)Element;
            ((UIElement)this.webView).Opacity = 0.0;
            WebView webView1 = this.webView;
            double x1 = App.VisibleBounds.X;
            Rect rect = App.WindowBounds;
            double x2 = rect.X;
            double left = x1 - x2;
            rect = App.VisibleBounds;
            double top = rect.Y - App.WindowBounds.Y;
            double right = App.WindowBounds.Right - App.VisibleBounds.Right;
            double bottom = App.WindowBounds.Bottom - App.WindowBounds.Bottom;
            Thickness thickness = new Thickness(left, top, right, bottom);
            ((FrameworkElement)webView1).Margin = thickness;
            ((ICollection<UIElement>)((Panel)this.LayoutRoot).Children).Add((UIElement)this.webView);

            Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.webView, "Opacity", 1.0, 0.2,
                (EasingFunctionBase)Ani.Ease((EasingMode)2, 1.0), 0.3), 
                (Timeline)Ani.DoubleAni((DependencyObject)Element, "X", 0.0, 0.7, 
                (EasingFunctionBase)Ani.Ease((EasingMode)0, 6.0), 0.3));

            URLConstructor urlConstructor = new URLConstructor("https://accounts.google.com/o/oauth2/auth");
            urlConstructor.SetValue("client_id", (object)YouTube.ClientID);
            urlConstructor.SetValue("redirect_uri", (object)YouTube.RedirectUri);
            urlConstructor.SetValue("scope", (object)YouTube.Scope);
            urlConstructor.SetValue("access_type", (object)"offline");
            urlConstructor.SetValue("response_type", (object)"code");
            this.webView.Navigate(urlConstructor.ToUri(UriKind.Absolute));
        }

        private async void webView_NavigationCompleted(
          WebView sender,
          WebViewNavigationCompletedEventArgs args)
        {
            string title = "";
            try
            {
                title = await this.webView.InvokeScriptAsync("eval", (IEnumerable<string>)new string[1]
                {
          "document.title"
                });
            }
            catch
            {
            }
            Helper.Write((object)("Browser Title: " + title));
            title.Contains("Permission");
            if (!title.Contains("code") || !title.Contains("="))
                return;
            string code = title.Substring(title.IndexOf('=') + 1);
            Helper.Write((object)("OAuth2.0 Code: " + code));
            this.CloseBrowser();
            try
            {
                YouTube.SignIn(code);
            }
            catch (Exception ex)
            {
                Helper.Write((object)nameof(DefaultPage), (object)("Sign in exception: " + (object)ex));
            }
        }

        private void titleGrid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!(sender is FrameworkElement) || e.PointerDeviceType != null || e.HoldingState != null)
                return;
            this.openTitleMenu(sender as FrameworkElement);
        }

        private void openTitleMenu(FrameworkElement e)
        {
            Ani.Begin((DependencyObject)this.titleGrid, "Opacity", 1.0, 0.2);
            MenuFlyout menuFlyout = new MenuFlyout();

            MenuFlyoutItem menuFlyoutItem1 = new MenuFlyoutItem();
            menuFlyoutItem1.Text = "home";
            MenuFlyoutItem menuFlyoutItem2 = menuFlyoutItem1;
            MenuFlyoutItem menuFlyoutItem3 = menuFlyoutItem2;

            menuFlyoutItem3.Click += (s, e1) =>
            {
                this.RootFrame.ClearBackStackAtNavigate();
                this.RootFrame.Navigate(typeof(HomePage));
            };

            MenuFlyoutItem menuFlyoutItem4 = new MenuFlyoutItem();
            menuFlyoutItem4.Text = "settings";

            MenuFlyoutItem menuFlyoutItem5 = menuFlyoutItem4;
            MenuFlyoutItem menuFlyoutItem6 = menuFlyoutItem5;

            menuFlyoutItem6.Click += (s, e2) 
                => ((App)Application.Current).OpenSettings();

            menuFlyout.Items.Add((MenuFlyoutItemBase)menuFlyoutItem2);
            menuFlyout.Items.Add((MenuFlyoutItemBase)menuFlyoutItem5);

            if (Settings.UserMode >= UserMode.Beta)
            {
                MenuFlyoutItem menuFlyoutItem7 = new MenuFlyoutItem();
                menuFlyoutItem7.Text = "debug";
                MenuFlyoutItem menuFlyoutItem8 = menuFlyoutItem7;
                MenuFlyoutItem menuFlyoutItem9 = menuFlyoutItem8;

                menuFlyoutItem9.Click += (s, e3) =>
                {
                    if (this.debugPanel == null)
                    {
                        this.debugPanel = new DebugInfoPanel();
                        this.debugPanel.IsHitTestVisible = false;
                        Grid.SetRowSpan(this.debugPanel, 5);
                    }
                    if (((Panel)this.LayoutRoot).Children.Contains(this.debugPanel))
                        ((Panel)this.LayoutRoot).Children.Remove(this.debugPanel);
                    else
                        ((Panel)this.LayoutRoot).Children.Add(this.debugPanel);
                };

                menuFlyout.Items.Add((MenuFlyoutItemBase)menuFlyoutItem8);
            }
          ((FlyoutBase)menuFlyout).ShowAt(e);
        }

        private void titleGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PointerDeviceType pointerDeviceType = e.PointerDeviceType;
        }

        private void titleGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (!(((RoutedEventArgs)e).OriginalSource is FrameworkElement) 
                || e.PointerDeviceType != PointerDeviceType.Mouse)
                return;

            this.openTitleMenu((FrameworkElement)this.titleGrid);
            e.Handled = true;
        }

        private void titleGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
        }

        private void player_MediaRunningChanged(object sender, MediaRunningChangedEventArgs e)
        {
            if (!e.MediaRunning)
            {
                this.player.RequestedTheme = ElementTheme.Default;
                if (!this.Shown)
                    Ani.Begin((DependencyObject)this.blackRec, "Opacity", 0.0, 0.1);
                this.showOrHideVideoButton();
            }
            else
            {
                this.player.RequestedTheme = ElementTheme.Dark;
                if (!this.Shown)
                {
                    Ani.Begin((DependencyObject)this.blackRec, "Opacity", 1.0, 0.1);
                    this.hideVideoButton();
                }
                this.showOrHideVideoButton();
            }
        }

        public async void ExitFullscreenMode()
        {
            App.IsFullScreen = false;
            if (this.overCanvas != null)
            {
                if (App.DeviceFamily == DeviceFamily.Desktop && this.mouseShownTimer != null)
                {
                    this.showLeftRight();
                    this.mouseShownTimer.Stop();
                    this.mouseShownTimer.Start();
                }
                if (this.overCanvas.Shown)
                {
                    ((UIElement)this.RootFrame).IsHitTestVisible = true;
                    Ani.Begin((DependencyObject)this.RootFrame, "Opacity", 1.0, 0.2);
                }
                else
                    ((UIElement)this.RootFrame).IsHitTestVisible = !this.player.ControlsShown;
                this.setPlayerShown(false);
                if (this.fullscreenReturnElement != null && this.fullscreenReturnElement != this)
                {
                    if (this.currentVideoTcs != null)
                    {
                        int num = await this.currentVideoTcs.Task ? 1 : 0;
                    }
                    IVideoContainer fullscreenReturnElement = this.fullscreenReturnElement;
                    this.fullscreenReturnElement = (IVideoContainer)null;
                    this.requestVideoPlayerInternal(fullscreenReturnElement, false);
                }
            }
            this.showOrHideVideoButton();
        }

        public async Task<bool> ToggleFullscreen()
        {
            if (App.IsFullScreen)
            {
                this.ExitFullscreenMode();
                return false;
            }
            bool val = App.IsFullScreen = true;
            if (val)
            {
                this.hideLeftRight();
                this.setPlayerShown(true);
                if (this.currentPlayerElement != null && this.currentPlayerElement != this)
                {
                    IVideoContainer fse = this.currentPlayerElement;
                    int num = await this.requestVideoPlayerInternal((IVideoContainer)null, false, false) ? 1 : 0;
                    this.fullscreenReturnElement = fse;
                    fse = (IVideoContainer)null;
                }
            }
            this.showOrHideVideoButton();
            return val;
        }

        public Task<bool> ShowPopup(
          Popup popup,
          Point position,
          Point transitionOffset,
          FadeType fadeType = FadeType.Almost,
          bool lightDismissed = true,
          Storyboard closeAnimation = null,
          bool hideAppBar = true)
        {
            Helper.Write((object)"Showing popup");
            if (this.popupTcs != null)
            {
                if (this.popupTcs != null)
                    Helper.Write((object)("Closing popup id: " + (object)this.popupTcs.Task.Id));
                this.popupTcs.TrySetResult(true);
                this.popupTcs = (TaskCompletionSource<bool>)null;
            }
            if (this.currentPopup != null)
            {
                Helper.Write((object)"Closing current popup before showing new one");
                this.ClosePopup();
            }
            this.popupAni = closeAnimation;
            this.popupTrans = transitionOffset;
            this.currentPopup = popup;

            this.currentPopup.HorizontalOffset = position.X;
            this.currentPopup.VerticalOffset = position.Y;
            this.currentPopup.IsLightDismissEnabled = false;

            this.easyPopupDismissal = lightDismissed;
            if ((transitionOffset.X != 0.0 || transitionOffset.Y != 0.0) && (popup.ChildTransitions == null 
                || ((ICollection<Transition>)popup.ChildTransitions).Count == 0))
            {
                Popup popup1 = popup;
                TransitionCollection transitionCollection = new TransitionCollection();
                EntranceThemeTransition entranceThemeTransition = new EntranceThemeTransition();
                entranceThemeTransition.FromHorizontalOffset = transitionOffset.X;
                entranceThemeTransition.FromVerticalOffset = transitionOffset.Y;
                ((ICollection<Transition>)transitionCollection).Add((Transition)entranceThemeTransition);
                popup1.ChildTransitions = transitionCollection;
            }
           
            Func<Point> popupArrangeMethod = DefaultPage.GetPopupArrangeMethod((DependencyObject)popup);
           
            if (popupArrangeMethod != null)
                position = popupArrangeMethod();

            this.currentPopup.HorizontalOffset = position.X;
            this.currentPopup.VerticalOffset = position.Y;
            this.currentPopup.IsOpen = true;
            if (popupArrangeMethod != null)
                position = popupArrangeMethod();
            this.currentPopup.HorizontalOffset = position.X;
            this.currentPopup.VerticalOffset= position.Y;
            ((UIElement)this).IsHitTestVisible = !lightDismissed;
            ((UIElement)this).CancelDirectManipulations();

            if (this.Shown)
                Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.RootFrame, "Opacity", 
                    (double)fadeType / (double)byte.MaxValue, 0.2), 
                    (Timeline)Ani.DoubleAni((DependencyObject)this.titleGrid, 
                    "Opacity", (double)fadeType / (double)byte.MaxValue, 0.2), 
                    (Timeline)Ani.DoubleAni((DependencyObject)this.player, 
                    "Opacity", (double)fadeType / (double)byte.MaxValue, 0.2));

            Popup currentPopup = this.currentPopup;

            currentPopup.Closed += this.currentPopup_Closed;

            this.popupTcs = new TaskCompletionSource<bool>();
            Helper.Write((object)("Showing popup id: " + (object)this.popupTcs.Task.Id));
            if (hideAppBar && this.page != null && this.page.BottomAppBar != null)
                ((UIElement)this.page.BottomAppBar).Visibility = Visibility.Collapsed;
            return this.popupTcs.Task;
        }

        public async Task WaitForPopupClose()
        {
            if (this.popupTcs == null)
                return;
            int num = await this.popupTcs.Task ? 1 : 0;
        }

        private void currentPopup_Closed(object sender, object e) => this.ClosePopup();

        private void appBarPlaceHolder_PointerEntered(object sender, PointerRoutedEventArgs e) 
            => ((UIElement)this.appBarFill).Opacity = 1.0;

        private void appBarPlaceHolder_PointerExited(object sender, PointerRoutedEventArgs e) 
            => ((UIElement)this.appBarFill).Opacity = 0.0;

        private void appBarPlaceHolder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.page == null || this.page.BottomAppBar == null)
                return;
            this.page.BottomAppBar.IsOpen = true;
        }

        private void appBarPlaceHolder_ManipulationDelta(
          object sender,
          ManipulationDeltaRoutedEventArgs e)
        {
            if (this.page == null || this.page.BottomAppBar == null 
                || e.Cumulative.Translation.Y >= -5.0 || Math.Abs(e.Cumulative.Translation.X) >= 5.0)
                return;
            this.page.BottomAppBar.IsOpen = true;
        }

        private void openVideoButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!this.Player.MediaRunning || this.Player.CurrentEntry == null)
                return;
            this.RootFrame.Navigate(typeof(VideoPage), (object)this.Player.CurrentEntry);
        }

        public Task<bool> ClosePopup()
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            Helper.Write((object)"Closing popup");
            if (this.popupTcs != null)
                Helper.Write((object)("Closing popup id: " + (object)this.popupTcs.Task.Id));
            if (this.currentPopup != null)
            {
                this.currentPopup.ChildTransitions = (TransitionCollection)null;

                this.currentPopup.Closed -= this.currentPopup_Closed;

                Popup popup = this.currentPopup;
                this.currentPopup = (Popup)null;
                if (popup.IsOpen)
                {
                    Transform transform;
                    ((UIElement)popup).RenderTransform = transform = (Transform)new TranslateTransform();
                    Transform Element = transform;
                    Storyboard ani = this.popupAni;

                    if (ani == null)
                        ani = Ani.Animation(Ani.DoubleAni((DependencyObject)popup.Child, "Opacity", 0.0, 0.1, 
                            (EasingFunctionBase)Ani.Ease((EasingMode)2, 1.0), 0.1), 
                            Ani.DoubleAni((DependencyObject)Element, "X", this.popupTrans.X, 0.2,
                            (EasingFunctionBase)Ani.Ease((EasingMode)1, 3.0)), 
                            Ani.DoubleAni((DependencyObject)Element, "Y", this.popupTrans.Y, 0.2, 
                            (EasingFunctionBase)Ani.Ease((EasingMode)1, 3.0)));
                    
                    EventHandler<object> del = (EventHandler<object>)null;
                    del = (EventHandler<object>)((sender, e) =>
                    {
                        if (popup.IsOpen)
                            popup.IsOpen = false;
                        if (this.popupTcs != null)
                            this.popupTcs.TrySetResult(true);
                        tcs.SetResult(true);

                        ((Timeline)ani).Completed -= del;
                    });
                    Storyboard storyboard = ani;
                    
                    storyboard.Completed += del;

                    ani.Begin();
                }
                else
                {
                    if (this.popupTcs != null)
                        this.popupTcs.TrySetResult(true);
                    tcs.SetResult(true);
                }
            }
            else
            {
                if (this.popupTcs != null)
                    this.popupTcs.TrySetResult(true);
                tcs.SetResult(true);
            }
            if (this.Shown)
            {
                Storyboard storyboard = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.RootFrame,
                    "Opacity", 1.0, 0.2), (Timeline)Ani.DoubleAni((DependencyObject)this.titleGrid, "Opacity", 
                    1.0, 0.2), (Timeline)Ani.DoubleAni((DependencyObject)this.player, "Opacity", 1.0, 0.2));

                storyboard.Completed += (s, e) =>
                {
                    ((UIElement)this).IsHitTestVisible = true;
                };

                if (this.page != null && this.page.BottomAppBar != null)
                    ((UIElement)this.page.BottomAppBar).Visibility = Visibility.Visible;
            }
            else
                ((UIElement)this).IsHitTestVisible = true;
            return tcs.Task;
        }

        private void player_PlayerControlsShownChanged(object sender, bool e)
        {
            if (App.IsFullScreen)
                ((UIElement)this.RootFrame).IsHitTestVisible = false;
            else if (Settings.RotationType == RotationType.System || this.lastSimpleOrientation == null)
                ((UIElement)this.RootFrame).IsHitTestVisible= !e;
            else
                ((UIElement)this.RootFrame).IsHitTestVisible = false;
        }

        private void titleGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Grid titleGrid = this.titleGrid;
            RectangleGeometry rectangleGeometry = new RectangleGeometry();
            Size newSize = e.NewSize;
            double width = newSize.Width;
            newSize = e.NewSize;
            double height = newSize.Height;
            rectangleGeometry.Rect = new Rect(0.0, 0.0, width, height);
            ((UIElement)titleGrid).Clip = rectangleGeometry;
        }

        private void LeftRightControl_LeftRightTapped(object sender, DirectionTappedEventArgs e)
        {
            if (this.overCanvas == null)
                return;
            switch (e.Direction)
            {
                case ControlDirection.Left:
                    this.overCanvas.ScrollToPage(this.overCanvas.SelectedPage - 1, false);
                    break;
                case ControlDirection.Right:
                    this.overCanvas.ScrollToPage(this.overCanvas.SelectedPage + 1, false);
                    break;
            }
        }

        public void VideoSet()
        {
        }

        public void VideoUnset()
        {
        }

        public FrameworkElement GetElement() => (FrameworkElement)this.LayoutRoot;

        public VideoDepth GetVideoDepth() => VideoDepth.Below;

        public bool GetBindVideoPlayerShown() => !App.IsFullScreen;

        public bool HasBackground() => false;

        public bool IsArrangeActive() => false;

     
        private class VisualTreeLoopHelper
        {
            public int MaxChildren;
            public int ChildIndex;
            public DependencyObject Obj;
        }

        private class DisplayClass86_0
        {
            internal DefaultPage u003E4;
            internal SimpleOrientation or;

            public DisplayClass86_0()
            {
            }
        }
    }
}



