// Decompiled with JetBrains decompiler
// Type: myTube.VideoPageThumb
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace myTube
{
  public sealed class VideoPageThumb : UserControl, IVideoContainer, IComponentConnector
  {
    private VideoInfoLoader loader;
    public TypeConstructor ClientConstructor;
    private Task<YouTubeInfo> preloadedInfo;
    public bool PlayAutomatically;
    public bool PlayAutomaticallyOnOpen;
    private const string Tag = "VideoPageThumb";
    private string id = "";
    private string checkForId = "";
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CompositeTransform buttonScale;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform playTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TranslateTransform musicTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Image thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar playProgress;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl playControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl musicControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl castingControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid castingGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock castingText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid musicGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock musicText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid playButtonGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private CompositeTransform playSymbolTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public VideoPageThumb()
    {
      this.InitializeComponent();
      this.loader = new VideoInfoLoader();
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), new SizeChangedEventHandler(this.VideoPageThumb_SizeChanged));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(VideoPageThumb_DataContextChanged)));
      ((FrameworkElement) this.musicText).put_Margin(new Thickness(0.0, -5.0, 0.0, 0.0));
      ((UIElement) this.castingControl).put_Visibility((Visibility) 1);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.VideoPageThumb_Loaded));
    }

    private void VideoPageThumb_Loaded(object sender, RoutedEventArgs e)
    {
      ContentControl playControl = this.playControl;
      double num1;
      ((UIElement) this.musicControl).put_Opacity(num1 = 0.0);
      double num2 = num1;
      ((UIElement) playControl).put_Opacity(num2);
      this.playTrans.put_Y(38.0);
      this.musicTrans.put_Y(66.5);
      Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.playControl, "Opacity", 1.0, 0.2), (Timeline) Ani.DoubleAni((DependencyObject) this.musicControl, "Opacity", 1.0, 0.3), (Timeline) Ani.DoubleAni((DependencyObject) this.playTrans, "Y", 0.0, 1.0, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)), (Timeline) Ani.DoubleAni((DependencyObject) this.musicTrans, "Y", 0.0, 1.0, (EasingFunctionBase) Ani.Ease((EasingMode) 0, 5.0)));
    }

    private async void VideoPageThumb_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(args.NewValue is YouTubeEntry))
        return;
      YouTubeEntry newValue = args.NewValue as YouTubeEntry;
      if (DefaultPage.Current != null && DefaultPage.Current.Player.MediaRunning && DefaultPage.Current.Player.CurrentEntry != null && DefaultPage.Current.Player.CurrentEntry.ID == newValue.ID)
      {
        Helper.Write((object) nameof (VideoPageThumb), (object) "Video already playing, requesting video player");
        DefaultPage.Current.RequestVideoPlayer((IVideoContainer) this);
      }
      if (!((args.NewValue as YouTubeEntry).ID != this.id))
        return;
      this.id = (args.NewValue as YouTubeEntry).ID;
      ((UIElement) this.thumb).put_Opacity(0.0);
      this.EvaluateTask(args.NewValue as YouTubeEntry);
    }

    private async void EvaluateTask(YouTubeEntry entry)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      // ISSUE: variable of a compiler-generated type
      VideoPageThumb.\u003C\u003Ec__DisplayClass10_0 cDisplayClass100;
      DispatchedHandler dispatchedHandler;
      this.preloadedInfo = App.TaskDispatcher.AddTask<YouTubeInfo>((Func<Task<YouTubeInfo>>) (async () =>
      {
        if (await App.GlobalObjects.TransferManager.GetTransferState(entry.ID) != TransferManager.State.None)
          return (YouTubeInfo) null;
        Helper.Write((object) nameof (VideoPageThumb), (object) ("Preloading video info for " + entry.ID));
        this.loader.UseNavigatePage = false;
        YouTubeInfo task;
        if (entry.AuthorDisplayName.ToUpper().Contains("VEVO"))
          task = await this.loader.LoadInfo(entry.ID, decipher: true);
        else
          task = await this.loader.LoadInfo(entry.ID, false);
        if (Settings.PlayAutomatically && this.PlayAutomatically || this.PlayAutomaticallyOnOpen)
        {
          // ISSUE: method pointer
          ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 1, dispatchedHandler ?? (dispatchedHandler = new DispatchedHandler((object) cDisplayClass100, __methodptr(\u003CEvaluateTask\u003Eb__1))));
        }
        return task;
      }));
    }

    private void VideoPageThumb_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      CompositeTransform buttonScale1 = this.buttonScale;
      CompositeTransform buttonScale2 = this.buttonScale;
      Size newSize1 = e.NewSize;
      double num1;
      double num2 = num1 = Math.Pow(newSize1.Width / 320.0, 0.3);
      buttonScale2.put_ScaleY(num1);
      double num3 = num2;
      buttonScale1.put_ScaleX(num3);
      CompositeTransform playSymbolTrans1 = this.playSymbolTrans;
      CompositeTransform playSymbolTrans2 = this.playSymbolTrans;
      Size newSize2 = e.NewSize;
      double num4;
      double num5 = num4 = Math.Pow(newSize2.Height / 50.0, 0.85);
      playSymbolTrans2.put_ScaleY(num4);
      double num6 = num5;
      playSymbolTrans1.put_ScaleX(num6);
      Grid playButtonGrid1 = this.playButtonGrid;
      Grid playButtonGrid2 = this.playButtonGrid;
      Size newSize3 = e.NewSize;
      double num7;
      double num8 = num7 = MyMath.Clamp(Math.Pow(newSize3.Height, 0.9), 0.0, 190.0);
      ((FrameworkElement) playButtonGrid2).put_Height(num7);
      double num9 = num8;
      ((FrameworkElement) playButtonGrid1).put_Width(num9);
    }

    private void addToButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      PopupMenu popupMenu = new PopupMenu();
      popupMenu.Commands.Add((IUICommand) new UICommand("favorites"));
      popupMenu.Commands.Add((IUICommand) new UICommand("watch later"));
      Point position = e.GetPosition(Window.Current.Content);
      position.Y -= 48.0;
      popupMenu.ShowAsync(position);
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      Size size = new Size(200.0, 200.0);
      if (availableSize.Width > availableSize.Height || double.IsInfinity(availableSize.Height))
      {
        size.Width = availableSize.Width;
        size.Height = availableSize.Width * 9.0 / 16.0;
      }
      else if (availableSize.Height > availableSize.Width || double.IsInfinity(availableSize.Width))
      {
        size.Height = availableSize.Height;
        size.Width = availableSize.Width * 16.0 / 9.0;
      }
      if (size.Height > availableSize.Height)
        size.Height = availableSize.Height;
      if (size.Width > availableSize.Width)
        size.Width = availableSize.Width;
      if (double.IsInfinity(size.Width) || double.IsInfinity(size.Height) || double.IsNaN(size.Width) || double.IsNaN(size.Height))
      {
        size.Width = 300.0;
        size.Height = 200.0;
      }
      this.Content.Measure(size);
      return size;
    }

    private async void Image_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (((FrameworkElement) this).DataContext == null || !(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      DefaultPage.Current.VideoPlayer.SetTypeConstructor(this.ClientConstructor);
      this.playProgress.put_IsIndeterminate(true);
      if (!await DefaultPage.Current.VideoPlayer.OpenVideo(((FrameworkElement) this).DataContext as YouTubeEntry, Settings.Quality, decipher: (((FrameworkElement) this).DataContext as YouTubeEntry).AuthorDisplayName.ToUpper().Contains("VEVO"), preloadedInfo: this.preloadedInfo, openBookmark: true))
        App.UpdateCipher(5.0);
      else
        DefaultPage.Current.RequestVideoPlayer((IVideoContainer) this);
      this.playProgress.put_IsIndeterminate(false);
    }

    private void BitmapImage_ImageOpened(object sender, RoutedEventArgs e) => Ani.Begin((DependencyObject) this.thumb, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "ThumbnailOpacity"], 0.3);

    private async void musicGrid_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (((FrameworkElement) this).DataContext == null || !(((FrameworkElement) this).DataContext is YouTubeEntry))
        return;
      DefaultPage.Current.VideoPlayer.SetTypeConstructor(this.ClientConstructor);
      this.playProgress.put_IsIndeterminate(true);
      if (!await (Window.Current.Content as DefaultPage).VideoPlayer.OpenVideo(((FrameworkElement) this).DataContext as YouTubeEntry, YouTubeQuality.Audio, decipher: (((FrameworkElement) this).DataContext as YouTubeEntry).AuthorDisplayName.ToUpper().Contains("VEVO"), preloadedInfo: this.preloadedInfo, openBookmark: true))
        App.UpdateCipher(5.0);
      this.playProgress.put_IsIndeterminate(false);
    }

    private async void castingControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    public void VideoSet()
    {
      Ani.Begin((DependencyObject) this, "Opacity", 0.0, 0.1);
      ((UIElement) this).put_IsHitTestVisible(false);
      // ISSUE: method pointer
      DefaultPage.Current.Player.MediaEnded -= new TypedEventHandler<VideoPlayer, MediaEndedEventArgs>((object) this, __methodptr(Player_MediaEnded));
      // ISSUE: method pointer
      DefaultPage.Current.Player.MediaEnded += new TypedEventHandler<VideoPlayer, MediaEndedEventArgs>((object) this, __methodptr(Player_MediaEnded));
    }

    private async void Player_MediaEnded(VideoPlayer sender, MediaEndedEventArgs args)
    {
      if (Settings.MiniPlayerType == MiniPlayerType.MiniPlayer && args.ContinuingWith != null && ((FrameworkElement) this).DataContext is YouTubeEntry && (args.CurrentEntry == null || args.CurrentEntry.ID == (((FrameworkElement) this).DataContext as YouTubeEntry).ID))
      {
        this.checkForId = args.ContinuingWith.ID;
        // ISSUE: method pointer
        DefaultPage.Current.Player.MediaOpened -= new TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>((object) this, __methodptr(Player_MediaOpened));
        // ISSUE: method pointer
        DefaultPage.Current.Player.MediaOpened += new TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>((object) this, __methodptr(Player_MediaOpened));
      }
      else
      {
        Helper.Write((object) nameof (VideoPageThumb), (object) "Video ended, resetting video player position");
        DefaultPage.Current.ResetVideoPlayer();
      }
    }

    private void Player_MediaOpened(VideoPlayer sender, MediaOpenedEventArgs args)
    {
      // ISSUE: method pointer
      DefaultPage.Current.Player.MediaOpened -= new TypedEventHandler<VideoPlayer, MediaOpenedEventArgs>((object) this, __methodptr(Player_MediaOpened));
      if (DefaultPage.Current.Player.CurrentEntry.ID == this.checkForId)
      {
        App.Instance.RootFrame.RemoveLastBackStackAtNavigate();
        App.Instance.RootFrame.Navigate(typeof (VideoPage), (object) DefaultPage.Current.VideoPlayer.CurrentEntry);
      }
      this.checkForId = (string) null;
    }

    private void Player_MediaRunningChanged(object sender, MediaRunningChangedEventArgs e)
    {
      DefaultPage.Current.Player.MediaRunningChanged -= new EventHandler<MediaRunningChangedEventArgs>(this.Player_MediaRunningChanged);
      if (!e.MediaRunning)
        return;
      if (DefaultPage.Current.Player.CurrentEntry.ID == this.checkForId)
      {
        App.Instance.RootFrame.RemoveLastBackStackAtNavigate();
        App.Instance.RootFrame.Navigate(typeof (VideoPage), (object) DefaultPage.Current.VideoPlayer.CurrentEntry);
      }
      this.checkForId = (string) null;
    }

    public void VideoUnset()
    {
      Ani.Begin((DependencyObject) this, "Opacity", 1.0, 0.2);
      ((UIElement) this).put_IsHitTestVisible(true);
      // ISSUE: method pointer
      DefaultPage.Current.Player.MediaEnded -= new TypedEventHandler<VideoPlayer, MediaEndedEventArgs>((object) this, __methodptr(Player_MediaEnded));
    }

    public FrameworkElement GetElement() => (FrameworkElement) this;

    public VideoDepth GetVideoDepth() => VideoDepth.Below;

    public bool GetBindVideoPlayerShown() => false;

    public bool HasBackground() => true;

    public bool IsArrangeActive() => true;

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///VideoPageThumb.xaml"), (ComponentResourceLocation) 0);
      this.buttonScale = (CompositeTransform) ((FrameworkElement) this).FindName("buttonScale");
      this.playTrans = (TranslateTransform) ((FrameworkElement) this).FindName("playTrans");
      this.musicTrans = (TranslateTransform) ((FrameworkElement) this).FindName("musicTrans");
      this.thumb = (Image) ((FrameworkElement) this).FindName("thumb");
      this.playProgress = (ProgressBar) ((FrameworkElement) this).FindName("playProgress");
      this.playControl = (ContentControl) ((FrameworkElement) this).FindName("playControl");
      this.musicControl = (ContentControl) ((FrameworkElement) this).FindName("musicControl");
      this.castingControl = (ContentControl) ((FrameworkElement) this).FindName("castingControl");
      this.castingGrid = (Grid) ((FrameworkElement) this).FindName("castingGrid");
      this.castingText = (TextBlock) ((FrameworkElement) this).FindName("castingText");
      this.musicGrid = (Grid) ((FrameworkElement) this).FindName("musicGrid");
      this.musicText = (TextBlock) ((FrameworkElement) this).FindName("musicText");
      this.playButtonGrid = (Grid) ((FrameworkElement) this).FindName("playButtonGrid");
      this.playSymbolTrans = (CompositeTransform) ((FrameworkElement) this).FindName("playSymbolTrans");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.Image_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.musicGrid_Tapped));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.castingControl_Tapped));
          break;
        case 4:
          BitmapImage bitmapImage = (BitmapImage) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(bitmapImage.add_ImageOpened), new Action<EventRegistrationToken>(bitmapImage.remove_ImageOpened), new RoutedEventHandler(this.BitmapImage_ImageOpened));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
