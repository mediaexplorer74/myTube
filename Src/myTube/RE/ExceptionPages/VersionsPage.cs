// Decompiled with JetBrains decompiler
// Type: myTube.ExceptionPages.VersionsPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.ExceptionPages
{
  public sealed class VersionsPage : Page, IComponentConnector
  {
    private ExceptionClient client;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ProgressBar loadingBar;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl versionsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public VersionsPage()
    {
      this.InitializeComponent();
      this.client = new ExceptionClient();
      this.put_NavigationCacheMode((NavigationCacheMode) 2);
    }

    private async void Refresh()
    {
      this.loadingBar.put_IsIndeterminate(true);
      try
      {
        this.versionsControl.put_ItemsSource((object) await this.client.GetVersions());
      }
      catch
      {
      }
      this.loadingBar.put_IsIndeterminate(false);
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (e.NavigationMode == 1)
        return;
      this.Refresh();
    }

    private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (VersionListPage), (sender as FrameworkElement).DataContext);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ExceptionPages/VersionsPage.xaml"), (ComponentResourceLocation) 0);
      this.loadingBar = (ProgressBar) ((FrameworkElement) this).FindName("loadingBar");
      this.versionsControl = (ItemsControl) ((FrameworkElement) this).FindName("versionsControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.TextBlock_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
