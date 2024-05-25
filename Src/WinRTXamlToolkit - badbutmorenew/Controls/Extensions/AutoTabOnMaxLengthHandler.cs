// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.AutoTabOnMaxLengthHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class AutoTabOnMaxLengthHandler
  {
    private TextBox _associatedObject;

    public AutoTabOnMaxLengthHandler(TextBox associatedObject) => this.Attach(associatedObject);

    private void Attach(TextBox associatedObject)
    {
      this.Detach();
      this._associatedObject = associatedObject;
      TextBox associatedObject1 = this._associatedObject;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(associatedObject1.add_SelectionChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(associatedObject1.remove_SelectionChanged), new RoutedEventHandler(this.AssociatedObjectOnSelectionChanged));
    }

    private void AssociatedObjectOnSelectionChanged(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this._associatedObject.SelectionStart != this._associatedObject.MaxLength)
        return;
      ((Control) this._associatedObject).MoveFocusForward();
    }

    public void Detach()
    {
      if (this._associatedObject == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._associatedObject.remove_SelectionChanged), new RoutedEventHandler(this.AssociatedObjectOnSelectionChanged));
      this._associatedObject = (TextBox) null;
    }

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
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._associatedObject).remove_GotFocus), new RoutedEventHandler(this.AssociatedObjectOnGotFocus));
        this._associatedObject = (TextBox) null;
      }
    }
  }
}
