// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.WatermarkPasswordBox
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(GroupName = "WatermarkStates", Name = "WatermarkVisible")]
  [StyleTypedProperty(Property = "WatermarkTextStyle", StyleTargetType = typeof (TextBlock))]
  [TemplatePart(Name = "PART_PasswordBox", Type = typeof (PasswordBox))]
  [TemplateVisualState(GroupName = "WatermarkStates", Name = "WatermarkHidden")]
  public sealed class WatermarkPasswordBox : Control
  {
    private const string WatermarkStatesGroupName = "WatermarkStates";
    private const string WatermarkVisibleStateName = "WatermarkVisible";
    private const string WatermarkHiddenStateName = "WatermarkHidden";
    private const string InnerPasswordBoxName = "PART_PasswordBox";
    private PasswordBox _innerPasswordBox;
    public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached("Watermark", (Type) typeof (object), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) null));
    public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.RegisterAttached("WatermarkText", (Type) typeof (string), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) null));
    public static readonly DependencyProperty WatermarkTextStyleProperty = DependencyProperty.Register(nameof (WatermarkTextStyle), (Type) typeof (Style), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) null));
    public static readonly DependencyProperty WatermarkTextStyleRelayProperty = DependencyProperty.RegisterAttached("WatermarkTextStyleRelay", (Type) typeof (Style), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) null));
    public static readonly DependencyProperty IsPasswordRevealButtonEnabledProperty = DependencyProperty.Register(nameof (IsPasswordRevealButtonEnabled), (Type) typeof (bool), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) false));
    public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(nameof (MaxLength), (Type) typeof (int), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof (Password), (Type) typeof (string), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) string.Empty));
    public static readonly DependencyProperty PasswordCharProperty = DependencyProperty.Register(nameof (PasswordChar), (Type) typeof (string), (Type) typeof (WatermarkPasswordBox), new PropertyMetadata((object) "●"));

    public static object GetWatermark(DependencyObject d) => d.GetValue(WatermarkPasswordBox.WatermarkProperty);

    public static void SetWatermark(DependencyObject d, object value) => d.SetValue(WatermarkPasswordBox.WatermarkProperty, value);

    public static string GetWatermarkText(DependencyObject d) => (string) d.GetValue(WatermarkPasswordBox.WatermarkTextProperty);

    public static void SetWatermarkText(DependencyObject d, string value) => d.SetValue(WatermarkPasswordBox.WatermarkTextProperty, (object) value);

    public Style WatermarkTextStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(WatermarkPasswordBox.WatermarkTextStyleProperty);
      set => ((DependencyObject) this).SetValue(WatermarkPasswordBox.WatermarkTextStyleProperty, (object) value);
    }

    public static Style GetWatermarkTextStyleRelay(DependencyObject d) => (Style) d.GetValue(WatermarkPasswordBox.WatermarkTextStyleRelayProperty);

    public static void SetWatermarkTextStyleRelay(DependencyObject d, Style value) => d.SetValue(WatermarkPasswordBox.WatermarkTextStyleRelayProperty, (object) value);

    public bool IsPasswordRevealButtonEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(WatermarkPasswordBox.IsPasswordRevealButtonEnabledProperty);
      set => ((DependencyObject) this).SetValue(WatermarkPasswordBox.IsPasswordRevealButtonEnabledProperty, (object) value);
    }

    public int MaxLength
    {
      get => (int) ((DependencyObject) this).GetValue(WatermarkPasswordBox.MaxLengthProperty);
      set => ((DependencyObject) this).SetValue(WatermarkPasswordBox.MaxLengthProperty, (object) value);
    }

    public string Password
    {
      get => (string) ((DependencyObject) this).GetValue(WatermarkPasswordBox.PasswordProperty);
      set => ((DependencyObject) this).SetValue(WatermarkPasswordBox.PasswordProperty, (object) value);
    }

    public string PasswordChar
    {
      get => (string) ((DependencyObject) this).GetValue(WatermarkPasswordBox.PasswordCharProperty);
      set => ((DependencyObject) this).SetValue(WatermarkPasswordBox.PasswordCharProperty, (object) value);
    }

    public event ContextMenuOpeningEventHandler ContextMenuOpening;

    public event RoutedEventHandler PasswordChanged;

    public WatermarkPasswordBox()
    {
      this.put_DefaultStyleKey((object) typeof (WatermarkPasswordBox));
      WatermarkPasswordBox watermarkPasswordBox = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) watermarkPasswordBox).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) watermarkPasswordBox).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) => this.UpdateWatermarkVisualState(false, false);

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._innerPasswordBox = (PasswordBox) this.GetTemplateChild("PART_PasswordBox");
      PasswordBox innerPasswordBox1 = this._innerPasswordBox;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(innerPasswordBox1.add_PasswordChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(innerPasswordBox1.remove_PasswordChanged), new RoutedEventHandler(this.InnerPasswordBoxOnPasswordChanged));
      PasswordBox innerPasswordBox2 = this._innerPasswordBox;
      WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) innerPasswordBox2).add_KeyUp), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) innerPasswordBox2).remove_KeyUp), new KeyEventHandler(this.InnerPasswordBoxOnKeyUp));
      PasswordBox innerPasswordBox3 = this._innerPasswordBox;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) innerPasswordBox3).add_GotFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) innerPasswordBox3).remove_GotFocus), new RoutedEventHandler(this.InnerPasswordBoxOnGotFocus));
      PasswordBox innerPasswordBox4 = this._innerPasswordBox;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) innerPasswordBox4).add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) innerPasswordBox4).remove_LostFocus), new RoutedEventHandler(this.InnerPasswordBoxOnLostFocus));
      PasswordBox innerPasswordBox5 = this._innerPasswordBox;
      WindowsRuntimeMarshal.AddEventHandler<ContextMenuOpeningEventHandler>((Func<ContextMenuOpeningEventHandler, EventRegistrationToken>) new Func<ContextMenuOpeningEventHandler, EventRegistrationToken>(innerPasswordBox5.add_ContextMenuOpening), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(innerPasswordBox5.remove_ContextMenuOpening), new ContextMenuOpeningEventHandler(this.InnerPasswordBoxOnContextMenuOpening));
      PasswordBox innerPasswordBox6 = this._innerPasswordBox;
      DependencyProperty passwordProperty = PasswordBox.PasswordProperty;
      Binding binding1 = new Binding();
      binding1.put_Path(new PropertyPath("Password"));
      binding1.put_Mode((BindingMode) 3);
      binding1.put_Source((object) this);
      Binding binding2 = binding1;
      ((FrameworkElement) innerPasswordBox6).SetBinding(passwordProperty, (BindingBase) binding2);
      PasswordBox innerPasswordBox7 = this._innerPasswordBox;
      DependencyProperty passwordCharProperty = PasswordBox.PasswordCharProperty;
      Binding binding3 = new Binding();
      binding3.put_Path(new PropertyPath("PasswordChar"));
      binding3.put_Mode((BindingMode) 3);
      binding3.put_Source((object) this);
      Binding binding4 = binding3;
      ((FrameworkElement) innerPasswordBox7).SetBinding(passwordCharProperty, (BindingBase) binding4);
      PasswordBox innerPasswordBox8 = this._innerPasswordBox;
      DependencyProperty buttonEnabledProperty = PasswordBox.IsPasswordRevealButtonEnabledProperty;
      Binding binding5 = new Binding();
      binding5.put_Path(new PropertyPath("IsPasswordRevealButtonEnabled"));
      binding5.put_Mode((BindingMode) 3);
      binding5.put_Source((object) this);
      Binding binding6 = binding5;
      ((FrameworkElement) innerPasswordBox8).SetBinding(buttonEnabledProperty, (BindingBase) binding6);
      PasswordBox innerPasswordBox9 = this._innerPasswordBox;
      DependencyProperty maxLengthProperty = PasswordBox.MaxLengthProperty;
      Binding binding7 = new Binding();
      binding7.put_Path(new PropertyPath("MaxLength"));
      binding7.put_Mode((BindingMode) 3);
      binding7.put_Source((object) this);
      Binding binding8 = binding7;
      ((FrameworkElement) innerPasswordBox9).SetBinding(maxLengthProperty, (BindingBase) binding8);
      this.UpdateWatermarkVisualState(false, false);
    }

    private void InnerPasswordBoxOnContextMenuOpening(
      object sender,
      ContextMenuEventArgs contextMenuEventArgs)
    {
      ContextMenuOpeningEventHandler contextMenuOpening = this.ContextMenuOpening;
      if (contextMenuOpening == null)
        return;
      contextMenuOpening((object) this, contextMenuEventArgs);
    }

    private void UpdateWatermarkVisualState()
    {
      DependencyObject focusedElement = FocusManager.GetFocusedElement() as DependencyObject;
      this.UpdateWatermarkVisualState(this == focusedElement || focusedElement != null && ((IEnumerable<DependencyObject>) focusedElement.GetAncestors()).Contains<DependencyObject>((DependencyObject) this));
    }

    private void UpdateWatermarkVisualState(bool isFocused, bool useTransitions = true)
    {
      if (this._innerPasswordBox == null)
        return;
      if (!isFocused && string.IsNullOrEmpty(this._innerPasswordBox.Password))
        VisualStateManager.GoToState((Control) this._innerPasswordBox, "WatermarkVisible", useTransitions);
      else
        VisualStateManager.GoToState((Control) this._innerPasswordBox, "WatermarkHidden", useTransitions);
    }

    private void InnerPasswordBoxOnGotFocus(object sender, RoutedEventArgs e) => this.UpdateWatermarkVisualState(true);

    private void InnerPasswordBoxOnLostFocus(object sender, RoutedEventArgs e) => this.UpdateWatermarkVisualState(false);

    private void InnerPasswordBoxOnPasswordChanged(object sender, RoutedEventArgs e)
    {
      this.UpdateWatermarkVisualState();
      RoutedEventHandler passwordChanged = this.PasswordChanged;
      if (passwordChanged == null)
        return;
      passwordChanged((object) this, e);
    }

    private void InnerPasswordBoxOnKeyUp(object sender, KeyRoutedEventArgs e) => this.UpdateWatermarkVisualState();

    public void SelectAll() => this._innerPasswordBox.SelectAll();
  }
}
