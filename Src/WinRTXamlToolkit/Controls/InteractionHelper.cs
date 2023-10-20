// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.InteractionHelper
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
  internal sealed class InteractionHelper
  {
    private const double SequentialClickThresholdInMilliseconds = 500.0;
    private const double SequentialClickThresholdInPixelsSquared = 9.0;
    private IUpdateVisualState _updateVisualState;

    public Control Control { get; private set; }

    public bool IsFocused { get; private set; }

    public bool IsMouseOver { get; private set; }

    public bool IsReadOnly { get; private set; }

    public bool IsPressed { get; private set; }

    private DateTime LastClickTime { get; set; }

    private Point LastClickPosition { get; set; }

    public int ClickCount { get; private set; }

    public InteractionHelper(Control control)
    {
      this.Control = control;
      this._updateVisualState = control as IUpdateVisualState;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) control).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) control).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      WindowsRuntimeMarshal.AddEventHandler<DependencyPropertyChangedEventHandler>((Func<DependencyPropertyChangedEventHandler, EventRegistrationToken>) new Func<DependencyPropertyChangedEventHandler, EventRegistrationToken>(control.add_IsEnabledChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(control.remove_IsEnabledChanged), new DependencyPropertyChangedEventHandler(this.OnIsEnabledChanged));
    }

    private void UpdateVisualState(bool useTransitions)
    {
      if (this._updateVisualState == null)
        return;
      this._updateVisualState.UpdateVisualState(useTransitions);
    }

    public void UpdateVisualStateBase(bool useTransitions)
    {
      if (!this.Control.IsEnabled)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Disabled", "Normal");
      else if (this.IsReadOnly)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "ReadOnly", "Normal");
      else if (this.IsPressed)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Pressed", "MouseOver", "Normal");
      else if (this.IsMouseOver)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "MouseOver", "Normal");
      else
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Normal");
      if (this.IsFocused)
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Focused", "Unfocused");
      else
        VisualStates.GoToState(this.Control, (useTransitions ? 1 : 0) != 0, "Unfocused");
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => this.UpdateVisualState(false);

    private void OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(bool) e.NewValue)
      {
        this.IsPressed = false;
        this.IsMouseOver = false;
        this.IsFocused = false;
      }
      this.UpdateVisualState(true);
    }

    public void OnIsReadOnlyChanged(bool value)
    {
      this.IsReadOnly = value;
      if (!value)
      {
        this.IsPressed = false;
        this.IsMouseOver = false;
        this.IsFocused = false;
      }
      this.UpdateVisualState(true);
    }

    public void OnApplyTemplateBase() => this.UpdateVisualState(false);

    public bool AllowGotFocus(RoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      bool isEnabled = this.Control.IsEnabled;
      if (isEnabled)
        this.IsFocused = true;
      return isEnabled;
    }

    public void OnGotFocusBase() => this.UpdateVisualState(true);

    public bool AllowLostFocus(RoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      bool isEnabled = this.Control.IsEnabled;
      if (isEnabled)
        this.IsFocused = false;
      return isEnabled;
    }

    public void OnLostFocusBase()
    {
      this.IsPressed = false;
      this.UpdateVisualState(true);
    }

    public bool AllowMouseEnter(PointerRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      bool isEnabled = this.Control.IsEnabled;
      if (isEnabled)
        this.IsMouseOver = true;
      return isEnabled;
    }

    public void OnMouseEnterBase() => this.UpdateVisualState(true);

    public bool AllowMouseLeave(PointerRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      bool isEnabled = this.Control.IsEnabled;
      if (isEnabled)
        this.IsMouseOver = false;
      return isEnabled;
    }

    public void OnMouseLeaveBase() => this.UpdateVisualState(true);

    public bool AllowMouseLeftButtonDown(PointerRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      bool isEnabled = this.Control.IsEnabled;
      if (isEnabled)
      {
        DateTime utcNow = DateTime.UtcNow;
        Point position = (Point) e.GetCurrentPoint((UIElement) this.Control).Position;
        double totalMilliseconds = (utcNow - this.LastClickTime).TotalMilliseconds;
        Point lastClickPosition = this.LastClickPosition;
        double num1 = position.X - lastClickPosition.X;
        double num2 = position.Y - lastClickPosition.Y;
        double num3 = num1 * num1 + num2 * num2;
        if (totalMilliseconds < 500.0 && num3 < 9.0)
          ++this.ClickCount;
        else
          this.ClickCount = 1;
        this.LastClickTime = utcNow;
        this.LastClickPosition = position;
        this.IsPressed = true;
      }
      else
        this.ClickCount = 1;
      return isEnabled;
    }

    public void OnMouseLeftButtonDownBase() => this.UpdateVisualState(true);

    public bool AllowMouseLeftButtonUp(PointerRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      bool isEnabled = this.Control.IsEnabled;
      if (isEnabled)
        this.IsPressed = false;
      return isEnabled;
    }

    public void OnMouseLeftButtonUpBase() => this.UpdateVisualState(true);

    public bool AllowKeyDown(KeyRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      return this.Control.IsEnabled;
    }

    public bool AllowKeyUp(KeyRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      return this.Control.IsEnabled;
    }

    public static VirtualKey GetLogicalKey(FlowDirection flowDirection, VirtualKey originalKey)
    {
      VirtualKey logicalKey = originalKey;
      if (flowDirection == 1)
      {
        switch (originalKey - 37)
        {
          case 0:
            logicalKey = (VirtualKey) 39;
            break;
          case 2:
            logicalKey = (VirtualKey) 37;
            break;
        }
      }
      return logicalKey;
    }
  }
}
