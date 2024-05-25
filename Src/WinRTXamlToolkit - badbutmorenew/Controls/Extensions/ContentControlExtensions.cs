// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ContentControlExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ContentControlExtensions
  {
    public static readonly DependencyProperty FadeTransitioningContentTemplateProperty = DependencyProperty.RegisterAttached("FadeTransitioningContentTemplate", (Type) typeof (DataTemplate), (Type) typeof (ContentControlExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ContentControlExtensions.OnFadeTransitioningContentTemplateChanged)));
    public static readonly DependencyProperty FadeInTransitioningContentTemplateProperty = DependencyProperty.RegisterAttached("FadeInTransitioningContentTemplate", (Type) typeof (DataTemplate), (Type) typeof (ContentControlExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ContentControlExtensions.OnFadeInTransitioningContentTemplateChanged)));

    public static DataTemplate GetFadeTransitioningContentTemplate(DependencyObject d) => (DataTemplate) d.GetValue(ContentControlExtensions.FadeTransitioningContentTemplateProperty);

    public static void SetFadeTransitioningContentTemplate(DependencyObject d, DataTemplate value) => d.SetValue(ContentControlExtensions.FadeTransitioningContentTemplateProperty, (object) value);

    private static async void OnFadeTransitioningContentTemplateChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      DataTemplate oldValue = (DataTemplate) e.OldValue;
      DataTemplate newFadeTransitioningContentTemplate = (DataTemplate) d.GetValue(ContentControlExtensions.FadeTransitioningContentTemplateProperty);
      ContentControl control = (ContentControl) d;
      await ((UIElement) control).FadeOut();
      control.put_ContentTemplate(newFadeTransitioningContentTemplate);
      await ((UIElement) control).FadeIn();
    }

    public static DataTemplate GetFadeInTransitioningContentTemplate(DependencyObject d) => (DataTemplate) d.GetValue(ContentControlExtensions.FadeInTransitioningContentTemplateProperty);

    public static void SetFadeInTransitioningContentTemplate(DependencyObject d, DataTemplate value) => d.SetValue(ContentControlExtensions.FadeInTransitioningContentTemplateProperty, (object) value);

    private static async void OnFadeInTransitioningContentTemplateChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      DataTemplate oldValue = (DataTemplate) e.OldValue;
      DataTemplate newFadeInTransitioningContentTemplate = (DataTemplate) d.GetValue(ContentControlExtensions.FadeTransitioningContentTemplateProperty);
      ContentControl control = (ContentControl) d;
      await ((UIElement) control).FadeOut(new TimeSpan?(TimeSpan.FromSeconds(0.0)));
      control.put_ContentTemplate(newFadeInTransitioningContentTemplate);
      await ((UIElement) control).FadeIn();
    }

    public static void PrepareContentControl(
      this ContentControl @this,
      object item,
      DataTemplate itemTemplate,
      DataTemplateSelector itemTemplateSelector)
    {
      if (item == @this)
        return;
      if (!((DependencyObject) @this).HasNonDefaultValue(ContentControl.ContentProperty))
        @this.put_Content(item);
      if (itemTemplate != null)
        ((DependencyObject) @this).SetValue(ContentControl.ContentTemplateProperty, (object) itemTemplate);
      if (itemTemplateSelector == null)
        return;
      ((DependencyObject) @this).SetValue(ContentControl.ContentTemplateSelectorProperty, (object) itemTemplateSelector);
    }

    public static void ClearContentControl(this ContentControl @this, object item)
    {
      if (item == @this)
        return;
      ((DependencyObject) @this).ClearValue(ContentControl.ContentProperty);
    }
  }
}
