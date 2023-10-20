// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.PasswordAutoTabOnMaxLengthHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class PasswordAutoTabOnMaxLengthHandler
  {
    private PasswordBox _associatedObject;

    public PasswordAutoTabOnMaxLengthHandler(PasswordBox associatedObject) => this.Attach(associatedObject);

    private void Attach(PasswordBox associatedObject)
    {
      this.Detach();
      this._associatedObject = associatedObject;
      PasswordBox associatedObject1 = this._associatedObject;
      WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) associatedObject1).add_KeyUp), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) associatedObject1).remove_KeyUp), new KeyEventHandler(this.AssociatedObjectOnKeyUp));
    }

    private void AssociatedObjectOnKeyUp(object sender, KeyRoutedEventArgs keyRoutedEventArgs)
    {
      if (this._associatedObject.Password.Length != this._associatedObject.MaxLength || PasswordAutoTabOnMaxLengthHandler.IsSystemKey(keyRoutedEventArgs.Key))
        return;
      ((Control) this._associatedObject).MoveFocusForward();
    }

    private static bool IsSystemKey(VirtualKey key) => key == 18 || key == 164 || key == 165 || key == 162 || key == 163 || key == 91 || key == 92 || key == 16 || key == 38 || key == 39 || key == 40 || key == 37 || key == 160 || key == 161 || key == 9 || key == 8 || key == 46 || key == 112 || key == 113 || key == 114 || key == 115 || key == 116 || key == 117 || key == 118 || key == 119 || key == 120 || key == 121 || key == 122 || key == 123 || key == 124 || key == 125 || key == 126 || key == (int) sbyte.MaxValue || key == 128 || key == 129 || key == 130 || key == 131 || key == 132 || key == 133 || key == 134 || key == 135 || key == 34 || key == 33 || key == 36 || key == 35 || key == 145 || key == 45 || key == 27;

    public void Detach()
    {
      if (this._associatedObject == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<KeyEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._associatedObject).remove_KeyUp), new KeyEventHandler(this.AssociatedObjectOnKeyUp));
      this._associatedObject = (PasswordBox) null;
    }
  }
}
