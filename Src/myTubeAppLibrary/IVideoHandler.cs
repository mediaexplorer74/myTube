// Decompiled with JetBrains decompiler
// Type: myTube.IVideoHandler
// Assembly: myTubeAppLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 421B05F1-0283-4856-94C3-9442AF560132
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTubeAppLibrary.dll

using RykenTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public abstract class IVideoHandler
  {
    protected string Tag = nameof (IVideoHandler);
    private DispatcherTimer preloadTimer;
    private float playbackRate = 1f;
    private float[] playbackSpeeds = new float[7]
    {
      0.25f,
      0.5f,
      0.75f,
      1f,
      1.25f,
      1.5f,
      2f
    };
    private VideoProjection projection;
    private double volume = 1.0;
    private double framerate = 30.0;
    private VideoHandlerState state;
    private Dictionary<string, VideoInfoPreload> preloads = new Dictionary<string, VideoInfoPreload>();
    private YouTubeQuality quality;
    private YouTubeQuality requestedQuality;
    private SubtitleDeclaration[] listOfCaptions = new SubtitleDeclaration[0];
    private bool mediaRunning;
    private object cancelCastingLock = new object();
    private bool shuffle = true;
    private MediaUsageArgs lastMediaUsageArgs;
    private Random shuffleRand = new Random();
    private object logLock = new object();
    private StringBuilder statusBuilder;
    private bool registered;

    public bool PreloadPlaylistVideos { get; set; } = true;

    public bool HasPlaylist => this.PlaylistHelper != null || this.PlaylistButtons.NextButton;

    public virtual PlaylistRepeatMode PlaylistRepeatMode { get; set; } = PlaylistRepeatMode.All;

    public virtual PlaylistRepeatMode NormalRepeatMode { get; set; } = PlaylistRepeatMode.One;

    public bool IsContinuousPlayback
    {
      get
      {
        if (!this.HasPlaylist)
          return this.NormalRepeatMode == PlaylistRepeatMode.One;
        return this.PlaylistRepeatMode == PlaylistRepeatMode.One || this.PlaylistRepeatMode == PlaylistRepeatMode.None;
      }
    }

    public event TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs> StepForwardBackwardRequested
    {
      add
      {
                TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs> typedEventHandler1 = default;//this.StepForwardBackwardRequested;
        TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs>>(ref this.StepForwardBackwardRequested, (TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs> typedEventHandler1 = default;//this.StepForwardBackwardRequested;
        TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
         // typedEventHandler1 = 
         //               Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, 
         //               StepForwardBackwardRequestedEventArgs>>(
         //                   ref this.StepForwardBackwardRequested, (TypedEventHandler<IVideoHandler, StepForwardBackwardRequestedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    protected SystemMediaTransportControls TransportControls { get; private set; }

    public bool ExtendedExecutionRequested { get; set; }

    public abstract TimeSpan Duration { get; protected set; }

    public ObservableCollection<VideoCastingDevice> CastingDevices { get; private set; } = new ObservableCollection<VideoCastingDevice>();

    public event TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs> PlaybackRateChanged
    {
      add
      {
        TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs> typedEventHandler1 = this.PlaybackRateChanged;
        TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs>>(ref this.PlaybackRateChanged, (TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs> typedEventHandler1 = this.PlaybackRateChanged;
        TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs>>(ref this.PlaybackRateChanged, (TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public bool SupportsVariablePlaybackRate { get; protected set; }

    public float PlaybackRate
    {
      get => this.playbackRate;
      set
      {
        if ((double) value == (double) this.PlaybackRate)
          return;
        float playbackRate = this.PlaybackRate;
        this.playbackRate = value;
        this.SetPlaybackRate(value);
        if (!this.SupportsVariablePlaybackRate)
          return;
                TypedEventHandler<IVideoHandler, PlaybackSpeedChangedEventArgs> playbackRateChanged = default;//this.PlaybackRateChanged;
        if (playbackRateChanged == null)
          return;
        playbackRateChanged.Invoke(this, new PlaybackSpeedChangedEventArgs()
        {
          OldSpeed = playbackRate,
          NewSpeed = value
        });
      }
    }

    public bool SupportsCaptions { get; protected set; }

    protected virtual void SetPlaybackRate(float rate)
    {
    }

    public float[] SupportedPlaybackRates
    {
      get => this.playbackSpeeds;
      set => this.playbackSpeeds = value;
    }

    public void NextPlaybackRate() => this.PlaybackRate = this.playbackSpeeds[(this.playbackSpeeds.IndexOfClosest(this.PlaybackRate) + 1) % this.playbackSpeeds.Length];

    public abstract bool SupportsBackgroundAudio { get; protected set; }

    public virtual bool SupportsForegroundVideo { get; protected set; } = true;

    public TimeSpan Position
    {
      get => this.PositionOverride;
      set
      {
        if (!(value != this.PositionOverride))
          return;
        this.PositionOverride = value;
                TypedEventHandler<IVideoHandler, PositionChangedEventArgs> positionChanged = default;//this.PositionChanged;
        if (positionChanged == null)
          return;
        positionChanged.Invoke(this, new PositionChangedEventArgs()
        {
          NewPosition = value
        });
      }
    }

    public virtual async Task<TimeSpan> GetPosition() => this.Position;

    public void SetPositionWithoutCallingEvent(TimeSpan value)
    {
      if (!(value != this.PositionOverride))
        return;
      this.PositionOverride = value;
    }

    protected abstract TimeSpan PositionOverride { get; set; }

    public event TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs> ProjectionChanged
    {
      add
      {
                TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs> typedEventHandler1 = default;//this.ProjectionChanged;
        TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs>>(ref this.ProjectionChanged, (TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs> typedEventHandler1 = default;//this.ProjectionChanged;
        TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs>>(ref this.ProjectionChanged, (TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public VideoProjection Projection
    {
      get => this.projection;
      protected set
      {
        if (this.projection == value)
          return;
        this.projection = value;
                TypedEventHandler<IVideoHandler, ProjectionChangedEventArgs> projectionChanged = default;//this.ProjectionChanged;
        if (projectionChanged == null)
          return;
        projectionChanged.Invoke(this, new ProjectionChangedEventArgs()
        {
          Projection = this.projection
        });
      }
    }

    public bool SupportsVolume { get; set; }

    public double Volume
    {
      get => this.volume;
      set
      {
        if (value > 1.0)
          value = 1.0;
        if (value < 0.0)
          value = 0.0;
        if (this.volume == value)
          return;
        this.volume = value;
        if (!this.SupportsVolume)
          return;
        this.VolumeChanged(value);
      }
    }

    protected abstract void VolumeChanged(double volume);

    public CoreDispatcher Dispatcher { get; private set; }

    public bool IsOpeningVideo { get; private set; }

    public string CastingDeviceTypeName { get; protected set; }

    public event TypedEventHandler<IVideoHandler, FramerateChangedEventArgs> FramerateChanged
    {
      add
      {
                TypedEventHandler<IVideoHandler, FramerateChangedEventArgs> typedEventHandler1 = default;//this.FramerateChanged;
        TypedEventHandler<IVideoHandler, FramerateChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, FramerateChangedEventArgs>>(ref this.FramerateChanged, (TypedEventHandler<IVideoHandler, FramerateChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, FramerateChangedEventArgs> typedEventHandler1 = default;//this.FramerateChanged;
        TypedEventHandler<IVideoHandler, FramerateChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, FramerateChangedEventArgs>>(ref this.FramerateChanged, (TypedEventHandler<IVideoHandler, FramerateChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public double Framerate
    {
      get => this.framerate;
      set
      {
        if (this.framerate == value)
          return;
        double framerate = this.framerate;
        this.framerate = value;
                TypedEventHandler<IVideoHandler, FramerateChangedEventArgs> framerateChanged = default;//this.FramerateChanged;
        if (framerateChanged == null)
          return;
        framerateChanged.Invoke(this, new FramerateChangedEventArgs()
        {
          OldFramerate = framerate,
          NewFramerate = value
        });
      }
    }

    public abstract bool IsSupported { get; }

    public bool SupportsLiveVideos { get; protected set; }

    public bool SupportsSavedVideos { get; protected set; }

    public PlaylistHelperBase PlaylistHelper { get; private set; }

    public bool RequiresUIThreadPlayback { get; protected set; } = true;

    public bool CastingIsRequired { get; protected set; }

    public bool SupportsCasting { get; protected set; }

    public bool HandlesTransportControls { get; protected set; }

    public bool HandlesPlaylists { get; protected set; }

    public bool SupportsAddToCurrentPlaylist { get; protected set; } = true;

    public VideoCastingDevice VideoCastingDevice { get; private set; }

    public event TypedEventHandler<IVideoHandler, MediaEndedArgs> MediaEnded
    {
      add
      {
                TypedEventHandler<IVideoHandler, MediaEndedArgs> typedEventHandler1 = default;//this.MediaEnded;
        TypedEventHandler<IVideoHandler, MediaEndedArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaEndedArgs>>(ref this.MediaEnded, (TypedEventHandler<IVideoHandler, MediaEndedArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, MediaEndedArgs> typedEventHandler1 = default;//this.MediaEnded;
        TypedEventHandler<IVideoHandler, MediaEndedArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaEndedArgs>>(ref this.MediaEnded, (TypedEventHandler<IVideoHandler, MediaEndedArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, MediaRunningChangedArgs> MediaRunningChanged
    {
      add
      {
                TypedEventHandler<IVideoHandler, MediaRunningChangedArgs> typedEventHandler1 = default;//this.MediaRunningChanged;
        TypedEventHandler<IVideoHandler, MediaRunningChangedArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaRunningChangedArgs>>(ref this.MediaRunningChanged, (TypedEventHandler<IVideoHandler, MediaRunningChangedArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, MediaRunningChangedArgs> typedEventHandler1 = default;//this.MediaRunningChanged;
        TypedEventHandler<IVideoHandler, MediaRunningChangedArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaRunningChangedArgs>>(ref this.MediaRunningChanged, (TypedEventHandler<IVideoHandler, MediaRunningChangedArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, VideoHandlerState> StateChanged
    {
      add
      {
                TypedEventHandler<IVideoHandler, VideoHandlerState> typedEventHandler1 = default;//this.StateChanged;
        TypedEventHandler<IVideoHandler, VideoHandlerState> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, VideoHandlerState>>(ref this.StateChanged, (TypedEventHandler<IVideoHandler, VideoHandlerState>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, VideoHandlerState> typedEventHandler1 = default;//this.StateChanged;
        TypedEventHandler<IVideoHandler, VideoHandlerState> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, VideoHandlerState>>(ref this.StateChanged, (TypedEventHandler<IVideoHandler, VideoHandlerState>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> MediaOpened
    {
      add
      {
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler1 = this.MediaOpened;
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>>(ref this.MediaOpened, (TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler1 = this.MediaOpened;
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>>(ref this.MediaOpened, (TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> MediaOpening
    {
      add
      {
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler1 = this.MediaOpening;
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>>(ref this.MediaOpening, (TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler1 = this.MediaOpening;
        TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>>(ref this.MediaOpening, (TypedEventHandler<IVideoHandler, MediaOpenedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, MediaFailedEventArgs> MediaFailed
    {
      add
      {
        TypedEventHandler<IVideoHandler, MediaFailedEventArgs> typedEventHandler1 = this.MediaFailed;
        TypedEventHandler<IVideoHandler, MediaFailedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaFailedEventArgs>>(ref this.MediaFailed, (TypedEventHandler<IVideoHandler, MediaFailedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, MediaFailedEventArgs> typedEventHandler1 = default;//this.MediaFailed;
        TypedEventHandler<IVideoHandler, MediaFailedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaFailedEventArgs>>(ref this.MediaFailed, (TypedEventHandler<IVideoHandler, MediaFailedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs> AvailableQualitiesChanged
    {
      add
      {
        TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs> typedEventHandler1 = default;//this.AvailableQualitiesChanged;
        TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs>>(ref this.AvailableQualitiesChanged, (TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs> typedEventHandler1 = default;//this.AvailableQualitiesChanged;
        TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs>>(ref this.AvailableQualitiesChanged, (TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, MediaThumbnailChanged> ThumbnailChanged
    {
      add
      {
                TypedEventHandler<IVideoHandler, MediaThumbnailChanged> typedEventHandler1 = default;//this.ThumbnailChanged;
        TypedEventHandler<IVideoHandler, MediaThumbnailChanged> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaThumbnailChanged>>(ref this.ThumbnailChanged, (TypedEventHandler<IVideoHandler, MediaThumbnailChanged>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
                TypedEventHandler<IVideoHandler, MediaThumbnailChanged> typedEventHandler1 =
                            default;// this.ThumbnailChanged;
        TypedEventHandler<IVideoHandler, MediaThumbnailChanged> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 =
          //              Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, MediaThumbnailChanged>>(ref this.ThumbnailChanged, (TypedEventHandler<IVideoHandler, MediaThumbnailChanged>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs> FoundCastingDevice
    {
      add
      {
        TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs> typedEventHandler1 = this.FoundCastingDevice;
        TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs>>(ref this.FoundCastingDevice, (TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs> typedEventHandler1 = this.FoundCastingDevice;
        TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs>>(ref this.FoundCastingDevice, (TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, PositionChangedEventArgs> PositionChanged
    {
      add
      {
        TypedEventHandler<IVideoHandler, PositionChangedEventArgs> typedEventHandler1 = this.PositionChanged;
        TypedEventHandler<IVideoHandler, PositionChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, PositionChangedEventArgs>>(ref this.PositionChanged, (TypedEventHandler<IVideoHandler, PositionChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, PositionChangedEventArgs> typedEventHandler1 = this.PositionChanged;
        TypedEventHandler<IVideoHandler, PositionChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, PositionChangedEventArgs>>(ref this.PositionChanged, (TypedEventHandler<IVideoHandler, PositionChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs> PlaylistButtonsSet
    {
      add
      {
        TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs> typedEventHandler1 = this.PlaylistButtonsSet;
        TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs>>(ref this.PlaylistButtonsSet, (TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs> typedEventHandler1 = this.PlaylistButtonsSet;
        TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs>>(ref this.PlaylistButtonsSet, (TypedEventHandler<IVideoHandler, PlaylistButtonsEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, NextVideoEventArgs> NextVideo
    {
      add
      {
        TypedEventHandler<IVideoHandler, NextVideoEventArgs> typedEventHandler1 = this.NextVideo;
        TypedEventHandler<IVideoHandler, NextVideoEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, NextVideoEventArgs>>(ref this.NextVideo, (TypedEventHandler<IVideoHandler, NextVideoEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, NextVideoEventArgs> typedEventHandler1 = this.NextVideo;
        TypedEventHandler<IVideoHandler, NextVideoEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, NextVideoEventArgs>>(ref this.NextVideo, (TypedEventHandler<IVideoHandler, NextVideoEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs> BookmarkableAction
    {
      add
      {
                TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs> typedEventHandler1 = default;//this.BookmarkableAction;
        TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          //typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs>>(ref this.BookmarkableAction, (TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs> typedEventHandler1 = this.BookmarkableAction;
        TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs>>(ref this.BookmarkableAction, (TypedEventHandler<IVideoHandler, BookmarkableActionEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public PlaylistButtonsEventArgs PlaylistButtons { get; } = new PlaylistButtonsEventArgs()
    {
      NextButton = false,
      PrevButton = false
    };

    public VideoHandlerState State
    {
      get => this.state;
      set
      {
        if (this.state == value)
          return;
        this.state = value;

                //RnD
        //this.StateChanged?.Invoke(this, this.state);
        switch (this.state)
        {
          case VideoHandlerState.Unknown:
            this.MediaRunning = false;
            this.setTransportControlsState((MediaPlaybackStatus) 2);
            break;
          case VideoHandlerState.Playing:
            this.MediaRunning = true;
            this.setTransportControlsState((MediaPlaybackStatus) 3);
            break;
          case VideoHandlerState.Paused:
            this.setTransportControlsState((MediaPlaybackStatus) 4);
            break;
          case VideoHandlerState.Stopped:
            this.MediaRunning = false;
            this.setTransportControlsState((MediaPlaybackStatus) 2);
            break;
          case VideoHandlerState.Buffering:
            this.MediaRunning = true;
            this.setTransportControlsState((MediaPlaybackStatus) 1);
            break;
        }
      }
    }

    public virtual bool SupportsEncoding(MediaEncoding encoding) => true;

    public void CallBookmarkableAction()
    {
      if (this.CurrentEntry == null)
        return;

      //RnD
      // ISSUE: reference to a compiler-generated field
      //this.BookmarkableAction?.Invoke(this, new BookmarkableActionEventArgs(this.CurrentEntry, this.Position));
    }

    private void setTransportControlsState(MediaPlaybackStatus status)
    {
      if (this.HandlesTransportControls || this.TransportControls == null)
        return;
      this.TransportControls.PlaybackStatus = status;
    }

    public bool SupportsFrameByFrameControl { get; protected set; }

    public YouTubeEntry AttemptedEntry { get; private set; }

    public YouTubeEntry CurrentEntry { get; private set; }

    public event TypedEventHandler<IVideoHandler, QualityChangedEventArgs> QualityChanged
    {
      add
      {
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler1 = this.QualityChanged;
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, QualityChangedEventArgs>>(ref this.QualityChanged, (TypedEventHandler<IVideoHandler, QualityChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler1 = this.QualityChanged;
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, QualityChangedEventArgs>>(ref this.QualityChanged, (TypedEventHandler<IVideoHandler, QualityChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, QualityChangedEventArgs> QualityChangeFailed
    {
      add
      {
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler1 = this.QualityChangeFailed;
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, QualityChangedEventArgs>>(ref this.QualityChangeFailed, (TypedEventHandler<IVideoHandler, QualityChangedEventArgs>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler1 = this.QualityChangeFailed;
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, QualityChangedEventArgs>>(ref this.QualityChangeFailed, (TypedEventHandler<IVideoHandler, QualityChangedEventArgs>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public event TypedEventHandler<IVideoHandler, SubtitleDeclaration[]> ListOfCaptionsChanged
    {
      add
      {
        TypedEventHandler<IVideoHandler, SubtitleDeclaration[]> typedEventHandler1 = this.ListOfCaptionsChanged;
        TypedEventHandler<IVideoHandler, SubtitleDeclaration[]> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, SubtitleDeclaration[]>>(ref this.ListOfCaptionsChanged, (TypedEventHandler<IVideoHandler, SubtitleDeclaration[]>) Delegate.Combine((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
      remove
      {
        TypedEventHandler<IVideoHandler, SubtitleDeclaration[]> typedEventHandler1 = this.ListOfCaptionsChanged;
        TypedEventHandler<IVideoHandler, SubtitleDeclaration[]> typedEventHandler2;
        do
        {
          typedEventHandler2 = typedEventHandler1;
          typedEventHandler1 = Interlocked.CompareExchange<TypedEventHandler<IVideoHandler, SubtitleDeclaration[]>>(ref this.ListOfCaptionsChanged, (TypedEventHandler<IVideoHandler, SubtitleDeclaration[]>) Delegate.Remove((Delegate) typedEventHandler2, (Delegate) value), typedEventHandler2);
        }
        while (typedEventHandler1 != typedEventHandler2);
      }
    }

    public SubtitleDeclaration[] ListOfCaptions
    {
      get => this.listOfCaptions;
      set
      {
        if (this.listOfCaptions == value)
          return;
        this.listOfCaptions = value;
        this.ListOfCaptionsChanged?.Invoke(this, value);
      }
    }

    public YouTubeQuality CurrentQuality
    {
      get => this.quality;
      private set
      {
        if (this.quality == value)
          return;
        this.quality = value;
      }
    }

    public YouTubeQuality RequestedQuality
    {
      get => this.requestedQuality;
      set
      {
        if (this.requestedQuality == value)
          return;
        this.requestedQuality = value;
      }
    }

    public IEnumerable<YouTubeQuality> AvailableQualities { get; private set; }

    public bool MediaRunning
    {
      get => this.mediaRunning;
      private set
      {
        if (this.mediaRunning == value)
          return;
        this.mediaRunning = value;
        this.OnMediaRunningChanged(value);

                //RnD
                TypedEventHandler<IVideoHandler, MediaRunningChangedArgs> mediaRunningChanged
                            = default;//this.MediaRunningChanged;
        if (mediaRunningChanged != null)
          mediaRunningChanged.Invoke(this, new MediaRunningChangedArgs()
          {
            MediaRunning = value
          });
        if (this.HandlesTransportControls || this.TransportControls == null)
          return;
        if (!this.IsOpeningVideo)
          this.TransportControls.IsEnabled = value;

        this.TransportControls.DisplayUpdater.Update();
      }
    }

    public bool CancelingCasting { get; private set; }

    public async Task CancelCasting()
    {
      lock (this.cancelCastingLock)
      {
        if (this.CancelingCasting)
          return;
        this.CancelingCasting = true;
      }
      this.RemoveThumbnail();
      await this.DeregisterCastingDevice();
      lock (this.cancelCastingLock)
        this.CancelingCasting = false;
    }

    public bool Shuffle
    {
      get => this.shuffle;
      set
      {
        if (this.shuffle == value)
          return;
        this.shuffle = value;
        this.ShuffleChanged(this.shuffle);
        if (this.PlaylistHelper == null)
          return;
        this.PlaylistHelper.Shuffle = value;
      }
    }

    protected virtual void ShuffleChanged(bool shuffle)
    {
    }

    protected virtual void OnMediaRunningChanged(bool mediaRunning)
    {
    }

    protected void UpdateThumbnail(ThumbnailQuality qual) => this.UpdateThumbnail(qual, (Stretch) 3);

    protected void UpdateThumbnail(ThumbnailQuality qual, Stretch stretch)
    {
      if (this.CurrentEntry != null)
      {
        this.UpdateThumbnail(this.CurrentEntry.GetThumb(qual), stretch);
      }
      else
      {
        if (this.AttemptedEntry == null)
          return;
        this.UpdateThumbnail(this.AttemptedEntry.GetThumb(qual), stretch);
      }
    }

    protected void UpdateThumbnail(Uri uri) => this.UpdateThumbnail(uri, (Stretch) 3);

    protected void UpdateThumbnail(Uri uri, Stretch stretch) => this.UpdateThumbnail(uri, (IRandomAccessStream) null, stretch);

    protected void UpdateThumbnail(IRandomAccessStream stream, Stretch stretch) => this.UpdateThumbnail((Uri) null, stream, stretch);

    private void UpdateThumbnail(Uri uri, IRandomAccessStream stream, Stretch stretch)
    {
            /*
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, MediaThumbnailChanged> thumbnailChanged = this.ThumbnailChanged;
      if (thumbnailChanged == null)
        return;
      thumbnailChanged.Invoke(this, new MediaThumbnailChanged()
      {
        Uri = uri,
        Stream = stream,
        StretchMode = stretch
      });
            */
    }

        protected void RemoveThumbnail()
        {
            this.UpdateThumbnail((Uri)null, (IRandomAccessStream)null, (Stretch)3);
        }

        private async void LoadCaptions()
    {
      if (this.CurrentEntry == null)
        return;
      if (!this.SupportsCaptions)
        return;
      try
      {
        this.ListOfCaptions = await Subtitle.GetLanguageList(this.CurrentEntry.ID);
      }
      catch
      {
      }
    }

    public async Task<Subtitle[]> GetCaptions(string language)
    {
      if (this.ListOfCaptions.Length != 0)
      {
        SubtitleDeclaration cap = ((IEnumerable<SubtitleDeclaration>) this.ListOfCaptions).Where<SubtitleDeclaration>((Func<SubtitleDeclaration, bool>) (c => c.LanguageCode == language)).FirstOrDefault<SubtitleDeclaration>() ?? ((IEnumerable<SubtitleDeclaration>) this.ListOfCaptions).Where<SubtitleDeclaration>((Func<SubtitleDeclaration, bool>) (c => c.Default)).FirstOrDefault<SubtitleDeclaration>();
        if (cap != null)
          return await this.GetCaptions(cap);
      }
      return new Subtitle[0];
    }

    public async Task<Subtitle[]> GetCaptions(SubtitleDeclaration cap)
    {
      SubtitleFormat[] subtitleFormatArray = new SubtitleFormat[3]
      {
        SubtitleFormat.SBV,
        SubtitleFormat.TTS,
        SubtitleFormat.VTT
      };
      for (int index = 0; index < subtitleFormatArray.Length; ++index)
      {
        SubtitleFormat format = subtitleFormatArray[index];
        try
        {
          return await cap.GetCaptions(format);
        }
        catch
        {
        }
      }
      subtitleFormatArray = (SubtitleFormat[]) null;
      return new Subtitle[0];
    }

    protected void ReportMediaEnded()
    {
      this.CallBookmarkableAction();
      YouTubeEntry entry = (YouTubeEntry) null;
      if (!this.HandlesPlaylists)
      {
        if (this.PlaylistHelper != null)
        {
          switch (this.PlaylistRepeatMode)
          {
            case PlaylistRepeatMode.One:
              entry = this.CurrentEntry;
              break;
            case PlaylistRepeatMode.All:
              entry = this.PlaylistHelper.GetEntry(1);
              break;
            default:
              entry = (YouTubeEntry) null;
              break;
          }
        }
        else
          entry = this.NormalRepeatMode != PlaylistRepeatMode.One ? (YouTubeEntry) null : this.CurrentEntry;
      }
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, MediaEndedArgs> mediaEnded = this.MediaEnded;
      if (mediaEnded != null)
        mediaEnded.Invoke(this, new MediaEndedArgs()
        {
          CurrentEntry = this.CurrentEntry,
          HasPlaylist = false,
          NextEntry = entry
        });
      if (entry != null && entry != this.CurrentEntry)
      {
        this.OpenVideo(entry, this.RequestedQuality, TimeSpan.Zero, false, true);
        // ISSUE: reference to a compiler-generated field
        TypedEventHandler<IVideoHandler, NextVideoEventArgs> nextVideo = this.NextVideo;
        if (nextVideo == null)
          return;
        nextVideo.Invoke(this, new NextVideoEventArgs()
        {
          CurrentEntry = this.CurrentEntry,
          NextEntry = entry
        });
      }
      else if (entry == this.CurrentEntry)
      {
        this.Position = TimeSpan.Zero;
        this.Play();
      }
      else
      {
        if (entry != null || this.HandlesPlaylists)
          return;
        this.Stop();
      }
    }

    public void ClearExpiredPreloadedVideos()
    {
      List<string> stringList = new List<string>();
      foreach (KeyValuePair<string, VideoInfoPreload> preload in this.preloads)
      {
        if (preload.Value.IsExpired)
          stringList.Add(preload.Key);
      }
      foreach (string key in stringList)
        this.preloads.Remove(key);
    }

    public async void PreloadVideo(string videoID)
    {
            //RnD
      int num = 0;
      VideoInfoPreload videoInfoPreload1;
      if (num != 0 && this.preloads.TryGetValue(videoID, out videoInfoPreload1) 
                && !videoInfoPreload1.IsExpired)
        return;
      try
      {
        VideoInfoPreload videoInfoPreload2 = await this.PreloadRequested(videoID);
        if (videoInfoPreload2 == null)
          return;
        if (this.preloads.ContainsKey(videoID))
          this.preloads[videoID] = videoInfoPreload2;
        else
          this.preloads.Add(videoID, videoInfoPreload2);
      }
      catch
      {
      }
    }

    public void PreloadVideo(string videoID, object videoPreloadObject)
    {
      try
      {
        VideoInfoPreload videoInfoPreload = this.PreloadRequestedWithObject(videoID, videoPreloadObject);
        if (videoInfoPreload == null)
          return;
        if (this.preloads.ContainsKey(videoID))
          this.preloads[videoID] = videoInfoPreload;
        else
          this.preloads.Add(videoID, videoInfoPreload);
      }
      catch
      {
      }
    }

    public bool HasPreloadInfo(string videoID)
    {
      VideoInfoPreload videoInfoPreload;
      return this.preloads.TryGetValue(videoID, out videoInfoPreload) && !videoInfoPreload.IsExpired;
    }

    protected VideoInfoPreload CreatePreloadInfo(
      string videoID,
      TimeSpan expiresIn,
      Task preloadTask)
    {
      return new VideoInfoPreload()
      {
        CreatedAt = DateTimeOffset.Now,
        ExpiresAt = DateTimeOffset.Now + expiresIn,
        DataTask = preloadTask
      };
    }

    protected VideoInfoPreload CreatePreloadInfo<T>(
      string videoID,
      TimeSpan expiresIn,
      T preloadData)
    {
      TaskCompletionSource<T> completionSource = new TaskCompletionSource<T>();
      completionSource.SetResult(preloadData);
      return new VideoInfoPreload()
      {
        CreatedAt = DateTimeOffset.Now,
        ExpiresAt = DateTimeOffset.Now + expiresIn,
        DataTask = (Task) completionSource.Task
      };
    }

    protected virtual VideoInfoPreload PreloadRequestedWithObject(
      string videoId,
      object preloadObject)
    {
      return (VideoInfoPreload) null;
    }

    protected virtual async Task<VideoInfoPreload> PreloadRequested(string videoID) => (VideoInfoPreload) null;

    protected void ReportNextEntry(YouTubeEntry nextEntry)
    {
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, NextVideoEventArgs> nextVideo = this.NextVideo;
      if (nextVideo == null)
        return;
      nextVideo.Invoke(this, new NextVideoEventArgs()
      {
        CurrentEntry = this.CurrentEntry,
        NextEntry = nextEntry
      });
    }

    public async Task<T> GetPreloadedInfo<T>(string videoID)
    {
      VideoInfoPreload videoInfoPreload;
      if (this.preloads.TryGetValue(videoID, out videoInfoPreload))
      {
        if (!videoInfoPreload.IsExpired)
          return await videoInfoPreload.GetData<T>();
        try
        {
          this.preloads.Remove(videoID);
        }
        catch
        {
        }
      }
      return default (T);
    }

    public void DestroyMediaPlayerBrush() => this.DestroyMediaPlayerBrushOverride();

    protected virtual void DestroyMediaPlayerBrushOverride()
    {
    }

    public object GetMediaPlayerBrush() => this.GetMediaPlayerBrushOverride();

    protected virtual object GetMediaPlayerBrushOverride() => (object) null;

    public MediaUsageArgs ApplyMediaElements(VideoHandlerRegistration registration)
    {
      if (this.registered)
        return this.lastMediaUsageArgs;
      this.registered = true;
      if (this.preloadTimer == null)
      {
        this.preloadTimer = new DispatcherTimer();
        this.preloadTimer.Interval = TimeSpan.FromSeconds(10.0);
        DispatcherTimer preloadTimer = this.preloadTimer;
        
        //RnD        
        //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, 
        //    EventRegistrationToken>(preloadTimer.add_Tick), 
        //    new Action<EventRegistrationToken>(preloadTimer.remove_Tick), 
        //    new EventHandler<object>(this.PreloadTimer_Tick));
      }
      MediaUsageArgs mediaUsageArgs = this.lastMediaUsageArgs = this.ApplyMediaElementsOverride(registration);
      if (!this.HandlesTransportControls)
      {
        this.TransportControls = this.GetSystemMediaTransportControls();
        if (this.TransportControls != null)
        {
          this.TransportControls.IsEnabled = true;
          this.TransportControls.DisplayUpdater.Type = (MediaPlaybackType) 1;
          SystemMediaTransportControls transportControls1 = this.TransportControls;

                    //RnD / TODO
                    /*
          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<SystemMediaTransportControls, 
              SystemMediaTransportControlsButtonPressedEventArgs>>(
              new Func<TypedEventHandler<SystemMediaTransportControls, 
              SystemMediaTransportControlsButtonPressedEventArgs>, 
              EventRegistrationToken>(transportControls1.add_ButtonPressed), 
              new Action<EventRegistrationToken>(transportControls1.remove_ButtonPressed), 
              new TypedEventHandler<SystemMediaTransportControls,
              SystemMediaTransportControlsButtonPressedEventArgs>((object) this,
              __methodptr(TransportControls_ButtonPressed)));

          SystemMediaTransportControls transportControls2 = this.TransportControls;

          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<SystemMediaTransportControls,
              SystemMediaTransportControlsPropertyChangedEventArgs>>(
              new Func<TypedEventHandler<SystemMediaTransportControls, 
              SystemMediaTransportControlsPropertyChangedEventArgs>, 
              EventRegistrationToken>(transportControls2.add_PropertyChanged), 
              new Action<EventRegistrationToken>(transportControls2.remove_PropertyChanged),
              new TypedEventHandler<SystemMediaTransportControls, 
              SystemMediaTransportControlsPropertyChangedEventArgs>((object) this,
              __methodptr(TransportControls_PropertyChanged)));
                    */
        }
      }
      this.Dispatcher = registration.Dispatcher;
      return mediaUsageArgs;
    }

    private void PreloadTimer_Tick(object sender, object e)
    {
      if (this.MediaRunning)
      {
        if (!this.HasPlaylist || this.PlaylistHelper == null)
          return;
        YouTubeEntry currentEntry = this.CurrentEntry;
      }
      else
        this.preloadTimer.Stop();
    }

    protected virtual SystemMediaTransportControls GetSystemMediaTransportControls() => SystemMediaTransportControls.GetForCurrentView();

    private void Smtc_ButtonPressed(
      SystemMediaTransportControls sender,
      SystemMediaTransportControlsButtonPressedEventArgs args)
    {
    }

    private void TransportControls_PropertyChanged(
      SystemMediaTransportControls sender,
      SystemMediaTransportControlsPropertyChangedEventArgs args)
    {
    }

    private async void TransportControls_ButtonPressed(
      SystemMediaTransportControls sender,
      SystemMediaTransportControlsButtonPressedEventArgs args)
    {
            /*
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IVideoHandler.\u003C\u003Ec__DisplayClass283_0 displayClass2830 = new IVideoHandler.\u003C\u003Ec__DisplayClass283_0();
      // ISSUE: reference to a compiler-generated field
      displayClass2830.args = args;
      // ISSUE: reference to a compiler-generated field
      displayClass2830.\u003C\u003E4__this = this;
      if (this.Dispatcher == null)
        return;
      // ISSUE: method pointer
      await this.Dispatcher.RunAsync((CoreDispatcherPriority) 1, new DispatchedHandler((object) displayClass2830, __methodptr(\u003CTransportControls_ButtonPressed\u003Eb__0)));
            */
    }

    public async Task Next() => await this.Next(1);

    public async Task Prev() => await this.Next(-1);

    protected virtual async Task Next(int offset)
    {
      IVideoHandler ivideoHandler = this;
      if (ivideoHandler.PlaylistHelper == null || ivideoHandler.IsOpeningVideo)
        return;
      YouTubeEntry entry = ivideoHandler.PlaylistHelper.GetEntry(offset);
      if (entry == null)
        return;
      ivideoHandler.CallBookmarkableAction();
      ivideoHandler.OpenVideo(entry, ivideoHandler.RequestedQuality, TimeSpan.Zero, false, true);
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, NextVideoEventArgs> nextVideo = ivideoHandler.NextVideo;
      if (nextVideo == null)
        return;
      nextVideo.Invoke(ivideoHandler, new NextVideoEventArgs()
      {
        CurrentEntry = ivideoHandler.CurrentEntry,
        NextEntry = entry
      });
    }

    protected abstract MediaUsageArgs ApplyMediaElementsOverride(
      VideoHandlerRegistration registration);

    public Task<bool> OpenVideo(YouTubeEntry entry, YouTubeQuality quality) => this.OpenVideo(entry, quality, TimeSpan.Zero);

    public Task<bool> OpenVideo(YouTubeEntry entry, YouTubeQuality quality, TimeSpan seekToOnOpen) => this.OpenVideo(entry, quality, seekToOnOpen, true, false);

    public async Task<bool> OpenVideoFromCurrentPlaylist(
      YouTubeEntry entry,
      YouTubeQuality quality,
      TimeSpan seekToOnOpen)
    {
      IVideoHandler ivideoHandler = this;
      if (ivideoHandler.PlaylistHelper == null || !ivideoHandler.PlaylistHelper.HasEntryID(entry.ID))
        return await ivideoHandler.OpenVideo(entry, quality, seekToOnOpen);
      ivideoHandler.PlaylistHelper.SetIndexBasedOnID(entry.ID);
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, NextVideoEventArgs> nextVideo = ivideoHandler.NextVideo;
      if (nextVideo != null)
        nextVideo.Invoke(ivideoHandler, new NextVideoEventArgs()
        {
          CurrentEntry = ivideoHandler.CurrentEntry,
          NextEntry = entry
        });
      return await ivideoHandler.OpenVideo(entry, quality, seekToOnOpen, false, true);
    }

    private async Task<bool> OpenVideo(
      YouTubeEntry entry,
      YouTubeQuality quality,
      TimeSpan seekToOnOpen,
      bool newPlaylist,
      bool keepCurrentPlaylist)
    {
      IVideoHandler ivideoHandler = this;
      if (ivideoHandler.IsOpeningVideo && ivideoHandler.AttemptedEntry != null && entry.ID == ivideoHandler.AttemptedEntry.ID)
        return true;
      YouTubeQuality requestedQuality = quality;
      bool different = entry != ivideoHandler.CurrentEntry;
      ivideoHandler.AttemptedEntry = entry;
      if (TransferManager.Current != null && ivideoHandler.SupportsSavedVideos)
      {
        Task<TransferManager.State> videoStateTask = TransferManager.Current.GetTransferState(entry.ID, TransferType.Video);
        TransferManager.State audioState = await TransferManager.Current.GetTransferState(entry.ID, TransferType.Audio);
        TransferManager.State state = await videoStateTask;
        if (audioState == TransferManager.State.Complete && state != TransferManager.State.Complete)
          quality = YouTubeQuality.Audio;
        videoStateTask = (Task<TransferManager.State>) null;
      }
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> mediaOpening = ivideoHandler.MediaOpening;
      if (mediaOpening != null)
        mediaOpening.Invoke(ivideoHandler, new MediaOpenedEventArgs()
        {
          FinalQuality = quality,
          RequestedQuality = requestedQuality,
          Entry = entry
        });
      YouTubeQuality qual = quality;
      ivideoHandler.IsOpeningVideo = true;
      ivideoHandler.listOfCaptions = new SubtitleDeclaration[0];
      try
      {
        qual = await ivideoHandler.OpenVideoOverride(entry, quality, seekToOnOpen);
      }
      catch (Exception ex)
      {
        ivideoHandler.IsOpeningVideo = false;
        // ISSUE: reference to a compiler-generated field
        TypedEventHandler<IVideoHandler, MediaFailedEventArgs> mediaFailed = ivideoHandler.MediaFailed;
        if (mediaFailed != null)
          mediaFailed.Invoke(ivideoHandler, new MediaFailedEventArgs()
          {
            Entry = entry,
            Exception = ex,
            Quality = quality,
            WillContinueToNextVideo = ivideoHandler.PlaylistHelper != null && !(ex is CastingException) && ivideoHandler.PlaylistRepeatMode != 0
          });
        if (ivideoHandler.PlaylistHelper != null && !(ex is CastingException) && ivideoHandler.PlaylistRepeatMode != PlaylistRepeatMode.None)
          ivideoHandler.Next();
        if (!ivideoHandler.IsOpeningVideo && ivideoHandler.State == VideoHandlerState.Unknown || ivideoHandler.State == VideoHandlerState.Stopped)
          ivideoHandler.MediaRunning = false;
        if (ex is CastingException)
          await ivideoHandler.CancelCasting();
        throw;
        return false;
      }
      if (ivideoHandler.SupportsVariablePlaybackRate)
        ivideoHandler.SetPlaybackRate(ivideoHandler.PlaybackRate);
      if (!keepCurrentPlaylist)
        ivideoHandler.SetPlaylist(entry, newPlaylist);
      else if (ivideoHandler.PreloadPlaylistVideos && ivideoHandler.PlaylistRepeatMode == PlaylistRepeatMode.All)
        ivideoHandler.PreloadVideo(ivideoHandler.PlaylistHelper.GetEntryWithoutChangingIndex(1).ID);
      ivideoHandler.IsOpeningVideo = false;
      ivideoHandler.RequestedQuality = quality;
      ivideoHandler.CurrentQuality = qual;
      ivideoHandler.CurrentEntry = entry;
      if (different)
        ivideoHandler.LoadCaptions();

      //RnD
            // ISSUE: reference to a compiler-generated field
            TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> mediaOpened
                      = default;//ivideoHandler.MediaOpened;
      if (mediaOpened != null)
        mediaOpened.Invoke(ivideoHandler, new MediaOpenedEventArgs()
        {
          FinalQuality = qual,
          RequestedQuality = quality,
          Entry = ivideoHandler.CurrentEntry
        });
      if (!ivideoHandler.HandlesTransportControls && ivideoHandler.TransportControls != null)
      {
        ivideoHandler.TransportControls.IsEnabled = true;
        ivideoHandler.TransportControls.DisplayUpdater.Type = ((MediaPlaybackType) 1);
        ivideoHandler.TransportControls.DisplayUpdater.MusicProperties.Title = (entry.Title);
        MusicDisplayProperties musicProperties 
                    = ivideoHandler.TransportControls.DisplayUpdater.MusicProperties;
        string authorDisplayName;

        ivideoHandler.TransportControls.DisplayUpdater.MusicProperties.AlbumArtist 
                    = (authorDisplayName = entry.AuthorDisplayName);
        
        string str = authorDisplayName;
        musicProperties.Artist = str;
        ivideoHandler.TransportControls.DisplayUpdater.Update();
        ivideoHandler.TransportControls.IsPauseEnabled = (true);
        ivideoHandler.TransportControls.IsPlayEnabled = (true);
        ivideoHandler.TransportControls.IsStopEnabled = (true);
        ivideoHandler.TransportControls.IsRewindEnabled = (true);
        ivideoHandler.TransportControls.IsFastForwardEnabled = (true);
        ivideoHandler.TransportControls.IsNextEnabled = (ivideoHandler.PlaylistButtons.NextButton);
        ivideoHandler.TransportControls.IsPreviousEnabled = (true);
      }
      ivideoHandler.ClearExpiredPreloadedVideos();
      return true;
    }

    protected void ReportVideoOpened(YouTubeEntry entry)
    {
      if (this.AttemptedEntry != null && !(this.AttemptedEntry.ID != entry.ID))
        return;
      this.AttemptedEntry = entry;
      this.CurrentEntry = entry;
            // ISSUE: reference to a compiler-generated field
            TypedEventHandler<IVideoHandler, MediaOpenedEventArgs> mediaOpened = default;//this.MediaOpened;
      if (mediaOpened == null)
        return;
      mediaOpened.Invoke(this, new MediaOpenedEventArgs()
      {
        FinalQuality = this.CurrentQuality,
        RequestedQuality = this.CurrentQuality,
        Entry = this.CurrentEntry
      });
    }

    public virtual async Task AddToCurrentPlaylist(YouTubeEntry entry)
    {
      if (!this.SupportsAddToCurrentPlaylist)
        return;
      if (this.PlaylistHelper == null)
      {
        RykenTube.PlaylistHelper playlistHelper = new RykenTube.PlaylistHelper((YouTubeClient<YouTubeEntry>) null);
        playlistHelper.Shuffle = this.Shuffle;
        playlistHelper.ShuffleSeed = this.shuffleRand.Next(0, 100000);
        RykenTube.PlaylistHelper helper = playlistHelper;
        try
        {
          await helper.Load();
        }
        catch
        {
        }
        this.PlaylistHelper = (PlaylistHelperBase) helper;
        if (this.MediaRunning)
          helper.AddEntry(this.CurrentEntry);
        helper.AddEntry(entry);
        if (this.MediaRunning)
          helper.SetIndexBasedOnID(this.CurrentEntry.ID);
        else
          helper.SetIndexBasedOnID(entry.ID);
        try
        {
          await this.SetPlaylistButtons(true, true);
        }
        catch
        {
        }
        helper = (RykenTube.PlaylistHelper) null;
      }
      else
        this.PlaylistHelper.AddEntry(entry);
    }

    private async Task SetPlaylist(YouTubeEntry entry, bool newPlaylist)
    {
      if (entry.Client != null && entry.Client.IsPlaylist)
      {
        if (this.HandlesPlaylists)
          return;
        if (newPlaylist || this.PlaylistHelper == null || !entry.Client.IsEqualTo(this.PlaylistHelper.Client))
        {
          RykenTube.PlaylistHelper playlistHelper = new RykenTube.PlaylistHelper(entry.Client);
          playlistHelper.Shuffle = this.Shuffle;
          playlistHelper.ShuffleSeed = this.shuffleRand.Next(0, 100000);
          RykenTube.PlaylistHelper helper = playlistHelper;
          this.PlaylistHelper = (PlaylistHelperBase) helper;
          helper.SetIndexBasedOnID(entry.ID);
          try
          {
            await helper.Load();
            helper.SetIndexBasedOnID(entry.ID);
            await this.SetPlaylistButtons(true, true);
            if (this.PreloadPlaylistVideos)
            {
              if (this.PlaylistRepeatMode == PlaylistRepeatMode.All)
                this.PreloadVideo(this.PlaylistHelper.GetEntryWithoutChangingIndex(1).ID);
            }
          }
          catch
          {
          }
          helper = (RykenTube.PlaylistHelper) null;
        }
        else
        {
          if (this.PlaylistHelper == null)
            return;
          this.PlaylistHelper.SetIndexBasedOnID(entry.ID);
          if (!this.PreloadPlaylistVideos || this.PlaylistRepeatMode != PlaylistRepeatMode.All)
            return;
          this.PreloadVideo(this.PlaylistHelper.GetEntryWithoutChangingIndex(1).ID);
        }
      }
      else
      {
        this.PlaylistHelper = (PlaylistHelperBase) null;
        await this.SetPlaylistButtons(false, false);
      }
    }

    protected virtual async Task SetPlaylistButtons(bool prevButton, bool nextButton)
    {
      IVideoHandler ivideoHandler = this;
      if (!ivideoHandler.HandlesTransportControls && ivideoHandler.TransportControls != null)
        ivideoHandler.TransportControls.IsNextEnabled = (nextButton);
      ivideoHandler.PlaylistButtons.NextButton = nextButton;
      ivideoHandler.PlaylistButtons.PrevButton = prevButton;

            //RnD
      // ISSUE: reference to a compiler-generated field
      //ivideoHandler.PlaylistButtonsSet?.Invoke(ivideoHandler, ivideoHandler.PlaylistButtons);
    }

    protected virtual async Task SetPlaylistOverride(YouTubeEntry entry)
    {
    }

    public string GetStatus()
    {
      lock (this.logLock)
      {
        if (this.statusBuilder == null)
          this.statusBuilder = new StringBuilder();
        this.statusBuilder.Clear();
        this.GetStatusOverride(this.statusBuilder);
        string str = this.statusBuilder.ToString();
        return string.Format("Handler: {0}\r\nState: {1}\r\nPosition: {2}\r\nVideo: {3}\r\nQuality: (Current: {4}), (Requested: {5})\r\n{6}\r\n", (object) this.GetType().Name, (object) this.State, (object) this.Position, this.CurrentEntry != null ? (object) (this.CurrentEntry.Title ?? "") : (object) "None", (object) this.CurrentQuality, (object) this.RequestedQuality, (object) str);
      }
    }

    protected virtual void GetStatusOverride(StringBuilder builder)
    {
    }

    protected abstract Task<YouTubeQuality> OpenVideoOverride(
      YouTubeEntry entry,
      YouTubeQuality quality,
      TimeSpan seekToOnOpen);

    public async Task<bool> RegisterCastingDevice(VideoCastingDevice device)
    {
      await this.CancelCasting();
      if (!this.SupportsCasting || !this.ShouldRegisterCastingDevice(device))
        return false;
      this.VideoCastingDevice = device;
      return true;
    }

    protected async Task DeregisterCastingDevice()
    {
      if (this.VideoCastingDevice == null)
        return;
      await this.DeregisterCastingDeviceOverride(this.VideoCastingDevice);
      this.VideoCastingDevice = (VideoCastingDevice) null;
    }

    protected abstract Task DeregisterCastingDeviceOverride(VideoCastingDevice device);

    protected virtual bool ShouldRegisterCastingDevice(VideoCastingDevice device) => false;

    public async Task Play() => await this.PlayOverride();

    public async Task Pause()
    {
      this.CallBookmarkableAction();
      await this.PauseOverride();
    }

    public async Task Stop()
    {
      this.CallBookmarkableAction();
      await this.StopOverride();
    }

    public virtual async Task NextFrame(int frameOffset)
    {
    }

    protected async void AddCastingDevice(VideoCastingDevice device)
    {
      IVideoHandler ivideoHandler = this;
      ivideoHandler.CastingDevices.Add(device);
      device.AddedAt = DateTimeOffset.Now;
      // ISSUE: reference to a compiler-generated field
      TypedEventHandler<IVideoHandler, FoundCastingDeviceEventArgs> foundCastingDevice = ivideoHandler.FoundCastingDevice;
      if (foundCastingDevice == null)
        return;
      foundCastingDevice.Invoke(ivideoHandler, new FoundCastingDeviceEventArgs()
      {
        Device = device
      });
    }

    public async Task<IList<VideoCastingDevice>> FindCastingDevices()
    {
      if (this.SupportsCasting)
        await this.FindCastingDevicesOverride();
      return (IList<VideoCastingDevice>) this.CastingDevices;
    }

    protected abstract Task FindCastingDevicesOverride();

    protected void SetAvailableQualities(IEnumerable<YouTubeQuality> qualities)
    {
      this.AvailableQualities = qualities;
            // ISSUE: reference to a compiler-generated field
            TypedEventHandler<IVideoHandler, AvailableQualitiesChangedEventArgs> qualitiesChanged = default;//this.AvailableQualitiesChanged;
      if (qualitiesChanged == null)
        return;
      qualitiesChanged.Invoke(this, new AvailableQualitiesChangedEventArgs()
      {
        AvailableQualities = qualities
      });
    }

    protected abstract Task PlayOverride();

    protected abstract Task PauseOverride();

    protected abstract Task StopOverride();

    public async Task ChangeQuality(YouTubeQuality quality) => await this.ChangeQuality(quality, true);

    public async Task ChangeQuality(YouTubeQuality quality, bool changedByUser)
    {
      IVideoHandler ivideoHandler = this;
      TimeSpan pos = ivideoHandler.Position;
      if (quality == ivideoHandler.CurrentQuality)
        return;
      YouTubeQuality current = ivideoHandler.CurrentQuality;
      YouTubeQuality? nullable = new YouTubeQuality?();
      if (ivideoHandler.SupportsBackgroundAudio || quality != YouTubeQuality.Audio)
        nullable = await ivideoHandler.ChangeQualityOverride(quality);
      if (nullable.HasValue)
      {
        ivideoHandler.RequestedQuality = quality;
        ivideoHandler.CurrentQuality = nullable.Value;
        QualityChangedEventArgs changedEventArgs = new QualityChangedEventArgs()
        {
          NewQuality = nullable.Value,
          OldQuality = current,
          RequestedQuality = quality,
          ChangedByUser = changedByUser,
          Position = pos
        };
        // ISSUE: reference to a compiler-generated field
        ivideoHandler.QualityChanged?.Invoke(ivideoHandler, changedEventArgs);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        TypedEventHandler<IVideoHandler, QualityChangedEventArgs> qualityChangeFailed = ivideoHandler.QualityChangeFailed;
        if (qualityChangeFailed == null)
          return;
        qualityChangeFailed.Invoke(ivideoHandler, new QualityChangedEventArgs()
        {
          NewQuality = quality,
          OldQuality = current,
          RequestedQuality = quality,
          ChangedByUser = changedByUser,
          Position = pos
        });
      }
    }

    protected async Task ReportNewQuality(YouTubeQuality quality)
    {
      IVideoHandler ivideoHandler = this;
      if (quality == ivideoHandler.CurrentQuality)
        return;
      YouTubeQuality currentQuality = ivideoHandler.CurrentQuality;
      ivideoHandler.CurrentQuality = quality;
      QualityChangedEventArgs changedEventArgs = new QualityChangedEventArgs()
      {
        NewQuality = quality,
        OldQuality = currentQuality,
        RequestedQuality = quality,
        ChangedByUser = false
      };
      // ISSUE: reference to a compiler-generated field
      //ivideoHandler.QualityChanged?.Invoke(ivideoHandler, changedEventArgs);
    }

    protected abstract Task<YouTubeQuality?> ChangeQualityOverride(YouTubeQuality quality);

    public void Deregister()
    {
      if (!this.registered)
        return;
      this.registered = false;
      this.DeregisterOverride();
      this.DeregisterCastingDevice();

      if (this.TransportControls == null)
        return;

      this.TransportControls.IsEnabled = false;
      // ISSUE: method pointer
      //WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<SystemMediaTransportControls, SystemMediaTransportControlsButtonPressedEventArgs>>(new Action<EventRegistrationToken>(this.TransportControls.remove_ButtonPressed), new TypedEventHandler<SystemMediaTransportControls, SystemMediaTransportControlsButtonPressedEventArgs>((object) this, __methodptr(TransportControls_ButtonPressed)));
      // ISSUE: method pointer
      //WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs>>(new Action<EventRegistrationToken>(this.TransportControls.remove_PropertyChanged), new TypedEventHandler<SystemMediaTransportControls, SystemMediaTransportControlsPropertyChangedEventArgs>((object) this, __methodptr(TransportControls_PropertyChanged)));
      this.TransportControls = (SystemMediaTransportControls) null;
    }

    protected abstract void DeregisterOverride();

    public void SetSize(Size size) => this.SetSizeOverride(size);

    protected virtual void SetSizeOverride(Size size)
    {
    }

    public void UpdateSphericalOrientationWithManipulation(
      float manipulationX,
      float manipulationY,
      Size elementSize)
    {
      if (this.Projection != VideoProjection.Spherical)
        return;
      this.UpdateSphericalOrientationWithManipulationOverride(
          manipulationX, manipulationY, elementSize);
    }

    protected virtual void UpdateSphericalOrientationWithManipulationOverride(
      float manipulationX,
      float manipulationY,
      Size elementSize)
    {
    }

    public void UpdateSphericalOrientationFromGyroscope(
      float readingX,
      float readingY,
      float readingZ)
    {
      this.UpdateSphericalOrientationFromGyroscopeOverride(readingX, readingY, readingZ);
    }

    protected virtual void UpdateSphericalOrientationFromGyroscopeOverride(
      float readingX,
      float readingY,
      float readingZ)
    {
    }

    public void UpdateSphericalOrientation(float? rotationX, float? rotationY, float? rotationZ) => this.UpdateSphericalOrientationOverride(rotationX, rotationY, rotationZ);

    protected virtual void UpdateSphericalOrientationOverride(
      float? rotationX,
      float? rotationY,
      float? rotationZ)
    {
    }

    public void ResetSphericalOrientation(float? rotationX, float? rotationY, float? rotationZ) => this.ResetSphericalOrientationOverride(rotationX, rotationY, rotationZ);

    protected virtual void ResetSphericalOrientationOverride(
      float? rotationX,
      float? rotationY,
      float? rotationZ)
    {
    }
  }
}
