// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.HeaderedItemsControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using WinRTXamlToolkit.Controls.Data;

namespace WinRTXamlToolkit.Controls
{
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (ContentPresenter))]
  public class HeaderedItemsControl : ItemsControl
  {
    public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof (Header), (Type) typeof (object), (Type) typeof (HeaderedItemsControl), new PropertyMetadata((object) null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderPropertyChanged)));
    public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof (HeaderTemplate), (Type) typeof (DataTemplate), (Type) typeof (HeaderedItemsControl), new PropertyMetadata((object) null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderTemplatePropertyChanged)));
    public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof (ItemContainerStyle), (Type) typeof (Style), (Type) typeof (HeaderedItemsControl), new PropertyMetadata((object) null, new PropertyChangedCallback(HeaderedItemsControl.OnItemContainerStylePropertyChanged)));

    internal bool HeaderIsItem { get; set; }

    public object Header
    {
      get => ((DependencyObject) this).GetValue(HeaderedItemsControl.HeaderProperty);
      set => ((DependencyObject) this).SetValue(HeaderedItemsControl.HeaderProperty, value);
    }

    private static void OnHeaderPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as HeaderedItemsControl).OnHeaderChanged(e.OldValue, e.NewValue);
    }

    public DataTemplate HeaderTemplate
    {
      get => ((DependencyObject) this).GetValue(HeaderedItemsControl.HeaderTemplateProperty) as DataTemplate;
      set => ((DependencyObject) this).SetValue(HeaderedItemsControl.HeaderTemplateProperty, (object) value);
    }

    private static void OnHeaderTemplatePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as HeaderedItemsControl).OnHeaderTemplateChanged(e.OldValue as DataTemplate, e.NewValue as DataTemplate);
    }

    public Style ItemContainerStyle
    {
      get => ((DependencyObject) this).GetValue(HeaderedItemsControl.ItemContainerStyleProperty) as Style;
      set => ((DependencyObject) this).SetValue(HeaderedItemsControl.ItemContainerStyleProperty, (object) value);
    }

    private static void OnItemContainerStylePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as HeaderedItemsControl).ItemsControlHelper.UpdateItemContainerStyle(e.NewValue as Style);
    }

    internal ItemsControlHelper ItemsControlHelper { get; private set; }

    public HeaderedItemsControl()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (HeaderedItemsControl));
      this.ItemsControlHelper = new ItemsControlHelper((ItemsControl) this);
    }

    protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
    {
    }

    protected virtual void OnHeaderTemplateChanged(
      DataTemplate oldHeaderTemplate,
      DataTemplate newHeaderTemplate)
    {
    }

    protected virtual void OnApplyTemplate()
    {
      this.ItemsControlHelper.OnApplyTemplate();
      ((FrameworkElement) this).OnApplyTemplate();
    }

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      ItemsControlHelper.PrepareContainerForItemOverride(element, this.ItemContainerStyle);
      HeaderedItemsControl.PrepareHeaderedItemsControlContainerForItemOverride(element, item, (ItemsControl) this, this.ItemContainerStyle);
      base.PrepareContainerForItemOverride(element, item);
    }

    internal static void PrepareHeaderedItemsControlContainerForItemOverride(
      DependencyObject element,
      object item,
      ItemsControl parent,
      Style parentItemContainerStyle)
    {
      if (!(element is HeaderedItemsControl control))
        return;
      HeaderedItemsControl.PrepareHeaderedItemsControlContainer(control, item, parent, parentItemContainerStyle);
    }

    private static void PrepareHeaderedItemsControlContainer(
      HeaderedItemsControl control,
      object item,
      ItemsControl parentItemsControl,
      Style parentItemContainerStyle)
    {
      if (control == item)
        return;
      DataTemplate dataTemplate = parentItemsControl.ItemTemplate;
      DataTemplateSelector templateSelector = parentItemsControl.ItemTemplateSelector;
      if (templateSelector != null)
      {
        ((DependencyObject) control).SetValue(ItemsControl.ItemTemplateSelectorProperty, (object) templateSelector);
        dataTemplate = templateSelector.SelectTemplate(item, (DependencyObject) control);
      }
      else if (dataTemplate != null)
        ((DependencyObject) control).SetValue(ItemsControl.ItemTemplateProperty, (object) dataTemplate);
      if (parentItemContainerStyle != null && HeaderedItemsControl.HasDefaultValue((Control) control, HeaderedItemsControl.ItemContainerStyleProperty))
        ((DependencyObject) control).SetValue(HeaderedItemsControl.ItemContainerStyleProperty, (object) parentItemContainerStyle);
      if (control.HeaderIsItem || HeaderedItemsControl.HasDefaultValue((Control) control, HeaderedItemsControl.HeaderProperty))
      {
        control.Header = item;
        control.HeaderIsItem = true;
      }
      if (dataTemplate != null)
        ((DependencyObject) control).SetValue(HeaderedItemsControl.HeaderTemplateProperty, (object) dataTemplate);
      if (parentItemContainerStyle != null && ((FrameworkElement) control).Style == null)
        ((DependencyObject) control).SetValue(FrameworkElement.StyleProperty, (object) parentItemContainerStyle);
      DataTemplate d = dataTemplate;
      if (d == null)
        return;
      HierarchicalDataTemplate hierarchy = DataTemplateExtensions.GetHierarchy((DependencyObject) d);
      if (hierarchy != null && hierarchy.ItemsSource != null && HeaderedItemsControl.HasDefaultValue((Control) control, ItemsControl.ItemsSourceProperty))
      {
        HeaderedItemsControl headeredItemsControl = control;
        DependencyProperty itemsSourceProperty = ItemsControl.ItemsSourceProperty;
        Binding binding1 = new Binding();
        binding1.put_Converter(hierarchy.ItemsSource.Converter);
        binding1.put_ConverterLanguage(hierarchy.ItemsSource.ConverterLanguage);
        binding1.put_ConverterParameter(hierarchy.ItemsSource.ConverterParameter);
        binding1.put_Mode(hierarchy.ItemsSource.Mode);
        binding1.put_Path(hierarchy.ItemsSource.Path);
        binding1.put_Source(control.Header);
        Binding binding2 = binding1;
        ((FrameworkElement) headeredItemsControl).SetBinding(itemsSourceProperty, (BindingBase) binding2);
      }
      if (hierarchy != null && hierarchy.IsItemTemplateSet && control.ItemTemplate == dataTemplate)
      {
        ((DependencyObject) control).ClearValue(ItemsControl.ItemTemplateProperty);
        if (hierarchy.ItemTemplate != null)
          control.put_ItemTemplate(hierarchy.ItemTemplate);
      }
      if (hierarchy == null || !hierarchy.IsItemContainerStyleSet || control.ItemContainerStyle != parentItemContainerStyle)
        return;
      ((DependencyObject) control).ClearValue(HeaderedItemsControl.ItemContainerStyleProperty);
      if (hierarchy.ItemContainerStyle == null)
        return;
      control.ItemContainerStyle = hierarchy.ItemContainerStyle;
    }

    private static bool HasDefaultValue(Control control, DependencyProperty property) => ((DependencyObject) control).ReadLocalValue(property) == DependencyProperty.UnsetValue;
  }
}
