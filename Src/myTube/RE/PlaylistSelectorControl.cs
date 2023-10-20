// Decompiled with JetBrains decompiler
// Type: myTube.PlaylistSelectorControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public sealed class PlaylistSelectorControl : UserControl, IComponentConnector
  {
    private PlaylistListClient client;
    private int page;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ObjectCollection playlists;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid addButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView playWatchList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ListView playlistList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public event EventHandler SelectionMade;

    public PlaylistSelectorControl()
    {
      this.InitializeComponent();
      this.client = (PlaylistListClient) new UserPlaylistListClient(50);
      if (YouTube.IsSignedIn && (YouTube.UserInfo == null || !YouTube.UserInfo.Incomplete))
        this.Load();
      ((Control) this).put_FontFamily(new FontFamily("Segoe WP"));
      ((IList<object>) ((ItemsControl) this.playWatchList).Items)[1] = (object) App.Strings["videos.lists.watchlater", "watch later"].ToLower();
      ((IList<object>) ((ItemsControl) this.playWatchList).Items)[0] = (object) App.Strings["videos.lists.favorites", "favorites"].ToLower();
    }

    public async Task Load()
    {
      try
      {
        PlaylistEntry[] feed = await this.client.GetFeed(this.page);
        if (feed == null)
          return;
        ++this.page;
        foreach (object obj in feed)
          ((Collection<object>) this.playlists).Add(obj);
      }
      catch
      {
      }
    }

    private async void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.SelectionMade != null)
        this.SelectionMade((object) this, new EventArgs());
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      YouTubeEntry ent = ((FrameworkElement) this).DataContext as YouTubeEntry;
      if (((Selector) this.playWatchList).SelectedIndex == 0)
      {
        try
        {
          int num = await YouTube.Favorite(ent.ID) ? 1 : 0;
        }
        catch
        {
          MessageDialog messageDialog = new MessageDialog("This video could not be added to your favorites", "Oops");
          messageDialog.Commands.Add((IUICommand) new UICommand("okay :("));
          messageDialog.ShowAsync();
        }
      }
      else if (((Selector) this.playWatchList).SelectedIndex == 1)
      {
        try
        {
          int num = await YouTube.WatchLater(ent.ID, Settings.WatchLater.AddVideosTo == PlaylistPosition.End ? -1 : 0) ? 1 : 0;
        }
        catch
        {
          MessageDialog messageDialog = new MessageDialog("This video could not be added to your watch later list", "Oops");
          messageDialog.Commands.Add((IUICommand) new UICommand("okay :("));
          messageDialog.ShowAsync();
        }
      }
      ent = (YouTubeEntry) null;
    }

    private async void playlistList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.SelectionMade != null)
        this.SelectionMade((object) this, new EventArgs());
      if (!(((Selector) this.playlistList).SelectedItem is PlaylistEntry) || !(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      PlaylistEntry plent = ((Selector) this.playlistList).SelectedItem as PlaylistEntry;
      try
      {
        YouTubeEntry ent = ((FrameworkElement) this).DataContext as YouTubeEntry;
        int num = await YouTube.AddToPlaylist(plent.ID, ent.ID, plent.Count) ? 1 : 0;
        App.GlobalObjects.OfflinePlaylistsCollection.SetOfflinePlaylistVideos(plent, false, ent.ID);
        ent = (YouTubeEntry) null;
      }
      catch
      {
        MessageDialog messageDialog = new MessageDialog("This video could not be added to this playlist", "Oops");
        messageDialog.Commands.Add((IUICommand) new UICommand("okay :("));
        messageDialog.ShowAsync();
      }
      plent = (PlaylistEntry) null;
    }

    private void addButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      AddPlaylistControl control = new AddPlaylistControl();
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
        e.GetPosition((UIElement) DefaultPage.Current);
        return new Point()
        {
          X = Math.Max(bounds.Width - ((FrameworkElement) control).Width - 19.0, (bounds.Width - ((FrameworkElement) control).Width) / 2.0),
          Y = 0.0
        };
      }));
      DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, -150.0));
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PlaylistSelectorControl.xaml"), (ComponentResourceLocation) 0);
      this.playlists = (ObjectCollection) ((FrameworkElement) this).FindName("playlists");
      this.addButton = (Grid) ((FrameworkElement) this).FindName("addButton");
      this.playWatchList = (ListView) ((FrameworkElement) this).FindName("playWatchList");
      this.playlistList = (ListView) ((FrameworkElement) this).FindName("playlistList");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.addButton_Tapped));
          break;
        case 2:
          Selector selector1 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector1.add_SelectionChanged), new Action<EventRegistrationToken>(selector1.remove_SelectionChanged), new SelectionChangedEventHandler(this.ListView_SelectionChanged));
          break;
        case 3:
          Selector selector2 = (Selector) target;
          WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>(new Func<SelectionChangedEventHandler, EventRegistrationToken>(selector2.add_SelectionChanged), new Action<EventRegistrationToken>(selector2.remove_SelectionChanged), new SelectionChangedEventHandler(this.playlistList_SelectionChanged));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
