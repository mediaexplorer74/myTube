// Decompiled with JetBrains decompiler
// Type: myTube.ShareTargets.ShareLinkPage
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
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace myTube.ShareTargets
{
  public sealed class ShareLinkPage : Page, IComponentConnector
  {
    private ShareOperation share;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid layout;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock urlText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar progress;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock noVideosText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock doneText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl addButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressRing progressRing;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Grid addButtonGrid;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TextBlock addText;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl itemsList;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl allContent;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl noneContent;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ShareLinkPage()
    {
      this.InitializeComponent();
      StatusBar.GetForCurrentView().HideAsync();
      ((Control) this).put_FontFamily(new FontFamily("Segoe WP"));
      ((ICollection<object>) this.itemsList.Items).Clear();
      ((UIElement) this.noVideosText).put_Visibility((Visibility) 1);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.ShareLinkPage_Loaded));
    }

    private void ShareLinkPage_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private void YouTube_SignInFailed(object sender, SignedInFailedEventArgs e)
    {
      if (this.share == null)
        return;
      Task.Run((Action) (() => this.share.ReportError("Unable to sign you in.")));
    }

    private void YouTube_SignedIn(object sender, EventArgs e) => ((DependencyObject) this).Dispatcher.RunAsync((CoreDispatcherPriority) 0, new DispatchedHandler((object) this, __methodptr(\u003CYouTube_SignedIn\u003Eb__4_0)));

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter is ShareOperation)
        this.share = (ShareOperation) e.Parameter;
      if (this.share != null)
      {
        if (Enumerable.Contains<string>((IEnumerable<string>) this.share.Data.AvailableFormats, StandardDataFormats.WebLink))
        {
          Task.Run((Action) (() => this.share.ReportStarted()));
          Uri webLinkAsync = await this.share.Data.GetWebLinkAsync();
          Task.Run((Action) (() => this.share.ReportDataRetrieved()));
          this.urlText.put_Text(webLinkAsync.OriginalString);
          this.progress.put_IsIndeterminate(true);
          ((Control) this.progress).put_IsEnabled(true);
          string[] fromUrl = await YouTubeURLHelper.FindFromURL(webLinkAsync.OriginalString);
          if (fromUrl.Length != 0)
          {
            this.AddItems(fromUrl);
          }
          else
          {
            ((UIElement) this.noVideosText).put_Opacity(0.0);
            ((UIElement) this.noVideosText).put_Visibility((Visibility) 0);
            Ani.Begin((DependencyObject) this.noVideosText, "Opacity", 1.0, 0.3);
          }
          Storyboard storyboard = Ani.Begin((DependencyObject) this.progress, "Opacity", 0.0, 0.3);
          WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.progress).put_Visibility((Visibility) 1)));
        }
        if (!YouTube.IsSignedIn)
        {
          ((UIElement) this.addButton).put_IsHitTestVisible(false);
          Ani.Begin((DependencyObject) this.addButton, "Opacity", 0.5, 0.1);
          if (SharedSettings.CurrentAccount != null)
          {
            YouTube.SignedIn += new EventHandler<SignedInEventArgs>(this.YouTube_SignedIn);
            YouTube.SignInFailed += new EventHandler<SignedInFailedEventArgs>(this.YouTube_SignInFailed);
            YouTube.RefreshSignIn(SharedSettings.CurrentAccount.RefreshToken);
          }
          else if (this.share != null)
            Task.Run((Action) (() => this.share.ReportError("You must be signed in.")));
        }
      }
      base.OnNavigatedTo(e);
    }

    private async void AddItems(string[] ids)
    {
      YouTubeEntryClient youTubeEntryClient = new YouTubeEntryClient();
      try
      {
        foreach (object obj in await youTubeEntryClient.GetBatchedInfo(ids))
          ((ICollection<object>) this.itemsList.Items).Add(obj);
      }
      catch
      {
      }
    }

    private async void addButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private void listItemHolding(object sender, HoldingRoutedEventArgs e)
    {
    }

    private async void playTapped(object sender, TappedRoutedEventArgs e)
    {
      e.put_Handled(true);
      if (!(sender is FrameworkElement frameworkElement))
        return;
      if (!(frameworkElement.DataContext is YouTubeEntry dataContext))
        return;
      try
      {
        Uri uri = new Uri("rykentube:Video?ID=" + dataContext.ID, UriKind.Absolute);
        LauncherOptions launcherOptions = new LauncherOptions();
        launcherOptions.put_DisplayApplicationPicker(true);
        int num = await Launcher.LaunchUriAsync(uri, launcherOptions) ? 1 : 0;
      }
      catch
      {
      }
      Task.Run((Action) (() => this.share.ReportCompleted()));
    }

    private void noneContent_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private void allContent_Tapped(object sender, TappedRoutedEventArgs e)
    {
    }

    private void SetWidthToHeightSizeChanged(object sender, SizeChangedEventArgs e) => (sender as FrameworkElement).put_Width((sender as FrameworkElement).ActualHeight);

    private async void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is IconTextButton icon))
        return;
      if (((FrameworkElement) icon).DataContext is YouTubeEntry ent)
      {
        ((UIElement) icon).put_IsHitTestVisible(false);
        Ani.Begin((DependencyObject) icon, "Opacity", 0.5, 0.2);
        if (!YouTube.IsSignedIn)
        {
          try
          {
            if (await YouTube.RefreshSignIn(SharedSettings.CurrentAccount.RefreshToken) == null)
              return;
          }
          catch
          {
            return;
          }
        }
        int num = await YouTube.WatchLater(ent.ID, Settings.WatchLater.AddVideosTo == PlaylistPosition.Beginning ? 0 : -1) ? 1 : 0;
        icon.Symbol = (Symbol) 57611;
      }
      ent = (YouTubeEntry) null;
    }

    private void IconTextButton_Loaded(object sender, RoutedEventArgs e)
    {
      if (SharedSettings.CurrentAccount != null || !(sender is FrameworkElement frameworkElement))
        return;
      ((UIElement) frameworkElement).put_Visibility((Visibility) 1);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ShareTargets/ShareLinkPage.xaml"), (ComponentResourceLocation) 0);
      this.layout = (Grid) ((FrameworkElement) this).FindName("layout");
      this.urlText = (TextBlock) ((FrameworkElement) this).FindName("urlText");
      this.progress = (ProgressBar) ((FrameworkElement) this).FindName("progress");
      this.noVideosText = (TextBlock) ((FrameworkElement) this).FindName("noVideosText");
      this.doneText = (TextBlock) ((FrameworkElement) this).FindName("doneText");
      this.addButton = (ContentControl) ((FrameworkElement) this).FindName("addButton");
      this.progressRing = (ProgressRing) ((FrameworkElement) this).FindName("progressRing");
      this.addButtonGrid = (Grid) ((FrameworkElement) this).FindName("addButtonGrid");
      this.addText = (TextBlock) ((FrameworkElement) this).FindName("addText");
      this.itemsList = (ItemsControl) ((FrameworkElement) this).FindName("itemsList");
      this.allContent = (ContentControl) ((FrameworkElement) this).FindName("allContent");
      this.noneContent = (ContentControl) ((FrameworkElement) this).FindName("noneContent");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.playTapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.IconTextButton_Tapped));
          FrameworkElement frameworkElement1 = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(frameworkElement1.add_Loaded), new Action<EventRegistrationToken>(frameworkElement1.remove_Loaded), new RoutedEventHandler(this.IconTextButton_Loaded));
          break;
        case 3:
          FrameworkElement frameworkElement2 = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(frameworkElement2.add_SizeChanged), new Action<EventRegistrationToken>(frameworkElement2.remove_SizeChanged), new SizeChangedEventHandler(this.SetWidthToHeightSizeChanged));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(uiElement3.add_Holding), new Action<EventRegistrationToken>(uiElement3.remove_Holding), new HoldingEventHandler(this.listItemHolding));
          break;
        case 5:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.playTapped));
          break;
        case 6:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.addButton_Tapped));
          break;
        case 7:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.allContent_Tapped));
          break;
        case 8:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement7.add_Tapped), new Action<EventRegistrationToken>(uiElement7.remove_Tapped), new TappedEventHandler(this.noneContent_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
