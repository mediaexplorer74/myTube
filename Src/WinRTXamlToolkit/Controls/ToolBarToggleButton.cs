// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ToolBarToggleButton
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using WinRTXamlToolkit.Input;

namespace WinRTXamlToolkit.Controls
{
  public sealed class ToolBarToggleButton : ToggleButton, IToolStripElement
  {
    private const string PlacedInBarStateName = "PlacedInBar";
    private const string PlacedInDropDownStateName = "PlacedInDropDown";
    private ToggleButtonAutomationPeer peer;
    private KeyGesture keyGesture;
    private KeyGestureRecognizer keyGestureRecognizer;
    private static readonly DependencyProperty _ShortcutProperty = DependencyProperty.Register(nameof (Shortcut), (Type) typeof (string), (Type) typeof (ToolBarToggleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolBarToggleButton.OnShortcutChanged)));
    private bool _isInDropDown;

    public static DependencyProperty ShortcutProperty => ToolBarToggleButton._ShortcutProperty;

    public string Shortcut
    {
      get => (string) ((DependencyObject) this).GetValue(ToolBarToggleButton.ShortcutProperty);
      set => ((DependencyObject) this).SetValue(ToolBarToggleButton.ShortcutProperty, (object) value);
    }

    private static void OnShortcutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToolBarToggleButton toolBarToggleButton = (ToolBarToggleButton) d;
      string oldValue = (string) e.OldValue;
      string shortcut = toolBarToggleButton.Shortcut;
      toolBarToggleButton.OnShortcutChanged(oldValue, shortcut);
    }

    private void OnShortcutChanged(string oldShortcut, string newShortcut)
    {
      AutomationProperties.SetAcceleratorKey((DependencyObject) this, newShortcut);
      if (this.keyGestureRecognizer != null)
      {
        this.keyGestureRecognizer.GestureRecognized -= new EventHandler(this.OnKeyGestureRecognized);
        this.keyGestureRecognizer.Dispose();
      }
      this.keyGesture = string.IsNullOrEmpty(newShortcut) ? (KeyGesture) null : KeyGesture.Parse(newShortcut);
      if (this.keyGesture != null)
      {
        this.keyGestureRecognizer = new KeyGestureRecognizer(this.keyGesture);
        this.keyGestureRecognizer.GestureRecognized += new EventHandler(this.OnKeyGestureRecognized);
      }
      this.UpdateToolTip();
    }

    private void OnKeyGestureRecognized(object sender, EventArgs e)
    {
      if (((Control) this).IsTabStop)
        ((Control) this).Focus((FocusState) 3);
      this.Toggle();
    }

    public bool IsInDropDown
    {
      get => this._isInDropDown;
      set
      {
        if (this._isInDropDown == value)
          return;
        this._isInDropDown = value;
        this.UpdateVisualStates(true);
        this.UpdateToolTip();
      }
    }

    public ToolBarToggleButton()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (ToolBarToggleButton));
      this.peer = new ToggleButtonAutomationPeer((ToggleButton) this);
    }

    private void UpdateVisualStates(bool useTransitions)
    {
      if (this.IsInDropDown)
        VisualStateManager.GoToState((Control) this, "PlacedInDropDown", useTransitions);
      else
        VisualStateManager.GoToState((Control) this, "PlacedInBar", useTransitions);
    }

    private void UpdateToolTip()
    {
      if (this.IsInDropDown || string.IsNullOrEmpty(this.Shortcut))
        ToolTipService.SetToolTip((DependencyObject) this, (object) null);
      else
        ToolTipService.SetToolTip((DependencyObject) this, (object) string.Format("({0})", (object) this.Shortcut));
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.UpdateVisualStates(false);
    }

    protected virtual AutomationPeer OnCreateAutomationPeer() => (AutomationPeer) this.peer;

    internal void Toggle() => this.peer.Toggle();
  }
}
