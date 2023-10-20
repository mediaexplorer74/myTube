// Decompiled with JetBrains decompiler
// Type: myTube.BetaPages.TileListPage
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Navigation;

namespace myTube.BetaPages
{
  public sealed class TileListPage : Page, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl list;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public TileListPage() => this.InitializeComponent();

    protected virtual async void OnNavigatedTo(NavigationEventArgs e)
    {
      if (e.NavigationMode != 1)
        this.list.put_ItemsSource((object) await SecondaryTile.FindAllAsync());
      base.OnNavigatedTo(e);
    }

    private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(sender is FrameworkElement))
        return;
      App.Instance.RootFrame.Navigate(typeof (TileInfoPage), (sender as FrameworkElement).DataContext);
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///BetaPages/TileListPage.xaml"), (ComponentResourceLocation) 0);
      this.list = (ItemsControl) ((FrameworkElement) this).FindName("list");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        UIElement uiElement = (UIElement) target;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement.add_Tapped), new Action<EventRegistrationToken>(uiElement.remove_Tapped), new TappedEventHandler(this.Grid_Tapped));
      }
      this._contentLoaded = true;
    }
  }
}
