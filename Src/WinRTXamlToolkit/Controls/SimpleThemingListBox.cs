// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.SimpleThemingListBox
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
  public class SimpleThemingListBox : ListBox
  {
    private readonly double ItemHeight = 40.0;
    private static readonly DependencyProperty _ScrollViewerBackgroundProperty = DependencyProperty.Register(nameof (ScrollViewerBackground), (Type) typeof (Brush), (Type) typeof (SimpleThemingListBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _ScrollViewerBorderBrushProperty = DependencyProperty.Register(nameof (ScrollViewerBorderBrush), (Type) typeof (Brush), (Type) typeof (SimpleThemingListBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _ScrollViewerBorderThicknessProperty = DependencyProperty.Register(nameof (ScrollViewerBorderThickness), (Type) typeof (Thickness), (Type) typeof (SimpleThemingListBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _PointerOverItemBackgroundProperty = DependencyProperty.Register(nameof (PointerOverItemBackground), (Type) typeof (Brush), (Type) typeof (SimpleThemingListBox), new PropertyMetadata((object) null));
    private static readonly DependencyProperty _PointerOverItemForegroundProperty = DependencyProperty.Register(nameof (PointerOverItemForeground), (Type) typeof (Brush), (Type) typeof (SimpleThemingListBox), new PropertyMetadata((object) null));

    public static DependencyProperty ScrollViewerBackgroundProperty => SimpleThemingListBox._ScrollViewerBackgroundProperty;

    public Brush ScrollViewerBackground
    {
      get => (Brush) ((DependencyObject) this).GetValue(SimpleThemingListBox.ScrollViewerBackgroundProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBox.ScrollViewerBackgroundProperty, (object) value);
    }

    public static DependencyProperty ScrollViewerBorderBrushProperty => SimpleThemingListBox._ScrollViewerBorderBrushProperty;

    public Brush ScrollViewerBorderBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(SimpleThemingListBox.ScrollViewerBorderBrushProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBox.ScrollViewerBorderBrushProperty, (object) value);
    }

    public static DependencyProperty ScrollViewerBorderThicknessProperty => SimpleThemingListBox._ScrollViewerBorderThicknessProperty;

    public Thickness ScrollViewerBorderThickness
    {
      get => (Thickness) ((DependencyObject) this).GetValue(SimpleThemingListBox.ScrollViewerBorderThicknessProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBox.ScrollViewerBorderThicknessProperty, (object) value);
    }

    public static DependencyProperty PointerOverItemBackgroundProperty => SimpleThemingListBox._PointerOverItemBackgroundProperty;

    public Brush PointerOverItemBackground
    {
      get => (Brush) ((DependencyObject) this).GetValue(SimpleThemingListBox.PointerOverItemBackgroundProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBox.PointerOverItemBackgroundProperty, (object) value);
    }

    public static DependencyProperty PointerOverItemForegroundProperty => SimpleThemingListBox._PointerOverItemForegroundProperty;

    public Brush PointerOverItemForeground
    {
      get => (Brush) ((DependencyObject) this).GetValue(SimpleThemingListBox.PointerOverItemForegroundProperty);
      set => ((DependencyObject) this).SetValue(SimpleThemingListBox.PointerOverItemForegroundProperty, (object) value);
    }

    protected virtual DependencyObject GetContainerForItemOverride()
    {
      SimpleThemingListBoxItem themingListBoxItem1 = new SimpleThemingListBoxItem();
      ((FrameworkElement) themingListBoxItem1).put_Height(this.ItemHeight);
      SimpleThemingListBoxItem containerForItemOverride = themingListBoxItem1;
      SimpleThemingListBoxItem themingListBoxItem2 = containerForItemOverride;
      DependencyProperty backgroundBrushProperty = SimpleThemingListBoxItem.PointerOverItemBackgroundBrushProperty;
      Binding binding1 = new Binding();
      binding1.put_Source((object) this);
      binding1.put_Path(new PropertyPath("PointerOverItemBackground"));
      Binding binding2 = binding1;
      ((FrameworkElement) themingListBoxItem2).SetBinding(backgroundBrushProperty, (BindingBase) binding2);
      SimpleThemingListBoxItem themingListBoxItem3 = containerForItemOverride;
      DependencyProperty foregroundBrushProperty = SimpleThemingListBoxItem.PointerOverItemForegroundBrushProperty;
      Binding binding3 = new Binding();
      binding3.put_Source((object) this);
      binding3.put_Path(new PropertyPath("PointerOverItemForeground"));
      Binding binding4 = binding3;
      ((FrameworkElement) themingListBoxItem3).SetBinding(foregroundBrushProperty, (BindingBase) binding4);
      return (DependencyObject) containerForItemOverride;
    }

    public double GetItemHeight() => this.ItemHeight;
  }
}
