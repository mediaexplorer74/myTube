// Decompiled with JetBrains decompiler
// Type: myTube.WatchLaterPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using myTube.SettingsPages;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class WatchLaterPage : Page, IComponentConnector
  {
    private static VideoListClient client;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList videoList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public static VideoListClient Client
    {
      get
      {
        if (WatchLaterPage.client == null)
          WatchLaterPage.client = !YouTube.IsSignedIn || YouTube.UserInfo != null || SharedSettings.CurrentAccount == null ? (VideoListClient) new PlaylistPageClient("WL") : (VideoListClient) new PlaylistPageClient("WL" + UserInfo.RemoveUCFromID(SharedSettings.CurrentAccount.UserID));
        return WatchLaterPage.client;
      }
    }

    public WatchLaterPage()
    {
      this.InitializeComponent();
      this.videoList.Client = WatchLaterPage.Client;
      VideoList videoList = this.videoList;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) videoList).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) videoList).remove_Loaded), new RoutedEventHandler(this.videoList_Loaded));
      this.BottomAppBar.put_ClosedDisplayMode((AppBarClosedDisplayMode) 1);
    }

    private void videoList_Loaded(object sender, RoutedEventArgs e)
    {
      IconButtonEvent iconButtonEvent1 = new IconButtonEvent();
      iconButtonEvent1.Text = App.Strings["common.remove", "remove"].ToLower();
      iconButtonEvent1.Symbol = (Symbol) 57608;
      IconButtonEvent iconButtonEvent2 = iconButtonEvent1;
      this.videoList.ContextMenu[2] = iconButtonEvent2;
      iconButtonEvent2.Selected += new EventHandler<IconButtonEventArgs>(this.remove_Selected);
    }

    private async void remove_Selected(object sender, IconButtonEventArgs e)
    {
      FrameworkElement originalSender1 = e.OriginalSender as FrameworkElement;
      e.Close();
      if (originalSender1 != null && originalSender1.DataContext is YouTubeEntry dataContext)
      {
        ((Collection<YouTubeEntry>) this.videoList.Entries).IndexOf(dataContext);
        if (dataContext.PlaylistID != null)
        {
          try
          {
            ((Collection<YouTubeEntry>) this.videoList.Entries).Remove(dataContext);
            int num = await YouTube.RemoveFromWatchLaterPage(dataContext.ID) ? 1 : 0;
          }
          catch (Exception ex)
          {
            new MessageDialog("We weren't able to remove this video from your watch later list :(", "Oops").ShowAsync();
          }
        }
      }
      if (!(e.OriginalSender is IEnumerable<YouTubeEntry> originalSender2))
        return;
      foreach (YouTubeEntry ent in originalSender2)
      {
        if (ent.PlaylistID != null)
        {
          int index = ((Collection<YouTubeEntry>) this.videoList.Entries).IndexOf(ent);
          if (index != -1)
            ((Collection<YouTubeEntry>) this.videoList.Entries).Remove(ent);
          try
          {
            int num = await YouTube.RemoveFromWatchLaterPage(ent.ID) ? 1 : 0;
          }
          catch
          {
            if (index != -1)
              ((Collection<YouTubeEntry>) this.videoList.Entries).Insert(index, ent);
          }
        }
      }
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1 || ((Collection<YouTubeEntry>) this.videoList.Entries).Count == 0)
      {
        this.videoList.Clear(false);
        if (e.Parameter is ClientAndFirstLoadTask parameter)
          this.videoList.Load(parameter.LoadTask);
        else
          this.videoList.Load();
      }
      else if (((Collection<YouTubeEntry>) this.videoList.Entries).Count == 0)
        this.videoList.Load();
      base.OnNavigatedTo(e);
    }

    private void AppBarButton_Click(object sender, RoutedEventArgs e)
    {
      WatchLaterSettings sett = new WatchLaterSettings();
      Popup popup1 = new Popup();
      popup1.put_Child((UIElement) sett);
      Popup popup2 = popup1;
      DefaultPage.SetPopupArrangeMethod((DependencyObject) popup2, (Func<Point>) (() =>
      {
        ((FrameworkElement) sett).put_Height(Math.Min(300.0, App.VisibleBounds.Height - 57.0));
        ((FrameworkElement) sett).put_Width(Math.Min(475.0, App.VisibleBounds.Width - 38.0));
        Rect visibleBounds = App.VisibleBounds;
        double x = (visibleBounds.Width - ((FrameworkElement) sett).Width) / 2.0;
        visibleBounds = App.VisibleBounds;
        double y = visibleBounds.Height - ((FrameworkElement) sett).Height - 19.0;
        return new Point(x, y);
      }));
      DefaultPage.Current.ShowPopup(popup2, new Point(), new Point(0.0, 80.0));
    }

    private void overCanvas_ShownChanged(object sender, bool e)
    {
      if (e)
        ((UIElement) this.BottomAppBar).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.BottomAppBar).put_Visibility((Visibility) 1);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///WatchLaterPage.xaml"), (ComponentResourceLocation) 0);
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.videoList = (VideoList) ((FrameworkElement) this).FindName("videoList");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase.add_Click), new Action<EventRegistrationToken>(buttonBase.remove_Click), new RoutedEventHandler(this.AppBarButton_Click));
          break;
        case 2:
          ((OverCanvas) target).ShownChanged += new EventHandler<bool>(this.overCanvas_ShownChanged);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
