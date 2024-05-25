using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using myTube.Cloud;
using myTube.Cloud.Clients;
using myTube.Cloud.Data;
using myTube.GlobalAppObjects;
using myTube.Helpers;
using myTube.MessageDialogs;
using myTube.Popups;
using myTube.ShareTargets;
using RykenTube;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestFramework;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Store;
using Windows.Graphics.Display;
using Windows.Media.Playback;
using Windows.Phone.UI.Input;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.System;
using Windows.System.Threading;
using Windows.System.UserProfile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.Web.Http.Filters;


namespace myTube
{

    public partial class App : Application
    {
        private const string Tag = "App";
        public const string ExceptionFileName = "exception.json";
        public const string SupportEmail = "rykenproductions@outlook.com";
        private static readonly EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
        private TransitionCollection transitions;
        private CustomFrame rootFrame;
        private static bool isFullScreen = false;
        private static TimeSpan startTime;
        private static GlobalObjects gObjects;
        private static List<string> apiKeys = new List<string>();
        //public List<ColorSchemes> AddedSchemes = new List<ColorSchemes>();
        //private ColorSchemes curentScheme;
        private Dictionary<string, Dictionary<string, SolidColorBrush>> themeCollection;
        //private static ThumbnailDispatcher thumbnailDispatcher = new ThumbnailDispatcher();
        private static TaskDispatcher taskDispatcher = new TaskDispatcher();
        private GlobalObjects globalObjects;
        private bool backgroundAudio;
        private YouTubeQuality backgroundQuality = YouTubeQuality.HQ;
        private StreamWriter logWriter;
        private bool initialThemeSetup;
        private List<Exception> exceptions = new List<Exception>();
        private LaunchActivatedEventArgs launchArgs;
        private IActivatedEventArgs activationArgs;
        private static DateTime signedInAt = DateTime.MinValue;
        private static bool trySignIn = true;
        private static DateTime cipherTime = DateTime.MinValue;
        private static DateTime lastCheckedMessaged = DateTime.MinValue;
        private TaskCompletionSource<bool> windowActivatedTask;
        private bool alreadyActivated;
        private ClipboardPopup clipboardControl;
        private bool initialized;
        private static TileArgs launchTile;
        private PageInfoCollection pageInfoCollection;
        private static object u003E9__135_0;
        private static ThumbnailDispatcher thumbnailDispatcher;
        private ColorSchemes curentScheme;

        public App()
        {
            Helper.Logged += new EventHandler<string>(this.Helper_Logged);
            Helper.StartTimer();
            App.Instance = this;

            this.InitializeComponent();

            Helper.Write((object)nameof(App), (object)"Initialized component");

            //TODO: handlers for unhandled exceptions
            //WindowsRuntimeMarshal.AddEventHandler<UnhandledExceptionEventHandler>(
            //    new Func<UnhandledExceptionEventHandler, EventRegistrationToken>(((Application)this)
            //    .add_UnhandledException), 
            //    new Action<EventRegistrationToken>(((Application)this).remove_UnhandledException), 
            //    new UnhandledExceptionEventHandler(this.App_UnhandledException));
            Application.Current.UnhandledException += this.App_UnhandledException;


            Helper.Write((object)nameof(App), "App events registered");
            Helper.Write((object)nameof(App), "Registered app events");

            YouTube.GetFilterMethod = (Func<IHttpFilter>)(() =>
            {
                HttpBaseProtocolFilter baseProtocolFilter = new HttpBaseProtocolFilter();
                baseProtocolFilter.MaxConnectionsPerServer = 128U;
                baseProtocolFilter.AutomaticDecompression = true;
                baseProtocolFilter.UseProxy = false;
                return (IHttpFilter)baseProtocolFilter;
            });

            YouTube.DeveloperKey
             = "AI39si6dXEJmpguaJJhUrdMQhjP-MTkCM8Nj1SZrHlRdRP_jq25wIBzK5TAHpdyh7woySA6kOVjJ9r80uiL9jU4Gnnx3KE499w";
            YouTube.RedirectUri
             = "urn:ietf:wg:oauth:2.0:oob";
            YouTube.ClientID
                = "424014257505-067odu7bdq7gg0dj7tvm1jkp7vf8ng9i.apps.googleusercontent.com";
            YouTube.ClientSecret
         = "l0ExNytvMCAuN0QJ0MnsI0_m";

            List<string> stringList = new List<string>();

            stringList.Add("AIzaSyCRuvaqjnVtmh6FOnfIDQ8XyDVWzi_6UIA");
            stringList.Add("AIzaSyCdLTxn2Jbn7d_4qCQ3OEMklYTMyKNZBXU");
            stringList.Add("AIzaSyDl4-bxkoPfLfpA7yc3Rj67ue-1FtU_Vtg");
            stringList.Add("AIzaSyC_sKKS0i_p5DtIsUMsg3nVZ022mJF5pe8");
            stringList.Add("AIzaSyBnEvrWFEjePAkAi_8Jo6e6_mzwe9aPaqk");
            stringList.Add("AIzaSyAn7aiEcVjR7UCHawWq32CqyHpS_37PQHc");
            stringList.Add("AIzaSyDeuLOy0xPMh55gxBwbTqG3gYFdRH6nI2E");
            App.apiKeys = stringList;
            YouTube.APIKey = App.apiKeys[0];

            Helper.Write((object)nameof(App), (object)"Set up RykenTube constants");
            YouTube.init();
            TileHelper.Platform = App.PlatformType;
            this.themeCollection = new Dictionary<string, Dictionary<string, SolidColorBrush>>();
            Helper.Write((object)"App constructor completed");

            //

            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            
            Helper.Write((object)nameof(OnLaunched), (object)"Started");
            TaskScheduler.UnobservedTaskException += 
                new EventHandler<UnobservedTaskExceptionEventArgs>(this.TaskScheduler_UnobservedTaskException);
            this.launchArgs = e;
            App.launchTile = (TileArgs)null;
            
            if (!string.IsNullOrEmpty(e.Arguments) && e.Arguments != nameof(App))
            {
                App.launchTile = new TileArgs(e.Arguments);
                if (Type.GetType(App.launchTile.PageType) == null)
                    App.launchTile = (TileArgs)null;
                Helper.Write((object)nameof(OnLaunched), (object)("Launching with arguments " + e.Arguments));
            }
            
            this.initialSetup();

            Helper.Write((object)nameof(OnLaunched), (object)"Finished");
             

            Frame rootFrame = Window.Current.Content as Frame;

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // навигации
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }



        public static DeviceFamily DeviceFamily
        {
            get
            {
                return DeviceFamily.Mobile;
            }
        }

        public CustomFrame RootFrame => this.rootFrame;

        public static PlatformType PlatformType
        {
            get
            {
                return PlatformType.WindowsPhone;
            }
        }

        public static bool SupportsComposition => false;

        public event EventHandler<YouTubeEntry> ClipboardEntryFound;

        public static bool IsFullScreen
        {
            get => App.isFullScreen;
            set => App.isFullScreen = value;
        }

        public static YouTubeQuality HighestQuality
        {
            get
            {
                if (App.DeviceFamily != DeviceFamily.Mobile)
                    return YouTubeQuality.HD2160;
                DisplayInformation forCurrentView = DisplayInformation.GetForCurrentView();
                double num = Math.Min(Window.Current.Bounds.Width * forCurrentView.RawPixelsPerViewPixel,
                    Window.Current.Bounds.Height * forCurrentView.RawPixelsPerViewPixel);
                if (num >= 1540.0)
                    return YouTubeQuality.HD2160;
                if (num >= 1100.0)
                    return YouTubeQuality.HD1440;
                if (num >= 1080.0)
                    return YouTubeQuality.HD1080p60;
                return num >= 640.0 ? YouTubeQuality.HD1080 : YouTubeQuality.HD;
            }
        }

        public static TimeSpan StartTime => App.startTime;

        public static string DeviceFriendlyName => App.deviceInfo.FriendlyName;

        public static string DeviceManufacturer => App.deviceInfo.SystemManufacturer;

        public static string DeviceName => App.deviceInfo.SystemProductName;

        public static string FullDeviceName => App.DeviceFamily == DeviceFamily.Desktop 
            ? "Windows Device" 
            : App.DeviceManufacturer + " " + App.DeviceName;

        public static string OperatingSystem => App.deviceInfo.OperatingSystem;

        public static GlobalObjects GlobalObjects
        {
            get
            {
                if (App.gObjects == null)
                    App.gObjects = ((IDictionary<object, object>)
                        Application.Current.Resources)[(object)"GlobalAppObjects"] as GlobalObjects;
                return App.gObjects;
            }
        }

        public static Strings Strings
        {
            get
            {
                return ((IDictionary<object, object>)Application.Current.Resources)[(object)nameof(Strings)] as Strings;
            }
        }

        public static ElementTheme OppositeTheme
        {
            get
            {
                return ElementTheme.Dark;//Settings.Theme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark;
            }
        }

        public static ElementTheme Theme
        {
            get
            {
                return default;//Settings.Theme;
            }
        }

        public static string GetPassword(UserMode mode)
        {
            if (mode == UserMode.Beta)
                return "rykentubebeta";
            return mode == UserMode.Owner ? "rykentubeowner" : (string)null;
        }

        public static Rect VisibleBounds => ApplicationView.GetForCurrentView().VisibleBounds;

        public static Rect WindowBounds => Window.Current.Bounds;

        public static App Instance { get; private set; }

        public static string CultureName => GlobalizationPreferences.Languages[0];

        public static CultureInfo CurrentCulture
        {
            get
            {
                try
                {
                    return new CultureInfo(App.CultureName);
                }
                catch
                {
                    return CultureInfo.CurrentUICulture;
                }
            }
        }

        public static string LanguageName
        {
            get
            {
                string languageName = App.CurrentCulture.EnglishName;
                if (languageName.Contains("(") && languageName.IndexOf('(') != 0)
                    languageName = languageName.Substring(0, languageName.IndexOf('('));
                while (languageName[languageName.Length - 1] == ' ')
                    languageName = languageName.Substring(0, languageName.Length - 1);
                return languageName;
            }
        }

        public static bool IsTrial
        {
            get
            {
                try
                {
                    return false;//Settings.UserMode < UserMode.Owner && string.IsNullOrWhiteSpace(Settings.ProductKey) && !Settings.WasPaidFor && CurrentApp.LicenseInformation.IsTrial;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static ThumbnailDispatcher ThumbnailDispatcher => App.thumbnailDispatcher;

        public static TaskDispatcher TaskDispatcher => App.taskDispatcher;

        public static async Task Purchase()
        {
            string str = await CurrentApp.RequestAppPurchaseAsync(true);
        }


        private async Task createLogStream()
        {
            this.logWriter = new StreamWriter(await WindowsRuntimeStorageExtensions.OpenStreamForWriteAsync((IStorageFile)await ApplicationData.Current.LocalFolder.CreateFileAsync("log.txt", (CreationCollisionOption)1)))
            {
                AutoFlush = true
            };
            using (List<string>.Enumerator enumerator = Helper.Logs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    this.logWriter.WriteLine(enumerator.Current);
            }
        }

        private void Helper_Logged(object sender, string e)
        {
            if (this.logWriter == null)
                return;
            this.logWriter.WriteLine(e);
        }

        public static string GetAPIKey(int index)
        {
            if (index > App.apiKeys.Count - 1)
                index = App.apiKeys.Count - 1;
            if (index < 0)
                index = 0;
            return App.apiKeys[index];
        }

        public void InitialThemeSetup()
        {
            if (this.initialThemeSetup)
                return;
            this.initialThemeSetup = true;
            //this.AddThemeToDictionary(ColorSchemes.Default);
            string key = "PhoneAccentBrush";

            //RnD
            ResourceDictionary themeDictionary1 = default;//this.GetThemeDictionary("Classic");

            if (!((IDictionary<object, object>)themeDictionary1).ContainsKey((object)"AccentBrush")
                    && !((IDictionary<object, object>)themeDictionary1).ContainsKey((object)"MenuBackground"))
            {
                ((IDictionary<object, object>)themeDictionary1).Add((object)"AccentBrush",
                    ((IDictionary<object, object>)this.Resources)[(object)key]);
                ((IDictionary<object, object>)themeDictionary1).Add((object)"MenuBackground",
                    ((IDictionary<object, object>)this.Resources)[(object)key]);
            }

            //RnD
            ResourceDictionary themeDictionary2 = default;//this.GetThemeDictionary("Accent");
            if (themeDictionary2 != null
                    && !((IDictionary<object, object>)themeDictionary2).ContainsKey((object)"AccentBrush")
                    && !((IDictionary<object, object>)themeDictionary2).ContainsKey((object)"MenuForegroundBrush"))
            {
                ((IDictionary<object, object>)themeDictionary2).Add((object)"AccentBrush",
                    ((IDictionary<object, object>)this.Resources)[(object)key]);
                ((IDictionary<object, object>)themeDictionary2).Add((object)"MenuForegroundBrush",
                    ((IDictionary<object, object>)this.Resources)[(object)key]);
            }
            //this.AddThemeToDictionary(ColorSchemes.Accent);
            //this.AddThemeToDictionary(ColorSchemes.Classic);
            //this.AddThemeToDictionary(ColorSchemes.YouTube);
        }

        public void ApplyTheme(ColorSchemes scheme)
        {
            if (scheme == this.curentScheme)
                return;
            this.InitialThemeSetup();
            this.curentScheme = scheme;
            string name = scheme == ColorSchemes.Default ? "" : scheme.ToString();
            Dictionary<string, SolidColorBrush> customThemeDictionary1 = this.GetCustomThemeDictionary(name);
            this.ApplyTheme(this.GetCustomThemeDictionary("Light"), (ApplicationTheme)0);
            this.ApplyTheme(this.GetCustomThemeDictionary("Dark"), (ApplicationTheme)1);
            Dictionary<string, SolidColorBrush> customThemeDictionary2 = this.GetCustomThemeDictionary(name + (object)(ApplicationTheme)1);
            Dictionary<string, SolidColorBrush> customThemeDictionary3 = this.GetCustomThemeDictionary(name + (object)(ApplicationTheme)0);
            if (customThemeDictionary1 != null)
            {
                this.ApplyTheme(customThemeDictionary1, (ApplicationTheme)0);
                this.ApplyTheme(customThemeDictionary1, (ApplicationTheme)1);
            }
            if (customThemeDictionary2 != null)
                this.ApplyTheme(customThemeDictionary2, (ApplicationTheme)1);
            if (customThemeDictionary3 == null)
                return;
            this.ApplyTheme(customThemeDictionary3, (ApplicationTheme)0);
        }

        private Dictionary<string, SolidColorBrush> GetCustomThemeDictionary(string name) 
            => this.themeCollection.ContainsKey(name) 
                ? this.themeCollection[name] 
                : (Dictionary<string, SolidColorBrush>)null;

        private Dictionary<string, SolidColorBrush> GetCustomThemeDictionary(
          ColorSchemes scheme,
          ApplicationTheme theme)
        {
            string key = (scheme == ColorSchemes.Default 
                ? (object)"" 
                : (object)scheme.ToString()).ToString() + (object)theme;
            return this.themeCollection.ContainsKey(key) 
                ? this.themeCollection[key] 
                : (Dictionary<string, SolidColorBrush>)null;
        }

        private void ApplyTheme(Dictionary<string, SolidColorBrush> dict, ApplicationTheme theme)
        {
            foreach (KeyValuePair<string, SolidColorBrush> keyValuePair in dict)
            {
                string key = keyValuePair.Key;
                SolidColorBrush solidColorBrush = keyValuePair.Value;
                if (key != null && solidColorBrush != null)
                {
                    this.SetThemeBrushColor(key, theme, solidColorBrush.Color);
                    this.SetThemeBrushOpacity(key, theme, ((Brush)solidColorBrush).Opacity);
                }
            }
        }

        private void AddThemeToDictionary(ColorSchemes scheme)
        {
            /*if (this.AddedSchemes.Contains(scheme))
                return;
            this.AddedSchemes.Add(scheme);
            string name = scheme == ColorSchemes.Default ? "" : scheme.ToString();
            ResourceDictionary themeDictionary;
            ResourceDictionary dict1 = themeDictionary = this.GetThemeDictionary(name);
            if (dict1 != null)
                this.AddThemeToDictionary(name, dict1);
            ResourceDictionary dict2 = this.GetThemeDictionary(name + (object)(ApplicationTheme)1) ?? themeDictionary;
            if (dict2 != null)
                this.AddThemeToDictionary(name + (object)(ApplicationTheme)1, dict2);
            ResourceDictionary dict3 = this.GetThemeDictionary(name + (object)(ApplicationTheme)0) ?? themeDictionary;
            if (dict3 == null)
                return;
            this.AddThemeToDictionary(name + (object)(ApplicationTheme)0, dict3);*/
        }

        private void AddThemeToDictionary(string name, ResourceDictionary dict)
        {
            Dictionary<string, SolidColorBrush> dictionary1;
            if (this.themeCollection.ContainsKey(name))
            {
                dictionary1 = this.themeCollection[name];
            }
            else
            {
                dictionary1 = new Dictionary<string, SolidColorBrush>();
                this.themeCollection.Add(name, dictionary1);
            }
            foreach (KeyValuePair<object, object> keyValuePair 
                in (IEnumerable<KeyValuePair<object, object>>)dict)
            {
                if (keyValuePair.Value is SolidColorBrush solidColorBrush1)
                {
                    if (dictionary1.ContainsKey(keyValuePair.Key as string))
                    {
                        Dictionary<string, SolidColorBrush> dictionary2 = dictionary1;
                        string key = keyValuePair.Key as string;
                        SolidColorBrush solidColorBrush = new SolidColorBrush(solidColorBrush1.Color);
                        ((Brush)solidColorBrush).Opacity = ((Brush)solidColorBrush1).Opacity;
                        dictionary2[key] = solidColorBrush;
                    }
                    else
                    {
                        Dictionary<string, SolidColorBrush> dictionary3 = dictionary1;
                        string key = keyValuePair.Key as string;
                        SolidColorBrush solidColorBrush = new SolidColorBrush(solidColorBrush1.Color);
                        ((Brush)solidColorBrush).Opacity = ((Brush)solidColorBrush1).Opacity;
                        dictionary3.Add(key, solidColorBrush);
                    }
                }
            }
        }

        private async void App_Resuming(object sender, object e)
        {
            Helper.Write((object)nameof(App_Resuming), (object)"Resuming");
            /*App.CheckMessages(45.0);
            PlayerControls.UpdateBackgroundAudioState();
            if (PlayerControls.BackgroundAudio)
                DefaultPage.Current.VideoPlayer.RegisterBackgroundEvent();
            if (this.backgroundAudio)
            {
                this.backgroundAudio = false;
                DefaultPage.Current.VideoPlayer.ChangeQuality(this.backgroundQuality);
            }
            int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
            try
            {
                await App.GlobalObjects.History.Update();
            }
            catch
            {
            }
            */
        }

        private async void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            Exception exception = e.Exception;
            e.Handled = true;
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            var cDisplayClass910 = new App.DisplayClass91_0();
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.u003E4 = this;
            try
            {
                if (Settings.UserMode != UserMode.Owner)
                {
                    using (List<Exception>.Enumerator enumerator = this.exceptions.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Exception current = enumerator.Current;
                            if (exception.GetType() == current.GetType() 
                                && exception.StackTrace == current.StackTrace)
                                return;
                        }
                    }
                }
                this.exceptions.Add(exception);
            }
            catch
            {
            }
            if (this.globalObjects == null)
                return;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.data = new ExceptionData(exception);
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.data.Versions.Add(this.globalObjects.Version.ToString());
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.data.Devices.Add(App.FullDeviceName);
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.data.EventHandlerMessage = e.Message;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.data.CausedCrash = true;
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.data.AppName = "myTube";
            // ISSUE: reference to a compiler-generated field
            Settings.UnhandledException = DataObject.ToJson((object)cDisplayClass910.data);
            // ISSUE: reference to a compiler-generated field
            cDisplayClass910.exceptionMessage = 
                "Oops, looks like we've run into an internal error the developer hasn't looked out for.\n\n" +
                "Would you like to report it?\n\n" +
                "Please include any information about how you came across this error in the report.";
            if (Settings.UserMode >= UserMode.Owner)
            {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                cDisplayClass910.exceptionMessage = cDisplayClass910.exceptionMessage + "\n\n" + exception.ToString();
            }
          // ISSUE: method pointer
          //((DependencyObject)this.RootFrame).Dispatcher.RunAsync((CoreDispatcherPriority)0, new DispatchedHandler((object)cDisplayClass910, __methodptr(\u003CApp_UnhandledException\u003Eb__0)));
        }

        private void YouTube_SignedOut(object sender, EventArgs e)
        {
            if (((IDictionary<string, object>)ApplicationData.Current.RoamingSettings.Values).ContainsKey("refreshToken"))
                ((IDictionary<string, object>)ApplicationData.Current.RoamingSettings.Values).Remove("refreshToken");
            SharedSettings.CurrentAccount = (SignInInfo)null;
            Settings.Save();
        }

        public static async Task CheckSignIn(double minutes, bool callAwait = true)
        {
            if (!YouTube.Initialized || YouTube.CurrentlySigningIn || DateTime.Now - App.signedInAt < TimeSpan.FromMinutes(minutes) || SharedSettings.CurrentAccount == null)
                return;
            if (App.trySignIn)
            {
                App.trySignIn = false;
                Helper.Write((object)"Signing back into YouTube as the time period has passed");
                if (callAwait)
                {
                    try
                    {
                        UserInfo userInfo = await YouTube.RefreshSignIn(SharedSettings.CurrentAccount.RefreshToken, SharedSettings.CurrentAccount.UserID);
                    }
                    catch (Exception ex)
                    {
                        Helper.Write((object)nameof(App), (object)("Sign in exception: " + (object)ex));
                    }
                    App.trySignIn = true;
                }
                else
                {
                    try
                    {
                        YouTube.RefreshSignIn(SharedSettings.CurrentAccount.RefreshToken, SharedSettings.CurrentAccount.UserID);
                    }
                    catch
                    {
                    }
                }
            }
            else
                Helper.Write((object)"Will not attempt tp sign in, as the app is already attempting to sign in");
        }

        private async void YouTube_SignedIn(object sender, EventArgs e)
        {
            App.trySignIn = true;
            Helper.Write((object)"Signed in");
            App.signedInAt = DateTime.Now;
            ((IDictionary<string, object>)ApplicationData.Current.RoamingSettings.Values)["refreshToken"] = (object)YouTube.RefreshToken;
            SharedSettings.CurrentAccount = YouTube.UserInfo == null || !SharedSettings.Accounts.HasAccount(YouTube.UserInfo.ID) || !YouTube.WasRefreshSignIn ? SharedSettings.Accounts.AddAccount(YouTube.UserInfo, YouTube.RefreshToken, YouTube.Scope) : SharedSettings.Accounts.AddAccount(YouTube.UserInfo, YouTube.RefreshToken, (string)null);
            Settings.Save();
            YouTube.GetSubscriptions();
        }

        public static async Task<string> UpdateCipher(double minutes = 10.0)
        {
            if (DateTime.Now - App.cipherTime < TimeSpan.FromMinutes(minutes))
            {
                Helper.Write((object)nameof(UpdateCipher), (object)"Too soon to update the cipher");
                return "Too soon";
            }
            try
            {
                return await Task.Run<string>((Func<Task<string>>)(async () =>
                {
                    CipherLoader ci = new CipherLoader();
                    FunctionAndVariable cipherFunction = await ci.GetCipherFunction("CevxZvSJLk8");
                    string cipherAlgorithm = ci.GetCipherAlgorithm(cipherFunction);
                    App.cipherTime = DateTime.Now;
                    YouTube.DecipherAlgorithm = cipherAlgorithm;
                    Settings.Cipher = YouTube.DecipherAlgorithm;
                    return YouTube.DecipherAlgorithm;
                }));
            }
            catch (Exception ex)
            {
                Helper.Write((object)("Error getting cipher: \n" + (object)ex));
            }
            HttpClient cl = new HttpClient();
            try
            {
                YouTube.DecipherAlgorithm = await cl.GetStringAsync("http://rykenapps.com/UploadService/myTube/newcipher.txt?randtime=" + (object)DateTime.Now.Ticks);
                App.cipherTime = DateTime.Now;
                Settings.Cipher = YouTube.DecipherAlgorithm;
                cl.Dispose();
                return Settings.Cipher;
            }
            catch (Exception ex)
            {
                Helper.Write((object)("Error getting cipher: \n" + (object)ex));
                return "Unable to get cipher";
            }
            finally
            {
                cl.Dispose();
            }
        }

        public static async Task CheckMessages(double minutesSinzeLastCheck)
        {
            if (DateTime.Now - App.lastCheckedMessaged < TimeSpan.FromMinutes(minutesSinzeLastCheck))
                return;
            try
            {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                var displayClass1020 = new App.DisplayClass102_0();
                App.lastCheckedMessaged = DateTime.Now;
                Helper.Write((object)"Checking for messages");
                // ISSUE: reference to a compiler-generated field
                displayClass1020.messCli = new MessageClient();
                // ISSUE: method pointer
                //await ((DependencyObject)App.Instance.RootFrame).Dispatcher.RunAsync((CoreDispatcherPriority)0, new DispatchedHandler((object)displayClass1020, __methodptr(\u003CCheckMessages\u003Eb__0)));
            }
            catch
            {
            }
        }

        public static async Task ShowMessage(Message message) => await App.showMessageInternal(message);

        private static async Task showMessageInternal(Message message)
        {
            MessageView messageView = new MessageView();
            ((FrameworkElement)messageView).DataContext = (object)message;
            ((Control)messageView).VerticalContentAlignment = (VerticalAlignment)0;
            MessageView Element1 = messageView;
            (Element1.Content as FrameworkElement).MinHeight = (double)byte.MaxValue;
            Rect bounds1 = Window.Current.Bounds;
            Rect bounds2 = ((FrameworkElement)App.Instance.RootFrame).GetBounds(Window.Current.Content);
            ((FrameworkElement)Element1).Width = Math.Min(bounds1.Width, 600.0);
            if (((FrameworkElement)Element1).Width > bounds1.Width - 200.0)
                ((FrameworkElement)Element1).Width = bounds1.Width;
            ((FrameworkElement)Element1).Height = Math.Min(bounds2.Bottom, 700.0);
            Point position = new Point();

            position.X = ((FrameworkElement)Element1).Width < bounds1.Width 
                ? bounds1.Width - ((FrameworkElement)Element1).Width - 76.0 
                : 0.0;

            Popup popup1 = new Popup();
            popup1.Child = ((UIElement)Element1);
            ((FrameworkElement)popup1).RequestedTheme = App.Theme;
            Popup popup2 = popup1;
            PlaneProjection Element2 = new PlaneProjection();
            Element2.RotationX = -45.0;
            Element1.Content.Projection = (Projection)Element2;
            Storyboard closeAnimation = Ani.Animation(Ani.DoubleAni((DependencyObject)Element1, 
                "Opacity", 0.0, 0.1, (EasingFunctionBase)null, 0.0));

            //Task<bool> task = DefaultPage.Current.ShowPopup(popup2, position, new Point(0.0, 0.0),
            //    lightDismissed: false, closeAnimation: closeAnimation);

            Ani.Begin((DependencyObject)Element2, "RotationX", 0.0, 0.35, 
                (EasingFunctionBase)Ani.Ease((EasingMode)0, 6.0));

            Helper.Write((object)"  -Awaiting popup");
            int num = 0;//await task ? 1 : 0;
            Helper.Write((object)"      -Popup awaited");
        }

        public static async void SendEmail(string emailAddress, string subject, string content)
        {
            content = content.Replace("\n", WebUtility.UrlEncode(Environment.NewLine));
            int num = await Launcher.LaunchUriAsync(new Uri("mailto:" + emailAddress 
                + "?subject=" + subject + "&body=" + content), new LauncherOptions()) ? 1 
                : 0;
        }

        public static void SendSupportEmail(string subject, string content)
        {
            subject = subject + " [" + (object)App.PlatformType + (Settings.UserMode > UserMode.Normal ? (object)(" " + (object)Settings.UserMode) : (object)"") + " " + (object)App.DeviceFamily + "]";
            subject = subject + " (" + (object)App.GlobalObjects.Version + ")";
            content += "\n\n=============\n";
            content = content + "Device info: " + Environment.NewLine + App.DeviceManufacturer + " " + App.DeviceName + "\n" + App.OperatingSystem;
            App.SendEmail("rykenproductions@outlook.com", subject, content);
        }

        //private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        //{
        //    e.put_Handled(this.GoBack());
        //}

        public bool GoBack()
        {
            if (this.rootFrame == null || !this.rootFrame.CanGoBack)
                return false;
            this.rootFrame.GoBack();
            return true;
        }

        public Task<bool> WindowActivatedTask => this.windowActivatedTask.Task;

        private async Task initialSetup()
        {
            string Tag = nameof(initialSetup);
            Helper.Write((object)Tag, (object)"Initial setup");
            this.globalObjects = App.GlobalObjects;
            YouTubeEntry.DefaultThumbnailQuality = ThumbnailQuality.SD;
            long num = (long)(MemoryManager.AppMemoryUsageLimit / 1048576UL);
            if ((ulong)num < 500UL)
                YouTubeEntry.DefaultThumbnailQuality = ThumbnailQuality.High;
            if ((ulong)num < 200UL)
                YouTubeEntry.DefaultThumbnailQuality = ThumbnailQuality.Med;
            if (!Settings.WasPaidFor && !App.IsTrial)
                Settings.WasPaidFor = true;
            YouTube.CacheHandler = (ICacheHandler)new YouTubeCacheHandler();
            this.ApplyTheme(Settings.ColorCheme);
            if (Settings.UserMode == UserMode.Beta)
                Settings.AutoShowDevMessages = true;
            try
            {
                await Task.Run((Func<Task>)(async () => await this.SetUpBackgroundTask(App.GlobalObjects)));
            }
            catch (Exception ex)
            {
                Helper.Write((object)Tag, (object)("Exception setting up background tasks: \n" + ex.ToString()));
            }
            // ISSUE: method pointer
            //ThreadPool.RunAsync(new WorkItemHandler((object)this, __methodptr(\u003CinitialSetup\u003Eb__112_1)));
            Helper.Write((object)"InitialSetup", (object)"Starting");
            if (this.rootFrame == null)
            {
                //await StatusBar.GetForCurrentView().HideAsync();
                this.windowActivatedTask = new TaskCompletionSource<bool>();
                await App.Strings.LoadXML(App.CurrentCulture.TwoLetterISOLanguageName + ".xml");
                DefaultPage defaultPage = new DefaultPage();
                Window.Current.Content = ((UIElement)defaultPage);
                Window.Current.Activate();

                Helper.Write((object)"InitialSetup", (object)"Created DefaultPage");
                this.rootFrame = defaultPage.Frame;
                this.rootFrame.CacheSize = 4;
                CustomFrame rootFrame = this.rootFrame;

                //WindowsRuntimeMarshal.AddEventHandler<NavigatedEventHandler>(
                //    new Func<NavigatedEventHandler, EventRegistrationToken>(((Frame)rootFrame).add_Navigated), 
                //    new Action<EventRegistrationToken>(((Frame)rootFrame).remove_Navigated), 
                //    new NavigatedEventHandler(this.RootFrame_FirstNavigated));

                Helper.Write((object)"InitialSetup", (object)"Completed window setup");
                Window current1 = Window.Current;
                
                //WindowsRuntimeMarshal.AddEventHandler<WindowActivatedEventHandler>(
                //    new Func<WindowActivatedEventHandler, EventRegistrationToken>(current1.add_Activated), 
                //    new Action<EventRegistrationToken>(current1.remove_Activated), 
                //    new WindowActivatedEventHandler(this.Current_Activated));
                current1.Activated += this.Current_Activated;
                current1.Activated -= this.Current_Activated;

                Window current2 = Window.Current;
                //WindowsRuntimeMarshal.AddEventHandler<WindowClosedEventHandler>(
                //    new Func<WindowClosedEventHandler, EventRegistrationToken>(current2.add_Closed), 
                //    new Action<EventRegistrationToken>(current2.remove_Closed), 
                //    new WindowClosedEventHandler(this.Current_Closed));

                current2.Closed += this.Current_Closed;
                current2.Closed -= this.Current_Closed;
            }
            bool loadOrigPage = true;
            if (App.launchTile == null && this.launchArgs != null && !this.initialized)
            {
                if (this.launchArgs != null 
                    && this.launchArgs.PreviousExecutionState == ApplicationExecutionState.Terminated 
                    && ((ContentControl)this.rootFrame).Content == null 
                    && DateTimeOffset.Now - Settings.SuspendedAt < TimeSpan.FromHours(3.0))
                {
                    try
                    {
                        App app = this;
                        PageInfoCollection pageInfoCollection1 = app.pageInfoCollection;
                        PageInfoCollection pageInfoCollection2 = await Settings.GetPageInfoCollection();
                        PageInfoCollection pageInfoCollection3 = app.pageInfoCollection = pageInfoCollection2;
                        app = (App)null;
                        PageInfo[] pages = pageInfoCollection3.Pages;
                        if (pages.Length > 1)
                        {
                            PageInfo pageInfo = pages[pages.Length - 1];
                            this.rootFrame.Navigate(pageInfo.PageType, pageInfo.Parameter);
                            loadOrigPage = false;
                        }
                    }
                    catch
                    {
                        loadOrigPage = true;
                        if (Debugger.IsAttached)
                            Debugger.Break();
                    }
                }
                Helper.Write((object)"InitialSetup", (object)"Finished default setup");
            }
            if (this.activationArgs != null)
            {
                if (this.activationArgs.Kind == ActivationKind.Protocol)
                {
                    Helper.Write((object)"Launching from protocol activation");
                    ProtocolActivatedEventArgs activationArgs = this.activationArgs as ProtocolActivatedEventArgs;
                    Uri uri = activationArgs.Uri;
                    string originalString = activationArgs.Uri.OriginalString;
                    URLConstructor urlConstructor = new URLConstructor(uri);
                    if (originalString.IndexOf("mytube:link=") == 0)
                    {
                        YouTubeURLInfo urlType = YouTubeURLHelper.GetUrlType(originalString.Replace("mytube:link=", ""));
                        loadOrigPage = false;
                        switch (urlType.Type)
                        {
                            case YouTubeURLType.Video:
                                //this.rootFrame.Navigate(typeof(VideoPage), (object)urlType.ID);
                                break;
                            case YouTubeURLType.Playlist:
                                //this.rootFrame.Navigate(typeof(PlaylistPage), (object)urlType.ID);
                                break;
                            case YouTubeURLType.Channel:
                                //this.rootFrame.Navigate(typeof(ChannelPage), (object)urlType.ID);
                                break;
                            default:
                                loadOrigPage = true;
                                break;
                        }
                        if (!loadOrigPage)
                        {
                            if (((ContentControl)this.rootFrame).Content != null)
                                this.rootFrame.ClearBackStackAtNavigate();
                            this.pageInfoCollection = new PageInfoCollection();
                            //this.pageInfoCollection.AddPage(new PageInfo(typeof(HomePage), (object)null));
                        }
                    }
                    else if (originalString.IndexOf("mytube:videoID=") == 0)
                    {
                        //this.rootFrame.Navigate(typeof(VideoPage), (object)originalString.Replace("mytube:videoID=", ""));
                        loadOrigPage = false;
                    }
                    else if (urlConstructor.BaseAddress.Contains("Video") && urlConstructor.ContainsKey("ID"))
                    {
                        loadOrigPage = false;
                        this.rootFrame.ClearBackStackAtNavigate();
                        //this.rootFrame.AddToBackStackAtNavigate(typeof(HomePage), (object)null);
                        //this.rootFrame.Navigate(typeof(VideoPage), (object)urlConstructor["ID"]);
                    }
                    else if (urlConstructor.BaseAddress.Contains("Search") && urlConstructor.ContainsKey("Term"))
                    {
                        loadOrigPage = false;
                        this.rootFrame.ClearBackStackAtNavigate();
                        //this.rootFrame.AddToBackStackAtNavigate(typeof(HomePage), (object)null);
                        //this.rootFrame.Navigate(typeof(SearchPage), (object)urlConstructor["Term"]);
                    }
                    else if (urlConstructor.BaseAddress.Contains("vnd.youtube:"))
                    {
                        string str = urlConstructor.BaseAddress.Substring(urlConstructor.BaseAddress.IndexOf(':') + 1);
                        YouTubeURLInfo urlType = YouTubeURLHelper.GetUrlType(str);
                        loadOrigPage = false;
                        this.rootFrame.ClearBackStackAtNavigate();
                        //this.rootFrame.AddToBackStackAtNavigate(typeof(HomePage), (object)null);
                        switch (urlType.Type)
                        {
                            case YouTubeURLType.None:
                                //this.rootFrame.Navigate(typeof(VideoPage), (object)str);
                                break;
                            case YouTubeURLType.Video:
                                //this.rootFrame.Navigate(typeof(VideoPage), (object)urlType.ID);
                                break;
                            case YouTubeURLType.Playlist:
                                //this.rootFrame.Navigate(typeof(PlaylistPage), (object)urlType.ID);
                                break;
                            case YouTubeURLType.Channel:
                                //this.rootFrame.Navigate(typeof(ChannelPage), (object)urlType.ID);
                                break;
                        }
                    }
                    else
                        loadOrigPage = true;
                }
                else if (this.activationArgs.Kind == ActivationKind.PickFileContinuation)
                {
                    FileOpenPickerContinuationEventArgs activationArgs 
                        = this.activationArgs as FileOpenPickerContinuationEventArgs;
                    loadOrigPage = false;
                    //if (this.rootFrame.CurrentSourcePageType == typeof(UploaderPage))
                    //    (((ContentControl)this.rootFrame).Content as UploaderPage).SelectFile(
                    //        (IStorageFile)Enumerable.FirstOrDefault<StorageFile>(
                    //            (IEnumerable<StorageFile>)activationArgs.Files));
                    //else
                    //    this.rootFrame.Navigate(typeof(UploaderPage), (object)Enumerable.FirstOrDefault<StorageFile>((IEnumerable<StorageFile>)activationArgs.Files));
                }
                else if (this.activationArgs.Kind == ActivationKind.ShareTarget)
                {
                    ShareTargetActivatedEventArgs activationArgs = this.activationArgs as ShareTargetActivatedEventArgs;
                    if (activationArgs.ShareOperation.Data.Contains(StandardDataFormats.StorageItems))
                    {
                        foreach (IStorageItem istorageItem in (IEnumerable<IStorageItem>)await activationArgs.ShareOperation.Data.GetStorageItemsAsync())
                        {
                            if (istorageItem is IStorageFile)
                            {
                                loadOrigPage = false;
                                //if (this.rootFrame.CurrentSourcePageType == typeof(UploaderPage))
                                //{
                                //    (((ContentControl)this.rootFrame).Content as UploaderPage).SelectFile(istorageItem as IStorageFile);
                                //}
                                //else
                                //{
                                //    this.rootFrame.Navigate(typeof(UploaderPage), (object)istorageItem);
                                //    break;
                                //}
                            }
                        }
                    }
                }
                Helper.Write((object)"InitialSetup", (object)"Finished activation args setup");
            }
            if (((ContentControl)this.rootFrame).Content == null & loadOrigPage && !this.initialized && App.launchTile == null)
            {
                Helper.Write((object)"InitialSetup", (object)"Creating default page");
                //this.rootFrame.Navigate(typeof(HomePage));
                Helper.Write((object)"InitialSetup", (object)"Navigated to homepage");
            }
            else if (App.launchTile != null)
            {
                this.rootFrame.ClearBackStackAtNavigate();
                //if (Type.GetType(App.launchTile.PageType) != typeof(HomePage))
                //    this.rootFrame.AddToBackStackAtNavigate(typeof(HomePage), (object)null);
                this.rootFrame.Navigate(Type.GetType(App.launchTile.PageType), (object)App.launchTile.Param);
                Helper.Write((object)"InitialSetup", (object)"Navigated to launchTile page");
            }
            SignInInfo currentAccount = SharedSettings.CurrentAccount;
            this.initialized = true;
            ++Settings.Runs;
            Helper.Write((object)"InitialSetup", (object)"Completed");
            TileData.Clean();
            await Task.Delay(500);
            if (Settings.UnhandledException == null)
                return;
            ExceptionData exception = DataObject.ToObject<ExceptionData>(Settings.UnhandledException);
            Settings.UnhandledException = (string)null;
            try
            {
                if (!await new ExceptionClient().AddException(exception, 
                    App.FullDeviceName, this.globalObjects.Version))
                    return;

                MessageDialog messageDialog = new MessageDialog(
                    "Looks like we ran into an issue the last time you used myTube. " +
                    "The details of the error have been sent to the developer.", "Uh-oh");
            }
            catch
            {
            }
        }

        private void YouTube_Logged(object sender, string e) => Helper.Write((object)e);

        private void Current_Closed(object sender, CoreWindowEventArgs e)
        {
        }

        //ToDo: Popup

        private async void YouTube_ErrorReported(YouTubeError e)
        {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            var displayClass1150 = new App.DisplayClass115_0();
            // ISSUE: reference to a compiler-generated field
            displayClass1150.u003E4 = this;
            // ISSUE: reference to a compiler-generated field
            displayClass1150.e = e;
            if (Settings.UserMode < UserMode.Owner || Window.Current == null
                || Window.Current.Content == null)
                return;
            // ISSUE: method pointer
            //await ((DependencyObject)Window.Current.Content).Dispatcher.RunAsync(
            //    (CoreDispatcherPriority)(- 1), new DispatchedHandler((object)displayClass1150, 
            //    __methodptr(\u003CYouTube_ErrorReported\u003Eb__0)));
        }

        private string writeOutDict(Dictionary<string, object> dict)
        {
            string str = "";
            using (Dictionary<string, object>.Enumerator enumerator = dict.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, object> current = enumerator.Current;
                    str = str + current.Key + ": " + current.Value + "\n";
                }
            }
            return str;
        }

        private void Test_Logged(object sender, TestLoggedEventArgs e) => Helper.Write((object)"Text",
            (object)e.Text);

        private async void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            Helper.Write((object)"Window", (object)("Activated! (" + (object)e.WindowActivationState + ")"));
            if (!this.alreadyActivated)
            {
                if (this.windowActivatedTask != null)
                    this.windowActivatedTask.TrySetResult(true);
                this.InitialChecks();
                this.alreadyActivated = true;
                App.startTime = Helper.EndTimer();
                if (Settings.UserMode >= UserMode.Owner)
                {
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Foreground = (Brush)new SolidColorBrush(Colors.White);
                    textBlock1.Text = "start time: " + (object)App.startTime.TotalSeconds + "s";
                    textBlock1.FontSize = 17.0;
                    TextBlock textBlock2 = textBlock1;
                    Border border1 = new Border();
                    border1.Background = (Brush)new SolidColorBrush(Colors.Black);
                    border1.Child = (UIElement)textBlock2;
                    Border border2 = border1;
                    Popup popup = new Popup();
                    popup.Child = (UIElement)border2;
                    Popup p = popup;
                    p.IsOpen = true;
                    await Task.Delay(2000);
                    p.IsOpen = false;
                    p = (Popup)null;
                }
            }
            CoreWindowActivationState windowActivationState = e.WindowActivationState;
        }

        private async Task InitialChecks()
        { 
            //TODO
            //ThreadPool.RunAsync(new WorkItemHandler((object)new App.DisplayClass121_0()
            //{
            //    gobj = App.GlobalObjects,
            //    strings = App.Strings
            //}, __methodptr(\u003CInitialChecks\u003Eb__0)));
        }


        private void YouTube_SignInFailed(object sender, SignedInFailedEventArgs e) => App.trySignIn = true;

        protected override void OnActivated(IActivatedEventArgs args)
        {
            Helper.Write((object)nameof(OnActivated), (object)"Started");
            base.OnActivated(args);
            bool flag = true;
            if (args is ProtocolActivatedEventArgs)
            {
                ProtocolActivatedEventArgs activatedEventArgs = args as ProtocolActivatedEventArgs;
                Helper.Write((object)nameof(OnActivated), (object)("Protocol, " + (object)activatedEventArgs.Uri));
                URLConstructor url = new URLConstructor(activatedEventArgs.Uri);
                if (url.BaseAddress.ToLower().Contains("personalassistant") && url.ContainsKey("LaunchContext"))
                {
                    Helper.Write((object)nameof(OnActivated), (object)"Launched from voice command");
                    string urlConstructor = url["LaunchContext"];
                    if (urlConstructor.StartsWith("TileArgs"))
                    {
                        App.launchTile = new TileArgs(urlConstructor);
                        Helper.Write((object)nameof(OnActivated), (object)("TileArgs from voice command: " + App.launchTile.ToString()));
                        flag = false;
                    }
                }
                else if (url.BaseAddress.ToLower().Contains("tileargs"))
                {
                    TileArgs tileArgs = new TileArgs(url);
                    flag = false;
                    App.launchTile = tileArgs;
                }
            }
            if (flag)
                this.activationArgs = args;
            this.initialSetup();
            ActivationKind kind = args.Kind;
        }

        public static TileArgs GetLaunchTileArgs(object key)
        {
            if (App.launchTile == null || Type.GetType(App.launchTile.PageType) != key.GetType())
                return (TileArgs)null;
            TileArgs launchTile = App.launchTile;
            App.launchTile = (TileArgs)null;
            return launchTile;
        }

        public static void DisposeLaunchArgumants()
        {
            Helper.Write((object)nameof(App), (object)"Launch arguments disposed of");
            App.launchTile = (TileArgs)null;
        }


        private void TaskScheduler_UnobservedTaskException(
          object sender,
          UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            foreach (Exception innerException in e.Exception.InnerExceptions)
            {
                // ISSUE: object of a compiler-generated type is created
                // ISSUE: variable of a compiler-generated type
                var displayClass1300 = new App.DisplayClass130_0();
                // ISSUE: reference to a compiler-generated field
                displayClass1300.u003E4 = this;
                try
                {
                    if (Settings.UserMode != UserMode.Owner)
                    {
                        using (List<Exception>.Enumerator enumerator = this.exceptions.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Exception current = enumerator.Current;
                                if (innerException.GetType() == current.GetType() 
                                    && innerException.StackTrace == current.StackTrace)
                                    return;
                            }
                        }
                    }
                    this.exceptions.Add(innerException);
                }
                catch
                {
                }
                // ISSUE: reference to a compiler-generated field
                displayClass1300.data = new ExceptionData(innerException);
                // ISSUE: reference to a compiler-generated field
                displayClass1300.data.Versions.Add(this.globalObjects.Version.ToString());
                // ISSUE: reference to a compiler-generated field
                displayClass1300.data.Devices.Add(App.FullDeviceName);
                // ISSUE: reference to a compiler-generated field
                displayClass1300.data.EventHandlerMessage = "From task scheduler, unobserved exception";
                // ISSUE: reference to a compiler-generated field
                displayClass1300.data.CausedCrash = true;
                // ISSUE: reference to a compiler-generated field
                displayClass1300.data.AppName = "myTube";

                // ISSUE: reference to a compiler-generated field
                Settings.UnhandledException = DataObject.ToJson((object)displayClass1300.data);
                string str1 = "Note: You are seeing this message because you are a beta tester\n\n" +
                    "Oops, looks like we've run into an internal error the developer hasn't looked out for.\n\n" +
                    "Would you like to report it?\n\n" +
                    "Please include any information about how you came across this error in the report.";
                if (Settings.UserMode >= UserMode.Owner)
                {
                    string str2 = str1 + "\n\n" + innerException.ToString();
                }
              // ISSUE: method pointer
              //((DependencyObject)this.RootFrame).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
              //    new DispatchedHandler((object)displayClass1300,
              //    __methodptr(\u003CTaskScheduler_UnobservedTaskException\u003Eb__0)));
            }
        }

        public async Task SetUpBackgroundTask(GlobalObjects obj = null)
        {
            string Tag = nameof(SetUpBackgroundTask);
            Helper.Write((object)Tag, (object)"Setting up background tasks");
            try
            {
                try
                {
                    if (obj == null)
                    {
                        Helper.Write((object)Tag, (object)"Getting GlobalObjects");
                        obj = App.GlobalObjects;
                    }
                    if (Settings.Version != obj.Version)
                    {
                        Helper.Write((object)Tag, (object)"App has been updated, performing necessary changes");
                        BackgroundExecutionManager.RemoveAccess();
                        Settings.UseNavigatePage = false;
                        YouTube.HTTPS = true;
                        if (Settings.Version == new Version(2, 7, 13, 0) || Settings.Version == new Version(2, 7, 0, 13))
                        {
                            MultipleSignInContainer accounts = SharedSettings.Accounts;
                            accounts.Clear();
                            SharedSettings.Accounts = accounts;
                            SharedSettings.CurrentAccount = (SignInInfo)null;
                        }
                        Settings.Version = obj.Version;
                    }
                }
                catch
                {
                }
                Helper.Write((object)Tag, (object)"Listing tiles");
                IReadOnlyList<SecondaryTile> tiles = await SecondaryTile.FindAllAsync();
                Helper.Write((object)Tag, (object)"Listing all registered background tasks");
                IReadOnlyDictionary<Guid, IBackgroundTaskRegistration> allTasks = BackgroundTaskRegistration.AllTasks;
                if (allTasks != null)
                {
                    try
                    {
                        foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> keyValuePair in (IEnumerable<KeyValuePair<Guid, IBackgroundTaskRegistration>>)allTasks)
                        {
                            try
                            {
                                if (keyValuePair.Value.Name.StartsWith("Typed"))
                                {
                                    bool flag = true;
                                    foreach (SecondaryTile secondaryTile in (IEnumerable<SecondaryTile>)tiles)
                                    {
                                        if (secondaryTile.TileId == keyValuePair.Value.Name)
                                        {
                                            flag = false;
                                            break;
                                        }
                                    }
                                    if (flag)
                                    {
                                        Helper.Write((object)Tag, (object)"Unregistering tile task");
                                        keyValuePair.Value.Unregister(true);
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                BackgroundAccessStatus backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
                TimeTrigger trigger1 = new TimeTrigger(30U, false);
                TimeTrigger trigger2 = new TimeTrigger(30U, false);
                foreach (SecondaryTile secondaryTile in (IEnumerable<SecondaryTile>)tiles)
                {
                    if (secondaryTile.TileId.StartsWith("Typed"))
                        App.RegisterBackgroundTask("WindowsPhoneBackgroundTask.TileUpdateTask", secondaryTile.TileId, (IBackgroundTrigger)trigger1, (IBackgroundCondition)null);
                }
                Helper.Write((object)Tag, (object)"Registering channel notifications task");
                App.RegisterBackgroundTask("ChannelNotificationsTask.RegularTask", "NotificationTask", (IBackgroundTrigger)trigger2, (IBackgroundCondition)null);
                App.UnregisterBackgroundTask("RegularTask");
                Helper.Write((object)Tag, (object)"Background tasks set up");
                tiles = (IReadOnlyList<SecondaryTile>)null;
            }
            catch (Exception ex)
            {
                Helper.Write((object)("Exception setting up background task:\n\n " + (object)ex));
            }
        }

        public static void UnregisterBackgroundTask(string name)
        {
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> allTask in (IEnumerable<KeyValuePair<Guid, IBackgroundTaskRegistration>>)BackgroundTaskRegistration.AllTasks)
            {
                if (allTask.Value.Name == name)
                {
                    allTask.Value.Unregister(true);
                    break;
                }
            }
        }

        public static BackgroundTaskRegistration RegisterBackgroundTask(
          string entryPoint,
          string name,
          IBackgroundTrigger trigger,
          IBackgroundCondition condition)
        {
            foreach (KeyValuePair<Guid, IBackgroundTaskRegistration> allTask 
                in (IEnumerable<KeyValuePair<Guid, IBackgroundTaskRegistration>>)
                BackgroundTaskRegistration.AllTasks)
            {
                if (allTask.Value.Name == name)
                    return (BackgroundTaskRegistration)allTask.Value;
            }
            BackgroundTaskBuilder backgroundTaskBuilder = new BackgroundTaskBuilder();
            backgroundTaskBuilder.TaskEntryPoint = entryPoint;
            backgroundTaskBuilder.Name = name;
            backgroundTaskBuilder.SetTrigger(trigger);
            if (condition != null)
                backgroundTaskBuilder.AddCondition(condition);
            return backgroundTaskBuilder.Register();
        }

        public static async Task CheckProductKey()
        {
            if (Settings.ProductKey != null || Settings.ProductKeyRequestId == null 
                || Settings.RykenUserID == null)
                return;
            ProductKeyClient productKeyClient = new ProductKeyClient();
            try
            {
                ProductKey productKey = await productKeyClient.GetProductKey(Settings.ProductKeyRequestId, Settings.RykenUserID);
                if (productKey.Key == null)
                    return;
                Settings.ProductKey = productKey.Key;
                MessageDialog messageDialog = new MessageDialog(
                    "Your product key request has been approve, and this copy of the app has been activated.", 
                    "Product key request approved");
            }
            catch
            {
            }
        }

        public static async Task CheckRykenUser()
        {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            var displayClass1350 = new App.DisplayClass135_0();
           
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method pointer
            //await ((DependencyObject)App.Instance.RootFrame).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
            //    App.u003E9__135_0 ?? (App.u003E9__135_0 
            //    = new DispatchedHandler((object)App.u003E9, __methodptr(CCheckRykenUser.u003Eb__135))));
            
            // ISSUE: reference to a compiler-generated field
            displayClass1350.userClient = new RykenUserClient();
            bool flag = false;
            if (Settings.RykenUserID != null)
            {
                try
                {
                    // ISSUE: object of a compiler-generated type is created
                    // ISSUE: variable of a compiler-generated type
                    var displayClass1351 = new App.DisplayClass135_1();
                    // ISSUE: reference to a compiler-generated field
                    displayClass1351.locals1 = displayClass1350;
                    // ISSUE: reference to a compiler-generated field
                    RykenUser user1 = displayClass1351.user;
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    RykenUser rykenUser1 = await displayClass1351.locals1.userClient.Login(Settings.RykenUserID);
                    // ISSUE: reference to a compiler-generated field
                    displayClass1351.user = rykenUser1;
                    // ISSUE: reference to a compiler-generated field
                    if (displayClass1351.user != null)
                    {
                        // ISSUE: reference to a compiler-generated field
                        int userMode = (int)displayClass1351.user.UserMode;
                        // ISSUE: reference to a compiler-generated field
                        if (displayClass1351.user.UserMode != Settings.UserMode)
                        {
                            // ISSUE: method pointer
                            //await ((DependencyObject)App.Instance.RootFrame).Dispatcher.RunAsync(
                            //(CoreDispatcherPriority)0, new DispatchedHandler((object)displayClass1351,
                            //__methodptr(\u003CCheckRykenUser\u003Eb__1)));
                        }
                    }
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if (displayClass1351.user != null && SharedSettings.CurrentAccount != null 
                        && SharedSettings.CurrentAccount.UserName != displayClass1351.user.Name)
                    {
                        try
                        {
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            // ISSUE: reference to a compiler-generated field
                            RykenUser rykenUser2 = await displayClass1351.locals1.userClient.ChangeName(
                                displayClass1351.user.Id, SharedSettings.CurrentAccount.UserName);
                        }
                        catch
                        {
                        }
                    }
                    // ISSUE: reference to a compiler-generated field
                    flag = displayClass1351.user != null;
                    displayClass1351 = (App.DisplayClass135_1) null;
                }
                catch
                {
                    flag = true;
                }
            }
            if (Settings.RykenUserID != null && flag)
                return;
            RykenUser user = (RykenUser)null;
            try
            {
                if (SharedSettings.CurrentAccount == null)
                {
                    // ISSUE: reference to a compiler-generated field
                    user = await displayClass1350.userClient.CreateNewUser();
                }
                else
                {
                    // ISSUE: reference to a compiler-generated field
                    user = await displayClass1350.userClient.CreateNewUser(SharedSettings.CurrentAccount.UserName);
                }
            }
            catch
            {
            }
            if (user != null)
                Settings.RykenUserID = user.Id;
            user = (RykenUser)null;
        }

        public object GetThemeResource(string key)
        {
            ResourceDictionary themeDictionary = this.GetThemeDictionary();
            return themeDictionary != null && ((IDictionary<object, object>)themeDictionary).ContainsKey((object)key) ? ((IDictionary<object, object>)themeDictionary)[(object)key] : (object)null;
        }

        public void SetThemeResource(string key, object value)
        {
            ResourceDictionary themeDictionary = this.GetThemeDictionary();
            if (themeDictionary == null)
                return;
            if (((IDictionary<object, object>)themeDictionary).ContainsKey((object)key))
                ((IDictionary<object, object>)themeDictionary)[(object)key] = value;
            else
                ((IDictionary<object, object>)themeDictionary).Add((object)key, value);
        }

        public void SetThemeBrushOpacity(string key, ApplicationTheme theme, double opacity)
        {
            ResourceDictionary themeDictionary = this.GetThemeDictionary(theme);
            if (!((IDictionary<object, object>)themeDictionary).ContainsKey((object)key))
                return;
            ((Brush)(((IDictionary<object, object>)themeDictionary)[(object)key] 
                as SolidColorBrush)).Opacity = opacity;
        }

        public void SetThemeBrushColor(string key, ApplicationTheme theme, Color color)
        {
            ResourceDictionary themeDictionary = this.GetThemeDictionary(theme);
            if (!((IDictionary<object, object>)themeDictionary).ContainsKey((object)key))
                return;
            (((IDictionary<object, object>)themeDictionary)[(object)key] 
                as SolidColorBrush).Color = color;
        }

        public ResourceDictionary GetThemeDictionary()
        {
            ElementTheme theme = Settings.Theme;

            if (theme == ElementTheme.Dark)
                return this.GetThemeDictionary((ApplicationTheme)1);

            return theme == ElementTheme.Light 
                ? this.GetThemeDictionary((ApplicationTheme)ElementTheme.Default) 
                : this.GetThemeDictionary(this.RequestedTheme);
        }

        public ResourceDictionary GetThemeDictionary(ApplicationTheme theme)
        {
            string key = theme.ToString();
            return this.Resources.ThemeDictionaries.ContainsKey((object)key) 
                ? (ResourceDictionary)this.Resources.ThemeDictionaries[(object)key]
                : (ResourceDictionary)null;
        }

        public ResourceDictionary GetThemeDictionary(string name)
        {
            foreach (ResourceDictionary mergedDictionary 
                in (IEnumerable<ResourceDictionary>)this.Resources.MergedDictionaries)
            {
                if (mergedDictionary.ThemeDictionaries.ContainsKey((object)name))
                    return mergedDictionary.ThemeDictionaries[(object)name] as ResourceDictionary;
            }
            return this.Resources.ThemeDictionaries.ContainsKey((object)name) 
                ? (ResourceDictionary)this.Resources.ThemeDictionaries[(object)name] 
                : (ResourceDictionary)null;
        }

        protected override async void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            base.OnShareTargetActivated(args);
            Helper.Write((object)"Received share contract");
            if (args.Kind == ActivationKind.ShareTarget)
            {
                if (args.ShareOperation.Data.Contains(StandardDataFormats.WebLink))
                {
                    Helper.Write((object)"Launching from WebLink");
                    CustomFrame frame = new CustomFrame();
                    Window.Current.Content = (UIElement)frame;
                    Window current1 = Window.Current;

                    //WindowsRuntimeMarshal.AddEventHandler<WindowActivatedEventHandler>(
                    //    new Func<WindowActivatedEventHandler, EventRegistrationToken>(current1.add_Activated),
                    //    new Action<EventRegistrationToken>(current1.remove_Activated), 
                    //    new WindowActivatedEventHandler(this.WindowShareTargetActivated));

                    this.windowActivatedTask = new TaskCompletionSource<bool>();
                    TaskCompletionSource<bool> visibiltyTask = new TaskCompletionSource<bool>();

                    Window current2 = Window.Current;
                    //WindowsRuntimeMarshal.AddEventHandler<WindowVisibilityChangedEventHandler>(
                    //    new Func<WindowVisibilityChangedEventHandler, EventRegistrationToken>(
                    //        current2.add_VisibilityChanged), new Action<EventRegistrationToken>(
                    //            current2.remove_VisibilityChanged), (WindowVisibilityChangedEventHandler)(
                    //            (s, e) =>
                    //    {
                    //        if (!e.Visible)
                    //            return;
                    //        visibiltyTask.TrySetResult(true);
                    //    }));
                    Window.Current.Activate();
                    int num = await visibiltyTask.Task ? 1 : 0;
                    frame.Navigate(typeof(ShareLinkPage), (object)args.ShareOperation);
                    frame = (CustomFrame)null;
                }
                else
                {
                    if (!args.ShareOperation.Data.Contains(StandardDataFormats.StorageItems))
                        return;
                    this.activationArgs = (IActivatedEventArgs)args;
                    this.initialSetup();
                }
            }
            else
            {
                ActivationKind kind = args.Kind;
            }
        }

        private void WindowShareTargetActivated(object sender, WindowActivatedEventArgs e)
        {
           // WindowsRuntimeMarshal.RemoveEventHandler<WindowActivatedEventHandler>(
           //     new Action<EventRegistrationToken>(Window.Current.remove_Activated), 
           //     new WindowActivatedEventHandler(this.WindowShareTargetActivated));
            this.windowActivatedTask.TrySetResult(true);
        }

        public void StartCallback(ApplicationInitializationCallbackParams par)
        {
            Helper.Write((object)"Start callback");
        }

        public void OpenSettings()
        {
            SettingsContainer settings = new SettingsContainer();
            Popup popup1 = new Popup();
            popup1.Child = ((UIElement)settings);
            ((FrameworkElement)popup1).RequestedTheme = (App.Theme);
            Popup popup2 = popup1;
            DefaultPage.SetPopupArrangeMethod((DependencyObject)popup2, (Func<Point>)(() =>
            {
                Rect bounds1 = Window.Current.Bounds;
                Rect bounds2 = (Window.Current.Content as FrameworkElement).GetBounds(Window.Current.Content);
                ((FrameworkElement)settings).Width = (Math.Min(500.0, bounds2.Width));
                ((FrameworkElement)settings).Height = (Math.Min(700.0, bounds2.Bottom - bounds1.Top));
                return new Point()
                {
                    X = bounds2.Left + (bounds2.Width - ((FrameworkElement)settings).Width) / 2.0,
                    Y = bounds2.Bottom - ((FrameworkElement)settings).Height
                };
            }));
            //DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, 150.0));
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            Window window = args.Window;
            //WindowsRuntimeMarshal.AddEventHandler<WindowClosedEventHandler>(new Func<WindowClosedEventHandler, EventRegistrationToken>(window.add_Closed), new Action<EventRegistrationToken>(window.remove_Closed), new WindowClosedEventHandler(this.Window_Closed));
            base.OnWindowCreated(args);
        }

        private async void Window_Closed(object sender, CoreWindowEventArgs e)
        {
            if (DefaultPage.Current == null)
                return;
            //DefaultPage.Current.VideoPlayer.SetBookmark(save: true);
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            Helper.Write((object)("First navigation, window size: " + (object)Window.Current.Bounds));
            if (this.pageInfoCollection != null)
            {
                PageInfo[] pages = this.pageInfoCollection.Pages;
                if (pages.Length > 1)
                {
                    for (int index = 0; index < pages.Length - 1; ++index)
                        this.rootFrame.BackStack.Add(new PageStackEntry(pages[index].PageType, pages[index].Parameter, (NavigationTransitionInfo)null));
                }
                this.pageInfoCollection = (PageInfoCollection)null;
            }
            Helper.Write((object)"First navigation complete");
        }

        private void rootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            bool completeDeferral = true;
            Helper.Write((object)"Suspending");
            try
            {
                await Settings.SavePageInfoCollection(this.RootFrame);
                Settings.SuspendedAt = DateTimeOffset.Now;
                //await DefaultPage.Current.VideoPlayer.SetBookmark(save: true);
            }
            catch
            {
                if (Debugger.IsAttached)
                    Debugger.Break();
            }
            if (DefaultPage.Current != null)
            {
                VideoPlayer vp = default;//DefaultPage.Current.VideoPlayer;
                if (App.DeviceFamily == DeviceFamily.Desktop)
                    BackgroundMediaPlayer.Shutdown();

                if (App.DeviceFamily == DeviceFamily.Mobile && Settings.ResumeAsAudio 
                    && vp.MediaElement.CurrentState == MediaElementState.Playing)
                {
                    this.backgroundQuality = vp.CurrentQuality;
                    await vp.ChangeQuality(YouTubeQuality.Audio, false);
                    this.backgroundAudio = true;
                    await Task.Delay(4000);
                    vp.DeregisterBackgroundEvent();
                }
                vp = (VideoPlayer)null;
            }
            Helper.Write((object)nameof(App), (object)"Suspension complete");
            if (!completeDeferral)
                return;
            deferral.Complete();
        }


        private Task<bool> waitForBackgroundMedia(TimeSpan timeout)
        {
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            var displayClass1520 = new App.DisplayClass152_0();
            
            // ISSUE: reference to a compiler-generated field
            displayClass1520.tcs = new TaskCompletionSource<bool>();
            // ISSUE: reference to a compiler-generated field
            displayClass1520.player = BackgroundMediaPlayer.Current;
            // ISSUE: reference to a compiler-generated field
            displayClass1520.opened = (TypedEventHandler<MediaPlayer, object>)null;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = timeout;
            // ISSUE: reference to a compiler-generated field
            displayClass1520.timer = dispatcherTimer;
            // ISSUE: reference to a compiler-generated field
            displayClass1520.tick = (EventHandler<object>)null;
            // ISSUE: reference to a compiler-generated field
            displayClass1520.dispatcher = Window.Current.Dispatcher;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: method pointer
            displayClass1520.opened = default;//new TypedEventHandler<MediaPlayer, object>((object)displayClass1520, 
                //__methodptr(\u003CwaitForBackgroundMedia\u003Eb__0));
            // ISSUE: reference to a compiler-generated field
            MediaPlayer player = displayClass1520.player;
            // ISSUE: reference to a compiler-generated field
            //WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MediaPlayer, object>>(new Func<TypedEventHandler<MediaPlayer, object>, EventRegistrationToken>(player.add_MediaOpened), new Action<EventRegistrationToken>(player.remove_MediaOpened), displayClass1520.opened);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            displayClass1520.tick = new EventHandler<object>(displayClass1520.waitForBackgroundMedia_u003Eb);
            // ISSUE: reference to a compiler-generated field
            displayClass1520.timer.Start();
            // ISSUE: reference to a compiler-generated field
            DispatcherTimer timer = displayClass1520.timer;
            // ISSUE: reference to a compiler-generated field
            //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(timer.add_Tick), new Action<EventRegistrationToken>(timer.remove_Tick), displayClass1520.tick);
            // ISSUE: reference to a compiler-generated field
            return displayClass1520.tcs.Task;
        }

        private class DisplayClass152_0
        {
            internal TaskCompletionSource<bool> tcs;
            internal MediaPlayer player;
            internal TypedEventHandler<MediaPlayer, object> opened;
            internal CoreDispatcher dispatcher;
            internal EventHandler<object> tick;
            internal DispatcherTimer timer;

            public DisplayClass152_0()
            {
            }

            internal void waitForBackgroundMedia_u003Eb(object sender, object e)
            {
                throw new NotImplementedException();
            }
        }

        private class DisplayClass91_0
        {
            internal App u003E4;
            internal ExceptionData data;
            internal string exceptionMessage;

            public DisplayClass91_0()
            {
            }
        }

        private class DisplayClass102_0
        {
            internal MessageClient messCli;

            public DisplayClass102_0()
            {
            }
        }

        private class DisplayClass115_0
        {
            internal App u003E4;
            internal YouTubeError e;

            public DisplayClass115_0()
            {
            }
        }

        private class DisplayClass130_0
        {
            internal App u003E4;
            internal ExceptionData data;

            public DisplayClass130_0()
            {
            }
        }

        private class DisplayClass135_0
        {
            internal RykenUserClient userClient;

            public DisplayClass135_0()
            {
            }
        }

        private class DisplayClass135_1
        {
            internal DisplayClass135_0 locals1;
            internal RykenUser user;

            public DisplayClass135_1()
            {
            }
        }
    }//App class


}//namespace


