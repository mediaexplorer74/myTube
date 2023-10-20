// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Common.ImplicitDataTemplateSelector
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace WinRTXamlToolkit.Common
{
  [ContentProperty(Name = "Resources")]
  public class ImplicitDataTemplateSelector : DataTemplateSelector
  {
    public ResourceDictionary Resources { get; set; }

    public ImplicitDataTemplateSelector() => this.Resources = new ResourceDictionary();

    protected virtual DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
      if (item != null)
      {
        string name = item.GetType().Name;
        object obj;
        if (((IDictionary<object, object>) this.Resources).TryGetValue((object) name, out obj))
          return obj is DataTemplate dataTemplate1 ? dataTemplate1 : throw new ArgumentException(string.Format("{0} resource defined in the ImplicitDataTemplateSelector needs to be of DataTemplate type.", (object) name));
        if (((IDictionary<object, object>) Application.Current.Resources).TryGetValue((object) name, out obj))
          return obj is DataTemplate dataTemplate2 ? dataTemplate2 : throw new ArgumentException(string.Format("{0} resource defined in the application resources needs to be of DataTemplate type, or one needs to be defined in the ImplicitDataTemplateSelector.", (object) name));
      }
      return base.SelectTemplateCore(item, container);
    }
  }
}
