// Decompiled with JetBrains decompiler
// Type: myTube.LikesPage
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
  public sealed class LikesPage : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList list;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList disliked;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public LikesPage() => this.InitializeComponent();

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (e.NavigationMode == 1)
        return;
      this.overCanvas.ScrollToIndex(0, true);
      if (this.list.Client == null || this.disliked.Client == null)
      {
        this.list.Client = (VideoListClient) new LikesClient(Rating.Like, 25);
        this.disliked.Client = (VideoListClient) new LikesClient(Rating.Dislike, 25);
      }
      else
      {
        if (((Collection<YouTubeEntry>) this.list.Entries).Count > 0)
          this.list.Clear(false);
        if (((Collection<YouTubeEntry>) this.disliked.Entries).Count > 0)
          this.disliked.Clear(false);
        this.list.Load();
        this.disliked.Load();
      }
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///LikesPage.xaml"), (ComponentResourceLocation) 0);
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.list = (VideoList) ((FrameworkElement) this).FindName("list");
      this.disliked = (VideoList) ((FrameworkElement) this).FindName("disliked");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
