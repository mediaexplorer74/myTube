// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.FieldValidationExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class FieldValidationExtensions
  {
    public static readonly DependencyProperty FormatProperty = DependencyProperty.RegisterAttached("Format", (Type) typeof (ValidationChecks), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) ValidationChecks.Any, new PropertyChangedCallback(FieldValidationExtensions.OnFormatChanged)));
    public static readonly DependencyProperty PatternProperty = DependencyProperty.RegisterAttached("Pattern", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(FieldValidationExtensions.OnPatternChanged)));
    public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid", (Type) typeof (bool), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) true));
    public static readonly DependencyProperty NonEmptyErrorMessageProperty = DependencyProperty.RegisterAttached("NonEmptyErrorMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) "Field can't be empty.", new PropertyChangedCallback(FieldValidationExtensions.OnNonEmptyErrorMessageChanged)));
    public static readonly DependencyProperty NumericErrorMessageProperty = DependencyProperty.RegisterAttached("NumericErrorMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) "Field value needs to be numeric.", new PropertyChangedCallback(FieldValidationExtensions.OnNumericErrorMessageChanged)));
    public static readonly DependencyProperty SpecificLengthErrorMessageProperty = DependencyProperty.RegisterAttached("SpecificLengthErrorMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) "Field needs to have {0} characters.", new PropertyChangedCallback(FieldValidationExtensions.OnSpecificLengthErrorMessageChanged)));
    public static readonly DependencyProperty PatternErrorMessageProperty = DependencyProperty.RegisterAttached("PatternErrorMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) "The field needs to match pattern \"{0}\".", new PropertyChangedCallback(FieldValidationExtensions.OnPatternErrorMessageChanged)));
    public static readonly DependencyProperty MinLengthErrorMessageProperty = DependencyProperty.RegisterAttached("MinLengthErrorMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) "Field needs to have at least {0} characters.", new PropertyChangedCallback(FieldValidationExtensions.OnMinLengthErrorMessageChanged)));
    public static readonly DependencyProperty DefaultErrorMessageProperty = DependencyProperty.RegisterAttached("DefaultErrorMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) "Invalid field value", new PropertyChangedCallback(FieldValidationExtensions.OnDefaultErrorMessageChanged)));
    public static readonly DependencyProperty ValidationMessageProperty = DependencyProperty.RegisterAttached("ValidationMessage", (Type) typeof (string), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ValidationMessageVisibilityProperty = DependencyProperty.RegisterAttached("ValidationMessageVisibility", (Type) typeof (Visibility), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) (Visibility) 1));
    public static readonly DependencyProperty FormatValidationHandlerProperty = DependencyProperty.RegisterAttached("FormatValidationHandler", (Type) typeof (object), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(FieldValidationExtensions.OnFormatValidationHandlerChanged)));
    public static readonly DependencyProperty ValidBrushProperty = DependencyProperty.RegisterAttached("ValidBrush", (Type) typeof (Brush), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) new SolidColorBrush(Colors.White), new PropertyChangedCallback(FieldValidationExtensions.OnValidBrushChanged)));
    public static readonly DependencyProperty InvalidBrushProperty = DependencyProperty.RegisterAttached("InvalidBrush", (Type) typeof (Brush), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) new SolidColorBrush(Colors.Pink), new PropertyChangedCallback(FieldValidationExtensions.OnInvalidBrushChanged)));
    public static readonly DependencyProperty MinLengthProperty = DependencyProperty.RegisterAttached("MinLength", (Type) typeof (int), (Type) typeof (FieldValidationExtensions), new PropertyMetadata((object) 8, new PropertyChangedCallback(FieldValidationExtensions.OnMinLengthChanged)));

    public static ValidationChecks GetFormat(DependencyObject d) => (ValidationChecks) d.GetValue(FieldValidationExtensions.FormatProperty);

    public static void SetFormat(DependencyObject d, ValidationChecks value) => d.SetValue(FieldValidationExtensions.FormatProperty, (object) value);

    private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => FieldValidationExtensions.SetupAndValidate(d);

    public static string GetPattern(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.PatternProperty);

    public static void SetPattern(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.PatternProperty, (object) value);

    private static void OnPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => FieldValidationExtensions.SetupAndValidate(d);

    public static bool GetIsValid(DependencyObject d) => (bool) d.GetValue(FieldValidationExtensions.IsValidProperty);

    public static void SetIsValid(DependencyObject d, bool value) => d.SetValue(FieldValidationExtensions.IsValidProperty, (object) value);

    public static string GetNonEmptyErrorMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.NonEmptyErrorMessageProperty);

    public static void SetNonEmptyErrorMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.NonEmptyErrorMessageProperty, (object) value);

    private static void OnNonEmptyErrorMessageChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FieldValidationExtensions.SetupAndValidate(d);
    }

    public static string GetNumericErrorMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.NumericErrorMessageProperty);

    public static void SetNumericErrorMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.NumericErrorMessageProperty, (object) value);

    private static void OnNumericErrorMessageChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FieldValidationExtensions.SetupAndValidate(d);
    }

    public static string GetSpecificLengthErrorMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.SpecificLengthErrorMessageProperty);

    public static void SetSpecificLengthErrorMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.SpecificLengthErrorMessageProperty, (object) value);

    private static void OnSpecificLengthErrorMessageChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FieldValidationExtensions.SetupAndValidate(d);
    }

    public static string GetPatternErrorMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.PatternErrorMessageProperty);

    public static void SetPatternErrorMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.PatternErrorMessageProperty, (object) value);

    private static void OnPatternErrorMessageChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FieldValidationExtensions.SetupAndValidate(d);
    }

    public static string GetMinLengthErrorMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.MinLengthErrorMessageProperty);

    public static void SetMinLengthErrorMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.MinLengthErrorMessageProperty, (object) value);

    private static void OnMinLengthErrorMessageChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      string oldValue = (string) e.OldValue;
      string str = (string) d.GetValue(FieldValidationExtensions.MinLengthErrorMessageProperty);
    }

    public static string GetDefaultErrorMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.DefaultErrorMessageProperty);

    public static void SetDefaultErrorMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.DefaultErrorMessageProperty, (object) value);

    private static void OnDefaultErrorMessageChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      string oldValue = (string) e.OldValue;
      string str = (string) d.GetValue(FieldValidationExtensions.DefaultErrorMessageProperty);
    }

    public static string GetValidationMessage(DependencyObject d) => (string) d.GetValue(FieldValidationExtensions.ValidationMessageProperty);

    public static void SetValidationMessage(DependencyObject d, string value) => d.SetValue(FieldValidationExtensions.ValidationMessageProperty, (object) value);

    public static Visibility GetValidationMessageVisibility(DependencyObject d) => (Visibility) d.GetValue(FieldValidationExtensions.ValidationMessageVisibilityProperty);

    public static void SetValidationMessageVisibility(DependencyObject d, Visibility value) => d.SetValue(FieldValidationExtensions.ValidationMessageVisibilityProperty, (object) value);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static object GetFormatValidationHandler(DependencyObject d) => d.GetValue(FieldValidationExtensions.FormatValidationHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetFormatValidationHandler(DependencyObject d, object value) => d.SetValue(FieldValidationExtensions.FormatValidationHandlerProperty, value);

    private static void OnFormatValidationHandlerChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TextBox textBox = d as TextBox;
      PasswordBox passwordBox = d as PasswordBox;
      if (textBox != null)
      {
        TextBoxFormatValidationHandler oldValue = (TextBoxFormatValidationHandler) e.OldValue;
        TextBoxFormatValidationHandler validationHandler = (TextBoxFormatValidationHandler) ((DependencyObject) textBox).GetValue(FieldValidationExtensions.FormatValidationHandlerProperty);
        oldValue?.Detach();
        validationHandler.Attach(textBox);
      }
      else
      {
        if (passwordBox == null)
          return;
        PasswordBoxFormatValidationHandler oldValue = (PasswordBoxFormatValidationHandler) e.OldValue;
        PasswordBoxFormatValidationHandler validationHandler = (PasswordBoxFormatValidationHandler) ((DependencyObject) passwordBox).GetValue(FieldValidationExtensions.FormatValidationHandlerProperty);
        oldValue?.Detach();
        validationHandler.Attach(passwordBox);
      }
    }

    public static Brush GetValidBrush(DependencyObject d) => (Brush) d.GetValue(FieldValidationExtensions.ValidBrushProperty);

    public static void SetValidBrush(DependencyObject d, Brush value) => d.SetValue(FieldValidationExtensions.ValidBrushProperty, (object) value);

    private static void OnValidBrushChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FieldValidationExtensions.SetupAndValidate(d);
    }

    public static Brush GetInvalidBrush(DependencyObject d) => (Brush) d.GetValue(FieldValidationExtensions.InvalidBrushProperty);

    public static void SetInvalidBrush(DependencyObject d, Brush value) => d.SetValue(FieldValidationExtensions.InvalidBrushProperty, (object) value);

    private static void OnInvalidBrushChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      FieldValidationExtensions.SetupAndValidate(d);
    }

    public static int GetMinLength(DependencyObject d) => (int) d.GetValue(FieldValidationExtensions.MinLengthProperty);

    public static void SetMinLength(DependencyObject d, int value) => d.SetValue(FieldValidationExtensions.MinLengthProperty, (object) value);

    private static void OnMinLengthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      int oldValue = (int) e.OldValue;
      int num = (int) d.GetValue(FieldValidationExtensions.MinLengthProperty);
    }

    private static void SetupAndValidate(DependencyObject dependencyObject)
    {
      TextBox d1 = dependencyObject as TextBox;
      PasswordBox d2 = dependencyObject as PasswordBox;
      if (d1 != null)
      {
        if (!(FieldValidationExtensions.GetFormatValidationHandler((DependencyObject) d1) is TextBoxFormatValidationHandler validationHandler1))
        {
          TextBoxFormatValidationHandler validationHandler = new TextBoxFormatValidationHandler();
          FieldValidationExtensions.SetFormatValidationHandler((DependencyObject) d1, (object) validationHandler);
        }
        else
          validationHandler1.Validate();
      }
      else
      {
        if (d2 == null)
          return;
        if (!(FieldValidationExtensions.GetFormatValidationHandler((DependencyObject) d2) is PasswordBoxFormatValidationHandler validationHandler3))
        {
          PasswordBoxFormatValidationHandler validationHandler2 = new PasswordBoxFormatValidationHandler();
          FieldValidationExtensions.SetFormatValidationHandler((DependencyObject) d2, (object) validationHandler2);
        }
        else
          validationHandler3.Validate();
      }
    }
  }
}
