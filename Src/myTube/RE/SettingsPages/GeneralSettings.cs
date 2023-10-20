// Decompiled with JetBrains decompiler
// Type: myTube.SettingsPages.GeneralSettings
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Popups;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube.SettingsPages
{
  public sealed class GeneralSettings : UserControl, IComponentConnector
  {
    private bool loaded;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel genaralPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel themePanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel playbackPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel betaPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel ownerPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button newUserButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button exceptionButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock cipher;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock user;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button cipherButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock backgroundTask;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock backgroundTaskException;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock startTime;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox useNavigationPage;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button sendFiles;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button sendAccount;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button sendAccountCollection;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock notificationTaskMessage;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox refreshTokenBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Button refreshTokenButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox qualityPicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox afterVideoBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox playbackLocationBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox playAutomatically;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox prefer60FPS;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox blurVideo;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox resumeAsAudio;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox warnData;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox themePicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox colorScheme;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox thumbnailPicker;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox transparentTile;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox rotationBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ComboBox region;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox searchButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CheckBox allowNotifications;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public GeneralSettings()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.GeneralSettings_Loaded));
    }

    private async void GeneralSettings_Loaded(object sender, RoutedEventArgs e)
    {
      if (App.PlatformType == PlatformType.WindowsUWP || App.DeviceFamily == DeviceFamily.Mobile)
        ((ToggleButton) this.transparentTile).put_IsChecked(new bool?(Settings.TileStyle == TileStyle.Transparent));
      else
        ((UIElement) this.transparentTile).put_Visibility((Visibility) 1);
      if (Settings.AllowChangeMiniPlayerType)
      {
        switch (Settings.MiniPlayerType)
        {
          case MiniPlayerType.Background:
            ((Selector) this.playbackLocationBox).put_SelectedIndex(1);
            break;
          case MiniPlayerType.MiniPlayer:
            ((Selector) this.playbackLocationBox).put_SelectedIndex(0);
            break;
        }
      }
      else
        ((UIElement) this.playbackLocationBox).put_Visibility((Visibility) 1);
      if (App.DeviceFamily != DeviceFamily.Mobile)
        ((UIElement) this.searchButton).put_Visibility((Visibility) 1);
      else
        ((ToggleButton) this.searchButton).put_IsChecked(new bool?(Settings.SearchInAppBar));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[0] as ComboBoxItem)).put_Content((object) App.Strings["settings.quality.lastselected", "keep last selected quality"].ToLower());
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[1] as ComboBoxItem)).put_Content((object) ("240p (" + App.Strings["settings.quality.lq", "low quality"].ToLower() + ")"));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[2] as ComboBoxItem)).put_Content((object) ("360p (" + App.Strings["settings.quality.hq", "high quality"].ToLower() + ")"));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[3] as ComboBoxItem)).put_Content((object) ("480p (" + App.Strings["settings.quality.sd", "standard definition"].ToLower() + ")"));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[4] as ComboBoxItem)).put_Content((object) ("720p (" + App.Strings["settings.quality.hd", "high definition"].ToLower() + ")"));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[5] as ComboBoxItem)).put_Content((object) ("1080p (" + App.Strings["settings.quality.fhd", "full high definition"].ToLower() + ")"));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[6] as ComboBoxItem)).put_Content((object) ("1440p (" + App.Strings["settings.quality.qhd", "quad high definition"].ToLower() + ")"));
      ((ContentControl) (((IList<object>) ((ItemsControl) this.qualityPicker).Items)[7] as ComboBoxItem)).put_Content((object) ("2160p (" + App.Strings["settings.quality.uhd", "ultra high definition"].ToLower() + " - 4K)"));
      ((ToggleButton) this.warnData).put_IsChecked(new bool?(Settings.WarnOnData));
      ((ToggleButton) this.useNavigationPage).put_IsChecked(new bool?(Settings.UseNavigatePage));
      ((ToggleButton) this.resumeAsAudio).put_IsChecked(new bool?(Settings.ResumeAsAudio));
      ((ToggleButton) this.prefer60FPS).put_IsChecked(new bool?(Settings.Allow60FPS));
      ((ContentControl) this.prefer60FPS).put_Content((object) (App.Strings["settings.allow60fps", "allow 60 FPS videos"] + " (beta)"));
      ((ToggleButton) this.playAutomatically).put_IsChecked(new bool?(Settings.PlayAutomatically));
      ((Selector) this.afterVideoBox).put_SelectedIndex((int) Settings.AfterVideoSection);
      try
      {
        if (App.HighestQuality < YouTubeQuality.HD2160)
          ((IList<object>) ((ItemsControl) this.qualityPicker).Items).RemoveAt(7);
        if (App.HighestQuality < YouTubeQuality.HD1440)
          ((IList<object>) ((ItemsControl) this.qualityPicker).Items).RemoveAt(6);
        if (App.HighestQuality < YouTubeQuality.HD1080)
          ((IList<object>) ((ItemsControl) this.qualityPicker).Items).RemoveAt(5);
      }
      catch
      {
      }
      ((ToggleButton) this.allowNotifications).put_IsChecked(new bool?(!SharedSettings.NotificationsOnlyOnWifi));
      App.Instance.InitialThemeSetup();
      if (App.Instance.AddedSchemes.Count > 1)
      {
        ((ItemsControl) this.colorScheme).put_ItemsSource((object) App.Instance.AddedSchemes);
        ((Selector) this.colorScheme).put_SelectedItem((object) Settings.ColorCheme);
      }
      else
        ((UIElement) this.colorScheme).put_Visibility((Visibility) 1);
      this.loaded = true;
      if (Settings.Theme == 2)
        ((Selector) this.themePicker).put_SelectedIndex(0);
      else
        ((Selector) this.themePicker).put_SelectedIndex(1);
      if (Settings.Thunbnail == ThumbnailStyle.New)
        ((Selector) this.thumbnailPicker).put_SelectedIndex(0);
      else
        ((Selector) this.thumbnailPicker).put_SelectedIndex(1);
      if (Settings.Quality > App.HighestQuality)
        Settings.Quality = App.HighestQuality;
      if (Settings.KeepSelectedQuality)
      {
        ((Selector) this.qualityPicker).put_SelectedIndex(0);
      }
      else
      {
        try
        {
          switch (Settings.Quality)
          {
            case YouTubeQuality.LQ:
              ((Selector) this.qualityPicker).put_SelectedIndex(1);
              break;
            case YouTubeQuality.HQ:
              ((Selector) this.qualityPicker).put_SelectedIndex(2);
              break;
            case YouTubeQuality.SD:
              ((Selector) this.qualityPicker).put_SelectedIndex(3);
              break;
            case YouTubeQuality.HD:
            case YouTubeQuality.HD60:
              ((Selector) this.qualityPicker).put_SelectedIndex(4);
              break;
            case YouTubeQuality.HD1080:
            case YouTubeQuality.HD1080p60:
              ((Selector) this.qualityPicker).put_SelectedIndex(5);
              break;
            case YouTubeQuality.HD1440:
              ((Selector) this.qualityPicker).put_SelectedIndex(6);
              break;
            case YouTubeQuality.HD2160:
              ((Selector) this.qualityPicker).put_SelectedIndex(7);
              break;
            default:
              ((Selector) this.qualityPicker).put_SelectedIndex(-1);
              break;
          }
        }
        catch
        {
          ((Selector) this.qualityPicker).put_SelectedIndex(0);
        }
      }
      if (App.DeviceFamily != DeviceFamily.Mobile)
        ((UIElement) this.rotationBox).put_Visibility((Visibility) 1);
      else
        ((Selector) this.rotationBox).put_SelectedIndex((int) Settings.RotationType);
      if (Settings.UserMode < UserMode.Beta)
      {
        ((UIElement) this.betaPanel).put_Visibility((Visibility) 1);
        ((UIElement) this.ownerPanel).put_Visibility((Visibility) 1);
      }
      else
      {
        this.cipher.put_Text(YouTube.DecipherAlgorithm);
        if (UserInformation.NameAccessAllowed)
        {
          TextBlock textBlock = this.user;
          string displayNameAsync = await UserInformation.GetDisplayNameAsync();
          textBlock.put_Text(displayNameAsync);
          textBlock = (TextBlock) null;
        }
        else
          this.user.put_Text("private");
        this.backgroundTask.put_Text("Background task last ran at " + (object) Settings.BackgroundTaskLastRun + " with a final state of " + Settings.BackgroundTaskState);
        if (((IDictionary<string, object>) ApplicationData.Current.LocalSettings.Values).ContainsKey("BackgroundTaskExceptions") && !string.IsNullOrWhiteSpace(((IDictionary<string, object>) ApplicationData.Current.LocalSettings.Values)["BackgroundTaskExceptions"].ToString()))
          this.backgroundTaskException.put_Text("Last background task exceptions:\n\n" + ((IDictionary<string, object>) ApplicationData.Current.LocalSettings.Values)["BackgroundTaskExceptions"]);
        else
          this.backgroundTaskException.put_Text("No exceptions recorded in background task");
        this.startTime.put_Text("Launch time: " + (object) App.StartTime.TotalSeconds + " seconds");
        if (Settings.UserMode < UserMode.Owner)
          ((UIElement) this.ownerPanel).put_Visibility((Visibility) 1);
      }
      using (List<RegionInfo>.Enumerator enumerator = RegionInfo.GetCountries().GetEnumerator())
      {
        while (enumerator.MoveNext())
          ((ICollection<object>) ((ItemsControl) this.region).Items).Add((object) enumerator.Current.CountryName);
      }
      ((Selector) this.region).put_SelectedIndex(RegionInfo.GetCountries().IndexOf(Settings.Region));
      if (Settings.UserMode >= UserMode.Beta && ((IDictionary<string, object>) ApplicationData.Current.LocalSettings.Values).ContainsKey("NotificationStatus"))
        this.notificationTaskMessage.put_Text("Notifications Task: " + ((IDictionary<string, object>) ApplicationData.Current.LocalSettings.Values)["NotificationStatus"].ToString());
      ((UIElement) this.blurVideo).put_Visibility((Visibility) 1);
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.loaded)
        return;
      try
      {
        ElementTheme theme = ((Selector) this.themePicker).SelectedIndex != 1 ? (ElementTheme) 2 : (ElementTheme) 1;
        if (theme != Settings.Theme)
          Settings.Theme = theme;
        DefaultPage.Current.SetTheme(theme);
        ((FrameworkElement) Helper.FindParent<Popup>((FrameworkElement) this, 20))?.put_RequestedTheme(theme);
      }
      catch (Exception ex)
      {
      }
    }

    private void thumbnailPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.loaded)
        return;
      try
      {
        ThumbnailStyle thumbnailStyle = ((Selector) this.thumbnailPicker).SelectedIndex != 0 ? ThumbnailStyle.Classic : ThumbnailStyle.New;
        App.GlobalObjects.VideoThumbTemplate = thumbnailStyle != ThumbnailStyle.Classic ? (DataTemplate) ((IDictionary<object, object>) Application.Current.Resources)[(object) "VideoThumbs2"] : (DataTemplate) ((IDictionary<object, object>) Application.Current.Resources)[(object) "VideoThumbs"];
        Settings.Thunbnail = thumbnailStyle;
      }
      catch
      {
      }
    }

    private void qualityPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.qualityPicker == null)
        return;
      Settings.KeepSelectedQuality = ((Selector) this.qualityPicker).SelectedIndex == 0;
      switch (((Selector) this.qualityPicker).SelectedIndex)
      {
        case 1:
          Settings.Quality = YouTubeQuality.LQ;
          break;
        case 2:
          Settings.Quality = YouTubeQuality.HQ;
          break;
        case 3:
          Settings.Quality = YouTubeQuality.SD;
          break;
        case 4:
          Settings.Quality = YouTubeQuality.HD60;
          break;
        case 5:
          Settings.Quality = YouTubeQuality.HD1080p60;
          break;
        case 6:
          Settings.Quality = YouTubeQuality.HD1440;
          break;
        case 7:
          Settings.Quality = YouTubeQuality.HD2160;
          break;
      }
    }

    private async void cipherButton_Click(object sender, RoutedEventArgs e)
    {
      ((Control) this.cipherButton).put_IsEnabled(false);
      TextBlock textBlock = this.cipher;
      string str = await App.UpdateCipher(0.0);
      textBlock.put_Text(str);
      textBlock = (TextBlock) null;
      ((Control) this.cipherButton).put_IsEnabled(true);
    }

    private async void newUserButton_Click(object sender, RoutedEventArgs e)
    {
      ((Control) this.newUserButton).put_IsEnabled(false);
      Settings.RykenUserID = (string) null;
      await App.CheckRykenUser();
      ((Control) this.newUserButton).put_IsEnabled(true);
    }

    private void rotationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (Settings.RotationType == (RotationType) ((Selector) this.rotationBox).SelectedIndex)
        return;
      Settings.RotationType = (RotationType) ((Selector) this.rotationBox).SelectedIndex;
      DefaultPage.Current.SetOrientationType();
    }

    private void colorScheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!(((Selector) this.colorScheme).SelectedItem is ColorSchemes))
        return;
      Settings.ColorCheme = (ColorSchemes) ((Selector) this.colorScheme).SelectedItem;
      App.Instance.ApplyTheme((ColorSchemes) ((Selector) this.colorScheme).SelectedItem);
      DefaultPage.Current.SetTheme(Settings.Theme);
    }

    private void prefer60FPS_Checked(object sender, RoutedEventArgs e) => Settings.Allow60FPS = true;

    private void prefer60FPS_Unchecked(object sender, RoutedEventArgs e) => Settings.Allow60FPS = false;

    private void resumeAsAudio_Checked(object sender, RoutedEventArgs e) => Settings.ResumeAsAudio = true;

    private void resumeAsAudio_Unchecked(object sender, RoutedEventArgs e) => Settings.ResumeAsAudio = false;

    private void useNavigationPage_Checked(object sender, RoutedEventArgs e) => Settings.UseNavigatePage = true;

    private void useNavigationPage_Unchecked(object sender, RoutedEventArgs e) => Settings.UseNavigatePage = false;

    private void region_SelectionChanged(object sender, SelectionChangedEventArgs e) => Settings.Region = YouTube.Region = RegionInfo.GetCountries()[((Selector) this.region).SelectedIndex];

    private void transparentTile_Unchecked(object sender, RoutedEventArgs e)
    {
      Settings.TileStyle = TileStyle.Default;
      TileHelper.ResetMainTile();
    }

    private void transparentTile_Checked(object sender, RoutedEventArgs e)
    {
      Settings.TileStyle = TileStyle.Transparent;
      TileHelper.SetMainTileImages("ms-appx:///Assets/SmallTileT.png", "ms-appx:///Assets/TileT.png", "ms-appx:///Assets/LargeTileT.png", TileUpdateManager.GetTemplateContent((TileTemplateType) 78));
    }

    private async void sendFiles_Click(object sender, RoutedEventArgs e)
    {
      List<string> values = await Helper.ListLocalFiles();
      if (values.Count <= 0)
        return;
      StringUploadPopup.Show(string.Join("\n", (IEnumerable<string>) values));
    }

    private void playAutomatically_Checked(object sender, RoutedEventArgs e) => Settings.PlayAutomatically = true;

    private void playAutomatically_Unchecked(object sender, RoutedEventArgs e) => Settings.PlayAutomatically = false;

    private void sendAccount_Click(object sender, RoutedEventArgs e)
    {
      if (!YouTube.IsSignedIn)
        return;
      StringUploadPopup.Show("ID: " + YouTube.UserInfo.ID + "\nRefresh Token: " + YouTube.RefreshToken);
    }

    private void sendAccountCollection_Click(object sender, RoutedEventArgs e) => StringUploadPopup.Show(SharedSettings.Accounts.ToSaveString());

    private void exceptionButton_Click(object sender, RoutedEventArgs e) => throw new Exception("Test exception for myTube... Ignore me!");

    private void allowNotifications_Checked(object sender, RoutedEventArgs e) => SharedSettings.NotificationsOnlyOnWifi = false;

    private void allowNotifications_Unchecked(object sender, RoutedEventArgs e) => SharedSettings.NotificationsOnlyOnWifi = true;

    private void warnData_Unchecked(object sender, RoutedEventArgs e) => Settings.WarnOnData = false;

    private void warnData_Checked(object sender, RoutedEventArgs e) => Settings.WarnOnData = true;

    private void afterVideoBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => Settings.AfterVideoSection = (AfterVideoSection) ((Selector) this.afterVideoBox).SelectedIndex;

    private void playbackLocationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      MiniPlayerType miniPlayerType = MiniPlayerType.MiniPlayer;
      if (((Selector) this.playbackLocationBox).SelectedIndex == 1)
        miniPlayerType = MiniPlayerType.Background;
      if (miniPlayerType == Settings.MiniPlayerType)
        return;
      Settings.MiniPlayerType = miniPlayerType;
      DefaultPage.Current.MiniPlayerTypeChanged();
    }

    private void searchButton_Checked(object sender, RoutedEventArgs e)
    {
      Settings.SearchInAppBar = true;
      DefaultPage.Current.SetSearchInAppBar(Settings.SearchInAppBar);
    }

    private void searchButton_Unchecked(object sender, RoutedEventArgs e)
    {
      Settings.SearchInAppBar = false;
      DefaultPage.Current.SetSearchInAppBar(Settings.SearchInAppBar);
    }

    private void blurVideo_Checked(object sender, RoutedEventArgs e) => Settings.BlurVideo = true;

    private void blurVideo_Unchecked(object sender, RoutedEventArgs e) => Settings.BlurVideo = false;

    private async void refreshTokenButton_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrWhiteSpace(this.refreshTokenBox.Text))
        return;
      UserInfo userInfo = await YouTube.RefreshSignIn(this.refreshTokenBox.Text);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///SettingsPages/GeneralSettings.xaml"), (ComponentResourceLocation) 0);
      this.genaralPanel = (StackPanel) ((FrameworkElement) this).FindName("genaralPanel");
      this.themePanel = (StackPanel) ((FrameworkElement) this).FindName("themePanel");
      this.playbackPanel = (StackPanel) ((FrameworkElement) this).FindName("playbackPanel");
      this.betaPanel = (StackPanel) ((FrameworkElement) this).FindName("betaPanel");
      this.ownerPanel = (StackPanel) ((FrameworkElement) this).FindName("ownerPanel");
      this.newUserButton = (Button) ((FrameworkElement) this).FindName("newUserButton");
      this.exceptionButton = (Button) ((FrameworkElement) this).FindName("exceptionButton");
      this.cipher = (TextBlock) ((FrameworkElement) this).FindName("cipher");
      this.user = (TextBlock) ((FrameworkElement) this).FindName("user");
      this.cipherButton = (Button) ((FrameworkElement) this).FindName("cipherButton");
      this.backgroundTask = (TextBlock) ((FrameworkElement) this).FindName("backgroundTask");
      this.backgroundTaskException = (TextBlock) ((FrameworkElement) this).FindName("backgroundTaskException");
      this.startTime = (TextBlock) ((FrameworkElement) this).FindName("startTime");
      this.useNavigationPage = (CheckBox) ((FrameworkElement) this).FindName("useNavigationPage");
      this.sendFiles = (Button) ((FrameworkElement) this).FindName("sendFiles");
      this.sendAccount = (Button) ((FrameworkElement) this).FindName("sendAccount");
      this.sendAccountCollection = (Button) ((FrameworkElement) this).FindName("sendAccountCollection");
      this.notificationTaskMessage = (TextBlock) ((FrameworkElement) this).FindName("notificationTaskMessage");
      this.refreshTokenBox = (TextBox) ((FrameworkElement) this).FindName("refreshTokenBox");
      this.refreshTokenButton = (Button) ((FrameworkElement) this).FindName("refreshTokenButton");
      this.qualityPicker = (ComboBox) ((FrameworkElement) this).FindName("qualityPicker");
      this.afterVideoBox = (ComboBox) ((FrameworkElement) this).FindName("afterVideoBox");
      this.playbackLocationBox = (ComboBox) ((FrameworkElement) this).FindName("playbackLocationBox");
      this.playAutomatically = (CheckBox) ((FrameworkElement) this).FindName("playAutomatically");
      this.prefer60FPS = (CheckBox) ((FrameworkElement) this).FindName("prefer60FPS");
      this.blurVideo = (CheckBox) ((FrameworkElement) this).FindName("blurVideo");
      this.resumeAsAudio = (CheckBox) ((FrameworkElement) this).FindName("resumeAsAudio");
      this.warnData = (CheckBox) ((FrameworkElement) this).FindName("warnData");
      this.themePicker = (ComboBox) ((FrameworkElement) this).FindName("themePicker");
      this.colorScheme = (ComboBox) ((FrameworkElement) this).FindName("colorScheme");
      this.thumbnailPicker = (ComboBox) ((FrameworkElement) this).FindName("thumbnailPicker");
      this.transparentTile = (CheckBox) ((FrameworkElement) this).FindName("transparentTile");
      this.rotationBox = (ComboBox) ((FrameworkElement) this).FindName("rotationBox");
      this.region = (ComboBox) ((FrameworkElement) this).FindName("region");
      this.searchButton = (CheckBox) ((FrameworkElement) this).FindName("searchButton");
      this.allowNotifications = (CheckBox) ((FrameworkElement) this).FindName("allowNotifications");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.newUserButton_Click));
          break;
        case 2:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.exceptionButton_Click));
          break;
        case 3:
          ButtonBase buttonBase3 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase3.add_Click), new Action<EventRegistrationToken>(buttonBase3.remove_Click), new RoutedEventHandler(this.cipherButton_Click));
          break;
        case 4:
          ToggleButton toggleButton1 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton1.add_Checked), new Action<EventRegistrationToken>(toggleButton1.remove_Checked), new RoutedEventHandler(this.useNavigationPage_Checked));
          ToggleButton toggleButton2 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton2.add_Unchecked), new Action<EventRegistrationToken>(toggleButton2.remove_Unchecked), new RoutedEventHandler(this.useNavigationPage_Unchecked));
          break;
        case 5:
          ButtonBase buttonBase4 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase4.add_Click), new Action<EventRegistrationToken>(buttonBase4.remove_Click), new RoutedEventHandler(this.sendFiles_Click));
          break;
        case 6:
          ButtonBase buttonBase5 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase5.add_Click), new Action<EventRegistrationToken>(buttonBase5.remove_Click), new RoutedEventHandler(this.sendAccount_Click));
          break;
        case 7:
          ButtonBase buttonBase6 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase6.add_Click), new Action<EventRegistrationToken>(buttonBase6.remove_Click), new RoutedEventHandler(this.sendAccountCollection_Click));
          break;
        case 8:
          ButtonBase buttonBase7 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase7.add_Click), new Action<EventRegistrationToken>(buttonBase7.remove_Click), new RoutedEventHandler(this.refreshTokenButton_Click));
          break;
        case 9:
          Selector selector1 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector1.add_SelectionChanged), new Action<EventRegistrationToken>(selector1.remove_SelectionChanged), new SelectionChangedEventHandler(this.qualityPicker_SelectionChanged));
          break;
        case 10:
          Selector selector2 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector2.add_SelectionChanged), new Action<EventRegistrationToken>(selector2.remove_SelectionChanged), new SelectionChangedEventHandler(this.afterVideoBox_SelectionChanged));
          break;
        case 11:
          Selector selector3 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector3.add_SelectionChanged), new Action<EventRegistrationToken>(selector3.remove_SelectionChanged), new SelectionChangedEventHandler(this.playbackLocationBox_SelectionChanged));
          break;
        case 12:
          ToggleButton toggleButton3 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton3.add_Checked), new Action<EventRegistrationToken>(toggleButton3.remove_Checked), new RoutedEventHandler(this.playAutomatically_Checked));
          ToggleButton toggleButton4 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton4.add_Unchecked), new Action<EventRegistrationToken>(toggleButton4.remove_Unchecked), new RoutedEventHandler(this.playAutomatically_Unchecked));
          break;
        case 13:
          ToggleButton toggleButton5 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton5.add_Checked), new Action<EventRegistrationToken>(toggleButton5.remove_Checked), new RoutedEventHandler(this.prefer60FPS_Checked));
          ToggleButton toggleButton6 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton6.add_Unchecked), new Action<EventRegistrationToken>(toggleButton6.remove_Unchecked), new RoutedEventHandler(this.prefer60FPS_Unchecked));
          break;
        case 14:
          ToggleButton toggleButton7 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton7.add_Checked), new Action<EventRegistrationToken>(toggleButton7.remove_Checked), new RoutedEventHandler(this.blurVideo_Checked));
          ToggleButton toggleButton8 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton8.add_Unchecked), new Action<EventRegistrationToken>(toggleButton8.remove_Unchecked), new RoutedEventHandler(this.blurVideo_Unchecked));
          break;
        case 15:
          ToggleButton toggleButton9 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton9.add_Checked), new Action<EventRegistrationToken>(toggleButton9.remove_Checked), new RoutedEventHandler(this.resumeAsAudio_Checked));
          ToggleButton toggleButton10 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton10.add_Unchecked), new Action<EventRegistrationToken>(toggleButton10.remove_Unchecked), new RoutedEventHandler(this.resumeAsAudio_Unchecked));
          break;
        case 16:
          ToggleButton toggleButton11 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton11.add_Unchecked), new Action<EventRegistrationToken>(toggleButton11.remove_Unchecked), new RoutedEventHandler(this.warnData_Unchecked));
          ToggleButton toggleButton12 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton12.add_Checked), new Action<EventRegistrationToken>(toggleButton12.remove_Checked), new RoutedEventHandler(this.warnData_Checked));
          break;
        case 17:
          Selector selector4 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector4.add_SelectionChanged), new Action<EventRegistrationToken>(selector4.remove_SelectionChanged), new SelectionChangedEventHandler(this.ListBox_SelectionChanged));
          break;
        case 18:
          Selector selector5 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector5.add_SelectionChanged), new Action<EventRegistrationToken>(selector5.remove_SelectionChanged), new SelectionChangedEventHandler(this.colorScheme_SelectionChanged));
          break;
        case 19:
          Selector selector6 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector6.add_SelectionChanged), new Action<EventRegistrationToken>(selector6.remove_SelectionChanged), new SelectionChangedEventHandler(this.thumbnailPicker_SelectionChanged));
          break;
        case 20:
          ToggleButton toggleButton13 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton13.add_Unchecked), new Action<EventRegistrationToken>(toggleButton13.remove_Unchecked), new RoutedEventHandler(this.transparentTile_Unchecked));
          ToggleButton toggleButton14 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton14.add_Checked), new Action<EventRegistrationToken>(toggleButton14.remove_Checked), new RoutedEventHandler(this.transparentTile_Checked));
          break;
        case 21:
          Selector selector7 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector7.add_SelectionChanged), new Action<EventRegistrationToken>(selector7.remove_SelectionChanged), new SelectionChangedEventHandler(this.rotationBox_SelectionChanged));
          break;
        case 22:
          Selector selector8 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector8.add_SelectionChanged), new Action<EventRegistrationToken>(selector8.remove_SelectionChanged), new SelectionChangedEventHandler(this.region_SelectionChanged));
          break;
        case 23:
          ToggleButton toggleButton15 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton15.add_Checked), new Action<EventRegistrationToken>(toggleButton15.remove_Checked), new RoutedEventHandler(this.searchButton_Checked));
          ToggleButton toggleButton16 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton16.add_Unchecked), new Action<EventRegistrationToken>(toggleButton16.remove_Unchecked), new RoutedEventHandler(this.searchButton_Unchecked));
          break;
        case 24:
          ToggleButton toggleButton17 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton17.add_Checked), new Action<EventRegistrationToken>(toggleButton17.remove_Checked), new RoutedEventHandler(this.allowNotifications_Checked));
          ToggleButton toggleButton18 = (ToggleButton) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(toggleButton18.add_Unchecked), new Action<EventRegistrationToken>(toggleButton18.remove_Unchecked), new RoutedEventHandler(this.allowNotifications_Unchecked));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
