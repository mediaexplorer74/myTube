// Decompiled with JetBrains decompiler
// Type: myTube.SearchPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Popups;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class SearchPage : Page, IComponentConnector
  {
    private SearchClient searchClient;
    private SearchPlaylistClient playlistClient;
    private ObservableCollection<string> suggestions;
    private SearchUsersClient userClient;
    private bool suggestionsShown;
    private string channelId;
    private bool searching;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CompositeTransform suggestionsTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserInfoCollection userInfoCollection;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid channelGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBox searchBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl settingsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListBox suggestionsBox;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock channelName;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl unsetChannelButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList videoList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer channelScroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PlaylistList playlists;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public SearchPage()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.SearchPage_Loaded));
      SearchClient searchClient1 = new SearchClient(50);
      searchClient1.APIKey = App.GetAPIKey(0);
      this.searchClient = searchClient1;
      SearchClient searchClient2 = this.searchClient;
      YouTubeEntryClient youTubeEntryClient = new YouTubeEntryClient();
      youTubeEntryClient.APIKey = App.GetAPIKey(1);
      searchClient2.RefreshClientOverride = (EntryClient<YouTubeEntry>) youTubeEntryClient;
      SearchUsersClient searchUsersClient = new SearchUsersClient(40);
      searchUsersClient.APIKey = App.GetAPIKey(4);
      searchUsersClient.RefreshInternally = false;
      searchUsersClient.UseAccessToken = false;
      this.userClient = searchUsersClient;
      this.videoList.Client = (VideoListClient) this.searchClient;
      SearchPlaylistClient searchPlaylistClient = new SearchPlaylistClient(20);
      searchPlaylistClient.APIKey = App.GetAPIKey(3);
      this.playlistClient = searchPlaylistClient;
      this.suggestions = new ObservableCollection<string>();
      this.playlists.Client = (YouTubeClient<PlaylistEntry>) this.playlistClient;
      ((ItemsControl) this.suggestionsBox).put_ItemsSource((object) this.suggestions);
      ((UIElement) this.suggestionsBox).put_Visibility((Visibility) 1);
      this.videoList.ListStrings.Default = "search away!";
    }

    protected virtual void OnKeyDown(KeyRoutedEventArgs e)
    {
      int num1 = 0;
      if (this.suggestionsShown)
      {
        if (e.Key == 40)
          num1 = 1;
        else if (e.Key == 38)
          num1 = -1;
        if (num1 != 0)
        {
          int num2 = ((Selector) this.suggestionsBox).SelectedIndex + num1;
          if (num2 < 0)
            num2 = 0;
          if (num2 > ((Collection<string>) this.suggestions).Count - 1)
            num2 = ((Collection<string>) this.suggestions).Count - 1;
          ((Selector) this.suggestionsBox).put_SelectedIndex(num2);
          this.suggestionsBox.ScrollIntoView(((Selector) this.suggestionsBox).SelectedItem);
        }
      }
      ((Control) this).OnKeyDown(e);
    }

    private void showSuggestions()
    {
      if (this.suggestionsShown)
        return;
      this.suggestionsShown = true;
      ((Selector) this.suggestionsBox).put_SelectedIndex(-1);
      ((UIElement) this.suggestionsBox).put_Opacity(0.0);
      ((UIElement) this.suggestionsBox).put_Visibility((Visibility) 0);
      this.suggestionsTrans.put_ScaleY(0.6);
      Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.suggestionsBox, "Opacity", 1.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.suggestionsTrans, "ScaleY", 1.0, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 3.0)));
    }

    private void hideSuggestions()
    {
      if (!this.suggestionsShown)
        return;
      this.suggestionsShown = false;
      Storyboard storyboard = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.suggestionsBox, "Opacity", 0.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.suggestionsTrans, "ScaleY", 0.5, 0.2, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 4.0)));
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) =>
      {
        if (!this.suggestionsShown)
        {
          ((UIElement) this.suggestionsBox).put_Visibility((Visibility) 1);
          ((Selector) this.suggestionsBox).put_SelectedIndex(-1);
        }
        else
        {
          this.suggestionsShown = false;
          this.showSuggestions();
        }
      }));
    }

    private void mergeCollection(string[] sugg)
    {
      for (int index = 0; index < ((Collection<string>) this.suggestions).Count; ++index)
      {
        if (!Enumerable.Contains<string>((IEnumerable<string>) sugg, ((Collection<string>) this.suggestions)[index]))
        {
          ((Collection<string>) this.suggestions).RemoveAt(index);
          --index;
        }
      }
      for (int index1 = 0; index1 < sugg.Length; ++index1)
      {
        if (!((Collection<string>) this.suggestions).Contains(sugg[index1]))
        {
          if (index1 == 0)
            ((Collection<string>) this.suggestions).Insert(0, sugg[index1]);
          else if (index1 == sugg.Length - 1)
          {
            ((Collection<string>) this.suggestions).Add(sugg[index1]);
          }
          else
          {
            int num = ((Collection<string>) this.suggestions).IndexOf(sugg[index1 - 1]);
            if (num != -1)
            {
              ((Collection<string>) this.suggestions).Insert(num + 1, sugg[index1]);
            }
            else
            {
              int index2 = ((Collection<string>) this.suggestions).IndexOf(sugg[index1 + 1]);
              if (index2 != -1)
                ((Collection<string>) this.suggestions).Insert(index2, sugg[index1]);
            }
          }
        }
      }
    }

    private void SearchPage_Loaded(object sender, RoutedEventArgs e)
    {
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1)
      {
        if (e.Parameter is string)
        {
          this.searchBox.put_Text(e.Parameter as string);
          await this.Search();
        }
        else
        {
          await Task.Delay(100);
          ((Control) this.searchBox).Focus((FocusState) 2);
          if (this.searchBox.Text.Length > 0)
            this.searchBox.SelectAll();
          this.overCanvas.ScrollToPage(0, true);
        }
      }
      if (e.Parameter is UserInfo)
        this.SetChannel(e.Parameter as UserInfo);
      else
        this.UnsetChannel();
      base.OnNavigatedTo(e);
    }

    private async void searchBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
      Helper.Write((object) ("Key down on search box: " + (object) e.Key));
      VirtualKey key = e.Key;
      if (key == 37 || key == 39)
        e.put_Handled(true);
      if (this.searching || e.Key != 13)
        return;
      this.searching = true;
      e.put_Handled(true);
      if (((Selector) this.suggestionsBox).SelectedItem != null)
        this.searchBox.put_Text(((Selector) this.suggestionsBox).SelectedItem.ToString());
      this.searchBox.put_Text(this.searchBox.Text.Trim());
      await this.Search();
      this.searching = false;
    }

    private async Task Search(bool videosOnly = false)
    {
      this.hideSuggestions();
      string loweredSearch = this.searchBox.Text.ToLower();
      UserMode mode = UserMode.Normal;
      if (loweredSearch.Contains(App.GetPassword(mode = UserMode.Beta)) || loweredSearch.Contains(App.GetPassword(mode = UserMode.Owner)))
      {
        if (mode == Settings.UserMode)
        {
          Settings.UserMode = UserMode.Normal;
          IUICommand iuiCommand = await new MessageDialog("User mode reset back to normal", "User mode changed").ShowAsync();
        }
        else
        {
          Settings.UserMode = mode;
          IUICommand iuiCommand = await new MessageDialog("User mode changed to: " + (object) mode, "User mode changed").ShowAsync();
        }
        await App.CheckRykenUser();
      }
      else if (loweredSearch.Contains("rykentubepaid"))
      {
        Settings.WasPaidFor = true;
        IUICommand iuiCommand = await new MessageDialog("Paid mode enabled.", "Payment mode changed").ShowAsync();
      }
      else
      {
        if (!videosOnly)
        {
          ((Collection<UserInfo>) this.userInfoCollection).Clear();
          this.playlists.Clear();
        }
        ((Control) this.searchBox).put_IsEnabled(false);
        this.searchClient.SearchTerm = this.searchBox.Text;
        this.userClient.SearchTerm = this.searchBox.Text;
        this.playlistClient.SearchTerm = this.searchBox.Text;
        this.searchClient.ChannelId = this.channelId;
        this.videoList.Clear(true);
        if (this.overCanvas.IsSelected((UIElement) this.videoList))
          await this.videoList.Load();
        ((Control) this.searchBox).put_IsEnabled(true);
        if (videosOnly)
          return;
        try
        {
          if (this.overCanvas.IsSelected((UIElement) this.playlists))
          {
            this.playlistClient.ChannelID = this.channelId;
            this.playlists.Load();
          }
        }
        catch
        {
        }
        try
        {
          if (!this.overCanvas.IsSelected((UIElement) this.channelScroll))
            return;
          UserInfo[] feed = await this.userClient.GetFeed(0);
          if (feed == null)
            return;
          foreach (UserInfo userInfo in feed)
            ((Collection<UserInfo>) this.userInfoCollection).Add(userInfo);
        }
        catch
        {
        }
      }
    }

    private void overCanvas_ShownChanged(object sender, bool e)
    {
      if (e)
      {
        Ani.Begin((DependencyObject) this.searchBox, "Opacity", 1.0, 0.2);
        Ani.Begin((DependencyObject) this.settingsControl, "Opacity", 1.0, 0.2);
        ((UIElement) this.searchBox).put_IsHitTestVisible(true);
        ((UIElement) this.settingsControl).put_IsHitTestVisible(true);
      }
      else
      {
        Ani.Begin((DependencyObject) this.searchBox, "Opacity", 0.0, 0.2);
        Ani.Begin((DependencyObject) this.settingsControl, "Opacity", 0.0, 0.2);
        ((UIElement) this.searchBox).put_IsHitTestVisible(false);
        ((UIElement) this.settingsControl).put_IsHitTestVisible(false);
      }
    }

    private void suggestionsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
    }

    private void suggestionsBox_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (((Selector) this.suggestionsBox).SelectedIndex == -1)
        return;
      this.searchBox.put_Text(((Selector) this.suggestionsBox).SelectedItem as string);
      this.Search();
      ((Selector) this.suggestionsBox).put_SelectedIndex(-1);
    }

    private async void searchBox_GotFocus(object sender, RoutedEventArgs e)
    {
      if (this.searchBox.Text.Length <= 0)
        return;
      try
      {
        string[] suggestions = await this.searchClient.GetSuggestions(this.searchBox.Text);
        this.mergeCollection(suggestions);
        if (suggestions.Length == 0 || this.searchBox.Text.Length <= 0)
          return;
        this.showSuggestions();
      }
      catch (Exception ex)
      {
      }
    }

    private void searchBox_LostFocus(object sender, RoutedEventArgs e) => this.hideSuggestions();

    private async void settingsControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      SearchFilterUI settings = new SearchFilterUI()
      {
        UploadDate = this.searchClient.Time,
        OrderBy = this.searchClient.OrderBy
      };
      Rect bounds1 = Window.Current.Bounds;
      Rect bounds2 = ((FrameworkElement) this.settingsControl).GetBounds((UIElement) DefaultPage.Current);
      ((FrameworkElement) settings).put_Width(Math.Min(bounds1.Width - 38.0, 360.0));
      ScaleTransform Element = new ScaleTransform();
      ((UIElement) settings).put_Opacity(0.1);
      ((UIElement) settings).put_RenderTransform((Transform) Element);
      ((UIElement) settings).put_RenderTransformOrigin(new Point(0.95, 0.05));
      Point position = new Point();
      position.Y = bounds1.Height > 400.0 ? bounds2.Top : 0.0;
      position.X = bounds2.Right - ((FrameworkElement) settings).Width;
      ((FrameworkElement) settings).put_MaxHeight(Math.Min(bounds1.Height - position.Y - 19.0, 700.0));
      Popup popup1 = new Popup();
      popup1.put_Child((UIElement) settings);
      ((FrameworkElement) popup1).put_RequestedTheme(App.Theme);
      Popup popup2 = popup1;
      ((FrameworkElement) popup2).put_Height(150.0);
      Storyboard storyboard = Ani.Animation(Ani.DoubleAni((DependencyObject) Element, "ScaleX", 1.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)), Ani.DoubleAni((DependencyObject) Element, "ScaleY", 1.0, 0.3, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)), Ani.DoubleAni((DependencyObject) settings, "Opacity", 1.0, 0.1));
      Storyboard closeAnimation = Ani.Animation(Ani.DoubleAni((DependencyObject) Element, "ScaleX", 0.5, 0.1, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)), Ani.DoubleAni((DependencyObject) Element, "ScaleY", 0.75, 0.1, (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)), Ani.DoubleAni((DependencyObject) settings, "Opacity", 0.0, 0.1));
      Task<bool> task = DefaultPage.Current.ShowPopup(popup2, position, new Point(0.0, 0.0), FadeType.Half, closeAnimation: closeAnimation);
      Element.put_ScaleX(0.5);
      Element.put_ScaleY(0.75);
      storyboard.Begin();
      int num = await task ? 1 : 0;
      bool flag = false;
      if (this.searchClient.Time != settings.UploadDate || this.searchClient.OrderBy != settings.OrderBy)
        flag = true;
      this.searchClient.OrderBy = settings.OrderBy;
      this.searchClient.Time = settings.UploadDate;
      if (!flag)
        return;
      this.Search(true);
    }

    private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement) || !(frameworkElement.DataContext is UserInfo dataContext))
        return;
      App.Instance.RootFrame.Navigate(typeof (ChannelPage), (object) dataContext.ID);
    }

    private async void SetChannel(UserInfo info)
    {
      if (!(this.channelId != info.ID))
        return;
      this.channelName.put_Text(info.UserDisplayName);
      ((UIElement) this.channelGrid).put_Visibility((Visibility) 0);
      this.channelId = info.ID;
      ((FrameworkElement) this.scroll).put_Margin(new Thickness(0.0, 123.5, 0.0, 0.0));
      this.videoList.Clear(true);
      await Task.Delay(300);
      this.playlists.Clear();
      ((Collection<UserInfo>) this.userInfoCollection).Clear();
    }

    private void UnsetChannel()
    {
      ((UIElement) this.channelGrid).put_Visibility((Visibility) 1);
      this.channelId = (string) null;
      ((FrameworkElement) this.scroll).put_Margin(new Thickness(0.0, 71.5, 0.0, 0.0));
    }

    private async void searchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.searching)
        return;
      try
      {
        if (this.searchBox.Text.Length > 0)
        {
          this.searchClient.CancelSuggestions();
          string[] suggestions = await this.searchClient.GetSuggestions(this.searchBox.Text);
          this.mergeCollection(suggestions);
          if (suggestions.Length != 0 && this.searchBox.Text.Length > 0 && !this.searching && ((Control) this.searchBox).IsEnabled)
            this.showSuggestions();
          else
            this.hideSuggestions();
          ((Selector) this.suggestionsBox).put_SelectedIndex(-1);
        }
        else
          this.hideSuggestions();
      }
      catch
      {
      }
    }

    private void unsetChannelButton_Tapped(object sender, TappedRoutedEventArgs e) => this.UnsetChannel();

    private async void overCanvas_SelectedPageChanged(
      object sender,
      OnSelectedPageChangedEventArgs e)
    {
      if (this.overCanvas.IsSelected((UIElement) this.videoList) && !string.IsNullOrWhiteSpace(this.searchClient.SearchTerm) && ((Collection<YouTubeEntry>) this.videoList.Entries).Count == 0)
        await this.videoList.Load();
      if (this.overCanvas.IsSelected((UIElement) this.playlists) && !string.IsNullOrWhiteSpace(this.playlistClient.SearchTerm) && ((Collection<PlaylistEntry>) this.playlists.Entries).Count == 0)
        await this.playlists.Load();
      if (!this.overCanvas.IsSelected((UIElement) this.channelScroll) || string.IsNullOrWhiteSpace(this.userClient.SearchTerm) || ((Collection<UserInfo>) this.userInfoCollection).Count != 0)
        return;
      UserInfo[] feed = await this.userClient.GetFeed(0);
      if (feed == null)
        return;
      foreach (UserInfo userInfo in feed)
        ((Collection<UserInfo>) this.userInfoCollection).Add(userInfo);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///SearchPage.xaml"), (ComponentResourceLocation) 0);
      this.suggestionsTrans = (CompositeTransform) ((FrameworkElement) this).FindName("suggestionsTrans");
      this.userInfoCollection = (UserInfoCollection) ((FrameworkElement) this).FindName("userInfoCollection");
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.channelGrid = (Grid) ((FrameworkElement) this).FindName("channelGrid");
      this.searchBox = (TextBox) ((FrameworkElement) this).FindName("searchBox");
      this.settingsControl = (ContentControl) ((FrameworkElement) this).FindName("settingsControl");
      this.suggestionsBox = (ListBox) ((FrameworkElement) this).FindName("suggestionsBox");
      this.channelName = (TextBlock) ((FrameworkElement) this).FindName("channelName");
      this.unsetChannelButton = (ContentControl) ((FrameworkElement) this).FindName("unsetChannelButton");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.videoList = (VideoList) ((FrameworkElement) this).FindName("videoList");
      this.channelScroll = (ScrollViewer) ((FrameworkElement) this).FindName("channelScroll");
      this.playlists = (PlaylistList) ((FrameworkElement) this).FindName("playlists");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.Grid_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>(new Func<KeyEventHandler, EventRegistrationToken>(uiElement2.add_KeyDown), new Action<EventRegistrationToken>(uiElement2.remove_KeyDown), new KeyEventHandler(this.searchBox_KeyDown));
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement3.add_GotFocus), new Action<EventRegistrationToken>(uiElement3.remove_GotFocus), new RoutedEventHandler(this.searchBox_GotFocus));
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(uiElement4.add_LostFocus), new Action<EventRegistrationToken>(uiElement4.remove_LostFocus), new RoutedEventHandler(this.searchBox_LostFocus));
          TextBox textBox = (TextBox) target;
          WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>(new Func<TextChangedEventHandler, EventRegistrationToken>(textBox.add_TextChanged), new Action<EventRegistrationToken>(textBox.remove_TextChanged), new TextChangedEventHandler(this.searchBox_TextChanged));
          break;
        case 3:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.settingsControl_Tapped));
          break;
        case 4:
          Selector selector = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector.add_SelectionChanged), new Action<EventRegistrationToken>(selector.remove_SelectionChanged), new SelectionChangedEventHandler(this.suggestionsBox_SelectionChanged));
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.suggestionsBox_Tapped));
          break;
        case 5:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement7.add_Tapped), new Action<EventRegistrationToken>(uiElement7.remove_Tapped), new TappedEventHandler(this.unsetChannelButton_Tapped));
          break;
        case 6:
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
          ((OverCanvas) target).SelectedPageChanged += new EventHandler<OnSelectedPageChangedEventArgs>(this.overCanvas_SelectedPageChanged);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
