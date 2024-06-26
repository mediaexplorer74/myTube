﻿// myTube.DateToDotString

using System;
using System.Collections.Generic;
using Windows.Globalization.DateTimeFormatting;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class DateToDotString : IValueConverter
  {
    private static readonly DateTimeFormatter formatter = new DateTimeFormatter("shortdate", (IEnumerable<string>) new string[1]
    {
      GlobalizationPreferences.Languages[0]
    });

    public object Convert(object value, Type targetType, object parameter, string language) => value != null && value is DateTime dateTime ? (object) DateToDotString.formatter.Format((DateTimeOffset) dateTime).Replace("/", ".") : (object) value.ToString();

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
