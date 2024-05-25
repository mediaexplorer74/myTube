// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.UpDownTextBox
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
  public class UpDownTextBox : TextBox
  {
    public event EventHandler UpPressed;

    private void RaiseUpPressed()
    {
      EventHandler upPressed = this.UpPressed;
      if (upPressed == null)
        return;
      EventArgs empty = EventArgs.Empty;
      upPressed((object) this, empty);
    }

    public event EventHandler DownPressed;

    private void RaiseDownPressed()
    {
      EventHandler downPressed = this.DownPressed;
      if (downPressed == null)
        return;
      EventArgs empty = EventArgs.Empty;
      downPressed((object) this, empty);
    }

    protected virtual void OnKeyDown(KeyRoutedEventArgs e)
    {
      if (e.Key == 38)
      {
        this.RaiseUpPressed();
        e.put_Handled(true);
      }
      else if (e.Key == 40)
      {
        this.RaiseDownPressed();
        e.put_Handled(true);
      }
      else
        ((Control) this).OnKeyDown(e);
    }
  }
}
