// Decompiled with JetBrains decompiler
// Type: myTube.HomePage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.ProductKeyPages;
using myTube.TestPages;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class HomePage : Page, IComponentConnector
  {
    private const string Tag = "HomePage";
    private const int maxSubs = 15;
    private bool changePageInLoaded = true;
    private object categoriesListSelectOnLoad;
    private string watchThis;
    private bool firstCategoryChange;
    private List<YouTubeEntry> preloadedVids;
    private YouTubeEntryClient entryClient = new YouTubeEntryClient();
    private ChannelNotifications notifiedAbout;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate categoriesTemplate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private DataTemplate categoriesTemplate2;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CommandBar bottomBar;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton pinSubsButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton refreshButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton pinCategoriesButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton purchaseButon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton productKey;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton suggestFeature;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton reportIssue;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton about;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton settingsButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton testButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList popular;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListBox categoriesList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserHome userHome;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList subscriptionsList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public HomePage()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.HomePage_Loaded));
      this.subscriptionsList.Client = (VideoListClient) new SubscriptionsPageClient();
      this.subscriptionsList.LoadOnScroll = true;
      VideoListClient client = this.subscriptionsList.Client;
      YouTubeEntryClient youTubeEntryClient = new YouTubeEntryClient(3);
      youTubeEntryClient.APIKey = App.GetAPIKey(0);
      youTubeEntryClient.UseAccessToken = false;
      client.RefreshClientOverride = (EntryClient<YouTubeEntry>) youTubeEntryClient;
      this.watchThis = App.Strings["videos.lists.watchthis", "watch this"].ToLower();
      ((Collection<object>) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "Categories"]).Insert(0, (object) this.watchThis);
      ((AppBar) this.bottomBar).put_ClosedDisplayMode((AppBarClosedDisplayMode) 0);
      ((FrameworkElement) this.bottomBar).put_MaxHeight(12.0);
    }

    private async void HomePage_Loaded(object sender, RoutedEventArgs e)
    {
      Helper.Write((object) nameof (HomePage), (object) "Loaded");
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.HomePage_Loaded));
      await Task.Delay(1000);
      if (this.changePageInLoaded && ((App) Application.Current).RootFrame.ForwardStack.Count == 0 && ((App) Application.Current).RootFrame.BackStack.Count == 0 && SharedSettings.CurrentAccount != null)
      {
        int num = await App.Instance.WindowActivatedTask ? 1 : 0;
        this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.subscriptionsList), false);
      }
      this.SetAppBar(this.overCanvas.SelectedPage);
      Helper.Write((object) nameof (HomePage), (object) "Load completed");
    }

    private async void Current_Activated2(object sender, WindowActivatedEventArgs e)
    {
      await Task.Delay(500);
      this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.subscriptionsList), false);
    }

    private async void categoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      try
      {
        if (((Selector) this.categoriesList).SelectedItem != (object) this.watchThis)
        {
          this.popular.DependsOnSignIn = false;
          Category cat = (((Selector) this.categoriesList).SelectedItem as CategoryTextInfo).Category;
          List<RegionInfo> regionsLoadedTask = await RegionInfo.RegionsLoadedTask;
          this.popular.Clear(true);
          Helper.Write((object) nameof (HomePage), (object) ("Seting category to " + (object) cat));
          VideoList popular = this.popular;
          FeedClient feedClient = new FeedClient(YouTubeFeed.Popular, cat, YouTubeTime.Today, 15);
          feedClient.APIKey = App.GetAPIKey(0);
          popular.Client = (VideoListClient) feedClient;
        }
        else if (SharedSettings.CurrentAccount == null)
        {
          DefaultPage.Current.OpenBrowser();
        }
        else
        {
          this.popular.DependsOnSignIn = true;
          this.popular.Client = (VideoListClient) new RecommendedPageClient(20);
        }
        if (this.firstCategoryChange)
          this.overCanvas.ScrollToPage(0, false, true);
        else
          this.firstCategoryChange = true;
      }
      catch
      {
      }
    }

    private async void overCanvas_SignInChanged(object sender, bool e)
    {
      this.userHome.SignInChanged(e);
      int num = e ? 1 : 0;
    }

    private double scoreActivity(YouTubeEntry entry)
    {
      double num1 = 0.0;
      double num2;
      switch (entry.ActivityType)
      {
        case YouTubeActivity.Upload:
          num2 = num1 + 450.0;
          break;
        case YouTubeActivity.Recommended:
          num2 = num1 + 200.0;
          break;
        case YouTubeActivity.Like:
          num2 = num1 + 300.0;
          break;
        default:
          num2 = num1 + 100.0;
          break;
      }
      return num2 - (DateTime.UtcNow - entry.Time).TotalHours * 3.0;
    }

    private void settingsButton_Click(object sender, RoutedEventArgs e) => App.Instance.OpenSettings();

    private void overCanvas_ShownChanged(object sender, bool e)
    {
      if (e)
        ((UIElement) this.bottomBar).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.bottomBar).put_Visibility((Visibility) 1);
    }

    private void suggestFeature_Tapped(object sender, RoutedEventArgs e) => App.SendSupportEmail("myTube Feature Suggestion", "");

    private void reportIssue_Tapped(object sender, RoutedEventArgs e) => App.SendSupportEmail("myTube Problem Report", "");

    private void about_Tapped(object sender, RoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (AboutPage));

    private async void pinSubsButton_Click(object sender, RoutedEventArgs e)
    {
      TypeConstructor tc = new TypeConstructor(typeof (SubscriptionsPageClient), new object[0]);
      tc.Construct();
      SecondaryTile tile = await TileHelper.CreateTile(tc, "Subscriptions", new TileArgs(typeof (HomePage), overCanvasPage: 3)
      {
        ShouldSignInFirst = true
      });
      Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>> renderTask = (Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>>) (async (el, rtb) => await DefaultPage.Current.RenderElementAsync(el, 0.5, rtb));
      Border border1 = new Border();
      ((FrameworkElement) border1).put_Width(Window.Current.Bounds.Width);
      ((FrameworkElement) border1).put_Height(Window.Current.Bounds.Height);
      Border border2 = border1;
      ProgressBar progressBar = new ProgressBar();
      ((Control) progressBar).put_Background((Brush) null);
      ((Control) progressBar).put_Foreground((Brush) App.Instance.GetThemeResource("AccentBrush"));
      progressBar.put_IsIndeterminate(true);
      border2.put_Child((UIElement) progressBar);
      Popup popup = new Popup();
      popup.put_Child((UIElement) border1);
      DefaultPage.Current.ShowPopup(popup, new Point(), new Point(), FadeType.Half, false);
      List<TileNotification> nots = await TileHelper.UpdateSecondaryTile(tc, renderTask, false);
      if (await tile.RequestCreateAsync())
      {
        TileUpdater forSecondaryTile = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId);
        forSecondaryTile.Clear();
        forSecondaryTile.EnableNotificationQueue(true);
        foreach (TileNotification tileNotification in nots)
          forSecondaryTile.Update(tileNotification);
        await App.Instance.SetUpBackgroundTask();
      }
      else
        TileHelper.CleanUpFolders();
      DefaultPage.Current.ClosePopup();
    }

    private async void pinCategoriesButton_Click(object sender, RoutedEventArgs e)
    {
      TypeConstructor tc;
      SecondaryTile tile;
      if (((Selector) this.categoriesList).SelectedItem != (object) this.watchThis)
      {
        if (((Selector) this.categoriesList).SelectedItem is CategoryTextInfo selectedItem)
        {
          int category = (int) selectedItem.Category;
        }
        tc = new TypeConstructor(typeof (FeedClient), new object[4]
        {
          (object) YouTubeFeed.Popular,
          (object) selectedItem.Category,
          (object) YouTubeTime.Today,
          (object) 5
        });
        TileArgs tileArgs = new TileArgs(typeof (HomePage), selectedItem.Category.ToString(), 0);
        tile = await TileHelper.CreateTile(tc, selectedItem.RealText, tileArgs);
      }
      else
      {
        tc = new TypeConstructor(typeof (RecommendedPageClient), new object[1]
        {
          (object) 5
        });
        tile = await TileHelper.CreateTile(tc, this.watchThis, new TileArgs(typeof (HomePage), "watch this", 0)
        {
          ShouldSignInFirst = true
        });
      }
      tc.Construct();
      Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>> renderTask = (Func<FrameworkElement, RenderTargetBitmap, Task<RenderTargetBitmap>>) (async (el, rtb) => await DefaultPage.Current.RenderElementAsync(el, 0.5, rtb));
      Border border1 = new Border();
      ((FrameworkElement) border1).put_Width(Window.Current.Bounds.Width);
      ((FrameworkElement) border1).put_Height(Window.Current.Bounds.Height);
      Border border2 = border1;
      ProgressBar progressBar = new ProgressBar();
      ((Control) progressBar).put_Background((Brush) null);
      ((Control) progressBar).put_Foreground((Brush) App.Instance.GetThemeResource("AccentBrush"));
      progressBar.put_IsIndeterminate(true);
      border2.put_Child((UIElement) progressBar);
      Popup popup = new Popup();
      popup.put_Child((UIElement) border1);
      DefaultPage.Current.ShowPopup(popup, new Point(), new Point(), FadeType.Half, false);
      List<TileNotification> nots = await TileHelper.UpdateSecondaryTile(tc, renderTask, false);
      if (await tile.RequestCreateAsync())
      {
        TileUpdater forSecondaryTile = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tile.TileId);
        forSecondaryTile.Clear();
        forSecondaryTile.EnableNotificationQueue(true);
        foreach (TileNotification tileNotification in nots)
          forSecondaryTile.Update(tileNotification);
        App.Instance.SetUpBackgroundTask();
      }
      else
        TileHelper.CleanUpFolders();
      DefaultPage.Current.ClosePopup();
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (App.IsTrial)
        ((UIElement) this.purchaseButon).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.purchaseButon).put_Visibility((Visibility) 1);
      if (App.IsTrial || Settings.ProductKeyRequestId != null || Settings.ProductKey != null || Settings.UserMode >= UserMode.Owner)
        ((UIElement) this.productKey).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.productKey).put_Visibility((Visibility) 1);
      if (Settings.UserMode < UserMode.Beta)
        ((UIElement) this.testButton).put_Visibility((Visibility) 1);
      else
        ((UIElement) this.testButton).put_Visibility((Visibility) 0);
      Helper.Write((object) nameof (HomePage), (object) nameof (OnNavigatedTo));
      TileArgs launch = App.GetLaunchTileArgs((object) this);
      if (e.NavigationMode != 1 && e.Parameter is string)
      {
        Category result = Category.All;
        if (Enum.TryParse<Category>(e.Parameter.ToString(), out result))
        {
          foreach (object obj in (Collection<object>) (((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "Categories"] as ObjectCollection))
          {
            if (obj is CategoryTextInfo)
            {
              CategoryTextInfo categoryTextInfo = obj as CategoryTextInfo;
              if (categoryTextInfo.Category == result)
              {
                ((Selector) this.categoriesList).put_SelectedItem((object) categoryTextInfo);
                break;
              }
            }
          }
        }
        else if (e.Parameter.ToString() == this.watchThis || e.Parameter.ToString() == "watch this")
          ((Selector) this.categoriesList).put_SelectedItem((object) this.watchThis);
      }
      else if (e.NavigationMode != 1)
      {
        if (Window.Current.Visible)
        {
          Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo - no predetermined category");
          ((Selector) this.categoriesList).put_SelectedIndex(1);
          Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo - Selected category");
        }
        else
        {
          Window current = Window.Current;
          WindowsRuntimeMarshal.AddEventHandler<WindowActivatedEventHandler>(new Func<WindowActivatedEventHandler, EventRegistrationToken>(current.add_Activated), new Action<EventRegistrationToken>(current.remove_Activated), new WindowActivatedEventHandler(this.Current_Activated));
        }
      }
      else if (((Selector) this.categoriesList).SelectedIndex == -1)
      {
        if (Window.Current.Visible)
        {
          Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo - no predetermined category");
          ((Selector) this.categoriesList).put_SelectedIndex(1);
          Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo - Selected category");
        }
        else
        {
          Window current = Window.Current;
          WindowsRuntimeMarshal.AddEventHandler<WindowActivatedEventHandler>(new Func<WindowActivatedEventHandler, EventRegistrationToken>(current.add_Activated), new Action<EventRegistrationToken>(current.remove_Activated), new WindowActivatedEventHandler(this.Current_Activated));
        }
      }
      if (launch != null)
      {
        this.changePageInLoaded = false;
        if (launch.OverCanvasPage != -1)
        {
          await Task.Delay(1000);
          this.overCanvas.ScrollToIndex(launch.OverCanvasPage, false);
        }
      }
      else
        this.changePageInLoaded = true;
      base.OnNavigatedTo(e);
      Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo completed");
    }

    private void Current_Activated(object sender, WindowActivatedEventArgs e)
    {
      WindowsRuntimeMarshal.RemoveEventHandler<WindowActivatedEventHandler>(new Action<EventRegistrationToken>(Window.Current.remove_Activated), new WindowActivatedEventHandler(this.Current_Activated));
      Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo - no predetermined category");
      ((Selector) this.categoriesList).put_SelectedIndex(1);
      Helper.Write((object) nameof (HomePage), (object) "OnNavigatedTo - Selected category");
    }

    private void overCanvas_SelectedPageChanged(object sender, OnSelectedPageChangedEventArgs e) => this.SetAppBar(e.NewPage);

    private void SetAppBar(int i)
    {
      if (i == OverCanvas.GetOverCanvasPage((DependencyObject) this.subscriptionsList) || i == OverCanvas.GetOverCanvasPage((DependencyObject) this.popular))
        this.BottomAppBar.put_ClosedDisplayMode((AppBarClosedDisplayMode) 0);
      else
        this.BottomAppBar.put_ClosedDisplayMode((AppBarClosedDisplayMode) 1);
      if (i == OverCanvas.GetOverCanvasPage((DependencyObject) this.subscriptionsList) || i == OverCanvas.GetOverCanvasPage((DependencyObject) this.userHome))
      {
        if (i == OverCanvas.GetOverCanvasPage((DependencyObject) this.subscriptionsList))
          ((UIElement) this.refreshButton).put_Visibility((Visibility) 0);
        else
          ((UIElement) this.refreshButton).put_Visibility((Visibility) 1);
        ((UIElement) this.pinSubsButton).put_Visibility((Visibility) 0);
      }
      else
      {
        AppBarButton pinSubsButton = this.pinSubsButton;
        Visibility visibility1;
        ((UIElement) this.refreshButton).put_Visibility((Visibility) (int) (visibility1 = (Visibility) 1));
        Visibility visibility2 = visibility1;
        ((UIElement) pinSubsButton).put_Visibility(visibility2);
      }
      if (i == OverCanvas.GetOverCanvasPage((DependencyObject) this.popular) || i == OverCanvas.GetOverCanvasPage((DependencyObject) this.categoriesList))
        ((UIElement) this.pinCategoriesButton).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.pinCategoriesButton).put_Visibility((Visibility) 1);
    }

    private void refreshButton_Click(object sender, RoutedEventArgs e)
    {
      this.subscriptionsList.Clear(true);
      this.subscriptionsList.Load();
    }

    private async void purchaseButon_Click(object sender, RoutedEventArgs e) => await App.Purchase();

    private void productKey_Click(object sender, RoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (KeyRequestPage));

    private void testButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private void testButton_Click(object sender, RoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (TestListPage));

    private async void subsSetupButton_Click(object sender, RoutedEventArgs e)
    {
    }

    private async void subscriptionsList_VideosLoaded(object sender, YouTubeEntry[] e)
    {
      if (((Collection<YouTubeEntry>) this.subscriptionsList.Entries).Count >= 50)
        return;
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      List<string> checkedChannels = new List<string>();
      bool changed = false;
      for (int i = 0; i < ((Collection<YouTubeEntry>) this.subscriptionsList.Entries).Count; ++i)
      {
        YouTubeEntry vid = ((Collection<YouTubeEntry>) this.subscriptionsList.Entries)[i];
        if (!checkedChannels.Contains(vid.Author))
        {
          checkedChannels.Add(vid.Author);
          if (App.GlobalObjects.ChannelNotifications.ContainsChannel(vid.Author))
          {
            if (this.notifiedAbout == null)
            {
              HomePage homePage = this;
              ChannelNotifications notifiedAbout = homePage.notifiedAbout;
              ChannelNotifications channelNotifications = await ChannelNotifications.Load(ApplicationData.Current.LocalFolder, "NotifiedAbout.json");
              homePage.notifiedAbout = channelNotifications;
              homePage = (HomePage) null;
            }
            if (this.notifiedAbout.AddOrModifyChannel(vid.Author, vid.ID))
              changed = true;
          }
        }
        vid = (YouTubeEntry) null;
      }
      if (changed)
      {
        try
        {
          await this.notifiedAbout.Save(ApplicationData.Current.LocalFolder, "NotifiedAbout.json");
        }
        catch
        {
        }
      }
      checkedChannels = (List<string>) null;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///HomePage.xaml"), (ComponentResourceLocation) 0);
      this.categoriesTemplate = (DataTemplate) ((FrameworkElement) this).FindName("categoriesTemplate");
      this.categoriesTemplate2 = (DataTemplate) ((FrameworkElement) this).FindName("categoriesTemplate2");
      this.bottomBar = (CommandBar) ((FrameworkElement) this).FindName("bottomBar");
      this.pinSubsButton = (AppBarButton) ((FrameworkElement) this).FindName("pinSubsButton");
      this.refreshButton = (AppBarButton) ((FrameworkElement) this).FindName("refreshButton");
      this.pinCategoriesButton = (AppBarButton) ((FrameworkElement) this).FindName("pinCategoriesButton");
      this.purchaseButon = (AppBarButton) ((FrameworkElement) this).FindName("purchaseButon");
      this.productKey = (AppBarButton) ((FrameworkElement) this).FindName("productKey");
      this.suggestFeature = (AppBarButton) ((FrameworkElement) this).FindName("suggestFeature");
      this.reportIssue = (AppBarButton) ((FrameworkElement) this).FindName("reportIssue");
      this.about = (AppBarButton) ((FrameworkElement) this).FindName("about");
      this.settingsButton = (AppBarButton) ((FrameworkElement) this).FindName("settingsButton");
      this.testButton = (AppBarButton) ((FrameworkElement) this).FindName("testButton");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.popular = (VideoList) ((FrameworkElement) this).FindName("popular");
      this.categoriesList = (ListBox) ((FrameworkElement) this).FindName("categoriesList");
      this.userHome = (UserHome) ((FrameworkElement) this).FindName("userHome");
      this.subscriptionsList = (VideoList) ((FrameworkElement) this).FindName("subscriptionsList");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.pinSubsButton_Click));
          break;
        case 2:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.refreshButton_Click));
          break;
        case 3:
          ButtonBase buttonBase3 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase3.add_Click), new Action<EventRegistrationToken>(buttonBase3.remove_Click), new RoutedEventHandler(this.pinCategoriesButton_Click));
          break;
        case 4:
          ButtonBase buttonBase4 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase4.add_Click), new Action<EventRegistrationToken>(buttonBase4.remove_Click), new RoutedEventHandler(this.purchaseButon_Click));
          break;
        case 5:
          ButtonBase buttonBase5 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase5.add_Click), new Action<EventRegistrationToken>(buttonBase5.remove_Click), new RoutedEventHandler(this.productKey_Click));
          break;
        case 6:
          ButtonBase buttonBase6 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase6.add_Click), new Action<EventRegistrationToken>(buttonBase6.remove_Click), new RoutedEventHandler(this.suggestFeature_Tapped));
          break;
        case 7:
          ButtonBase buttonBase7 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase7.add_Click), new Action<EventRegistrationToken>(buttonBase7.remove_Click), new RoutedEventHandler(this.reportIssue_Tapped));
          break;
        case 8:
          ButtonBase buttonBase8 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase8.add_Click), new Action<EventRegistrationToken>(buttonBase8.remove_Click), new RoutedEventHandler(this.about_Tapped));
          break;
        case 9:
          ButtonBase buttonBase9 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase9.add_Click), new Action<EventRegistrationToken>(buttonBase9.remove_Click), new RoutedEventHandler(this.settingsButton_Click));
          break;
        case 10:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.testButton_Tapped));
          ButtonBase buttonBase10 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase10.add_Click), new Action<EventRegistrationToken>(buttonBase10.remove_Click), new RoutedEventHandler(this.testButton_Click));
          break;
        case 11:
          ((OverCanvas) target).SignInChanged += new EventHandler<bool>(this.overCanvas_SignInChanged);
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
          ((OverCanvas) target).SelectedPageChanged += new EventHandler<OnSelectedPageChangedEventArgs>(this.overCanvas_SelectedPageChanged);
          break;
        case 12:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.categoriesList_SelectionChanged));
          break;
        case 13:
          ((VideoList) target).VideosLoaded += new EventHandler<YouTubeEntry[]>(this.subscriptionsList_VideosLoaded);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
