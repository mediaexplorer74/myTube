// Decompiled with JetBrains decompiler
// Type: myTube.PlaylistList
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace myTube
{
  public sealed class PlaylistList : UserControl, IComponentConnector
  {
    public static DependencyProperty ListPaddingProperty = DependencyProperty.Register(nameof (ListPadding), typeof (Thickness), typeof (PlaylistList), new PropertyMetadata((object) new Thickness(0.0, 0.0, -19.0, 0.0)));
    public static DependencyProperty ThumbPaddingProperty = DependencyProperty.Register(nameof (ThumbPadding), typeof (Thickness), typeof (PlaylistList), new PropertyMetadata((object) new Thickness(0.0, 0.0, 19.0, 19.0)));
    public static DependencyProperty EntriesProperty = DependencyProperty.Register(nameof (Entries), typeof (ObservableCollection<PlaylistEntry>), typeof (PlaylistList), new PropertyMetadata((object) null));
    public static DependencyProperty ClientProperty = DependencyProperty.Register(nameof (Client), typeof (YouTubeClient<PlaylistEntry>), typeof (PlaylistList), new PropertyMetadata((object) null, new PropertyChangedCallback(PlaylistList.OnClientChanged)));
    public static DependencyProperty LoadOnScrollProperty = DependencyProperty.Register(nameof (LoadOnScroll), typeof (bool), typeof (PlaylistList), new PropertyMetadata((object) true));
    public static DependencyProperty AutomaticallyLoadDataProperty = DependencyProperty.Register(nameof (AutomaticallyLoadData), typeof (bool), typeof (PlaylistList), new PropertyMetadata((object) true));
    public Func<PlaylistEntry, bool> Filter;
    private int page;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock loadingText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl ItemList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    private static void OnClientChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PlaylistList playlistList = d as PlaylistList;
      playlistList.Clear();
      ((UIElement) playlistList.loadingText).put_Visibility((Visibility) 0);
      playlistList.loadingText.put_Text(((IDictionary<object, object>) ((FrameworkElement) playlistList).Resources)[(object) "loadingString"] as string);
      playlistList.page = 0;
      if (!playlistList.AutomaticallyLoadData)
        return;
      playlistList.Load();
    }

    public ObservableCollection<PlaylistEntry> Entries
    {
      get => (ObservableCollection<PlaylistEntry>) ((DependencyObject) this).GetValue(PlaylistList.EntriesProperty);
      set => ((DependencyObject) this).SetValue(PlaylistList.EntriesProperty, (object) value);
    }

    public Thickness ListPadding
    {
      get => (Thickness) ((DependencyObject) this).GetValue(PlaylistList.ListPaddingProperty);
      set => ((DependencyObject) this).SetValue(PlaylistList.ListPaddingProperty, (object) value);
    }

    public Thickness ThumbPadding
    {
      get => (Thickness) ((DependencyObject) this).GetValue(PlaylistList.ThumbPaddingProperty);
      set => ((DependencyObject) this).SetValue(PlaylistList.ThumbPaddingProperty, (object) value);
    }

    public YouTubeClient<PlaylistEntry> Client
    {
      get => (YouTubeClient<PlaylistEntry>) ((DependencyObject) this).GetValue(PlaylistList.ClientProperty);
      set => ((DependencyObject) this).SetValue(PlaylistList.ClientProperty, (object) value);
    }

    public bool AutomaticallyLoadData
    {
      get => (bool) ((DependencyObject) this).GetValue(PlaylistList.AutomaticallyLoadDataProperty);
      set => ((DependencyObject) this).SetValue(PlaylistList.AutomaticallyLoadDataProperty, (object) value);
    }

    public bool LoadOnScroll
    {
      get => (bool) ((DependencyObject) this).GetValue(PlaylistList.LoadOnScrollProperty);
      set => ((DependencyObject) this).SetValue(PlaylistList.LoadOnScrollProperty, (object) value);
    }

    public event EventHandler FailedToLoad;

    public PlaylistList()
    {
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(VideoList_DataContextChanged)));
      this.Entries = new ObservableCollection<PlaylistEntry>();
      ((FrameworkElement) this.ItemList).put_DataContext((object) this);
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.VideoList_SizeChanged));
      ScrollViewer scroll = this.scroll;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<ScrollViewerViewChangedEventArgs>>(new Func<EventHandler<ScrollViewerViewChangedEventArgs>, EventRegistrationToken>(scroll.add_ViewChanged), new Action<EventRegistrationToken>(scroll.remove_ViewChanged), new EventHandler<ScrollViewerViewChangedEventArgs>(this.scroll_ViewChanged));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.PlaylistList_Loaded));
    }

    private void PlaylistList_Loaded(object sender, RoutedEventArgs e)
    {
      for (int index = 0; index < ((Collection<PlaylistEntry>) this.Entries).Count; ++index)
      {
        if (!((Collection<PlaylistEntry>) this.Entries)[index].Exists)
        {
          ((Collection<PlaylistEntry>) this.Entries).RemoveAt(index);
          --index;
        }
      }
    }

    private void scroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
      if (this.Client == null || this.Client.IsBusy || !this.LoadOnScroll || this.scroll.ScrollableHeight - this.scroll.VerticalOffset >= 800.0)
        return;
      this.Load();
    }

    private void VideoList_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(args.NewValue is PlaylistListClient newValue))
        return;
      this.Client = (YouTubeClient<PlaylistEntry>) newValue;
    }

    private void VideoList_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (e.NewSize.Width > 1760.0)
        (this.ItemList.ItemsPanelRoot as ItemsWrapGrid).put_ItemWidth((e.NewSize.Width - 0.1) / 4.0);
      else if (e.NewSize.Width > 1440.0)
        (this.ItemList.ItemsPanelRoot as ItemsWrapGrid).put_ItemWidth((e.NewSize.Width - 0.1) / 3.0);
      else if (e.NewSize.Width > 700.0)
        (this.ItemList.ItemsPanelRoot as ItemsWrapGrid).put_ItemWidth((e.NewSize.Width - 0.1) / 2.0);
      else
        (this.ItemList.ItemsPanelRoot as ItemsWrapGrid).put_ItemWidth(double.NaN);
    }

    public async void Clear()
    {
      ((Collection<PlaylistEntry>) this.Entries).Clear();
      this.page = 0;
    }

    public async Task Load()
    {
      try
      {
        PlaylistEntry[] playlistEntryArray;
        if (!this.Client.CanLoadPage(this.page))
          playlistEntryArray = new PlaylistEntry[0];
        else
          playlistEntryArray = await this.Client.GetFeed(this.page);
        ++this.page;
        foreach (PlaylistEntry playlistEntry in playlistEntryArray)
        {
          if (this.Filter == null)
            ((Collection<PlaylistEntry>) this.Entries).Add(playlistEntry);
          else if (this.Filter(playlistEntry))
            ((Collection<PlaylistEntry>) this.Entries).Add(playlistEntry);
        }
        if (((Collection<PlaylistEntry>) this.Entries).Count == 0)
        {
          ((UIElement) this.loadingText).put_Visibility((Visibility) 0);
          this.loadingText.put_Text(((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "noPlaylistsString"] as string);
        }
        else
          ((UIElement) this.loadingText).put_Visibility((Visibility) 1);
      }
      catch
      {
        if (this.FailedToLoad != null)
          this.FailedToLoad((object) this, (EventArgs) null);
        if (((Collection<PlaylistEntry>) this.Entries).Count != 0)
          return;
        ((UIElement) this.loadingText).put_Visibility((Visibility) 0);
        this.loadingText.put_Text(((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "noPlaylistsString"] as string);
      }
    }

    private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.AddedItems.Count != 1)
        return;
      ((App) Application.Current).RootFrame.Navigate(typeof (VideoPage), (object) (e.AddedItems[0] as YouTubeEntry));
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///PlaylistList.xaml"), (ComponentResourceLocation) 0);
      this.userControl = (UserControl) ((FrameworkElement) this).FindName("userControl");
      this.loadingText = (TextBlock) ((FrameworkElement) this).FindName("loadingText");
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.ItemList = (ItemsControl) ((FrameworkElement) this).FindName("ItemList");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
