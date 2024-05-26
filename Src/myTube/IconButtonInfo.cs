﻿// myTube.IconButtonInfo

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace myTube
{
  public class IconButtonInfo : DependencyObject
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (IconButtonInfo), new PropertyMetadata((object) null));
    public static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof (Name), typeof (string), typeof (IconButtonInfo), new PropertyMetadata((object) null));
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof (Symbol), typeof (Symbol), typeof (IconButtonInfo), new PropertyMetadata((object) (Symbol) 57629));
    public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register(nameof (DataContext), typeof (object), typeof (IconButtonInfo), new PropertyMetadata((object) null));

    public string Text
    {
      get => (string) this.GetValue(IconButtonInfo.TextProperty);
      set => this.SetValue(IconButtonInfo.TextProperty, (object) value);
    }

    public string Name
    {
      get => (string) this.GetValue(IconButtonInfo.NameProperty);
      set => this.SetValue(IconButtonInfo.NameProperty, (object) value);
    }

    public Symbol Symbol
    {
      get => (Symbol) this.GetValue(IconButtonInfo.SymbolProperty);
      set => this.SetValue(IconButtonInfo.SymbolProperty, (object) value);
    }

    public object DataContext
    {
      get => this.GetValue(IconButtonInfo.DataContextProperty);
      set => this.SetValue(IconButtonInfo.DataContextProperty, value);
    }
  }
}
