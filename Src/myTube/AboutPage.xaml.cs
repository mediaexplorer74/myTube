// myTube.AboutPage

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using myTube.BetaPages;
using myTube.ExceptionPages;
using myTube.MessageDialogs;
using myTube.ProductKeyPages;
using myTube.UserPages;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml.Documents;

namespace myTube
{
    public sealed partial class AboutPage : Page
    {  
        public AboutPage()
        {
            this.InitializeComponent();
       
            ((FrameworkElement)this).SizeChanged += new SizeChangedEventHandler(this.AboutPage_SizeChanged);
            ((FrameworkElement)this).SizeChanged -= new SizeChangedEventHandler(this.AboutPage_SizeChanged);


            this.NavigationCacheMode = (NavigationCacheMode)2;

            if (Settings.UserMode < UserMode.Owner)
            {
                ((UIElement)this.addNewMessage).Visibility = ((Visibility)1);
                ((UIElement)this.translationFormat).Visibility = ((Visibility)1);
                ((UIElement)this.manageKeys).Visibility = ((Visibility)1);
                ((UIElement)this.manageUsers).Visibility = ((Visibility)1);
            }

            if (Settings.UserMode < UserMode.Beta)
            {
                ((UIElement)this.translate).Visibility = ((Visibility)1);
                ((UIElement)this.tiles).Visibility = ((Visibility)1);
                ((UIElement)this.exceptions).Visibility = ((Visibility)1);
            }
            
            if ((App.PlatformType != PlatformType.WindowsPhone || !App.IsTrial
                      && Settings.ProductKey == null || App.PlatformType != PlatformType.Windows
                      || Settings.ProductKey == null) && Settings.UserMode < UserMode.Owner)
                ((UIElement)this.productKey).Visibility = ((Visibility)1);
            
            if (App.IsTrial && App.PlatformType == PlatformType.WindowsPhone || Settings.ProductKey != null
                      || Settings.ProductKeyRequestId != null || Settings.UserMode >= UserMode.Owner)
                ((UIElement)this.productKey).put_Visibility((Visibility)0);

            if (Settings.UserMode == UserMode.Beta)
                ((UIElement)this.joinBeta).put_Visibility((Visibility)1);

            this.Loaded += this.AboutPage_Loaded;
        }

        private void AboutPage_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (UIElement child in (IEnumerable<UIElement>)((Panel)this.stackPanel).Children)
            {
                if (child is ContentControl contentControl)
                    ((Control)contentControl).FontFamily = new FontFamily("Segoe WP");
            }

            this.Loaded -= this.AboutPage_Loaded;
        }

        private void AboutPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrameworkElement content = ((ContentControl)this.scrollViewer).Content as FrameworkElement;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e) => base.OnNavigatedTo(e);

        private void addNewMessage_Tapped(object sender, TappedRoutedEventArgs e)
                => App.Instance.RootFrame.Navigate(typeof(NewMessage));

        private void viewRecentMessages_Tapped(object sender, TappedRoutedEventArgs e)
                => App.Instance.RootFrame.Navigate(typeof(RecentMessages));

        private void translationFormat_Tapped(object sender, TappedRoutedEventArgs e)
                => App.Instance.RootFrame.Navigate(typeof(StringFormatPage), (object)StringFormatCollection.GlobalCollection);

        private void translate_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof(TranslationPage), (object)StringFormatCollection.GlobalCollection);

        private void tiles_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof(TileListPage));

        private void productKey_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof(KeyRequestPage));

        private void manageKeys_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof(KeyManagementPage));

        private void manageUsers_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof(UserManagement));

        private void exceptions_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof(VersionsPage));

        private async void joinBeta_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int num = await Launcher.LaunchUriAsync(
                new Uri("https://www.microsoft.com/store/apps/9wzdncrdt29j")) ? 1 : 0;
        }

        private async void twitterLink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            Uri uri = new Uri("twitter://user?screen_name=RykenApps");
            LauncherOptions launcherOptions = new LauncherOptions();
            launcherOptions.FallbackUri = new Uri("https://twitter.com/RykenApps");
            int num = await Launcher.LaunchUriAsync(uri, launcherOptions) ? 1 : 0;
        }
      
    }
}
