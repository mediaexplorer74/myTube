// Decompiled with JetBrains decompiler
// Type: myTube.DefaultPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Debug;
using myTube.Helpers;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Media.Transcoding;
using Windows.Phone.UI.Input;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.AwaitableUI;

namespace myTube
{
  public sealed class DefaultPage : UserControl, IVideoContainer, IComponentConnector
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
    private OverCanvas overCanvas;
    private TranslateTransform stackPanelTrans;
    private TranslateTransform titleTrans;
    private TranslateTransform pivotTrans;
    public static readonly DependencyProperty ShownProperty = DependencyProperty.Register(nameof (Shown), typeof (bool), typeof (DefaultPage), new PropertyMetadata((object) true, new PropertyChangedCallback(DefaultPage.ShownPropertyChanged)));
    public static readonly DependencyProperty BannerReadyProperty = DependencyProperty.Register(nameof (BannerReady), typeof (bool), typeof (DefaultPage), new PropertyMetadata((object) false, new PropertyChangedCallback(DefaultPage.BannerReadyPropertyChanged)));
    public static readonly DependencyProperty PopupCloseAnimationProperty = DependencyProperty.RegisterAttached("PopupCloseAnimation", typeof (Storyboard), typeof (Popup), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PopupArrangeMethodProperty = DependencyProperty.RegisterAttached("PopupArrangeMethod", typeof (Func<Point>), typeof (Popup), new PropertyMetadata((object) null));
    public bool KeyboardControls = true;
    private LeftRightControl leftRight;
    private Popup currentPopup;
    private Page page;
    private SimpleOrientation lastSimpleOrientation;
    private RotationType rotationType = RotationType.Custom;
    private CompositeTransform trans = new CompositeTransform();
    private static DefaultPage lastDefaultPage = (DefaultPage) null;
    private const double AppBarMinimalHeight = 24.0;
    private const double AppBarCompactHeight = 57.5;
    private bool easyPopupDismissal;
    private ApplicationView appView;
    private AppBarButton appBarSearch;
    private uint videoPlayerIndex;
    private bool setMarginsOnStopScrolling;
    private bool playerShown;
    private Visibility appBarVis = (Visibility) 1;
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
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform appBarTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid LayoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState DefaultState;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState DefaultPhone;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState NarrowState;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState NarrowPhone;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState TinyState;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState TinyPhone;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState TinyUWP;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState TinyLandscapePhone;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Canvas renderingCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle blackRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoPlayer player;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CustomFrame RootFrame;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl openVideoButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid titleGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid appBarPlaceHolder;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle appBarFill;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ColumnDefinition buttonColumn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ColumnDefinition titleColumn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ColumnDefinition searchColumn;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle backgroundRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock title;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PivotHeader pivot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button backButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button searchButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CompositeTransform searchTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private SymbolIcon searchSymbol;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CompositeTransform searchSymbolTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform backTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private SymbolIcon backSymbol;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public VideoPlayer Player => this.player;

    public OverCanvas OverCanvas => this.overCanvas;

    public TimeSpan PlayerArrangeActiveTick => App.DeviceFamily == DeviceFamily.Desktop ? TimeSpan.FromSeconds(1.0 / 120.0) : TimeSpan.FromSeconds(1.0 / 30.0);

    public TimeSpan PlayerArrangeInactiveTick => App.DeviceFamily == DeviceFamily.Desktop ? TimeSpan.FromSeconds(0.5) : TimeSpan.FromSeconds(1.0);

    public static Storyboard GetPopupCloseAnimation(DependencyObject obj) => (Storyboard) obj.GetValue(DefaultPage.PopupCloseAnimationProperty);

    public static void SetPopupCloseAnimation(DependencyObject obj, Storyboard value) => obj.SetValue(DefaultPage.PopupCloseAnimationProperty, (object) value);

    public static Func<Point> GetPopupArrangeMethod(DependencyObject obj) => (Func<Point>) obj.GetValue(DefaultPage.PopupArrangeMethodProperty);

    public static void SetPopupArrangeMethod(DependencyObject obj, Func<Point> value) => obj.SetValue(DefaultPage.PopupArrangeMethodProperty, (object) value);

    private static void BannerReadyPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      DefaultPage defaultPage = d as DefaultPage;
      if ((bool) e.NewValue)
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) defaultPage.backgroundRec, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) defaultPage).Resources)[(object) "BackgroundRecOpacity"], 0.2));
      else
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) defaultPage.backgroundRec, "Opacity", 0.0, 0.2));
    }

    private static async void ShownPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      DefaultPage page = d as DefaultPage;
      bool newValue = (bool) e.NewValue;
      if (page.playerShown)
        page.player.Hidden = false;
      else if (page.lastVideoBind)
        page.player.Hidden = newValue;
      page.player.ControlsShown = false;
      if (!newValue)
      {
        ((UIElement) page.blackRec).put_Visibility((Visibility) 0);
        Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) page.titleGrid, "Opacity", 0.0, 0.25), page.player.MediaRunning ? (Timeline) Ani.DoubleAni((DependencyObject) page.blackRec, "Opacity", 1.0, 0.25, (EasingFunctionBase) Ani.Ease((EasingMode) 2, 1.0), 0.25) : (Timeline) null);
        int num;
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => num = page.Shown ? 1 : 0));
        ((UIElement) page.titleGrid).put_IsHitTestVisible(false);
      }
      else
      {
        ((UIElement) page.titleGrid).put_Visibility((Visibility) 0);
        Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) page.titleGrid, "Opacity", 1.0, 0.2), page.player.MediaRunning ? (Timeline) Ani.DoubleAni((DependencyObject) page.blackRec, "Opacity", 0.0, 0.2) : (Timeline) null);
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
        {
          if (!page.Shown)
            return;
          ((UIElement) page.blackRec).put_Visibility((Visibility) 1);
        }));
        ((UIElement) page.titleGrid).put_IsHitTestVisible(true);
      }
    }

    public bool Shown
    {
      get => (bool) ((DependencyObject) this).GetValue(DefaultPage.ShownProperty);
      set => ((DependencyObject) this).SetValue(DefaultPage.ShownProperty, (object) value);
    }

    public bool BannerReady
    {
      get => (bool) ((DependencyObject) this).GetValue(DefaultPage.BannerReadyProperty);
      set => ((DependencyObject) this).SetValue(DefaultPage.BannerReadyProperty, (object) DefaultPage.BannerReadyProperty);
    }

    public VideoPlayer VideoPlayer => this.player;

    public Popup CurrentPopup => this.currentPopup;

    public bool PopupShown => this.currentPopup != null;

    public RotationType RotationType
    {
      get => this.rotationType;
      set => this.rotationType = value;
    }

    public static DefaultPage Current => Window.Current != null && Window.Current.Content is DefaultPage ? Window.Current.Content as DefaultPage : DefaultPage.lastDefaultPage;

    public event EventHandler<BackPressedEventArgs> BackPressed;

    public CustomFrame Frame => this.RootFrame;

    public DefaultPage()
    {
      DefaultPage.lastDefaultPage = this;
      Helper.Write((object) nameof (DefaultPage), (object) "Constructor");
      this.InitializeComponent();
      Helper.Write((object) nameof (DefaultPage), (object) "InitializedComponent");
      DependencyProperty shownProperty = DefaultPage.ShownProperty;
      Binding binding1 = new Binding();
      binding1.put_Path(new PropertyPath(nameof (Shown)));
      ((FrameworkElement) this).SetBinding(shownProperty, (BindingBase) binding1);
      AppBarButton appBarButton = new AppBarButton();
      SymbolIcon symbolIcon = new SymbolIcon();
      symbolIcon.put_Symbol((Symbol) 57626);
      appBarButton.put_Icon((IconElement) symbolIcon);
      appBarButton.put_Label("search");
      this.appBarSearch = appBarButton;
      AppBarButton appBarSearch = this.appBarSearch;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) appBarSearch).add_Click), new Action<EventRegistrationToken>(((ButtonBase) appBarSearch).remove_Click), new RoutedEventHandler(this.searchButton_Click));
      this.appView = ApplicationView.GetForCurrentView();
      ((Control) this).put_FontFamily(new FontFamily("Segoe WP"));
      ApplicationView appView = this.appView;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<ApplicationView, object>>(new Func<TypedEventHandler<ApplicationView, object>, EventRegistrationToken>(appView.add_VisibleBoundsChanged), new Action<EventRegistrationToken>(appView.remove_VisibleBoundsChanged), new TypedEventHandler<ApplicationView, object>((object) this, __methodptr(view_VisibleBoundsChanged)));
      this.appView.SetDesiredBoundsMode((ApplicationViewBoundsMode) 1);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.DefaultPage_Loaded));
      this.rootTrans = new TranslateTransform();
      this.titleTrans = new TranslateTransform();
      this.pivotTrans = new TranslateTransform();
      ((UIElement) this.RootFrame).put_RenderTransform((Transform) this.rootTrans);
      Helper.Write((object) nameof (DefaultPage), (object) "Create TranslateTransforms");
      CustomFrame rootFrame1 = this.RootFrame;
      WindowsRuntimeMarshal.AddEventHandler<NavigatedEventHandler>(new Func<NavigatedEventHandler, EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) rootFrame1).add_Navigated), new Action<EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) rootFrame1).remove_Navigated), new NavigatedEventHandler(this.RootFrame_Navigated));
      CustomFrame rootFrame2 = this.RootFrame;
      WindowsRuntimeMarshal.AddEventHandler<NavigatingCancelEventHandler>(new Func<NavigatingCancelEventHandler, EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) rootFrame2).add_Navigating), new Action<EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) rootFrame2).remove_Navigating), new NavigatingCancelEventHandler(this.RootFrame_Navigating));
      this.RootFrame.NavigationCalled += new EventHandler<NavigationMode>(this.RootFrame_NavigationCalled);
      Button backButton = this.backButton;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) backButton).add_Click), new Action<EventRegistrationToken>(((ButtonBase) backButton).remove_Click), new RoutedEventHandler(this.backButton_Click));
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.DefaultPage_SizeChanged));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<BackPressedEventArgs>>(new Func<EventHandler<BackPressedEventArgs>, EventRegistrationToken>(HardwareButtons.add_BackPressed), new Action<EventRegistrationToken>(HardwareButtons.remove_BackPressed), new EventHandler<BackPressedEventArgs>(this.HardwareButtons_BackPressed));
      ((UIElement) this).put_ManipulationMode((ManipulationModes) 3);
      Helper.Write((object) nameof (DefaultPage), (object) ("Set ManipulatonMode to " + (object) ((UIElement) this).ManipulationMode));
      YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
      YouTube.SignedOut += new EventHandler<SignedOutEventArgs>(this.YouTube_SignedOut);
      WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>(new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(((UIElement) this).add_ManipulationDelta), new Action<EventRegistrationToken>(((UIElement) this).remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.DefaultPage_ManipulationDelta));
      WindowsRuntimeMarshal.AddEventHandler<ManipulationStartedEventHandler>(new Func<ManipulationStartedEventHandler, EventRegistrationToken>(((UIElement) this).add_ManipulationStarted), new Action<EventRegistrationToken>(((UIElement) this).remove_ManipulationStarted), new ManipulationStartedEventHandler(this.DefaultPage_ManipulationStarted));
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.DefaultPage_Tapped));
      WindowsRuntimeMarshal.AddEventHandler<ManipulationCompletedEventHandler>(new Func<ManipulationCompletedEventHandler, EventRegistrationToken>(((UIElement) this).add_ManipulationCompleted), new Action<EventRegistrationToken>(((UIElement) this).remove_ManipulationCompleted), new ManipulationCompletedEventHandler(this.DefaultPage_ManipulationCompleted));
      CoreWindow coreWindow1 = Window.Current.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, KeyEventArgs>>(new Func<TypedEventHandler<CoreWindow, KeyEventArgs>, EventRegistrationToken>(coreWindow1.add_KeyDown), new Action<EventRegistrationToken>(coreWindow1.remove_KeyDown), new TypedEventHandler<CoreWindow, KeyEventArgs>((object) this, __methodptr(CoreWindow_KeyDown)));
      CoreWindow coreWindow2 = Window.Current.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, KeyEventArgs>>(new Func<TypedEventHandler<CoreWindow, KeyEventArgs>, EventRegistrationToken>(coreWindow2.add_KeyUp), new Action<EventRegistrationToken>(coreWindow2.remove_KeyUp), new TypedEventHandler<CoreWindow, KeyEventArgs>((object) this, __methodptr(CoreWindow_KeyUp)));
      CoreWindow coreWindow3 = Window.Current.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, PointerEventArgs>>(new Func<TypedEventHandler<CoreWindow, PointerEventArgs>, EventRegistrationToken>(coreWindow3.add_PointerPressed), new Action<EventRegistrationToken>(coreWindow3.remove_PointerPressed), new TypedEventHandler<CoreWindow, PointerEventArgs>((object) this, __methodptr(CoreWindow_PointerPressed)));
      CoreWindow coreWindow4 = Window.Current.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, PointerEventArgs>>(new Func<TypedEventHandler<CoreWindow, PointerEventArgs>, EventRegistrationToken>(coreWindow4.add_PointerEntered), new Action<EventRegistrationToken>(coreWindow4.remove_PointerEntered), new TypedEventHandler<CoreWindow, PointerEventArgs>((object) this, __methodptr(CoreWindow_PointerEntered)));
      Window current = Window.Current;
      WindowsRuntimeMarshal.AddEventHandler<WindowSizeChangedEventHandler>(new Func<WindowSizeChangedEventHandler, EventRegistrationToken>(current.add_SizeChanged), new Action<EventRegistrationToken>(current.remove_SizeChanged), new WindowSizeChangedEventHandler(this.Current_SizeChanged));
      Helper.Write((object) nameof (DefaultPage), (object) "Created CoreWindow key and pointer events");
      this.SetTheme(Settings.Theme);
      App.GlobalObjects.VideoThumbTemplate = Settings.Thunbnail != ThumbnailStyle.Classic ? (DataTemplate) ((IDictionary<object, object>) Application.Current.Resources)[(object) "VideoThumbs2"] : (DataTemplate) ((IDictionary<object, object>) Application.Current.Resources)[(object) "VideoThumbs"];
      DependencyProperty bannerReadyProperty = DefaultPage.BannerReadyProperty;
      Binding binding2 = new Binding();
      binding2.put_Path(new PropertyPath(nameof (BannerReady)));
      binding2.put_FallbackValue((object) false);
      binding2.put_TargetNullValue((object) false);
      ((FrameworkElement) this).SetBinding(bannerReadyProperty, (BindingBase) binding2);
      DisplayInformation forCurrentView = DisplayInformation.GetForCurrentView();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<DisplayInformation, object>>(new Func<TypedEventHandler<DisplayInformation, object>, EventRegistrationToken>(forCurrentView.add_OrientationChanged), new Action<EventRegistrationToken>(forCurrentView.remove_OrientationChanged), new TypedEventHandler<DisplayInformation, object>((object) this, __methodptr(DefaultPage_OrientationChanged)));
      CustomFrame rootFrame3 = this.RootFrame;
      WindowsRuntimeMarshal.AddEventHandler<NavigatedEventHandler>(new Func<NavigatedEventHandler, EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) rootFrame3).add_Navigated), new Action<EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) rootFrame3).remove_Navigated), new NavigatedEventHandler(this.FirstNavi));
      ((UIElement) this).put_RenderTransform((Transform) this.trans);
      ((UIElement) this).put_RenderTransformOrigin(new Point(0.5, 0.5));
      Helper.Write((object) nameof (DefaultPage), (object) "Created");
      CoreDispatcher dispatcher = ((DependencyObject) this).Dispatcher;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreDispatcher, AcceleratorKeyEventArgs>>(new Func<TypedEventHandler<CoreDispatcher, AcceleratorKeyEventArgs>, EventRegistrationToken>(dispatcher.add_AcceleratorKeyActivated), new Action<EventRegistrationToken>(dispatcher.remove_AcceleratorKeyActivated), new TypedEventHandler<CoreDispatcher, AcceleratorKeyEventArgs>((object) this, __methodptr(Dispatcher_AcceleratorKeyActivated)));
      this.videoPlayerIndex = (uint) ((IList<UIElement>) ((Panel) this.LayoutRoot).Children).IndexOf((UIElement) this.player);
    }

    private void Dispatcher_AcceleratorKeyActivated(
      CoreDispatcher sender,
      AcceleratorKeyEventArgs args)
    {
      if (args.EventType != null && args.EventType != 4 || args.VirtualKey != 13 || !args.KeyStatus.IsMenuKeyDown || !this.player.MediaRunning || this.overCanvas == null)
        return;
      this.ToggleFullscreen();
      this.player.Controls.UpdateFullscreenButton();
    }

    private void view_VisibleBoundsChanged(ApplicationView sender, object args)
    {
      this.setMargins();
      if (this.overCanvas == null)
        return;
      ((UIElement) this.overCanvas).InvalidateMeasure();
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
      ((FrameworkElement) this.RootFrame).put_Margin(thickness1);
      ((FrameworkElement) this.titleGrid).put_Margin(thickness2);
    }

    protected virtual Size MeasureOverride(Size availableSize) => ((FrameworkElement) this).MeasureOverride(availableSize);

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
      WindowsRuntimeMarshal.RemoveEventHandler<NavigatedEventHandler>(new Action<EventRegistrationToken>(((Windows.UI.Xaml.Controls.Frame) this.RootFrame).remove_Navigated), new NavigatedEventHandler(this.FirstNavi));
      this.SetOrientationType();
    }

    public void SetOrientationType() => this.SetOrientationType(Settings.RotationType);

    public void SetOrientationType(RotationType type)
    {
      if (type == RotationType.Custom)
      {
        DisplayInformation.put_AutoRotationPreferences((DisplayOrientations) 2);
        this.player.Controls.ControlsState = PlayerControlsState.Compact;
      }
      else
      {
        if (type != RotationType.System)
          return;
        DisplayInformation.put_AutoRotationPreferences((DisplayOrientations) 7);
        this.player.Controls.ControlsState = PlayerControlsState.Default;
      }
    }

    private void DefaultPage_OrientationChanged(DisplayInformation sender, object args) => this.SetOrientation(sender.CurrentOrientation);

    private void DefaultPage_OrientationChanged(
      SimpleOrientationSensor sender,
      SimpleOrientationSensorOrientationChangedEventArgs args)
    {
      this.Rotate(args.Orientation);
    }

    public void LockRotation(SimpleOrientation or)
    {
      Helper.Write((object) nameof (DefaultPage), (object) ("Locking rotation to " + (object) or));
      Accel.Stop();
      this.lockRotation = or;
      this.rotationLocked = true;
      this.Rotate(or);
    }

    public void UnlockRotation()
    {
      Accel.Start();
      this.rotationLocked = false;
      Helper.Write((object) nameof (DefaultPage), (object) "Unlocking rotation");
      SimpleOrientation orient = Accel.Orient;
      if (orient == this.lockRotation)
        return;
      this.Rotate(orient);
    }

    public async void Rotate(SimpleOrientation or, bool actuallyRotate = true)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DefaultPage.\u003C\u003Ec__DisplayClass86_0 cDisplayClass860 = new DefaultPage.\u003C\u003Ec__DisplayClass86_0();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass860.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass860.or = or;
      if (this.currentPopup != null)
        return;
      if (this.rotationLocked)
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass860.or = this.lockRotation;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (Settings.RotationType != RotationType.Custom || cDisplayClass860.or != null && cDisplayClass860.or != 3 && cDisplayClass860.or != 1 || cDisplayClass860.or == this.lastSimpleOrientation)
        return;
      // ISSUE: reference to a compiler-generated field
      Helper.Write((object) nameof (DefaultPage), (object) ("Rotating to " + (object) cDisplayClass860.or));
      // ISSUE: reference to a compiler-generated field
      this.lastSimpleOrientation = cDisplayClass860.or;
      // ISSUE: method pointer
      await ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 1, new DispatchedHandler((object) cDisplayClass860, __methodptr(\u003CRotate\u003Eb__0)));
    }

    private void setPlayerShown(bool res)
    {
      this.playerShown = res;
      if (this.Shown && this.page != null && this.page.BottomAppBar != null)
      {
        if (this.lastSimpleOrientation == null && !res)
          ((UIElement) this.page.BottomAppBar).put_Visibility((Visibility) 0);
        else if (((UIElement) this.page.BottomAppBar).Visibility != 1 & res)
        {
          this.appBarVis = ((UIElement) this.page.BottomAppBar).Visibility;
          ((UIElement) this.page.BottomAppBar).put_Visibility((Visibility) 1);
        }
        else
          this.appBarVis = (Visibility) 0;
      }
      ((UIElement) this.RootFrame).put_IsHitTestVisible(!res);
      ((UIElement) this.titleGrid).put_IsHitTestVisible(!res && this.Shown);
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
          Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.RootFrame, "Opacity", opacity, 0.4), (Timeline) Ani.DoubleAni((DependencyObject) this.titleGrid, "Opacity", opacity, 0.4));
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
          {
            ((UIElement) this.blackRec).put_Visibility((Visibility) 0);
            Ani.Begin((DependencyObject) this.blackRec, "Opacity", 1.0 - opacity, 0.2);
            Grid titleGrid = this.titleGrid;
            Visibility visibility1;
            ((UIElement) this.RootFrame).put_Visibility((Visibility) (int) (visibility1 = (Visibility) 1));
            Visibility visibility2 = visibility1;
            ((UIElement) titleGrid).put_Visibility(visibility2);
          }));
        }
        else
        {
          Grid titleGrid = this.titleGrid;
          Visibility visibility3;
          ((UIElement) this.RootFrame).put_Visibility((Visibility) (int) (visibility3 = (Visibility) 0));
          Visibility visibility4 = visibility3;
          ((UIElement) titleGrid).put_Visibility(visibility4);
          Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.RootFrame, "Opacity", opacity, 0.4), (Timeline) Ani.DoubleAni((DependencyObject) this.titleGrid, "Opacity", opacity, 0.4), (Timeline) Ani.DoubleAni((DependencyObject) this.blackRec, "Opacity", 1.0 - opacity, 0.1));
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
          {
            if (!this.Shown)
              return;
            ((UIElement) this.blackRec).put_Visibility((Visibility) 1);
          }));
        }
      }
      else if (res)
      {
        Grid titleGrid = this.titleGrid;
        Visibility visibility5;
        ((UIElement) this.RootFrame).put_Visibility((Visibility) (int) (visibility5 = (Visibility) 1));
        Visibility visibility6 = visibility5;
        ((UIElement) titleGrid).put_Visibility(visibility6);
      }
      else
      {
        Grid titleGrid = this.titleGrid;
        Visibility visibility7;
        ((UIElement) this.RootFrame).put_Visibility((Visibility) (int) (visibility7 = (Visibility) 0));
        Visibility visibility8 = visibility7;
        ((UIElement) titleGrid).put_Visibility(visibility8);
      }
    }

    public async Task<RenderTargetBitmap> RenderElementAsync(
      FrameworkElement el,
      double scale,
      RenderTargetBitmap ren = null)
    {
      ((UIElement) this.renderingCanvas).put_Visibility((Visibility) 0);
      Helper.Write((object) "Rendering RenderTargetBitmap in DefaultPage");
      Helper.WriteMemory("Memory before rendering element");
      ((UIElement) el).put_Opacity(0.1);
      ((ICollection<UIElement>) ((Panel) this.renderingCanvas).Children).Add((UIElement) el);
      await el.WaitForImagesToLoad(5000);
      if (ren == null)
        ren = new RenderTargetBitmap();
      ((UIElement) el).put_Opacity(1.0);
      await ren.RenderAsync((UIElement) el, (int) (el.ActualWidth * scale), (int) (el.ActualHeight * scale));
      ((ICollection<UIElement>) ((Panel) this.renderingCanvas).Children).Remove((UIElement) el);
      Helper.WriteMemory("Memory after rendering element");
      ((UIElement) this.renderingCanvas).put_Visibility((Visibility) 1);
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
      args.put_Handled(true);
      this.GoBack();
    }

    private void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
    {
      if (args.Handled || FocusManager.GetFocusedElement() is TextBox)
        return;
      if ((args.VirtualKey == 32 || args.VirtualKey == 75) && !this.VideoPlayer.Hidden)
      {
        if (this.VideoPlayer.MediaElement.CurrentState == 3)
          this.VideoPlayer.MediaElement.Pause();
        else
          this.VideoPlayer.MediaElement.Play();
      }
      if (!this.VideoPlayer.MediaRunning || this.VideoPlayer.Hidden || this.VideoPlayer.MediaElement.CurrentState != 3 && this.VideoPlayer.MediaElement.CurrentState != 4 && this.VideoPlayer.MediaElement.CurrentState != 2)
        return;
      TimeSpan timeSpan1 = this.VideoPlayer.MediaElement.Position;
      TimeSpan timeSpan2 = this.VideoPlayer.MediaElement.NaturalDuration.TimeSpan;
      bool flag = false;
      VirtualKey virtualKey = args.VirtualKey;
      switch (virtualKey - 48)
      {
        case 0:
          timeSpan1 = TimeSpan.Zero;
          flag = true;
          break;
        case 1:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.1);
          flag = true;
          break;
        case 2:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.2);
          flag = true;
          break;
        case 3:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.3);
          flag = true;
          break;
        case 4:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.4);
          flag = true;
          break;
        case 5:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.5);
          flag = true;
          break;
        case 6:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.6);
          flag = true;
          break;
        case 7:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.7);
          flag = true;
          break;
        case 8:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.8);
          flag = true;
          break;
        case 9:
          timeSpan1 = TimeSpan.FromSeconds(timeSpan2.TotalSeconds * 0.9);
          flag = true;
          break;
        default:
          if (virtualKey != 74)
          {
            if (virtualKey == 76)
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
        this.VideoPlayer.MediaElement.put_Position(timeSpan3 = timeSpan1);
        TimeSpan timeSpan4 = timeSpan3;
        audioElement.put_Position(timeSpan4);
      }
      else
        this.VideoPlayer.MediaElement.put_Position(timeSpan1);
    }

    private async void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
    {
      if (args.KeyStatus.IsKeyReleased || args.Handled || FocusManager.GetFocusedElement() is TextBox)
        return;
      if (args.VirtualKey == 27 || args.VirtualKey == 166)
        args.put_Handled(this.GoBack());
      if (args.VirtualKey == 36 || args.VirtualKey == 172)
      {
        this.RootFrame.Navigate(typeof (HomePage));
        this.RootFrame.ClearBackStackAtNavigate();
      }
      if (this.overCanvas == null || this.movingWithDpad || App.IsFullScreen)
        return;
      if (args.VirtualKey == 37)
      {
        this.movingWithDpad = true;
        this.overCanvas.ScrollToPage(this.overCanvas.SelectedPage - 1, false);
      }
      else if (args.VirtualKey == 39)
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
      this.leftRightTimer.put_Interval(TimeSpan.FromSeconds(0.5));
      DispatcherTimer leftRightTimer = this.leftRightTimer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(leftRightTimer.add_Tick), new Action<EventRegistrationToken>(leftRightTimer.remove_Tick), new EventHandler<object>(this.leftRightTimer_Tick));
      this.leftRightFadeIn.Add((Timeline) Ani.DoubleAni((DependencyObject) this.leftRight, "Opacity", 1.0, 0.1));
      this.leftRightFadeOut.Add((Timeline) Ani.DoubleAni((DependencyObject) this.leftRight, "Opacity", 0.0, 0.1));
      Storyboard leftRightFadeOut = this.leftRightFadeOut;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) leftRightFadeOut).add_Completed), new Action<EventRegistrationToken>(((Timeline) leftRightFadeOut).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.leftRight).put_Visibility((Visibility) 1)));
    }

    protected virtual void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
    {
      if (this.overCanvas != null && !this.Shown && App.DeviceFamily == DeviceFamily.Desktop)
      {
        if (this.player.MediaRunning)
          this.ToggleFullscreen();
        else
          this.ExitFullscreenMode();
      }
      ((Control) this).OnDoubleTapped(e);
    }

    protected virtual void OnPointerMoved(PointerRoutedEventArgs e)
    {
      if ((e.Pointer.PointerDeviceType == 2 || e.Pointer.PointerDeviceType == 1) && this.leftRight != null)
      {
        if (this.leftRight == null)
          return;
        this.makeLeftRightTimer();
        PointerPoint currentPoint = e.GetCurrentPoint((UIElement) this.leftRight);
        if (currentPoint.Position.X < 380.0 || currentPoint.Position.X > ((FrameworkElement) this).ActualWidth - 380.0)
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
      if (e.Pointer.PointerDeviceType == 2)
      {
        if (!this.player.Hidden && ((FrameworkElement) this.player).ContainsPoint(e))
        {
          if (this.player.MediaRunning)
            this.player.ControlsShown = true;
          if (this.player.ControlsWatch.IsRunning)
            this.player.ControlsWatch.Restart();
          if (this.player.MediaElement.CurrentState == 3 || this.player.MediaElement.CurrentState == 4)
            this.createMouseShownTimer();
        }
        else if (!this.player.Controls.IsSeeking && this.player.ControlsShown)
        {
          Helper.Write((object) nameof (DefaultPage), (object) "Hiding player controls on OnPointerMoved event");
          this.player.ControlsShown = false;
        }
      }
      ((Control) this).OnPointerMoved(e);
    }

    private void createMouseShownTimer()
    {
      if (this.mouseShownTimer != null)
        return;
      this.mouseShownTimer = new DispatcherTimer();
      this.mouseShownTimer.put_Interval(TimeSpan.FromSeconds(3.0));
    }

    private void mouseShownTimer_Tick(object sender, object e) => this.mouseShownTimer.Stop();

    private void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
    {
    }

    private void showLeftRight()
    {
      if (this.leftRight == null || this.leftRightShown)
        return;
      ((UIElement) this.leftRight).put_Visibility((Visibility) 0);
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

    protected virtual void OnPointerEntered(PointerRoutedEventArgs e)
    {
      ((Control) this).OnPointerEntered(e);
      if (this.currentPlayerElement == null || !this.player.ControlsShown || ((FrameworkElement) this.player).ContainsPoint(e))
        return;
      this.player.ControlsShown = false;
    }

    protected virtual void OnPointerWheelChanged(PointerRoutedEventArgs e)
    {
      if (this.currentPlayerElement != null && this.currentPlayerElement != this.LayoutRoot)
        this.player.ControlsShown = false;
      ((Control) this).OnPointerWheelChanged(e);
    }

    protected virtual void OnPointerPressed(PointerRoutedEventArgs e)
    {
      ((Control) this).OnPointerPressed(e);
      if (((RoutedEventArgs) e).OriginalSource is TextBlock originalSource && originalSource.IsTextSelectionEnabled || ((RoutedEventArgs) e).OriginalSource is TextBox)
        return;
      int num1 = this.player.ControlsShown ? 1 : 0;
      PointerPoint currentPoint = e.GetCurrentPoint((UIElement) this);
      if (!this.movingOvercanvas && this.overCanvas != null && !this.player.ControlsShown && e.Pointer.PointerDeviceType == 2)
      {
        int num2 = currentPoint.Properties.IsLeftButtonPressed ? 1 : 0;
      }
      this.movingOvercanvas = false;
    }

    protected virtual void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
    {
      ((Control) this).OnManipulationDelta(e);
      if (!this.movingOvercanvas)
        return;
      this.overCanvas.Move(e.Delta.Translation.X);
    }

    protected virtual void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
    {
      ((Control) this).OnManipulationCompleted(e);
      if (!this.movingOvercanvas)
        return;
      ((UIElement) this).put_ManipulationMode((ManipulationModes) 65536);
      if (this.overCanvas != null)
        this.overCanvas.EndMove();
      this.movingOvercanvas = false;
    }

    protected virtual void OnPointerReleased(PointerRoutedEventArgs e)
    {
      ((Control) this).OnPointerReleased(e);
      MediaTranscoder mediaTranscoder = new MediaTranscoder();
      if (!this.movingOvercanvas)
        return;
      if (this.overCanvas != null)
        this.overCanvas.EndMove();
      ((UIElement) this).put_ManipulationMode((ManipulationModes) 65536);
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
      e.GetPosition((UIElement) this.player.Controls.MainGrid);
      if ((!this.player.ControlsShown || !this.player.Controls.IsTapping(e)) && (this.playerShown || ((FrameworkElement) this.player).ContainsPoint(e)))
      {
        this.player.ControlsShown = !this.player.ControlsShown;
      }
      else
      {
        if (!this.player.ControlsShown || ((FrameworkElement) this.player).ContainsPoint(e))
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
      if (Settings.MiniPlayerType == MiniPlayerType.MiniPlayer && this.player.ControlsShown && !this.player.Controls.IsSeeking && !((FrameworkElement) this.player).ContainsPoint((FrameworkElement) this, e.Position))
      {
        Helper.Write((object) nameof (DefaultPage), (object) "Hiding player controls in ManipulationDelta event");
        this.player.ControlsShown = false;
      }
      else
      {
        e.put_Handled(true);
        if (!this.player.ControlsShown)
          this.player.ControlsShown = true;
        double wid;
        Point delta;
        Point total;
        if (this.lastSimpleOrientation == 1)
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
        else if (this.lastSimpleOrientation == 3)
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

    private void YouTube_SignedOut(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) -1, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedOut\u003Eb__122_0)));

    private void YouTube_SignedIn(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) -1, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedIn\u003Eb__123_0)));

    private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
    {
      if (this.BackPressed != null)
        this.BackPressed(sender, e);
      if (e.Handled)
        return;
      e.put_Handled(this.GoBack());
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
      if (this.webView != null && ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Contains((UIElement) this.webView))
      {
        this.CloseBrowser();
        return true;
      }
      if (this.RootFrame != null)
      {
        if ((!this.VideoPlayer.Hidden || this.playerShown) && this.VideoPlayer.MediaElement != null && Settings.MiniPlayerType == MiniPlayerType.Background)
        {
          if ((this.VideoPlayer.MediaElement.CurrentState == 3 || this.VideoPlayer.MediaElement.CurrentState == 4 || this.VideoPlayer.MediaElement.CurrentState == 2) && Settings.MiniPlayerType == MiniPlayerType.Background)
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
              this.OverCanvas.ScrollToIndex(((ICollection<UIElement>) this.OverCanvas.Children).Count - 1, false, true);
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
      this.goingBack = e == 1;
      if (this.RootFrame.Animate)
      {
        Storyboard storyboard = Ani.Animation(Ani.DoubleAni((DependencyObject) this.titleTrans, "Y", this.goingBack ? 45.0 : -45.0, 0.1, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)), Ani.DoubleAni((DependencyObject) this.pivotTrans, "Y", this.goingBack ? 70.0 : -70.0, 0.075, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)), Ani.DoubleAni((DependencyObject) this.pivot, "Opacity", 0.0, 0.075), Ani.DoubleAni((DependencyObject) this.title, "Opacity", 0.0, 0.1), Ani.DoubleAni((DependencyObject) this.backgroundRec, "Opacity", 0.0, 0.1));
        this.frameNavigatingAnimation = storyboard;
        storyboard.Begin();
      }
      else
      {
        TextBlock title = this.title;
        double num1;
        ((UIElement) this.pivot).put_Opacity(num1 = 0.0);
        double num2 = num1;
        ((UIElement) title).put_Opacity(num2);
        this.titleTrans.put_Y(this.goingBack ? 15.0 : -15.0);
        this.pivotTrans.put_Y(this.goingBack ? 20.0 : -20.0);
      }
    }

    private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
    }

    private void DefaultPage_Loaded(object sender, RoutedEventArgs e)
    {
      Helper.Write((object) nameof (DefaultPage), (object) "Loaded");
      ((UIElement) this.titleGrid).put_RenderTransform((Transform) (this.stackPanelTrans = new TranslateTransform()));
      ((UIElement) this.pivot).put_RenderTransform((Transform) this.pivotTrans);
      ((UIElement) this.title).put_RenderTransform((Transform) this.titleTrans);
      Helper.Write((object) nameof (DefaultPage), (object) "Getting orientation sensor");
      Accel.Dispatcher = ((DependencyObject) this).Dispatcher;
      Accel.OrientChanged += new OrientChangedEventHandler(this.Accel_OrientChanged);
      Helper.Write((object) "Got orientation sensor");
    }

    private void Accel_OrientChanged(OrientChangedEventArgs e) => this.Rotate(e.Orientation);

    private void DefaultPage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      this.MiniPlayerTypeChanged();
      if (this.currentPopup != null)
      {
        Func<Point> popupArrangeMethod = DefaultPage.GetPopupArrangeMethod((DependencyObject) this.currentPopup);
        if (popupArrangeMethod != null)
        {
          Point point = popupArrangeMethod();
          this.currentPopup.put_HorizontalOffset(point.X);
          this.currentPopup.put_VerticalOffset(point.Y);
        }
      }
      string str = e.NewSize.Height > 450.0 ? (e.NewSize.Width <= 500.0 || e.NewSize.Height <= 500.0 ? "TinyPhone" : (e.NewSize.Width > 800.0 ? "DefaultPhone" : "NarrowPhone")) : "TinyLandscapePhone";
      if (!(str != this.lastStateName))
        return;
      VisualStateManager.GoToState((Control) this, str, false);
      this.lastStateName = str;
    }

    private void backButton_Click(object sender, RoutedEventArgs e) => ((App) Application.Current).GoBack();

    private void RootFrame_Navigated(object sender, NavigationEventArgs e)
    {
      App.CheckSignIn(45.0);
      if (((ContentControl) this.RootFrame).Content == null || !(((ContentControl) this.RootFrame).Content is FrameworkElement content))
        return;
      if (content is Page)
        this.SetPage(content as Page);
      if (content.GetType() == this.lastType)
      {
        this.fe_Loaded((object) content, (RoutedEventArgs) null);
      }
      else
      {
        this.lastType = content.GetType();
        FrameworkElement frameworkElement = content;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement.add_Loaded), new Action<EventRegistrationToken>(frameworkElement.remove_Loaded), new RoutedEventHandler(this.fe_Loaded));
      }
      if (!(content is OverCanvas))
        return;
      this.SetOverCanvas(content as OverCanvas);
    }

    private void SetPage(Page page)
    {
      if (this.page != null && this.page.BottomAppBar is CommandBar && this.page.BottomAppBar is CommandBar bottomAppBar && ((ICollection<ICommandBarElement>) bottomAppBar.PrimaryCommands).Contains((ICommandBarElement) this.appBarSearch))
        ((ICollection<ICommandBarElement>) bottomAppBar.PrimaryCommands).Remove((ICommandBarElement) this.appBarSearch);
      this.page = page;
      this.SetOrientation(DisplayInformation.GetForCurrentView().CurrentOrientation);
      this.SetSearchInAppBar(Settings.SearchInAppBar);
    }

    public void SetSearchInAppBar(bool set)
    {
      if (this.page == null | this.page.BottomAppBar == null || !(this.page.BottomAppBar is CommandBar bottomAppBar))
        return;
      if (set)
      {
        ((UIElement) this.searchButton).put_Opacity(0.0);
        ((UIElement) this.searchButton).put_IsHitTestVisible(false);
        ((UIElement) this.searchButton).put_Visibility((Visibility) 1);
        if (bottomAppBar == null || ((ICollection<ICommandBarElement>) bottomAppBar.PrimaryCommands).Contains((ICommandBarElement) this.appBarSearch))
          return;
        this.appBarSearch.put_Label(App.Strings["search.search", "search"]);
        ((ICollection<ICommandBarElement>) bottomAppBar.PrimaryCommands).Add((ICommandBarElement) this.appBarSearch);
        ((Control) this.appBarSearch).put_IsEnabled(true);
      }
      else
      {
        ((UIElement) this.searchButton).put_Opacity(1.0);
        ((UIElement) this.searchButton).put_IsHitTestVisible(true);
        ((UIElement) this.searchButton).put_Visibility((Visibility) 0);
        if (bottomAppBar == null || !((ICollection<ICommandBarElement>) bottomAppBar.PrimaryCommands).Contains((ICommandBarElement) this.appBarSearch))
          return;
        ((ICollection<ICommandBarElement>) bottomAppBar.PrimaryCommands).Remove((ICommandBarElement) this.appBarSearch);
      }
    }

    public void SetTheme(ElementTheme theme) => ((FrameworkElement) this).put_RequestedTheme(theme);

    private void setAppBarPlaceholder(bool open)
    {
      if (open)
      {
        this.appBarTrans.put_Y(((FrameworkElement) this.appBarPlaceHolder).ActualHeight);
        ((UIElement) this.appBarPlaceHolder).put_Visibility((Visibility) 0);
        Ani.Begin((DependencyObject) this.appBarTrans, "Y", 0.0, 0.4, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0));
      }
      else
      {
        if (((UIElement) this.appBarPlaceHolder).Visibility != null)
          return;
        Storyboard storyboard = Ani.Begin((DependencyObject) this.appBarTrans, "Y", ((FrameworkElement) this.appBarPlaceHolder).ActualHeight, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 4.0));
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.appBarPlaceHolder).put_Visibility((Visibility) 1)));
      }
    }

    private async void fe_Loaded(object sender, RoutedEventArgs e)
    {
      if (sender is FrameworkElement fe)
      {
        this.iterations = 0;
        OverCanvas oc = await this.SearchForOverCanvas((DependencyObject) fe);
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(fe.remove_Loaded), new RoutedEventHandler(this.fe_Loaded));
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
        return (OverCanvas) null;
      for (int i = 0; i < numChildren; ++i)
      {
        ++this.iterations;
        DependencyObject child = VisualTreeHelper.GetChild(obj, i);
        if (child is OverCanvas)
          return child as OverCanvas;
        if (this.iterations > 100)
          return (OverCanvas) null;
        OverCanvas overCanvas = await this.SearchForOverCanvas(child);
        if (overCanvas != null)
          return overCanvas;
      }
      return (OverCanvas) null;
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
      ((FrameworkElement) this).put_DataContext((object) oc);
      if (oc != null)
      {
        PivotHeader pivot1 = this.pivot;
        DependencyProperty stringsProperty = PivotHeader.StringsProperty;
        Binding binding1 = new Binding();
        binding1.put_Path(new PropertyPath("PageTitles"));
        ((FrameworkElement) pivot1).SetBinding(stringsProperty, (BindingBase) binding1);
        PivotHeader pivot2 = this.pivot;
        DependencyProperty indexProperty = PivotHeader.IndexProperty;
        Binding binding2 = new Binding();
        binding2.put_Path(new PropertyPath("SelectedPage"));
        ((FrameworkElement) pivot2).SetBinding(indexProperty, (BindingBase) binding2);
        this.pivot.OverCanvas = oc;
      }
      else
        this.title.put_Text("");
      double num = 60.0;
      double Duration = 1.1;
      if (this.frameNavigatingAnimation != null)
        this.frameNavigatingAnimation.Stop();
      this.pivotTrans.put_Y(this.goingBack ? -num : num);
      this.titleTrans.put_Y(this.goingBack ? -num * 0.677 : num * 0.677);
      Storyboard sb = Ani.Animation(Ani.DoubleAni((DependencyObject) this.pivotTrans, "Y", 0.0, Duration, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 7.0)), Ani.DoubleAni((DependencyObject) this.titleTrans, "Y", 0.0, Duration * 0.667, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)), Ani.DoubleAni((DependencyObject) this.title, "Opacity", 1.0, 0.2), Ani.DoubleAni((DependencyObject) this.pivot, "Opacity", 1.0, 0.2));
      if (((oc == null ? 0 : (oc.BannerReady ? 1 : 0)) & (bannerReady ? 1 : 0)) != 0)
        sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this.backgroundRec, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "BackgroundRecOpacity"], 0.2));
      if (!this.RootFrame.Animate)
        ((Timeline) sb).put_SpeedRatio(2.0);
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
          int num1 = await this.requestVideoPlayerInternal((IVideoContainer) null, true) ? 1 : 0;
          this.bindVideoPlayer(true);
          this.reservedVideoElement = temp;
          temp = (IVideoContainer) null;
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
        ((Panel) this.LayoutRoot).Children.Move((uint) ((IList<UIElement>) ((Panel) this.LayoutRoot).Children).IndexOf((UIElement) this.player), this.videoPlayerIndex);
        if (this.playerArrangeTimer != null)
          this.playerArrangeTimer.Stop();
        this.currentPlayerElement = (IVideoContainer) null;
        if (this.fullscreenReturnElement != null)
        {
          if (callEvent && this.fullscreenReturnElement != null)
            this.fullscreenReturnElement.VideoUnset();
          this.fullscreenReturnElement = (IVideoContainer) null;
        }
        VideoPlayer player = this.player;
        double num1;
        ((FrameworkElement) this.player).put_Height(num1 = double.NaN);
        double num2 = num1;
        ((FrameworkElement) player).put_Width(num2);
        ((FrameworkElement) this.player).put_HorizontalAlignment((HorizontalAlignment) 3);
        ((FrameworkElement) this.player).put_VerticalAlignment((VerticalAlignment) 3);
        if (this.playerTrans != null)
        {
          TranslateTransform playerTrans = this.playerTrans;
          double num3;
          this.playerTrans.put_Y(num3 = 0.0);
          double num4 = num3;
          playerTrans.put_X(num4);
        }
        ((Control) this.player).put_Background((Brush) null);
      }
      else
      {
        if (this.playerArrangeTimer == null)
        {
          this.playerArrangeTimer = new DispatcherTimer();
          this.playerArrangeTimer.put_Interval(TimeSpan.FromSeconds(1.0 / 120.0));
          DispatcherTimer playerArrangeTimer = this.playerArrangeTimer;
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(playerArrangeTimer.add_Tick), new Action<EventRegistrationToken>(playerArrangeTimer.remove_Tick), new EventHandler<object>(this.PlayerArrangeTimer_Tick));
        }
        if (this.playerTrans == null)
        {
          this.playerTrans = new TranslateTransform();
          ((UIElement) this.player).put_RenderTransform((Transform) this.playerTrans);
        }
        ((Control) this.player).put_Background((Brush) new SolidColorBrush(Colors.Black));
        this.currentPlayerElement = parent;
        if (this.fullscreenReturnElement != null)
          this.fullscreenReturnElement = parent;
        ((FrameworkElement) this.player).put_HorizontalAlignment((HorizontalAlignment) 0);
        ((FrameworkElement) this.player).put_VerticalAlignment((VerticalAlignment) 0);
        this.PlayerArrangeTimer_Tick((object) null, (object) null);
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
          uint num1 = (uint) (((IList<UIElement>) ((Panel) this.LayoutRoot).Children).IndexOf((UIElement) this.RootFrame) + 1);
          uint num2 = (uint) ((IList<UIElement>) ((Panel) this.LayoutRoot).Children).IndexOf((UIElement) this.player);
          if ((int) num1 != (int) num2)
            ((Panel) this.LayoutRoot).Children.Move(num2, num1);
        }
        else
        {
          uint num = (uint) ((IList<UIElement>) ((Panel) this.LayoutRoot).Children).IndexOf((UIElement) this.player);
          if ((int) num != (int) this.videoPlayerIndex)
            ((Panel) this.LayoutRoot).Children.Move(num, this.videoPlayerIndex);
        }
        Point position = ((UIElement) this.currentPlayerElement.GetElement()).GetPosition((UIElement) this);
        this.playerTrans.put_Y(position.Y);
        this.playerTrans.put_X(position.X);
        if (((FrameworkElement) this.player).Width != this.currentPlayerElement.GetElement().ActualWidth)
          ((FrameworkElement) this.player).put_Width(this.currentPlayerElement.GetElement().ActualWidth);
        if (((FrameworkElement) this.player).Height != this.currentPlayerElement.GetElement().ActualHeight)
          ((FrameworkElement) this.player).put_Height(this.currentPlayerElement.GetElement().ActualHeight);
        this.bindVideoPlayer(this.currentPlayerElement.GetBindVideoPlayerShown());
        if (this.currentPlayerElement.HasBackground())
        {
          if (((Control) this.player).Background == null)
            ((Control) this.player).put_Background((Brush) new SolidColorBrush(Colors.Black));
        }
        else if (((Control) this.player).Background != null)
          ((Control) this.player).put_Background((Brush) null);
        if (this.currentPlayerElement.IsArrangeActive())
          this.playerArrangeTimer.put_Interval(this.PlayerArrangeActiveTick);
        else
          this.playerArrangeTimer.put_Interval(this.PlayerArrangeInactiveTick);
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
      Helper.Write((object) nameof (DefaultPage), (object) "Resetting video player");
      return await this.requestVideoPlayerInternal((IVideoContainer) this, true);
    }

    private Task<bool> requestVideoPlayerInternal(
      IVideoContainer element,
      bool bindShown,
      bool callEvent = true)
    {
      Helper.Write((object) nameof (DefaultPage), (object) ("Requesting video player control on " + (object) element ?? "NULL"));
      TaskCompletionSource<bool> tcs;
      if (Settings.MiniPlayerType != MiniPlayerType.MiniPlayer && this.currentPlayerElement == null)
      {
        this.reservedVideoElement = element;
        tcs = new TaskCompletionSource<bool>();
        tcs.SetResult(false);
        return tcs.Task;
      }
      if (App.IsFullScreen && element != null && element != this)
      {
        Helper.Write((object) nameof (DefaultPage), (object) "VideoPlayer currently in fullscreen, exiting method");
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
        this.waitingVideoTcs = (TaskCompletionSource<bool>) null;
      }
      else
        tcs = new TaskCompletionSource<bool>();
      if (this.currentPlayerElement == null && element == null || this.currentPlayerElement == this && element == this)
      {
        Helper.Write((object) nameof (DefaultPage), (object) "Video player has already been reset, exiting method");
        tcs.TrySetResult(true);
        return tcs.Task;
      }
      if (element == this.currentPlayerElement && (!App.IsFullScreen || this.fullscreenReturnElement == element))
      {
        Helper.Write((object) nameof (DefaultPage), (object) "Element already set as video player control, exiting method");
        tcs.SetResult(true);
        this.showOrHideVideoButton();
        return tcs.Task;
      }
      if (this.busyPlacingVideo)
      {
        Helper.Write((object) nameof (DefaultPage), (object) "Already setting other element as video player control, this element will be placed afterwords. Exiting method.");
        this.waitingBind = bindShown;
        this.waitingVideoElement = element;
        if (this.waitingVideoTcs != null)
          this.waitingVideoTcs.TrySetResult(false);
        this.waitingVideoTcs = this.waitingVideoTcs != tcs ? tcs : new TaskCompletionSource<bool>();
        this.showOrHideVideoButton();
        return tcs.Task;
      }
      this.busyPlacingVideo = true;
      this.currentVideoTcs = tcs;
      if (this.playerArrangeTimer != null)
        this.playerArrangeTimer.Stop();
      this.removePlayerFromParent(callEvent);
      ((UIElement) this.player).put_Opacity(0.0);
      this.addPlayerToParent(element, callEvent);
      Storyboard storyboard = Ani.Begin((DependencyObject) this.player, "Opacity", 1.0, 0.5);
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
      {
        this.busyPlacingVideo = false;
        if (this.waitingVideoElement != null)
        {
          this.requestVideoPlayerInternal(this.waitingVideoElement, this.waitingBind);
          this.waitingVideoElement = (IVideoContainer) null;
        }
        tcs.SetResult(((FrameworkElement) this.player).Parent == element);
        this.showOrHideVideoButton();
      }));
      return tcs.Task;
    }

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
        ((UIElement) this.openVideoButton).put_IsHitTestVisible(flag = true);
        this.videoButtonShown = flag;
        ((UIElement) this.openVideoButton).put_Visibility((Visibility) 0);
        Ani.Begin((DependencyObject) this.openVideoButton, "Opacity", 1.0, 0.2);
      }
    }

    private void hideVideoButton()
    {
      if (!this.videoButtonShown)
        return;
      bool flag;
      ((UIElement) this.openVideoButton).put_IsHitTestVisible(flag = false);
      this.videoButtonShown = flag;
      Ani.Begin((DependencyObject) this.openVideoButton, "Opacity", 0.0, 0.2);
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
      if (((ContentControl) this.RootFrame).Content != null && ((ContentControl) this.RootFrame).Content is SearchPage)
        return;
      object parameter = (object) null;
      if (this.overCanvas != null)
        parameter = this.overCanvas.GetSearchParam();
      this.RootFrame.Navigate(typeof (SearchPage), parameter);
    }

    public void CloseBrowser()
    {
      if (this.webView == null || !((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Contains((UIElement) this.webView))
        return;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>>(new Action<EventRegistrationToken>(this.webView.remove_NavigationCompleted), new TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>((object) this, __methodptr(webView_NavigationCompleted)));
      TranslateTransform translateTransform = new TranslateTransform();
      translateTransform.put_X(0.0);
      TranslateTransform Element = translateTransform;
      ((UIElement) this.webView).put_RenderTransform((Transform) Element);
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.webView, "Opacity", 0.0, 0.3), (Timeline) Ani.DoubleAni((DependencyObject) Element, "X", -100.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 6.0)));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
      {
        ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.webView);
        this.webView = (WebView) null;
      }));
    }

    public async void OpenBrowser(string message = null)
    {
      if (message != null)
      {
        if (await new MessageDialog(message, "Message").ShowAsync(App.Strings["common.okay", "okay"].ToLower(), App.Strings["common.cancel", "cancel"].ToLower()) == 1)
          return;
      }
      if (this.webView == null)
      {
        this.webView = new WebView();
        Grid.SetRowSpan((FrameworkElement) this.webView, 10);
        Grid.SetColumnSpan((FrameworkElement) this.webView, 10);
        WebView webView = this.webView;
        // ISSUE: method pointer
        WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>>(new Func<TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>, EventRegistrationToken>(webView.add_NavigationCompleted), new Action<EventRegistrationToken>(webView.remove_NavigationCompleted), new TypedEventHandler<WebView, WebViewNavigationCompletedEventArgs>((object) this, __methodptr(webView_NavigationCompleted)));
      }
      TranslateTransform translateTransform = new TranslateTransform();
      translateTransform.put_X(100.0);
      TranslateTransform Element = translateTransform;
      ((UIElement) this.webView).put_RenderTransform((Transform) Element);
      ((UIElement) this.webView).put_Opacity(0.0);
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
      ((FrameworkElement) webView1).put_Margin(thickness);
      ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Add((UIElement) this.webView);
      Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.webView, "Opacity", 1.0, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 2, 1.0), 0.3), (Timeline) Ani.DoubleAni((DependencyObject) Element, "X", 0.0, 0.7, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 6.0), 0.3));
      URLConstructor urlConstructor = new URLConstructor("https://accounts.google.com/o/oauth2/auth");
      urlConstructor.SetValue("client_id", (object) YouTube.ClientID);
      urlConstructor.SetValue("redirect_uri", (object) YouTube.RedirectUri);
      urlConstructor.SetValue("scope", (object) YouTube.Scope);
      urlConstructor.SetValue("access_type", (object) "offline");
      urlConstructor.SetValue("response_type", (object) "code");
      this.webView.Navigate(urlConstructor.ToUri(UriKind.Absolute));
    }

    private async void webView_NavigationCompleted(
      WebView sender,
      WebViewNavigationCompletedEventArgs args)
    {
      string title = "";
      try
      {
        title = await this.webView.InvokeScriptAsync("eval", (IEnumerable<string>) new string[1]
        {
          "document.title"
        });
      }
      catch
      {
      }
      Helper.Write((object) ("Browser Title: " + title));
      title.Contains("Permission");
      if (!title.Contains("code") || !title.Contains("="))
        return;
      string code = title.Substring(title.IndexOf('=') + 1);
      Helper.Write((object) ("OAuth2.0 Code: " + code));
      this.CloseBrowser();
      try
      {
        YouTube.SignIn(code);
      }
      catch (Exception ex)
      {
        Helper.Write((object) nameof (DefaultPage), (object) ("Sign in exception: " + (object) ex));
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
      Ani.Begin((DependencyObject) this.titleGrid, "Opacity", 1.0, 0.2);
      MenuFlyout menuFlyout = new MenuFlyout();
      MenuFlyoutItem menuFlyoutItem1 = new MenuFlyoutItem();
      menuFlyoutItem1.put_Text("home");
      MenuFlyoutItem menuFlyoutItem2 = menuFlyoutItem1;
      MenuFlyoutItem menuFlyoutItem3 = menuFlyoutItem2;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(menuFlyoutItem3.add_Click), new Action<EventRegistrationToken>(menuFlyoutItem3.remove_Click), (RoutedEventHandler) ((s, args) =>
      {
        this.RootFrame.ClearBackStackAtNavigate();
        this.RootFrame.Navigate(typeof (HomePage));
      }));
      MenuFlyoutItem menuFlyoutItem4 = new MenuFlyoutItem();
      menuFlyoutItem4.put_Text("settings");
      MenuFlyoutItem menuFlyoutItem5 = menuFlyoutItem4;
      MenuFlyoutItem menuFlyoutItem6 = menuFlyoutItem5;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(menuFlyoutItem6.add_Click), new Action<EventRegistrationToken>(menuFlyoutItem6.remove_Click), (RoutedEventHandler) ((s, args) => ((App) Application.Current).OpenSettings()));
      menuFlyout.Items.Add((MenuFlyoutItemBase) menuFlyoutItem2);
      menuFlyout.Items.Add((MenuFlyoutItemBase) menuFlyoutItem5);
      if (Settings.UserMode >= UserMode.Beta)
      {
        MenuFlyoutItem menuFlyoutItem7 = new MenuFlyoutItem();
        menuFlyoutItem7.put_Text("debug");
        MenuFlyoutItem menuFlyoutItem8 = menuFlyoutItem7;
        MenuFlyoutItem menuFlyoutItem9 = menuFlyoutItem8;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(menuFlyoutItem9.add_Click), new Action<EventRegistrationToken>(menuFlyoutItem9.remove_Click), (RoutedEventHandler) ((s, args) =>
        {
          if (this.debugPanel == null)
          {
            DebugInfoPanel debugInfoPanel = new DebugInfoPanel();
            ((UIElement) debugInfoPanel).put_IsHitTestVisible(false);
            this.debugPanel = debugInfoPanel;
            Grid.SetRowSpan((FrameworkElement) this.debugPanel, 5);
          }
          if (((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Contains((UIElement) this.debugPanel))
            ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Remove((UIElement) this.debugPanel);
          else
            ((ICollection<UIElement>) ((Panel) this.LayoutRoot).Children).Add((UIElement) this.debugPanel);
        }));
        menuFlyout.Items.Add((MenuFlyoutItemBase) menuFlyoutItem8);
      }
      ((FlyoutBase) menuFlyout).ShowAt(e);
    }

    private void titleGrid_Tapped(object sender, TappedRoutedEventArgs e)
    {
      PointerDeviceType pointerDeviceType = e.PointerDeviceType;
    }

    private void titleGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
      if (!(((RoutedEventArgs) e).OriginalSource is FrameworkElement) || e.PointerDeviceType != 2)
        return;
      this.openTitleMenu((FrameworkElement) this.titleGrid);
      e.put_Handled(true);
    }

    private void titleGrid_PointerExited(object sender, PointerRoutedEventArgs e)
    {
    }

    private void player_MediaRunningChanged(object sender, MediaRunningChangedEventArgs e)
    {
      if (!e.MediaRunning)
      {
        ((FrameworkElement) this.player).put_RequestedTheme((ElementTheme) 0);
        if (!this.Shown)
          Ani.Begin((DependencyObject) this.blackRec, "Opacity", 0.0, 0.1);
        this.showOrHideVideoButton();
      }
      else
      {
        ((FrameworkElement) this.player).put_RequestedTheme((ElementTheme) 2);
        if (!this.Shown)
        {
          Ani.Begin((DependencyObject) this.blackRec, "Opacity", 1.0, 0.1);
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
          ((UIElement) this.RootFrame).put_IsHitTestVisible(true);
          Ani.Begin((DependencyObject) this.RootFrame, "Opacity", 1.0, 0.2);
        }
        else
          ((UIElement) this.RootFrame).put_IsHitTestVisible(!this.player.ControlsShown);
        this.setPlayerShown(false);
        if (this.fullscreenReturnElement != null && this.fullscreenReturnElement != this)
        {
          if (this.currentVideoTcs != null)
          {
            int num = await this.currentVideoTcs.Task ? 1 : 0;
          }
          IVideoContainer fullscreenReturnElement = this.fullscreenReturnElement;
          this.fullscreenReturnElement = (IVideoContainer) null;
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
          int num = await this.requestVideoPlayerInternal((IVideoContainer) null, false, false) ? 1 : 0;
          this.fullscreenReturnElement = fse;
          fse = (IVideoContainer) null;
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
      Helper.Write((object) "Showing popup");
      if (this.popupTcs != null)
      {
        if (this.popupTcs != null)
          Helper.Write((object) ("Closing popup id: " + (object) this.popupTcs.Task.Id));
        this.popupTcs.TrySetResult(true);
        this.popupTcs = (TaskCompletionSource<bool>) null;
      }
      if (this.currentPopup != null)
      {
        Helper.Write((object) "Closing current popup before showing new one");
        this.ClosePopup();
      }
      this.popupAni = closeAnimation;
      this.popupTrans = transitionOffset;
      this.currentPopup = popup;
      this.currentPopup.put_HorizontalOffset(position.X);
      this.currentPopup.put_VerticalOffset(position.Y);
      this.currentPopup.put_IsLightDismissEnabled(false);
      this.easyPopupDismissal = lightDismissed;
      if ((transitionOffset.X != 0.0 || transitionOffset.Y != 0.0) && (popup.ChildTransitions == null || ((ICollection<Transition>) popup.ChildTransitions).Count == 0))
      {
        Popup popup1 = popup;
        TransitionCollection transitionCollection = new TransitionCollection();
        EntranceThemeTransition entranceThemeTransition = new EntranceThemeTransition();
        entranceThemeTransition.put_FromHorizontalOffset(transitionOffset.X);
        entranceThemeTransition.put_FromVerticalOffset(transitionOffset.Y);
        ((ICollection<Transition>) transitionCollection).Add((Transition) entranceThemeTransition);
        popup1.put_ChildTransitions(transitionCollection);
      }
      Func<Point> popupArrangeMethod = DefaultPage.GetPopupArrangeMethod((DependencyObject) popup);
      if (popupArrangeMethod != null)
        position = popupArrangeMethod();
      this.currentPopup.put_HorizontalOffset(position.X);
      this.currentPopup.put_VerticalOffset(position.Y);
      this.currentPopup.put_IsOpen(true);
      if (popupArrangeMethod != null)
        position = popupArrangeMethod();
      this.currentPopup.put_HorizontalOffset(position.X);
      this.currentPopup.put_VerticalOffset(position.Y);
      ((UIElement) this).put_IsHitTestVisible(!lightDismissed);
      ((UIElement) this).CancelDirectManipulations();
      if (this.Shown)
        Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.RootFrame, "Opacity", (double) fadeType / (double) byte.MaxValue, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.titleGrid, "Opacity", (double) fadeType / (double) byte.MaxValue, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.player, "Opacity", (double) fadeType / (double) byte.MaxValue, 0.2));
      Popup currentPopup = this.currentPopup;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(currentPopup.add_Closed), new Action<EventRegistrationToken>(currentPopup.remove_Closed), new EventHandler<object>(this.currentPopup_Closed));
      this.popupTcs = new TaskCompletionSource<bool>();
      Helper.Write((object) ("Showing popup id: " + (object) this.popupTcs.Task.Id));
      if (hideAppBar && this.page != null && this.page.BottomAppBar != null)
        ((UIElement) this.page.BottomAppBar).put_Visibility((Visibility) 1);
      return this.popupTcs.Task;
    }

    public async Task WaitForPopupClose()
    {
      if (this.popupTcs == null)
        return;
      int num = await this.popupTcs.Task ? 1 : 0;
    }

    private void currentPopup_Closed(object sender, object e) => this.ClosePopup();

    private void appBarPlaceHolder_PointerEntered(object sender, PointerRoutedEventArgs e) => ((UIElement) this.appBarFill).put_Opacity(1.0);

    private void appBarPlaceHolder_PointerExited(object sender, PointerRoutedEventArgs e) => ((UIElement) this.appBarFill).put_Opacity(0.0);

    private void appBarPlaceHolder_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.page == null || this.page.BottomAppBar == null)
        return;
      this.page.BottomAppBar.put_IsOpen(true);
    }

    private void appBarPlaceHolder_ManipulationDelta(
      object sender,
      ManipulationDeltaRoutedEventArgs e)
    {
      if (this.page == null || this.page.BottomAppBar == null || e.Cumulative.Translation.Y >= -5.0 || Math.Abs(e.Cumulative.Translation.X) >= 5.0)
        return;
      this.page.BottomAppBar.put_IsOpen(true);
    }

    private void openVideoButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!this.Player.MediaRunning || this.Player.CurrentEntry == null)
        return;
      this.RootFrame.Navigate(typeof (VideoPage), (object) this.Player.CurrentEntry);
    }

    public Task<bool> ClosePopup()
    {
      TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
      Helper.Write((object) "Closing popup");
      if (this.popupTcs != null)
        Helper.Write((object) ("Closing popup id: " + (object) this.popupTcs.Task.Id));
      if (this.currentPopup != null)
      {
        this.currentPopup.put_ChildTransitions((TransitionCollection) null);
        WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>(new Action<EventRegistrationToken>(this.currentPopup.remove_Closed), new EventHandler<object>(this.currentPopup_Closed));
        Popup popup = this.currentPopup;
        this.currentPopup = (Popup) null;
        if (popup.IsOpen)
        {
          Transform transform;
          ((UIElement) popup).put_RenderTransform(transform = (Transform) new TranslateTransform());
          Transform Element = transform;
          Storyboard ani = this.popupAni;
          if (ani == null)
            ani = Ani.Animation(Ani.DoubleAni((DependencyObject) popup.Child, "Opacity", 0.0, 0.1, (EasingFunctionBase) Ani.Ease((EasingMode) 2, 1.0), 0.1), Ani.DoubleAni((DependencyObject) Element, "X", this.popupTrans.X, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)), Ani.DoubleAni((DependencyObject) Element, "Y", this.popupTrans.Y, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)));
          EventHandler<object> del = (EventHandler<object>) null;
          del = (EventHandler<object>) ((sender, e) =>
          {
            if (popup.IsOpen)
              popup.put_IsOpen(false);
            if (this.popupTcs != null)
              this.popupTcs.TrySetResult(true);
            tcs.SetResult(true);
            WindowsRuntimeMarshal.RemoveEventHandler<EventHandler<object>>(new Action<EventRegistrationToken>(((Timeline) ani).remove_Completed), del);
          });
          Storyboard storyboard = ani;
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), del);
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
        Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.RootFrame, "Opacity", 1.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.titleGrid, "Opacity", 1.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.player, "Opacity", 1.0, 0.2));
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this).put_IsHitTestVisible(true)));
        if (this.page != null && this.page.BottomAppBar != null)
          ((UIElement) this.page.BottomAppBar).put_Visibility((Visibility) 0);
      }
      else
        ((UIElement) this).put_IsHitTestVisible(true);
      return tcs.Task;
    }

    private void player_PlayerControlsShownChanged(object sender, bool e)
    {
      if (App.IsFullScreen)
        ((UIElement) this.RootFrame).put_IsHitTestVisible(false);
      else if (Settings.RotationType == RotationType.System || this.lastSimpleOrientation == null)
        ((UIElement) this.RootFrame).put_IsHitTestVisible(!e);
      else
        ((UIElement) this.RootFrame).put_IsHitTestVisible(false);
    }

    private void titleGrid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      Grid titleGrid = this.titleGrid;
      RectangleGeometry rectangleGeometry = new RectangleGeometry();
      Size newSize = e.NewSize;
      double width = newSize.Width;
      newSize = e.NewSize;
      double height = newSize.Height;
      rectangleGeometry.put_Rect(new Rect(0.0, 0.0, width, height));
      ((UIElement) titleGrid).put_Clip(rectangleGeometry);
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

    public FrameworkElement GetElement() => (FrameworkElement) this.LayoutRoot;

    public VideoDepth GetVideoDepth() => VideoDepth.Below;

    public bool GetBindVideoPlayerShown() => !App.IsFullScreen;

    public bool HasBackground() => false;

    public bool IsArrangeActive() => false;

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///DefaultPage.xaml"), (ComponentResourceLocation) 0);
      this.appBarTrans = (TranslateTransform) ((FrameworkElement) this).FindName("appBarTrans");
      this.LayoutRoot = (Grid) ((FrameworkElement) this).FindName("LayoutRoot");
      this.DefaultState = (VisualState) ((FrameworkElement) this).FindName("DefaultState");
      this.DefaultPhone = (VisualState) ((FrameworkElement) this).FindName("DefaultPhone");
      this.NarrowState = (VisualState) ((FrameworkElement) this).FindName("NarrowState");
      this.NarrowPhone = (VisualState) ((FrameworkElement) this).FindName("NarrowPhone");
      this.TinyState = (VisualState) ((FrameworkElement) this).FindName("TinyState");
      this.TinyPhone = (VisualState) ((FrameworkElement) this).FindName("TinyPhone");
      this.TinyUWP = (VisualState) ((FrameworkElement) this).FindName("TinyUWP");
      this.TinyLandscapePhone = (VisualState) ((FrameworkElement) this).FindName("TinyLandscapePhone");
      this.renderingCanvas = (Canvas) ((FrameworkElement) this).FindName("renderingCanvas");
      this.blackRec = (Rectangle) ((FrameworkElement) this).FindName("blackRec");
      this.player = (VideoPlayer) ((FrameworkElement) this).FindName("player");
      this.RootFrame = (CustomFrame) ((FrameworkElement) this).FindName("RootFrame");
      this.openVideoButton = (ContentControl) ((FrameworkElement) this).FindName("openVideoButton");
      this.titleGrid = (Grid) ((FrameworkElement) this).FindName("titleGrid");
      this.appBarPlaceHolder = (Grid) ((FrameworkElement) this).FindName("appBarPlaceHolder");
      this.appBarFill = (Rectangle) ((FrameworkElement) this).FindName("appBarFill");
      this.buttonColumn = (ColumnDefinition) ((FrameworkElement) this).FindName("buttonColumn");
      this.titleColumn = (ColumnDefinition) ((FrameworkElement) this).FindName("titleColumn");
      this.searchColumn = (ColumnDefinition) ((FrameworkElement) this).FindName("searchColumn");
      this.backgroundRec = (Rectangle) ((FrameworkElement) this).FindName("backgroundRec");
      this.title = (TextBlock) ((FrameworkElement) this).FindName("title");
      this.pivot = (PivotHeader) ((FrameworkElement) this).FindName("pivot");
      this.backButton = (Button) ((FrameworkElement) this).FindName("backButton");
      this.searchButton = (Button) ((FrameworkElement) this).FindName("searchButton");
      this.searchTrans = (CompositeTransform) ((FrameworkElement) this).FindName("searchTrans");
      this.searchSymbol = (SymbolIcon) ((FrameworkElement) this).FindName("searchSymbol");
      this.searchSymbolTrans = (CompositeTransform) ((FrameworkElement) this).FindName("searchSymbolTrans");
      this.backTrans = (TranslateTransform) ((FrameworkElement) this).FindName("backTrans");
      this.backSymbol = (SymbolIcon) ((FrameworkElement) this).FindName("backSymbol");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((VideoPlayer) target).MediaRunningChanged += new EventHandler<MediaRunningChangedEventArgs>(this.player_MediaRunningChanged);
          ((VideoPlayer) target).PlayerControlsShownChanged += new EventHandler<bool>(this.player_PlayerControlsShownChanged);
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.openVideoButton_Tapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(uiElement2.add_Holding), new Action<EventRegistrationToken>(uiElement2.remove_Holding), new HoldingEventHandler(this.titleGrid_Holding));
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.titleGrid_Tapped));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement4.add_RightTapped), new Action<EventRegistrationToken>(uiElement4.remove_RightTapped), new RightTappedEventHandler(this.titleGrid_RightTapped));
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement5.add_PointerExited), new Action<EventRegistrationToken>(uiElement5.remove_PointerExited), new PointerEventHandler(this.titleGrid_PointerExited));
          FrameworkElement frameworkElement = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(frameworkElement.add_SizeChanged), new Action<EventRegistrationToken>(frameworkElement.remove_SizeChanged), new SizeChangedEventHandler(this.titleGrid_SizeChanged));
          break;
        case 4:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement6.add_PointerEntered), new Action<EventRegistrationToken>(uiElement6.remove_PointerEntered), new PointerEventHandler(this.appBarPlaceHolder_PointerEntered));
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(uiElement7.add_PointerExited), new Action<EventRegistrationToken>(uiElement7.remove_PointerExited), new PointerEventHandler(this.appBarPlaceHolder_PointerExited));
          UIElement uiElement8 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement8.add_Tapped), new Action<EventRegistrationToken>(uiElement8.remove_Tapped), new TappedEventHandler(this.appBarPlaceHolder_Tapped));
          UIElement uiElement9 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>(new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(uiElement9.add_ManipulationDelta), new Action<EventRegistrationToken>(uiElement9.remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.appBarPlaceHolder_ManipulationDelta));
          break;
        case 5:
          ButtonBase buttonBase = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase.add_Click), new Action<EventRegistrationToken>(buttonBase.remove_Click), new RoutedEventHandler(this.searchButton_Click));
          break;
      }
      this._contentLoaded = true;
    }

    private class VisualTreeLoopHelper
    {
      public int MaxChildren;
      public int ChildIndex;
      public DependencyObject Obj;
    }
  }
}
