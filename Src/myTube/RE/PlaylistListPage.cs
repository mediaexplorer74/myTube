// Decompiled with JetBrains decompiler
// Type: myTube.PlaylistListPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class PlaylistListPage : Page, IComponentConnector
  {
    private string lastID = "";
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PlaylistList list;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PlaylistList savedList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private PlaylistList myMixList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public PlaylistListPage()
    {
      this.InitializeComponent();
      this.savedList.Filter = this.myMixList.Filter = (Func<PlaylistEntry, bool>) (p => YouTube.UserInfo == null || p.AuthorID != YouTube.UserInfo.ID);
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1)
        this.overCanvas.ScrollToPage(0, true);
      if (e.Parameter != null)
      {
        if (e.Parameter is UserInfo)
        {
          UserInfo parameter = e.Parameter as UserInfo;
          if (this.lastID != parameter.ID)
          {
            this.lastID = parameter.ID;
            this.list.Client = (YouTubeClient<PlaylistEntry>) new UserPlaylistListClient(parameter.ID, 20);
            this.savedList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.Saved, parameter.ID);
            ((FrameworkElement) this).put_DataContext((object) parameter);
            PlaylistList savedList = this.savedList;
            Visibility visibility1;
            ((UIElement) this.myMixList).put_Visibility((Visibility) (int) (visibility1 = !YouTube.IsSignedIn || !(YouTube.UserInfo.ID == parameter.ID) ? (Visibility) 1 : (Visibility) 0));
            Visibility visibility2 = visibility1;
            ((UIElement) savedList).put_Visibility(visibility2);
            if (((UIElement) this.savedList).Visibility == null)
            {
              this.savedList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.Saved, parameter.ID);
              this.myMixList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.MyMix);
            }
          }
        }
        else if (e.Parameter is string)
        {
          string parameter = e.Parameter as string;
          if (parameter != this.lastID)
          {
            this.lastID = parameter;
            this.list.Client = (YouTubeClient<PlaylistEntry>) new UserPlaylistListClient(parameter, 20);
            PlaylistList savedList = this.savedList;
            Visibility visibility3;
            ((UIElement) this.myMixList).put_Visibility((Visibility) (int) (visibility3 = !YouTube.IsSignedIn || !(YouTube.UserInfo.ID == parameter) ? (Visibility) 1 : (Visibility) 0));
            Visibility visibility4 = visibility3;
            ((UIElement) savedList).put_Visibility(visibility4);
            if (((UIElement) this.savedList).Visibility == null)
            {
              this.savedList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.Saved, parameter);
              this.myMixList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.MyMix);
            }
            UserInfoClient userInfoClient = new UserInfoClient();
            try
            {
              ((FrameworkElement) this).put_DataContext((object) await userInfoClient.GetInfo(parameter));
            }
            catch
            {
            }
          }
        }
      }
      else if (YouTube.IsSignedIn)
      {
        if (YouTube.UserInfo.ID != this.lastID || e.NavigationMode != 1)
        {
          this.lastID = YouTube.UserInfo.ID;
          ((FrameworkElement) this).put_DataContext((object) YouTube.UserInfo);
          this.list.Client = (YouTubeClient<PlaylistEntry>) new UserPlaylistListClient(20);
          PlaylistList savedList = this.savedList;
          Visibility visibility5;
          ((UIElement) this.myMixList).put_Visibility((Visibility) (int) (visibility5 = (Visibility) 0));
          Visibility visibility6 = visibility5;
          ((UIElement) savedList).put_Visibility(visibility6);
          this.savedList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.Saved);
          this.myMixList.Client = (YouTubeClient<PlaylistEntry>) new PlaylistListPageClient(PlaylistType.MyMix);
        }
      }
      else if (((Collection<PlaylistEntry>) this.list.Entries).Count == 0)
        this.LoadOfflinePlaylist();
      base.OnNavigatedTo(e);
    }

    private async void LoadOfflinePlaylist()
    {
      if (((Collection<PlaylistEntry>) this.list.Entries).Count != 0)
        return;
      this.list.Clear();
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      OfflinePlaylist[] offlinePlaylistArray = App.GlobalObjects.OfflinePlaylistsCollection.Playlists;
      for (int index1 = 0; index1 < offlinePlaylistArray.Length; ++index1)
      {
        OfflinePlaylist p = offlinePlaylistArray[index1];
        if (p.Videos.Length != 0)
        {
          bool add = false;
          string[] strArray = p.Videos;
          for (int index2 = 0; index2 < strArray.Length; ++index2)
          {
            if (await App.GlobalObjects.TransferManager.GetTransferState(strArray[index2]) != TransferManager.State.None)
            {
              add = true;
              break;
            }
          }
          strArray = (string[]) null;
          if (add)
            ((Collection<PlaylistEntry>) this.list.Entries).Add(p.Playlist);
        }
        p = (OfflinePlaylist) null;
      }
      offlinePlaylistArray = (OfflinePlaylist[]) null;
    }

    private async void list_FailedToLoad(object sender, EventArgs e)
    {
      if (((Collection<PlaylistEntry>) this.list.Entries).Count != 0)
        return;
      this.LoadOfflinePlaylist();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PlaylistListPage.xaml"), (ComponentResourceLocation) 0);
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.list = (PlaylistList) ((FrameworkElement) this).FindName("list");
      this.savedList = (PlaylistList) ((FrameworkElement) this).FindName("savedList");
      this.myMixList = (PlaylistList) ((FrameworkElement) this).FindName("myMixList");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        ((PlaylistList) target).FailedToLoad += new EventHandler(this.list_FailedToLoad);
      this._contentLoaded = true;
    }
  }
}
