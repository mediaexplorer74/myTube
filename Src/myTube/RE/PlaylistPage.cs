// Decompiled with JetBrains decompiler
// Type: myTube.PlaylistPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
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
  public sealed class PlaylistPage : Page, IComponentConnector
  {
    private string lastID;
    private IconButtonEvent remove;
    private bool originallyBookmarked;
    private string lastPlaylistID;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CommandBar appBar;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton editButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton sortButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton pinButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton bookmarkButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList list;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList offline;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public PlaylistPage()
    {
      IconButtonEvent iconButtonEvent = new IconButtonEvent();
      iconButtonEvent.Symbol = (Symbol) 57608;
      this.remove = iconButtonEvent;
      this.lastPlaylistID = "";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.InitializeComponent();
      this.remove.Text = App.Strings["common.remove", nameof (remove)].ToLower();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(PlaylistPage_DataContextChanged)));
      this.offline.ListStrings.NoItems = this.offline.ListStrings.Default = App.Strings["playlists.offline.novideos", "save videos from this playlist to create an offline version of it"].ToLower();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.PlaylistPage_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.PlaylistPage_Unloaded));
      this.remove.Selected += new EventHandler<IconButtonEventArgs>(this.remove_Selected);
      this.list.LoadFailed += new EventHandler<Exception>(this.list_LoadFailed);
    }

    private async void list_LoadFailed(object sender, Exception e)
    {
      await Task.Delay(250);
      if (((UIElement) this.offline).Visibility == 1)
        return;
      if (((Collection<YouTubeEntry>) this.offline.Entries).Count > 0)
      {
        this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.offline), false);
      }
      else
      {
        await Task.Delay(300);
        if (((Collection<YouTubeEntry>) this.offline.Entries).Count <= 0)
          return;
        this.overCanvas.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) this.offline), false);
      }
    }

    private async void remove_Selected(object sender, IconButtonEventArgs e)
    {
      e.Close();
      PlaylistEntry playlist = ((FrameworkElement) this).DataContext as PlaylistEntry;
      if (playlist != null && e.OriginalSender is FrameworkElement originalSender1)
      {
        YouTubeEntry ent = originalSender1.DataContext as YouTubeEntry;
        if (ent != null && ent.PlaylistID != null)
        {
          int index = ((Collection<YouTubeEntry>) this.list.Entries).IndexOf(ent);
          if (index != -1)
          {
            ((Collection<YouTubeEntry>) this.list.Entries).RemoveAt(index);
            try
            {
              int num = await YouTube.RemoveFromPlaylist(playlist.ID, ent.PlaylistID) ? 1 : 0;
            }
            catch
            {
              ((Collection<YouTubeEntry>) this.list.Entries).Insert(index, ent);
            }
          }
        }
        ent = (YouTubeEntry) null;
      }
      if (!(e.OriginalSender is IEnumerable<YouTubeEntry> originalSender2))
        return;
      foreach (YouTubeEntry ent in originalSender2)
      {
        if (ent != null && ent.PlaylistID != null)
        {
          int index = ((Collection<YouTubeEntry>) this.list.Entries).IndexOf(ent);
          if (index != -1)
          {
            ((Collection<YouTubeEntry>) this.list.Entries).RemoveAt(index);
            try
            {
              int num = await YouTube.RemoveFromPlaylist(playlist.ID, ent.PlaylistID) ? 1 : 0;
            }
            catch
            {
              ((Collection<YouTubeEntry>) this.list.Entries).Insert(index, ent);
            }
          }
        }
      }
    }

    private async void PlaylistPage_Unloaded(object sender, RoutedEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      App.GlobalObjects.TransferManager.OnAction -= new EventHandler<TransferManagerActionEventArgs>(this.TransferManager_OnAction);
      YouTube.SignedIn -= new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
    }

    private async void PlaylistPage_Loaded(object sender, RoutedEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      App.GlobalObjects.TransferManager.OnAction += new EventHandler<TransferManagerActionEventArgs>(this.TransferManager_OnAction);
      YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
    }

    private async void YouTube_SignedIn(object sender, EventArgs e) => await ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedIn\u003Eb__8_0)));

    private async void TransferManager_OnAction(object sender, TransferManagerActionEventArgs e)
    {
      if (e.VideoID == null || !(((FrameworkElement) this).DataContext is PlaylistEntry dataContext) || ((UIElement) this.offline).Visibility != null)
        return;
      switch (e.Action)
      {
        case TransferManagerAction.Added:
          bool flag = false;
          foreach (ClientDataBase entry in (Collection<YouTubeEntry>) this.offline.Entries)
          {
            if (entry.ID == e.VideoID)
            {
              flag = true;
              break;
            }
          }
          if (flag)
            break;
          OfflinePlaylist offlinePlaylist = App.GlobalObjects.OfflinePlaylistsCollection.GetOfflinePlaylist(dataContext.ID);
          if (offlinePlaylist == null || !Enumerable.Contains<string>((IEnumerable<string>) offlinePlaylist.Videos, e.VideoID))
            break;
          string videoString = await App.GlobalObjects.TransferManager.GetVideoString(e.VideoID);
          if (videoString != null)
          {
            ((Collection<YouTubeEntry>) this.offline.Entries).Add(new YouTubeEntry(videoString));
            break;
          }
          using (IEnumerator<YouTubeEntry> enumerator = ((Collection<YouTubeEntry>) this.list.Entries).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              YouTubeEntry current = enumerator.Current;
              if (current.ID == e.VideoID)
              {
                ((Collection<YouTubeEntry>) this.offline.Entries).Add(current);
                break;
              }
            }
            break;
          }
        case TransferManagerAction.Removed:
          if (((UIElement) this.offline).Visibility != null)
            break;
          for (int index = 0; index < ((Collection<YouTubeEntry>) this.offline.Entries).Count; ++index)
          {
            if (((Collection<YouTubeEntry>) this.offline.Entries)[index].ID == e.VideoID)
            {
              ((Collection<YouTubeEntry>) this.offline.Entries).RemoveAt(index);
              break;
            }
          }
          break;
      }
    }

    private async void PlaylistPage_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      PlaylistEntry entry = ((FrameworkElement) this).DataContext as PlaylistEntry;
      if (this.list.IsSorting)
      {
        AppBarButton sortButton = this.sortButton;
        SymbolIcon symbolIcon = new SymbolIcon();
        symbolIcon.put_Symbol((Symbol) 57716);
        sortButton.put_Icon((IconElement) symbolIcon);
      }
      else
      {
        AppBarButton sortButton = this.sortButton;
        SymbolIcon symbolIcon = new SymbolIcon();
        symbolIcon.put_Symbol((Symbol) 57716);
        sortButton.put_Icon((IconElement) symbolIcon);
      }
      if (entry == null || !(this.lastPlaylistID != entry.ID))
        return;
      AppBarButton sortButton1 = this.sortButton;
      SymbolIcon symbolIcon1 = new SymbolIcon();
      symbolIcon1.put_Symbol((Symbol) 57716);
      sortButton1.put_Icon((IconElement) symbolIcon1);
      this.lastPlaylistID = entry.ID;
      OfflinePlaylist offPlay = App.GlobalObjects.OfflinePlaylistsCollection.GetOfflinePlaylist(entry.ID);
      if (offPlay != null)
      {
        this.overCanvas.ArrangeStyle = ArrangeStyle.Panorama;
        ((UIElement) this.offline).put_Visibility((Visibility) 0);
        YouTubeEntry[] allEntries = await offPlay.GetAllEntries();
        XElement xelement = new XElement((XName) "OfflineVideos");
        foreach (YouTubeEntry youTubeEntry in allEntries)
          xelement.Add((object) new XElement((XName) "video")
          {
            Value = youTubeEntry.OriginalString
          });
        this.offline.Client = (VideoListClient) new OfflinePlaylistClient(WebUtility.UrlEncode(((object) xelement).ToString()), 15);
      }
      if ((entry.PlaylistOfSignedInUser ? 1 : (YouTube.UserInfo == null ? 0 : (YouTube.UserInfo.ID == entry.AuthorID ? 1 : 0))) == 0)
      {
        ((UIElement) this.bookmarkButton).put_Visibility((Visibility) 0);
        if (entry.Bookmarked)
        {
          this.originallyBookmarked = true;
          AppBarButton bookmarkButton = this.bookmarkButton;
          SymbolIcon symbolIcon2 = new SymbolIcon();
          symbolIcon2.put_Symbol((Symbol) 57608);
          bookmarkButton.put_Icon((IconElement) symbolIcon2);
          this.bookmarkButton.put_Label(App.Strings["common.remove", "remove"].ToLower());
        }
        else
        {
          this.originallyBookmarked = true;
          AppBarButton bookmarkButton = this.bookmarkButton;
          SymbolIcon symbolIcon3 = new SymbolIcon();
          symbolIcon3.put_Symbol((Symbol) 57609);
          bookmarkButton.put_Icon((IconElement) symbolIcon3);
          this.bookmarkButton.put_Label(App.Strings["common.bookmark", "bookmark"].ToLower());
        }
        AppBarButton editButton = this.editButton;
        Visibility visibility1;
        ((UIElement) this.sortButton).put_Visibility((Visibility) (int) (visibility1 = (Visibility) 1));
        Visibility visibility2 = visibility1;
        ((UIElement) editButton).put_Visibility(visibility2);
      }
      else
      {
        ((UIElement) this.bookmarkButton).put_Visibility((Visibility) 1);
        AppBarButton editButton = this.editButton;
        Visibility visibility3;
        ((UIElement) this.sortButton).put_Visibility((Visibility) (int) (visibility3 = (Visibility) 0));
        Visibility visibility4 = visibility3;
        ((UIElement) editButton).put_Visibility(visibility4);
      }
      if (offPlay == null)
      {
        this.overCanvas.ArrangeStyle = ArrangeStyle.Pivot;
        ((UIElement) this.offline).put_Visibility((Visibility) 1);
      }
      else
      {
        this.overCanvas.ArrangeStyle = ArrangeStyle.Panorama;
        ((UIElement) this.offline).put_Visibility((Visibility) 0);
      }
      offPlay = (OfflinePlaylist) null;
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter == null)
        return;
      if (e.Parameter is PlaylistEntry)
      {
        PlaylistEntry parameter = e.Parameter as PlaylistEntry;
        if (!(parameter.ID != this.lastID))
          return;
        this.lastID = parameter.ID;
        ((FrameworkElement) this).put_DataContext((object) parameter);
        this.list.Client = (VideoListClient) new PlaylistClient(parameter.ID, 20);
      }
      else
      {
        if (!(e.Parameter is string))
          return;
        TileArgs launchTileArgs = App.GetLaunchTileArgs((object) this);
        if (launchTileArgs != null && launchTileArgs.ShouldSignInFirst)
        {
          try
          {
            UserInfo signInTask = await YouTube.SignInTask;
          }
          catch
          {
          }
        }
        string id = e.Parameter as string;
        if (id != this.lastID)
        {
          this.lastID = id;
          this.list.Client = (VideoListClient) new PlaylistClient(id, 20);
          PlaylistEntryClient playlistEntryClient = new PlaylistEntryClient();
          try
          {
            ((FrameworkElement) this).put_DataContext((object) await playlistEntryClient.GetInfo(id));
          }
          catch
          {
            if (YouTube.IsSignedIn)
            {
              if (id == YouTube.UserInfo.FavoritesPlaylist)
                this.overCanvas.Title = App.Strings["videos.lists.favorites", "favorites"].ToUpper();
            }
          }
        }
        id = (string) null;
      }
    }

    private async void list_VideosLoaded(object sender, YouTubeEntry[] e)
    {
      if (!(((FrameworkElement) this).DataContext is PlaylistEntry))
        return;
      PlaylistEntry dataContext = ((FrameworkElement) this).DataContext as PlaylistEntry;
      if (SharedSettings.CurrentAccount == null || !dataContext.PlaylistOfSignedInUser)
        return;
      List<string> s = new List<string>();
      YouTubeEntry[] youTubeEntryArray = e;
      for (int index = 0; index < youTubeEntryArray.Length; ++index)
      {
        YouTubeEntry v = youTubeEntryArray[index];
        bool flag = false;
        if (this.offline.Client is OfflinePlaylistClient)
        {
          using (List<YouTubeEntry>.Enumerator enumerator = (this.offline.Client as OfflinePlaylistClient).Entries.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              if (enumerator.Current.ID == v.ID)
              {
                flag = true;
                break;
              }
            }
          }
          if (!flag)
          {
            if (await App.GlobalObjects.TransferManager.GetTransferState(v.ID) != TransferManager.State.None)
              ((Collection<YouTubeEntry>) this.offline.Entries).Add(v);
          }
        }
        s.Add(v.ID);
        v = (YouTubeEntry) null;
      }
      youTubeEntryArray = (YouTubeEntry[]) null;
      App.GlobalObjects.OfflinePlaylistsCollection.SetOfflinePlaylistVideos(((FrameworkElement) this).DataContext as PlaylistEntry, false, s.ToArray());
      await App.GlobalObjects.OfflinePlaylistsCollection.Save();
      s = (List<string>) null;
    }

    private void list_SetContextMenu(object sender, IconButtonEventCollection e)
    {
      if (!(((FrameworkElement) this).DataContext is PlaylistEntry dataContext) || !dataContext.PlaylistOfSignedInUser)
        return;
      e.Insert(0, this.remove);
    }

    private void editButton_Click(object sender, RoutedEventArgs e)
    {
      this.overCanvas.ScrollToIndex(0, false);
      AddPlaylistControl control = new AddPlaylistControl()
      {
        Mode = PlaylistEditMode.Edit
      };
      Popup popup1 = new Popup();
      popup1.put_Child((UIElement) control);
      Popup popup2 = popup1;
      ((FrameworkElement) popup2).put_DataContext(((FrameworkElement) this).DataContext);
      ((FrameworkElement) control).put_DataContext(((FrameworkElement) this).DataContext);
      DefaultPage.SetPopupArrangeMethod((DependencyObject) popup2, (Func<Point>) (() =>
      {
        Rect bounds = Window.Current.Bounds;
        ((FrameworkElement) control).put_Width(Math.Min(bounds.Width, 500.0));
        ((FrameworkElement) control).put_Height(bounds.Height);
        return new Point()
        {
          X = Math.Max(bounds.Width - ((FrameworkElement) control).Width - 19.0, (bounds.Width - ((FrameworkElement) control).Width) / 2.0),
          Y = 0.0
        };
      }));
      DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, -150.0));
    }

    private async void pinButton_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is PlaylistEntry))
        return;
      TypeConstructor tc = new TypeConstructor(typeof (PlaylistClient), new object[3]
      {
        (object) (((FrameworkElement) this).DataContext as PlaylistEntry).ID,
        (object) 5,
        null
      });
      tc.Construct();
      SecondaryTile tile = await TileHelper.CreateTile(tc, (((FrameworkElement) this).DataContext as PlaylistEntry).Title, new TileArgs(typeof (PlaylistPage), (((FrameworkElement) this).DataContext as PlaylistEntry).ID, 0)
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
        App.Instance.SetUpBackgroundTask();
      }
      else
        TileHelper.CleanUpFolders();
      DefaultPage.Current.ClosePopup();
      tc = (TypeConstructor) null;
      tile = (SecondaryTile) null;
      nots = (List<TileNotification>) null;
    }

    private void overCanvas_ShownChanged(object sender, bool e) => ((UIElement) this.BottomAppBar).put_Visibility(e ? (Visibility) 0 : (Visibility) 1);

    private async void sortButton_Click(object sender, RoutedEventArgs e)
    {
      if (!this.list.IsSorting)
      {
        this.list.BeginSort();
        AppBarButton sortButton = this.sortButton;
        SymbolIcon symbolIcon = new SymbolIcon();
        symbolIcon.put_Symbol((Symbol) 57611);
        sortButton.put_Icon((IconElement) symbolIcon);
      }
      else
      {
        Dictionary<YouTubeEntry, int> dictionary = this.list.EndSort();
        AppBarButton sortButton = this.sortButton;
        SymbolIcon symbolIcon = new SymbolIcon();
        symbolIcon.put_Symbol((Symbol) 57716);
        sortButton.put_Icon((IconElement) symbolIcon);
        if (((FrameworkElement) this).DataContext is PlaylistEntry playlist)
        {
          Dictionary<YouTubeEntry, int>.Enumerator enumerator = dictionary.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<YouTubeEntry, int> current = enumerator.Current;
              YouTubeEntry key = current.Key;
              int index = current.Value;
              try
              {
                YouTubeEntry youTubeEntry = await YouTube.RearrangePlaylist(playlist.ID, key.PlaylistID, key.ID, index);
              }
              catch
              {
              }
            }
          }
          finally
          {
            enumerator.Dispose();
          }
          enumerator = new Dictionary<YouTubeEntry, int>.Enumerator();
        }
        playlist = (PlaylistEntry) null;
      }
    }

    private async void bookmarkButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private async void bookmarkButton_Click(object sender, RoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is PlaylistEntry ent))
        return;
      ((Control) this.bookmarkButton).put_IsEnabled(false);
      try
      {
        if (await YouTube.BookmarkPlaylist(ent.ID, !ent.Bookmarked))
        {
          ent.Bookmarked = !ent.Bookmarked;
          if (ent.Bookmarked)
          {
            ent.Exists = true;
            AppBarButton bookmarkButton = this.bookmarkButton;
            SymbolIcon symbolIcon = new SymbolIcon();
            symbolIcon.put_Symbol((Symbol) 57608);
            bookmarkButton.put_Icon((IconElement) symbolIcon);
            this.bookmarkButton.put_Label(App.Strings["common.remove", "remove"].ToLower());
          }
          else
          {
            if (this.originallyBookmarked)
              ent.Exists = false;
            AppBarButton bookmarkButton = this.bookmarkButton;
            SymbolIcon symbolIcon = new SymbolIcon();
            symbolIcon.put_Symbol((Symbol) 57609);
            bookmarkButton.put_Icon((IconElement) symbolIcon);
            this.bookmarkButton.put_Label(App.Strings["common.bookmark", "bookmark"].ToLower());
          }
        }
      }
      catch
      {
        throw;
      }
      ((Control) this.bookmarkButton).put_IsEnabled(true);
    }

    private void list_SetMultiselectContextMenu(object sender, IconButtonEventCollection e)
    {
      if (!(((FrameworkElement) this).DataContext is PlaylistEntry dataContext) || !dataContext.PlaylistOfSignedInUser)
        return;
      e.Insert(0, this.remove);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PlaylistPage.xaml"), (ComponentResourceLocation) 0);
      this.appBar = (CommandBar) ((FrameworkElement) this).FindName("appBar");
      this.editButton = (AppBarButton) ((FrameworkElement) this).FindName("editButton");
      this.sortButton = (AppBarButton) ((FrameworkElement) this).FindName("sortButton");
      this.pinButton = (AppBarButton) ((FrameworkElement) this).FindName("pinButton");
      this.bookmarkButton = (AppBarButton) ((FrameworkElement) this).FindName("bookmarkButton");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.list = (VideoList) ((FrameworkElement) this).FindName("list");
      this.offline = (VideoList) ((FrameworkElement) this).FindName("offline");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase1 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase1.add_Click), new Action<EventRegistrationToken>(buttonBase1.remove_Click), new RoutedEventHandler(this.editButton_Click));
          break;
        case 2:
          ButtonBase buttonBase2 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase2.add_Click), new Action<EventRegistrationToken>(buttonBase2.remove_Click), new RoutedEventHandler(this.sortButton_Click));
          break;
        case 3:
          ButtonBase buttonBase3 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase3.add_Click), new Action<EventRegistrationToken>(buttonBase3.remove_Click), new RoutedEventHandler(this.pinButton_Click));
          break;
        case 4:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.bookmarkButton_Tapped));
          ButtonBase buttonBase4 = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase4.add_Click), new Action<EventRegistrationToken>(buttonBase4.remove_Click), new RoutedEventHandler(this.bookmarkButton_Click));
          break;
        case 5:
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
          break;
        case 6:
          ((VideoList) target).VideosLoaded += new EventHandler<YouTubeEntry[]>(this.list_VideosLoaded);
          ((VideoList) target).SetContextMenu += new EventHandler<IconButtonEventCollection>(this.list_SetContextMenu);
          ((VideoList) target).SetMultiselectContextMenu += new EventHandler<IconButtonEventCollection>(this.list_SetMultiselectContextMenu);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
