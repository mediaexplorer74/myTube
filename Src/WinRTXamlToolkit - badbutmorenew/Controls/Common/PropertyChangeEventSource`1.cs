// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Common.PropertyChangeEventSource`1
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Controls.Common
{
  public class PropertyChangeEventSource<TPropertyType> : FrameworkElement
  {
    private readonly DependencyObject _source;
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), (Type) typeof (TPropertyType), (Type) typeof (PropertyChangeEventSource<TPropertyType>), new PropertyMetadata((object) default (TPropertyType), new PropertyChangedCallback(PropertyChangeEventSource<TPropertyType>.OnValueChanged)));

    public event EventHandler<TPropertyType> ValueChanged;

    public TPropertyType Value
    {
      get => (TPropertyType) ((DependencyObject) this).GetValue(PropertyChangeEventSource<TPropertyType>.ValueProperty);
      set => ((DependencyObject) this).SetValue(PropertyChangeEventSource<TPropertyType>.ValueProperty, (object) value);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      PropertyChangeEventSource<TPropertyType> changeEventSource = (PropertyChangeEventSource<TPropertyType>) d;
      TPropertyType oldValue = (TPropertyType) e.OldValue;
      TPropertyType newValue = changeEventSource.Value;
      changeEventSource.OnValueChanged(oldValue, newValue);
    }

    private void OnValueChanged(TPropertyType oldValue, TPropertyType newValue)
    {
      EventHandler<TPropertyType> valueChanged = this.ValueChanged;
      if (valueChanged == null)
        return;
      valueChanged((object) this._source, newValue);
    }

    public PropertyChangeEventSource(
      DependencyObject source,
      string propertyName,
      BindingMode bindingMode = 3)
    {
      this._source = source;
      Binding binding1 = new Binding();
      binding1.put_Source((object) source);
      binding1.put_Path(new PropertyPath(propertyName));
      binding1.put_Mode(bindingMode);
      Binding binding2 = binding1;
      this.SetBinding(PropertyChangeEventSource<TPropertyType>.ValueProperty, (BindingBase) binding2);
    }
  }
}
