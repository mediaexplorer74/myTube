// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.UniformGrid
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
  public class UniformGrid : Panel
  {
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof (Columns), (Type) typeof (int), (Type) typeof (UniformGrid), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(nameof (Rows), (Type) typeof (int), (Type) typeof (UniformGrid), new PropertyMetadata((object) 0));
    public static readonly DependencyProperty FirstColumnProperty = DependencyProperty.Register(nameof (FirstColumn), (Type) typeof (int), (Type) typeof (UniformGrid), new PropertyMetadata((object) 0));
    private int _columns;
    private int _rows;

    public int Columns
    {
      get => (int) ((DependencyObject) this).GetValue(UniformGrid.ColumnsProperty);
      set => ((DependencyObject) this).SetValue(UniformGrid.ColumnsProperty, (object) value);
    }

    public int FirstColumn
    {
      get => (int) ((DependencyObject) this).GetValue(UniformGrid.FirstColumnProperty);
      set => ((DependencyObject) this).SetValue(UniformGrid.FirstColumnProperty, (object) value);
    }

    public int Rows
    {
      get => (int) ((DependencyObject) this).GetValue(UniformGrid.RowsProperty);
      set => ((DependencyObject) this).SetValue(UniformGrid.RowsProperty, (object) value);
    }

    protected virtual Size ArrangeOverride(Size arrangeSize)
    {
      Rect rect = new Rect(0.0, 0.0, arrangeSize.Width / (double) this._columns, arrangeSize.Height / (double) this._rows);
      double width = rect.Width;
      double num = arrangeSize.Width - 1.0;
      rect.X += rect.Width * (double) this.FirstColumn;
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        child.Arrange((Rect) rect);
        if (child.Visibility != 1)
        {
          rect.X += width;
          if (rect.X >= num)
          {
            rect.Y += rect.Height;
            rect.X = 0.0;
          }
        }
      }
      return arrangeSize;
    }

    protected virtual Size MeasureOverride(Size constraint)
    {
      this.UpdateComputedValues();
      Size size = new Size(constraint.Width / (double) this._columns, constraint.Height / (double) this._rows);
      double num1 = 0.0;
      double num2 = 0.0;
      int index = 0;
      for (int count = ((ICollection<UIElement>) this.Children).Count; index < count; ++index)
      {
        UIElement child = ((IList<UIElement>) this.Children)[index];
        child.Measure((Size) size);
        Size desiredSize = (Size) child.DesiredSize;
        if (num1 < desiredSize.Width)
          num1 = desiredSize.Width;
        if (num2 < desiredSize.Height)
          num2 = desiredSize.Height;
      }
      return new Size(num1 * (double) this._columns, num2 * (double) this._rows);
    }

    private void UpdateComputedValues()
    {
      this._columns = this.Columns;
      this._rows = this.Rows;
      if (this.FirstColumn >= this._columns)
        this.FirstColumn = 0;
      if (this._rows != 0 && this._columns != 0)
        return;
      int d = 0;
      int index = 0;
      for (int count = ((ICollection<UIElement>) this.Children).Count; index < count; ++index)
      {
        if (((IList<UIElement>) this.Children)[index].Visibility != 1)
          ++d;
      }
      if (d == 0)
        d = 1;
      if (this._rows == 0)
      {
        if (this._columns > 0)
        {
          this._rows = (d + this.FirstColumn + (this._columns - 1)) / this._columns;
        }
        else
        {
          this._rows = (int) Math.Sqrt((double) d);
          if (this._rows * this._rows < d)
            ++this._rows;
          this._columns = this._rows;
        }
      }
      else
      {
        if (this._columns != 0)
          return;
        this._columns = (d + (this._rows - 1)) / this._rows;
      }
    }
  }
}
