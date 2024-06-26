﻿// myTube.TimeShortener

using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class TimeShortener : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (!(value is TimeSpan timeSpan))
        return value;
      if (timeSpan == TimeSpan.Zero)
        return (object) "LIVE";
      string newValue;
      if (timeSpan.TotalHours >= 1.0)
        newValue = timeSpan.Hours.ToString() + ":" + (timeSpan.Minutes < 10 ? (object) "0" : (object) "") + (object) timeSpan.Minutes + ":" + (timeSpan.Seconds < 10 ? (object) "0" : (object) "") + (object) timeSpan.Seconds;
      else
        newValue = timeSpan.Minutes.ToString() + ":" + (timeSpan.Seconds < 10 ? (object) "0" : (object) "") + (object) timeSpan.Seconds;
      string str = (string) null;
      if (parameter != null)
        str = parameter.ToString();
      if (str != null)
      {
        if (str.Contains("*"))
          newValue = str.Replace("*", newValue);
      }
      return (object) newValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
