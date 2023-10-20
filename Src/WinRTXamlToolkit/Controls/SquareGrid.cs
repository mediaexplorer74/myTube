// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.SquareGrid
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
  public class SquareGrid : Panel
  {
    protected virtual Size MeasureOverride(Size availableSize)
    {
      double num1 = Math.Min(availableSize.Width, availableSize.Height);
      double num2 = ((ICollection<UIElement>) this.Children).Count == 0 ? 1.0 : Math.Ceiling(Math.Sqrt((double) ((ICollection<UIElement>) this.Children).Count));
      double num3 = num1 / num2;
      Size size1 = new Size(num3, num3);
      double num4 = 0.0;
      double num5 = 0.0;
      double num6 = Math.Round(num3);
      int num7 = 0;
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        if (((UIElement) this).UseLayoutRounding)
        {
          double num8 = Math.Round(((double) num7 % num2 + 1.0) * num3);
          Size size2 = new Size(num8 - num4, num6 - num5);
          child.Measure((Size) size2);
          ++num7;
          if ((double) num7 % num2 != 0.0)
          {
            num4 = num8;
          }
          else
          {
            num5 = num6;
            num6 = Math.Round(Math.Floor(1.0 + (double) num7 / num2) * num3);
            num4 = 0.0;
          }
        }
        else
          child.Measure((Size) size1);
      }
      return new Size(num1, num1);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      double num1 = Math.Min(finalSize.Width, finalSize.Height);
      Size size = new Size(num1, num1);
      double num2 = ((ICollection<UIElement>) this.Children).Count == 0 ? 1.0 : Math.Ceiling(Math.Sqrt((double) ((ICollection<UIElement>) this.Children).Count));
      double num3 = num1 / num2;
      int num4 = 0;
      double x = 0.0;
      double y = 0.0;
      double num5 = Math.Round(num3);
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        if (((UIElement) this).UseLayoutRounding)
        {
          double num6 = Math.Round(((double) num4 % num2 + 1.0) * num3);
          Rect rect = new Rect(x, y, num6 - x, num5 - y);
          child.Arrange((Rect) rect);
          ++num4;
          if ((double) num4 % num2 != 0.0)
          {
            x = num6;
          }
          else
          {
            y = num5;
            num5 = Math.Round(Math.Floor(1.0 + (double) num4 / num2) * num3);
            x = 0.0;
          }
        }
        else
        {
          x = (double) num4 % num2 * num3;
          y = Math.Floor((double) num4 / num2) * num3;
          Rect rect = new Rect(x, y, num3, num3);
          child.Arrange((Rect) rect);
          ++num4;
        }
      }
      return size;
    }
  }
}
