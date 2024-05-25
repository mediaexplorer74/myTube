// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ValidationChecks
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;

namespace WinRTXamlToolkit.Controls.Extensions
{
  [Flags]
  public enum ValidationChecks
  {
    Any = 0,
    NonEmpty = 1,
    Numeric = 2,
    SpecificLength = 4,
    MinLength = 8,
    MatchesRegexPattern = 16, // 0x00000010
    EqualsPattern = 32, // 0x00000020
    IncludesLowercase = 64, // 0x00000040
    IncludesUppercase = 128, // 0x00000080
    IncludesDigits = 256, // 0x00000100
    IncludesSymbol = 512, // 0x00000200
    NoDoubles = 1024, // 0x00000400
    NonEmptyNumeric = Numeric | NonEmpty, // 0x00000003
    IsStrongPassword = NoDoubles | IncludesSymbol | IncludesDigits | IncludesUppercase | IncludesLowercase | MinLength, // 0x000007C8
  }
}
