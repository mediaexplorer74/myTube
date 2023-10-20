// Decompiled with JetBrains decompiler
// Type: myTube.HistoryPage
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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class HistoryPage : Page, IComponentConnector
  {
    private IconButtonEvent removeYouTubeHistory;
    private IconButtonEvent removeMyTubeHistory;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private AppBarButton clearButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList mytubeHistory;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList youtubeHistory;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public HistoryPage()
    {
      this.InitializeComponent();
      ((Control) this.clearButton).put_IsEnabled(false);
      this.youtubeHistory.SetContextMenu += new EventHandler<IconButtonEventCollection>(this.YoutubeHistory_SetContextMenu);
      this.mytubeHistory.SetContextMenu += new EventHandler<IconButtonEventCollection>(this.MytubeHistory_SetContextMenu);
      this.mytubeHistory.SetMultiselectContextMenu += new EventHandler<IconButtonEventCollection>(this.MytubeHistory_SetMultiselectContextMenu);
      IconButtonEvent iconButtonEvent1 = new IconButtonEvent();
      iconButtonEvent1.Text = App.Strings["common.remove", "remove"].ToLower();
      iconButtonEvent1.Symbol = (Symbol) 57608;
      this.removeYouTubeHistory = iconButtonEvent1;
      IconButtonEvent iconButtonEvent2 = new IconButtonEvent();
      iconButtonEvent2.Text = App.Strings["common.remove", "remove"].ToLower();
      iconButtonEvent2.Symbol = (Symbol) 57608;
      this.removeMyTubeHistory = iconButtonEvent2;
      this.removeYouTubeHistory.Selected += new EventHandler<IconButtonEventArgs>(this.RemoveYouTubeHistory_Selected);
      this.removeMyTubeHistory.Selected += new EventHandler<IconButtonEventArgs>(this.RemoveMyTubeHistory_Selected);
    }

    private void MytubeHistory_SetMultiselectContextMenu(object sender, IconButtonEventCollection e) => e.Insert(0, this.removeMyTubeHistory);

    private void MytubeHistory_SetContextMenu(object sender, IconButtonEventCollection e) => e.Insert(0, this.removeMyTubeHistory);

    private async void RemoveMyTubeHistory_Selected(object sender, IconButtonEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      if ((e.OriginalSender is FrameworkElement originalSender1 ? originalSender1.DataContext : (object) null) is YouTubeEntry dataContext)
      {
        App.GlobalObjects.History.RemoveEntry(dataContext);
        if (((Collection<YouTubeEntry>) this.mytubeHistory.Entries).Contains(dataContext))
          ((Collection<YouTubeEntry>) this.mytubeHistory.Entries).Remove(dataContext);
        await App.GlobalObjects.History.Save();
      }
      if (e.OriginalSender is IEnumerable<YouTubeEntry> originalSender2)
      {
        foreach (YouTubeEntry entry in originalSender2)
        {
          App.GlobalObjects.History.RemoveEntry(entry);
          if (((Collection<YouTubeEntry>) this.mytubeHistory.Entries).Contains(entry))
            ((Collection<YouTubeEntry>) this.mytubeHistory.Entries).Remove(entry);
          await App.GlobalObjects.History.Save();
        }
      }
      e.Close();
    }

    private async void RemoveYouTubeHistory_Selected(object sender, IconButtonEventArgs e)
    {
      YouTubeEntry ent = (e.OriginalSender is FrameworkElement originalSender ? originalSender.DataContext : (object) null) as YouTubeEntry;
      e.Close();
      if (ent == null || YouTube.UserInfo == null || ent.PlaylistID == null)
        return;
      int index = ((Collection<YouTubeEntry>) this.youtubeHistory.Entries).IndexOf(ent);
      if (index == -1)
        return;
      ((Collection<YouTubeEntry>) this.youtubeHistory.Entries).Remove(ent);
      try
      {
        int num = await YouTube.RemoveFromPlaylist("HL" + UserInfo.RemoveUCFromID(YouTube.UserInfo.ID), ent.PlaylistID) ? 1 : 0;
      }
      catch
      {
        ((Collection<YouTubeEntry>) this.youtubeHistory.Entries).Insert(index, ent);
      }
    }

    private void YoutubeHistory_SetContextMenu(object sender, IconButtonEventCollection e)
    {
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1 || this.mytubeHistory.LoadVideosFunc == null || this.youtubeHistory.Client == null)
      {
        this.mytubeHistory.LoadVideosFunc = new Func<int, Task<YouTubeEntry[]>>(App.GlobalObjects.History.GetEntries);
        this.youtubeHistory.Client = (VideoListClient) new HistoryPageClient(20);
      }
      base.OnNavigatedTo(e);
    }

    private void mytubeHistory_VideosLoaded(object sender, YouTubeEntry[] e) => ((Control) this.clearButton).put_IsEnabled(((Collection<YouTubeEntry>) this.mytubeHistory.Entries).Count > 0);

    private async void clearButton_Click(object sender, RoutedEventArgs e)
    {
      this.overCanvas.ScrollToIndex(0, false);
      MessageDialog messageDialog = new MessageDialog(App.Strings["dialogs.videos.clearhistory", "Are you sure you want to clear your history"], App.Strings["common.areyousure", "Are you sure?"]);
      UICommand yes = new UICommand(App.Strings["common.yes", "yes"].ToLower());
      UICommand uiCommand = new UICommand(App.Strings["common.no", "no"].ToLower());
      messageDialog.Commands.Add((IUICommand) yes);
      messageDialog.Commands.Add((IUICommand) uiCommand);
      if (await messageDialog.ShowAsync() != yes)
        return;
      ((Control) this.clearButton).put_IsEnabled(false);
      App.GlobalObjects.History.Clear();
      try
      {
        await App.GlobalObjects.History.Save();
      }
      catch
      {
      }
      this.mytubeHistory.Clear(true);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///HistoryPage.xaml"), (ComponentResourceLocation) 0);
      this.clearButton = (AppBarButton) ((FrameworkElement) this).FindName("clearButton");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.mytubeHistory = (VideoList) ((FrameworkElement) this).FindName("mytubeHistory");
      this.youtubeHistory = (VideoList) ((FrameworkElement) this).FindName("youtubeHistory");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ButtonBase buttonBase = (ButtonBase) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(buttonBase.add_Click), new Action<EventRegistrationToken>(buttonBase.remove_Click), new RoutedEventHandler(this.clearButton_Click));
          break;
        case 2:
          ((VideoList) target).VideosLoaded += new EventHandler<YouTubeEntry[]>(this.mytubeHistory_VideosLoaded);
          break;
      }
      this._contentLoaded = true;
    }
  }
}
