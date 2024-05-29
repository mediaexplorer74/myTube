// myTube.VideoPlayer

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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using UriTester;
using Windows.Devices.Enumeration;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.Media.Playback;
using Windows.System.Display;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Http;
using WinRTXamlToolkit.AwaitableUI;

namespace myTube
{
    public sealed partial class VideoPlayer : UserControl
    {
        public static object AudioExtensionObject = new object();
        public static double VideoSpeed = 1.0;
        private const double MediaElementOpacity = 0.99;

        public static DependencyProperty SourceProperty 
            = DependencyProperty.Register(nameof(Source), 
            typeof(Uri), typeof(VideoPlayer), 
            new PropertyMetadata((object)null, 
                new PropertyChangedCallback(VideoPlayer.OnSourcePropertyChanged)));

        public static DependencyProperty HiddenProperty 
            = DependencyProperty.Register(nameof(Hidden), 
            typeof(bool), typeof(VideoPlayer), 
            new PropertyMetadata((object)false, 
                new PropertyChangedCallback(VideoPlayer.OnHiddenPropertyChanged)));

        public static DependencyProperty PlaybackOpacityProperty 
            = DependencyProperty.Register(nameof(PlaybackOpacity), 
                typeof(double), typeof(VideoPlayer), 
                new PropertyMetadata((object)0.25, 
                    new PropertyChangedCallback(VideoPlayer.OnPlaybackOpacityPropertyChanged)));

        public static DependencyProperty ControlsShownProperty 
            = DependencyProperty.Register(nameof(ControlsShown), 
                typeof(bool), typeof(VideoPlayer), 
                new PropertyMetadata((object)true, 
                    new PropertyChangedCallback(VideoPlayer.OnControlsShownPropertyChanged)));

        public static DependencyProperty MediaRunningProperty 
            = DependencyProperty.Register(nameof(MediaRunning),
                typeof(bool), typeof(VideoPlayer), 
                new PropertyMetadata((object)false, 
                    new PropertyChangedCallback(VideoPlayer.OnMediaRunningPropertyChanged)));

        private bool focusOnVideo = true;
        private bool openBookmark;
        private VideoInfoLoader loader;
        private TimeSpan seekToOnOpen = TimeSpan.MinValue;
        private readonly TimeSpan WatchTimeLimit = TimeSpan.FromMinutes(60.0);
        private readonly TimeSpan WatchTimeInterval = TimeSpan.FromHours(3.0);
        private readonly TimeSpan WatchTimeCooldown = TimeSpan.FromMinutes(5.0);
        private DateTimeOffset StartedWatchingAt = DateTimeOffset.MinValue;
        private DateTimeOffset TrialEndedAt = DateTimeOffset.MinValue;
        private bool RunningTrial;
        private bool WaitingForTrialCooldown;
        private Stopwatch WatchStopwatch;
        private static bool initializedAudio = false;
        private bool warnedAboutData;
        private bool warnedAboutSwitchToData;
        private const string Tag = "VideoPlayer";
        private const string CastingTag = "Casting";
        private TaskCompletionSource<bool> tcs;
        private int tries;
        private YouTubeInfo attemptedInfo;
        private YouTubeInfo successfulInfo;
        private YouTubeEntry lastEntry;
        private YouTubeEntry currentEntry;
        private YouTubeQuality quality = YouTubeQuality.HD;
        private List<AnnotationInfo> annotations;
        private ObservableCollection<AnnotationInfo> visibleAnnotations 
            = new ObservableCollection<AnnotationInfo>();
        private ObservableCollection<Subtitle> visibleSubtitles = new ObservableCollection<Subtitle>();
        private Subtitle[] subtitles;
        private DispatcherTimer annotationsTimer;
        public static readonly TimeSpan KeepControlsOpenFor = TimeSpan.FromSeconds(3.0);
        private Stopwatch controlsWatch;
        private TransferManager.State lastState;
        private Storyboard openControlsAni;
        private Storyboard closeControlsAni;
        private Storyboard showMediaAni;
        private Storyboard showMediaAni99;
        private Storyboard hideMediaAni;
        private Storyboard fadeMediaAni;
        private DisplayRequest displayRequest;
        private TypeConstructor typeConstructor;
        private PlaylistHelper playlistHelper;
        private static MediaElement audioElement;
        private static bool audioElementCreated = false;
        private ObservableCollection<DeviceViewModel> devices;
        private DispatcherTimer castingTimer;
        private Random random = new Random();
        private int seed;
        private static bool useSpeedTimer = true;
        private static bool useRelativeSpeed = true;
        private static DispatcherTimer speedTimer;
        private static DispatcherTimer checkTimer;
        private bool openedOnce;
        private static Popup audioPopup;
        private const double MaxMediaElementDifference = 0.07;
        private const double HugeMediaElementDifference = 1.0;
        private const double MaxMediaElementDifferenceInaccurate = 0.1;
        private const double MaxMediaElementRate = 2.0;
        private static bool settingOffset = false;
        private static MediaElementState lastAudioState = (MediaElementState)0;
        private TypeConstructor lastUsedTypeConstructor;
        private TimeSpan? unloadedPos;
        private bool loaded;
        private bool backgroundEvents;
        private CompositeTransform mediaTrans = new CompositeTransform();
        private SimpleOrientation lastOr;
        private CancellationTokenSource cancel;
        private bool thumbShown;
        private string lastEntryId;
        private bool firstOffset;
        private static TimeSpan offset = TimeSpan.FromSeconds(0.0);
        private MediaElementState lastMediaElementState;
                                           
        //TEMP
        //private PlayerControls controls = new PlayerControls();

        private static void OnSourcePropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            VideoPlayer videoPlayer = d as VideoPlayer;
            if (videoPlayer.mediaElement == null)
                return;
            videoPlayer.mediaElement.Source = (Uri)e.NewValue;
        }

        private static void OnMediaRunningPropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            VideoPlayer videoPlayer = d as VideoPlayer;
            int num1 = (bool)e.NewValue ? 1 : 0;
            if (num1 != 0)
                (d as VideoPlayer).displayRequest.RequestActive();
            else
                (d as VideoPlayer).displayRequest.RequestRelease();
            if (num1 == 0)
            {
                if (videoPlayer.Hidden)
                    videoPlayer.hideMediaAni.Begin();
                ((UIElement)videoPlayer.stopButton).Visibility = Visibility.Collapsed;
            }
            else
            {
                if (App.DeviceFamily == DeviceFamily.Mobile)
                {
                    int num2 = Settings.ResumeAsAudio ? 1 : 0;
                }
                if (videoPlayer.Hidden)
                    videoPlayer.fadeMediaAni.Begin();
                else
                    videoPlayer.showMediaAni99.Begin();
                if (App.DeviceFamily == DeviceFamily.Desktop)
                    videoPlayer.stopButton.Visibility = Visibility.Visible;
                else
                    videoPlayer.stopButton.Visibility = Visibility.Collapsed;
            }
            if ((d as VideoPlayer).MediaRunningChanged == null)
                return;
            (d as VideoPlayer).MediaRunningChanged((object)d, new MediaRunningChangedEventArgs()
            {
                MediaRunning = (bool)e.NewValue,
                FocusOnVideo = (d as VideoPlayer).focusOnVideo
            });
        }

        private static async void OnControlsShownPropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            VideoPlayer vp = d as VideoPlayer;
            bool newValue = (bool)e.NewValue;
            try
            {
                if (vp.controls.RenderTransform == null)
                {
                    ScaleTransform scaleTransform = new ScaleTransform();
                }
                if (newValue)
                {
                    if (vp.titleGrid.Opacity > 0.0)
                        vp.titleGrid.Visibility = Visibility.Visible;
                    vp.controls.StartTimer();
                    vp.Controls.UpdateFullscreenButton();
                    vp.setControlsRenderTransformOrigin();
                    vp.Controls.Visibility = Visibility.Visible;
                    vp.openControlsAni.Begin();
                    vp.controlsWatch.Restart();
                    Ani.Begin((DependencyObject)vp.titleTrans, "Y", 0.0, 0.5, 
                        (EasingFunctionBase)Ani.Ease((EasingMode)0, 6.0));
                }
                else
                {
                    vp.controls.StopTimer();
                    vp.closeControlsAni.Begin();
                    vp.controls.CloseMenu();
                    vp.Controls.HideVolume();
                    vp.controlsWatch.Reset();
                   
                    Storyboard storyboard = Ani.Begin((DependencyObject)vp.titleTrans, "Y", -130.0, 0.3, 
                        (EasingFunctionBase)Ani.Ease((EasingMode)1, 4.0));
                    
                    storyboard.Completed += (s, e1) =>
                    {
                        if (vp.ControlsShown)
                            return;
                        vp.titleGrid.Visibility = Visibility.Collapsed;
                    };
                }
                if (vp.PlayerControlsShownChanged == null)
                    return;
                vp.PlayerControlsShownChanged((object)vp, newValue);
            }
            catch
            {
            }
        }

        private static void OnPlaybackOpacityPropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnHiddenPropertyChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            try
            {
                VideoPlayer videoPlayer = d as VideoPlayer;
                bool newValue = (bool)e.NewValue;
                Helper.Write((object)nameof(VideoPlayer), (object)("Hidden changed to " + newValue.ToString()));
                if (newValue)
                {
                    if (App.DeviceFamily == DeviceFamily.Mobile)
                        ApplicationView.GetForCurrentView().SuppressSystemOverlays = false;

                    Ani.Begin((DependencyObject)videoPlayer.blurRectangle, "Opacity", 1.0, 0.3);
                    if (!videoPlayer.openedOnce)
                        Ani.Begin((DependencyObject)videoPlayer.noVideoText, "Opacity", 0.0, 0.2);
                    if (videoPlayer.mediaElement == null)
                        return;
                    if (videoPlayer.MediaRunning)
                    {
                        Helper.Write((object)nameof(VideoPlayer), (object)"Fading mediaElement");
                        videoPlayer.fadeMediaAni.Begin();
                    }
                    else
                    {
                        Helper.Write((object)nameof(VideoPlayer), (object)"Hiding mediaElement");
                        videoPlayer.hideMediaAni.Begin();
                    }
                    if (!videoPlayer.thumbShown)
                        return;
                    Ani.Begin((DependencyObject)videoPlayer.musicThumb, "Opacity", videoPlayer.PlaybackOpacity, 0.1);
                }
                else
                {
                    if (App.DeviceFamily == DeviceFamily.Mobile)
                        ApplicationView.GetForCurrentView().SuppressSystemOverlays = true;

                    Storyboard storyboard = Ani.Begin((DependencyObject)videoPlayer.blurRectangle, 
                        "Opacity", 0.0, 0.2);

                    
                    storyboard.Completed += (s, e_0) =>
                    {
                        // Do nothing
                    };

                    if (videoPlayer.mediaElement == null)
                        return;
                    if (!videoPlayer.openedOnce)
                        Ani.Begin((DependencyObject)videoPlayer.noVideoText, "Opacity", 1.0, 0.25);
                    Helper.Write((object)nameof(VideoPlayer), (object)"Showing mediaElement");
                    videoPlayer.showMediaAni99.Begin();
                    if (!videoPlayer.thumbShown)
                        return;
                    Ani.Begin((DependencyObject)videoPlayer.musicThumb, "Opacity", 1.0, 0.1);
                }
            }
            catch
            {
            }
        }

        public Uri Source
        {
            get => (Uri)((DependencyObject)this).GetValue(VideoPlayer.SourceProperty);
            set => ((DependencyObject)this).SetValue(VideoPlayer.SourceProperty, (object)value);
        }

        public bool Hidden
        {
            get => (bool)((DependencyObject)this).GetValue(VideoPlayer.HiddenProperty);
            set => ((DependencyObject)this).SetValue(VideoPlayer.HiddenProperty, (object)value);
        }

        public bool ControlsShown
        {
            get => (bool)((DependencyObject)this).GetValue(VideoPlayer.ControlsShownProperty);
            set => ((DependencyObject)this).SetValue(VideoPlayer.ControlsShownProperty, (object)value);
        }

        public double PlaybackOpacity
        {
            get => (double)((DependencyObject)this).GetValue(VideoPlayer.PlaybackOpacityProperty);
            set => ((DependencyObject)this).SetValue(VideoPlayer.PlaybackOpacityProperty, (object)value);
        }


        //RnD it
        /*
        public event TypedEventHandler<VideoPlayer, MediaEndedEventArgs> MediaEnded
        {
            add
            {
                TypedEventHandler<VideoPlayer, MediaEndedEventArgs> typedEventHandler1 = this.MediaEnded;
                TypedEventHandler<VideoPlayer, MediaEndedEventArgs> typedEventHandler2;
                do
                {
                    typedEventHandler2 = typedEventHandler1;
                    typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<VideoPlayer, MediaEndedEventArgs>>(ref this.MediaEnded, (TypedEventHandler<VideoPlayer, MediaEndedEventArgs>)Delegate.Combine((Delegate)typedEventHandler2, (Delegate)value), typedEventHandler2);
                }
                while (typedEventHandler1 != typedEventHandler2);
            }
            remove
            {
                TypedEventHandler<VideoPlayer, MediaEndedEventArgs> typedEventHandler1 = this.MediaEnded;
                TypedEventHandler<VideoPlayer, MediaEndedEventArgs> typedEventHandler2;
                do
                {
                    typedEventHandler2 = typedEventHandler1;
                    typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<VideoPlayer, MediaEndedEventArgs>>(ref this.MediaEnded, (TypedEventHandler<VideoPlayer, MediaEndedEventArgs>)Delegate.Remove((Delegate)typedEventHandler2, (Delegate)value), typedEventHandler2);
                }
                while (typedEventHandler1 != typedEventHandler2);
            }
        }

        public event TypedEventHandler<VideoPlayer, MediaOpenedEventArgs> MediaOpened
        {
            add
            {
                TypedEventHandler<VideoPlayer, MediaOpenedEventArgs> typedEventHandler1 = this.MediaOpened;
                TypedEventHandler<VideoPlayer, MediaOpenedEventArgs> typedEventHandler2;
                do
                {
                    typedEventHandler2 = typedEventHandler1;
                    typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>>(ref this.MediaOpened, (TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>)Delegate.Combine((Delegate)typedEventHandler2, (Delegate)value), typedEventHandler2);
                }
                while (typedEventHandler1 != typedEventHandler2);
            }
            remove
            {
                TypedEventHandler<VideoPlayer, MediaOpenedEventArgs> typedEventHandler1 = this.MediaOpened;
                TypedEventHandler<VideoPlayer, MediaOpenedEventArgs> typedEventHandler2;
                do
                {
                    typedEventHandler2 = typedEventHandler1;
                    typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>>(ref this.MediaOpened, (TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>)Delegate.Remove((Delegate)typedEventHandler2, (Delegate)value), typedEventHandler2);
                }
                while (typedEventHandler1 != typedEventHandler2);
            }
        }
        */

        public PlayerControls Controls
        {
            get
            {
                return this.controls;
            }
        }

        public YouTubeQuality CurrentQuality => this.quality;

        public MediaElement MediaElement => this.mediaElement;

        public YouTubeEntry CurrentEntry => this.currentEntry;

        public bool MediaRunning
        {
            get => (bool)((DependencyObject)this).GetValue(VideoPlayer.MediaRunningProperty);
            private set => ((DependencyObject)this).SetValue(VideoPlayer.MediaRunningProperty, (object)value);
        }

        public event EventHandler<MediaRunningChangedEventArgs> MediaRunningChanged;

        public event EventHandler<bool> PlayerControlsShownChanged;

        public Stopwatch ControlsWatch => this.controlsWatch;

        public bool HasPlaylist => this.playlistHelper != null;

        public static MediaElement AudioElement => VideoPlayer.audioElement;

        public object MediaEnded { get; private set; }

        public VideoPlayer()
        {
            Helper.Write((object)nameof(VideoPlayer), (object)"Constructor");
            System.Diagnostics.Debug.WriteLine(nameof(VideoPlayer), " Constructor");

            this.InitializeComponent();

            // inline text 
            try
            {
                this.titleText.Text = "videos.player.novideo";// App.Strings["videos.player.novideo", "no video"].ToLower();
            }
            catch
            {
                this.titleText.Text = "videos.player.novideo";
            }

            if (DefaultPage.Current != null)
                DefaultPage.Current.LockRotation((SimpleOrientation)0);

            Helper.Write((object)nameof(VideoPlayer), (object)"Initialized component");
            System.Diagnostics.Debug.WriteLine(nameof(VideoPlayer), " Initialized component");

            this.controlsWatch = new Stopwatch();
            Helper.Write((object)nameof(VideoPlayer), (object)"Created video player stopwatch");
            System.Diagnostics.Debug.WriteLine(nameof(VideoPlayer), " Created video player stopwatch");

            this.displayRequest = new DisplayRequest();
            Helper.Write((object)nameof(VideoPlayer), (object)"Created display request");
            System.Diagnostics.Debug.WriteLine(nameof(VideoPlayer), " Created display request");

            this.UseLayoutRounding = false;
            Helper.Write((object)nameof(VideoPlayer), (object)"Disabled layout rounding");
            System.Diagnostics.Debug.WriteLine(nameof(VideoPlayer), " Disabled layout rounding");

            this.loader = new VideoInfoLoader();
            Helper.Write((object)nameof(VideoPlayer), (object)"Created video info loader");
            System.Diagnostics.Debug.WriteLine(nameof(VideoPlayer), " Created video info loader");

            // FrameworkElement
            this.Unloaded += this.VideoPlayer_Unloaded;

            Grid menuGrid = this.controls.MenuGrid;
            menuGrid.PointerEntered += this.ControlsPointerEntered;

            Grid mainGrid = this.controls.MainGrid;
            mainGrid.PointerEntered += this.ControlsPointerEntered;

            DispatcherTimer timer = this.controls.Timer;
            timer.Tick += this.Timer_Tick;

            this.controls.MenuShownChanged 
                += new EventHandler<bool>(this.controls_MenuShownChanged);
            this.controls.SeekingChanged 
                += new EventHandler<bool>(this.controls_SeekingChanged);
            this.controls.PlaylistButtonPressed 
                += new PlaylistOffsetEventHandler(this.controls_PlaylistButtonPressed);
            this.controls.VolumeChanged 
                += new EventHandler<SliderValueChangedEventArgs>(this.Controls_VolumeChanged);
            this.controls.RequestTimerRestart 
                += new EventHandler(this.Controls_RequestTimerRestart);

            this.Loaded += this.VideoPlayer_Loaded;
            Helper.Write((object)nameof(VideoPlayer), (object)"End of constructor");

            this.WatchStopwatch = new Stopwatch();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.25);
            this.annotationsTimer = dispatcherTimer;
            DispatcherTimer annotationsTimer = this.annotationsTimer;

            this.annotationsTimer.Tick += this.annotationsTimer_Tick;

            this.castingControl.Visibility = Visibility.Collapsed;
            
            this.castingTimer = new DispatcherTimer();
            this.castingTimer.Interval = TimeSpan.FromSeconds(1.0);
            DispatcherTimer castingTimer = this.castingTimer;

            castingTimer.Tick += (sender, e) 
                => this.CastingTimer_Tick(sender, e);
        }

        public void SetVideoSpeed(double speed)
        {
            VideoPlayer.VideoSpeed = speed;
            this.mediaElement.PlaybackRate = speed;
            VideoPlayer.audioElement.PlaybackRate = speed;
            BackgroundMediaPlayer.Current.PlaybackRate = speed;
        }

        private void setShuffle()
        {
            if (this.playlistHelper == null)
                return;
            this.playlistHelper.Shuffle = Settings.Shuffle;
        }

        private void CastingTimer_Tick(object sender, object e)
        {
        }

        private void Controls_RequestTimerRestart(object sender, EventArgs e) => this.controlsWatch.Restart();

        private void Controls_VolumeChanged(object sender, SliderValueChangedEventArgs e)
        {
            this.controlsWatch.Restart();
            Settings.Volume = MyMath.Clamp(e.NewValue, 0.05, 1.0);
            MediaElement mediaElement = this.MediaElement;
            double newValue;
            VideoPlayer.audioElement.Volume = newValue = e.NewValue;
            double num = newValue;
            mediaElement.Volume = num;
        }

        private void setSelectedDevice(DeviceViewModel dvm)
        {
            if (this.devices == null)
                return;
            foreach (DeviceViewModel device in (Collection<DeviceViewModel>)this.devices)
                device.Selected = dvm == device;
        }

        private void setSelectedDevice(DeviceInformation dvm)
        {
            if (this.devices == null)
                return;
            foreach (DeviceViewModel device in (Collection<DeviceViewModel>)this.devices)
                device.Selected = dvm != null && dvm == device.Device;
        }

        public void SetMargins(Rect visibleBounds, Rect windowBounds)
        {
        }

        public static async Task InitializeBackgroundAudio()
        {
            if (VideoPlayer.initializedAudio)
                return;
            VideoPlayer.initializedAudio = true;
            BackgroundMediaPlayer.Current.Volume = 1.0;
            await Task.Delay(100);
        }

        private void annotationsTimer_Tick(object sender, object e) => this.SetAnnotations();

        public void SetAnnotations()
        {
            if (Settings.Annotations)
            {
                if (this.annotations != null && this.quality != YouTubeQuality.Audio)
                {
                    this.annotationsControl.Visibility = Visibility.Visible;
                    if (this.mediaElement.CurrentState == (MediaElementState)3 
                        || this.mediaElement.CurrentState == (MediaElementState)4)
                    {
                        TimeSpan position = this.mediaElement.Position;
                        using (List<AnnotationInfo>.Enumerator enumerator = this.annotations.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                AnnotationInfo current = enumerator.Current;
                                if (current.StartTime <= position && current.EndTime >= position)
                                {
                                    if (!((Collection<AnnotationInfo>)this.visibleAnnotations).Contains(current))
                                        ((Collection<AnnotationInfo>)this.visibleAnnotations).Add(current);
                                }
                                else if (((Collection<AnnotationInfo>)this.visibleAnnotations).Contains(current))
                                    ((Collection<AnnotationInfo>)this.visibleAnnotations).Remove(current);
                            }
                        }
                    }
                }
                else
                {
                    this.visibleAnnotations.Clear();
                    this.annotationsControl.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.annotationsControl.Visibility = Visibility.Collapsed;
                this.visibleAnnotations.Clear();
            }
            if (Settings.Subtitles)
            {
                if (this.subtitles != null && this.quality != YouTubeQuality.Audio)
                {
                    this.subtitlesControl.Visibility = Visibility.Visible;
                    if (this.mediaElement.CurrentState == (MediaElementState)3 
                        || this.mediaElement.CurrentState == (MediaElementState)4)
                    {
                        TimeSpan position = this.mediaElement.Position;
                        foreach (Subtitle subtitle in this.subtitles)
                        {
                            if (subtitle.StartTime <= position && subtitle.EndTime >= position)
                            {
                                if (!this.visibleSubtitles.Contains(subtitle))
                                    this.visibleSubtitles.Add(subtitle);
                            }
                            else if (this.visibleSubtitles.Contains(subtitle))
                                this.visibleSubtitles.Remove(subtitle);
                        }
                    }
                }
                else
                {
                    this.visibleSubtitles.Clear();
                    this.subtitlesControl.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                this.visibleSubtitles.Clear();
                this.subtitlesControl.Visibility = Visibility.Collapsed;
            }

            if (this.annotationsControl.Visibility == null || this.subtitlesControl.Visibility == null)
                this.annotationsViewBox.Visibility = Visibility.Visible;
            else
                this.annotationsViewBox.Visibility = Visibility.Collapsed;
        }

        public static void ShowOrHidAudioPopup(bool show)
        {
            VideoPlayer.audioPopup.IsOpen = show;
        }

        private static void CreateAudioAlement(MediaElement m)
        {
            if (VideoPlayer.audioElementCreated)
                return;
            VideoPlayer.audioElementCreated = true;
            MediaElement mediaElement = new MediaElement();

            mediaElement.IsHitTestVisible = false;
            mediaElement.AutoPlay = false;
            mediaElement.AudioCategory = 
                App.DeviceFamily == DeviceFamily.Desktop ? (AudioCategory)2 : (AudioCategory)1;

            VideoPlayer.audioElement = mediaElement;
            VideoPlayer.audioElement.Volume = Settings.Volume;

            VideoPlayer.audioElement.SeekCompleted += VideoPlayer.audioElement_SeekCompleted;
            VideoPlayer.audioElement.CurrentStateChanged += VideoPlayer.audioElement_CurrentStateChanged;

            Popup popup = new Popup();
            popup.IsHitTestVisible = false;
            popup.Opacity = 0.0;
            popup.Child = VideoPlayer.audioElement;
            popup.Visibility = Visibility.Collapsed;
            popup.IsLightDismissEnabled = false;
            VideoPlayer.audioPopup = popup;
            VideoPlayer.audioPopup.IsOpen = true;
            VideoPlayer.speedTimer = new DispatcherTimer();
            VideoPlayer.speedTimer.Interval = TimeSpan.FromSeconds(0.04);
            DispatcherTimer speedTimer = VideoPlayer.speedTimer;

            VideoPlayer.speedTimer.Tick += VideoPlayer.speedTimer_Tick;

            VideoPlayer.checkTimer = new DispatcherTimer();
            VideoPlayer.checkTimer.Interval = TimeSpan.FromSeconds(1.0);
            DispatcherTimer checkTimer = VideoPlayer.checkTimer;

            VideoPlayer.checkTimer.Tick += VideoPlayer.checkTimer_Tick;
        }

        private static void checkTimer_Tick(object sender, object e) => VideoPlayer.syncElements();

        private static async void speedTimer_Tick(object sender, object e)
        {
            VideoPlayer videoPlayer = DefaultPage.Current.VideoPlayer;
            MediaElement mediaElement = DefaultPage.Current.VideoPlayer.MediaElement;
            if (!VideoPlayer.audioElement.HasMedia())
            {
                mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
                VideoPlayer.speedTimer.Stop();
            }
            else
            {
                if (VideoPlayer.audioElement.PlaybackRate != VideoPlayer.VideoSpeed)
                    VideoPlayer.audioElement.PlaybackRate = VideoPlayer.VideoSpeed;
                TimeSpan timeSpan = VideoPlayer.audioElement.Position - mediaElement.Position;
                double totalSeconds1 = timeSpan.TotalSeconds;
                bool flag = videoPlayer.successfulInfo != null 
                    && videoPlayer.successfulInfo.QualityIs60FPS(videoPlayer.quality);
                double diff = flag ? 0.1 : 0.07;
                if (Math.Abs(totalSeconds1) > diff)
                {
                    if (!VideoPlayer.useRelativeSpeed)
                    {
                        double num = 2.0;
                        if (totalSeconds1 > 0.0)
                            mediaElement.PlaybackRate = num * VideoPlayer.VideoSpeed;
                        else
                            mediaElement.PlaybackRate = 1.0 * VideoPlayer.VideoSpeed / num;
                    }
                    else
                    {
                        double num = MyMath.Clamp((1.0 + Math.Abs(totalSeconds1) * 2.0)
                            .Round(0.5, RoundDirection.Up), 1.0, 2.0) * VideoPlayer.VideoSpeed;
                        if (totalSeconds1 < 0.0)
                            num = VideoPlayer.VideoSpeed / num;
                        if (mediaElement.PlaybackRate == num)
                            return;
                        mediaElement.PlaybackRate = num;
                    }
                }
                else
                {
                    if (mediaElement.PlaybackRate != VideoPlayer.VideoSpeed)
                        mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
                    VideoPlayer.speedTimer.Stop();
                    if (flag)
                        return;
                    await Task.Delay(1000);
                    timeSpan = VideoPlayer.audioElement.Position - mediaElement.Position;
                    double totalSeconds2 = timeSpan.TotalSeconds;
                    if (Math.Abs(totalSeconds2) <= diff)
                        return;
                    if (Math.Abs(totalSeconds2) > 1.0)
                    {
                        VideoPlayer.audioElement.Position = mediaElement.Position;
                        await Task.Delay(1000);
                    }
                    VideoPlayer.speedTimer.Start();
                }
            }
        }

        private static async void syncElements()
        {
            VideoPlayer videoPlayer = DefaultPage.Current.VideoPlayer;
            MediaElement mediaElement = DefaultPage.Current.VideoPlayer.mediaElement;
            double maxDiff = videoPlayer.successfulInfo != null 
                && videoPlayer.successfulInfo.QualityIs60FPS(videoPlayer.quality) 
                ? 0.1 
                : 0.07;

            if (mediaElement.CurrentState == (MediaElementState)5 || mediaElement.CurrentState == null)
            {
                VideoPlayer.audioElement.Stop();
            }
            else
            {
                if (mediaElement.CurrentState == (MediaElementState)3 && VideoPlayer.audioElement.HasMedia()
                    && VideoPlayer.audioElement.CurrentState != (MediaElementState)3 
                    && VideoPlayer.audioElement.CurrentState != (MediaElementState)1)
                {
                    if (VideoPlayer.audioElement.CurrentState != (MediaElementState)2)
                    {
                        try
                        {
                            VideoPlayer.audioElement.Play();
                            return;
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
                if (!VideoPlayer.audioElement.HasMedia())
                    mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
                if (mediaElement.CurrentState == (MediaElementState)2 
                    || mediaElement.CurrentState == (MediaElementState)1)
                    await Task.Delay(VideoPlayer.useSpeedTimer ? 1000 : 2000);
                else
                    await Task.Delay(250);
                if (VideoPlayer.useSpeedTimer)
                {
                    double num = Math.Abs((VideoPlayer.audioElement.Position - mediaElement.Position).TotalSeconds);
                    if (num > maxDiff && num < 1.0)
                    {
                        VideoPlayer.speedTimer.Start();
                        if (VideoPlayer.checkTimer.IsEnabled)
                            return;
                        VideoPlayer.checkTimer.Start();
                    }
                    else
                    {
                        if (num <= 1.0)
                            return;
                        VideoPlayer.speedTimer.Stop();
                        VideoPlayer.audioElement.Position = mediaElement.Position;
                    }
                }
                else if (!VideoPlayer.settingOffset)
                {
                    if (mediaElement.CurrentState == (MediaElementState)3 
                        && Math.Abs((VideoPlayer.audioElement.Position - mediaElement.Position).TotalSeconds) > 0.1)
                    {
                        VideoPlayer.audioElement.Volume = 0.0;
                        VideoPlayer.settingOffset = true;
                        VideoPlayer.audioElement.Position = mediaElement.Position;
                    }
                    else
                        VideoPlayer.audioElement.Volume = mediaElement.Volume;
                }
                else
                {
                    VideoPlayer.settingOffset = false;
                    if (mediaElement.CurrentState == (MediaElementState)3  && 
                        Math.Abs((VideoPlayer.audioElement.Position - mediaElement.Position).TotalSeconds) > 0.07)
                    {
                        VideoPlayer.audioElement.Volume = 0.0;
                        await Task.Delay(250);
                        VideoPlayer.offset = mediaElement.Position - VideoPlayer.audioElement.Position;
                        VideoPlayer.audioElement.Position = mediaElement.Position + VideoPlayer.offset;
                    }
                    else
                        VideoPlayer.audioElement.Volume = mediaElement.Volume;
                }
            }
        }

        private static void audioElement_SeekCompleted(object sender, RoutedEventArgs e)
        {
            if (!VideoPlayer.checkTimer.IsEnabled)
                VideoPlayer.checkTimer.Start();
            VideoPlayer.syncElements();
        }

        private static void audioElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.audioElement.CurrentState == VideoPlayer.lastAudioState)
                return;
            VideoPlayer.lastAudioState = VideoPlayer.audioElement.CurrentState;
            if (DefaultPage.Current.VideoPlayer.successfulInfo == null)
                return;
            MediaElement mediaElement = DefaultPage.Current.VideoPlayer.mediaElement;
            switch (VideoPlayer.audioElement.CurrentState - 1)
            {
                case (MediaElementState)0:
                    VideoPlayer.checkTimer.Stop();
                    break;
                case (MediaElementState)2:
                    VideoPlayer.syncElements();
                    if (VideoPlayer.checkTimer.IsEnabled)
                        break;
                    VideoPlayer.checkTimer.Start();
                    break;
                case (MediaElementState)3:
                    VideoPlayer.checkTimer.Stop();
                    break;
                case (MediaElementState)4:
                    VideoPlayer.checkTimer.Stop();
                    break;
                default:
                    VideoPlayer.checkTimer.Stop();
                    break;
            }
        }

        private async void createPlaylistHelper()
        {
            if (this.typeConstructor != null)
            {
                if (this.lastUsedTypeConstructor == null || this.typeConstructor != this.lastUsedTypeConstructor)
                {
                    this.seed = this.random.Next(0, 121);
                    object client = this.typeConstructor.Construct();
                    if (client is VideoListClient)
                    {
                        PlaylistHelper playlistHelper = new PlaylistHelper((YouTubeClient<YouTubeEntry>)(client as VideoListClient));
                        playlistHelper.ShuffleSeed = this.seed;
                        this.playlistHelper = playlistHelper;
                        this.playlistHelper.SetIndexBasedOnID(this.lastEntry.ID);
                        await this.playlistHelper.Load();
                        this.controls.ShowPlaylistPanel();
                    }
                    else
                    {
                        this.controls.HidePlaylistPanel();
                        this.playlistHelper = (PlaylistHelper)null;
                    }
                }
            }
            else
            {
                this.controls.HidePlaylistPanel();
                this.playlistHelper = (PlaylistHelper)null;
            }
            this.lastUsedTypeConstructor = this.typeConstructor;
        }

        private void controls_PlaylistButtonPressed(object sender, int offset)
        {
            if (this.playlistHelper == null)
                return;
            this.setShuffle();
            YouTubeEntry entry = this.playlistHelper.GetEntry(offset);
            this.progress.IsIndeterminate = true;
            this.callMediaEnded(new MediaEndedEventArgs()
            {
                ContinuingWith = entry
            });
            this.OpenVideo(entry, this.quality);
            if (!this.ControlsShown || !this.controlsWatch.IsRunning)
                return;
            this.controlsWatch.Restart();
        }

        private void Current_CurrentStateChanged(MediaPlayer sender, object args)
        {
        }

        private async void BackgroundMediaPlayer_MessageReceivedFromBackground(
          object sender,
          MediaPlayerDataReceivedEventArgs e)
        {

            MediaOpened mediaOpened = new VideoPlayer.MediaOpened();
           
            mediaOpened.u003E4 = this;
            mediaOpened.e = e;
            Helper.Write((object)nameof(VideoPlayer), (object)"Message received from background");
            mediaOpened.success = false;
            if (((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("seed") 
                && ((IDictionary<string, object>)mediaOpened.e.Data)["seed"] != null)
            {
                this.seed = (int)((IDictionary<string, object>)mediaOpened.e.Data)["seed"];
            }
            if (((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("success"))
            {
                if (((IDictionary<string, object>)mediaOpened.e.Data)["success"] is bool && this.tcs != null)
                {
                    this.tcs.TrySetResult(mediaOpened.success 
                        = (bool)((IDictionary<string, object>)mediaOpened.e.Data)["success"]);

                    Helper.Write((object)nameof(VideoPlayer), 
                        (object)string.Format
                        ("Received success value from background audio {0}", 
                        (object)mediaOpened.success));
                   
                    if (mediaOpened.success && 
                        ((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("entry"))
                    {
                        this.currentEntry = this.lastEntry = 
                            new YouTubeEntry(((IDictionary<string, object>)mediaOpened.e.Data)["entry"] as string);

                        //RnD
                        //this.MediaOpened.Invoke(this, new MediaOpenedEventArgs() 
                        //{ 
                        //    //RnD
                        //    //CurrentEntry = this.currentEntry 
                        //});
                    }
                    this.tcs = (TaskCompletionSource<bool>)null;
                }
                await ((DependencyObject)this).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    BackgroundMediaPlayer_MessageReceivedFromBackground(mediaOpened, null);
                });
            }
            
        
            if (((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("entry"))
            {
             
                YouTubeEntry ent = new YouTubeEntry(((IDictionary<string, object>)mediaOpened.e.Data)
                    ["entry"] as string);
              
                if (((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("needsRefresh") 
                    && (bool)((IDictionary<string, object>)mediaOpened.e.Data)["needsRefresh"])
                {
                    try
                    {
                        if ((await new YouTubeEntryClient().GetBatchedJson(ent.ID))["items"] is JArray jarray)
                        {
                            if (jarray.Count > 0)
                                ent.SetValuesFromJson(jarray[0]);
                        }
                    }
                    catch
                    {
                    }
                }
                this.currentEntry = this.lastEntry = ent;

                await ((DependencyObject)this).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                   BackgroundMediaPlayer_MessageReceivedFromBackground(this, null);
                });

                ent = (YouTubeEntry)null;
            }
            // ISSUE: reference to a compiler-generated field
            if (((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("infoXML"))
            {

                XElement x = XElement.Parse(((IDictionary<string, object>)mediaOpened.e.Data)["infoXML"] as string);

                this.successfulInfo = this.attemptedInfo = new YouTubeInfo(x.Value,
                    x.GetBool("Watch", true), x.GetBool("Cipher"), x.GetBool("NavigatePage"));

                await ((DependencyObject)this).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    BackgroundMediaPlayer_MessageReceivedFromBackground(this, null);
                });
            }
            
            if (((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("error") 
                && ((IDictionary<string, object>)mediaOpened.e.Data)["error"] is string)
            {
                await ((DependencyObject)this).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    BackgroundMediaPlayer_MessageReceivedFromBackground(mediaOpened, null);
                });
            }
            
            if (!((IDictionary<string, object>)mediaOpened.e.Data).ContainsKey("ended"))
                return;

            await ((DependencyObject)this).Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                BackgroundMediaPlayer_MessageReceivedFromBackground(this, null);
            });
        }

        private void VideoPlayer_Unloaded(object sender, RoutedEventArgs e)
        {
            //
        }

        public void SetUnloadPos()
        {
            if (this.mediaElement.CurrentState == (MediaElementState)2 
                || this.mediaElement.CurrentState == (MediaElementState)3 
                || this.mediaElement.CurrentState == (MediaElementState)4)
            {
                Helper.Write((object)nameof(VideoPlayer), 
                    (object)("MediaElement unloaded at position " + (object)this.mediaElement.Position));

                this.unloadedPos = new TimeSpan?(this.mediaElement.Position);
            }
            else
                this.unloadedPos = new TimeSpan?();
        }

        private async void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            Helper.Write((object)nameof(VideoPlayer), (object)"Loaded");
            if (this.loaded)
                return;

            this.loaded = true;

            this.FontFamily = DefaultPage.Current.FontFamily;

            try
            {
                this.controls.MediaElement = this.mediaElement;
                this.mediaElement.CurrentStateChanged += this.mediaElement_CurrentStateChanged;
                this.mediaElement.MediaOpened += this.mediaElement_MediaOpened;
                this.mediaElement.MediaFailed += this.mediaElement_MediaFailed;
                this.mediaElement.MediaEnded += this.mediaElement_MediaEnded;



                if (App.DeviceFamily == DeviceFamily.Mobile)
                    this.setMediaElementScale(VideoPlayer.ScaleType.Screen, 1.0);
                if (App.DeviceFamily != DeviceFamily.Desktop)
                    this.mediaElement.AudioCategory = AudioCategory.ForegroundOnlyMedia;

                VideoPlayer.CreateAudioAlement(this.mediaElement);
            }
            catch
            {
            }
            this.openControlsAni = Ani.Animation(Ani.DoubleAni(
                this.controls.RenderTransform, "ScaleX", 1.0, 0.45, Ani.Ease(EasingMode.EaseOut, 7.0)), 
                Ani.DoubleAni(this.controls.RenderTransform, "ScaleY", 
                1.0, 0.45, Ani.Ease(EasingMode.EaseOut, 7.0)), 
                Ani.DoubleAni(this.controls, "Opacity", 1.0, 0.15));

            this.closeControlsAni = Ani.Animation(Ani.DoubleAni(
                this.controls.RenderTransform, "ScaleX", 0.65, 0.2, Ani.Ease(EasingMode.EaseIn, 4.0)), 
                Ani.DoubleAni(this.controls.RenderTransform, "ScaleY", 
                0.9, 0.2, Ani.Ease(EasingMode.EaseIn, 4.0)),
                Ani.DoubleAni(this.controls, "Opacity", 0.0, 0.2));

            Storyboard closeControlsAni = this.closeControlsAni;

            closeControlsAni.Completed += (s_1, e_1) =>
            {
                if (this.ControlsShown)
                    return;
                this.controls.Visibility = Visibility.Collapsed;
            };

            this.SizeChanged += this.VideoPlayer_SizeChanged;

            this.showMediaAni99 = Ani.Animation(Ani.DoubleAni((DependencyObject)this.mediaElement, "Opacity", 0.99, 0.2));
            this.showMediaAni = Ani.Animation(Ani.DoubleAni((DependencyObject)this.mediaElement, "Opacity", 1.0, 0.2));
            this.hideMediaAni = Ani.Animation(Ani.DoubleAni((DependencyObject)this.mediaElement, "Opacity", 0.0, 0.2));
            this.fadeMediaAni = Ani.Animation(Ani.DoubleAni((DependencyObject)this.mediaElement, "Opacity", this.PlaybackOpacity, 0.2));
            this.ControlsShown = false;
            try
            {
                int num = await App.Instance.WindowActivatedTask ? 1 : 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ex] VideoPlayer_Loaded ex 1: " + ex.Message);
            }

            try
            {
                this.TrialEndedAt = Settings.TrialEndedAt;
                this.StartedWatchingAt = Settings.StartedWatchingAt;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ex] VideoPlayer_Loaded ex 2: " + ex.Message);
            }

            this.annotationsControl.ItemsSource = this.visibleAnnotations;
            this.subtitlesControl.ItemsSource = this.visibleSubtitles;
            this.RegisterDashPlugin();
            double num1 = Settings.Volume;
            if (App.DeviceFamily == DeviceFamily.Mobile)
                num1 = 1.0;
            this.mediaElement.Volume = num1;
        }

        private void RegisterDashPlugin()
        {
        }

        private void mediaElement_SeekCompleted(object sender, RoutedEventArgs e)
        {
            if (VideoPlayer.audioElement.HasMedia())
                VideoPlayer.audioElement.Volume = this.mediaElement.Volume;
            this.SetAnnotations();
        }

        public void RegisterBackgroundEvent()
        {
            if (this.backgroundEvents)
                return;
            this.backgroundEvents = true;
            Helper.Write((object)nameof(VideoPlayer), (object)"Registering background player message event");
            
            this.Controls.RegisterBackgroundEvents();

            BackgroundMediaPlayer.MessageReceivedFromBackground += this.BackgroundMediaPlayer_MessageReceivedFromBackground;

            MediaPlayer current = BackgroundMediaPlayer.Current;
                        
            current.CurrentStateChanged += (s_3, e_3) =>
            {
                this.Current_CurrentStateChanged(s_3, e_3);
            };

            Helper.Write((object)nameof(VideoPlayer), (object)"Background player message event registered");
        }

        public void DeregisterBackgroundEvent()
        {
            if (!this.backgroundEvents)
                return;
            this.backgroundEvents = false;
            Helper.Write((object)nameof(VideoPlayer), (object)"Deregistering background player message event");
            this.Controls.DeregisterBackgroundEvents();

            BackgroundMediaPlayer.MessageReceivedFromBackground -= this.BackgroundMediaPlayer_MessageReceivedFromBackground;

            BackgroundMediaPlayer.Current.CurrentStateChanged -= this.Current_CurrentStateChanged;

            Helper.Write((object)nameof(VideoPlayer), (object)"Background player message event deregistered");
        }

        private void controls_SeekingChanged(object sender, bool e)
        {
            if (e)
            {
                this.controlsWatch.Reset();
            }
            else
            {
                if (!this.ControlsShown)
                    return;
                this.controlsWatch.Restart();
            }
        }

        private void controls_MenuShownChanged(object sender, bool e)
        {
            if (e)
                this.controlsWatch.Reset();
            else
                this.controlsWatch.Restart();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (!this.ControlsShown || !this.controlsWatch.IsRunning || !(this.controlsWatch.Elapsed > VideoPlayer.KeepControlsOpenFor))
                return;
            this.ControlsShown = false;
        }

        private void ControlsPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!this.ControlsShown || !this.controlsWatch.IsRunning)
                return;
            this.controlsWatch.Restart();
        }

        private async void mediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.currentEntry = (YouTubeEntry)null;
            this.MediaRunning = false;
            if (this.tries > 15)
            {
                this.focusOnVideo = true;
                this.progress.IsIndeterminate = false;
                if (this.tcs == null)
                    return;
                this.tcs.TrySetResult(false);
            }
            else
            {
               await Task.Run(() => this.mediaElement_MediaFailed(null, null));
            }
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (this.warnedAboutData && Settings.WarnOnData && !GlobalProperties.IsMobileDataConnection)
                this.warnedAboutData = false;

            this.MediaRunning = false;
            this.SetBookmark(true);

            if (this.playlistHelper != null)
            {
                switch (Settings.PlaylistRepeatMode)
                {
                 case PlaylistRepeatMode.None:
                    this.mediaElement.Stop();
                    break;
                  case PlaylistRepeatMode.One:
                    this.mediaElement.Position = TimeSpan.Zero;
                    this.mediaElement.Play();
                    break;
                  case PlaylistRepeatMode.All:
                    this.setShuffle();
                    YouTubeEntry entry = this.playlistHelper.GetEntry(1);
                    if (entry != null)
                    {
                        this.callMediaEnded(new MediaEndedEventArgs()
                        {
                            ContinuingWith = entry
                        });
                        this.progress.IsIndeterminate = true;
                        this.OpenVideo(entry, this.quality);
                        break;
                    }
                    this.mediaElement.Stop();
                    break;
                }
            }
            else if (Settings.NormalRepeatMode == PlaylistRepeatMode.One)
            {
                this.mediaElement.Position = TimeSpan.Zero;
                this.mediaElement.Play();
            }
            else
                this.mediaElement.Stop();
        }

        private void callMediaEnded(MediaEndedEventArgs e)
        {
            //RnD
            if (this.MediaEnded == null)
                return;
            if (e.CurrentEntry == null)
                e.CurrentEntry = this.CurrentEntry;
           
           // this.MediaEnded.Invoke(this, e);
        }

        private async void VideoPlayer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize != e.PreviousSize)
                this.setMediaElementSize(e.NewSize.Width + 1.0, e.NewSize.Height + 1.0);
            int num = this.Hidden ? 1 : 0;
        }

        private void setControlsRenderTransformOrigin()
        {
            if (double.IsNaN(((FrameworkElement)this.controls.MainGrid).ActualHeight) 
                || double.IsNaN(((FrameworkElement)this.controls).ActualHeight) 
                || ((FrameworkElement)this.Controls).ActualHeight <= 0.0 
                || ((FrameworkElement)this.Controls.MainGrid).ActualHeight <= 0.0)
                return;
            this.controls.RenderTransformOrigin = 
                new Point(0.5, (((FrameworkElement)this.Controls).ActualHeight 
                - ((FrameworkElement)this.Controls.MainGrid).ActualHeight / 2.0) 
                / ((FrameworkElement)this.Controls).ActualHeight);
        }

        public async Task<bool> Rotate(SimpleOrientation or)
        {
            bool returnVal = false;
            returnVal = or != null;
            if (or == this.lastOr)
                return returnVal;
            this.lastOr = or;
            if (this.Hidden)
            {
                if (returnVal)
                    this.showMediaAni.Begin();
                else if (this.MediaRunning)
                    this.fadeMediaAni.Begin();
                else
                    this.hideMediaAni.Begin();
            }
            FrameworkElement frameworkElement = this.viewBox.Child == this.mediaElement 
                ? (FrameworkElement)(object)this.viewBox 
                : (FrameworkElement)(object)this.mediaElement;
            frameworkElement.RenderTransform = (Transform)this.mediaTrans;
            this.annotationsViewBox.RenderTransform = (Transform)this.mediaTrans;
            this.annotationsViewBox.RenderTransformOrigin = new Point(0.5, 0.5);
            frameworkElement.RenderTransformOrigin = new Point(0.5, 0.5);

            double angle = 0.0;
            double To1 = 1.0;
            double To2 = 1.0;
            bool flag = true;
            Stretch stretch = Settings.Stretch;
            Rect bounds1 = Window.Current.Bounds;
            double height1 = bounds1.Height;
            bounds1 = Window.Current.Bounds;
            double width = bounds1.Width;
            double windowSize;
            double elSize;
            if ((double)this.mediaElement.NaturalVideoWidth 
                / (double)this.mediaElement.NaturalVideoHeight > height1 / width)
            {
                bounds1 = Window.Current.Bounds;
                windowSize = bounds1.Height;
                elSize = frameworkElement.ActualWidth;
            }
            else
            {
                bounds1 = Window.Current.Bounds;
                windowSize = bounds1.Width;
                elSize = frameworkElement.ActualHeight;
            }
            SimpleOrientation simpleOrientation1 = or;
            if (simpleOrientation1 != SimpleOrientation.Rotated90DegreesCounterclockwise)
            {
                if (simpleOrientation1 == SimpleOrientation.Rotated270DegreesCounterclockwise)
                {
                    angle = -90.0;
                    To2 = 0.0;
                    flag = false;
                    To1 = windowSize / elSize;
                }
            }
            else
            {
                angle = 90.0;
                To2 = 0.0;
                flag = false;
                To1 = windowSize / elSize;
            }
            if (this.ControlsShown && To2 > 0.5)
                this.titleGrid.Visibility = Visibility.Visible;

            Storyboard storyboard = Ani.Begin(Ani.DoubleAni(
                this.mediaTrans, "Rotation", angle, 0.375, 4.0), 
                Ani.DoubleAni(this.mediaTrans, "ScaleX", To1, 0.375, 4.0), 
                Ani.DoubleAni(this.mediaTrans, "ScaleY", To1, 0.375, 4.0),
                Ani.DoubleAni(this.titleGrid, "Opacity", To2, 0.3));

            storyboard.Completed += (object sender, object e) =>
            {
                if (this.titleGrid.Opacity != 0.0)
                    return;
                this.titleGrid.Visibility = Visibility.Collapsed;
            };

            this.titleGrid.IsHitTestVisible = flag;
            if (this.Controls.RenderTransform is CompositeTransform controlTrans)
            {
                bool layout = false;
                if (this.controls.Opacity == 0.0)
                {
                    this.controls.Visibility = Visibility.Visible;
                    this.cancel = new CancellationTokenSource();
                    layout = true;
                    this.controls.Opacity = 0.1;
                    Task task = this.controls.WaitForLayoutUpdateAsync();
                    this.controls.InvalidateMeasure();
                    this.controls.InvalidateArrange();
                    this.controls.UpdateLayout();
                    await task;
                }
                else if (this.cancel != null)
                    this.cancel = (CancellationTokenSource)null;
                if (!layout || this.cancel != null)
                {
                    double To3 = 0.0;
                    double To4 = 0.0;
                    Rect bounds2 = ((FrameworkElement)this.Controls).GetBounds(Window.Current.Content);
                    bounds2.X -= controlTrans.TranslateX;
                    bounds2.Y -= controlTrans.TranslateY;
                    SimpleOrientation simpleOrientation2 = or;
                    if (simpleOrientation2 != SimpleOrientation.Rotated90DegreesCounterclockwise)
                    {
                        if (simpleOrientation2 == SimpleOrientation.Rotated270DegreesCounterclockwise)
                        {
                            bounds1 = Window.Current.Bounds;
                            double height2 = bounds1.Height;
                            double y1 = bounds2.Y;
                            double height3 = bounds2.Height;
                            Point renderTransformOrigin = this.controls.RenderTransformOrigin;
                            double y2 = renderTransformOrigin.Y;
                            double num1 = height3 * y2;
                            double num2 = y1 + num1;
                            To4 = height2 - num2 - this.controls.ActualWidth / 2.0 - 38.0;
                            bounds1 = Window.Current.Bounds;
                            double num3 = bounds1.Width / 2.0;
                            double actualHeight = this.controls.ActualHeight;
                            renderTransformOrigin = this.controls.RenderTransformOrigin;
                            double num4 = 1.0 - renderTransformOrigin.Y;
                            double num5 = actualHeight * num4;
                            To3 = num3 - num5 - 38.0;
                        }
                    }
                    else
                    {
                        To4 = - (bounds2.Y + bounds2.Height * this.controls.RenderTransformOrigin.Y)
                            + this.controls.ActualWidth / 2.0 + 38.0;
                        bounds1 = Window.Current.Bounds;
                        To3 = - (bounds1.Width / 2.0) + this.controls.ActualHeight
                            * (1.0 - this.controls.RenderTransformOrigin.Y) + 38.0;
                    }
                    if (layout)
                        this.controls.Opacity = 0.0;
                    if (this.ControlsShown)
                    {
                        Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)controlTrans,
                            "Rotation", 
                            angle, 0.9, 4.0), (Timeline)Ani.DoubleAni((DependencyObject)controlTrans,
                            "TranslateX", To3, 0.9, 4.0), (Timeline)Ani.DoubleAni((DependencyObject)controlTrans, 
                            "TranslateY", To4, 0.9, 4.0));
                    }
                    else
                    {
                        controlTrans.Rotation = angle;
                        controlTrans.TranslateX = To3;
                        controlTrans.TranslateY = To4;
                    }
                }
            }
            return returnVal;
        }

        public async Task ShowMusicThumb()
        {
            if (this.thumbShown && this.lastEntry != null 
                && this.lastEntry.ID == this.lastEntryId || this.lastEntry == null)
                return;
            this.lastEntryId = this.lastEntry.ID;
            this.thumbShown = true;
            this.musicThumb.Opacity = this.Hidden ? 0.0 : this.PlaybackOpacity;
            this.musicThumb.Visibility = Visibility.Visible;
            int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
            Uri uri = await App.GlobalObjects.TransferManager.GetThumbUri(this.lastEntry.ID);
            if (uri == (Uri)null)
                uri = this.lastEntry.GetThumb(ThumbnailQuality.SD);
            this.musicBitmap.UriSource = uri;
        }

        public void HideMusicThumb()
        {
            if (!this.thumbShown)
                return;
            this.thumbShown = false;
            this.lastEntryId = "";
            Storyboard storyboard = Ani.Begin((DependencyObject)this.musicThumb, "Opacity", 0.0, 0.2);

            storyboard.Completed += async (s, e_0) =>
            {
                if (this.thumbShown)
                    return;
                this.musicThumb.Visibility = Visibility.Visible;
            };

        }

        private void SetTitle()
        {
            if (this.currentEntry == null)
                return;
            this.titleText.Text = this.currentEntry.Title;
            this.authorText.Text = this.currentEntry.AuthorDisplayName;
        }

        private async void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            Helper.Write((object)nameof(VideoPlayer), (object)"MediaElement.MediaOpened");

            int num1 = await App.GlobalObjects.InitializedTask ? 1 : 0;
            this.annotationsTimer.Stop();
            ((Collection<AnnotationInfo>)this.visibleAnnotations).Clear();
            ((Collection<Subtitle>)this.visibleSubtitles).Clear();
            this.openedOnce = true;
            this.currentEntry = this.lastEntry;
            bool sameInfo = true;
            if (this.successfulInfo != this.attemptedInfo)
            {
                sameInfo = false;
                this.successfulInfo = this.attemptedInfo;
            }
            this.controls.SetPlaybackSpeed(1.0);
            VideoPlayer.audioElement.IsLooping = false;
            if (this.unloadedPos.HasValue)
            {
                this.mediaElement.Position = this.unloadedPos.Value;
                this.unloadedPos = new TimeSpan?();
            }
            if (VideoPlayer.audioElement.CurrentState == MediaElementState.Opening)
            {
                int num2 = await VideoPlayer.audioElement.WaitForOpened() ? 1 : 0;
            }
            if (this.seekToOnOpen.TotalSeconds > 0.0)
            {
                this.mediaElement.Position = this.seekToOnOpen;
                if (VideoPlayer.audioElement.HasMedia())
                    VideoPlayer.audioElement.Position = this.seekToOnOpen;
                this.seekToOnOpen = TimeSpan.MinValue;
            }
            this.mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
            try
            {
                this.mediaElement.Play();
                Helper.Write((object)nameof(VideoPlayer), (object)"Called play on MediaElement");
            }
            catch
            {
            }
            if (this.Hidden)
            {
                if (this.focusOnVideo && DefaultPage.Current != null 
                    && DefaultPage.Current.OverCanvas != null 
                    && Settings.MiniPlayerType == MiniPlayerType.Background)
                {
                    Helper.Write((object)nameof(VideoPlayer), (object)"Closing OverCanvas");
                    OverCanvas overCanvas = DefaultPage.Current.OverCanvas;
                    overCanvas.ScrollToPage(overCanvas.Snaps.Count - 2, false);
                }
            }
            else
            {
                int num3 = this.openBookmark ? 1 : 0;
            }
            this.progress.IsIndeterminate = false;
            double num4 = ((FrameworkElement)this.mediaElement).ActualWidth 
                / ((FrameworkElement)this.mediaElement).ActualHeight;
            double num5 = (double)this.mediaElement.NaturalVideoWidth 
                / (double)this.mediaElement.NaturalVideoHeight;
            double num6 = num5;
            if (num4 != num6)
            {
                Helper.Write((object)nameof(VideoPlayer), (object)"Setting media element aspect ratio");
                this.mediaElement.Height = ((FrameworkElement)this.mediaElement).Width / num5;
                this.annotationsControl.Height = ((FrameworkElement)this.annotationsControl).Width 
                    * ((double)this.mediaElement.NaturalVideoHeight / (double)this.mediaElement.NaturalVideoWidth);
            }

            this.noVideoText.Opacity = 0.0;
            this.SetTitle();
            this.tries = 0;
            
            try
            {
                this.controls.SetSelectedQuality(this.quality);
                this.controls.SetVisibleButtons(this.successfulInfo);
                Helper.Write((object)nameof(VideoPlayer), (object)"Set player controls buttons");
            }
            catch (Exception ex)
            {
                //
            }
            try
            {
                if (this.tcs != null)
                {
                    Helper.Write((object)nameof(VideoPlayer), (object)"Set task result");
                    this.tcs.TrySetResult(true);
                }
            }
            catch (Exception ex)
            {
                //
            }

            this.MediaRunning = true;

            // TODO
            //if (this.MediaOpened != null)
            //{
                // TODO
                //this.MediaOpened.Invoke(this, new MediaOpenedEventArgs()
                //{
                //    CurrentEntry = this.currentEntry
                //});
            //}
            
            this.focusOnVideo = true;
            this.createPlaylistHelper();
            this.controls.SetShuffleIconVisibility(this.HasPlaylist);
            
            try
            {
                App.GlobalObjects.History.AddEntry(this.lastEntry);
                App.GlobalObjects.History.Save();
                Helper.Write((object)"Saved history");
            }
            catch (Exception ex)
            {
            }
            
            if (this.successfulInfo != null)
            {
                if (this.successfulInfo.AnnotationsLink != null)
                {
                    try
                    {
                        if (!sameInfo && this.quality != YouTubeQuality.Audio)
                        {
                            this.annotations = (List<AnnotationInfo>)null;
                            Helper.Write((object)nameof(VideoPlayer), (object)"Loading annotations info");
                            this.annotations = AnnotationInfo.GetAnnotations(XElement.Parse(
                                await new HttpClient().GetStringAsync(this.successfulInfo.AnnotationsLink
                                .ToUri(UriKind.Absolute))));

                            this.annotationsTimer.Start();
                            Helper.Write((object)"Loaded annotations and started timer");
                        }
                        else
                        {
                            Helper.Write((object)nameof(VideoPlayer), 
                                (object)"Same info, not loading annotations");
                            if (this.annotations != null)
                                this.annotationsTimer.Start();
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    Helper.Write((object)nameof(VideoPlayer), 
                        (object)"No annotations on this video, disabling annotations");
                    this.annotationsTimer.Stop();
                    ((Collection<AnnotationInfo>)this.visibleAnnotations).Clear();
                    this.annotations = (List<AnnotationInfo>)null;
                }
            }
            else
            {
                Helper.Write((object)nameof(VideoPlayer), 
                    (object)"No annotations on this video, disabling annotations");
                this.annotationsTimer.Stop();
                ((Collection<AnnotationInfo>)this.visibleAnnotations).Clear();
                this.annotations = (List<AnnotationInfo>)null;
            }
            if (this.currentEntry == null)
                return;
            this.subtitles = (Subtitle[])null;
            CaptionsDeclaration[] capsList = (CaptionsDeclaration[])null;
            try
            {
                SubtitleDeclaration[] subsList = await Subtitle.GetLanguageList(this.currentEntry.ID);
                if (subsList.Length != 0)
                {
                    SubtitleDeclaration dec = (SubtitleDeclaration)null;
                    foreach (SubtitleDeclaration subtitleDeclaration in subsList)
                    {
                        if (subtitleDeclaration.Default)
                        {
                            dec = subtitleDeclaration;
                            break;
                        }
                    }
                    if (dec == null)
                        dec = subsList[0];
                    SubtitleFormat[] subtitleFormatArray1 = new SubtitleFormat[3]
                    {
            SubtitleFormat.SBV,
            SubtitleFormat.TTS,
            SubtitleFormat.VTT
                    };
                    Subtitle[] subs = (Subtitle[])null;
                    SubtitleFormat[] subtitleFormatArray = subtitleFormatArray1;
                    for (int index = 0; index < subtitleFormatArray.Length; ++index)
                    {
                        SubtitleFormat format = subtitleFormatArray[index];
                        try
                        {
                            subs = await dec.GetCaptions(format);
                            break;
                        }
                        catch
                        {
                        }
                    }
                    subtitleFormatArray = (SubtitleFormat[])null;
                    if (subs != null)
                    {
                        this.subtitles = subs;
                        this.annotationsTimer.Start();
                    }
                    dec = (SubtitleDeclaration)null;
                    subs = (Subtitle[])null;
                }
                capsList = (CaptionsDeclaration[])subsList;
                subsList = (SubtitleDeclaration[])null;
            }
            catch
            {
            }
            this.Controls.SetCaptions(capsList);
            GC.Collect();
            capsList = (CaptionsDeclaration[])null;
        }

        private void setMediaElementScale(VideoPlayer.ScaleType type, double scale)
        {
            double num;
            switch (type)
            {
                case VideoPlayer.ScaleType.Video:
                    num = (double)this.MediaElement.NaturalVideoWidth;
                    break;
                case VideoPlayer.ScaleType.Layout:
                    Rect bounds = Window.Current.Bounds;
                    double width = bounds.Width;
                    bounds = Window.Current.Bounds;
                    double height = bounds.Height;
                    num = Math.Max(width, height);
                    break;
                case VideoPlayer.ScaleType.Pixel:
                    num = 1.0;
                    break;
                case VideoPlayer.ScaleType.Screen:
                    double pixelsPerViewPixel = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    num = (Math.Max(Window.Current.Bounds.Width, Window.Current.Bounds.Height) * pixelsPerViewPixel).Round(RoundDirection.Up);
                    break;
                default:
                    num = (double)this.MediaElement.NaturalVideoWidth;
                    break;
            }
            this.mediaElement.Width = num * scale;
        }

        public async Task ChangeQuality(YouTubeQuality qual, bool saveQuality = true)
        {
            if (this.warnedAboutData && Settings.WarnOnData && !GlobalProperties.IsMobileDataConnection)
                this.warnedAboutData = false;
            Helper.Write((object)nameof(VideoPlayer), (object)string.Format("Changing quality to {0}", (object)qual));
            if (this.successfulInfo == null && this.lastEntry == null)
                return;
            PlayerControls.UpdateBackgroundAudioState();
            bool flag = PlayerControls.BackgroundAudio;

            if (this.MediaElement.CurrentState == (MediaElementState)3 
                || this.MediaElement.CurrentState == (MediaElementState)4 
                || this.MediaElement.CurrentState == (MediaElementState)2)
                flag = false;

            try
            {
                this.seekToOnOpen = !flag ? this.mediaElement.Position : BackgroundMediaPlayer.Current.Position;
            }
            catch
            {
                return;
            }
            try
            {
                this.tries = 0;
                if (qual == YouTubeQuality.Audio)
                {
                    this.MediaElement.Stop();
                    this.MediaElement.Source = (Uri)null;
                    await VideoPlayer.InitializeBackgroundAudio();
                    this.RegisterBackgroundEvent();
                    ValueSet valueSet = new ValueSet();
                    valueSet.Add("key", (object)YouTube.APIKey);
                    valueSet.Add("accessToken", (object)YouTube.AccessToken);
                    valueSet.Add("videoID", this.successfulInfo == null ? (this.lastEntry == null ? (object)(string)null : (object)this.lastEntry.ID) : (object)this.successfulInfo.ID);
                    valueSet.Add("info", this.successfulInfo == null ? (object)(string)null : (object)this.successfulInfo.OriginalString);
                    valueSet.Add("watch", (object)(bool)(this.successfulInfo == null ? true : (this.successfulInfo.WatchPage ? true : false)));
                    valueSet.Add("useNavigatePage", (object)(bool)(this.successfulInfo == null ? (Settings.UseNavigatePage ? true : false) : (this.successfulInfo.UseNavigatePage ? true : false)));
                    valueSet.Add("entry", this.lastEntry != null ? (object)this.lastEntry.OriginalString : (object)(string)null);
                    valueSet.Add("decipher", (object)(bool)(this.successfulInfo == null ? false : (this.successfulInfo.Decipher ? true : false)));
                    valueSet.Add("play", (object)true);
                    valueSet.Add("client", this.typeConstructor != null ? (object)this.typeConstructor.ToString() : (object)(string)null);
                    valueSet.Add("cipher", (object)YouTube.DecipherAlgorithm);
                    valueSet.Add("seekToOnOpen", (object)(int)this.seekToOnOpen.TotalSeconds);
                    PlaylistRepeatMode playlistRepeatMode = Settings.PlaylistRepeatMode;
                    valueSet.Add("playlistRepeatMode", (object)playlistRepeatMode.ToString());
                    playlistRepeatMode = Settings.PlaylistRepeatMode;
                    valueSet.Add("normalRepeatMode", (object)playlistRepeatMode.ToString());
                    valueSet.Add("seed", (object)this.seed);
                    BackgroundMediaPlayer.SendMessageToBackground(valueSet);
                    this.seekToOnOpen = TimeSpan.MinValue;
                    this.ShowMusicThumb();
                    this.controls.SetSelectedQuality(qual);
                }
                else if (this.successfulInfo != null)
                {
                    if (PlayerControls.BackgroundAudio)
                        BackgroundMediaPlayer.Current.SetUriSource((Uri)null);
                    this.DeregisterBackgroundEvent();
                    this.successfulInfo.Allow60FPS = Settings.Allow60FPS;
                    this.quality = this.successfulInfo.HighestQuality(qual);
                    this.focusOnVideo = false;
                    Helper.Write((object)nameof(VideoPlayer), (object)"Setting mediaElement source", 1);
                    this.Source = this.successfulInfo.GetLink(this.quality).ToUri(UriKind.Absolute);
                   
                    if (this.successfulInfo.QualityNeedsAudio(this.quality) && VideoPlayer.audioElement != null 
                        && this.successfulInfo.HasQuality(YouTubeQuality.Audio))
                    {
                        Helper.Write((object)nameof(VideoPlayer), (object)"Setting audioElement source", 1);
                        VideoPlayer.audioElement.Source = this.successfulInfo.GetLink(YouTubeQuality.Audio)
                            .ToUri(UriKind.Absolute);
                    }
                    else
                    {
                        Helper.Write((object)nameof(VideoPlayer),
                            (object)"Setting audioElement source to null", 1);
                        VideoPlayer.audioElement.Source = (Uri)null;
                    }
                    this.controls.SetSelectedQuality(this.quality);
                    if (((!Settings.KeepSelectedQuality ? 0 : ( qual != YouTubeQuality.Audio ? 1 : 0) ) 
                      & (saveQuality ? 1 : 0)) != 0)
                        Settings.Quality = qual;

                    if (this.quality == YouTubeQuality.Audio)
                        this.ShowMusicThumb();
                    else
                        this.HideMusicThumb();
                }
                else
                    this.OpenVideo(this.lastEntry, qual);
                this.progress.IsIndeterminate = true;
            }
            catch
            {
                this.focusOnVideo = true;
                this.progress.IsIndeterminate = false;
            }
        }

        private void setMediaElementSize(double X, double Y)
        {
        }

        public async Task<bool> OpenVideo(
          YouTubeEntry entry,
          YouTubeQuality quality,
          bool watch = true,
          bool decipher = false,
          Task<YouTubeInfo> preloadedInfo = null,
          bool openBookmark = false)
        {

            // TODO
            OpenVideoDisplayClass openVideoDisplayClass = new VideoPlayer.OpenVideoDisplayClass();
            
            openVideoDisplayClass.u003E4 = this;
            openVideoDisplayClass.entry = entry;
            openVideoDisplayClass.quality = quality;
            openVideoDisplayClass.watch = watch;
            openVideoDisplayClass.decipher = decipher;
            openVideoDisplayClass.preloadedInfo = preloadedInfo;
            this.openBookmark = openBookmark;
            
            this.tcs = new TaskCompletionSource<bool>();
            this.tries = 0;
            
            CoreDispatcher dispatcher = Window.Current.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //default quality
                this.OpenVideo(openVideoDisplayClass.entry, openVideoDisplayClass.quality);
            });
            

            Helper.Write((object)nameof(VideoPlayer), (object)"OpenVideo", 1);
            return true;//this.tcs.Task;
        }

        private async Task openVideo(
          YouTubeEntry entry,
          YouTubeQuality quality,
          bool watch,
          bool decipher,
          TransferManager.State state,
          Task<YouTubeInfo> preloadedInfo)
        {
            // TODO
            OpenVideoDisplayClass171_0 COpenVideoDisplay = new VideoPlayer.OpenVideoDisplayClass171_0();
            COpenVideoDisplay.u003E4 = this;
            COpenVideoDisplay.entry = entry;
            COpenVideoDisplay.quality = quality;
            COpenVideoDisplay.watch = watch;
            COpenVideoDisplay.decipher = decipher;
            COpenVideoDisplay.state = state;

            try
            {
                // ISSUE: object of a compiler-generated type is created
                
                var displayClass1711 = new VideoPlayer.DisplayClass171_1();
                
                displayClass1711.clone1 = COpenVideoDisplay;
                await VideoPlayer.InitializeBackgroundAudio();
                await App.CheckSignIn(45.0);
                this.loader.UseNavigatePage = Settings.UseNavigatePage;
            
                ++this.tries;
            
                this.lastState = displayClass1711.clone1.state;
                this.currentEntry = (YouTubeEntry)null;
             
                this.lastEntry = displayClass1711.clone1.entry;
               
                displayClass1711.bgAudio = displayClass1711.clone1.quality 
                    == YouTubeQuality.Audio && (App.PlatformType == PlatformType.WindowsPhone
                    || App.PlatformType == PlatformType.WindowsUWP);
             
                if (displayClass1711.clone1.state == TransferManager.State.Complete)
        {
                    try
                    {
                        await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            this.openVideo(
                                displayClass1711.clone1.entry, 
                                displayClass1711.clone1.quality,
                                displayClass1711.clone1.watch, 
                                displayClass1711.clone1.decipher,
                                displayClass1711.clone1.state, 
                                preloadedInfo);
                        });
                    }
                    catch
                    {
                        this.tcs.SetResult(false);
                    }
                }
        else
                {
                    // ISSUE
                    var displayClass1712 = new VideoPlayer.DisplayClass171_2();
                    displayClass1712.clone2 = displayClass1711;
                    YouTubeInfo info = displayClass1712.info;
                    VideoPlayer videoPlayer = this;
                    YouTubeInfo attemptedInfo = videoPlayer.attemptedInfo;
                    YouTubeInfo youTubeInfo = await (preloadedInfo == null
                        ? this.loader.LoadInfo(displayClass1712.clone2.clone1.entry.ID, 
                        displayClass1712.clone2.clone1.watch, 
                        displayClass1712.clone2.clone1.decipher) : preloadedInfo);
                    // ISSUE: reference to a compiler-generated field
                    displayClass1712.info = videoPlayer.attemptedInfo = youTubeInfo;
                    videoPlayer = (VideoPlayer)null;
                 
                    // ISSUE: reference to a compiler-generated field
                    if (displayClass1712.clone2.bgAudio)
          {
                        await VideoPlayer.InitializeBackgroundAudio();
                        this.DeregisterBackgroundEvent();
                        this.RegisterBackgroundEvent();

                        await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Low, 
                        () =>
                        {
                            this.openVideo(entry, quality, watch, decipher, state, preloadedInfo);
                        });

                        ValueSet valueSet = new ValueSet();
                        valueSet.Add("key", 
                            (object)YouTube.APIKey);
                        valueSet.Add("accessToken", 
                            (object)YouTube.AccessToken);
                        //TODO
                        //valueSet.Add("videoID",
                        //    (object)displayClass1712.videoplayer2.u003E8_locals1.entry.ID);
                        //valueSet.Add(nameof(entry), 
                        //    (object)displayClass1712.videoplayer2.u003E8_locals1.entry.OriginalString);
                        valueSet.Add("info",
                            (object)displayClass1712.info.OriginalString);
                        valueSet.Add("useNavigatePage", 
                            (object)displayClass1712.info.UseNavigatePage);
                        valueSet.Add(nameof(watch), 
                            (object)displayClass1712.info.WatchPage);
                        valueSet.Add(nameof(decipher), (object)displayClass1712.info.Decipher);
                        valueSet.Add("play", (object)true);
                        valueSet.Add("client", 
                            this.typeConstructor != null 
                            ? (object)this.typeConstructor.ToString() : (object)(string)null);
                        valueSet.Add("cipher", 
                            (object)YouTube.DecipherAlgorithm);
                        valueSet.Add("playlistRepeatMode", 
                            (object)Settings.PlaylistRepeatMode.ToString());
                        valueSet.Add("normalRepeatMode", 
                            (object)Settings.PlaylistRepeatMode.ToString());
                        valueSet.Add("seed", (object)this.seed);
                        BackgroundMediaPlayer.SendMessageToBackground(valueSet);
                        
                        
                        await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            //this.openVideo(this, null);
                            this.openVideo(entry, quality, watch, decipher, state, preloadedInfo);
                        });
                    }
          else
                    {
                        // ISSUE: reference to a compiler-generated field
                        displayClass1712.info.Allow60FPS = Settings.Allow60FPS;
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        // ISSUE: reference to a compiler-generated field
                        this.quality = displayClass1712.info
                            .HighestQuality(displayClass1712.clone2.clone1.quality);

                        await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                        {
                            this.openVideo(displayClass1712.clone2.clone1.entry, displayClass1712.info.HighestQuality(displayClass1712.clone2.clone1.quality),
                                displayClass1712.clone2.clone1.watch, displayClass1712.clone2.clone1.decipher,
                                displayClass1712.clone2.clone1.state, null);
                        });
                    }
                    displayClass1712 = (VideoPlayer.DisplayClass171_2) null;
                }
                displayClass1711 = (VideoPlayer.DisplayClass171_1) null;
            }
            catch
            {
                if (this.tries > 10)
                {
                    if (this.tcs == null)
                        return;
                    this.tcs.TrySetResult(false);
                }
                else
                {
                    // ISSUE: reference to a compiler-generated field                    
                    this.openVideo(COpenVideoDisplay.entry, COpenVideoDisplay.quality, 
                        !COpenVideoDisplay.watch, COpenVideoDisplay.decipher, COpenVideoDisplay.state, 
                        (Task<YouTubeInfo>)null);
                }
            }
        }

        private async void mediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (this.warnedAboutData && Settings.WarnOnData && !GlobalProperties.IsMobileDataConnection)
                this.warnedAboutData = false;

            if (this.mediaElement.CurrentState == this.lastMediaElementState)
                return;

            if (!this.warnedAboutData && !this.warnedAboutSwitchToData 
                && Settings.WarnOnData && GlobalProperties.IsMobileDataConnection 
                && this.successfulInfo != null)
            {
                new MessageDialog("You are now using mobile data.", "Mobile Data Warning").ShowAsync();
                this.warnedAboutSwitchToData = true;
            }
            this.lastMediaElementState = this.mediaElement.CurrentState;
            this.CheckWatchTimer();

            Helper.Write((object)nameof(VideoPlayer), 
                (object)("Current State Changed: " + (object)this.mediaElement.CurrentState + ", " +
                "Playback rate: " + (object)this.mediaElement.PlaybackRate));

            switch (this.mediaElement.CurrentState - 1)
            {
                case (MediaElementState)0:
                    this.annotationsTimer.Stop();
                    this.PauseWatchTimer();
                    VideoPlayer.speedTimer.Stop();
                    VideoPlayer.checkTimer.Stop();
                    this.mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
                    if (VideoPlayer.audioElement.CurrentState != (MediaElementState)3 
                        && VideoPlayer.audioElement.CurrentState != (MediaElementState)2)
                        break;
                    VideoPlayer.audioElement.Pause();
                    break;
                case (MediaElementState)1:
                    this.PauseWatchTimer();
                    this.MediaRunning = true;
                    if (VideoPlayer.audioElement.HasMedia())
                        VideoPlayer.audioElement.Pause();
                    this.annotationsTimer.Stop();
                    this.SetAnnotations();
                    break;
                case (MediaElementState)2:
                    this.MediaRunning = true;
                    DefaultPage.Current.UnlockRotation();
                    if (VideoPlayer.audioElement.HasMedia())
                    {
                        try
                        {
                            VideoPlayer.audioElement.Play();
                        }
                        catch
                        {
                        }
                        VideoPlayer.audioElement.Volume = this.mediaElement.Volume;
                        VideoPlayer.syncElements();
                        VideoPlayer.checkTimer.Start();
                    }
                    else
                    {
                        VideoPlayer.speedTimer.Stop();
                        VideoPlayer.checkTimer.Stop();
                        this.mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
                    }

                    if (!this.WaitingForTrialCooldown)
                        this.StartWatchTimer();
                    else
                        this.mediaElement.Stop();

                    if (this.annotations == null && this.subtitles == null 
                        || this.quality == YouTubeQuality.Audio)
                        break;

                    this.annotationsTimer.Start();
                    break;
                case (MediaElementState)3:
                    this.SetBookmark();
                    if (VideoPlayer.audioElement.HasMedia())
                    {
                        VideoPlayer.audioElement.Pause();
                        VideoPlayer.audioElement.Position = this.mediaElement.Position;
                    }
                    else
                        this.mediaElement.PlaybackRate = VideoPlayer.VideoSpeed;
                    this.annotationsTimer.Stop();
                    this.SetAnnotations();
                    break;
                case (MediaElementState)4:
                    this.PauseWatchTimer();
                    this.MediaRunning = false;
                    DefaultPage.Current.ExitFullscreenMode();
                    this.callMediaEnded(new MediaEndedEventArgs());
                    if (!this.Hidden)
                    {
                        OverCanvas overCanvas = DefaultPage.Current != null ? DefaultPage.Current.OverCanvas : (OverCanvas)null;
                        if (overCanvas != null)
                        {
                            if (overCanvas.SelectedPage == -1)
                                overCanvas.ScrollToIndex(0, false, true);
                            else if (overCanvas.FlipStyle == FlipStyle.Classic)
                            {
                                if (((ContentControl)DefaultPage.Current.Frame).Content is VideoPage)
                                {
                                    switch (Settings.AfterVideoSection)
                                    {
                                        case AfterVideoSection.Details:
                                            overCanvas.ScrollToIndex(0, false);
                                            break;
                                        case AfterVideoSection.Comments:
                                            overCanvas.ScrollToIndex(1, false);
                                            break;
                                        case AfterVideoSection.Suggested:
                                            overCanvas.ScrollToIndex(2, false);
                                            break;
                                    }
                                }
                                else
                                {
                                    overCanvas.ScrollToIndex(((ICollection<UIElement>)overCanvas.Children).Count 
                                        - 1, false, true);
                                }
                            }
                        }
                    }
                    if (DefaultPage.Current != null)
                        DefaultPage.Current.LockRotation((SimpleOrientation)0);
                    this.annotationsTimer.Stop();
                    ((Collection<AnnotationInfo>)this.visibleAnnotations).Clear();
                    ((Collection<Subtitle>)this.visibleSubtitles).Clear();
                    VideoPlayer.speedTimer.Stop();
                    VideoPlayer.checkTimer.Stop();
                    if (VideoPlayer.audioElement.HasMedia())
                        VideoPlayer.audioElement.Stop();
                    this.HideMusicThumb();
                    break;
                default:
                    this.annotationsTimer.Stop();
                    this.PauseWatchTimer();
                    break;
            }
        }

        public async Task SetBookmark(bool forceSet = false, bool save = false)
        {
            if ((this.MediaElement.CurrentState == (MediaElementState)1 
                || this.MediaElement.CurrentState == null 
                || this.MediaElement.CurrentState == (MediaElementState)5) && 
                !forceSet || this.lastEntry == null)
                return;
            App.GlobalObjects.TimeBookmarksManager.SetBookmark(this.lastEntry, this.MediaElement.Position);
            if (!save)
                return;
            await App.GlobalObjects.TimeBookmarksManager.Save();
        }

        public void SetTypeConstructor(TypeConstructor tC) => this.typeConstructor = tC;

        public void SetSourceVideo(Stream stream)
        {
            this.mediaElement.SetSource(WindowsRuntimeStreamExtensions.AsRandomAccessStream(stream), "video/mp4");
        }

        private void testElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void musicBitmap_ImageOpened(object sender, RoutedEventArgs e)
        {
            if (this.Hidden)
                Ani.Begin((DependencyObject)this.musicThumb, "Opacity", this.PlaybackOpacity, 0.3);
            else
                Ani.Begin((DependencyObject)this.musicThumb, "Opacity", 1.0, 0.3);
        }

        private void detailsButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.currentEntry == null)
                return;
            if (((ContentControl)App.Instance.RootFrame).Content != null 
                && App.Instance.RootFrame.CurrentSourcePageType == typeof(VideoPage) 
                && (((ContentControl)App.Instance.RootFrame).Content as FrameworkElement).DataContext is YouTubeEntry && ((((ContentControl)App.Instance.RootFrame).Content as FrameworkElement).DataContext as YouTubeEntry).ID == this.currentEntry.ID)
            {
                DefaultPage.Current.OverCanvas.ScrollToIndex(0, false);
            }
            else
            {
                if (DefaultPage.Current.OverCanvas != null && !DefaultPage.Current.OverCanvas.Shown)
                {
                    if (DefaultPage.Current.OverCanvas.SelectedPage == -1)
                        DefaultPage.Current.OverCanvas.ScrollToIndex(0, false);
                    else
                        DefaultPage.Current.OverCanvas.ScrollToIndex(
                            ((ICollection<UIElement>)DefaultPage.Current.OverCanvas.Children).Count - 1, false);
                }
                App.Instance.RootFrame.Navigate(typeof(VideoPage), (object)this.currentEntry);
            }
        }

        private async void castingControl_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void StartWatchTimer()
        {
            if (App.IsTrial)
            {
                if (!this.RunningTrial)
                {
                    this.CheckWatchTimer();
                    if (!this.WaitingForTrialCooldown)
                    {
                        this.WatchStopwatch.Reset();
                        this.RunningTrial = true;
                        this.StartedWatchingAt = Settings.StartedWatchingAt = DateTimeOffset.UtcNow;
                    }
                }
                if (this.WaitingForTrialCooldown)
                    return;
                this.WatchStopwatch.Start();
            }
            else
            {
                this.RunningTrial = false;
                this.WatchStopwatch.Reset();
            }
        }

        private void PauseWatchTimer() => this.WatchStopwatch.Stop();

        private async void CheckWatchTimer()
        {
            if (App.IsTrial)
            {
                if (DateTimeOffset.UtcNow - this.StartedWatchingAt < this.WatchTimeInterval 
                    && (this.WatchStopwatch.Elapsed > this.WatchTimeLimit 
                    || DateTimeOffset.UtcNow - this.TrialEndedAt < this.WatchTimeCooldown) 
                    && !this.WaitingForTrialCooldown)
                {
                    this.RunningTrial = false;
                    this.WaitingForTrialCooldown = true;
                    this.WatchStopwatch.Reset();
                    this.TrialEndedAt = Settings.TrialEndedAt = DateTimeOffset.UtcNow;
                }
                else if (this.WaitingForTrialCooldown)
                {
                    if (DateTimeOffset.UtcNow - this.TrialEndedAt < this.WatchTimeCooldown)
                    {
                        await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Low, 
                        () =>
                        {
                            CheckWatchTimer();
                        });
                    }
                    else
                    {
                        this.WatchStopwatch.Reset();
                        this.RunningTrial = false;
                        this.WaitingForTrialCooldown = false;
                    }
                }
                else
                {
                    if (!(DateTimeOffset.UtcNow - this.StartedWatchingAt > this.WatchTimeInterval))
                        return;
                    this.RunningTrial = false;
                    this.WaitingForTrialCooldown = false;
                    this.WatchStopwatch.Reset();
                }
            }
            else
            {
                this.RunningTrial = false;
                this.WaitingForTrialCooldown = false;
            }
        }

        public void RequestYouTubeEntryFromBackground()
        {
            ValueSet valueSet = new ValueSet();
            ((IDictionary<string, object>)valueSet).Add("requestEntry", (object)true);
            BackgroundMediaPlayer.SendMessageToBackground(valueSet);
        }

        private void layoutRoot_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!this.ControlsShown || !this.controls.IsTapping(e))
                return;
            this.controlsWatch.Restart();
        }

        private void titleTextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double width = e.NewSize.Width;
            double right = this.titleTextBlock.Padding.Right;
        }

        private async void titleGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void stopButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            this.MediaElement.Stop();
        }

      
        private enum ScaleType
        {
            Video,
            Layout,
            Pixel,
            Screen,
        }

        private class MediaOpened
        {
            internal VideoPlayer u003E4;
            internal MediaPlayerDataReceivedEventArgs e;
            internal bool success;

            public MediaOpened()
            {
            }
        }

        private class OpenVideoDisplayClass
        {
            internal Task<YouTubeInfo> preloadedInfo;
            internal VideoPlayer u003E4;
            internal  YouTubeEntry entry;
            internal YouTubeQuality quality;
            internal  bool watch;
            internal  bool decipher;

            public OpenVideoDisplayClass()
            { }

        }

        private class OpenVideoDisplayClass171_0
        {
            internal VideoPlayer u003E4;
            internal YouTubeEntry entry;
            internal YouTubeQuality quality;
            internal bool watch;
            internal bool decipher;
            internal TransferManager.State state;

            public OpenVideoDisplayClass171_0()
            {
            }
        }

        private class DisplayClass171_2
        {
            internal VideoPlayer.DisplayClass171_1 clone2;
            internal YouTubeInfo info;
            internal VideoPlayer videoplayer2;

            public DisplayClass171_2()
            {
            }
        }

        private class DisplayClass171_1
        {
            internal OpenVideoDisplayClass171_0 clone1;
            internal bool bgAudio;

            public DisplayClass171_1()
            {
            }
        }
    }
}

/*
namespace myTube
{
    public sealed partial class VideoPlayer 
    {
        public YouTubeQuality CurrentQuality;
        public MediaElement MediaElement;

        internal bool ControlsShown;
        internal bool Hidden;
        internal bool MediaRunning;
        internal PlayerControls Controls;
        internal MediaElement ControlsWatch;
        internal object AudioElement;

        public async Task ChangeQuality(YouTubeQuality audio, bool v)
        {
            throw new NotImplementedException();
        }

        public void DeregisterBackgroundEvent()
        {
            throw new NotImplementedException();
        }
    }
}
*/