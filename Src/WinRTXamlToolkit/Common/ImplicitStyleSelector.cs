// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Common.ImplicitStyleSelector
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
  public class ImplicitStyleSelector : StyleSelector
  {
    public ResourceDictionary Resources { get; set; }

    public ImplicitStyleSelector() => this.Resources = new ResourceDictionary();

    protected virtual Style SelectStyleCore(object item, DependencyObject container)
    {
      if (item != null)
      {
        string name = item.GetType().Name;
        object obj;
        if (((IDictionary<object, object>) this.Resources).TryGetValue((object) name, out obj))
          return obj is Style style1 ? style1 : throw new ArgumentException(string.Format("{0} resource defined in the ImplicitStyleSelector needs to be of Style type.", (object) name));
        if (((IDictionary<object, object>) Application.Current.Resources).TryGetValue((object) name, out obj))
          return obj is Style style2 ? style2 : throw new ArgumentException(string.Format("{0} resource defined in the application resources needs to be of Style type, or one needs to be defined in the ImplicitStyleSelector.", (object) name));
      }
      return base.SelectStyleCore(item, container);
    }
  }
}
