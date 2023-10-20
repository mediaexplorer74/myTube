// Decompiled with JetBrains decompiler
// Type: myTube.VideoThumb2
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using myTube.Overlays;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class VideoThumb2 : 
    UserControl,
    ISortableThumbnail<YouTubeEntry>,
    ISelectableThumbnail,
    IComponentConnector
  {
    private ThumbnailDispatcher thumbnailDispatcher;
    private string lastID = "";
    private SortingControl sortingControl;
    private bool selected;
    private SelectControl selectControl;
    private Dictionary<string, bool> selectedIds = new Dictionary<string, bool>();
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform progressScale;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private BitmapImage bitmap;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layoutRoot;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState Normal;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerDown;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private VisualState PointerUp;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Control16by9 thumbContainer;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle progressRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid watchedGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Image thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ThumbnailDispatcher ThumbnailDispatcher
    {
      get => this.thumbnailDispatcher != null ? this.thumbnailDispatcher : App.ThumbnailDispatcher;
      set => this.thumbnailDispatcher = value;
    }

    public VideoThumb2()
    {
      ((FrameworkElement) this).put_Tag((object) "VideoPageThumb2");
      this.InitializeComponent();
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(VideoThumb2_DataContextChanged)));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.VideoThumb2_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.VideoThumb2_Unloaded));
    }

    private async void VideoThumb2_Unloaded(object sender, RoutedEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      App.GlobalObjects.TransferManager.OnAction -= new EventHandler<TransferManagerActionEventArgs>(this.TransferManager_OnAction);
    }

    private async void VideoThumb2_Loaded(object sender, RoutedEventArgs e)
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      App.GlobalObjects.TransferManager.OnAction += new EventHandler<TransferManagerActionEventArgs>(this.TransferManager_OnAction);
      this.CheckIfWatched();
      if (!(((FrameworkElement) this).DataContext is YouTubeEntry dataContext))
        return;
      this.DoDownloadProgressStuff(dataContext.ID);
    }

    private async void CheckIfWatched()
    {
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      bool flag = false;
      if (App.GlobalObjects != null && App.GlobalObjects.History != null && ((FrameworkElement) this).DataContext is YouTubeEntry)
        flag = App.GlobalObjects.History.HasEntry((((FrameworkElement) this).DataContext as YouTubeEntry).ID);
      ((UIElement) this.thumb).put_Opacity(flag ? 0.5 : 1.0);
      ((UIElement) this.watchedGrid).put_Visibility(flag ? (Visibility) 0 : (Visibility) 1);
    }

    private async void TransferManager_OnAction(object sender, TransferManagerActionEventArgs e) => this.DoDownloadProgressStuff(e.VideoID);

    private void ProgressCallback(DownloadOperation download)
    {
      double d = Math.Max((double) download.Progress.BytesReceived / (double) download.Progress.TotalBytesToReceive, 0.05);
      if (d == 1.0)
      {
        Ani.Begin((DependencyObject) this.progressScale, "ScaleX", 0.0, 0.4, 4.0);
        Storyboard storyboard = Ani.Begin((DependencyObject) this.progressRec, "Opacity", 0.0, 0.4);
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.progressRec).put_Visibility((Visibility) 1)));
      }
      else
      {
        if (double.IsNaN(d) || double.IsInfinity(d))
          return;
        Ani.Begin((DependencyObject) this.progressScale, "ScaleX", 1.0 - d, 0.4, 4.0);
      }
    }

    private async void DoDownloadProgressStuff(string ID)
    {
      YouTubeEntry ent = ((FrameworkElement) this).DataContext as YouTubeEntry;
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      if (ent == null || !(ent.ID == ID))
        return;
      if (await App.TaskDispatcher.AddTask<TransferManager.State>((Func<Task<TransferManager.State>>) (() => App.GlobalObjects.TransferManager.GetTransferState(ID))) == TransferManager.State.Downloading)
      {
        this.progressScale.put_ScaleX(1.0);
        TransferInfo info = App.GlobalObjects.TransferManager.GetTransferInfo(ent);
        int transferState = (int) await App.GlobalObjects.TransferManager.GetTransferState(ID, TransferType.Video);
        DownloadOperation download = await App.GlobalObjects.TransferManager.GetDownload(info, transferState == 1 ? TransferType.Video : TransferType.Audio);
        if (download != null)
        {
          ((UIElement) this.progressRec).put_Visibility((Visibility) 0);
          Ani.Begin((DependencyObject) this.progressRec, "Opacity", (double) ((IDictionary<object, object>) ((FrameworkElement) this).Resources)[(object) "progressOpacity"], 0.3);
          WindowsRuntimeSystemExtensions.AsTask<DownloadOperation, DownloadOperation>(download.AttachAsync(), (IProgress<DownloadOperation>) new Progress<DownloadOperation>(new Action<DownloadOperation>(this.ProgressCallback)));
        }
        info = (TransferInfo) null;
      }
      else
      {
        Storyboard storyboard = Ani.Begin((DependencyObject) this.progressRec, "Opacity", 0.0, 0.3);
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.progressRec).put_Visibility((Visibility) 1)));
      }
    }

    protected virtual void OnPointerPressed(PointerRoutedEventArgs e)
    {
      ((UIElement) this).CapturePointer(e.Pointer);
      VisualStateManager.GoToState((Control) this, "PointerDown", true);
    }

    private async void VideoThumb2_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      await this.DoDataContextStuff();
    }

    private async Task DoDataContextStuff()
    {
      YouTubeEntry ent = ((FrameworkElement) this).DataContext as YouTubeEntry;
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      if (ent != null)
      {
        if (ent.ID == this.lastID)
          return;
        this.lastID = ent.ID;
        this.Selected = this.selectedIds.ContainsKey(ent.ID) && this.selectedIds[ent.ID];
        this.DoDownloadProgressStuff(ent.ID);
        if (this.bitmap != null)
        {
          ((UIElement) this.thumbContainer).put_Opacity(0.0);
          Uri uri;
          if (App.GlobalObjects.TransferManager != null && App.GlobalObjects.TransferManager.GetTransferInfo(ent) != null)
          {
            Uri thumbUri = await App.GlobalObjects.TransferManager.GetThumbUri(ent.ID);
            uri = !(thumbUri != (Uri) null) ? ent.Thumbnail : thumbUri;
          }
          else
            uri = ent.Thumbnail;
          this.bitmap.put_UriSource(uri);
        }
      }
      this.CheckIfWatched();
    }

    protected virtual void OnPointerReleased(PointerRoutedEventArgs e) => ((UIElement) this).ReleasePointerCapture(e.Pointer);

    protected virtual void OnPointerCaptureLost(PointerRoutedEventArgs e)
    {
      VisualStateManager.GoToState((Control) this, "PointerUp", true);
      ((Control) this).OnPointerCaptureLost(e);
    }

    private void UserControl_Tapped(object sender, TappedRoutedEventArgs e) => VideoList.TappedThumb((FrameworkElement) this);

    private void BitmapImage_ImageOpened(object sender, RoutedEventArgs e) => Ani.Begin((DependencyObject) this.thumbContainer, "Opacity", 1.0, 0.2);

    private void BitmapImage_ImageFailed(object sender, ExceptionRoutedEventArgs e) => ((UIElement) this.thumbContainer).put_Opacity(1.0);

    private void UserControl_RightTapped(object sender, RightTappedRoutedEventArgs e) => VideoList.RightTappedThumb((FrameworkElement) this, e);

    private void UserControl_Holding(object sender, HoldingRoutedEventArgs e) => VideoList.HoldingThumb((FrameworkElement) this, e);

    public bool Selected
    {
      get => this.selected;
      set
      {
        if (this.selected == value)
          return;
        this.selected = value;
        if (this.selectedIds.ContainsKey((((FrameworkElement) this).DataContext as YouTubeEntry).ID))
          this.selectedIds[(((FrameworkElement) this).DataContext as YouTubeEntry).ID] = value;
        else
          this.selectedIds.Add((((FrameworkElement) this).DataContext as YouTubeEntry).ID, value);
        EventHandler<bool> selectChanged = this.SelectChanged;
        if (selectChanged == null)
          return;
        selectChanged((object) this, value);
      }
    }

    public void BeginSorting()
    {
      if (this.sortingControl == null)
      {
        this.sortingControl = new SortingControl();
        Grid.SetRowSpan((FrameworkElement) this.sortingControl, 4);
        Grid.SetColumnSpan((FrameworkElement) this.sortingControl, 4);
        this.sortingControl.SortingTapped += new EventHandler<DirectionTappedEventArgs>(this.sortingControl_SortingTapped);
      }
      if (((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Contains((UIElement) this.sortingControl))
        return;
      ((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Add((UIElement) this.sortingControl);
    }

    private void sortingControl_SortingTapped(object sender, DirectionTappedEventArgs e)
    {
      EventHandler<SortTappedEventArgs<YouTubeEntry>> sortTapped = this.SortTapped;
      if (sortTapped == null)
        return;
      sortTapped((object) this, new SortTappedEventArgs<YouTubeEntry>()
      {
        Direction = e.Direction,
        Object = ((FrameworkElement) this).DataContext as YouTubeEntry
      });
    }

    public void EndSorting()
    {
      if (this.sortingControl == null || !((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Contains((UIElement) this.sortingControl))
        return;
      ((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Remove((UIElement) this.sortingControl);
    }

    public void BeginSelecting()
    {
      if (this.selectControl == null)
      {
        this.selectControl = new SelectControl();
        Grid.SetRowSpan((FrameworkElement) this.selectControl, 4);
        Grid.SetColumnSpan((FrameworkElement) this.selectControl, 4);
      }
      this.selectControl.ConnectToThumnbnail((ISelectableThumbnail) this);
      ((UIElement) this.selectControl).put_Opacity(1.0);
      if (((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Contains((UIElement) this.selectControl))
        return;
      ((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Add((UIElement) this.selectControl);
    }

    public void EndSelecting()
    {
      if (this.selectControl != null && ((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Contains((UIElement) this.selectControl))
      {
        Storyboard storyboard = Ani.Begin((DependencyObject) this.selectControl, "Opacity", 0.0, 0.2);
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((ICollection<UIElement>) ((Panel) this.layoutRoot).Children).Remove((UIElement) this.selectControl)));
      }
      this.selectedIds.Clear();
    }

    public event EventHandler<SortTappedEventArgs<YouTubeEntry>> SortTapped;

    public event EventHandler<bool> SelectChanged;

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///VideoThumb2.xaml"), (ComponentResourceLocation) 0);
      this.userControl = (UserControl) ((FrameworkElement) this).FindName("userControl");
      this.progressScale = (ScaleTransform) ((FrameworkElement) this).FindName("progressScale");
      this.bitmap = (BitmapImage) ((FrameworkElement) this).FindName("bitmap");
      this.layoutRoot = (Grid) ((FrameworkElement) this).FindName("layoutRoot");
      this.Normal = (VisualState) ((FrameworkElement) this).FindName("Normal");
      this.PointerDown = (VisualState) ((FrameworkElement) this).FindName("PointerDown");
      this.PointerUp = (VisualState) ((FrameworkElement) this).FindName("PointerUp");
      this.thumbContainer = (Control16by9) ((FrameworkElement) this).FindName("thumbContainer");
      this.progressRec = (Rectangle) ((FrameworkElement) this).FindName("progressRec");
      this.watchedGrid = (Grid) ((FrameworkElement) this).FindName("watchedGrid");
      this.thumb = (Image) ((FrameworkElement) this).FindName("thumb");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.UserControl_Tapped));
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(uiElement2.add_RightTapped), new Action<EventRegistrationToken>(uiElement2.remove_RightTapped), new RightTappedEventHandler(this.UserControl_RightTapped));
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(uiElement3.add_Holding), new Action<EventRegistrationToken>(uiElement3.remove_Holding), new HoldingEventHandler(this.UserControl_Holding));
          break;
        case 2:
          BitmapImage bitmapImage1 = (BitmapImage) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(bitmapImage1.add_ImageOpened), new Action<EventRegistrationToken>(bitmapImage1.remove_ImageOpened), new RoutedEventHandler(this.BitmapImage_ImageOpened));
          BitmapImage bitmapImage2 = (BitmapImage) target;
          WindowsRuntimeMarshal.AddEventHandler<ExceptionRoutedEventHandler>(new Func<ExceptionRoutedEventHandler, EventRegistrationToken>(bitmapImage2.add_ImageFailed), new Action<EventRegistrationToken>(bitmapImage2.remove_ImageFailed), new ExceptionRoutedEventHandler(this.BitmapImage_ImageFailed));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
