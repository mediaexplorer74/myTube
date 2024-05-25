// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.AutoSelectOnFocusHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class AutoSelectOnFocusHandler
  {
    private TextBox _associatedObject;

    public AutoSelectOnFocusHandler(TextBox associatedObject) => this.Attach(associatedObject);

    private void Attach(TextBox associatedObject)
    {
      this.Detach();
      this._associatedObject = associatedObject;
      TextBox associatedObject1 = this._associatedObject;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) associatedObject1).add_GotFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) associatedObject1).remove_GotFocus), new RoutedEventHandler(this.AssociatedObjectOnGotFocus));
    }

    private void AssociatedObjectOnGotFocus(object sender, RoutedEventArgs routedEventArgs) => this._associatedObject.SelectAll();

    public void Detach()
    {
      if (this._associatedObject == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<KeyEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._associatedObject).remove_KeyUp), new KeyEventHandler(this.AssociatedObjectOnGotFocus));
      this._associatedObject = (TextBox) null;
    }
  }
}
