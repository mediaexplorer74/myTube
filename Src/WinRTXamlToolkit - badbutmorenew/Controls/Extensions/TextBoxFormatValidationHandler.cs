// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.TextBoxFormatValidationHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class TextBoxFormatValidationHandler : FieldValidationHandler<TextBox>
  {
    internal void Detach()
    {
      WindowsRuntimeMarshal.RemoveEventHandler<TextChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this.Field.remove_TextChanged), new TextChangedEventHandler(this.OnTextBoxTextChanged));
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this.Field).remove_Loaded), new RoutedEventHandler(this.OnTextBoxLoaded));
      this.Field = (TextBox) null;
    }

    internal void Attach(TextBox textBox)
    {
      if (this.Field == textBox)
        return;
      if (this.Field != null)
        this.Detach();
      this.Field = textBox;
      TextBox field1 = this.Field;
      WindowsRuntimeMarshal.AddEventHandler<TextChangedEventHandler>((Func<TextChangedEventHandler, EventRegistrationToken>) new Func<TextChangedEventHandler, EventRegistrationToken>(field1.add_TextChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(field1.remove_TextChanged), new TextChangedEventHandler(this.OnTextBoxTextChanged));
      TextBox field2 = this.Field;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) field2).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) field2).remove_Loaded), new RoutedEventHandler(this.OnTextBoxLoaded));
      this.Validate();
    }

    private void OnTextBoxLoaded(object sender, RoutedEventArgs e) => this.Validate();

    private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e) => this.Validate();

    protected override string GetFieldValue() => this.Field.Text;

    protected override int GetMaxLength() => this.Field.MaxLength;
  }
}
