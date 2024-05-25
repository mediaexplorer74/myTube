// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.PasswordBoxFormatValidationHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class PasswordBoxFormatValidationHandler : FieldValidationHandler<PasswordBox>
  {
    internal void Detach()
    {
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.Field.remove_PasswordChanged), new RoutedEventHandler(this.OnPasswordChanged));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this.Field).remove_Loaded), new RoutedEventHandler(this.OnPasswordBoxLoaded));
      this.Field = (PasswordBox) null;
    }

    internal void Attach(PasswordBox passwordBox)
    {
      if (this.Field == passwordBox)
        return;
      if (this.Field != null)
        this.Detach();
      this.Field = passwordBox;
      PasswordBox field1 = this.Field;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(field1.add_PasswordChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(field1.remove_PasswordChanged), new RoutedEventHandler(this.OnPasswordChanged));
      PasswordBox field2 = this.Field;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) field2).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) field2).remove_Loaded), new RoutedEventHandler(this.OnPasswordBoxLoaded));
      this.Validate();
    }

    private void OnPasswordBoxLoaded(object sender, RoutedEventArgs e) => this.Validate();

    private void OnPasswordChanged(object sender, RoutedEventArgs routedEventArgs) => this.Validate();

    protected override string GetFieldValue() => this.Field.Password;

    protected override int GetMaxLength() => this.Field.MaxLength;
  }
}
