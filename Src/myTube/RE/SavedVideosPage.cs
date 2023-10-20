// Decompiled with JetBrains decompiler
// Type: myTube.SavedVideosPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class SavedVideosPage : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList videoList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VideoList audioList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public SavedVideosPage()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.SavedVideosPage_Loaded));
    }

    private void SavedVideosPage_Loaded(object sender, RoutedEventArgs e)
    {
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      if (e.NavigationMode == 1 && ((Collection<YouTubeEntry>) this.videoList.Entries).Count != 0)
        return;
      this.videoList.LoadVideosFunc = (Func<int, Task<YouTubeEntry[]>>) (page => App.GlobalObjects.TransferManager.GetEntries(page, TransferType.Video));
      this.audioList.LoadVideosFunc = (Func<int, Task<YouTubeEntry[]>>) (page => App.GlobalObjects.TransferManager.GetEntries(page, TransferType.Audio));
      this.videoList.Clear(false);
      this.videoList.Load();
      this.audioList.Clear(false);
      this.audioList.Load();
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///SavedVideosPage.xaml"), (ComponentResourceLocation) 0);
      this.videoList = (VideoList) ((FrameworkElement) this).FindName("videoList");
      this.audioList = (VideoList) ((FrameworkElement) this).FindName("audioList");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
