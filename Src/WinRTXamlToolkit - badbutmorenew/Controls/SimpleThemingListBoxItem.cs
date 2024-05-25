// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.SimpleThemingListBoxItem
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
  public class SimpleThemingListBoxItem : ListBoxItem
  {
    private static readonly DependencyProperty _PointerOverItemBackgroundBrushProperty = DependencyProperty.Register(nameof (PointerOverItemBackgroundBrush), (Type) typeof (Brush), (Type) typeof (SimpleThemingListBoxItem), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _PointerOverItemForegroundBrushProperty = DependencyProperty.Register(nameof (PointerOverItemForegroundBrush), (Type) typeof (Brush), (Type) typeof (SimpleThemingListBoxItem), new PropertyMetadata((object) null));

    public static DependencyProperty PointerOverItemBackgroundBrushProperty => SimpleThemingListBoxItem._PointerOverItemBackgroundBrushProperty;

    public Brush PointerOverItemBackgroundBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(SimpleThemingListBoxItem.PointerOverItemBackgroundBrushProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBoxItem.PointerOverItemBackgroundBrushProperty, (object) value);
    }

    public static DependencyProperty PointerOverItemForegroundBrushProperty => SimpleThemingListBoxItem._PointerOverItemForegroundBrushProperty;

    public Brush PointerOverItemForegroundBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(SimpleThemingListBoxItem.PointerOverItemForegroundBrushProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBoxItem.PointerOverItemForegroundBrushProperty, (object) value);
    }

    protected Brush OriginalForegroundBrush { get; set; }

    public SimpleThemingListBoxItem()
    {
      SimpleThemingListBoxItem themingListBoxItem1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) themingListBoxItem1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) themingListBoxItem1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      SimpleThemingListBoxItem themingListBoxItem2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) themingListBoxItem2).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) themingListBoxItem2).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.OriginalForegroundBrush = ((Control) this).Foreground;
      SimpleThemingListBoxItem themingListBoxItem1 = this;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) themingListBoxItem1).add_PointerEntered), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) themingListBoxItem1).remove_PointerEntered), new PointerEventHandler(this.OnPointerEntered));
      SimpleThemingListBoxItem themingListBoxItem2 = this;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) themingListBoxItem2).add_PointerExited), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) themingListBoxItem2).remove_PointerExited), new PointerEventHandler(this.OnPointerExited));
      SimpleThemingListBoxItem themingListBoxItem3 = this;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) themingListBoxItem3).add_PointerCaptureLost), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) themingListBoxItem3).remove_PointerCaptureLost), new PointerEventHandler(this.OnPointerCaptureLost));
      SimpleThemingListBoxItem themingListBoxItem4 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) themingListBoxItem4).add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) themingListBoxItem4).remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this).remove_PointerEntered), new PointerEventHandler(this.OnPointerEntered));
      WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this).remove_PointerExited), new PointerEventHandler(this.OnPointerExited));
      WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this).remove_PointerCaptureLost), new PointerEventHandler(this.OnPointerCaptureLost));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this).remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    private void OnLostFocus(object sender, RoutedEventArgs e) => this.OnPointerLeftItem();

    private void OnPointerExited(object sender, PointerRoutedEventArgs e) => this.OnPointerLeftItem();

    private void OnPointerEntered(object sender, PointerRoutedEventArgs e) => this.OnPointerOverItem();

    private void OnPointerCaptureLost(object sender, PointerRoutedEventArgs e) => this.OnPointerLeftItem();

    private void OnPointerLeftItem()
    {
      ((Control) this).put_Background((Brush) new SolidColorBrush(Colors.Transparent));
      ((Control) this).put_Foreground(this.OriginalForegroundBrush);
    }

    private void OnPointerOverItem()
    {
      ((Control) this).put_Background(this.PointerOverItemBackgroundBrush);
      ((Control) this).put_Foreground(this.PointerOverItemForegroundBrush);
    }
  }
}
