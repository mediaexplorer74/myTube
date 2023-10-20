// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.FieldValidationHandler`1
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public abstract class FieldValidationHandler<T> where T : Control
  {
    protected T Field;

    protected abstract string GetFieldValue();

    protected abstract int GetMaxLength();

    protected virtual int GetMinLength() => FieldValidationExtensions.GetMinLength((DependencyObject) (object) this.Field);

    internal void Validate()
    {
      ValidationChecks format = FieldValidationExtensions.GetFormat((DependencyObject) (object) this.Field);
      bool isEmpty;
      if (!this.ValidateNonEmpty(format, out isEmpty) || !this.ValidateNumeric(format, isEmpty) || !this.ValidateSpecificLength(format) || !this.ValidateMinLength(format) || !this.ValidateMatchesRegexPattern(format) || !this.ValidateEqualsPattern(format) || !this.ValidateIncludesLowercase(format) || !this.ValidateIncludesUppercase(format) || !this.ValidateIncludesDigits(format) || !this.ValidateIncludesSymbols(format) || !this.ValidateNoDoubles(format))
        return;
      this.MarkValid();
    }

    private bool ValidateNonEmpty(ValidationChecks format, out bool isEmpty)
    {
      bool flag = (format & ValidationChecks.NonEmpty) != ValidationChecks.Any;
      isEmpty = string.IsNullOrWhiteSpace(this.GetFieldValue());
      if (!flag || !isEmpty)
        return true;
      this.MarkInvalid(FieldValidationExtensions.GetNonEmptyErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateNumeric(ValidationChecks format, bool isEmpty)
    {
      if ((format & ValidationChecks.Numeric) == ValidationChecks.Any || isEmpty || this.IsNumeric())
        return true;
      this.MarkInvalid(FieldValidationExtensions.GetNumericErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateSpecificLength(ValidationChecks format)
    {
      if ((format & ValidationChecks.SpecificLength) == ValidationChecks.Any || this.GetMaxLength() <= 0 || this.GetMaxLength() == this.GetFieldValue().Length)
        return true;
      this.MarkInvalid(string.Format(FieldValidationExtensions.GetSpecificLengthErrorMessage((DependencyObject) (object) this.Field) ?? "", (object) this.GetMaxLength()));
      return false;
    }

    private bool ValidateMinLength(ValidationChecks format)
    {
      if ((format & ValidationChecks.MinLength) == ValidationChecks.Any || this.GetMinLength() <= this.GetFieldValue().Length)
        return true;
      this.MarkInvalid(string.Format(FieldValidationExtensions.GetMinLengthErrorMessage((DependencyObject) (object) this.Field) ?? "", (object) this.GetMinLength()));
      return false;
    }

    private bool ValidateMatchesRegexPattern(ValidationChecks format)
    {
      bool flag = (format & ValidationChecks.MatchesRegexPattern) != ValidationChecks.Any;
      string pattern = FieldValidationExtensions.GetPattern((DependencyObject) (object) this.Field);
      if (!flag || pattern == null || Regex.IsMatch(this.GetFieldValue(), pattern))
        return true;
      this.MarkInvalid(string.Format(FieldValidationExtensions.GetPatternErrorMessage((DependencyObject) (object) this.Field) ?? "", (object) pattern));
      return false;
    }

    private bool ValidateEqualsPattern(ValidationChecks format)
    {
      bool flag = (format & ValidationChecks.EqualsPattern) != ValidationChecks.Any;
      string pattern = FieldValidationExtensions.GetPattern((DependencyObject) (object) this.Field);
      if (!flag || pattern == null || this.GetFieldValue().Equals(pattern, StringComparison.Ordinal))
        return true;
      this.MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateIncludesLowercase(ValidationChecks format)
    {
      if ((format & ValidationChecks.IncludesLowercase) == ValidationChecks.Any)
        return true;
      string fieldValue = this.GetFieldValue();
      for (int index = 0; index < fieldValue.Length; ++index)
      {
        if (char.IsLower(fieldValue, index))
          return true;
      }
      this.MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateIncludesUppercase(ValidationChecks format)
    {
      if ((format & ValidationChecks.IncludesUppercase) == ValidationChecks.Any)
        return true;
      string fieldValue = this.GetFieldValue();
      for (int index = 0; index < fieldValue.Length; ++index)
      {
        if (char.IsUpper(fieldValue, index))
          return true;
      }
      this.MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateIncludesDigits(ValidationChecks format)
    {
      if ((format & ValidationChecks.IncludesDigits) == ValidationChecks.Any)
        return true;
      string fieldValue = this.GetFieldValue();
      for (int index = 0; index < fieldValue.Length; ++index)
      {
        if (char.IsDigit(fieldValue, index))
          return true;
      }
      this.MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateIncludesSymbols(ValidationChecks format)
    {
      if ((format & ValidationChecks.IncludesSymbol) == ValidationChecks.Any)
        return true;
      string fieldValue = this.GetFieldValue();
      for (int index = 0; index < fieldValue.Length; ++index)
      {
        if (char.IsSymbol(fieldValue, index) || char.IsPunctuation(fieldValue, index))
          return true;
      }
      this.MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage((DependencyObject) (object) this.Field));
      return false;
    }

    private bool ValidateNoDoubles(ValidationChecks format)
    {
      if ((format & ValidationChecks.IncludesSymbol) != ValidationChecks.Any)
      {
        string fieldValue = this.GetFieldValue();
        for (int index1 = 0; index1 < fieldValue.Length; ++index1)
        {
          for (int index2 = 1; index1 + index2 * 2 < fieldValue.Length; ++index2)
          {
            bool flag = true;
            for (int index3 = 0; index3 < index2; ++index3)
            {
              if ((int) fieldValue[index1 + index3] != (int) fieldValue[index1 + index2 + index3])
              {
                flag = false;
                break;
              }
            }
            if (flag)
            {
              this.MarkInvalid(FieldValidationExtensions.GetDefaultErrorMessage((DependencyObject) (object) this.Field));
              return false;
            }
          }
        }
      }
      return true;
    }

    private bool IsNumeric() => double.TryParse(this.GetFieldValue(), out double _);

    protected virtual void MarkValid()
    {
      this.Field.put_Background(FieldValidationExtensions.GetValidBrush((DependencyObject) (object) this.Field));
      FieldValidationExtensions.SetIsValid((DependencyObject) (object) this.Field, true);
      FieldValidationExtensions.SetValidationMessage((DependencyObject) (object) this.Field, (string) null);
      FieldValidationExtensions.SetValidationMessageVisibility((DependencyObject) (object) this.Field, (Visibility) 1);
    }

    protected virtual void MarkInvalid(string errorMessage)
    {
      this.Field.put_Background(FieldValidationExtensions.GetInvalidBrush((DependencyObject) (object) this.Field));
      FieldValidationExtensions.SetIsValid((DependencyObject) (object) this.Field, false);
      FieldValidationExtensions.SetValidationMessage((DependencyObject) (object) this.Field, errorMessage);
      FieldValidationExtensions.SetValidationMessageVisibility((DependencyObject) (object) this.Field, (Visibility) 0);
    }
  }
}
