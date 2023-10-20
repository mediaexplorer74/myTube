// Decompiled with JetBrains decompiler
// Type: myTube.ExceptionPages.VersionListPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Cloud;
using myTube.Cloud.Clients;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
  public sealed class VersionListPage : Page, IComponentConnector
  {
    private ExceptionClient client;
    private ObservableCollection<ExceptionData> exceptions;
    private int page;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ScrollViewer scroll;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl itemsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public VersionListPage()
    {
      this.InitializeComponent();
      this.client = new ExceptionClient();
      this.exceptions = new ObservableCollection<ExceptionData>();
      this.itemsControl.put_ItemsSource((object) this.exceptions);
      this.put_NavigationCacheMode((NavigationCacheMode) 2);
    }

    private async void Refresh()
    {
      ((Collection<ExceptionData>) this.exceptions).Clear();
      this.page = 0;
      try
      {
        this.Load();
      }
      catch
      {
      }
    }

    private async void Load()
    {
      if ((object) (((FrameworkElement) this).DataContext as Version) == null)
        return;
      try
      {
        ExceptionData[] latestForVersion = await this.client.GetLatestForVersion(this.page, 20, ((FrameworkElement) this).DataContext as Version);
        ++this.page;
        ((IList<ExceptionData>) this.exceptions).Add<ExceptionData>((IList<ExceptionData>) latestForVersion);
      }
      catch
      {
      }
    }

    protected virtual void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.Parameter != ((FrameworkElement) this).DataContext)
      {
        OverCanvas.SetOverCanvasTitle((DependencyObject) this.scroll, e.Parameter.ToString());
        ((FrameworkElement) this).put_DataContext(e.Parameter);
        this.Refresh();
      }
      base.OnNavigatedTo(e);
    }

    private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e) => App.Instance.RootFrame.Navigate(typeof (ExceptionDetails), (sender as FrameworkElement).DataContext);

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ExceptionPages/VersionListPage.xaml"), (ComponentResourceLocation) 0);
      this.scroll = (ScrollViewer) ((FrameworkElement) this).FindName("scroll");
      this.itemsControl = (ItemsControl) ((FrameworkElement) this).FindName("itemsControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.StackPanel_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
