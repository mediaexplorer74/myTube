// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Data.HierarchicalDataTemplate
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Controls.Data
{
  public class HierarchicalDataTemplate : FrameworkElement
  {
    private DataTemplate _itemTemplate;
    private Style _itemContainerStyle;

    public Binding ItemsSource { get; set; }

    internal bool IsItemTemplateSet { get; private set; }

    public DataTemplate ItemTemplate
    {
      get => this._itemTemplate;
      set
      {
        this.IsItemTemplateSet = true;
        this._itemTemplate = value;
      }
    }

    internal bool IsItemContainerStyleSet { get; private set; }

    public Style ItemContainerStyle
    {
      get => this._itemContainerStyle;
      set
      {
        this.IsItemContainerStyleSet = true;
        this._itemContainerStyle = value;
      }
    }
  }
}
