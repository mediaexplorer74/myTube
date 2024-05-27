// myTube.GlobalAppObjects.GlobalObjects

using RykenTube;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace myTube.GlobalAppObjects
{
  public partial class GlobalObjects : INotifyPropertyChanged
  {
    private const string Tag = "GlobalObjects";
    private OfflinePlaylistsCollection offlinePlaylists;
    private ChannelNotifications channelNotifications;
    private DataTemplate videoThumbTemplate;
    private TimeBookmarksManager bookmarksManager;
    private TransferManager transferManager;
    private UploadsManager uploadsManager;
    private RoamingHistory<YouTubeEntry, YouTubeEntryClient> history;
    private Thickness defaultMargin;
    private TaskCompletionSource<bool> tcs;

    public ChannelNotifications ChannelNotifications => this.channelNotifications;

    public OfflinePlaylistsCollection OfflinePlaylistsCollection => this.offlinePlaylists;

    public TimeBookmarksManager TimeBookmarksManager => this.bookmarksManager;

    public DataTemplate VideoThumbTemplate
    {
      get => this.videoThumbTemplate;
      set
      {
        if (this.videoThumbTemplate == value)
          return;
        this.videoThumbTemplate = value;
        this.opc(nameof (VideoThumbTemplate));
      }
    }

    public TransferManager TransferManager
    {
        get
        {
            return this.transferManager;
        }
    }

    public UploadsManager UploadsManager
    {
        get
        {
            return this.uploadsManager;
        }
    }

    public string PackageName
    {
        get
        {
            return Package.Current.Id.Name;
        }
    }

    public string PackageFamilyName
    {
        get
        {
            return Package.Current.Id.FamilyName;
        }
    }

    public Version Version
    {
      get
      {
        PackageVersion version = Package.Current.Id.Version;
        return new Version(
            (int) version.Major, 
            (int) version.Minor, 
            (int) version.Build, 
            (int) version.Revision);
      }
    }

        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public RoamingHistory<YouTubeEntry, YouTubeEntryClient> History
        {
            get
            {
                return this.history;
            }
        }

        public Thickness DefaultMargin
        {
            get
            {
                return this.defaultMargin;
            }
        }

        public Task<bool> InitializedTask => this.tcs.Task;

    public GlobalObjects()
    {
      this.tcs = new TaskCompletionSource<bool>();
      this.defaultMargin = new Thickness(0.0, 9.5, 0.0, 0.0);
    }

    public async Task Initialize()
    {
        await this.init();
    }

    private async Task init()
    {
      Helper.Write((object) nameof (GlobalObjects), (object) "Initializing");
      this.bookmarksManager = new TimeBookmarksManager();
      await this.bookmarksManager.Load();
      Helper.Write((object) nameof (GlobalObjects), (object) "Loaded bookmarks manager");
      GlobalObjects globalObjects = this;
      TransferManager transferManager1 = globalObjects.transferManager;
      TransferManager transferManager2 = await TransferManager.Load();
      globalObjects.transferManager = transferManager2;
      globalObjects = (GlobalObjects) null;
      globalObjects = this;
      UploadsManager uploadsManager1 = globalObjects.uploadsManager;
      UploadsManager uploadsManager2 = await UploadsManager.Load();
      globalObjects.uploadsManager = uploadsManager2;
      globalObjects = (GlobalObjects) null;

      await this.uploadsManager.Clean();

      Helper.Write((object) nameof (GlobalObjects), (object) "Loaded transfer manager");
      try
      {
        globalObjects = this;
        RoamingHistory<YouTubeEntry, YouTubeEntryClient> history = globalObjects.history;
        RoamingHistory<YouTubeEntry, YouTubeEntryClient> roamingHistory 
                    = await RoamingHistory<YouTubeEntry, YouTubeEntryClient>.Load();
        globalObjects.history = roamingHistory;
        globalObjects = (GlobalObjects) null;
      }
      catch
      {
        this.history = new RoamingHistory<YouTubeEntry, YouTubeEntryClient>();
      }
      Helper.Write((object) nameof (GlobalObjects), (object) "Loaded history");
      globalObjects = this;
      OfflinePlaylistsCollection offlinePlaylists = globalObjects.offlinePlaylists;
      OfflinePlaylistsCollection playlistsCollection = await OfflinePlaylistsCollection.Load();
      globalObjects.offlinePlaylists = playlistsCollection;
      globalObjects = (GlobalObjects) null;
      Helper.Write((object) nameof (GlobalObjects), (object) "Loaded offline playlists collection");
      Helper.Write((object) nameof (GlobalObjects), (object) "Initializeation complete");
      globalObjects = this;
      ChannelNotifications channelNotifications1 = globalObjects.channelNotifications;
      ChannelNotifications channelNotifications2 = await ChannelNotifications.Load();
      globalObjects.channelNotifications = channelNotifications2;
      globalObjects = (GlobalObjects) null;
      this.tcs.TrySetResult(true);
    }

    private void opc(string prop)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;
  }
}
