// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Input.KeyCommand
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Windows.Input;

namespace WinRTXamlToolkit.Input
{
  public class KeyCommand : ICommand
  {
    public string KeyGestureString { get; set; }

    public event EventHandler Invoked;

    private void RaiseInvoked()
    {
      EventHandler invoked = this.Invoked;
      if (invoked == null)
        return;
      invoked((object) this, EventArgs.Empty);
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
    }

    public event EventHandler CanExecuteChanged;
  }
}
