// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.StyleExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Linq;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class StyleExtensions
  {
    public static object GetPropertyValue(this Style style, DependencyProperty property)
    {
      Setter setter = ((IEnumerable) style.Setters).Cast<Setter>().FirstOrDefault<Setter>((Func<Setter, bool>) (s => s.Property == property));
      if (setter != null)
        return setter.Value;
      return style.BasedOn != null ? style.BasedOn.GetPropertyValue(property) : DependencyProperty.UnsetValue;
    }
  }
}
