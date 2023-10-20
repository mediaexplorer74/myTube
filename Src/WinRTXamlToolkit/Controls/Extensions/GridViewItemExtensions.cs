// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.GridViewItemExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class GridViewItemExtensions
  {
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", (Type) typeof (bool), (Type) typeof (GridViewItemExtensions), new PropertyMetadata((object) true, new PropertyChangedCallback(GridViewItemExtensions.OnIsEnabledChanged)));
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.RegisterAttached("IsSelected", (Type) typeof (bool), (Type) typeof (GridViewItemExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(GridViewItemExtensions.OnIsSelectedChanged)));

    public static bool GetIsEnabled(DependencyObject d) => (bool) d.GetValue(GridViewItemExtensions.IsEnabledProperty);

    public static void SetIsEnabled(DependencyObject d, bool value) => d.SetValue(GridViewItemExtensions.IsEnabledProperty, (object) value);

    private static async void OnIsEnabledChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      int num = (bool) e.OldValue ? 1 : 0;
      bool newIsEnabled = (bool) d.GetValue(GridViewItemExtensions.IsEnabledProperty);
      if (!d.IsInVisualTree())
        await ((FrameworkElement) d).WaitForLoadedAsync();
      if (!(d is GridViewItem gridViewItem1))
        gridViewItem1 = ((IEnumerable) d.GetAncestors()).OfType<GridViewItem>().FirstOrDefault<GridViewItem>();
      GridViewItem gridViewItem = gridViewItem1;
      ((Control) gridViewItem)?.put_IsEnabled(newIsEnabled);
    }

    public static bool GetIsSelected(DependencyObject d) => (bool) d.GetValue(GridViewItemExtensions.IsSelectedProperty);

    public static void SetIsSelected(DependencyObject d, bool value) => d.SetValue(GridViewItemExtensions.IsSelectedProperty, (object) value);

    private static async void OnIsSelectedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      int num = (bool) e.OldValue ? 1 : 0;
      bool newIsSelected = (bool) d.GetValue(GridViewItemExtensions.IsSelectedProperty);
      if (!d.IsInVisualTree())
        await ((FrameworkElement) d).WaitForLoadedAsync();
      if (!(d is GridViewItem gridViewItem1))
        gridViewItem1 = ((IEnumerable) d.GetAncestors()).OfType<GridViewItem>().FirstOrDefault<GridViewItem>();
      GridViewItem gridViewItem = gridViewItem1;
      ((SelectorItem) gridViewItem)?.put_IsSelected(newIsSelected);
    }
  }
}
