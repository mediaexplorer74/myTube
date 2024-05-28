// myTube.WrapPanel

using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace myTube
{
  public partial class WrapPanel : Panel
  {
    public static DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), 
        typeof (double), typeof (WrapPanel), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(WrapPanel.OnItemHeightChanged)));

    private static void OnItemHeightChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((UIElement) (d as WrapPanel)).InvalidateMeasure();
    }

    public double ItemHeight
    {
      get => (double) ((DependencyObject) this).GetValue(WrapPanel.ItemHeightProperty);
      set => ((DependencyObject) this).SetValue(WrapPanel.ItemHeightProperty, (object) value);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      double x = 0.0;
      double y = 0.0;
      double num1 = 0.0;
      double num2 = double.IsNaN(this.ItemHeight) ? 0.0 : this.ItemHeight;
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        Size desiredSize = child.DesiredSize;
        if (x + desiredSize.Width >= finalSize.Width + 0.5)
        {
          x = 0.0;
          y += num2;
        }
        child.Arrange(new Rect(x, y, desiredSize.Width, double.IsNaN(this.ItemHeight) 
            ? desiredSize.Height 
            : this.ItemHeight));
        if (double.IsNaN(this.ItemHeight) && desiredSize.Height > num2)
          num2 = desiredSize.Height;
        x += desiredSize.Width;
        if (x > num1)
          num1 = x;
      }
      return finalSize;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      double width = 0.0;
      double num3 = double.IsNaN(this.ItemHeight) ? 0.0 : this.ItemHeight;
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        child.Measure(new Size(availableSize.Width, double.IsNaN(this.ItemHeight) 
            ? availableSize.Height : this.ItemHeight));
        Size desiredSize = child.DesiredSize;
        if (double.IsNaN(this.ItemHeight) && desiredSize.Height > num3)
          num3 = desiredSize.Height;
        num1 += desiredSize.Width;
        if (num1 >= availableSize.Width)
        {
          num1 = desiredSize.Width;
          num2 += num3;
        }
        if (num1 > width)
          width = num1;
      }
      return new Size(width, num2 + num3);
    }
  }
}
