// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Data.DataTemplateExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Data
{
  public static class DataTemplateExtensions
  {
    public static readonly DependencyProperty HierarchyProperty = DependencyProperty.RegisterAttached("Hierarchy", (Type) typeof (HierarchicalDataTemplate), (Type) typeof (DataTemplateExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(DataTemplateExtensions.OnHierarchyChanged)));

    public static HierarchicalDataTemplate GetHierarchy(DependencyObject d) => (HierarchicalDataTemplate) d.GetValue(DataTemplateExtensions.HierarchyProperty);

    public static void SetHierarchy(DependencyObject d, HierarchicalDataTemplate value) => d.SetValue(DataTemplateExtensions.HierarchyProperty, (object) value);

    private static void OnHierarchyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      HierarchicalDataTemplate oldValue = (HierarchicalDataTemplate) e.OldValue;
      HierarchicalDataTemplate hierarchicalDataTemplate = (HierarchicalDataTemplate) d.GetValue(DataTemplateExtensions.HierarchyProperty);
    }
  }
}
