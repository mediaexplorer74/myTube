// myTube.Converters.SubtitlePlacementToVerticalAlignment


using RykenTube;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace myTube
{
  public class SubtitlePlacementToVerticalAlignment : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, string language)
    {
      if (!(value is SubtitlePlacement subtitlePlacement))
        return (object) (VerticalAlignment) 2;

      if (subtitlePlacement == SubtitlePlacement.Bottom)
        return (object) (VerticalAlignment) 2;

      return subtitlePlacement == SubtitlePlacement.Top 
                ? (object) (VerticalAlignment) 0 
                : (object) (VerticalAlignment) 2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) 
            => throw new NotImplementedException();
  }
}
