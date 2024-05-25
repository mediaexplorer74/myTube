// myTube.ShareTargets.ShareLinkPage

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
  public sealed partial class ShareLinkPage : Page
  {
    private ShareOperation share;
        public FrameworkElement icon;
        public ListView itemsList;

        public ShareLinkPage()
    {
      //this.InitializeComponent();
      //StatusBar.GetForCurrentView().HideAsync();
      //((Control) this).put_FontFamily(new FontFamily("Segoe WP"));
      //((ICollection<object>) this.itemsList.Items).Clear();
      //((UIElement) this.noVideosText).put_Visibility((Visibility) 1);
      //WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.ShareLinkPage_Loaded));
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

    private void YouTube_SignedIn(object sender, EventArgs e)
    {
        //((DependencyObject)this).Dispatcher.RunAsync((CoreDispatcherPriority)0, 
        //    new DispatchedHandler((object)this, __methodptr(\u003CYouTube_SignedIn\u003Eb__4_0)));
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter is ShareOperation)
        this.share = (ShareOperation) e.Parameter;
      if (this.share != null)
      {
        if (Enumerable.Contains<string>((IEnumerable<string>) this.share.Data.AvailableFormats, 
            StandardDataFormats.WebLink))
        {
          Task.Run((Action) (() => this.share.ReportStarted()));
          Uri webLinkAsync = await this.share.Data.GetWebLinkAsync();
          Task.Run((Action) (() => this.share.ReportDataRetrieved()));
          //this.urlText.Text = (webLinkAsync.OriginalString);
          //this.progress.IsIndeterminate = true;
          //((Control) this.progress).put_IsEnabled(true);
          string[] fromUrl = await YouTubeURLHelper.FindFromURL(webLinkAsync.OriginalString);
          if (fromUrl.Length != 0)
          {
            this.AddItems(fromUrl);
          }
          else
          {
            //((UIElement) this.noVideosText).Opacity = (0.0);
            //((UIElement) this.noVideosText).Visibility = ((Visibility) 0);
            //Ani.Begin((DependencyObject) this.noVideosText, "Opacity", 1.0, 0.3);
          }
          //Storyboard storyboard = Ani.Begin((DependencyObject) this.progress, "Opacity", 0.0, 0.3);
          //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, 
          //    EventRegistrationToken>(((Timeline) storyboard).add_Completed), 
          //    new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), 
          //    (EventHandler<object>) ((_param1, _param2) => ((UIElement) this.progress).Visibility = ((Visibility) 1)));
        }
        if (!YouTube.IsSignedIn)
        {
          //((UIElement) this.addButton).IsHitTestVisible = false;
          //Ani.Begin((DependencyObject) this.addButton, "Opacity", 0.5, 0.1);
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
      e.Handled = true;
      if (!(sender is FrameworkElement frameworkElement))
        return;
      if (!(frameworkElement.DataContext is YouTubeEntry dataContext))
        return;
      try
      {
        Uri uri = new Uri("rykentube:Video?ID=" + dataContext.ID, UriKind.Absolute);
        LauncherOptions launcherOptions = new LauncherOptions();
        launcherOptions.DisplayApplicationPicker = true;
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

    private void SetWidthToHeightSizeChanged(object sender, SizeChangedEventArgs e)
    {
        (sender as FrameworkElement).Width = (sender as FrameworkElement).ActualHeight;
    }

    private async void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
            YouTubeEntry ent = default;
      //if (!(sender is IconTextButton icon))
      //  return;
            if (((FrameworkElement) icon).DataContext is YouTubeEntry)
             {
                /*
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
               //int num = await YouTube.WatchLater(ent.ID, Settings.WatchLater.AddVideosTo == PlaylistPosition.Beginning ? 0 : -1) ? 1 : 0;
               //icon.Symbol = (Symbol) 57611;
                */
             }
            
          ent = (YouTubeEntry) null;
    }

    private void IconTextButton_Loaded(object sender, RoutedEventArgs e)
    {
      if (SharedSettings.CurrentAccount != null || !(sender is FrameworkElement frameworkElement))
        return;
      ((UIElement) frameworkElement).Visibility = (Visibility) 1;
    }
  
  }
}
