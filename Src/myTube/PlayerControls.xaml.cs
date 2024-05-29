// myTube.PlayerControls

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
using myTube.CustomMath;
using myTube.Helpers;
using RykenTube;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using Windows.Devices.Input;

namespace myTube
{
    public partial class PlayerControls : UserControl
    {
        public static readonly DependencyProperty MediaElementProperty
            = DependencyProperty.Register(nameof(MediaElement),
                typeof(MediaElement), typeof(PlayerControls),
                new PropertyMetadata((object)null,
                    new PropertyChangedCallback(PlayerControls.OnMediaElementPropertyChanged)));

        public static readonly DependencyProperty ControlsStateProperty
            = DependencyProperty.Register(nameof(ControlsState),
                typeof(PlayerControlsState), typeof(PlayerControls),
                new PropertyMetadata((object)PlayerControlsState.Default,
                    new PropertyChangedCallback(PlayerControls.OnControlsStateChanged)));

        public static readonly DependencyProperty PanelOrientationProperty
            = DependencyProperty.Register(nameof(PanelOrientation),
                typeof(Orientation), typeof(PlayerControls),
                new PropertyMetadata((object)(Orientation)1));

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

        //TEMP
        //private SymbolIcon symbolIcon;
        //private Run timeRun;
        //private Run totalTimeRun;
        //private ListBox qualityPicker;
        private TextBlock videoSpeedText = new TextBlock();
        private CustomSlider volumeSlider = new CustomSlider();
        private QualityButtonInfoCollection qualityButtons = new QualityButtonInfoCollection();
        //private TextBlock menuButton;
        private TextBlock shuffleIcon = new TextBlock();
        private Border volumeBorder = new Border();

        /*
    private UserControl root;
    private TranslateTransform qualityTrans;
    private TranslateTransform menuTrans;
    private QualityButtonInfoCollection qualityButtons;
    private Style playlistIconStyle;
    private Grid layoutRoot;
    private VisualStateGroup normalModes;
    private VisualStateGroup normalSizeModes;
    private VisualState NormalSize;
    private VisualState SmallSize;
    private VisualState Normal;
    private VisualState Compact;
    private StackPanel playlistPanel;
    private StackPanel fullScreenButtonGrid;
    private Grid menuGrid;
    private Grid mainGrid;
    private Rectangle progressRec;
    private Rectangle loadingRec;
    private Rectangle smallRec1;
    private Rectangle smallRec2;
    private Rectangle seekingRec;
    private TextBlock audioText;
    private TextBlock seekInstructions;
    private Grid menuButtonGrid;
    private TextBlock menuButton;
    private Run timeRun;
    private Run totalTimeRun;
    private TranslateTransform seekingRecTrans;
    private StackPanel menuPanel;
    private ContentControl captions;
    private ContentControl annotations;
    private ContentControl lockRotation;
    private ContentControl shuffleMode;
    private ContentControl playlistMode;
    private ContentControl videoSpeed;
    private TextBlock videoSpeedText;
    private TextBlock playlistIcon;
    private TextBlock shuffleIcon;
    private ListBox qualityPicker;
    private IconTextButton volumeButton;
    private Border volumeBorder;
    private IconTextButton fullScreenButton;
    private CustomSlider volumeSlider;
    private IconTextButton prevButton;
    private IconTextButton nextButton;
*/

        public event EventHandler RequestTimerRestart;

        private static void OnControlsStateChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            PlayerControls playerControls = d as PlayerControls;
            switch ((PlayerControlsState)e.NewValue)
            {
                case PlayerControlsState.Default:
                    VisualStateManager.GoToState((Control)playerControls, "Normal", false);
                    break;
                case PlayerControlsState.Compact:
                    VisualStateManager.GoToState((Control)playerControls, "Compact", false);
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
                oldValue.CurrentStateChanged -= playerControls.med_CurrentStateChanged;
                oldValue.DownloadProgressChanged -= playerControls.med_BufferingProgressChanged;
                oldValue.SeekCompleted -= playerControls.med_SeekCompleted;
            }
            MediaElement newValue = e.NewValue as MediaElement;
            newValue.CurrentStateChanged += playerControls.med_CurrentStateChanged;
            newValue.DownloadProgressChanged += playerControls.med_BufferingProgressChanged;
            newValue.SeekCompleted += playerControls.med_SeekCompleted;
            playerControls.stateChanged(newValue);
        }

        private void med_SeekCompleted(object sender, RoutedEventArgs e)
            => this.stateChanged((MediaElement)sender);

        private void med_BufferingProgressChanged(object sender, RoutedEventArgs e)
            => this.downloadChanged(this.MediaElement.DownloadProgress, this.MediaElement.DownloadProgressOffset);

        private void downloadChanged(MediaElement med)
            => this.downloadChanged(med.DownloadProgress, med.DownloadProgressOffset);

        private void downloadChanged(double progress, double offset)
        {
            Storyboard sb = new Storyboard();
            if (!double.IsNaN(progress - offset) && this.downloadTrans.ScaleX != progress - offset)
                sb.Add((Timeline)Ani.DoubleAni((DependencyObject)this.downloadTrans,
                    "ScaleX", progress - offset, 0.4, 5.0));
            if (!double.IsNaN(offset)
                && this.downloadTrans.TranslateX != offset * ((FrameworkElement)this.loadingRec).ActualWidth)
                sb.Add((Timeline)Ani.DoubleAni((DependencyObject)this.downloadTrans,
                    "TranslateX", offset * ((FrameworkElement)this.loadingRec).ActualWidth, 0.4, 5.0));
            if (((ICollection<Timeline>)sb.Children).Count <= 0)
                return;
            sb.Begin();
        }

        private void med_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            MediaElement med = (MediaElement)sender;
            if (med.CurrentState == this.lastState)
                return;
            this.lastState = med.CurrentState;
            this.stateChanged(med);
        }

        private void stateChanged(MediaElement med)
        {
            Helper.Write((object)nameof(PlayerControls),
                (object)("State changed method called with value of " + (object)med.CurrentState));
            switch ((int)med.CurrentState)
            {
                case 0:
                    this.symbolIcon.Symbol = (Symbol)57608;
                    break;
                case 1:
                case 2:
                    this.symbolIcon.Symbol = (Symbol)57612;
                    break;
                case 3:
                    this.symbolIcon.Symbol = (Symbol)57603;
                    break;
                case 4:
                    this.symbolIcon.Symbol = (Symbol)57602;
                    break;
                case 5:
                    this.symbolIcon.Symbol = (Symbol)57602;
                    break;
                default:
                    this.symbolIcon.Symbol = (Symbol)57612;
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
            get => (Orientation)((DependencyObject)this).GetValue(PlayerControls.PanelOrientationProperty);
            set => ((DependencyObject)this).SetValue(PlayerControls.PanelOrientationProperty, (object)value);
        }

        public MediaElement MediaElement
        {
            get => (MediaElement)((DependencyObject)this).GetValue(PlayerControls.MediaElementProperty);
            set => ((DependencyObject)this).SetValue(PlayerControls.MediaElementProperty, (object)value);
        }

        public PlayerControlsState ControlsState
        {
            get => (PlayerControlsState)((DependencyObject)this).GetValue(PlayerControls.ControlsStateProperty);
            set => ((DependencyObject)this).SetValue(PlayerControls.ControlsStateProperty, (object)value);
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
                PlayerControls.backgroundAudio = (currentState1 == (MediaPlayerState)3
                    || currentState1 == (MediaPlayerState)4 || currentState1 == (MediaPlayerState)2)
                    && (currentState2 == (MediaElementState)5 || currentState2 == (MediaElementState)0);
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
            scaleTransform.ScaleX = 0.0;
            this.recTrans = scaleTransform;
            CompositeTransform compositeTransform = new CompositeTransform();
            compositeTransform.ScaleX = 1.0;
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

            //Experimental
            // ISSUE: explicit constructor call (TODO)
            //base.ctor();
            this.InitializeComponent();


            this.menuWatch = new Stopwatch();
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            DispatcherTimer timer = this.timer;
         
            this.timer.Tick += this.timer_Tick;

            this.timeShortener = new TimeShortener();


            //this.InitializeComponent();

            this.progressRec.RenderTransform = (Transform)this.recTrans;
            this.loadingRec.RenderTransform = (Transform)this.downloadTrans;

            this.mainGrid.PointerEntered += this.PlayerControls_PointerEntered;
            this.menuGrid.PointerEntered += this.menuGrid_PointerEntered;
            this.menuGrid.PointerMoved += this.menuGrid_PointerEntered;
            this.menuGrid.PointerPressed += this.menuGrid_PointerEntered;
            this.menuGrid.PointerReleased += this.menuGrid_PointerEntered;
            this.PointerMoved += this.PlayerControls_PointerMoved;
            this.mainGrid.Tapped += this.PlayerControls_Tapped;
            this.Loaded += this.PlayerControls_Loaded;
            this.SizeChanged += this.PlayerControls_SizeChanged;
            this.playlistPanel.Opacity = 0.0;
            this.playlistPanel.IsHitTestVisible = false;

            if (App.DeviceFamily == DeviceFamily.Mobile)
            {
                this.volumeButton.Visibility = Visibility.Collapsed;
                this.fullScreenButton.Visibility = Visibility.Collapsed;
            }
            this.timeUpdateTimer = new DispatcherTimer();
            this.timeUpdateTimer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);
            DispatcherTimer timeUpdateTimer = this.timeUpdateTimer;
            
            this.timeUpdateTimer.Tick += this.timeUpdateTimer_Tick;

            this.qualityPicker.FontFamily = DefaultPage.Current.FontFamily;

        }
     



        public void SetPlaybackSpeed(double speed)
        {
            DefaultPage.Current.VideoPlayer.SetVideoSpeed(speed);
            this.videoSpeedText.Text = speed.Round(0.01).ToString() + "x";
        }

        public void RegisterBackgroundEvents()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged += Current_CurrentStateChanged;
            BackgroundMediaPlayer.Current.MediaOpened += Current_MediaOpened;
        }

        public void DeregisterBackgroundEvents()
        {
            BackgroundMediaPlayer.Current.CurrentStateChanged -= Current_CurrentStateChanged;
            BackgroundMediaPlayer.Current.MediaOpened -= Current_MediaOpened;
        }

        private async void Current_MediaOpened(MediaPlayer sender, object args)
        {
           await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Current_MediaOpened(BackgroundMediaPlayer.Current, null);
            });
        }

        private void timeUpdateTimer_Tick(object sender, object e)
        {
            if (this.seeking)
            {
                if (PlayerControls.BackgroundAudio)
                    this.timeRun.Text = 
                        (this.timeShortener.Convert((object)TimeSpan.FromSeconds(
                            this.totalTime.TotalSeconds * this.recTrans.ScaleX), 
                            typeof(string), (object)null, (string)null).ToString());
                else
                    this.timeRun.Text = 
                        (this.timeShortener.Convert((object)TimeSpan.FromSeconds(
                            this.totalTime.TotalSeconds * this.recTrans.ScaleX), 
                            typeof(string), (object)null, (string)null).ToString());
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
            this.menuGrid.MaxWidth = e.NewSize.Width;
            if (e.NewSize.Width < 330.0)
                this.PanelOrientation = (Orientation)0;
            else
                this.PanelOrientation = (Orientation)1;
        }

        public void UpdateFullscreenButton()
        {
            // Not Implemented 
        }

        private async void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                //TODO
                var displayClass800 = new PlayerControls.DisplayClass80_0()
                {
                    sender = sender
                };
                //TODO
                //this.Current_CurrentStateChanged(displayClass800);
            });
        }

        public void ShowPlaylistPanel()
        {
            if (this.playlistPanelShown)
                return;
            Ani.Begin((DependencyObject)this.playlistPanel, "Opacity", 1.0, 0.2);
            bool flag;
            ((UIElement)this.playlistPanel).IsHitTestVisible = flag = true;
            this.playlistPanelShown = flag;
        }

        public void HidePlaylistPanel()
        {
            if (!this.playlistPanelShown)
                return;

            Ani.Begin((DependencyObject)this.playlistPanel, "Opacity", 0.0, 0.2);

            bool flag = false;            
            this.playlistPanel.IsHitTestVisible = flag;
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
                Ani.Begin((DependencyObject)this.playlistPanel, "Opacity", 0.0, 0.2);
                this.playlistPanel.IsHitTestVisible = false;
            }
            Storyboard storyboard = Ani.Animation(Ani.DoubleAni((DependencyObject)this.menuGrid,
                "Opacity", 1.0, 0.1), Ani.DoubleAni((DependencyObject)this.qualityTrans, 
                "Y", 0.0, 0.5, (EasingFunctionBase)Ani.Ease((EasingMode)0, 5.0)), 
                Ani.DoubleAni((DependencyObject)this.menuTrans, "Y", 0.0, 0.5, 
                (EasingFunctionBase)Ani.Ease((EasingMode)0, 5.0)));
            this.SetPlaylistModeIcon();
            this.SetAnnotationsIcon();
            this.SetCaptionsIcon();
            this.SetShuffleIcon();
            if (((ItemsControl)this.qualityPicker).ItemsPanelRoot != null && this.tempInfo != null)
                this.SetVisibleButtons(this.tempInfo);

            storyboard.Begin();

            this.menuGrid.IsHitTestVisible = this.menuShown = true;
            if (this.menuWatch.IsRunning)
                this.menuWatch.Reset();
            this.menuWatch.Start();

            if (this.MenuShownChanged == null)
                return;
            this.MenuShownChanged((object)this, true);
        }

        public void CloseMenu()
        {
            if (!this.menuShown)
                return;
            if (this.playlistPanelShown)
            {
                Ani.Begin((DependencyObject)this.playlistPanel, "Opacity", 1.0, 0.2);
                this.playlistPanel.IsHitTestVisible = true;
            }
            double To = 42.0;
            double num = 2.0;
            Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.menuGrid, "Opacity", 0.0, 0.2), 
                (Timeline)Ani.DoubleAni((DependencyObject)this.qualityTrans, "Y",
                To, 0.2, (EasingFunctionBase)Ani.Ease((EasingMode)1, 5.0)),
                (Timeline)Ani.DoubleAni((DependencyObject)this.menuTrans, "Y",
                To * num, 0.2, (EasingFunctionBase)Ani.Ease((EasingMode)1, 5.0)));

            this.menuGrid.IsHitTestVisible = this.menuShown = false;
            this.menuWatch.Reset();
            if (this.MenuShownChanged == null)
                return;
            this.MenuShownChanged((object)this, false);
        }

        private void PlayerControls_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.ControlsState != PlayerControlsState.Default || e.PointerDeviceType != PointerDeviceType.Mouse)
                return;
            Point position = e.GetPosition((UIElement)this.progressRec);
            if (this.seeking || position.X <= 0.0 
                || position.X * this.recTrans.ScaleX >= ((FrameworkElement)this.progressRec).ActualWidth)
                return;
            this.recTrans.ScaleX = 
                position.X * this.recTrans.ScaleX / ((FrameworkElement)this.progressRec).ActualWidth;
            this.MediaElement.Position = 
                TimeSpan.FromSeconds(this.MediaElement.NaturalDuration.TimeSpan.TotalSeconds 
                * this.recTrans.ScaleX);
        }

        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
            base.OnPointerCaptureLost(e);

            if (!this.mouseSeeking || !this.seeking)
                return;
            this.mouseSeeking = false;
            this.seeking = false;
            if (this.SeekingChanged != null)
                this.SeekingChanged((object)this, false);
            Ani.Begin((DependencyObject)this.seekingRec, "Opacity", 0.0, 0.2);
            MediaElement mediaElement = this.MediaElement;
            MediaElement audioElement = VideoPlayer.AudioElement;
            TimeSpan timeSpan1 = this.MediaElement.NaturalDuration.TimeSpan;
            TimeSpan timeSpan2;
            TimeSpan timeSpan3 = timeSpan2 = TimeSpan.FromSeconds(timeSpan1.TotalSeconds * this.recTrans.ScaleX);
            audioElement.Position = timeSpan2;
            TimeSpan timeSpan4 = timeSpan3;
            mediaElement.Position = timeSpan4;
        }

        private void PlayerControls_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.ControlsState != PlayerControlsState.Default || this.volumeSlider.IsChanging)
                return;
            PointerPoint currentPoint = e.GetCurrentPoint((UIElement)this.progressRec);
            Point position;
            if (!this.seeking)
            {
                position = currentPoint.Position;
                if (position.X > 0.0)
                {
                    position = currentPoint.Position;
                    if (position.X * this.recTrans.ScaleX < ((FrameworkElement)this.progressRec).ActualWidth && e.Pointer.IsInContact)
                    {
                        position = currentPoint.Position;
                        if (position.Y > 0.0)
                        {
                            this.mouseSeeking = true;
                            this.seeking = true;
                            if (this.SeekingChanged != null)
                                this.SeekingChanged((object)this, true);
                            ((UIElement)this).CapturePointer(e.Pointer);
                            Ani.Begin((DependencyObject)this.seekingRec, "Opacity", 1.0, 0.2);
                        }
                    }
                }
            }
            if (!this.mouseSeeking || !this.seeking)
                return;
            ScaleTransform recTrans = this.recTrans;
            position = currentPoint.Position;
            double num = position.X * this.recTrans.ScaleX / ((FrameworkElement)this.progressRec).ActualWidth;
            recTrans.ScaleX = num;
            this.recTrans.ScaleX = MyMath.Clamp(this.recTrans.ScaleX, 0.0, 1.0);
            this.timeRun.Text = 
                this.timeShortener.Convert(
                    (object)TimeSpan.FromSeconds(
                        this.MediaElement.NaturalDuration.TimeSpan.TotalSeconds 
                        * this.recTrans.ScaleX), typeof(string), (object)null, (string)null).ToString();
        }

        private void PlayerControls_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (this.seeking)
                return;
            e.GetCurrentPoint((UIElement)this.progressRec);
        }

        public void StartTimer()
        {
            Helper.Write((object)nameof(PlayerControls), (object)"Timer started");
            PlayerControls.UpdateBackgroundAudioState();
            try
            {
                if (PlayerControls.BackgroundAudio)
                {
                    this.totalTime = BackgroundMediaPlayer.Current.NaturalDuration;
                    this.totalTimeRun.Text = this.timeShortener.Convert((object)this.totalTime,
                        typeof(string), (object)null, (string)null).ToString();
                }
                else
                {
                    Duration naturalDuration = this.MediaElement.NaturalDuration;
                    if (this.MediaElement.NaturalDuration.HasTimeSpan)
                    {
                        this.totalTime = this.MediaElement.NaturalDuration.TimeSpan;
                        this.totalTimeRun.Text = this.timeShortener.Convert((object)this.totalTime, 
                            typeof(string), (object)null, (string)null).ToString();
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
                if (this.MediaElement.CurrentState == (MediaElementState)3 
                    || this.MediaElement.CurrentState == (MediaElementState)4 
                    || this.MediaElement.CurrentState == (MediaElementState)2)
                    flag = false;
                if (flag)
                {
                    try
                    {
                        this.timeRun.Text = 
                            this.timeShortener.Convert((object)BackgroundMediaPlayer.Current.Position,
                            typeof(string), (object)null, (string)null).ToString();
                        this.recTrans.ScaleX = 
                            BackgroundMediaPlayer.Current.Position.TotalSeconds / this.totalTime.TotalSeconds;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    this.timeRun.Text = 
                        this.timeShortener.Convert((object)this.MediaElement.Position, typeof(string), 
                        (object)null, (string)null).ToString();
                    try
                    {
                        this.recTrans.ScaleX = 
                            this.MediaElement.Position.TotalSeconds / this.totalTime.TotalSeconds;
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
                            this.seekingRecTrans.X 
                                = BackgroundMediaPlayer.Current.Position.TotalSeconds 
                                / this.totalTime.TotalSeconds 
                                * ((FrameworkElement)this.progressRec).ActualWidth 
                                - ((FrameworkElement)this.seekingRec).ActualWidth / 2.0;
                        else
                            this.seekingRecTrans.X = this.MediaElement.Position.TotalSeconds 
                                / this.totalTime.TotalSeconds * ((FrameworkElement)this.progressRec).ActualWidth
                                - ((FrameworkElement)this.seekingRec).ActualWidth / 2.0;
                    }
                    catch
                    {
                    }
                }
            }
            if (!this.menuShown || !this.menuWatch.IsRunning 
                || !(this.menuWatch.Elapsed > PlayerControls.KeepMenuOpenFor))
                return;
            this.CloseMenu();
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!PlayerControls.BackgroundAudio)
            {
                if (this.MediaElement == null)
                    return;
                if (this.MediaElement.CurrentState == (MediaElementState)3)
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
            else if (BackgroundMediaPlayer.Current.CurrentState == (MediaPlayerState)3)
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

        private void Grid_Tapped_1(object sender, TappedRoutedEventArgs e) => Helper.Write((object)"Player controls tapped");

        public void PerformManipulation(Point delta, Point total, double wid = double.NaN)
        {
            if (this.MediaElement == null || this.mouseSeeking)
                return;
            if (!this.seeking)
            {
                this.seekMultiplier = 0.5;
                this.seeking = true;
                if (this.SeekingChanged != null)
                    this.SeekingChanged((object)this, true);
                Ani.Begin((DependencyObject)this.seekingRec, "Opacity", 1.0, 0.1);
                PlayerControls.UpdateBackgroundAudioState();
                this.width = !double.IsNaN(wid) ? wid : ((FrameworkElement)this).ActualWidth;
                double d = !PlayerControls.BackgroundAudio ? this.MediaElement.Position.TotalSeconds / this.MediaElement.NaturalDuration.TimeSpan.TotalSeconds : BackgroundMediaPlayer.Current.Position.TotalSeconds / BackgroundMediaPlayer.Current.NaturalDuration.TotalSeconds;
                if (!double.IsNaN(d) && !double.IsInfinity(d))
                    this.recTrans.ScaleX = d;
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
                Math.Sqrt(((UIElement)this.seekingRec).Opacity);
            }
            catch
            {
            }
            ScaleTransform recTrans = this.recTrans;
            recTrans.ScaleX = 
                recTrans.ScaleX + delta.X * this.seekMultiplier * ((UIElement)this).Opacity / this.width;
            this.recTrans.ScaleX = MyMath.Clamp(this.recTrans.ScaleX, 0.0, 1.0);
        }

        public void EndManipulation()
        {
            PlayerControls.UpdateBackgroundAudioState();
            if (this.MediaElement == null || !this.seeking || this.mouseSeeking)
                return;
            TimeSpan timeSpan1 = TimeSpan.FromSeconds(
                ( !PlayerControls.backgroundAudio 
                  ? this.MediaElement.NaturalDuration.TimeSpan 
                  : BackgroundMediaPlayer.Current.NaturalDuration).TotalSeconds * this.recTrans.ScaleX );
            this.timeUpdateTimer.Stop();
            this.mouseSeeking = false;
            this.seeking = false;
            Ani.Begin((DependencyObject)this.seekingRec, "Opacity", 0.0, 0.2);
            if (this.SeekingChanged != null)
                this.SeekingChanged((object)this, false);
            if (PlayerControls.backgroundAudio)
                BackgroundMediaPlayer.Current.Position = timeSpan1;
            else if (VideoPlayer.AudioElement.HasMedia())
            {
                VideoPlayer.AudioElement.Volume = 0.0;
                MediaElement mediaElement = this.MediaElement;
                TimeSpan timeSpan2;
                VideoPlayer.AudioElement.Position = timeSpan2 = timeSpan1;
                TimeSpan timeSpan3 = timeSpan2;
                mediaElement.Position = timeSpan3;
            }
            else
                this.MediaElement.Position = timeSpan1;
        }

        public bool IsTapping(TappedRoutedEventArgs e)
        {
            bool flag = this.mainGrid.ContainsPoint(e.GetPosition(this.mainGrid));

            if (this.menuShown)
                return ((((FrameworkElement)this.qualityPicker).ContainsPoint(e.GetPosition(
                    (UIElement)this.qualityPicker)) 
                    ? 1 
                    : (((FrameworkElement)this.menuPanel).ContainsPoint(e.GetPosition((UIElement)this.menuPanel))
                    ? 1 : 0)) | (flag ? 1 : 0)) != 0;

            return this.playlistPanelShown 
                ? ((FrameworkElement)this.playlistPanel)
                  .ContainsPoint(e.GetPosition((UIElement)this.playlistPanel)) | flag 
                : flag;
        }

        public bool IsTapping(PointerRoutedEventArgs e)
        {
            bool flag = ((FrameworkElement)this.mainGrid)
                .ContainsPoint(e.GetCurrentPoint((UIElement)this.mainGrid).Position);
            
            if (this.menuShown)
                return ((((FrameworkElement)this.qualityPicker)
                    .ContainsPoint(e.GetCurrentPoint((UIElement)this.qualityPicker).Position) 
                    ? 1 : (((FrameworkElement)this.menuPanel)
                      .ContainsPoint(e.GetCurrentPoint((UIElement)this.menuPanel).Position) ? 1 : 0)) | (flag 
                      ? 1 
                      : 0)) != 0;
            
            return this.playlistPanelShown 
                ? ((FrameworkElement)this.playlistPanel)
                  .ContainsPoint(e.GetCurrentPoint((UIElement)this.playlistPanel).Position) | flag 
                : flag;
        }

        public void SetSelectedQuality(YouTubeQuality quality)
        {
            this.changeQuality = false;
            for (int index = 0; index < ((Collection<QualityButtonInfo>)this.qualityButtons).Count; ++index)
            {
                if (((Collection<QualityButtonInfo>)this.qualityButtons)[index].Quality == quality)
                {
                    this.qualityPicker.SelectedIndex = index;
                    this.menuButton.Text = ((Collection<QualityButtonInfo>)this.qualityButtons)[index].Title;
                    break;
                }
            }
            this.changeQuality = true;
        }

        public void SetCaptions(CaptionsDeclaration[] caps)
        {
            this.captions.Visibility = 
                caps != null && caps.Length != 0 ? (Visibility)0 : (Visibility)1;
        }

        public void SetShuffleIconVisibility(bool visible)
        {
            if (visible)
                this.shuffleMode.Visibility = Visibility.Visible;
            else
                this.shuffleMode.Visibility = Visibility.Collapsed;
        }

        public void SetVisibleButtons(YouTubeInfo info)
        {
            if (info != null)
            {
                this.qualityPicker.Visibility = Visibility.Visible;
                foreach (QualityButtonInfo qualityButton in (Collection<QualityButtonInfo>)this.qualityButtons)
                {
                    bool flag = qualityButton.Quality <= App.HighestQuality 
                        && info.HasQuality(qualityButton.Quality);
                    qualityButton.IsEnabled = flag;
                }
                if (info.AnnotationsLink != null)
                    this.annotations.Visibility = Visibility.Visible;
                else
                    this.annotations.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.qualityPicker.Visibility = Visibility.Collapsed;
                this.menuButton.Text = "S";
            }
        }

        private async void qualityPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.changeQuality || this.qualityPicker == null || DefaultPage.Current == null)
                return;

            VideoPlayer videoPlayer = DefaultPage.Current.VideoPlayer;
            if (videoPlayer == null 
                || !(((Selector)this.qualityPicker).SelectedItem is QualityButtonInfo selectedItem))
                return;
            await videoPlayer.ChangeQuality(selectedItem.Quality);
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
            this.PlaylistButtonPressed((object)this, -1);
        }

        private void nextTapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.PlaylistButtonPressed == null)
                return;
            this.PlaylistButtonPressed((object)this, 1);
        }

        private void lockRotation_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Accel.Locked)
            {
                Accel.Unlock();
                Ani.Begin((DependencyObject)this.lockRotation, "Opacity", (double)((IDictionary<object, object>)((FrameworkElement)this).Resources)[(object)"menuItemFade"], 0.2);
            }
            else
            {
                Accel.Lock();
                Ani.Begin((DependencyObject)this.lockRotation, "Opacity", 1.0, 0.2);
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
            ((IDictionary<string, object>)valueSet).Add("playlistRepeatMode", (object)Settings.PlaylistRepeatMode.ToString());
            ((IDictionary<string, object>)valueSet).Add("normalRepeatMode", (object)Settings.PlaylistRepeatMode.ToString());
            BackgroundMediaPlayer.SendMessageToBackground(valueSet);
            this.SetPlaylistModeIcon();
        }

        private void SetAnnotationsIcon()
        {
            if (!Settings.Annotations)
                Ani.Begin((DependencyObject)this.annotations, "Opacity", (double)((IDictionary<object, object>)((FrameworkElement)this).Resources)[(object)"menuItemFade"], 0.2);
            else
                Ani.Begin((DependencyObject)this.annotations, "Opacity", 1.0, 0.2);
        }

        private void SetShuffleIcon()
        {
            if (!Settings.Shuffle)
                Ani.Begin((DependencyObject)this.shuffleIcon, "Opacity", 
                    (double)((IDictionary<object, object>)(
                    (FrameworkElement)this).Resources)[(object)"menuItemFade"], 0.2);
            else
                Ani.Begin((DependencyObject)this.shuffleIcon, "Opacity", 1.0, 0.2);
        }

        private void SetCaptionsIcon()
        {
            if (!Settings.Subtitles)
                Ani.Begin((DependencyObject)this.captions, "Opacity", (double)((IDictionary<object, object>)((FrameworkElement)this).Resources)[(object)"menuItemFade"], 0.2);
            else
                Ani.Begin((DependencyObject)this.captions, "Opacity", 1.0, 0.2);
        }

        private void SetPlaylistModeIcon()
        {
            if (DefaultPage.Current.VideoPlayer.HasPlaylist)
            {
                switch (Settings.PlaylistRepeatMode)
                {
                    case PlaylistRepeatMode.None:
                        this.playlistIcon.Text = '\uE1CD'.ToString();
                        Ani.Begin((DependencyObject)this.playlistIcon, "Opacity", 
                            (double)((IDictionary<object, object>)(this).Resources)[(object)"menuItemFade"], 0.2);
                        break;
                    case PlaylistRepeatMode.One:
                        this.playlistIcon.Text = '\uE1CC'.ToString();
                        Ani.Begin((DependencyObject)this.playlistIcon, "Opacity", 1.0, 0.2);
                        break;
                    case PlaylistRepeatMode.All:
                        this.playlistIcon.Text = '\uE1CD'.ToString();
                        Ani.Begin((DependencyObject)this.playlistIcon, "Opacity", 1.0, 0.2);
                        break;
                }
            }
            else
            {
                switch (Settings.NormalRepeatMode)
                {
                    case PlaylistRepeatMode.None:
                        this.playlistIcon.Text = '\uE1CC'.ToString();
                        Ani.Begin((DependencyObject)this.playlistIcon, "Opacity", 
                            (double)((IDictionary<object, object>)(this).Resources)[(object)"menuItemFade"], 0.2);
                        break;
                    case PlaylistRepeatMode.One:
                        this.playlistIcon.Text = '\uE1CC'.ToString();
                        Ani.Begin((DependencyObject)this.playlistIcon, "Opacity", 1.0, 0.2);
                        break;
                    case PlaylistRepeatMode.All:
                        this.playlistIcon.Text = '\uE1CD'.ToString();
                        Ani.Begin((DependencyObject)this.playlistIcon, "Opacity", 1.0, 0.2);
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
            e.Handled = true;
            DefaultPage current = DefaultPage.Current;
            if (current == null)
                return;
            //TODO
            //this.fullScreenButton.Symbol = !await current.ToggleFullscreen() ? (Symbol)57817 : (Symbol)57816;
        }


        private void volumeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            if (this.RequestTimerRestart != null)
                this.RequestTimerRestart((object)this, (EventArgs)null);
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
            this.volumeBorder.Opacity = 0.0;
            this.volumeBorder.Visibility = Visibility.Visible;
            this.volumeBorder.IsHitTestVisible = false;
            if (this.MediaElement != null)
                this.volumeSlider.SetRelativeValue(Math.Sqrt(this.MediaElement.Volume));
            Storyboard storyboard = Ani.Begin((DependencyObject)this.volumeBorder, "Opacity", 1.0, 0.4);

            storyboard.Completed += (s, e) =>
            {
                ((UIElement)this.volumeBorder).IsHitTestVisible = true;
            };
        }

        public void HideVolume() => this.hideVolume();

        private void hideVolume()
        {
            if (!this.volumeShown)
                return;
            this.volumeShown = false;
            this.volumeBorder.IsHitTestVisible = false;
            this.volumeBorder.Visibility = Visibility.Collapsed;
        }

        private void volumeSlider_ValueChanged(object sender, SliderValueChangedEventArgs e)
        {
            if (this.VolumeChanged == null)
                return;
            this.VolumeChanged((object)this, new SliderValueChangedEventArgs()
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
            this.RequestTimerRestart((object)this, (EventArgs)null);
        }

        private void captions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Settings.Subtitles = !Settings.Subtitles;
            DefaultPage.Current.VideoPlayer.SetAnnotations();
            this.SetCaptionsIcon();
        }

        private class DisplayClass80_0
        {
            public DisplayClass80_0()
            {
            }

            public MediaPlayer sender { get; set; }
        }              
    }
}
