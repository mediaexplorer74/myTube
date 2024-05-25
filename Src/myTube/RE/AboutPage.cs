// Decompiled with JetBrains decompiler
// Type: myTube.AboutPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.BetaPages;
using myTube.ExceptionPages;
using myTube.MessageDialogs;
using myTube.ProductKeyPages;
using myTube.UserPages;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public sealed class AboutPage : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scrollViewer;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private OverCanvas overCanvas;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private StackPanel stackPanel;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl viewRecentMessages;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl joinBeta;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl addNewMessage;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl translationFormat;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl translate;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl tiles;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl manageUsers;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl productKey;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl manageKeys;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl exceptions;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Hyperlink twitterLink;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public AboutPage()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(
          new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_SizeChanged), 
          new Action<EventRegistrationToken>(((FrameworkElement) this).remove_SizeChanged), 
          new SizeChangedEventHandler(this.AboutPage_SizeChanged));

      this.put_NavigationCacheMode((NavigationCacheMode) 2);
      if (Settings.UserMode < UserMode.Owner)
      {
        ((UIElement) this.addNewMessage).put_Visibility((Visibility) 1);
        ((UIElement) this.translationFormat).put_Visibility((Visibility) 1);
        ((UIElement) this.manageKeys).put_Visibility((Visibility) 1);
        ((UIElement) this.manageUsers).put_Visibility((Visibility) 1);
      }
      if (Settings.UserMode < UserMode.Beta)
      {
        ((UIElement) this.translate).put_Visibility((Visibility) 1);
        ((UIElement) this.tiles).put_Visibility((Visibility) 1);
        ((UIElement) this.exceptions).put_Visibility((Visibility) 1);
      }
      if ((App.PlatformType != PlatformType.WindowsPhone || !App.IsTrial 
                && Settings.ProductKey == null || App.PlatformType != PlatformType.Windows 
                || Settings.ProductKey == null) && Settings.UserMode < UserMode.Owner)
        ((UIElement) this.productKey).put_Visibility((Visibility) 1);
      if (App.IsTrial && App.PlatformType == PlatformType.WindowsPhone || Settings.ProductKey != null 
                || Settings.ProductKeyRequestId != null || Settings.UserMode >= UserMode.Owner)
        ((UIElement) this.productKey).put_Visibility((Visibility) 0);

      if (Settings.UserMode == UserMode.Beta)
        ((UIElement) this.joinBeta).put_Visibility((Visibility) 1);
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(
          new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), 
          new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), 
          new RoutedEventHandler(this.AboutPage_Loaded));
    }

    private void AboutPage_Loaded(object sender, RoutedEventArgs e)
    {
      foreach (UIElement child in (IEnumerable<UIElement>) ((Panel) this.stackPanel).Children)
      {
        if (child is ContentControl contentControl)
          ((Control) contentControl).put_FontFamily(new FontFamily("Segoe WP"));
      }
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(new Action<EventRegistrationToken>(
          ((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.AboutPage_Loaded));
    }

    private void AboutPage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      FrameworkElement content = ((ContentControl) this.scrollViewer).Content as FrameworkElement;
    }

    protected virtual async void OnNavigatedTo(NavigationEventArgs e) => base.OnNavigatedTo(e);

    private void addNewMessage_Tapped(object sender, TappedRoutedEventArgs e) 
            => App.Instance.RootFrame.Navigate(typeof (NewMessage));

    private void viewRecentMessages_Tapped(object sender, TappedRoutedEventArgs e) 
            => App.Instance.RootFrame.Navigate(typeof (RecentMessages));

    private void translationFormat_Tapped(object sender, TappedRoutedEventArgs e) 
            => App.Instance.RootFrame.Navigate(typeof (StringFormatPage), (object) StringFormatCollection.GlobalCollection);

    private void translate_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (TranslationPage), (object) StringFormatCollection.GlobalCollection);

    private void tiles_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (TileListPage));

    private void productKey_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (KeyRequestPage));

    private void manageKeys_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (KeyManagementPage));

    private void manageUsers_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (UserManagement));

    private void exceptions_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (VersionsPage));

    private async void joinBeta_Tapped(object sender, TappedRoutedEventArgs e)
    {
      int num = await Launcher.LaunchUriAsync(new Uri("https://www.microsoft.com/store/apps/9wzdncrdt29j")) ? 1 : 0;
    }

    private async void twitterLink_Click(Hyperlink sender, HyperlinkClickEventArgs args)
    {
      Uri uri = new Uri("twitter://user?screen_name=RykenApps");
      LauncherOptions launcherOptions = new LauncherOptions();
      launcherOptions.put_FallbackUri(new Uri("https://twitter.com/RykenApps"));
      int num = await Launcher.LaunchUriAsync(uri, launcherOptions) ? 1 : 0;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///AboutPage.xaml"), (ComponentResourceLocation) 0);
      this.scrollViewer = (ScrollViewer) ((FrameworkElement) this).FindName("scrollViewer");
      this.overCanvas = (OverCanvas) ((FrameworkElement) this).FindName("overCanvas");
      this.stackPanel = (StackPanel) ((FrameworkElement) this).FindName("stackPanel");
      this.viewRecentMessages = (ContentControl) ((FrameworkElement) this).FindName("viewRecentMessages");
      this.joinBeta = (ContentControl) ((FrameworkElement) this).FindName("joinBeta");
      this.addNewMessage = (ContentControl) ((FrameworkElement) this).FindName("addNewMessage");
      this.translationFormat = (ContentControl) ((FrameworkElement) this).FindName("translationFormat");
      this.translate = (ContentControl) ((FrameworkElement) this).FindName("translate");
      this.tiles = (ContentControl) ((FrameworkElement) this).FindName("tiles");
      this.manageUsers = (ContentControl) ((FrameworkElement) this).FindName("manageUsers");
      this.productKey = (ContentControl) ((FrameworkElement) this).FindName("productKey");
      this.manageKeys = (ContentControl) ((FrameworkElement) this).FindName("manageKeys");
      this.exceptions = (ContentControl) ((FrameworkElement) this).FindName("exceptions");
      this.twitterLink = (Hyperlink) ((FrameworkElement) this).FindName("twitterLink");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.viewRecentMessages_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.joinBeta_Tapped));
          break;
        case 3:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.addNewMessage_Tapped));
          break;
        case 4:
          UIElement uiElement4 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement4.add_Tapped), new Action<EventRegistrationToken>(uiElement4.remove_Tapped), new TappedEventHandler(this.translationFormat_Tapped));
          break;
        case 5:
          UIElement uiElement5 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement5.add_Tapped), new Action<EventRegistrationToken>(uiElement5.remove_Tapped), new TappedEventHandler(this.translate_Tapped));
          break;
        case 6:
          UIElement uiElement6 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement6.add_Tapped), new Action<EventRegistrationToken>(uiElement6.remove_Tapped), new TappedEventHandler(this.tiles_Tapped));
          break;
        case 7:
          UIElement uiElement7 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement7.add_Tapped), new Action<EventRegistrationToken>(uiElement7.remove_Tapped), new TappedEventHandler(this.manageUsers_Tapped));
          break;
        case 8:
          UIElement uiElement8 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement8.add_Tapped), new Action<EventRegistrationToken>(uiElement8.remove_Tapped), new TappedEventHandler(this.productKey_Tapped));
          break;
        case 9:
          UIElement uiElement9 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement9.add_Tapped), new Action<EventRegistrationToken>(uiElement9.remove_Tapped), new TappedEventHandler(this.manageKeys_Tapped));
          break;
        case 10:
          UIElement uiElement10 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement10.add_Tapped), new Action<EventRegistrationToken>(uiElement10.remove_Tapped), new TappedEventHandler(this.exceptions_Tapped));
          break;
        case 11:
          Hyperlink hyperlink = (Hyperlink) target;
          // ISSUE: method pointer
          WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>>(new Func<TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>, EventRegistrationToken>(hyperlink.add_Click), new Action<EventRegistrationToken>(hyperlink.remove_Click), new TypedEventHandler<Hyperlink, HyperlinkClickEventArgs>((object) this, __methodptr(twitterLink_Click)));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
