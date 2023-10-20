// Decompiled with JetBrains decompiler
// Type: myTube.PlayerControls
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.CustomMath;
using myTube.Helpers;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class PlayerControls : UserControl, IComponentConnector
  {
    public static readonly DependencyProperty MediaElementProperty = DependencyProperty.Register(nameof (MediaElement), typeof (MediaElement), typeof (PlayerControls), new PropertyMetadata((object) null, new PropertyChangedCallback(PlayerControls.OnMediaElementPropertyChanged)));
    public static readonly DependencyProperty ControlsStateProperty = DependencyProperty.Register(nameof (ControlsState), typeof (PlayerControlsState), typeof (PlayerControls), new PropertyMetadata((object) PlayerControlsState.Default, new PropertyChangedCallback(PlayerControls.OnControlsStateChanged)));
    public static readonly DependencyProperty PanelOrientationProperty = DependencyProperty.Register(nameof (PanelOrientation), typeof (Orientation), typeof (PlayerControls), new PropertyMetadata((object) (Orientation) 1));
    private MediaElementState lastState;
    public static Curve SeekingCurve;
    private ScaleTransform recTrans;
    private CompositeTransform downloadTrans;
    private DispatcherTimer timer;
    private bool seeking;
    private bool mouseSeeking;
    private TimeShortener timeShortener;
    private bool menuShown;
    private static bool backgroundAudio = false;
    public static readonly TimeSpan KeepMenuOpenFor = TimeSpan.FromSeconds(2.0);
    private Stopwatch menuWatch;
    public const double SeekingRecOpacity = 0.5;
    private bool playlistPanelShown;
    private TimeSpan totalTime;
    private DispatcherTimer timeUpdateTimer;
    private List<double> playbackSpeeds;
    private double seekMultiplier;
    private double width;
    private bool changeQuality;
    private YouTubeInfo tempInfo;
    private bool volumeShown;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl root;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform qualityTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform menuTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private QualityButtonInfoCollection qualityButtons;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Style playlistIconStyle;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup normalModes;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualStateGroup normalSizeModes;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState NormalSize;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState SmallSize;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Normal;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Compact;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel playlistPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel fullScreenButtonGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid menuGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid mainGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle progressRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle loadingRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle smallRec1;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle smallRec2;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle seekingRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock audioText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock seekInstructions;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid menuButtonGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock menuButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private SymbolIcon symbolIcon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run timeRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Run totalTimeRun;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform seekingRecTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel menuPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl captions;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl annotations;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl lockRotation;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl shuffleMode;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl playlistMode;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl videoSpeed;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock videoSpeedText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock playlistIcon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock shuffleIcon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListBox qualityPicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton volumeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Border volumeBorder;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton fullScreenButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CustomSlider volumeSlider;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton prevButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton nextButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler RequestTimerRestart;

    private static void OnControlsStateChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PlayerControls playerControls = d as PlayerControls;
      switch ((PlayerControlsState) e.NewValue)
      {
        case PlayerControlsState.Default:
          VisualStateManager.GoToState((Control) playerControls, "Normal", false);
          break;
        case PlayerControlsState.Compact:
          VisualStateManager.GoToState((Control) playerControls, "Compact", false);
          break;
      }
    }

    private static void OnMediaElementPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      PlayerControls playerControls = d as PlayerControls;
      if (e.OldValue != null)
      {
        MediaElement oldValue = e.OldValue as MediaElement;
        // ISSUE: virtual method pointer
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>((object) oldValue, __vmethodptr(oldValue, remove_CurrentStateChanged)), new RoutedEventHandler(playerControls.med_CurrentStateChanged));
        // ISSUE: virtual method pointer
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>((object) oldValue, __vmethodptr(oldValue, remove_DownloadProgressChanged)), new RoutedEventHandler(playerControls.med_BufferingProgressChanged));
        // ISSUE: virtual method pointer
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>((object) oldValue, __vmethodptr(oldValue, remove_SeekCompleted)), new RoutedEventHandler(playerControls.med_SeekCompleted));
      }
      MediaElement newValue = e.NewValue as MediaElement;
      MediaElement mediaElement1 = newValue;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(mediaElement1.add_CurrentStateChanged), new Action<EventRegistrationToken>(mediaElement1.remove_CurrentStateChanged), new RoutedEventHandler(playerControls.med_CurrentStateChanged));
      MediaElement mediaElement2 = newValue;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(mediaElement2.add_DownloadProgressChanged), new Action<EventRegistrationToken>(mediaElement2.remove_DownloadProgressChanged), new RoutedEventHandler(playerControls.med_BufferingProgressChanged));
      MediaElement mediaElement3 = newValue;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(mediaElement3.add_SeekCompleted), new Action<EventRegistrationToken>(mediaElement3.remove_SeekCompleted), new RoutedEventHandler(playerControls.med_SeekCompleted));
      playerControls.stateChanged(newValue);
    }

    private void med_SeekCompleted(object sender, RoutedEventArgs e) => this.stateChanged((MediaElement) sender);

    private void med_BufferingProgressChanged(object sender, RoutedEventArgs e) => this.downloadChanged(this.MediaElement.DownloadProgress, this.MediaElement.DownloadProgressOffset);

    private void downloadChanged(MediaElement med) => this.downloadChanged(med.DownloadProgress, med.DownloadProgressOffset);

    private void downloadChanged(double progress, double offset)
    {
      Storyboard sb = new Storyboard();
      if (!double.IsNaN(progress - offset) && this.downloadTrans.ScaleX != progress - offset)
        sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this.downloadTrans, "ScaleX", progress - offset, 0.4, 5.0));
      if (!double.IsNaN(offset) && this.downloadTrans.TranslateX != offset * ((FrameworkElement) this.loadingRec).ActualWidth)
        sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this.downloadTrans, "TranslateX", offset * ((FrameworkElement) this.loadingRec).ActualWidth, 0.4, 5.0));
      if (((ICollection<Timeline>) sb.Children).Count <= 0)
        return;
      sb.Begin();
    }

    private void med_CurrentStateChanged(object sender, RoutedEventArgs e)
    {
      MediaElement med = (MediaElement) sender;
      if (med.CurrentState == this.lastState)
        return;
      this.lastState = med.CurrentState;
      this.stateChanged(med);
    }

    private void stateChanged(MediaElement med)
    {
      Helper.Write((object) nameof (PlayerControls), (object) ("State changed method called with value of " + (object) med.CurrentState));
      switch ((int) med.CurrentState)
      {
        case 0:
          this.symbolIcon.put_Symbol((Symbol) 57608);
          break;
        case 1:
        case 2:
          this.symbolIcon.put_Symbol((Symbol) 57612);
          break;
        case 3:
          this.symbolIcon.put_Symbol((Symbol) 57603);
          break;
        case 4:
          this.symbolIcon.put_Symbol((Symbol) 57602);
          break;
        case 5:
          this.symbolIcon.put_Symbol((Symbol) 57602);
          break;
        default:
          this.symbolIcon.put_Symbol((Symbol) 57612);
          break;
      }
    }

    static PlayerControls()
    {
      PlayerControls.SeekingCurve = new Curve();
      PlayerControls.SeekingCurve.AddPoint(1.0, 0.6);
      PlayerControls.SeekingCurve.AddPoint(5.5, 1.0);
      PlayerControls.SeekingCurve.AddPoint(20.0, 1.75);
    }

    public Orientation PanelOrientation
    {
      get => (Orientation) ((DependencyObject) this).GetValue(PlayerControls.PanelOrientationProperty);
      set => ((DependencyObject) this).SetValue(PlayerControls.PanelOrientationProperty, (object) value);
    }

    public MediaElement MediaElement
    {
      get => (MediaElement) ((DependencyObject) this).GetValue(PlayerControls.MediaElementProperty);
      set => ((DependencyObject) this).SetValue(PlayerControls.MediaElementProperty, (object) value);
    }

    public PlayerControlsState ControlsState
    {
      get => (PlayerControlsState) ((DependencyObject) this).GetValue(PlayerControls.ControlsStateProperty);
      set => ((DependencyObject) this).SetValue(PlayerControls.ControlsStateProperty, (object) value);
    }

    public DispatcherTimer Timer => this.timer;

    public bool IsSeeking => this.seeking;

    public bool IsMouseSeeking => this.mouseSeeking;

    public bool MenuShown => this.menuShown;

    public Grid MainGrid => this.mainGrid;

    public Grid MenuGrid => this.menuGrid;

    public static void UpdateBackgroundAudioState()
    {
      try
      {
        MediaPlayerState currentState1 = BackgroundMediaPlayer.Current.CurrentState;
        MediaElementState currentState2 = DefaultPage.Current.VideoPlayer.MediaElement.CurrentState;
        PlayerControls.backgroundAudio = (currentState1 == 3 || currentState1 == 4 || currentState1 == 2) && (currentState2 == 5 || currentState2 == 0);
      }
      catch
      {
        PlayerControls.backgroundAudio = false;
      }
    }

    public static bool BackgroundAudio => PlayerControls.backgroundAudio;

    public StackPanel PlaylistPanel => this.playlistPanel;

    public bool PlaylistPanelShown => this.playlistPanelShown;

    public event EventHandler<bool> MenuShownChanged;

    public event EventHandler<bool> SeekingChanged;

    public event EventHandler<SliderValueChangedEventArgs> VolumeChanged;

    public event PlaylistOffsetEventHandler PlaylistButtonPressed;

    public PlayerControls()
    {
      ScaleTransform scaleTransform = new ScaleTransform();
      scaleTransform.put_ScaleX(0.0);
      this.recTrans = scaleTransform;
      CompositeTransform compositeTransform = new CompositeTransform();
      compositeTransform.put_ScaleX(1.0);
      this.downloadTrans = compositeTransform;
      this.menuShown = true;
      this.totalTime = TimeSpan.Zero;
      List<double> doubleList = new List<double>();
      doubleList.Add(1.0);
      doubleList.Add(1.25);
      doubleList.Add(1.5);
      doubleList.Add(2.0);
      doubleList.Add(0.25);
      doubleList.Add(0.5);
      this.playbackSpeeds = doubleList;
      this.seekMultiplier = 1.0;
      this.width = 100.0;
      this.changeQuality = true;
      this.volumeShown = true;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.menuWatch = new Stopwatch();
      this.timer = new DispatcherTimer();
      this.timer.put_Interval(TimeSpan.FromSeconds(1.0 / 30.0));
      DispatcherTimer timer = this.timer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(timer.add_Tick), new Action<EventRegistrationToken>(timer.remove_Tick), new EventHandler<object>(this.timer_Tick));
      this.timeShortener = new TimeShortener();
      this.InitializeComponent();
      ((UIElement) this.progressRec).put_RenderTransform((Transform) this.recTrans);
      ((UIElement) this.loadingRec).put_RenderTransform((Transform) this.downloadTrans);
      Grid mainGrid1 = this.mainGrid;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) mainGrid1).add_PointerEntered), new Action<EventRegistrationToken>(((UIElement) mainGrid1).remove_PointerEntered), new PointerEventHandler(this.PlayerControls_PointerEntered));
      Grid menuGrid1 = this.menuGrid;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) menuGrid1).add_PointerEntered), new Action<EventRegistrationToken>(((UIElement) menuGrid1).remove_PointerEntered), new PointerEventHandler(this.menuGrid_PointerEntered));
      Grid menuGrid2 = this.menuGrid;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) menuGrid2).add_PointerMoved), new Action<EventRegistrationToken>(((UIElement) menuGrid2).remove_PointerMoved), new PointerEventHandler(this.menuGrid_PointerEntered));
      Grid menuGrid3 = this.menuGrid;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) menuGrid3).add_PointerPressed), new Action<EventRegistrationToken>(((UIElement) menuGrid3).remove_PointerPressed), new PointerEventHandler(this.menuGrid_PointerEntered));
      Grid menuGrid4 = this.menuGrid;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) menuGrid4).add_PointerReleased), new Action<EventRegistrationToken>(((UIElement) menuGrid4).remove_PointerReleased), new PointerEventHandler(this.menuGrid_PointerEntered));
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>(new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) this).add_PointerMoved), new Action<EventRegistrationToken>(((UIElement) this).remove_PointerMoved), new PointerEventHandler(this.PlayerControls_PointerMoved));
      Grid mainGrid2 = this.mainGrid;
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) mainGrid2).add_Tapped), new Action<EventRegistrationToken>(((UIElement) mainGrid2).remove_Tapped), new TappedEventHandler(this.PlayerControls_Tapped));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.PlayerControls_Loaded));
      ((UIElement) this.playlistPanel).put_Opacity(0.0);
      ((UIElement) this.playlistPanel).put_IsHitTestVisible(false);
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.PlayerControls_SizeChanged));
      if (App.DeviceFamily == DeviceFamily.Mobile)
      {
        ((UIElement) this.volumeButton).put_Visibility((Visibility) 1);
        ((UIElement) this.fullScreenButton).put_Visibility((Visibility) 1);
      }
      this.timeUpdateTimer = new DispatcherTimer();
      this.timeUpdateTimer.put_Interval(TimeSpan.FromSeconds(1.0 / 30.0));
      DispatcherTimer timeUpdateTimer = this.timeUpdateTimer;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(timeUpdateTimer.add_Tick), new Action<EventRegistrationToken>(timeUpdateTimer.remove_Tick), new EventHandler<object>(this.timeUpdateTimer_Tick));
      ((Control) this.qualityPicker).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
    }

    public void SetPlaybackSpeed(double speed)
    {
      DefaultPage.Current.VideoPlayer.SetVideoSpeed(speed);
      this.videoSpeedText.put_Text(speed.Round(0.01).ToString() + "x");
    }

    public void RegisterBackgroundEvents()
    {
      MediaPlayer current1 = BackgroundMediaPlayer.Current;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MediaPlayer, object>>(new Func<TypedEventHandler<MediaPlayer, object>, EventRegistrationToken>(current1.add_CurrentStateChanged), new Action<EventRegistrationToken>(current1.remove_CurrentStateChanged), new TypedEventHandler<MediaPlayer, object>((object) this, __methodptr(Current_CurrentStateChanged)));
      MediaPlayer current2 = BackgroundMediaPlayer.Current;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MediaPlayer, object>>(new Func<TypedEventHandler<MediaPlayer, object>, EventRegistrationToken>(current2.add_MediaOpened), new Action<EventRegistrationToken>(current2.remove_MediaOpened), new TypedEventHandler<MediaPlayer, object>((object) this, __methodptr(Current_MediaOpened)));
    }

    public void DeregisterBackgroundEvents()
    {
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<MediaPlayer, object>>(new Action<EventRegistrationToken>(BackgroundMediaPlayer.Current.remove_CurrentStateChanged), new TypedEventHandler<MediaPlayer, object>((object) this, __methodptr(Current_CurrentStateChanged)));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<MediaPlayer, object>>(new Action<EventRegistrationToken>(BackgroundMediaPlayer.Current.remove_MediaOpened), new TypedEventHandler<MediaPlayer, object>((object) this, __methodptr(Current_MediaOpened)));
    }

    private void Current_MediaOpened(MediaPlayer sender, object args) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CCurrent_MediaOpened\u003Eb__76_0)));

    private void timeUpdateTimer_Tick(object sender, object e)
    {
      if (this.seeking)
      {
        if (PlayerControls.BackgroundAudio)
          this.timeRun.put_Text(this.timeShortener.Convert((object) TimeSpan.FromSeconds(this.totalTime.TotalSeconds * this.recTrans.ScaleX), typeof (string), (object) null, (string) null).ToString());
        else
          this.timeRun.put_Text(this.timeShortener.Convert((object) TimeSpan.FromSeconds(this.totalTime.TotalSeconds * this.recTrans.ScaleX), typeof (string), (object) null, (string) null).ToString());
      }
      else
        this.timeUpdateTimer.Stop();
    }

    private void PlayerControls_Loaded(object sender, RoutedEventArgs e)
    {
      this.hideVolume();
      this.CloseMenu();
    }

    private void PlayerControls_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      ((FrameworkElement) this.menuGrid).put_MaxWidth(e.NewSize.Width);
      if (e.NewSize.Width < 330.0)
        this.PanelOrientation = (Orientation) 0;
      else
        this.PanelOrientation = (Orientation) 1;
    }

    private void Current_CurrentStateChanged(MediaPlayer sender, object args) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) -1, new DispatchedHandler((object) new PlayerControls.\u003C\u003Ec__DisplayClass80_0()
    {
      \u003C\u003E4__this = this,
      sender = sender
    }, __methodptr(\u003CCurrent_CurrentStateChanged\u003Eb__0)));

    public void ShowPlaylistPanel()
    {
      if (this.playlistPanelShown)
        return;
      Ani.Begin((DependencyObject) this.playlistPanel, "Opacity", 1.0, 0.2);
      bool flag;
      ((UIElement) this.playlistPanel).put_IsHitTestVisible(flag = true);
      this.playlistPanelShown = flag;
    }

    public void HidePlaylistPanel()
    {
      if (!this.playlistPanelShown)
        return;
      Ani.Begin((DependencyObject) this.playlistPanel, "Opacity", 0.0, 0.2);
      bool flag;
      ((UIElement) this.playlistPanel).put_IsHitTestVisible(flag = false);
      this.playlistPanelShown = flag;
    }

    private void menuGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
      if (!this.menuShown || !this.menuWatch.IsRunning)
        return;
      this.menuWatch.Restart();
    }

    public void OpenMenu()
    {
      if (this.menuShown)
        return;
      if (this.playlistPanelShown)
      {
        Ani.Begin((DependencyObject) this.playlistPanel, "Opacity", 0.0, 0.2);
        ((UIElement) this.playlistPanel).put_IsHitTestVisible(false);
      }
      Storyboard storyboard = Ani.Animation(Ani.DoubleAni((DependencyObject) this.menuGrid, "Opacity", 1.0, 0.1), Ani.DoubleAni((DependencyObject) this.qualityTrans, "Y", 0.0, 0.5, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)), Ani.DoubleAni((DependencyObject) this.menuTrans, "Y", 0.0, 0.5, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
      this.SetPlaylistModeIcon();
      this.SetAnnotationsIcon();
      this.SetCaptionsIcon();
      this.SetShuffleIcon();
      if (((ItemsControl) this.qualityPicker).ItemsPanelRoot != null && this.tempInfo != null)
        this.SetVisibleButtons(this.tempInfo);
      storyboard.Begin();
      ((UIElement) this.menuGrid).put_IsHitTestVisible(this.menuShown = true);
      if (this.menuWatch.IsRunning)
        this.menuWatch.Reset();
      this.menuWatch.Start();
      if (this.MenuShownChanged == null)
        return;
      this.MenuShownChanged((object) this, true);
    }

    public void CloseMenu()
    {
      if (!this.menuShown)
        return;
      if (this.playlistPanelShown)
      {
        Ani.Begin((DependencyObject) this.playlistPanel, "Opacity", 1.0, 0.2);
        ((UIElement) this.playlistPanel).put_IsHitTestVisible(true);
      }
      double To = 42.0;
      double num = 2.0;
      Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.menuGrid, "Opacity", 0.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.qualityTrans, "Y", To, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 5.0)), (Timeline) Ani.DoubleAni((DependencyObject) this.menuTrans, "Y", To * num, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 5.0)));
      ((UIElement) this.menuGrid).put_IsHitTestVisible(this.menuShown = false);
      this.menuWatch.Reset();
      if (this.MenuShownChanged == null)
        return;
      this.MenuShownChanged((object) this, false);
    }

    private void PlayerControls_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.ControlsState != PlayerControlsState.Default || e.PointerDeviceType != 2)
        return;
      Point position = e.GetPosition((UIElement) this.progressRec);
      if (this.seeking || position.X <= 0.0 || position.X * this.recTrans.ScaleX >= ((FrameworkElement) this.progressRec).ActualWidth)
        return;
      this.recTrans.put_ScaleX(position.X * this.recTrans.ScaleX / ((FrameworkElement) this.progressRec).ActualWidth);
      this.MediaElement.put_Position(TimeSpan.FromSeconds(this.MediaElement.NaturalDuration.TimeSpan.TotalSeconds * this.recTrans.ScaleX));
    }

    protected virtual void OnPointerCaptureLost(PointerRoutedEventArgs e)
    {
      ((Control) this).OnPointerCaptureLost(e);
      if (!this.mouseSeeking || !this.seeking)
        return;
      this.mouseSeeking = false;
      this.seeking = false;
      if (this.SeekingChanged != null)
        this.SeekingChanged((object) this, false);
      Ani.Begin((DependencyObject) this.seekingRec, "Opacity", 0.0, 0.2);
      MediaElement mediaElement = this.MediaElement;
      MediaElement audioElement = VideoPlayer.AudioElement;
      TimeSpan timeSpan1 = this.MediaElement.NaturalDuration.TimeSpan;
      TimeSpan timeSpan2;
      TimeSpan timeSpan3 = timeSpan2 = TimeSpan.FromSeconds(timeSpan1.TotalSeconds * this.recTrans.ScaleX);
      audioElement.put_Position(timeSpan2);
      TimeSpan timeSpan4 = timeSpan3;
      mediaElement.put_Position(timeSpan4);
    }

    private void PlayerControls_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
      if (this.ControlsState != PlayerControlsState.Default || this.volumeSlider.IsChanging)
        return;
      PointerPoint currentPoint = e.GetCurrentPoint((UIElement) this.progressRec);
      Point position;
      if (!this.seeking)
      {
        position = currentPoint.Position;
        if (position.X > 0.0)
        {
          position = currentPoint.Position;
          if (position.X * this.recTrans.ScaleX < ((FrameworkElement) this.progressRec).ActualWidth && e.Pointer.IsInContact)
          {
            position = currentPoint.Position;
            if (position.Y > 0.0)
            {
              this.mouseSeeking = true;
              this.seeking = true;
              if (this.SeekingChanged != null)
                this.SeekingChanged((object) this, true);
              ((UIElement) this).CapturePointer(e.Pointer);
              Ani.Begin((DependencyObject) this.seekingRec, "Opacity", 1.0, 0.2);
            }
          }
        }
      }
      if (!this.mouseSeeking || !this.seeking)
        return;
      ScaleTransform recTrans = this.recTrans;
      position = currentPoint.Position;
      double num = position.X * this.recTrans.ScaleX / ((FrameworkElement) this.progressRec).ActualWidth;
      recTrans.put_ScaleX(num);
      this.recTrans.put_ScaleX(MyMath.Clamp(this.recTrans.ScaleX, 0.0, 1.0));
      this.timeRun.put_Text(this.timeShortener.Convert((object) TimeSpan.FromSeconds(this.MediaElement.NaturalDuration.TimeSpan.TotalSeconds * this.recTrans.ScaleX), typeof (string), (object) null, (string) null).ToString());
    }

    private void PlayerControls_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
      if (this.seeking)
        return;
      e.GetCurrentPoint((UIElement) this.progressRec);
    }

    public void StartTimer()
    {
      Helper.Write((object) nameof (PlayerControls), (object) "Timer started");
      PlayerControls.UpdateBackgroundAudioState();
      try
      {
        if (PlayerControls.BackgroundAudio)
        {
          this.totalTime = BackgroundMediaPlayer.Current.NaturalDuration;
          this.totalTimeRun.put_Text(this.timeShortener.Convert((object) this.totalTime, typeof (string), (object) null, (string) null).ToString());
        }
        else
        {
          Duration naturalDuration = this.MediaElement.NaturalDuration;
          if (this.MediaElement.NaturalDuration.HasTimeSpan)
          {
            this.totalTime = this.MediaElement.NaturalDuration.TimeSpan;
            this.totalTimeRun.put_Text(this.timeShortener.Convert((object) this.totalTime, typeof (string), (object) null, (string) null).ToString());
          }
        }
      }
      catch
      {
      }
      this.timer.Start();
      if (this.MediaElement == null)
        return;
      this.volumeSlider.SetRelativeValue(Math.Sqrt(this.MediaElement.Volume));
    }

    public void StopTimer() => this.timer.Stop();

    private void timer_Tick(object sender, object e)
    {
      try
      {
        PlayerControls.UpdateBackgroundAudioState();
      }
      catch
      {
      }
      bool flag = PlayerControls.BackgroundAudio;
      if (this.MediaElement != null && !this.seeking)
      {
        if (this.MediaElement.CurrentState == 3 || this.MediaElement.CurrentState == 4 || this.MediaElement.CurrentState == 2)
          flag = false;
        if (flag)
        {
          try
          {
            this.timeRun.put_Text(this.timeShortener.Convert((object) BackgroundMediaPlayer.Current.Position, typeof (string), (object) null, (string) null).ToString());
            this.recTrans.put_ScaleX(BackgroundMediaPlayer.Current.Position.TotalSeconds / this.totalTime.TotalSeconds);
          }
          catch
          {
          }
        }
        else
        {
          this.timeRun.put_Text(this.timeShortener.Convert((object) this.MediaElement.Position, typeof (string), (object) null, (string) null).ToString());
          try
          {
            this.recTrans.put_ScaleX(this.MediaElement.Position.TotalSeconds / this.totalTime.TotalSeconds);
          }
          catch
          {
          }
        }
      }
      if (this.MediaElement != null)
      {
        if (this.seeking)
        {
          try
          {
            if (flag)
              this.seekingRecTrans.put_X(BackgroundMediaPlayer.Current.Position.TotalSeconds / this.totalTime.TotalSeconds * ((FrameworkElement) this.progressRec).ActualWidth - ((FrameworkElement) this.seekingRec).ActualWidth / 2.0);
            else
              this.seekingRecTrans.put_X(this.MediaElement.Position.TotalSeconds / this.totalTime.TotalSeconds * ((FrameworkElement) this.progressRec).ActualWidth - ((FrameworkElement) this.seekingRec).ActualWidth / 2.0);
          }
          catch
          {
          }
        }
      }
      if (!this.menuShown || !this.menuWatch.IsRunning || !(this.menuWatch.Elapsed > PlayerControls.KeepMenuOpenFor))
        return;
      this.CloseMenu();
    }

    private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!PlayerControls.BackgroundAudio)
      {
        if (this.MediaElement == null)
          return;
        if (this.MediaElement.CurrentState == 3)
        {
          this.MediaElement.Pause();
        }
        else
        {
          try
          {
            this.MediaElement.Play();
          }
          catch
          {
          }
        }
      }
      else if (BackgroundMediaPlayer.Current.CurrentState == 3)
      {
        BackgroundMediaPlayer.Current.Pause();
      }
      else
      {
        try
        {
          BackgroundMediaPlayer.Current.Play();
        }
        catch
        {
        }
      }
    }

    private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e) => Helper.Write((object) "Player controls tapped");

    public void PerformManipulation(Point delta, Point total, double wid = double.NaN)
    {
      if (this.MediaElement == null || this.mouseSeeking)
        return;
      if (!this.seeking)
      {
        this.seekMultiplier = 0.5;
        this.seeking = true;
        if (this.SeekingChanged != null)
          this.SeekingChanged((object) this, true);
        Ani.Begin((DependencyObject) this.seekingRec, "Opacity", 1.0, 0.1);
        PlayerControls.UpdateBackgroundAudioState();
        this.width = !double.IsNaN(wid) ? wid : ((FrameworkElement) this).ActualWidth;
        double d = !PlayerControls.BackgroundAudio ? this.MediaElement.Position.TotalSeconds / this.MediaElement.NaturalDuration.TimeSpan.TotalSeconds : BackgroundMediaPlayer.Current.Position.TotalSeconds / BackgroundMediaPlayer.Current.NaturalDuration.TotalSeconds;
        if (!double.IsNaN(d) && !double.IsInfinity(d))
          this.recTrans.put_ScaleX(d);
        this.timeUpdateTimer.Start();
      }
      double num1 = delta.X / this.width;
      if (this.seekMultiplier < 1.0)
      {
        double num2 = Math.Abs(total.X / 60.0);
        if (num2 > this.seekMultiplier)
          this.seekMultiplier = num2;
        if (this.seekMultiplier > 1.0)
          this.seekMultiplier = 1.0;
      }
      if (delta.X != 0.0)
      {
        double d = PlayerControls.SeekingCurve.GetValue(Math.Abs(delta.X));
        if (!double.IsNaN(d) && !double.IsInfinity(d))
          this.seekMultiplier = d;
      }
      try
      {
        Math.Sqrt(((UIElement) this.seekingRec).Opacity);
      }
      catch
      {
      }
      ScaleTransform recTrans = this.recTrans;
      recTrans.put_ScaleX(recTrans.ScaleX + delta.X * this.seekMultiplier * ((UIElement) this).Opacity / this.width);
      this.recTrans.put_ScaleX(MyMath.Clamp(this.recTrans.ScaleX, 0.0, 1.0));
    }

    public void EndManipulation()
    {
      PlayerControls.UpdateBackgroundAudioState();
      if (this.MediaElement == null || !this.seeking || this.mouseSeeking)
        return;
      TimeSpan timeSpan1 = TimeSpan.FromSeconds((!PlayerControls.backgroundAudio ? this.MediaElement.NaturalDuration.TimeSpan : BackgroundMediaPlayer.Current.NaturalDuration).TotalSeconds * this.recTrans.ScaleX);
      this.timeUpdateTimer.Stop();
      this.mouseSeeking = false;
      this.seeking = false;
      Ani.Begin((DependencyObject) this.seekingRec, "Opacity", 0.0, 0.2);
      if (this.SeekingChanged != null)
        this.SeekingChanged((object) this, false);
      if (PlayerControls.backgroundAudio)
        BackgroundMediaPlayer.Current.put_Position(timeSpan1);
      else if (VideoPlayer.AudioElement.HasMedia())
      {
        VideoPlayer.AudioElement.put_Volume(0.0);
        MediaElement mediaElement = this.MediaElement;
        TimeSpan timeSpan2;
        VideoPlayer.AudioElement.put_Position(timeSpan2 = timeSpan1);
        TimeSpan timeSpan3 = timeSpan2;
        mediaElement.put_Position(timeSpan3);
      }
      else
        this.MediaElement.put_Position(timeSpan1);
    }

    public bool IsTapping(TappedRoutedEventArgs e)
    {
      bool flag = ((FrameworkElement) this.mainGrid).ContainsPoint(e.GetPosition((UIElement) this.mainGrid));
      if (this.menuShown)
        return ((((FrameworkElement) this.qualityPicker).ContainsPoint(e.GetPosition((UIElement) this.qualityPicker)) ? 1 : (((FrameworkElement) this.menuPanel).ContainsPoint(e.GetPosition((UIElement) this.menuPanel)) ? 1 : 0)) | (flag ? 1 : 0)) != 0;
      return this.playlistPanelShown ? ((FrameworkElement) this.playlistPanel).ContainsPoint(e.GetPosition((UIElement) this.playlistPanel)) | flag : flag;
    }

    public bool IsTapping(PointerRoutedEventArgs e)
    {
      bool flag = ((FrameworkElement) this.mainGrid).ContainsPoint(e.GetCurrentPoint((UIElement) this.mainGrid).Position);
      if (this.menuShown)
        return ((((FrameworkElement) this.qualityPicker).ContainsPoint(e.GetCurrentPoint((UIElement) this.qualityPicker).Position) ? 1 : (((FrameworkElement) this.menuPanel).ContainsPoint(e.GetCurrentPoint((UIElement) this.menuPanel).Position) ? 1 : 0)) | (flag ? 1 : 0)) != 0;
      return this.playlistPanelShown ? ((FrameworkElement) this.playlistPanel).ContainsPoint(e.GetCurrentPoint((UIElement) this.playlistPanel).Position) | flag : flag;
    }

    public void SetSelectedQuality(YouTubeQuality quality)
    {
      this.changeQuality = false;
      for (int index = 0; index < ((Collection<QualityButtonInfo>) this.qualityButtons).Count; ++index)
      {
        if (((Collection<QualityButtonInfo>) this.qualityButtons)[index].Quality == quality)
        {
          ((Selector) this.qualityPicker).put_SelectedIndex(index);
          this.menuButton.put_Text(((Collection<QualityButtonInfo>) this.qualityButtons)[index].Title);
          break;
        }
      }
      this.changeQuality = true;
    }

    public void SetCaptions(CaptionsDeclaration[] caps) => ((UIElement) this.captions).put_Visibility(caps != null && caps.Length != 0 ? (Visibility) 0 : (Visibility) 1);

    public void SetShuffleIconVisibility(bool visible)
    {
      if (visible)
        ((UIElement) this.shuffleMode).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.shuffleMode).put_Visibility((Visibility) 1);
    }

    public void SetVisibleButtons(YouTubeInfo info)
    {
      if (info != null)
      {
        ((UIElement) this.qualityPicker).put_Visibility((Visibility) 0);
        foreach (QualityButtonInfo qualityButton in (Collection<QualityButtonInfo>) this.qualityButtons)
        {
          bool flag = qualityButton.Quality <= App.HighestQuality && info.HasQuality(qualityButton.Quality);
          qualityButton.IsEnabled = flag;
        }
        if (info.AnnotationsLink != null)
          ((UIElement) this.annotations).put_Visibility((Visibility) 0);
        else
          ((UIElement) this.annotations).put_Visibility((Visibility) 1);
      }
      else
      {
        ((UIElement) this.qualityPicker).put_Visibility((Visibility) 1);
        this.menuButton.put_Text("S");
      }
    }

    private void qualityPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.changeQuality || this.qualityPicker == null || DefaultPage.Current == null)
        return;
      VideoPlayer videoPlayer = DefaultPage.Current.VideoPlayer;
      if (videoPlayer == null || !(((Selector) this.qualityPicker).SelectedItem is QualityButtonInfo selectedItem))
        return;
      videoPlayer.ChangeQuality(selectedItem.Quality);
    }

    private void menuButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.menuShown)
        this.CloseMenu();
      else
        this.OpenMenu();
    }

    private void prevTapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.PlaylistButtonPressed == null)
        return;
      this.PlaylistButtonPressed((object) this, -1);
    }

    private void nextTapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.PlaylistButtonPressed == null)
        return;
      this.PlaylistButtonPressed((object) this, 1);
    }

    private void lockRotation_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (Accel.Locked)
      {
        Accel.Unlock();
        Ani.Begin((DependencyObject) this.lockRotation, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "menuItemFade"], 0.2);
      }
      else
      {
        Accel.Lock();
        Ani.Begin((DependencyObject) this.lockRotation, "Opacity", 1.0, 0.2);
      }
    }

    private void playlistMode_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (DefaultPage.Current.VideoPlayer.HasPlaylist)
      {
        switch (Settings.PlaylistRepeatMode)
        {
          case PlaylistRepeatMode.One:
            Settings.PlaylistRepeatMode = PlaylistRepeatMode.None;
            break;
          case PlaylistRepeatMode.All:
            Settings.PlaylistRepeatMode = PlaylistRepeatMode.One;
            break;
          default:
            Settings.PlaylistRepeatMode = PlaylistRepeatMode.All;
            break;
        }
      }
      else
        Settings.NormalRepeatMode = Settings.NormalRepeatMode != PlaylistRepeatMode.One ? PlaylistRepeatMode.One : PlaylistRepeatMode.None;
      ValueSet valueSet = new ValueSet();
      ((IDictionary<string, object>) valueSet).Add("playlistRepeatMode", (object) Settings.PlaylistRepeatMode.ToString());
      ((IDictionary<string, object>) valueSet).Add("normalRepeatMode", (object) Settings.PlaylistRepeatMode.ToString());
      BackgroundMediaPlayer.SendMessageToBackground(valueSet);
      this.SetPlaylistModeIcon();
    }

    private void SetAnnotationsIcon()
    {
      if (!Settings.Annotations)
        Ani.Begin((DependencyObject) this.annotations, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "menuItemFade"], 0.2);
      else
        Ani.Begin((DependencyObject) this.annotations, "Opacity", 1.0, 0.2);
    }

    private void SetShuffleIcon()
    {
      if (!Settings.Shuffle)
        Ani.Begin((DependencyObject) this.shuffleIcon, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "menuItemFade"], 0.2);
      else
        Ani.Begin((DependencyObject) this.shuffleIcon, "Opacity", 1.0, 0.2);
    }

    private void SetCaptionsIcon()
    {
      if (!Settings.Subtitles)
        Ani.Begin((DependencyObject) this.captions, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "menuItemFade"], 0.2);
      else
        Ani.Begin((DependencyObject) this.captions, "Opacity", 1.0, 0.2);
    }

    private void SetPlaylistModeIcon()
    {
      if (DefaultPage.Current.VideoPlayer.HasPlaylist)
      {
        switch (Settings.PlaylistRepeatMode)
        {
          case PlaylistRepeatMode.None:
            this.playlistIcon.put_Text('\uE1CD'.ToString());
            Ani.Begin((DependencyObject) this.playlistIcon, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "menuItemFade"], 0.2);
            break;
          case PlaylistRepeatMode.One:
            this.playlistIcon.put_Text('\uE1CC'.ToString());
            Ani.Begin((DependencyObject) this.playlistIcon, "Opacity", 1.0, 0.2);
            break;
          case PlaylistRepeatMode.All:
            this.playlistIcon.put_Text('\uE1CD'.ToString());
            Ani.Begin((DependencyObject) this.playlistIcon, "Opacity", 1.0, 0.2);
            break;
        }
      }
      else
      {
        switch (Settings.NormalRepeatMode)
        {
          case PlaylistRepeatMode.None:
            this.playlistIcon.put_Text('\uE1CC'.ToString());
            Ani.Begin((DependencyObject) this.playlistIcon, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "menuItemFade"], 0.2);
            break;
          case PlaylistRepeatMode.One:
            this.playlistIcon.put_Text('\uE1CC'.ToString());
            Ani.Begin((DependencyObject) this.playlistIcon, "Opacity", 1.0, 0.2);
            break;
          case PlaylistRepeatMode.All:
            this.playlistIcon.put_Text('\uE1CD'.ToString());
            Ani.Begin((DependencyObject) this.playlistIcon, "Opacity", 1.0, 0.2);
            break;
        }
      }
    }

    private void annotations_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Settings.Annotations = !Settings.Annotations;
      DefaultPage.Current.VideoPlayer.SetAnnotations();
      this.SetAnnotationsIcon();
    }

    private async void fullScreenButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.put_Handled(true);
      DefaultPage current = DefaultPage.Current;
      if (current == null)
        return;
      this.fullScreenButton.Symbol = !await current.ToggleFullscreen() ? (Symbol) 57817 : (Symbol) 57816;
    }

    public void UpdateFullscreenButton()
    {
    }

    private void volumeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      e.put_Handled(true);
      if (this.RequestTimerRestart != null)
        this.RequestTimerRestart((object) this, (EventArgs) null);
      this.toggleVolume();
    }

    private void toggleVolume()
    {
      if (this.volumeShown)
        this.hideVolume();
      else
        this.showVolume();
    }

    private void showVolume()
    {
      if (this.volumeShown)
        return;
      this.volumeShown = true;
      ((UIElement) this.volumeBorder).put_Opacity(0.0);
      ((UIElement) this.volumeBorder).put_Visibility((Visibility) 0);
      ((UIElement) this.volumeBorder).put_IsHitTestVisible(false);
      if (this.MediaElement != null)
        this.volumeSlider.SetRelativeValue(Math.Sqrt(this.MediaElement.Volume));
      Storyboard storyboard = Ani.Begin((DependencyObject) this.volumeBorder, "Opacity", 1.0, 0.4);
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.volumeBorder).put_IsHitTestVisible(true)));
    }

    public void HideVolume() => this.hideVolume();

    private void hideVolume()
    {
      if (!this.volumeShown)
        return;
      this.volumeShown = false;
      ((UIElement) this.volumeBorder).put_IsHitTestVisible(false);
      ((UIElement) this.volumeBorder).put_Visibility((Visibility) 1);
    }

    private void volumeSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
    {
      if (this.VolumeChanged == null)
        return;
      this.VolumeChanged((object) this, new SliderValueChangedEventArgs()
      {
        NewValue = Math.Pow(MyMath.BetweenValue(this.volumeSlider.Minimum, this.volumeSlider.Maximum, e.NewValue), 2.0)
      });
    }

    private void shuffleMode_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Settings.Shuffle = !Settings.Shuffle;
      this.SetShuffleIcon();
    }

    private void videoSpeed_Tapped(object sender, TappedRoutedEventArgs e)
    {
      this.SetPlaybackSpeed(this.playbackSpeeds[(this.playbackSpeeds.IndexOf(VideoPlayer.VideoSpeed) + 1) % this.playbackSpeeds.Count]);
      if (this.RequestTimerRestart == null)
        return;
      this.RequestTimerRestart((object) this, (EventArgs) null);
    }

    private void captions_Tapped(object sender, TappedRoutedEventArgs e)
    {
      Settings.Subtitles = !Settings.Subtitles;
      DefaultPage.Current.VideoPlayer.SetAnnotations();
      this.SetCaptionsIcon();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PlayerControls.xaml"), (ComponentResourceLocation) 0);
      this.root = (UserControl) ((FrameworkElement) this).FindName("root");
      this.qualityTrans = (TranslateTransform) ((FrameworkElement) this).FindName("qualityTrans");
      this.menuTrans = (TranslateTransform) ((FrameworkElement) this).FindName("menuTrans");
      this.qualityButtons = (QualityButtonInfoCollection) ((FrameworkElement) this).FindName("qualityButtons");
      this.playlistIconStyle = (Style) ((FrameworkElement) this).FindName("playlistIconStyle");
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.normalModes = (VisualStateGroup) ((FrameworkElement) this).FindName("normalModes");
      this.normalSizeModes = (VisualStateGroup) ((FrameworkElement) this).FindName("normalSizeModes");
      this.NormalSize = (VisualState) ((FrameworkElement) this).FindName("NormalSize");
      this.SmallSize = (VisualState) ((FrameworkElement) this).FindName("SmallSize");
      this.Normal = (VisualState) ((FrameworkElement) this).FindName("Normal");
      this.Compact = (VisualState) ((FrameworkElement) this).FindName("Compact");
      this.playlistPanel = (StackPanel) ((FrameworkElement) this).FindName("playlistPanel");
      this.fullScreenButtonGrid = (StackPanel) ((FrameworkElement) this).FindName("fullScreenButtonGrid");
      this.menuGrid = (Grid) ((FrameworkElement) this).FindName("menuGrid");
      this.mainGrid = (Grid) ((FrameworkElement) this).FindName("mainGrid");
      this.progressRec = (Rectangle) ((FrameworkElement) this).FindName("progressRec");
      this.loadingRec = (Rectangle) ((FrameworkElement) this).FindName("loadingRec");
      this.smallRec1 = (Rectangle) ((FrameworkElement) this).FindName("smallRec1");
      this.smallRec2 = (Rectangle) ((FrameworkElement) this).FindName("smallRec2");
      this.seekingRec = (Rectangle) ((FrameworkElement) this).FindName("seekingRec");
      this.audioText = (TextBlock) ((FrameworkElement) this).FindName("audioText");
      this.seekInstructions = (TextBlock) ((FrameworkElement) this).FindName("seekInstructions");
      this.menuButtonGrid = (Grid) ((FrameworkElement) this).FindName("menuButtonGrid");
      this.menuButton = (TextBlock) ((FrameworkElement) this).FindName("menuButton");
      this.symbolIcon = (SymbolIcon) ((FrameworkElement) this).FindName("symbolIcon");
      this.timeRun = (Run) ((FrameworkElement) this).FindName("timeRun");
      this.totalTimeRun = (Run) ((FrameworkElement) this).FindName("totalTimeRun");
      this.seekingRecTrans = (TranslateTransform) ((FrameworkElement) this).FindName("seekingRecTrans");
      this.menuPanel = (StackPanel) ((FrameworkElement) this).FindName("menuPanel");
      this.captions = (ContentControl) ((FrameworkElement) this).FindName("captions");
      this.annotations = (ContentControl) ((FrameworkElement) this).FindName("annotations");
      this.lockRotation = (ContentControl) ((FrameworkElement) this).FindName("lockRotation");
      this.shuffleMode = (ContentControl) ((FrameworkElement) this).FindName("shuffleMode");
      this.playlistMode = (ContentControl) ((FrameworkElement) this).FindName("playlistMode");
      this.videoSpeed = (ContentControl) ((FrameworkElement) this).FindName("videoSpeed");
      this.videoSpeedText = (TextBlock) ((FrameworkElement) this).FindName("videoSpeedText");
      this.playlistIcon = (TextBlock) ((FrameworkElement) this).FindName("playlistIcon");
      this.shuffleIcon = (TextBlock) ((FrameworkElement) this).FindName("shuffleIcon");
      this.qualityPicker = (ListBox) ((FrameworkElement) this).FindName("qualityPicker");
      this.volumeButton = (IconTextButton) ((FrameworkElement) this).FindName("volumeButton");
      this.volumeBorder = (Border) ((FrameworkElement) this).FindName("volumeBorder");
      this.fullScreenButton = (IconTextButton) ((FrameworkElement) this).FindName("fullScreenButton");
      this.volumeSlider = (CustomSlider) ((FrameworkElement) this).FindName("volumeSlider");
      this.prevButton = (IconTextButton) ((FrameworkElement) this).FindName("prevButton");
      this.nextButton = (IconTextButton) ((FrameworkElement) this).FindName("nextButton");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.Grid_Tapped_1));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.menuButton_Tapped));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.Grid_Tapped));
          break;
        case 4:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.captions_Tapped));
          break;
        case 5:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.annotations_Tapped));
          break;
        case 6:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.lockRotation_Tapped));
          break;
        case 7:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement7.add_Tapped), new Action<EventRegistrationToken>(uiElement7.remove_Tapped), new TappedEventHandler(this.shuffleMode_Tapped));
          break;
        case 8:
          UIElement uiElement8 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement8.add_Tapped), new Action<EventRegistrationToken>(uiElement8.remove_Tapped), new TappedEventHandler(this.playlistMode_Tapped));
          break;
        case 9:
          UIElement uiElement9 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement9.add_Tapped), new Action<EventRegistrationToken>(uiElement9.remove_Tapped), new TappedEventHandler(this.videoSpeed_Tapped));
          break;
        case 10:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.qualityPicker_SelectionChanged));
          break;
        case 11:
          UIElement uiElement10 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement10.add_Tapped), new Action<EventRegistrationToken>(uiElement10.remove_Tapped), new TappedEventHandler(this.volumeButton_Tapped));
          break;
        case 12:
          UIElement uiElement11 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement11.add_Tapped), new Action<EventRegistrationToken>(uiElement11.remove_Tapped), new TappedEventHandler(this.fullScreenButton_Tapped));
          break;
        case 13:
          ((CustomSlider) target).ValueChanged += new EventHandler<SliderValueChangedEventArgs>(this.volumeSlider_ValueChanged);
          break;
        case 14:
          UIElement uiElement12 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement12.add_Tapped), new Action<EventRegistrationToken>(uiElement12.remove_Tapped), new TappedEventHandler(this.prevTapped));
          break;
        case 15:
          UIElement uiElement13 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement13.add_Tapped), new Action<EventRegistrationToken>(uiElement13.remove_Tapped), new TappedEventHandler(this.nextTapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
