// Decompiled with JetBrains decompiler
// Type: myTube.DownloaderPanel
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class DownloaderPanel : UserControl, IComponentConnector
  {
    private static Dictionary<YouTubeQuality, string> QualityNames;
    private const double SmallestDownloadSize = 0.025;
    private List<IDownloadPanelCallbackReceiver> callbackReceivers = new List<IDownloadPanelCallbackReceiver>();
    private List<Action<DownloadOperation>> videoProgressCallbacks = new List<Action<DownloadOperation>>();
    private List<Action<DownloadOperation>> audioProgressCallbacks = new List<Action<DownloadOperation>>();
    private Task<YouTubeInfo> downloadInfoTask;
    private Task<Dictionary<YouTubeQuality, long>> fileSizesTask;
    private VideoInfoLoader loader;
    private string lastID = "";
    private Popup context;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl main;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock audioNecessary;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl sendInfo;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle videoRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton videoSaveButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle audioRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton audioSaveButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform audioRecTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScaleTransform videoRecTrans;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public List<Action<DownloadOperation>> VideoProgressCallbacks => this.videoProgressCallbacks;

    public List<Action<DownloadOperation>> AudioProgressCallbacks => this.audioProgressCallbacks;

    public List<IDownloadPanelCallbackReceiver> CallbackReceivers => this.callbackReceivers;

    public YouTubeEntry Entry => ((FrameworkElement) this).DataContext != null && ((FrameworkElement) this).DataContext is YouTubeEntry ? ((FrameworkElement) this).DataContext as YouTubeEntry : (YouTubeEntry) null;

    public DownloaderPanel()
    {
      this.InitializeComponent();
      this.loader = new VideoInfoLoader();
      ((Control) this).put_FontFamily(((Control) DefaultPage.Current).FontFamily);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.DownloaderPanel_Loaded));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(DownloaderPanel_DataContextChanged)));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.DownloaderPanel_Unloaded));
    }

    private void DownloaderPanel_Unloaded(object sender, RoutedEventArgs e)
    {
      if (this.context == null || !this.context.IsOpen)
        return;
      this.context.put_IsOpen(false);
    }

    private void DownloaderPanel_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void showSendInfo(bool forceShow = false)
    {
      if (!forceShow && Settings.UserMode < UserMode.Beta)
        return;
      ((UIElement) this.sendInfo).put_Visibility((Visibility) 0);
    }

    private async void DownloaderPanel_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (this.Entry == null || this.Entry.ID == this.lastID)
        return;
      this.fileSizesTask = (Task<Dictionary<YouTubeQuality, long>>) null;
      this.downloadInfoTask = (Task<YouTubeInfo>) null;
      this.lastID = this.Entry.ID;
      ((UIElement) this).put_IsHitTestVisible(false);
      int num = await App.GlobalObjects.InitializedTask ? 1 : 0;
      ((UIElement) this).put_IsHitTestVisible(true);
      TransferInfo transfer = App.GlobalObjects.TransferManager.GetTransferInfo(this.Entry);
      if (transfer != null)
      {
        TransferManager.State state = await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Video);
        TransferManager.State state2 = await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Audio);
        int transferState = (int) await App.GlobalObjects.TransferManager.GetTransferState(this.Entry);
        if (state2 != TransferManager.State.None && state != TransferManager.State.None && transfer.IsAdaptive)
          ((UIElement) this.audioNecessary).put_Visibility((Visibility) 0);
        else
          ((UIElement) this.audioNecessary).put_Visibility((Visibility) 1);
        if (transferState == 0)
          this.creatTasks();
        if (state == TransferManager.State.Downloading)
        {
          DownloadOperation download = await App.GlobalObjects.TransferManager.GetDownload(transfer, TransferType.Video);
          if (download != null)
          {
            WindowsRuntimeSystemExtensions.AsTask<DownloadOperation, DownloadOperation>(download.AttachAsync(), (IProgress<DownloadOperation>) new Progress<DownloadOperation>(new Action<DownloadOperation>(this.VideoProgressCallback)));
            this.showSendInfo(download.Progress.Status == 7);
          }
        }
        if (state2 == TransferManager.State.Downloading)
        {
          DownloadOperation download = await App.GlobalObjects.TransferManager.GetDownload(transfer, TransferType.Audio);
          if (download != null)
          {
            WindowsRuntimeSystemExtensions.AsTask<DownloadOperation, DownloadOperation>(download.AttachAsync(), (IProgress<DownloadOperation>) new Progress<DownloadOperation>(new Action<DownloadOperation>(this.AudioProgressCallback)));
            this.showSendInfo(download.Progress.Status == 7);
          }
        }
        this.SetIcon(this.videoSaveButton, this.videoRecTrans, state, TransferType.Video);
        this.SetIcon(this.audioSaveButton, this.audioRecTrans, state2, TransferType.Audio);
      }
      else
        this.creatTasks();
      transfer = (TransferInfo) null;
    }

    private async void creatTasks()
    {
      if (this.downloadInfoTask == null)
      {
        this.loader.UseNavigatePage = Settings.UseNavigatePage;
        this.downloadInfoTask = this.loader.LoadInfoAllMethods(this.Entry.ID);
      }
      if (this.fileSizesTask != null)
        return;
      try
      {
        YouTubeInfo info;
        YouTubeInfo youTubeInfo = info;
        info = await this.downloadInfoTask;
        List<YouTubeQuality> list = Enumerable.ToList<YouTubeQuality>(Enumerable.Where<YouTubeQuality>((IEnumerable<YouTubeQuality>) Enumerable.ToList<YouTubeQuality>((IEnumerable<YouTubeQuality>) TransferManager.SupportedVideoQualities), (Func<YouTubeQuality, bool>) (q =>
        {
          if (q > App.HighestQuality || !info.HasQuality(q))
            return false;
          return !info.QualityIs60FPS(q) || Settings.Allow60FPS;
        })));
        if (!list.Contains(YouTubeQuality.Audio))
          list.Add(YouTubeQuality.Audio);
        this.fileSizesTask = info.GetFileSizes(list.ToArray());
      }
      catch
      {
      }
    }

    private async void SetIcon(
      IconTextButton icon,
      ScaleTransform trans,
      TransferManager.State state,
      TransferType type)
    {
      Helper.Write((object) this, (object) ("Setting icon: " + (object) type + ", " + (object) state));
      switch (state)
      {
        case TransferManager.State.Downloading:
          icon.Text = App.Strings["common.cancel", "cancel"].ToLower();
          icon.Symbol = (Symbol) 57610;
          DownloadOperation download = await App.GlobalObjects.TransferManager.GetDownload(App.GlobalObjects.TransferManager.GetTransferInfo(this.Entry), type);
          if (download != null)
          {
            try
            {
              Ani.Begin((DependencyObject) trans, "ScaleX", Math.Max(download.ProgressToDouble(), 0.025), 0.4, 4.0);
              break;
            }
            catch
            {
              Ani.Begin((DependencyObject) trans, "ScaleX", 0.025, 0.4, 4.0);
              break;
            }
          }
          else
          {
            Ani.Begin((DependencyObject) trans, "ScaleX", 0.025, 0.4, 4.0);
            break;
          }
        case TransferManager.State.Complete:
          icon.Text = App.Strings["common.delete", "delete"].ToLower();
          icon.Symbol = (Symbol) 57607;
          Ani.Begin((DependencyObject) trans, "ScaleX", 1.0, 0.4, 4.0);
          break;
        default:
          icon.Text = App.Strings["common.save", "save"].ToLower();
          icon.Symbol = (Symbol) 57624;
          Ani.Begin((DependencyObject) trans, "ScaleX", 0.0, 0.4, 4.0);
          break;
      }
    }

    private void VideoProgressCallback(DownloadOperation download)
    {
      foreach (Action<DownloadOperation> progressCallback in this.videoProgressCallbacks)
        progressCallback(download);
      double num = Math.Max((double) download.Progress.BytesReceived / (double) download.Progress.TotalBytesToReceive, 0.025);
      if (num == 1.0)
      {
        this.DoCallbacks();
        this.SetIcon(this.videoSaveButton, this.videoRecTrans, TransferManager.State.Complete, TransferType.Video);
      }
      else
      {
        if (double.IsInfinity(num))
          return;
        if (double.IsNaN(num))
          return;
        try
        {
          Ani.Begin((DependencyObject) this.videoRecTrans, "ScaleX", num, 0.4, 4.0);
        }
        catch
        {
        }
      }
    }

    private void AudioProgressCallback(DownloadOperation download)
    {
      double num = Math.Max((double) download.Progress.BytesReceived / (double) download.Progress.TotalBytesToReceive, 0.025);
      if (num == 1.0)
      {
        this.SetIcon(this.audioSaveButton, this.audioRecTrans, TransferManager.State.Complete, TransferType.Audio);
      }
      else
      {
        if (double.IsInfinity(num))
          return;
        if (double.IsNaN(num))
          return;
        try
        {
          Ani.Begin((DependencyObject) this.audioRecTrans, "ScaleX", num, 0.4, 4.0);
        }
        catch
        {
        }
      }
    }

    private async void videoSaveButton_Tapped(object sender, TappedRoutedEventArgs e) => await this.Download(TransferType.Video, this.videoSaveButton, this.videoRecTrans);

    private async void audioSaveButton_Tapped(object sender, TappedRoutedEventArgs e) => await this.Download(TransferType.Audio, this.audioSaveButton, this.audioRecTrans);

    private async Task Download(TransferType type, IconTextButton button, ScaleTransform trans)
    {
      Helper.Write((object) this, (object) ("Download method: " + (object) type + ", " + ((FrameworkElement) button).Name));
      ((UIElement) button).put_Opacity(0.5);
      ((UIElement) button).put_IsHitTestVisible(false);
      if (this.Entry != null)
      {
        TransferManager.State state = await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, type);
        switch (state)
        {
          case TransferManager.State.None:
            YouTubeInfo info;
            try
            {
              if (this.downloadInfoTask == null)
              {
                YouTubeInfo youTubeInfo = info;
                info = await (this.downloadInfoTask = this.loader.LoadInfoAllMethods(this.Entry.ID));
              }
              else
              {
                YouTubeInfo youTubeInfo = info;
                info = await this.downloadInfoTask;
              }
              info.Allow60FPS = Settings.Allow60FPS;
              info.Allow60FPS = false;
            }
            catch (Exception ex)
            {
              Helper.Write((object) ("Error getting info fopr video:\n" + (object) ex));
              new MessageDialog(App.Strings["dialogs.videos.unabletoloadinfo", "We were unable to load the info for this video"]).ShowAsync();
              this.downloadInfoTask = (Task<YouTubeInfo>) null;
              this.fileSizesTask = (Task<Dictionary<YouTubeQuality, long>>) null;
              Ani.Begin((DependencyObject) this.videoSaveButton, "Opacity", 1.0, 0.2);
              ((UIElement) this.videoSaveButton).put_IsHitTestVisible(true);
              this.DoCallbacks();
              return;
            }
            try
            {
              IUICommand command = (IUICommand) null;
              if (type == TransferType.Video)
              {
                Dictionary<YouTubeQuality, long> sizes = (Dictionary<YouTubeQuality, long>) null;
                try
                {
                  if (this.fileSizesTask == null)
                  {
                    List<YouTubeQuality> list = Enumerable.ToList<YouTubeQuality>(Enumerable.Where<YouTubeQuality>((IEnumerable<YouTubeQuality>) Enumerable.ToList<YouTubeQuality>((IEnumerable<YouTubeQuality>) TransferManager.SupportedVideoQualities), (Func<YouTubeQuality, bool>) (q =>
                    {
                      if (q > App.HighestQuality || !info.HasQuality(q))
                        return false;
                      return !info.QualityIs60FPS(q) || Settings.Allow60FPS;
                    })));
                    if (!list.Contains(YouTubeQuality.Audio))
                      list.Add(YouTubeQuality.Audio);
                    sizes = await info.GetFileSizes(list.ToArray());
                  }
                  else
                    sizes = await this.fileSizesTask;
                }
                catch
                {
                }
                PopupMenu popupMenu = new PopupMenu();
                Dictionary<YouTubeQuality, UICommand> commands = new Dictionary<YouTubeQuality, UICommand>();
                List<IconButtonEvent> iconButtonEventList1 = new List<IconButtonEvent>();
                foreach (YouTubeQuality supportedVideoQuality in TransferManager.SupportedVideoQualities)
                {
                  if (info.HasQuality(supportedVideoQuality) && DownloaderPanel.QualityNames.ContainsKey(supportedVideoQuality))
                  {
                    string qualityName = DownloaderPanel.QualityNames[supportedVideoQuality];
                    if (sizes != null && sizes.ContainsKey(supportedVideoQuality))
                    {
                      long num1 = sizes[supportedVideoQuality];
                      if (info.QualityNeedsAudio(supportedVideoQuality) && sizes.ContainsKey(YouTubeQuality.Audio))
                        num1 += sizes[YouTubeQuality.Audio];
                      double num2 = Math.Round((double) num1 / 1048576.0, 1);
                      string str = qualityName + " (" + (object) num2 + " MB)";
                      IconButtonEvent iconButtonEvent1 = new IconButtonEvent();
                      iconButtonEvent1.Symbol = (Symbol) 57624;
                      iconButtonEvent1.Text = str;
                      iconButtonEvent1.DataContext = (object) supportedVideoQuality;
                      IconButtonEvent iconButtonEvent2 = iconButtonEvent1;
                      iconButtonEventList1.Add(iconButtonEvent2);
                    }
                  }
                }
                List<IconButtonEvent> iconButtonEventList2 = iconButtonEventList1;
                IconButtonEvent iconButtonEvent = new IconButtonEvent();
                iconButtonEvent.Symbol = (Symbol) 57610;
                iconButtonEvent.Text = App.Strings["common.cancel", "cancel"].ToLower();
                iconButtonEventList2.Insert(0, iconButtonEvent);
                VideoContextMenu videoContextMenu = new VideoContextMenu()
                {
                  ItemsSource = iconButtonEventList1
                };
                Popup popup1 = new Popup();
                popup1.put_Child((UIElement) videoContextMenu);
                Popup popup2 = popup1;
                this.context = popup2;
                Rect bounds1 = (this.Content as FrameworkElement).GetBounds(Window.Current.Content);
                Rect bounds2 = ((FrameworkElement) button).GetBounds(Window.Current.Content);
                Rect visibleBounds = App.VisibleBounds;
                ((FrameworkElement) videoContextMenu).put_Width(Math.Min(bounds1.Width, 300.0));
                ((FrameworkElement) videoContextMenu).put_Height(Math.Min(visibleBounds.Height - 38.0, 361.0));
                popup2.put_HorizontalOffset(bounds1.Right - ((FrameworkElement) videoContextMenu).Width);
                popup2.put_VerticalOffset(Math.Max(0.0, bounds2.Top - ((FrameworkElement) videoContextMenu).Height + 95.0));
                popup2.put_IsOpen(true);
                DefaultPage.Current.SurpressPopupClosing = true;
                object obj = await videoContextMenu.WaitForDataContext();
                DefaultPage.Current.SurpressPopupClosing = false;
                ((FrameworkElement) button).GetBounds((UIElement) DefaultPage.Current).Center();
                if (command != null || obj is YouTubeQuality)
                {
                  YouTubeQuality qual = YouTubeQuality.HD;
                  if (command != null)
                  {
                    foreach (KeyValuePair<YouTubeQuality, UICommand> keyValuePair in commands)
                    {
                      if (keyValuePair.Value == command)
                      {
                        qual = keyValuePair.Key;
                        break;
                      }
                    }
                  }
                  else
                    qual = (YouTubeQuality) obj;
                  Helper.Write((object) this, (object) "About to download video");
                  if (info.QualityNeedsAudio(qual))
                  {
                    if (await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Audio) == TransferManager.State.None)
                    {
                      DownloadOperation downloadOperation = await App.GlobalObjects.TransferManager.StartTransfer(this.Entry, info, YouTubeQuality.Audio, new Progress<DownloadOperation>(new Action<DownloadOperation>(this.AudioProgressCallback)));
                      this.SetIcon(this.audioSaveButton, this.audioRecTrans, TransferManager.State.Downloading, TransferType.Video);
                    }
                    ((UIElement) this.audioNecessary).put_Visibility((Visibility) 0);
                  }
                  DownloadOperation downloadOperation1 = await App.GlobalObjects.TransferManager.StartTransfer(this.Entry, info, qual, new Progress<DownloadOperation>(new Action<DownloadOperation>(this.VideoProgressCallback)));
                  this.SetIcon(button, trans, TransferManager.State.Downloading, TransferType.Video);
                  Helper.Write((object) this, (object) "Video download started");
                  this.showSendInfo();
                }
                sizes = (Dictionary<YouTubeQuality, long>) null;
                commands = (Dictionary<YouTubeQuality, UICommand>) null;
              }
              else if (info.HasQuality(YouTubeQuality.Audio))
              {
                Helper.Write((object) this, (object) "About to download audio");
                DownloadOperation downloadOperation = await App.GlobalObjects.TransferManager.StartTransfer(this.Entry, info, YouTubeQuality.Audio, new Progress<DownloadOperation>(new Action<DownloadOperation>(this.AudioProgressCallback)));
                this.SetIcon(button, trans, TransferManager.State.Downloading, TransferType.Audio);
                Helper.Write((object) this, (object) "Audio download started");
                this.showSendInfo();
              }
              else
                new MessageDialog("There is no audio stream for this video").ShowAsync();
              command = (IUICommand) null;
            }
            catch (Exception ex)
            {
              Helper.Write((object) this, (object) ex);
            }
            break;
          case TransferManager.State.Downloading:
            try
            {
              if (type == TransferType.Audio)
              {
                TransferInfo transferInfo = App.GlobalObjects.TransferManager.GetTransferInfo(this.Entry);
                if (transferInfo != null && transferInfo.IsAdaptive)
                {
                  if (await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Video) != TransferManager.State.None)
                  {
                    if (await new MessageDialog(App.Strings["dialogs.videos.deleteaudioportion", "If you delete the audio portion of this file, the video portion will be deleted as well"], App.Strings["dialogs.titles.areyousure", "Are you sure?"]).ShowAsync(App.Strings["common.yes", "yes"].ToLower(), App.Strings["common.no", "no"].ToLower()) == 0)
                    {
                      int num = await App.GlobalObjects.TransferManager.DeleteTransfer(this.Entry, TransferType.Video) ? 1 : 0;
                      this.SetIcon(this.videoSaveButton, this.videoRecTrans, TransferManager.State.None, TransferType.Audio);
                    }
                    else
                    {
                      ((UIElement) button).put_Opacity(1.0);
                      ((UIElement) button).put_IsHitTestVisible(true);
                      return;
                    }
                  }
                }
              }
              int num3 = await App.GlobalObjects.TransferManager.DeleteTransfer(this.Entry, type) ? 1 : 0;
              this.SetIcon(button, trans, TransferManager.State.None, TransferType.Video);
              ((UIElement) this.audioNecessary).put_Visibility((Visibility) 1);
              break;
            }
            catch
            {
              new MessageDialog("Unable to cancel this download").ShowAsync();
              this.SetIcon(button, trans, TransferManager.State.Downloading, TransferType.Video);
              break;
            }
          case TransferManager.State.Complete:
            try
            {
              if (type == TransferType.Audio)
              {
                TransferInfo transferInfo = App.GlobalObjects.TransferManager.GetTransferInfo(this.Entry);
                if (transferInfo != null && transferInfo.IsAdaptive)
                {
                  if (await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Video) != TransferManager.State.None)
                  {
                    if (await new MessageDialog(App.Strings["dialogs.videos.deleteaudioportion", "If you delete the audio portion of this file, the video portion will be deleted as well"], App.Strings["dialogs.titles.areyousure", "Are you sure?"]).ShowAsync(App.Strings["common.yes", "yes"].ToLower(), App.Strings["common.no", "no"].ToLower()) == 0)
                    {
                      int num = await App.GlobalObjects.TransferManager.DeleteTransfer(this.Entry, TransferType.Video) ? 1 : 0;
                      this.SetIcon(this.videoSaveButton, this.videoRecTrans, TransferManager.State.None, TransferType.Audio);
                    }
                    else
                    {
                      ((UIElement) button).put_Opacity(1.0);
                      ((UIElement) button).put_IsHitTestVisible(true);
                      return;
                    }
                  }
                }
              }
              ((UIElement) this.audioNecessary).put_Visibility((Visibility) 1);
              int num4 = await App.GlobalObjects.TransferManager.DeleteTransfer(this.Entry, type) ? 1 : 0;
              this.SetIcon(button, trans, TransferManager.State.None, type);
              break;
            }
            catch
            {
              new MessageDialog("Unable to delete this file").ShowAsync();
              this.SetIcon(button, trans, TransferManager.State.Complete, type);
              break;
            }
        }
      }
      this.DoCallbacks();
      Ani.Begin((DependencyObject) button, "Opacity", 1.0, 0.2);
      ((UIElement) button).put_IsHitTestVisible(true);
    }

    private void DoCallbacks()
    {
      if (this.callbackReceivers == null)
        return;
      using (List<IDownloadPanelCallbackReceiver>.Enumerator enumerator = this.callbackReceivers.GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.DownloadPanelCallback(this);
      }
    }

    private async void sendInfoToDev()
    {
      if (this.Entry == null)
        return;
      TransferInfo t = App.GlobalObjects.TransferManager.GetTransferInfo(this.Entry);
      TransferManager.State vstate = await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Video);
      TransferManager.State transferState = await App.GlobalObjects.TransferManager.GetTransferState(this.Entry, TransferType.Audio);
      if (t != null)
      {
        string s = "";
        s = s + "Title: " + this.Entry.Title + "\n";
        s = s + "ID: " + this.Entry.ID + "\n";
        s = s + "myTube Uri: mytube:Video?ID=" + this.Entry.ID + "\n";
        s = s + "Video/Audio Transfer State: " + (object) vstate + " / " + (object) transferState + "\n";
        DownloadOperation download1 = await App.GlobalObjects.TransferManager.GetDownload(t, TransferType.Video);
        if (download1 != null)
        {
          s += "Video Download Operation:\n===========\n";
          s = this.appendTransferInfo(s, download1);
        }
        else
          s += "Video Download Operation: None\n";
        DownloadOperation download2 = await App.GlobalObjects.TransferManager.GetDownload(t, TransferType.Audio);
        if (download2 != null)
        {
          s += "Audio Download Operation:\n===========\n";
          s = this.appendTransferInfo(s, download2);
        }
        else
          s += "Audio Download Operation: None\n";
        App.SendSupportEmail("myTube Background Transfer Info", s);
        s = (string) null;
      }
      t = (TransferInfo) null;
    }

    private async void sendInfo_Tapped(object sender, TappedRoutedEventArgs e) => this.sendInfoToDev();

    private string appendTransferInfo(string s, DownloadOperation download)
    {
      ResponseInformation responseInformation = download.GetResponseInformation();
      s = s + "Method: " + download.Method + "\n";
      s = s + "Total Size (bytes): " + (object) download.Progress.TotalBytesToReceive + "\n";
      s = s + "Bytes Received (bytes): " + (object) download.Progress.BytesReceived + "(" + (object) (download.ProgressToDouble() * 100.0) + " percent)\n";
      s = s + "File Path: " + ((IStorageItem) download.ResultFile).Path + "\n";
      s = s + "Status: " + (object) download.Progress.Status + "\n";
      s = s + "Has Restarted: " + download.Progress.HasRestarted.ToString() + "\n";
      s = s + "Has Response Changed: " + download.Progress.HasResponseChanged.ToString() + "\n";
      s = s + "Priority: " + (object) download.Priority + "\n";
      s = s + "Cost Policy: " + (object) download.CostPolicy + "\n";
      if (responseInformation != null)
      {
        s = s + "Resumable: " + responseInformation.IsResumable.ToString() + "\n";
        s = s + "Status code: " + (object) responseInformation.StatusCode + "\n";
        s += "Headers:\n";
        foreach (KeyValuePair<string, string> header in (IEnumerable<KeyValuePair<string, string>>) responseInformation.Headers)
          s = s + "    " + header.Key + ": " + header.Value + "\n";
        s += "\n";
      }
      else
        s += "\nNo response Info\n";
      s += "----------\n\n";
      return s;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///DownloaderPanel.xaml"), (ComponentResourceLocation) 0);
      this.main = (UserControl) ((FrameworkElement) this).FindName("main");
      this.audioNecessary = (TextBlock) ((FrameworkElement) this).FindName("audioNecessary");
      this.sendInfo = (ContentControl) ((FrameworkElement) this).FindName("sendInfo");
      this.videoRec = (Rectangle) ((FrameworkElement) this).FindName("videoRec");
      this.videoSaveButton = (IconTextButton) ((FrameworkElement) this).FindName("videoSaveButton");
      this.audioRec = (Rectangle) ((FrameworkElement) this).FindName("audioRec");
      this.audioSaveButton = (IconTextButton) ((FrameworkElement) this).FindName("audioSaveButton");
      this.audioRecTrans = (ScaleTransform) ((FrameworkElement) this).FindName("audioRecTrans");
      this.videoRecTrans = (ScaleTransform) ((FrameworkElement) this).FindName("videoRecTrans");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.sendInfo_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.videoSaveButton_Tapped));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.audioSaveButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }

    static DownloaderPanel()
    {
      Dictionary<YouTubeQuality, string> dictionary = new Dictionary<YouTubeQuality, string>();
      dictionary.Add(YouTubeQuality.LQ, "240p");
      dictionary.Add(YouTubeQuality.HQ, "360p");
      dictionary.Add(YouTubeQuality.SD, "480p");
      dictionary.Add(YouTubeQuality.HD, "720p");
      dictionary.Add(YouTubeQuality.HD1080, "1080p");
      dictionary.Add(YouTubeQuality.HD60, "720p60");
      dictionary.Add(YouTubeQuality.HD1080p60, "1080p60");
      dictionary.Add(YouTubeQuality.HD1440, "1440p");
      dictionary.Add(YouTubeQuality.HD2160, "2160p");
      DownloaderPanel.QualityNames = dictionary;
    }
  }
}
