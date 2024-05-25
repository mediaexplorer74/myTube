// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.WrapPanel
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls
{
  public class WrapPanel : Panel
  {
    private bool _ignorePropertyChange;
    public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof (ItemHeight), (Type) typeof (double), (Type) typeof (WrapPanel), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(WrapPanel.OnItemHeightOrWidthPropertyChanged)));
    public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof (ItemWidth), (Type) typeof (double), (Type) typeof (WrapPanel), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(WrapPanel.OnItemHeightOrWidthPropertyChanged)));
    public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), (Type) typeof (Orientation), (Type) typeof (WrapPanel), new PropertyMetadata((object) (Orientation) 1, new PropertyChangedCallback(WrapPanel.OnOrientationPropertyChanged)));

    public double ItemHeight
    {
      get => (double) ((DependencyObject) this).GetValue(WrapPanel.ItemHeightProperty);
      set => ((DependencyObject) this).SetValue(WrapPanel.ItemHeightProperty, (object) value);
    }

    public double ItemWidth
    {
      get => (double) ((DependencyObject) this).GetValue(WrapPanel.ItemWidthProperty);
      set => ((DependencyObject) this).SetValue(WrapPanel.ItemWidthProperty, (object) value);
    }

    public Orientation Orientation
    {
      get => (Orientation) ((DependencyObject) this).GetValue(WrapPanel.OrientationProperty);
      set => ((DependencyObject) this).SetValue(WrapPanel.OrientationProperty, (object) value);
    }

    private static void OnOrientationPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      WrapPanel wrapPanel = (WrapPanel) d;
      Orientation newValue = (Orientation) e.NewValue;
      if (wrapPanel._ignorePropertyChange)
      {
        wrapPanel._ignorePropertyChange = false;
      }
      else
      {
        if (newValue != 1 && newValue != null)
        {
          wrapPanel._ignorePropertyChange = true;
          ((DependencyObject) wrapPanel).SetValue(WrapPanel.OrientationProperty, (object) (Orientation) e.OldValue);
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Properties.Resources.WrapPanel_OnOrientationPropertyChanged_InvalidValue", (object) newValue), "value");
        }
        ((UIElement) wrapPanel).InvalidateMeasure();
      }
    }

    private static void OnItemHeightOrWidthPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      WrapPanel wrapPanel = (WrapPanel) d;
      double newValue = (double) e.NewValue;
      if (wrapPanel._ignorePropertyChange)
      {
        wrapPanel._ignorePropertyChange = false;
      }
      else
      {
        if (!double.IsNaN(newValue) && (newValue <= 0.0 || double.IsPositiveInfinity(newValue)))
        {
          wrapPanel._ignorePropertyChange = true;
          ((DependencyObject) wrapPanel).SetValue(e.Property, (object) (double) e.OldValue);
          throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Properties.Resources.WrapPanel_OnItemHeightOrWidthPropertyChanged_InvalidValue", (object) newValue), "value");
        }
        ((UIElement) wrapPanel).InvalidateMeasure();
      }
    }

    protected virtual Size MeasureOverride(Size constraint)
    {
      Orientation orientation = this.Orientation;
      OrientedSize orientedSize1 = new OrientedSize(orientation);
      OrientedSize orientedSize2 = new OrientedSize(orientation);
      OrientedSize orientedSize3 = new OrientedSize(orientation, constraint.Width, constraint.Height);
      double itemWidth = this.ItemWidth;
      double itemHeight = this.ItemHeight;
      bool flag1 = !double.IsNaN(itemWidth);
      bool flag2 = !double.IsNaN(itemHeight);
      Size size = new Size(flag1 ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        child.Measure((Size) size);
        OrientedSize orientedSize4 = new OrientedSize(orientation, flag1 ? itemWidth : ((Size) child.DesiredSize).Width, flag2 ? itemHeight : ((Size) child.DesiredSize).Height);
        if (NumericExtensions.IsGreaterThan(orientedSize1.Direct + orientedSize4.Direct, orientedSize3.Direct))
        {
          orientedSize2.Direct = Math.Max(orientedSize1.Direct, orientedSize2.Direct);
          orientedSize2.Indirect += orientedSize1.Indirect;
          orientedSize1 = orientedSize4;
          if (NumericExtensions.IsGreaterThan(orientedSize4.Direct, orientedSize3.Direct))
          {
            orientedSize2.Direct = Math.Max(orientedSize4.Direct, orientedSize2.Direct);
            orientedSize2.Indirect += orientedSize4.Indirect;
            orientedSize1 = new OrientedSize(orientation);
          }
        }
        else
        {
          orientedSize1.Direct += orientedSize4.Direct;
          orientedSize1.Indirect = Math.Max(orientedSize1.Indirect, orientedSize4.Indirect);
        }
      }
      orientedSize2.Direct = Math.Max(orientedSize1.Direct, orientedSize2.Direct);
      orientedSize2.Indirect += orientedSize1.Indirect;
      return new Size(orientedSize2.Width, orientedSize2.Height);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      Orientation orientation = this.Orientation;
      OrientedSize orientedSize1 = new OrientedSize(orientation);
      OrientedSize orientedSize2 = new OrientedSize(orientation, finalSize.Width, finalSize.Height);
      double itemWidth = this.ItemWidth;
      double itemHeight = this.ItemHeight;
      bool flag1 = !itemWidth.IsNaN();
      bool flag2 = !itemHeight.IsNaN();
      double indirectOffset = 0.0;
      double? directDelta = orientation == 1 ? (flag1 ? new double?(itemWidth) : new double?()) : (flag2 ? new double?(itemHeight) : new double?());
      UIElementCollection children = this.Children;
      int count = ((ICollection<UIElement>) children).Count;
      int lineStart = 0;
      for (int index = 0; index < count; ++index)
      {
        UIElement uiElement = ((IList<UIElement>) children)[index];
        OrientedSize orientedSize3 = new OrientedSize(orientation, flag1 ? itemWidth : ((Size) uiElement.DesiredSize).Width, flag2 ? itemHeight : ((Size) uiElement.DesiredSize).Height);
        if (NumericExtensions.IsGreaterThan(orientedSize1.Direct + orientedSize3.Direct, orientedSize2.Direct))
        {
          this.ArrangeLine(lineStart, index, directDelta, indirectOffset, orientedSize1.Indirect);
          indirectOffset += orientedSize1.Indirect;
          orientedSize1 = orientedSize3;
          if (NumericExtensions.IsGreaterThan(orientedSize3.Direct, orientedSize2.Direct))
          {
            this.ArrangeLine(index, ++index, directDelta, indirectOffset, orientedSize3.Indirect);
            indirectOffset += orientedSize1.Indirect;
            orientedSize1 = new OrientedSize(orientation);
          }
          lineStart = index;
        }
        else
        {
          orientedSize1.Direct += orientedSize3.Direct;
          orientedSize1.Indirect = Math.Max(orientedSize1.Indirect, orientedSize3.Indirect);
        }
      }
      if (lineStart < count)
        this.ArrangeLine(lineStart, count, directDelta, indirectOffset, orientedSize1.Indirect);
      return finalSize;
    }

    private void ArrangeLine(
      int lineStart,
      int lineEnd,
      double? directDelta,
      double indirectOffset,
      double indirectGrowth)
    {
      double num1 = 0.0;
      Orientation orientation = this.Orientation;
      bool flag = orientation == 1;
      UIElementCollection children = this.Children;
      for (int index = lineStart; index < lineEnd; ++index)
      {
        UIElement uiElement = ((IList<UIElement>) children)[index];
        OrientedSize orientedSize = new OrientedSize(orientation, ((Size) uiElement.DesiredSize).Width, ((Size) uiElement.DesiredSize).Height);
        double num2 = directDelta.HasValue ? directDelta.Value : orientedSize.Direct;
        Rect rect = flag ? new Rect(num1, indirectOffset, num2, indirectGrowth) : new Rect(indirectOffset, num1, indirectGrowth, num2);
        uiElement.Arrange((Rect) rect);
        num1 += num2;
      }
    }
  }
}
