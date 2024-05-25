// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.NumericUpDown
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Common;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(GroupName = "DecrementalButtonStates", Name = "DecrementDisabled")]
  [TemplatePart(Name = "PART_ValueTextBox", Type = typeof (TextBox))]
  [TemplatePart(Name = "PART_ValueBar", Type = typeof (FrameworkElement))]
  [TemplatePart(Name = "PART_DragOverlay", Type = typeof (UIElement))]
  [TemplatePart(Name = "PART_DecrementButton", Type = typeof (RepeatButton))]
  [TemplatePart(Name = "PART_IncrementButton", Type = typeof (RepeatButton))]
  [TemplateVisualState(GroupName = "IncrementalButtonStates", Name = "IncrementEnabled")]
  [TemplateVisualState(GroupName = "IncrementalButtonStates", Name = "IncrementDisabled")]
  [TemplateVisualState(GroupName = "DecrementalButtonStates", Name = "DecrementEnabled")]
  public sealed class NumericUpDown : RangeBase
  {
    private const string DecrementButtonName = "PART_DecrementButton";
    private const string IncrementButtonName = "PART_IncrementButton";
    private const string DragOverlayName = "PART_DragOverlay";
    private const string ValueTextBoxName = "PART_ValueTextBox";
    private const string ValueBarName = "PART_ValueBar";
    private const double MinMouseDragDelta = 2.0;
    private UIElement _dragOverlay;
    private UpDownTextBox _valueTextBox;
    private RepeatButton _decrementButton;
    private RepeatButton _incrementButton;
    private FrameworkElement _valueBar;
    private bool _isDragUpdated;
    private bool _isChangingTextWithCode;
    private bool _isChangingValueWithCode;
    private double _unusedManipulationDelta;
    private bool _isDraggingWithMouse;
    private MouseDevice _mouseDevice;
    private double _totalDeltaX;
    private double _totalDeltaY;
    public static readonly DependencyProperty ValueFormatProperty = DependencyProperty.Register(nameof (ValueFormat), (Type) typeof (string), (Type) typeof (NumericUpDown), new PropertyMetadata((object) "F2", new PropertyChangedCallback(NumericUpDown.OnValueFormatChanged)));
    public static readonly DependencyProperty ValueBarVisibilityProperty = DependencyProperty.Register(nameof (ValueBarVisibility), (Type) typeof (NumericUpDownValueBarVisibility), (Type) typeof (NumericUpDown), new PropertyMetadata((object) NumericUpDownValueBarVisibility.Visible, new PropertyChangedCallback(NumericUpDown.OnValueBarVisibilityChanged)));
    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(nameof (IsReadOnly), (Type) typeof (bool), (Type) typeof (NumericUpDown), new PropertyMetadata((object) false, new PropertyChangedCallback(NumericUpDown.OnIsReadOnlyChanged)));
    public static readonly DependencyProperty DragSpeedProperty = DependencyProperty.Register(nameof (DragSpeed), (Type) typeof (double), (Type) typeof (NumericUpDown), new PropertyMetadata((object) double.NaN));
    private bool _hasFocus;

    public string ValueFormat
    {
      get => (string) ((DependencyObject) this).GetValue(NumericUpDown.ValueFormatProperty);
      set => ((DependencyObject) this).SetValue(NumericUpDown.ValueFormatProperty, (object) value);
    }

    private static void OnValueFormatChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) d;
      string oldValue = (string) e.OldValue;
      string valueFormat = numericUpDown.ValueFormat;
      numericUpDown.OnValueFormatChanged(oldValue, valueFormat);
    }

    private void OnValueFormatChanged(string oldValueFormat, string newValueFormat) => this.UpdateValueText();

    public NumericUpDownValueBarVisibility ValueBarVisibility
    {
      get => (NumericUpDownValueBarVisibility) ((DependencyObject) this).GetValue(NumericUpDown.ValueBarVisibilityProperty);
      set => ((DependencyObject) this).SetValue(NumericUpDown.ValueBarVisibilityProperty, (object) value);
    }

    private static void OnValueBarVisibilityChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) d;
      NumericUpDownValueBarVisibility oldValue = (NumericUpDownValueBarVisibility) e.OldValue;
      NumericUpDownValueBarVisibility valueBarVisibility = numericUpDown.ValueBarVisibility;
      numericUpDown.OnValueBarVisibilityChanged(oldValue, valueBarVisibility);
    }

    private void OnValueBarVisibilityChanged(
      NumericUpDownValueBarVisibility oldValueBarVisibility,
      NumericUpDownValueBarVisibility newValueBarVisibility)
    {
      this.UpdateValueBar();
    }

    public bool IsReadOnly
    {
      get => (bool) ((DependencyObject) this).GetValue(NumericUpDown.IsReadOnlyProperty);
      set => ((DependencyObject) this).SetValue(NumericUpDown.IsReadOnlyProperty, (object) value);
    }

    private static void OnIsReadOnlyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      NumericUpDown numericUpDown = (NumericUpDown) d;
      bool oldValue = (bool) e.OldValue;
      bool isReadOnly = numericUpDown.IsReadOnly;
      numericUpDown.OnIsReadOnlyChanged(oldValue, isReadOnly);
    }

    private void OnIsReadOnlyChanged(bool oldIsReadOnly, bool newIsReadOnly) => this.UpdateIsReadOnlyDependants();

    public double DragSpeed
    {
      get => (double) ((DependencyObject) this).GetValue(NumericUpDown.DragSpeedProperty);
      set => ((DependencyObject) this).SetValue(NumericUpDown.DragSpeedProperty, (object) value);
    }

    public NumericUpDown()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (NumericUpDown));
      NumericUpDown numericUpDown1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) numericUpDown1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) numericUpDown1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      NumericUpDown numericUpDown2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) numericUpDown2).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) numericUpDown2).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      if (this._dragOverlay == null)
        return;
      CoreWindow coreWindow1 = Window.Current.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, PointerEventArgs>>((Func<TypedEventHandler<CoreWindow, PointerEventArgs>, EventRegistrationToken>) new Func<TypedEventHandler<CoreWindow, PointerEventArgs>, EventRegistrationToken>(coreWindow1.add_PointerReleased), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(coreWindow1.remove_PointerReleased), new TypedEventHandler<CoreWindow, PointerEventArgs>((object) this, __methodptr(CoreWindowOnPointerReleased)));
      CoreWindow coreWindow2 = Window.Current.CoreWindow;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<CoreWindow, VisibilityChangedEventArgs>>((Func<TypedEventHandler<CoreWindow, VisibilityChangedEventArgs>, EventRegistrationToken>) new Func<TypedEventHandler<CoreWindow, VisibilityChangedEventArgs>, EventRegistrationToken>(coreWindow2.add_VisibilityChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(coreWindow2.remove_VisibilityChanged), new TypedEventHandler<CoreWindow, VisibilityChangedEventArgs>((object) this, __methodptr(OnCoreWindowVisibilityChanged)));
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<CoreWindow, PointerEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(Window.Current.CoreWindow.remove_PointerReleased), new TypedEventHandler<CoreWindow, PointerEventArgs>((object) this, __methodptr(CoreWindowOnPointerReleased)));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<CoreWindow, VisibilityChangedEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(Window.Current.CoreWindow.remove_VisibilityChanged), new TypedEventHandler<CoreWindow, VisibilityChangedEventArgs>((object) this, __methodptr(OnCoreWindowVisibilityChanged)));
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      if (DesignMode.DesignModeEnabled)
        return;
      NumericUpDown numericUpDown1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) numericUpDown1).add_GotFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) numericUpDown1).remove_GotFocus), new RoutedEventHandler(this.OnGotFocus));
      NumericUpDown numericUpDown2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) numericUpDown2).add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) numericUpDown2).remove_LostFocus), new RoutedEventHandler(this.OnLostFocus));
      NumericUpDown numericUpDown3 = this;
      WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) numericUpDown3).add_PointerWheelChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) numericUpDown3).remove_PointerWheelChanged), new PointerEventHandler(this.OnPointerWheelChanged));
      this._valueTextBox = ((Control) this).GetTemplateChild("PART_ValueTextBox") as UpDownTextBox;
      this._dragOverlay = ((Control) this).GetTemplateChild("PART_DragOverlay") as UIElement;
      this._decrementButton = ((Control) this).GetTemplateChild("PART_DecrementButton") as RepeatButton;
      this._incrementButton = ((Control) this).GetTemplateChild("PART_IncrementButton") as RepeatButton;
      this._valueBar = ((Control) this).GetTemplateChild("PART_ValueBar") as FrameworkElement;
      if (this._valueTextBox != null)
      {
        UpDownTextBox valueTextBox1 = this._valueTextBox;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) valueTextBox1).add_LostFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) valueTextBox1).remove_LostFocus), new RoutedEventHandler(this.OnValueTextBoxLostFocus));
        UpDownTextBox valueTextBox2 = this._valueTextBox;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) valueTextBox2).add_GotFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) valueTextBox2).remove_GotFocus), new RoutedEventHandler(this.OnValueTextBoxGotFocus));
        this._valueTextBox.put_Text(this.Value.ToString());
        UpDownTextBox valueTextBox3 = this._valueTextBox;
        WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>((Func<TextChangedEventHandler, EventRegistrationToken>) new Func<TextChangedEventHandler, EventRegistrationToken>(((TextBox) valueTextBox3).add_TextChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((TextBox) valueTextBox3).remove_TextChanged), new TextChangedEventHandler(this.OnValueTextBoxTextChanged));
        UpDownTextBox valueTextBox4 = this._valueTextBox;
        WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) valueTextBox4).add_KeyDown), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) valueTextBox4).remove_KeyDown), new KeyEventHandler(this.OnValueTextBoxKeyDown));
        this._valueTextBox.UpPressed += (EventHandler) ((s, e) => this.Increment());
        this._valueTextBox.DownPressed += (EventHandler) ((s, e) => this.Decrement());
        UpDownTextBox valueTextBox5 = this._valueTextBox;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) valueTextBox5).add_PointerExited), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) valueTextBox5).remove_PointerExited), new PointerEventHandler(this.OnValueTextBoxPointerExited));
      }
      if (this._dragOverlay != null)
      {
        UIElement dragOverlay1 = this._dragOverlay;
        WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>((Func<TappedEventHandler, EventRegistrationToken>) new Func<TappedEventHandler, EventRegistrationToken>(dragOverlay1.add_Tapped), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(dragOverlay1.remove_Tapped), new TappedEventHandler(this.OnDragOverlayTapped));
        this._dragOverlay.put_ManipulationMode((ManipulationModes) 3);
        UIElement dragOverlay2 = this._dragOverlay;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(dragOverlay2.add_PointerPressed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(dragOverlay2.remove_PointerPressed), new PointerEventHandler(this.OnDragOverlayPointerPressed));
        UIElement dragOverlay3 = this._dragOverlay;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(dragOverlay3.add_PointerReleased), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(dragOverlay3.remove_PointerReleased), new PointerEventHandler(this.OnDragOverlayPointerReleased));
        UIElement dragOverlay4 = this._dragOverlay;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(dragOverlay4.add_PointerCaptureLost), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(dragOverlay4.remove_PointerCaptureLost), new PointerEventHandler(this.OnDragOverlayPointerCaptureLost));
      }
      if (this._decrementButton != null)
      {
        RepeatButton decrementButton = this._decrementButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) decrementButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) decrementButton).remove_Click), new RoutedEventHandler(this.OnDecrementButtonClick));
        new PropertyChangeEventSource<bool>((DependencyObject) this._decrementButton, "IsPressed").ValueChanged += new EventHandler<bool>(this.OnDecrementButtonIsPressedChanged);
      }
      if (this._incrementButton != null)
      {
        RepeatButton incrementButton = this._incrementButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) incrementButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) incrementButton).remove_Click), new RoutedEventHandler(this.OnIncrementButtonClick));
        new PropertyChangeEventSource<bool>((DependencyObject) this._incrementButton, "IsPressed").ValueChanged += new EventHandler<bool>(this.OnIncrementButtonIsPressedChanged);
      }
      if (this._valueBar != null)
      {
        FrameworkElement valueBar = this._valueBar;
        WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(valueBar.add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(valueBar.remove_SizeChanged), new SizeChangedEventHandler(this.OnValueBarSizeChanged));
        this.UpdateValueBar();
      }
      this.UpdateIsReadOnlyDependants();
      this.SetValidIncrementDirection();
    }

    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
    {
      if (!this._hasFocus)
        return;
      if (pointerRoutedEventArgs.GetCurrentPoint((UIElement) this).Properties.MouseWheelDelta < 0)
        this.Decrement();
      else
        this.Increment();
      pointerRoutedEventArgs.put_Handled(true);
    }

    private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs) => this._hasFocus = false;

    private void OnGotFocus(object sender, RoutedEventArgs routedEventArgs) => this._hasFocus = true;

    private void OnValueTextBoxTextChanged(object sender, TextChangedEventArgs textChangedEventArgs) => this.UpdateValueFromText();

    private void OnValueTextBoxKeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (e.Key != 13 || !this.UpdateValueFromText())
        return;
      this.UpdateValueText();
      this._valueTextBox.SelectAll();
      e.put_Handled(true);
    }

    private bool UpdateValueFromText()
    {
      double result;
      if (this._isChangingTextWithCode || !double.TryParse(this._valueTextBox.Text, NumberStyles.Any, (IFormatProvider) CultureInfo.CurrentUICulture, out result) && !Calculator.TryCalculate(this._valueTextBox.Text, out result))
        return false;
      this._isChangingValueWithCode = true;
      this.SetValueAndUpdateValidDirections(result);
      this._isChangingValueWithCode = false;
      return true;
    }

    private void OnDecrementButtonIsPressedChanged(object decrementButton, bool isPressed)
    {
    }

    private void OnIncrementButtonIsPressedChanged(object incrementButton, bool isPressed)
    {
    }

    private void OnDecrementButtonClick(object sender, RoutedEventArgs routedEventArgs) => this.Decrement();

    private void OnIncrementButtonClick(object sender, RoutedEventArgs routedEventArgs) => this.Increment();

    private bool Decrement() => this.SetValueAndUpdateValidDirections(this.Value - this.SmallChange);

    private bool Increment() => this.SetValueAndUpdateValidDirections(this.Value + this.SmallChange);

    private void OnValueTextBoxGotFocus(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this._dragOverlay != null)
        this._dragOverlay.put_IsHitTestVisible(false);
      this._valueTextBox.SelectAll();
    }

    private void OnValueTextBoxLostFocus(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this._dragOverlay == null)
        return;
      this._dragOverlay.put_IsHitTestVisible(true);
      this.UpdateValueText();
    }

    private void OnDragOverlayPointerPressed(object sender, PointerRoutedEventArgs e)
    {
      this._dragOverlay.CapturePointer(e.Pointer);
      this._totalDeltaX = 0.0;
      this._totalDeltaY = 0.0;
      if (e.Pointer.PointerDeviceType == 2)
      {
        this._isDraggingWithMouse = true;
        this._mouseDevice = MouseDevice.GetForCurrentView();
        MouseDevice mouseDevice = this._mouseDevice;
        // ISSUE: method pointer
        WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<MouseDevice, MouseEventArgs>>((Func<TypedEventHandler<MouseDevice, MouseEventArgs>, EventRegistrationToken>) new Func<TypedEventHandler<MouseDevice, MouseEventArgs>, EventRegistrationToken>(mouseDevice.add_MouseMoved), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(mouseDevice.remove_MouseMoved), new TypedEventHandler<MouseDevice, MouseEventArgs>((object) this, __methodptr(OnMouseDragged)));
      }
      else
      {
        UIElement dragOverlay = this._dragOverlay;
        WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>((Func<ManipulationDeltaEventHandler, EventRegistrationToken>) new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(dragOverlay.add_ManipulationDelta), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(dragOverlay.remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnDragOverlayManipulationDelta));
      }
    }

    private async void CoreWindowOnPointerReleased(CoreWindow sender, PointerEventArgs args)
    {
      if (!this._isDragUpdated)
        return;
      args.put_Handled(true);
      await Task.Delay(100);
      if (this._valueTextBox == null)
        return;
      ((Control) this._valueTextBox).put_IsTabStop(true);
    }

    private async void OnDragOverlayPointerReleased(object sender, PointerRoutedEventArgs args) => await this.EndDragging(args);

    private async void OnDragOverlayPointerCaptureLost(object sender, PointerRoutedEventArgs args) => await this.EndDragging(args);

    private async Task EndDragging(PointerRoutedEventArgs args)
    {
      if (this._isDraggingWithMouse)
      {
        this._isDraggingWithMouse = false;
        // ISSUE: method pointer
        WindowsRuntimeMarshal.RemoveEventHandler<TypedEventHandler<MouseDevice, MouseEventArgs>>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._mouseDevice.remove_MouseMoved), new TypedEventHandler<MouseDevice, MouseEventArgs>((object) this, __methodptr(OnMouseDragged)));
        this._mouseDevice = (MouseDevice) null;
      }
      else if (this._dragOverlay != null)
        WindowsRuntimeMarshal.RemoveEventHandler<ManipulationDeltaEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._dragOverlay.remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnDragOverlayManipulationDelta));
      if (!this._isDragUpdated)
        return;
      args?.put_Handled(true);
      await Task.Delay(100);
      if (this._valueTextBox == null)
        return;
      ((Control) this._valueTextBox).put_IsTabStop(true);
    }

    private void OnCoreWindowVisibilityChanged(CoreWindow sender, VisibilityChangedEventArgs args)
    {
      if (args.Visible)
        return;
      this.EndDragging((PointerRoutedEventArgs) null);
    }

    private void OnMouseDragged(MouseDevice sender, MouseEventArgs args)
    {
      int x = args.MouseDelta.X;
      int y = args.MouseDelta.Y;
      if (x > 200 || x < -200 || y > 200 || y < -200)
        return;
      this._totalDeltaX += (double) x;
      this._totalDeltaY += (double) y;
      if (this._totalDeltaX <= 2.0 && this._totalDeltaX >= -2.0 && this._totalDeltaY <= 2.0 && this._totalDeltaY >= -2.0)
        return;
      this.UpdateByDragging(this._totalDeltaX, this._totalDeltaY);
      this._totalDeltaX = 0.0;
      this._totalDeltaY = 0.0;
    }

    private void OnDragOverlayManipulationDelta(
      object sender,
      ManipulationDeltaRoutedEventArgs manipulationDeltaRoutedEventArgs)
    {
      if (this.UpdateByDragging(((Point) manipulationDeltaRoutedEventArgs.Delta.Translation).X, ((Point) manipulationDeltaRoutedEventArgs.Delta.Translation).Y))
        return;
      manipulationDeltaRoutedEventArgs.put_Handled(true);
    }

    private bool UpdateByDragging(double dx, double dy)
    {
      if (!((Control) this).IsEnabled || this.IsReadOnly || dx == 0.0 && dy == 0.0)
        return false;
      this.ApplyManipulationDelta(Math.Abs(dx) <= Math.Abs(dy) ? -dy : dx);
      if (this._valueTextBox != null)
        ((Control) this._valueTextBox).put_IsTabStop(false);
      this._isDragUpdated = true;
      return true;
    }

    private void ApplyManipulationDelta(double delta)
    {
      if (Math.Sign(delta) == Math.Sign(this._unusedManipulationDelta))
        this._unusedManipulationDelta += delta;
      else
        this._unusedManipulationDelta = delta;
      if (this._unusedManipulationDelta <= 0.0 && this.Value == this.Minimum)
        this._unusedManipulationDelta = 0.0;
      else if (this._unusedManipulationDelta >= 0.0 && this.Value == this.Maximum)
      {
        this._unusedManipulationDelta = 0.0;
      }
      else
      {
        double num = Window.Current == null ? 768.0 : Math.Min(((Rect) Window.Current.Bounds).Width, ((Rect) Window.Current.Bounds).Height);
        double d = this.DragSpeed;
        if (double.IsNaN(d) || double.IsInfinity(d))
          d = this.Maximum - this.Minimum;
        if (double.IsNaN(d) || double.IsInfinity(d))
          d = double.MaxValue;
        this.SetValueAndUpdateValidDirections(this.Value + d * this._unusedManipulationDelta / num);
        this._unusedManipulationDelta = 0.0;
      }
    }

    private void OnDragOverlayTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
    {
      if (!((Control) this).IsEnabled || this._valueTextBox == null || !((Control) this._valueTextBox).IsTabStop)
        return;
      ((Control) this._valueTextBox).Focus((FocusState) 3);
    }

    private void OnValueTextBoxPointerExited(object sender, PointerRoutedEventArgs e)
    {
    }

    protected virtual void OnValueChanged(double oldValue, double newValue)
    {
      base.OnValueChanged(oldValue, newValue);
      this.UpdateValueBar();
      if (this._isChangingValueWithCode)
        return;
      this.UpdateValueText();
    }

    private void UpdateValueBar()
    {
      if (this._valueBar == null)
        return;
      if (this.ValueBarVisibility == NumericUpDownValueBarVisibility.Collapsed)
      {
        ((UIElement) this._valueBar).put_Visibility((Visibility) 1);
      }
      else
      {
        FrameworkElement valueBar = this._valueBar;
        RectangleGeometry rectangleGeometry1 = new RectangleGeometry();
        rectangleGeometry1.put_Rect((Rect) new Rect()
        {
          X = 0.0,
          Y = 0.0,
          Height = this._valueBar.ActualHeight,
          Width = (this._valueBar.ActualWidth * (this.Value - this.Minimum) / (this.Maximum - this.Minimum))
        });
        RectangleGeometry rectangleGeometry2 = rectangleGeometry1;
        ((UIElement) valueBar).put_Clip(rectangleGeometry2);
      }
    }

    private void OnValueBarSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => this.UpdateValueBar();

    private void UpdateValueText()
    {
      if (this._valueTextBox == null)
        return;
      this._isChangingTextWithCode = true;
      this._valueTextBox.put_Text(this.Value.ToString(this.ValueFormat));
      this._isChangingTextWithCode = false;
    }

    private void UpdateIsReadOnlyDependants()
    {
      if (this._decrementButton != null)
        ((UIElement) this._decrementButton).put_Visibility(this.IsReadOnly ? (Visibility) 1 : (Visibility) 0);
      if (this._incrementButton == null)
        return;
      ((UIElement) this._incrementButton).put_Visibility(this.IsReadOnly ? (Visibility) 1 : (Visibility) 0);
    }

    private bool SetValueAndUpdateValidDirections(double value)
    {
      double num = this.Value;
      this.put_Value(value);
      this.SetValidIncrementDirection();
      return this.Value != num;
    }

    private void SetValidIncrementDirection()
    {
      if (this.Value < this.Maximum)
        VisualStateManager.GoToState((Control) this, "IncrementEnabled", true);
      if (this.Value > this.Minimum)
        VisualStateManager.GoToState((Control) this, "DecrementEnabled", true);
      if (this.Value == this.Maximum)
        VisualStateManager.GoToState((Control) this, "IncrementDisabled", true);
      if (this.Value != this.Minimum)
        return;
      VisualStateManager.GoToState((Control) this, "DecrementDisabled", true);
    }
  }
}
