// myTube.ToAgoString


using System;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class ToAgoString : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      DateTime dateTime = DateTime.UtcNow;
      DateTime utcNow = DateTime.UtcNow;
      switch (value)
      {
        case long fileTime:
          try
          {
            dateTime = DateTime.FromFileTimeUtc(fileTime);
            break;
          }
          catch
          {
            dateTime = DateTime.FromBinary((long) value);
            break;
          }
        case double _:
          dateTime = parameter == null ? DateTime.UtcNow - TimeSpan.FromDays((double) value) : (parameter != (object) "days" ? (parameter != (object) "hours" ? DateTime.UtcNow - TimeSpan.FromDays((double) value) : DateTime.UtcNow - TimeSpan.FromHours((double) value)) : DateTime.UtcNow - TimeSpan.FromDays((double) value));
          break;
        case DateTime _:
          if (parameter == null)
          {
            dateTime = (DateTime) value;
            break;
          }
          switch (parameter.ToString())
          {
            case "toUTC":
              dateTime = ((DateTime) value).ToUniversalTime();
              break;
            case "fromUTC":
              dateTime = ((DateTime) value).ToLocalTime();
              break;
            default:
              dateTime = (DateTime) value;
              break;
          }
          break;
      }
      TimeSpan time = utcNow - dateTime;
      if (time.TotalMinutes < 1.0)
        return (object) "less than a minute ago";
      if (time.TotalHours < 1.0)
        return (int) time.TotalMinutes == 1 ? (object) "a minute ago" : (object) (((int) time.TotalMinutes).ToString() + " minutes ago");
      if (time.TotalDays < 1.0)
        return (int) time.TotalHours == 1 ? (object) "an hour ago" : (object) (((int) time.TotalHours).ToString() + " hours ago");
      if (time.TotalWeeks() < 1.0)
        return (int) time.TotalDays == 1 ? (object) "a day ago" : (object) (((int) time.TotalDays).ToString() + " days ago");
      if (time.TotalMonths() < 1.0)
        return (int) time.TotalWeeks() == 1 ? (object) "a week ago" : (object) (((int) time.TotalWeeks()).ToString() + " weeks ago");
      if (time.TotalMonths() >= 12.0)
        return (object) "over a year ago";
      return (int) time.TotalMonths() == 1 ? (object) "a month ago" : (object) (((int) time.TotalMonths()).ToString() + " months ago");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
  }
}
