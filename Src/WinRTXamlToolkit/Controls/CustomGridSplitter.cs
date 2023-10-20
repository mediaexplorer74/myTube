// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.CustomGridSplitter
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(GroupName = "OrientationStates", Name = "VerticalOrientation")]
  [TemplateVisualState(GroupName = "OrientationStates", Name = "HorizontalOrientation")]
  [StyleTypedProperty(Property = "PreviewStyle", StyleTargetType = typeof (GridSplitterPreviewControl))]
  public class CustomGridSplitter : Control
  {
    private const string OrientationStatesGroupName = "OrientationStates";
    private const string VerticalOrientationStateName = "VerticalOrientation";
    private const string HorizontalOrientationStateName = "HorizontalOrientation";
    private const double DefaultKeyboardIncrement = 1.0;
    private Point _lastPosition;
    private Point _previewDraggingStartPosition;
    private bool _isDragging;
    private bool _isDraggingPreview;
    private GridResizeDirection _effectiveResizeDirection;
    private Grid _parentGrid;
    private Grid _previewPopupHostGrid;
    private Popup _previewPopup;
    private Grid _previewGrid;
    private CustomGridSplitter _previewGridSplitter;
    private Border _previewControlBorder;
    private GridSplitterPreviewControl _previewControl;
    public static readonly DependencyProperty ResizeBehaviorProperty = DependencyProperty.Register(nameof (ResizeBehavior), (Type) typeof (GridResizeBehavior), (Type) typeof (CustomGridSplitter), new PropertyMetadata((object) GridResizeBehavior.BasedOnAlignment));
    public static readonly DependencyProperty ResizeDirectionProperty = DependencyProperty.Register(nameof (ResizeDirection), (Type) typeof (GridResizeDirection), (Type) typeof (CustomGridSplitter), new PropertyMetadata((object) GridResizeDirection.Auto, new PropertyChangedCallback(CustomGridSplitter.OnResizeDirectionChanged)));
    public static readonly DependencyProperty KeyboardIncrementProperty = DependencyProperty.Register(nameof (KeyboardIncrement), (Type) typeof (double), (Type) typeof (CustomGridSplitter), new PropertyMetadata((object) 1.0));
    public static readonly DependencyProperty ShowsPreviewProperty = DependencyProperty.Register(nameof (ShowsPreview), (Type) typeof (bool), (Type) typeof (CustomGridSplitter), new PropertyMetadata((object) false));
    public static readonly DependencyProperty PreviewStyleProperty = DependencyProperty.Register(nameof (PreviewStyle), (Type) typeof (Style), (Type) typeof (CustomGridSplitter), new PropertyMetadata((object) null));
    private uint? _dragPointer;

    public event EventHandler DraggingCompleted;

    public GridResizeBehavior ResizeBehavior
    {
      get => (GridResizeBehavior) ((DependencyObject) this).GetValue(CustomGridSplitter.ResizeBehaviorProperty);
      set => ((DependencyObject) this).SetValue(CustomGridSplitter.ResizeBehaviorProperty, (object) value);
    }

    public GridResizeDirection ResizeDirection
    {
      get => (GridResizeDirection) ((DependencyObject) this).GetValue(CustomGridSplitter.ResizeDirectionProperty);
      set => ((DependencyObject) this).SetValue(CustomGridSplitter.ResizeDirectionProperty, (object) value);
    }

    private static void OnResizeDirectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CustomGridSplitter customGridSplitter = (CustomGridSplitter) d;
      GridResizeDirection oldValue = (GridResizeDirection) e.OldValue;
      GridResizeDirection resizeDirection = customGridSplitter.ResizeDirection;
      customGridSplitter.OnResizeDirectionChanged(oldValue, resizeDirection);
    }

    protected virtual void OnResizeDirectionChanged(
      GridResizeDirection oldResizeDirection,
      GridResizeDirection newResizeDirection)
    {
      this.DetermineResizeCursor();
      this.UpdateVisualState();
    }

    public double KeyboardIncrement
    {
      get => (double) ((DependencyObject) this).GetValue(CustomGridSplitter.KeyboardIncrementProperty);
      set => ((DependencyObject) this).SetValue(CustomGridSplitter.KeyboardIncrementProperty, (object) value);
    }

    public bool ShowsPreview
    {
      get => (bool) ((DependencyObject) this).GetValue(CustomGridSplitter.ShowsPreviewProperty);
      set => ((DependencyObject) this).SetValue(CustomGridSplitter.ShowsPreviewProperty, (object) value);
    }

    public Style PreviewStyle
    {
      get => (Style) ((DependencyObject) this).GetValue(CustomGridSplitter.PreviewStyleProperty);
      set => ((DependencyObject) this).SetValue(CustomGridSplitter.PreviewStyleProperty, (object) value);
    }

    private GridResizeDirection DetermineEffectiveResizeDirection() => this.ResizeDirection == GridResizeDirection.Columns || this.ResizeDirection != GridResizeDirection.Rows && (((FrameworkElement) this).HorizontalAlignment != 3 || (((FrameworkElement) this).HorizontalAlignment != 3 || ((FrameworkElement) this).VerticalAlignment == 3) && ((FrameworkElement) this).HorizontalAlignment == 3 && ((FrameworkElement) this).VerticalAlignment == 3 && ((FrameworkElement) this).ActualWidth <= ((FrameworkElement) this).ActualHeight) ? GridResizeDirection.Columns : GridResizeDirection.Rows;

    private GridResizeBehavior DetermineEffectiveResizeBehavior()
    {
      if (this.ResizeBehavior == GridResizeBehavior.CurrentAndNext)
        return GridResizeBehavior.CurrentAndNext;
      if (this.ResizeBehavior == GridResizeBehavior.PreviousAndCurrent)
        return GridResizeBehavior.PreviousAndCurrent;
      if (this.ResizeBehavior == GridResizeBehavior.PreviousAndNext)
        return GridResizeBehavior.PreviousAndNext;
      if (this.DetermineEffectiveResizeDirection() == GridResizeDirection.Rows)
      {
        if (((FrameworkElement) this).VerticalAlignment == null)
          return GridResizeBehavior.PreviousAndCurrent;
        return ((FrameworkElement) this).VerticalAlignment == 2 ? GridResizeBehavior.CurrentAndNext : GridResizeBehavior.PreviousAndNext;
      }
      if (((FrameworkElement) this).HorizontalAlignment == null)
        return GridResizeBehavior.PreviousAndCurrent;
      return ((FrameworkElement) this).HorizontalAlignment == 2 ? GridResizeBehavior.CurrentAndNext : GridResizeBehavior.PreviousAndNext;
    }

    private void DetermineResizeCursor()
    {
      int effectiveResizeDirection = (int) this.DetermineEffectiveResizeDirection();
    }

    public CustomGridSplitter()
    {
      this.put_DefaultStyleKey((object) typeof (CustomGridSplitter));
      this.put_IsTabStop(true);
      this.DetermineResizeCursor();
      CustomGridSplitter customGridSplitter = this;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(((FrameworkElement) customGridSplitter).add_LayoutUpdated), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) customGridSplitter).remove_LayoutUpdated), new EventHandler<object>(this.OnLayoutUpdated));
      this.UpdateVisualState();
    }

    private void OnLayoutUpdated(object sender, object e) => this.UpdateVisualState();

    private void UpdateVisualState()
    {
      if (this.DetermineEffectiveResizeDirection() == GridResizeDirection.Columns)
        VisualStateManager.GoToState((Control) this, "VerticalOrientation", true);
      else
        VisualStateManager.GoToState((Control) this, "HorizontalOrientation", true);
    }

    protected virtual void OnPointerEntered(PointerRoutedEventArgs e) => this.DetermineResizeCursor();

    protected virtual void OnPointerPressed(PointerRoutedEventArgs e)
    {
      if (this._dragPointer.HasValue)
        return;
      this._dragPointer = new uint?(e.Pointer.PointerId);
      this._effectiveResizeDirection = this.DetermineEffectiveResizeDirection();
      this._parentGrid = this.GetGrid();
      this._previewDraggingStartPosition = (Point) e.GetCurrentPoint((UIElement) this._parentGrid).Position;
      this._lastPosition = this._previewDraggingStartPosition;
      this._isDragging = true;
      if (this.ShowsPreview)
        this.StartPreviewDragging(e);
      else
        this.StartDirectDragging(e);
    }

    private void StartPreviewDragging(PointerRoutedEventArgs e)
    {
      this._isDraggingPreview = true;
      Popup popup = new Popup();
      ((FrameworkElement) popup).put_Width(((FrameworkElement) this._parentGrid).ActualWidth);
      ((FrameworkElement) popup).put_Height(((FrameworkElement) this._parentGrid).ActualHeight);
      this._previewPopup = popup;
      this._previewPopup.put_IsOpen(true);
      Grid grid1 = new Grid();
      ((FrameworkElement) grid1).put_VerticalAlignment((VerticalAlignment) 3);
      ((FrameworkElement) grid1).put_HorizontalAlignment((HorizontalAlignment) 3);
      this._previewPopupHostGrid = grid1;
      ((ICollection<UIElement>) ((Panel) this._parentGrid).Children).Add((UIElement) this._previewPopupHostGrid);
      if (((ICollection<RowDefinition>) this._parentGrid.RowDefinitions).Count > 0)
        Grid.SetRowSpan((FrameworkElement) this._previewPopupHostGrid, ((ICollection<RowDefinition>) this._parentGrid.RowDefinitions).Count);
      if (((ICollection<ColumnDefinition>) this._parentGrid.ColumnDefinitions).Count > 0)
        Grid.SetColumnSpan((FrameworkElement) this._previewPopupHostGrid, ((ICollection<ColumnDefinition>) this._parentGrid.ColumnDefinitions).Count);
      ((ICollection<UIElement>) ((Panel) this._previewPopupHostGrid).Children).Add((UIElement) this._previewPopup);
      Grid grid2 = new Grid();
      ((FrameworkElement) grid2).put_Width(((FrameworkElement) this._parentGrid).ActualWidth);
      ((FrameworkElement) grid2).put_Height(((FrameworkElement) this._parentGrid).ActualHeight);
      this._previewGrid = grid2;
      this._previewPopup.put_Child((UIElement) this._previewGrid);
      foreach (RowDefinition rowDefinition1 in (IEnumerable<RowDefinition>) this._parentGrid.RowDefinitions)
      {
        RowDefinition rowDefinition2 = new RowDefinition();
        rowDefinition2.put_Height(rowDefinition1.Height);
        rowDefinition2.put_MaxHeight(rowDefinition1.MaxHeight);
        rowDefinition2.put_MinHeight(rowDefinition1.MinHeight);
        ((ICollection<RowDefinition>) this._previewGrid.RowDefinitions).Add(rowDefinition2);
      }
      foreach (ColumnDefinition columnDefinition1 in (IEnumerable<ColumnDefinition>) this._parentGrid.ColumnDefinitions)
      {
        GridLength width = columnDefinition1.Width;
        double maxWidth = columnDefinition1.MaxWidth;
        double minWidth = columnDefinition1.MinWidth;
        ColumnDefinition columnDefinition2 = new ColumnDefinition();
        columnDefinition2.put_Width(width);
        columnDefinition1.put_MinWidth(minWidth);
        if (!double.IsInfinity(columnDefinition1.MaxWidth))
          columnDefinition1.put_MaxWidth(maxWidth);
        ((ICollection<ColumnDefinition>) this._previewGrid.ColumnDefinitions).Add(columnDefinition2);
      }
      CustomGridSplitter customGridSplitter = new CustomGridSplitter();
      ((UIElement) customGridSplitter).put_Opacity(0.0);
      customGridSplitter.ShowsPreview = false;
      ((FrameworkElement) customGridSplitter).put_Width(((FrameworkElement) this).Width);
      ((FrameworkElement) customGridSplitter).put_Height(((FrameworkElement) this).Height);
      ((FrameworkElement) customGridSplitter).put_Margin(((FrameworkElement) this).Margin);
      ((FrameworkElement) customGridSplitter).put_VerticalAlignment(((FrameworkElement) this).VerticalAlignment);
      ((FrameworkElement) customGridSplitter).put_HorizontalAlignment(((FrameworkElement) this).HorizontalAlignment);
      customGridSplitter.ResizeBehavior = this.ResizeBehavior;
      customGridSplitter.ResizeDirection = this.ResizeDirection;
      customGridSplitter.KeyboardIncrement = this.KeyboardIncrement;
      this._previewGridSplitter = customGridSplitter;
      Grid.SetColumn((FrameworkElement) this._previewGridSplitter, Grid.GetColumn((FrameworkElement) this));
      int columnSpan = Grid.GetColumnSpan((FrameworkElement) this);
      if (columnSpan > 0)
        Grid.SetColumnSpan((FrameworkElement) this._previewGridSplitter, columnSpan);
      Grid.SetRow((FrameworkElement) this._previewGridSplitter, Grid.GetRow((FrameworkElement) this));
      int rowSpan = Grid.GetRowSpan((FrameworkElement) this);
      if (rowSpan > 0)
        Grid.SetRowSpan((FrameworkElement) this._previewGridSplitter, rowSpan);
      ((ICollection<UIElement>) ((Panel) this._previewGrid).Children).Add((UIElement) this._previewGridSplitter);
      Border border = new Border();
      ((FrameworkElement) border).put_Width(((FrameworkElement) this).Width);
      ((FrameworkElement) border).put_Height(((FrameworkElement) this).Height);
      ((FrameworkElement) border).put_Margin(((FrameworkElement) this).Margin);
      ((FrameworkElement) border).put_VerticalAlignment(((FrameworkElement) this).VerticalAlignment);
      ((FrameworkElement) border).put_HorizontalAlignment(((FrameworkElement) this).HorizontalAlignment);
      this._previewControlBorder = border;
      Grid.SetColumn((FrameworkElement) this._previewControlBorder, Grid.GetColumn((FrameworkElement) this));
      if (columnSpan > 0)
        Grid.SetColumnSpan((FrameworkElement) this._previewControlBorder, columnSpan);
      Grid.SetRow((FrameworkElement) this._previewControlBorder, Grid.GetRow((FrameworkElement) this));
      if (rowSpan > 0)
        Grid.SetRowSpan((FrameworkElement) this._previewControlBorder, rowSpan);
      ((ICollection<UIElement>) ((Panel) this._previewGrid).Children).Add((UIElement) this._previewControlBorder);
      this._previewControl = new GridSplitterPreviewControl();
      if (this.PreviewStyle != null)
        ((FrameworkElement) this._previewControl).put_Style(this.PreviewStyle);
      this._previewControlBorder.put_Child((UIElement) this._previewControl);
      this._previewPopup.put_Child((UIElement) this._previewGrid);
      this._previewGridSplitter._dragPointer = new uint?(e.Pointer.PointerId);
      this._previewGridSplitter._effectiveResizeDirection = this.DetermineEffectiveResizeDirection();
      this._previewGridSplitter._parentGrid = this._previewGrid;
      this._previewGridSplitter._lastPosition = (Point) e.GetCurrentPoint((UIElement) this._previewGrid).Position;
      this._previewGridSplitter._isDragging = true;
      this._previewGridSplitter.StartDirectDragging(e);
      this._previewGridSplitter.DraggingCompleted += new EventHandler(this.PreviewGridSplitter_DraggingCompleted);
    }

    private void PreviewGridSplitter_DraggingCompleted(object sender, EventArgs e)
    {
      for (int index = 0; index < ((ICollection<RowDefinition>) this._previewGrid.RowDefinitions).Count; ++index)
        ((IList<RowDefinition>) this._parentGrid.RowDefinitions)[index].put_Height(((IList<RowDefinition>) this._previewGrid.RowDefinitions)[index].Height);
      for (int index = 0; index < ((ICollection<ColumnDefinition>) this._previewGrid.ColumnDefinitions).Count; ++index)
        ((IList<ColumnDefinition>) this._parentGrid.ColumnDefinitions)[index].put_Width(((IList<ColumnDefinition>) this._previewGrid.ColumnDefinitions)[index].Width);
      this._previewGridSplitter.DraggingCompleted -= new EventHandler(this.PreviewGridSplitter_DraggingCompleted);
      ((ICollection<UIElement>) ((Panel) this._parentGrid).Children).Remove((UIElement) this._previewPopupHostGrid);
      this._isDragging = false;
      this._isDraggingPreview = false;
      this._dragPointer = new uint?();
      this._parentGrid = (Grid) null;
      if (this.DraggingCompleted == null)
        return;
      this.DraggingCompleted((object) this, EventArgs.Empty);
    }

    private void StartDirectDragging(PointerRoutedEventArgs e)
    {
      this._isDraggingPreview = false;
      ((UIElement) this).CapturePointer(e.Pointer);
    }

    protected virtual void OnPointerMoved(PointerRoutedEventArgs e)
    {
      if (!this._isDragging)
        return;
      uint? dragPointer = this._dragPointer;
      uint pointerId = e.Pointer.PointerId;
      if (((int) dragPointer.GetValueOrDefault() != (int) pointerId ? 1 : (!dragPointer.HasValue ? 1 : 0)) != 0)
        return;
      Point position = (Point) e.GetCurrentPoint((UIElement) this._parentGrid).Position;
      if (this._isDraggingPreview)
        this.ContinuePreviewDragging(position);
      else
        this.ContinueDirectDragging(position);
      this._lastPosition = position;
    }

    private void ContinuePreviewDragging(Point position)
    {
    }

    private void ContinueDirectDragging(Point position)
    {
      if (this._effectiveResizeDirection == GridResizeDirection.Columns)
        this.ResizeColumns(this._parentGrid, position.X - this._lastPosition.X);
      else
        this.ResizeRows(this._parentGrid, position.Y - this._lastPosition.Y);
    }

    protected virtual void OnPointerReleased(PointerRoutedEventArgs e)
    {
      if (!this._isDragging)
        return;
      uint? dragPointer = this._dragPointer;
      uint pointerId = e.Pointer.PointerId;
      if (((int) dragPointer.GetValueOrDefault() != (int) pointerId ? 1 : (!dragPointer.HasValue ? 1 : 0)) != 0)
        return;
      ((UIElement) this).ReleasePointerCapture(e.Pointer);
      this._isDragging = false;
      this._isDraggingPreview = false;
      this._dragPointer = new uint?();
      this._parentGrid = (Grid) null;
      if (this.DraggingCompleted == null)
        return;
      this.DraggingCompleted((object) this, EventArgs.Empty);
    }

    protected virtual void OnKeyDown(KeyRoutedEventArgs e)
    {
      base.OnKeyDown(e);
      if (this.DetermineEffectiveResizeDirection() == GridResizeDirection.Columns)
      {
        if (e.Key == 37)
        {
          this.ResizeColumns(this.GetGrid(), -this.KeyboardIncrement);
          e.put_Handled(true);
        }
        else
        {
          if (e.Key != 39)
            return;
          this.ResizeColumns(this.GetGrid(), this.KeyboardIncrement);
          e.put_Handled(true);
        }
      }
      else if (e.Key == 38)
      {
        this.ResizeRows(this.GetGrid(), -this.KeyboardIncrement);
        e.put_Handled(true);
      }
      else
      {
        if (e.Key != 40)
          return;
        this.ResizeRows(this.GetGrid(), this.KeyboardIncrement);
        e.put_Handled(true);
      }
    }

    private void ResizeColumns(Grid grid, double deltaX)
    {
      GridResizeBehavior effectiveResizeBehavior = this.DetermineEffectiveResizeBehavior();
      int column = Grid.GetColumn((FrameworkElement) this);
      int index1;
      int index2;
      switch (effectiveResizeBehavior)
      {
        case GridResizeBehavior.PreviousAndCurrent:
          index1 = column - 1;
          index2 = column;
          break;
        case GridResizeBehavior.PreviousAndNext:
          index1 = column - 1;
          index2 = column + 1;
          break;
        default:
          index1 = column;
          index2 = column + 1;
          break;
      }
      if (index2 >= ((ICollection<ColumnDefinition>) grid.ColumnDefinitions).Count)
        return;
      ColumnDefinition columnDefinition1 = ((IList<ColumnDefinition>) grid.ColumnDefinitions)[index1];
      ColumnDefinition columnDefinition2 = ((IList<ColumnDefinition>) grid.ColumnDefinitions)[index2];
      GridUnitType gridUnitType1 = columnDefinition1.Width.GridUnitType;
      GridUnitType gridUnitType2 = columnDefinition2.Width.GridUnitType;
      double actualWidth1 = columnDefinition1.ActualWidth;
      double actualWidth2 = columnDefinition2.ActualWidth;
      double maxWidth1 = columnDefinition1.MaxWidth;
      double maxWidth2 = columnDefinition2.MaxWidth;
      double minWidth1 = columnDefinition1.MinWidth;
      double minWidth2 = columnDefinition2.MinWidth;
      if (actualWidth1 + deltaX > maxWidth1)
        deltaX = Math.Max(0.0, columnDefinition1.MaxWidth - actualWidth1);
      if (actualWidth1 + deltaX < minWidth1)
        deltaX = Math.Min(0.0, columnDefinition1.MinWidth - actualWidth1);
      if (actualWidth2 - deltaX > maxWidth2)
        deltaX = -Math.Max(0.0, columnDefinition2.MaxWidth - actualWidth2);
      if (actualWidth2 - deltaX < minWidth2)
        deltaX = -Math.Min(0.0, columnDefinition2.MinWidth - actualWidth2);
      double num1 = actualWidth1 + deltaX;
      double num2 = actualWidth2 - deltaX;
      double num3 = 0.0;
      double actualWidth3 = ((FrameworkElement) grid).ActualWidth;
      if (gridUnitType1 == GridUnitType.Star || gridUnitType2 == GridUnitType.Star)
      {
        foreach (ColumnDefinition columnDefinition3 in (IEnumerable<ColumnDefinition>) grid.ColumnDefinitions)
        {
          if (columnDefinition3.Width.GridUnitType == GridUnitType.Star)
            num3 += columnDefinition3.Width.Value;
          else
            actualWidth3 -= columnDefinition3.ActualWidth;
        }
      }
      if (gridUnitType1 == GridUnitType.Star)
      {
        if (gridUnitType2 == GridUnitType.Star)
        {
          if (actualWidth3 < 1.0)
            return;
          double num4 = columnDefinition1.Width.Value;
          double num5 = Math.Max(0.0, num3 * num1 / actualWidth3);
          columnDefinition1.put_Width(new GridLength(num5, GridUnitType.Star));
          columnDefinition2.put_Width(new GridLength(Math.Max(0.0, columnDefinition2.Width.Value - num5 + num4), GridUnitType.Star));
        }
        else
        {
          double num6 = actualWidth3 + actualWidth2 - num2;
          if (num6 - num1 >= 1.0)
          {
            double num7 = Math.Max(0.0, (num3 - columnDefinition1.Width.Value) * num1 / (num6 - num1));
            columnDefinition1.put_Width(new GridLength(num7, GridUnitType.Star));
          }
        }
      }
      else
        columnDefinition1.put_Width(new GridLength(num1, GridUnitType.Pixel));
      if (gridUnitType2 == GridUnitType.Star)
      {
        if (gridUnitType1 == GridUnitType.Star)
          return;
        double num8 = actualWidth3 + actualWidth1 - num1;
        if (num8 - num2 < 1.0)
          return;
        double num9 = Math.Max(0.0, (num3 - columnDefinition2.Width.Value) * num2 / (num8 - num2));
        columnDefinition2.put_Width(new GridLength(num9, GridUnitType.Star));
      }
      else
        columnDefinition2.put_Width(new GridLength(num2, GridUnitType.Pixel));
    }

    private void ResizeRows(Grid grid, double deltaX)
    {
      GridResizeBehavior effectiveResizeBehavior = this.DetermineEffectiveResizeBehavior();
      int row = Grid.GetRow((FrameworkElement) this);
      int index1;
      int index2;
      switch (effectiveResizeBehavior)
      {
        case GridResizeBehavior.PreviousAndCurrent:
          index1 = row - 1;
          index2 = row;
          break;
        case GridResizeBehavior.PreviousAndNext:
          index1 = row - 1;
          index2 = row + 1;
          break;
        default:
          index1 = row;
          index2 = row + 1;
          break;
      }
      if (index2 >= ((ICollection<RowDefinition>) grid.RowDefinitions).Count)
        return;
      RowDefinition rowDefinition1 = ((IList<RowDefinition>) grid.RowDefinitions)[index1];
      RowDefinition rowDefinition2 = ((IList<RowDefinition>) grid.RowDefinitions)[index2];
      GridUnitType gridUnitType1 = rowDefinition1.Height.GridUnitType;
      GridUnitType gridUnitType2 = rowDefinition2.Height.GridUnitType;
      double actualHeight1 = rowDefinition1.ActualHeight;
      double actualHeight2 = rowDefinition2.ActualHeight;
      double maxHeight1 = rowDefinition1.MaxHeight;
      double maxHeight2 = rowDefinition2.MaxHeight;
      double minHeight1 = rowDefinition1.MinHeight;
      double minHeight2 = rowDefinition2.MinHeight;
      if (actualHeight1 + deltaX > maxHeight1)
        deltaX = Math.Max(0.0, rowDefinition1.MaxHeight - actualHeight1);
      if (actualHeight1 + deltaX < minHeight1)
        deltaX = Math.Min(0.0, rowDefinition1.MinHeight - actualHeight1);
      if (actualHeight2 - deltaX > maxHeight2)
        deltaX = -Math.Max(0.0, rowDefinition2.MaxHeight - actualHeight2);
      if (actualHeight2 - deltaX < minHeight2)
        deltaX = -Math.Min(0.0, rowDefinition2.MinHeight - actualHeight2);
      double num1 = actualHeight1 + deltaX;
      double num2 = actualHeight2 - deltaX;
      double num3 = 0.0;
      double actualHeight3 = ((FrameworkElement) grid).ActualHeight;
      if (gridUnitType1 == GridUnitType.Star || gridUnitType2 == GridUnitType.Star)
      {
        foreach (RowDefinition rowDefinition3 in (IEnumerable<RowDefinition>) grid.RowDefinitions)
        {
          if (rowDefinition3.Height.GridUnitType == GridUnitType.Star)
            num3 += rowDefinition3.Height.Value;
          else
            actualHeight3 -= rowDefinition3.ActualHeight;
        }
      }
      if (gridUnitType1 == GridUnitType.Star)
      {
        if (gridUnitType2 == GridUnitType.Star)
        {
          if (actualHeight3 < 1.0)
            return;
          double num4 = rowDefinition1.Height.Value;
          double num5 = Math.Max(0.0, num3 * num1 / actualHeight3);
          rowDefinition1.put_Height(new GridLength(num5, GridUnitType.Star));
          rowDefinition2.put_Height(new GridLength(Math.Max(0.0, rowDefinition2.Height.Value - num5 + num4), GridUnitType.Star));
        }
        else
        {
          double num6 = actualHeight3 + actualHeight2 - num2;
          if (num6 - num1 >= 1.0)
          {
            double num7 = Math.Max(0.0, (num3 - rowDefinition1.Height.Value) * num1 / (num6 - num1));
            rowDefinition1.put_Height(new GridLength(num7, GridUnitType.Star));
          }
        }
      }
      else
        rowDefinition1.put_Height(new GridLength(num1, GridUnitType.Pixel));
      if (gridUnitType2 == GridUnitType.Star)
      {
        if (gridUnitType1 == GridUnitType.Star)
          return;
        double num8 = actualHeight3 + actualHeight1 - num1;
        if (num8 - num2 < 1.0)
          return;
        double num9 = Math.Max(0.0, (num3 - rowDefinition2.Height.Value) * num2 / (num8 - num2));
        rowDefinition2.put_Height(new GridLength(num9, GridUnitType.Star));
      }
      else
        rowDefinition2.put_Height(new GridLength(num2, GridUnitType.Pixel));
    }

    private Grid GetGrid() => ((FrameworkElement) this).Parent is Grid parent ? parent : throw new InvalidOperationException("CustomGridSplitter only works when hosted in a Grid.");
  }
}
