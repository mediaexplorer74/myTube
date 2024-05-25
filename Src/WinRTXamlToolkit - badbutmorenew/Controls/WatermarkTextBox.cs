// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.WatermarkTextBox
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(GroupName = "WatermarkStates", Name = "WatermarkHidden")]
  [TemplateVisualState(GroupName = "WatermarkStates", Name = "WatermarkVisible")]
  [StyleTypedProperty(Property = "WatermarkTextStyle", StyleTargetType = typeof (TextBlock))]
  public class WatermarkTextBox : TextBox
  {
    private const string WatermarkStatesGroupName = "WatermarkStates";
    private const string WatermarkVisibleStateName = "WatermarkVisible";
    private const string WatermarkHiddenStateName = "WatermarkHidden";
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(nameof (Watermark), (Type) typeof (object), (Type) typeof (WatermarkTextBox), new PropertyMetadata((object) null));
    public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register(nameof (WatermarkText), (Type) typeof (string), (Type) typeof (WatermarkTextBox), new PropertyMetadata((object) null));
    public static readonly DependencyProperty WatermarkTextStyleProperty = DependencyProperty.Register(nameof (WatermarkTextStyle), (Type) typeof (Style), (Type) typeof (WatermarkTextBox), new PropertyMetadata((object) null));

    public object Watermark
    {
      get => ((DependencyObject) this).GetValue(WatermarkTextBox.WatermarkProperty);
      set => ((DependencyObject) this).SetValue(WatermarkTextBox.WatermarkProperty, value);
    }

    public string WatermarkText
    {
      get => (string) ((DependencyObject) this).GetValue(WatermarkTextBox.WatermarkTextProperty);
      set => ((DependencyObject) this).SetValue(WatermarkTextBox.WatermarkTextProperty, (object) value);
    }

    public Style WatermarkTextStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(WatermarkTextBox.WatermarkTextStyleProperty);
      set => ((DependencyObject) this).SetValue(WatermarkTextBox.WatermarkTextStyleProperty, (object) value);
    }

    public WatermarkTextBox()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (WatermarkTextBox));
      WatermarkTextBox watermarkTextBox = this;
      WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>((Func<TextChangedEventHandler, EventRegistrationToken>) new Func<TextChangedEventHandler, EventRegistrationToken>(((TextBox) watermarkTextBox).add_TextChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((TextBox) watermarkTextBox).remove_TextChanged), new TextChangedEventHandler(this.OnTextChanged));
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.UpdateWatermarkVisualState();
    }

    private void UpdateWatermarkVisualState()
    {
      DependencyObject focusedElement = FocusManager.GetFocusedElement() as DependencyObject;
      this.UpdateWatermarkVisualState(this == focusedElement || focusedElement != null && ((IEnumerable<DependencyObject>) focusedElement.GetAncestors()).Contains<DependencyObject>((DependencyObject) this));
    }

    private void UpdateWatermarkVisualState(bool isFocused)
    {
      if (!isFocused && string.IsNullOrEmpty(this.Text))
        VisualStateManager.GoToState((Control) this, "WatermarkVisible", true);
      else
        VisualStateManager.GoToState((Control) this, "WatermarkHidden", true);
    }

    protected virtual void OnGotFocus(RoutedEventArgs e)
    {
      ((Control) this).OnGotFocus(e);
      this.UpdateWatermarkVisualState(true);
    }

    protected virtual void OnLostFocus(RoutedEventArgs e)
    {
      ((Control) this).OnLostFocus(e);
      this.UpdateWatermarkVisualState(false);
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e) => this.UpdateWatermarkVisualState();
  }
}
