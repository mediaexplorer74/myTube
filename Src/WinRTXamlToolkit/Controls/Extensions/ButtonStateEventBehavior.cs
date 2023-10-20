// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ButtonStateEventBehavior
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class ButtonStateEventBehavior : FrameworkElement
  {
    private ButtonBase _button;
    public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof (IsPressed), (Type) typeof (bool), (Type) typeof (ButtonStateEventBehavior), new PropertyMetadata((object) false, new PropertyChangedCallback(ButtonStateEventBehavior.OnIsPressedChanged)));
    public static readonly DependencyProperty UpCommandProperty = DependencyProperty.Register(nameof (UpCommand), (Type) typeof (ICommand), (Type) typeof (ButtonStateEventBehavior), new PropertyMetadata((object) null));
    public static readonly DependencyProperty UpCommandParameterProperty = DependencyProperty.Register(nameof (UpCommandParameter), (Type) typeof (object), (Type) typeof (ButtonStateEventBehavior), new PropertyMetadata((object) null));
    public static readonly DependencyProperty DownCommandProperty = DependencyProperty.Register(nameof (DownCommand), (Type) typeof (ICommand), (Type) typeof (ButtonStateEventBehavior), new PropertyMetadata((object) null));
    public static readonly DependencyProperty DownCommandParameterProperty = DependencyProperty.Register(nameof (DownCommandParameter), (Type) typeof (object), (Type) typeof (ButtonStateEventBehavior), new PropertyMetadata((object) null));

    public event EventHandler Up;

    public event EventHandler Down;

    public bool IsPressed
    {
      get => (bool) ((DependencyObject) this).GetValue(ButtonStateEventBehavior.IsPressedProperty);
      set => ((DependencyObject) this).SetValue(ButtonStateEventBehavior.IsPressedProperty, (object) value);
    }

    private static void OnIsPressedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ButtonStateEventBehavior stateEventBehavior = (ButtonStateEventBehavior) d;
      bool oldValue = (bool) e.OldValue;
      bool isPressed = stateEventBehavior.IsPressed;
      stateEventBehavior.OnIsPressedChanged(oldValue, isPressed);
    }

    private void OnIsPressedChanged(bool oldIsPressed, bool newIsPressed)
    {
      if (newIsPressed)
        this.OnDownInternal();
      else
        this.OnUpInternal();
    }

    public ICommand UpCommand
    {
      get => (ICommand) ((DependencyObject) this).GetValue(ButtonStateEventBehavior.UpCommandProperty);
      set => ((DependencyObject) this).SetValue(ButtonStateEventBehavior.UpCommandProperty, (object) value);
    }

    public object UpCommandParameter
    {
      get => ((DependencyObject) this).GetValue(ButtonStateEventBehavior.UpCommandParameterProperty);
      set => ((DependencyObject) this).SetValue(ButtonStateEventBehavior.UpCommandParameterProperty, value);
    }

    public ICommand DownCommand
    {
      get => (ICommand) ((DependencyObject) this).GetValue(ButtonStateEventBehavior.DownCommandProperty);
      set => ((DependencyObject) this).SetValue(ButtonStateEventBehavior.DownCommandProperty, (object) value);
    }

    public object DownCommandParameter
    {
      get => ((DependencyObject) this).GetValue(ButtonStateEventBehavior.DownCommandParameterProperty);
      set => ((DependencyObject) this).SetValue(ButtonStateEventBehavior.DownCommandParameterProperty, value);
    }

    public void Attach(ButtonBase button)
    {
      this.Detach();
      if (button == null)
        return;
      this._button = button;
      DependencyProperty isPressedProperty = ButtonStateEventBehavior.IsPressedProperty;
      Binding binding1 = new Binding();
      binding1.put_Source((object) button);
      binding1.put_Path(new PropertyPath("IsPressed"));
      Binding binding2 = binding1;
      this.SetBinding(isPressedProperty, (BindingBase) binding2);
    }

    public void Detach()
    {
      if (this._button == null)
        return;
      this._button = (ButtonBase) null;
      ((DependencyObject) this).ClearValue(ButtonStateEventBehavior.IsPressedProperty);
    }

    protected virtual void OnUp()
    {
    }

    protected virtual void OnDown()
    {
    }

    private void OnUpInternal()
    {
      this.OnUp();
      EventHandler up = this.Up;
      if (up != null)
        up((object) this._button, EventArgs.Empty);
      if (this.UpCommand == null || !this.UpCommand.CanExecute(this.UpCommandParameter))
        return;
      this.UpCommand.Execute(this.UpCommandParameter);
    }

    private void OnDownInternal()
    {
      this.OnDown();
      EventHandler down = this.Down;
      if (down != null)
        down((object) this._button, EventArgs.Empty);
      if (this.DownCommand == null || !this.DownCommand.CanExecute(this.DownCommandParameter))
        return;
      this.DownCommand.Execute(this.DownCommandParameter);
    }
  }
}
