// myTube.Settings


using Newtonsoft.Json.Linq;
using RykenTube;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public static class Settings
  {
    private const string PageInfoCollectionFileName = "PageInfoCollection.xml";
    private static ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
    private static ApplicationDataContainer roaming;
    private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
    private static StorageFolder roamingFolder;

    public static bool Allow60FPS
    {
      set => ((IDictionary<string, object>) Settings.local.Values)["Prefer60FPS"] = (object) value;
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)["Prefer60FPS"];
        }
        catch
        {
          return false;
        }
      }
    }

    public static string LastClipboardId
    {
      get => ((IDictionary<string, object>) Settings.local.Values).ContainsKey(nameof (LastClipboardId)) ? (string) ((IDictionary<string, object>) Settings.local.Values)[nameof (LastClipboardId)] : (string) null;
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (LastClipboardId)] = (object) value;
    }

    public static string UnhandledException
    {
      get => ((IDictionary<string, object>) Settings.local.Values).ContainsKey(nameof (UnhandledException)) ? ((IDictionary<string, object>) Settings.local.Values)[nameof (UnhandledException)] as string : (string) null;
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (UnhandledException)] = (object) value;
    }

    public static bool AllowChangeMiniPlayerType => App.DeviceFamily == DeviceFamily.Desktop;

    public static MiniPlayerType MiniPlayerType
        {
            get
            {
                try
                {
                    return App.DeviceFamily == DeviceFamily.Mobile
                                  ? MiniPlayerType.Background
                                  : (MiniPlayerType)Enum.Parse(typeof(MiniPlayerType), ((IDictionary<string, object>)Settings.local.Values)[nameof(MiniPlayerType)].ToString());
                }
                catch
                {
                    return App.DeviceFamily == DeviceFamily.Desktop
                                  ? MiniPlayerType.MiniPlayer : MiniPlayerType.Background;
                }
            }
            set
            {
                ((IDictionary<string, object>)
                      Settings.local.Values)[nameof(MiniPlayerType)] = (object)value.ToString();
            }
        }

        public static ColorSchemes ColorCheme
        {
            set
            {
                ((IDictionary<string, object>)Settings.local.Values)["ColorScheme"] 
                    = (object)value.ToString();
            }

            get
            {
                try
                {
                    Helper.Write((object)"Returning ColorScheme");
                    if (((IDictionary<string, object>)Settings.local.Values).ContainsKey("ColorScheme"))
                        return (ColorSchemes)Enum.Parse(typeof(ColorSchemes),
                            ((IDictionary<string, object>)Settings.local.Values)["ColorScheme"].ToString());

                    Helper.Write((object)"ColorScheme doesn't exist, returning default");
                    return ColorSchemes.Default;
                }
                catch
                {
                    Helper.Write((object)"Returning Default ColorScheme");
                    return ColorSchemes.Default;
                }
            }
        }

        public static YouTubeOrder CommentsOrder
        {
            get
            {
                try
                {
                    return ((IDictionary<string, object>)Settings.roaming.Values)
                                  .ContainsKey(nameof(CommentsOrder))
                                  ? (YouTubeOrder)Enum.Parse(typeof(YouTubeOrder),
                                  ((IDictionary<string, object>)Settings.roaming.Values)[nameof(CommentsOrder)]
                                  .ToString())
                                  : YouTubeOrder.Relevance;
                }
                catch
                {
                    return YouTubeOrder.Relevance;
                }
            }
            set
            {
                ((IDictionary<string, object>)Settings.roaming.Values)[nameof(CommentsOrder)]
                    = (object)value.ToString();
            }
        }

        public static RotationType RotationType
        {
            get
            {
                try
                {
                    //RnD
                    return RotationType.System;/*App.DeviceFamily == DeviceFamily.Desktop
                                  ? RotationType.System
                                  : (RotationType)Enum.Parse(typeof(RotationType),
                                  ((IDictionary<string, object>)Settings.roaming.Values)["rotationType"]
                                  .ToString());*/
                }
                catch
                {
                    return App.DeviceFamily == DeviceFamily.Mobile
                                  ? RotationType.Custom
                                  : RotationType.System;
                }
            }
            set
            {
                ((IDictionary<string, object>)Settings.roaming.Values)["rotationType"]
                    = (object)value.ToString();
            }
    }

    public static double Volume
    {
      get => ((IDictionary<string, object>) Settings.local.Values).ContainsKey(nameof (Volume)) 
                ? (double) ((IDictionary<string, object>) Settings.local.Values)[nameof (Volume)] 
                : 1.0;

      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (Volume)] 
                = (object) value;
    }

    public static bool SearchInAppBar
    {
      get
      {
        if (App.DeviceFamily != DeviceFamily.Mobile)
          return false;
        if (((IDictionary<string, object>) Settings.roaming.Values).ContainsKey(nameof (SearchInAppBar)))
          return (bool) ((IDictionary<string, object>) Settings.roaming.Values)[nameof (SearchInAppBar)];
        return App.DeviceFamily == DeviceFamily.Mobile;
      }
      set => ((IDictionary<string, object>) Settings.roaming.Values)[nameof (SearchInAppBar)] = (object) value;
    }

    public static bool UsedNewMessageSystem
    {
      get => ((IDictionary<string, object>) Settings.roaming.Values).ContainsKey(nameof (UsedNewMessageSystem)) && (bool) ((IDictionary<string, object>) Settings.roaming.Values)[nameof (UsedNewMessageSystem)];
      set => ((IDictionary<string, object>) Settings.roaming.Values)[nameof (UsedNewMessageSystem)] = (object) value;
    }

    public static bool BlurVideo
    {
      get => false;
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (BlurVideo)] = (object) value;
    }

    public static long LastMessageDate
    {
      get
      {
        try
        {
          return (long) ((IDictionary<string, object>) Settings.roaming.Values)["lastMessageDate"];
        }
        catch
        {
          return -1;
        }
      }
      set => ((IDictionary<string, object>) Settings.roaming.Values)["lastMessageDate"] = (object) value;
    }

    public static string Cipher
    {
      get
      {
        try
        {
          return ((IDictionary<string, object>) Settings.local.Values)["cipher"].ToString();
        }
        catch
        {
          return (string) null;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)["cipher"] = (object) value;
    }

    public static DateTimeOffset TranslationDownloadedAt
    {
      get => ((IDictionary<string, object>) Settings.local.Values)
                .ContainsKey(nameof (TranslationDownloadedAt)) 
                ? (DateTimeOffset) ((IDictionary<string, object>) 
                Settings.local.Values)[nameof (TranslationDownloadedAt)]
                : DateTimeOffset.MinValue;
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (TranslationDownloadedAt)]
                = (object) value;
    }

    public static DateTimeOffset SuspendedAt
    {
      get
      {
        try
        {
          return (DateTimeOffset) ((IDictionary<string, object>) Settings.local.Values)["suspendedAt"];
        }
        catch
        {
          return DateTimeOffset.MinValue;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)["suspendedAt"] = (object) value;
    }

    public static ElementTheme Theme
    {
      get
      {
        try
        {
          return ((IDictionary<string, object>) Settings.local.Values).ContainsKey("elementTheme")
                        ? (ElementTheme) Enum.Parse(typeof (ElementTheme), 
                        (string) ((IDictionary<string, object>) Settings.local.Values)["elementTheme"])
                        : (ElementTheme) 2;
        }
        catch
        {
          return (ElementTheme) 2;
        }
      }
      set
      {
        if (Settings.Theme == value)
          return;
        ((IDictionary<string, object>) Settings.local.Values)["elementTheme"] = (object) value.ToString();
      }
    }

    public static ThumbnailStyle Thunbnail
    {
      get
      {
        try
        {
          return ((IDictionary<string, object>) Settings.local.Values).ContainsKey("thumbnail")
                        ? (ThumbnailStyle) Enum.Parse(typeof (ThumbnailStyle), 
                        (string) ((IDictionary<string, object>) Settings.local.Values)["thumbnail"])
                        : ThumbnailStyle.New;
        }
        catch
        {
          return ThumbnailStyle.New;
        }
      }
      set
      {
        if (Settings.Thunbnail == value)
          return;
        ((IDictionary<string, object>) Settings.local.Values)["thumbnail"] = (object) value.ToString();
      }
    }

    public static YouTubeQuality Quality
    {
      get
      {
        try
        {
          return (YouTubeQuality) Enum.Parse(typeof (YouTubeQuality),
              (string) ((IDictionary<string, object>) Settings.local.Values)["quality"]);
        }
        catch
        {
          return YouTubeQuality.HQ;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)["quality"] = (object) value.ToString();
    }

    public static bool KeepSelectedQuality
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)["keepQuality"];
        }
        catch
        {
          return true;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)["keepQuality"] = (object) value;
    }

    public static bool Shuffle
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (Shuffle)];
        }
        catch
        {
          return false;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (Shuffle)] = (object) value;
    }

    public static int Runs
    {
      get => ((IDictionary<string, object>) Settings.local.Values).ContainsKey("runs")
                ? (int) ((IDictionary<string, object>) Settings.local.Values)["runs"] : 0;
      set => ((IDictionary<string, object>) Settings.local.Values)["runs"] = (object) value;
    }

    public static UserMode UserMode
    {
      get
      {
        try
        {
          return (UserMode) Enum.Parse(typeof (UserMode), 
              (string) ((IDictionary<string, object>) Settings.roaming.Values)[nameof (UserMode)]);
        }
        catch
        {
          return UserMode.Normal;
        }
      }
      set => ((IDictionary<string, object>) 
                Settings.roaming.Values)[nameof (UserMode)] = (object) value.ToString();
    }

    public static string RykenUserID
    {
      get
      {
        try
        {
          return (string) ((IDictionary<string, object>) Settings.roaming.Values)[nameof (RykenUserID)];
        }
        catch
        {
          return (string) null;
        }
      }
      set => ((IDictionary<string, object>) Settings.roaming.Values)[nameof (RykenUserID)] = (object) value;
    }

    public static Version Version
        {
            get
            {
                try
                {
                    return new Version(2, 9, 0, 0);//return Version.Parse(((IDictionary<string, object>) 
                                                   //    Settings.local.Values)[nameof (Version)].ToString());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[ex] Settings - Version - get: " + ex.Message);
                    return new Version(1, 0, 0, 0);
                }
            }
            set
            {
                try
                {
                    ((IDictionary<string, object>)Settings.local.Values)
                          [nameof(Version)] = (object)value.ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[ex] Settings - Version - set: " + ex.Message);
                }
            }
        }

        public static DateTimeOffset BackgroundTaskLastRun
    {
      get
      {
        try
        {
          return (DateTimeOffset) ((IDictionary<string, object>) 
                        Settings.local.Values)[nameof (BackgroundTaskLastRun)];
        }
        catch
        {
          return DateTimeOffset.MinValue;
        }
      }
    }

    public static PlaylistRepeatMode PlaylistRepeatMode
    {
      get
      {
        try
        {
          return (PlaylistRepeatMode) Enum.Parse(typeof (PlaylistRepeatMode),
              ((IDictionary<string, object>) Settings.local.Values)[nameof (PlaylistRepeatMode)].ToString());
        }
        catch
        {
          return PlaylistRepeatMode.All;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (PlaylistRepeatMode)]
                = (object) value.ToString();
    }

    public static AfterVideoSection AfterVideoSection
    {
      get
      {
        try
        {
          return (AfterVideoSection) Enum.Parse(typeof (AfterVideoSection), ((IDictionary<string, object>)
              Settings.local.Values)[nameof (AfterVideoSection)].ToString());
        }
        catch
        {
          return AfterVideoSection.Suggested;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (AfterVideoSection)]
                = (object) value.ToString();
    }

    public static PlaylistRepeatMode NormalRepeatMode
    {
      get
      {
        try
        {
          return (PlaylistRepeatMode) Enum.Parse(typeof (PlaylistRepeatMode), ((IDictionary<string, object>) Settings.local.Values)[nameof (NormalRepeatMode)].ToString());
        }
        catch
        {
          return PlaylistRepeatMode.None;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (NormalRepeatMode)] = (object) value.ToString();
    }

    public static string BackgroundTaskState
    {
      get
      {
        try
        {
          return (string) ((IDictionary<string, object>) Settings.local.Values)[nameof (BackgroundTaskState)];
        }
        catch
        {
          return (string) null;
        }
      }
    }

    public static bool ResumeAsAudio
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (ResumeAsAudio)];
        }
        catch
        {
          return false;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (ResumeAsAudio)] = (object) value;
    }

    public static bool WarnOnData
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (WarnOnData)];
        }
        catch
        {
          return false;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (WarnOnData)] = (object) value;
    }

    public static bool WasPaidFor
    {
      get => ((IDictionary<string, object>) Settings.roaming.Values).ContainsKey(nameof (WasPaidFor)) && (bool) ((IDictionary<string, object>) Settings.roaming.Values)[nameof (WasPaidFor)];
      set => ((IDictionary<string, object>) Settings.roaming.Values)[nameof (WasPaidFor)] = (object) value;
    }

    public static bool AutoShowDevMessages
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (AutoShowDevMessages)];
        }
        catch
        {
          return false;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (AutoShowDevMessages)] = (object) value;
    }

    public static Stretch Stretch
    {
      get
      {
        try
        {
          return (Stretch) Enum.Parse(typeof (Stretch), (string) ((IDictionary<string, object>) Settings.local.Values)[nameof (Stretch)]);
        }
        catch
        {
          return (Stretch) 2;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (Stretch)] = (object) value.ToString();
    }

    public static bool FoundOldVideos
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (FoundOldVideos)];
        }
        catch
        {
          return false;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)["FoundOldValues"] = (object) value;
    }

    public static bool UseNavigatePage
    {
      get => false;
      set => ((IDictionary<string, object>) Settings.local.Values)["UseNavigationPage"] = (object) value;
    }

    public static DateTimeOffset TrialEndedAt
    {
      get
      {
        try
        {
          return ((IDictionary<string, object>) Settings.local.Values).ContainsKey(nameof (TrialEndedAt)) ? (DateTimeOffset) ((IDictionary<string, object>) Settings.local.Values)[nameof (TrialEndedAt)] : DateTimeOffset.MinValue;
        }
        catch
        {
          return DateTimeOffset.MinValue;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (TrialEndedAt)] = (object) value;
    }

    public static DateTimeOffset StartedWatchingAt
    {
      get
      {
        try
        {
          return (DateTimeOffset) ((IDictionary<string, object>) Settings.local.Values)[nameof (StartedWatchingAt)];
        }
        catch
        {
          return DateTimeOffset.MinValue;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (StartedWatchingAt)] = (object) value;
    }

    public static TileStyle TileStyle
    {
      get
      {
        try
        {
          return (TileStyle) Enum.Parse(typeof (TileStyle), 
              (string) ((IDictionary<string, object>) Settings.local.Values)[nameof (TileStyle)]);
        }
        catch
        {
          return TileStyle.Default;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (TileStyle)]
                = (object) value.ToString();
    }

    public static RegionInfo Region
    {
      get
      {
        try
        {
          return RegionInfo.GetFromCode((string) ((IDictionary<string, object>) 
              Settings.roaming.Values)[nameof (Region)]);
        }
        catch
        {
          try
          {
            return RegionInfo.GetFromCode((string) ((IDictionary<string, object>) 
                Settings.local.Values)[nameof (Region)]);
          }
          catch
          {
            return new RegionInfo("Worldwide (All)", "US");
          }
        }
      }
      set => ((IDictionary<string, object>) Settings.roaming.Values)
                [nameof (Region)] = (object) value.CountryCode;
    }

    public static string ProductKeyRequestId
    {
      get
      {
        try
        {
          return (string) ((IDictionary<string, object>) Settings.roaming.Values)["ProductKeyReqestId"];
        }
        catch
        {
          return (string) null;
        }
      }
      set => ((IDictionary<string, object>) Settings.roaming.Values)["ProductKeyReqestId"] = (object) value;
    }

    public static string ProductKey
    {
      get
      {
        try
        {
          return (string) ((IDictionary<string, object>) Settings.roaming.Values)[nameof (ProductKey)];
        }
        catch
        {
          return (string) null;
        }
      }
      set => ((IDictionary<string, object>) Settings.roaming.Values)[nameof (ProductKey)] = (object) value;
    }

    public static bool PlayAutomatically
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (PlayAutomatically)];
        }
        catch
        {
          return false;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (PlayAutomatically)] = (object) value;
    }

    public static bool Annotations
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (Annotations)];
        }
        catch
        {
          return true;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (Annotations)] = (object) value;
    }

    public static bool Subtitles
    {
      get
      {
        try
        {
          return (bool) ((IDictionary<string, object>) Settings.local.Values)[nameof (Subtitles)];
        }
        catch
        {
          return true;
        }
      }
      set => ((IDictionary<string, object>) Settings.local.Values)[nameof (Subtitles)] = (object) value;
    }

    static Settings()
    {
      Settings.roaming = ApplicationData.Current.RoamingSettings;
      Settings.roamingFolder = ApplicationData.Current.RoamingFolder;
      ApplicationData current = ApplicationData.Current;
    
      //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<ApplicationData, object>>
      //          (new Func<TypedEventHandler<ApplicationData, object>, 
      //          EventRegistrationToken>(current.add_DataChanged), new Action<EventRegistrationToken>(
      //              current.remove_DataChanged), new TypedEventHandler<ApplicationData, object>((object) null,
      //              __methodptr(Current_DataChanged)));
      current.DataChanged += Current_DataChanged;

    ((IDictionary<string, object>) Settings.roaming.Values).ContainsKey("accounts");
      ((IDictionary<string, object>) Settings.roaming.Values).ContainsKey("currentAccount");
    }

        private static void Current_DataChanged(ApplicationData sender, object args)
        {
            Helper.Write((object)"Data changed");
        }

        public static async Task<PageInfoCollection> GetPageInfoCollection()
    {
      try
      {
        return new PageInfoCollection(
            XElement.Parse(await FileIO.ReadTextAsync(
                (IStorageFile) await Settings.localFolder.GetFileAsync("PageInfoCollection.xml"))));
      }
      catch
      {
        if (Debugger.IsAttached)
          Debugger.Break();
        return (PageInfoCollection) null;
      }
    }

    public static PageInfoCollection GetPageInfoCollection(CustomFrame frame)
    {
      PageInfoCollection pageInfoCollection = new PageInfoCollection();

      if (frame != null)
      {
        for (int index = 0; index < frame.BackStack.Count; ++index)
            pageInfoCollection.AddPage(new PageInfo(frame.BackStack[index]));
        if (((ContentControl)frame).Content != null)
            pageInfoCollection.AddPage(new PageInfo(frame.SourcePageType,
                (((ContentControl)frame).Content as FrameworkElement).DataContext));
      }
      return pageInfoCollection;
    }

    public static async Task SavePageInfoCollection(CustomFrame frame)
    {
      try
      {
        PageInfoCollection pic = Settings.GetPageInfoCollection(frame);
        await FileIO.WriteTextAsync(
            (IStorageFile) await Settings.localFolder.CreateFileAsync("PageInfoCollection.xml",
            (CreationCollisionOption) 1), ((object) pic.XML).ToString());
        pic = (PageInfoCollection) null;
      }
      catch (Exception ex)
      {
        if (!Debugger.IsAttached)
          return;
        Debugger.Break();
      }
    }

    public static void Save()
    {
    }

    public static class WatchLater
    {
      public static PlaylistPosition AddVideosTo
      {
        get
        {
          try
          {
            return (PlaylistPosition) Enum.Parse(typeof (PlaylistPosition), 
                (string) ((IDictionary<string, object>) Settings.roaming.Values)["watchLaterPos"]);
          }
          catch
          {
            return PlaylistPosition.End;
          }
        }
        set => ((IDictionary<string, object>) Settings.roaming.Values)["watchLaterPos"] 
                    = (object) value.ToString();
      }
    }

    public static class Subscriptions
    {
      private const string Key = "subscriptionData";
      private const string filterName = "subscriptionFilters.json";
      private static Dictionary<string, List<string>> filters;
      private static ApplicationDataCompositeValue values;
      private static ApplicationDataContainer local = ApplicationData.Current.LocalSettings;
      private static ApplicationDataContainer roaming = ApplicationData.Current.RoamingSettings;
      private static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
      private static StorageFolder roamingFolder = ApplicationData.Current.RoamingFolder;

      public static bool AllChannels
      {
        get
        {
          try
          {
            return (bool) ((IDictionary<string, object>) Settings.Subscriptions.values)["allChannels"];
          }
          catch
          {
            return true;
          }
        }
        set => ((IDictionary<string, object>) Settings.Subscriptions.values)["allChannels"]
                    = (object) value;
      }

      public static bool FilterChannels
      {
        get
        {
          try
          {
            return (bool) ((IDictionary<string, object>) Settings.Subscriptions.values)["filterChannels"];
          }
          catch
          {
            return false;
          }
        }
        set => ((IDictionary<string, object>) Settings.Subscriptions.values)["filterChannels"] 
                    = (object) value;
      }

      static Subscriptions()
      {
        if (((IDictionary<string, object>) Settings.Subscriptions.local.Values)
                    .ContainsKey("subscriptionData"))
          Settings.Subscriptions.values = (ApplicationDataCompositeValue) 
                        ((IDictionary<string, object>) Settings.Subscriptions.local.Values)["subscriptionData"];
        else
          Settings.Subscriptions.values = new ApplicationDataCompositeValue();
      }

      private static async Task InitializeFilter()
      {
        if (Settings.Subscriptions.filters != null)
          return;
        try
        {
          Settings.Subscriptions.filters = JToken.Parse(
              await FileIO.ReadTextAsync((IStorageFile) 
              await Settings.Subscriptions.localFolder.GetFileAsync("subscriptionFilters.json")))
                        .ToObject<Dictionary<string, List<string>>>();
        }
        catch
        {
          Settings.Subscriptions.filters = new Dictionary<string, List<string>>();
        }
      }

      public static async Task<List<string>> GetFilteredItems(string userChannelId)
      {
        if (!Settings.Subscriptions.FilterChannels)
          return new List<string>();
        await Settings.Subscriptions.InitializeFilter();
        return !Settings.Subscriptions.filters.ContainsKey(userChannelId) 
                    ? new List<string>() 
                    : new List<string>((IEnumerable<string>) Settings.Subscriptions.filters[userChannelId]);
      }

      public static async Task<bool> IsFilteredOut(string userChannelId, string subChannelId)
      {
        await Settings.Subscriptions.InitializeFilter();
        return Settings.Subscriptions.filters.ContainsKey(userChannelId)
                    && Settings.Subscriptions.filters[userChannelId].Contains(subChannelId);
      }

      public static async Task SetFilter(string userChannelId, List<string> subscriptionIds)
      {
        if (subscriptionIds == null)
          return;
        await Settings.Subscriptions.InitializeFilter();
        if (Settings.Subscriptions.filters.ContainsKey(userChannelId))
          Settings.Subscriptions.filters[userChannelId] = subscriptionIds;
        else
          Settings.Subscriptions.filters.Add(userChannelId, subscriptionIds);
      }

      public static async Task Save()
      {
        ((IDictionary<string, object>) Settings.Subscriptions.local.Values)["subscriptionData"] 
                    = (object) Settings.Subscriptions.values;
        if (Settings.Subscriptions.filters == null)
          return;
        JToken token = JToken.FromObject((object) Settings.Subscriptions.filters);
        await FileIO.WriteTextAsync(
            (IStorageFile) await Settings.Subscriptions.localFolder.CreateFileAsync(
                "subscriptionFilters.json", (CreationCollisionOption) 1), ((object) token).ToString());
        token = (JToken) null;
      }
    }
  }
}
