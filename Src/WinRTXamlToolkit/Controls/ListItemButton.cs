// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ListItemButton
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
  public class ListItemButton : ContentControl
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), (Type) typeof (ICommand), (Type) typeof (ListItemButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ListItemButton.OnCommandChanged)));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), (Type) typeof (object), (Type) typeof (ListItemButton), new PropertyMetadata((object) null, new PropertyChangedCallback(ListItemButton.OnCommandParameterChanged)));

    public ICommand Command
    {
      get => (ICommand) ((DependencyObject) this).GetValue(ListItemButton.CommandProperty);
      set => ((DependencyObject) this).SetValue(ListItemButton.CommandProperty, (object) value);
    }

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ListItemButton listItemButton = (ListItemButton) d;
      ICommand oldValue = (ICommand) e.OldValue;
      ICommand command = listItemButton.Command;
      listItemButton.OnCommandChanged(oldValue, command);
    }

    protected virtual void OnCommandChanged(ICommand oldCommand, ICommand newCommand)
    {
    }

    public object CommandParameter
    {
      get => ((DependencyObject) this).GetValue(ListItemButton.CommandParameterProperty);
      set => ((DependencyObject) this).SetValue(ListItemButton.CommandParameterProperty, value);
    }

    private static void OnCommandParameterChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ListItemButton listItemButton = (ListItemButton) d;
      object oldValue = e.OldValue;
      object commandParameter = listItemButton.CommandParameter;
      listItemButton.OnCommandParameterChanged(oldValue, commandParameter);
    }

    protected virtual void OnCommandParameterChanged(
      object oldCommandParameter,
      object newCommandParameter)
    {
    }

    public event RoutedEventHandler Click;

    public ListItemButton() => ((Control) this).put_DefaultStyleKey((object) typeof (ListItemButton));

    protected virtual void OnTapped(TappedRoutedEventArgs e)
    {
      ((Control) this).OnTapped(e);
      if (this.Click != null)
        this.Click((object) this, new RoutedEventArgs());
      if (this.Command == null || !this.Command.CanExecute(this.CommandParameter))
        return;
      this.Command.Execute(this.CommandParameter);
    }

    protected virtual void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
    {
    }

    protected virtual void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
    {
    }
  }
}
