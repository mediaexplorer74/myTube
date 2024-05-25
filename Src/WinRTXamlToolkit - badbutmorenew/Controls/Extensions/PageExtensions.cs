// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.PageExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class PageExtensions
  {
    private static readonly DependencyProperty _BottomAppBarProperty = DependencyProperty.RegisterAttached("BottomAppBar", (Type) typeof (AppBar), (Type) typeof (PageExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(PageExtensions.OnBottomAppBarChanged)));
    private static readonly DependencyProperty _TopAppBarProperty = DependencyProperty.RegisterAttached("TopAppBar", (Type) typeof (AppBar), (Type) typeof (PageExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(PageExtensions.OnTopAppBarChanged)));

    public static DependencyProperty BottomAppBarProperty => PageExtensions._BottomAppBarProperty;

    public static AppBar GetBottomAppBar(DependencyObject d) => (AppBar) d.GetValue(PageExtensions.BottomAppBarProperty);

    public static void SetBottomAppBar(DependencyObject d, AppBar value) => d.SetValue(PageExtensions.BottomAppBarProperty, (object) value);

    private static async void OnBottomAppBarChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AppBar oldValue = (AppBar) e.OldValue;
      AppBar newBottomAppBar = (AppBar) d.GetValue(PageExtensions.BottomAppBarProperty);
      if (DesignMode.DesignModeEnabled)
        return;
      FrameworkElement fe = (FrameworkElement) d;
      Page parentPage = ((DependencyObject) fe).GetFirstAncestorOfType<Page>();
      if (parentPage == null)
      {
        await fe.WaitForLoadedAsync();
        parentPage = ((DependencyObject) fe).GetFirstAncestorOfType<Page>();
        if (parentPage == null)
          throw new InvalidOperationException("PageExtensions.BottomAppBar is used to set the BottomAppBar on a parent page control and so it needs to be used in a control that is hosted in a Page.");
      }
      parentPage.put_BottomAppBar(newBottomAppBar);
    }

    public static DependencyProperty TopAppBarProperty => PageExtensions._TopAppBarProperty;

    public static AppBar GetTopAppBar(DependencyObject d) => (AppBar) d.GetValue(PageExtensions.TopAppBarProperty);

    public static void SetTopAppBar(DependencyObject d, AppBar value) => d.SetValue(PageExtensions.TopAppBarProperty, (object) value);

    private static async void OnTopAppBarChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      AppBar oldValue = (AppBar) e.OldValue;
      AppBar newTopAppBar = (AppBar) d.GetValue(PageExtensions.TopAppBarProperty);
      if (DesignMode.DesignModeEnabled)
        return;
      FrameworkElement fe = (FrameworkElement) d;
      Page parentPage = ((DependencyObject) fe).GetFirstAncestorOfType<Page>();
      if (parentPage == null)
      {
        await fe.WaitForLoadedAsync();
        parentPage = ((DependencyObject) fe).GetFirstAncestorOfType<Page>();
        if (parentPage == null)
          throw new InvalidOperationException("PageExtensions.TopAppBar is used to set the TopAppBar on a parent page control and so it needs to be used in a control that is hosted in a Page.");
      }
      parentPage.put_TopAppBar(newTopAppBar);
    }
  }
}
