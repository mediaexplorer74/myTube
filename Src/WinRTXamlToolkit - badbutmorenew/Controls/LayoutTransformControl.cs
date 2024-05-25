// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.LayoutTransformControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.Common;

namespace WinRTXamlToolkit.Controls
{
  [ContentProperty(Name = "Child")]
  public class LayoutTransformControl : Control
  {
    private const double AcceptableDelta = 0.0001;
    private const int DecimalsAfterRound = 4;
    public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(nameof (Child), (Type) typeof (FrameworkElement), (Type) typeof (LayoutTransformControl), new PropertyMetadata((object) null, new PropertyChangedCallback(LayoutTransformControl.ChildChanged)));
    public static readonly DependencyProperty TransformProperty = DependencyProperty.Register(nameof (Transform), (Type) typeof (Transform), (Type) typeof (LayoutTransformControl), new PropertyMetadata((object) null, new PropertyChangedCallback(LayoutTransformControl.TransformChanged)));
    private Panel _layoutRoot;
    private MatrixTransform _matrixTransform;
    private Matrix _transformation;
    private Size _childActualSize = Size.Empty;
    private readonly Dictionary<Transform, List<PropertyChangeEventSource<double>>> _transformPropertyChangeEventSources = new Dictionary<Transform, List<PropertyChangeEventSource<double>>>();

    public FrameworkElement Child
    {
      get => (FrameworkElement) ((DependencyObject) this).GetValue(LayoutTransformControl.ChildProperty);
      set => ((DependencyObject) this).SetValue(LayoutTransformControl.ChildProperty, (object) value);
    }

    private static void ChildChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((LayoutTransformControl) o).OnChildChanged((FrameworkElement) e.NewValue);

    private void OnChildChanged(FrameworkElement newContent)
    {
      if (this._layoutRoot == null)
        return;
      ((ICollection<UIElement>) this._layoutRoot.Children).Clear();
      if (newContent != null)
        ((ICollection<UIElement>) this._layoutRoot.Children).Add((UIElement) newContent);
      ((UIElement) this).InvalidateMeasure();
    }

    public Transform Transform
    {
      get => (Transform) ((DependencyObject) this).GetValue(LayoutTransformControl.TransformProperty);
      set => ((DependencyObject) this).SetValue(LayoutTransformControl.TransformProperty, (object) value);
    }

    private static void TransformChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((LayoutTransformControl) o).OnTransformChanged(e.OldValue as Transform, (Transform) e.NewValue);

    private void OnTransformChanged(Transform oldValue, Transform newValue)
    {
      if (oldValue != null)
        this.UnsubscribeFromTransformPropertyChanges(oldValue);
      if (newValue != null)
        this.SubscribeToTransformPropertyChanges(newValue);
      this.ProcessTransform();
    }

    public LayoutTransformControl()
    {
      this.put_IsTabStop(false);
      ((UIElement) this).put_UseLayoutRounding(false);
      this.put_Template((ControlTemplate) XamlReader.Load("<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'><Grid x:Name='LayoutRoot' Background='{TemplateBinding Background}'><Grid.RenderTransform><MatrixTransform x:Name='MatrixTransform'/></Grid.RenderTransform></Grid></ControlTemplate>"));
    }

    protected virtual void OnApplyTemplate()
    {
      FrameworkElement child = this.Child;
      this.Child = (FrameworkElement) null;
      ((FrameworkElement) this).OnApplyTemplate();
      this._layoutRoot = this.GetTemplateChild("LayoutRoot") as Panel;
      this._matrixTransform = this.GetTemplateChild("MatrixTransform") as MatrixTransform;
      this.Child = child;
      this.TransformUpdated();
    }

    private void UnsubscribeFromTransformPropertyChanges(Transform transform)
    {
      using (List<PropertyChangeEventSource<double>>.Enumerator enumerator = this._transformPropertyChangeEventSources[transform].GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.ValueChanged -= new EventHandler<double>(this.OnTransformPropertyChanged);
      }
      this._transformPropertyChangeEventSources.Remove(transform);
    }

    private void SubscribeToTransformPropertyChanges(Transform transform)
    {
      if (transform is TransformGroup transformGroup)
      {
        foreach (Transform child in (IEnumerable<Transform>) transformGroup.Children)
          this.SubscribeToTransformPropertyChanges(child);
      }
      else
      {
        List<PropertyChangeEventSource<double>> changeEventSourceList = new List<PropertyChangeEventSource<double>>();
        this._transformPropertyChangeEventSources.Add(transform, changeEventSourceList);
        if (transform is RotateTransform source3)
        {
          PropertyChangeEventSource<double> changeEventSource = new PropertyChangeEventSource<double>((DependencyObject) source3, "Angle");
          changeEventSource.ValueChanged += new EventHandler<double>(this.OnTransformPropertyChanged);
          changeEventSourceList.Add(changeEventSource);
        }
        else if (transform is ScaleTransform source2)
        {
          PropertyChangeEventSource<double> changeEventSource1 = new PropertyChangeEventSource<double>((DependencyObject) source2, "ScaleX");
          changeEventSource1.ValueChanged += new EventHandler<double>(this.OnTransformPropertyChanged);
          changeEventSourceList.Add(changeEventSource1);
          PropertyChangeEventSource<double> changeEventSource2 = new PropertyChangeEventSource<double>((DependencyObject) source2, "ScaleY");
          changeEventSource2.ValueChanged += new EventHandler<double>(this.OnTransformPropertyChanged);
          changeEventSourceList.Add(changeEventSource2);
        }
        else if (transform is SkewTransform source1)
        {
          PropertyChangeEventSource<double> changeEventSource3 = new PropertyChangeEventSource<double>((DependencyObject) source1, "AngleX");
          changeEventSource3.ValueChanged += new EventHandler<double>(this.OnTransformPropertyChanged);
          changeEventSourceList.Add(changeEventSource3);
          PropertyChangeEventSource<double> changeEventSource4 = new PropertyChangeEventSource<double>((DependencyObject) source1, "AngleY");
          changeEventSource4.ValueChanged += new EventHandler<double>(this.OnTransformPropertyChanged);
          changeEventSourceList.Add(changeEventSource4);
        }
        else
        {
          if (!(transform is MatrixTransform source))
            return;
          PropertyChangeEventSource<double> changeEventSource = new PropertyChangeEventSource<double>((DependencyObject) source, "Matrix");
          changeEventSource.ValueChanged += new EventHandler<double>(this.OnTransformPropertyChanged);
          changeEventSourceList.Add(changeEventSource);
        }
      }
    }

    private void OnTransformPropertyChanged(object sender, double e) => this.TransformUpdated();

    public void TransformUpdated() => this.ProcessTransform();

    private void ProcessTransform()
    {
      this._transformation = LayoutTransformControl.RoundMatrix(this.GetTransformMatrix(this.Transform), 4);
      if (this._matrixTransform != null)
        this._matrixTransform.put_Matrix(this._transformation);
      ((UIElement) this).InvalidateMeasure();
    }

    private Matrix GetTransformMatrix(Transform transform)
    {
      switch (transform)
      {
        case TransformGroup transformGroup:
          Matrix matrix1 = Matrix.Identity;
          foreach (Transform child in (IEnumerable<Transform>) transformGroup.Children)
            matrix1 = LayoutTransformControl.MatrixMultiply(matrix1, this.GetTransformMatrix(child));
          return matrix1;
        case RotateTransform rotateTransform:
          double num1 = 6.2831853071795862 * rotateTransform.Angle / 360.0;
          double m12 = Math.Sin(num1);
          double num2 = Math.Cos(num1);
          return new Matrix(num2, m12, -m12, num2, 0.0, 0.0);
        case ScaleTransform scaleTransform:
          return new Matrix(scaleTransform.ScaleX, 0.0, 0.0, scaleTransform.ScaleY, 0.0, 0.0);
        case SkewTransform skewTransform:
          double angleX = skewTransform.AngleX;
          return new Matrix(1.0, 6.2831853071795862 * skewTransform.AngleY / 360.0, 6.2831853071795862 * angleX / 360.0, 1.0, 0.0, 0.0);
        case MatrixTransform matrixTransform:
          return matrixTransform.Matrix;
        case CompositeTransform _:
          throw new NotSupportedException("CompositeTransforms are not supported (yet) by the LayoutTransformControl.");
        default:
          return Matrix.Identity;
      }
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      FrameworkElement child = this.Child;
      if (this._layoutRoot == null || child == null)
        return Size.Empty;
      ((UIElement) this._layoutRoot).Measure(!(this._childActualSize == Size.Empty) ? (Size) this._childActualSize : (Size) this.ComputeLargestTransformedSize(availableSize));
      Rect rect = LayoutTransformControl.RectTransform(new Rect(0.0, 0.0, ((Size) ((UIElement) this._layoutRoot).DesiredSize).Width, ((Size) ((UIElement) this._layoutRoot).DesiredSize).Height), this._transformation);
      return new Size(rect.Width, rect.Height);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      FrameworkElement child = this.Child;
      if (this._layoutRoot == null || child == null)
        return finalSize;
      Size a = this.ComputeLargestTransformedSize(finalSize);
      if (LayoutTransformControl.IsSizeSmaller(a, (Size) ((UIElement) this._layoutRoot).DesiredSize))
        a = (Size) ((UIElement) this._layoutRoot).DesiredSize;
      Rect rect = LayoutTransformControl.RectTransform(new Rect(0.0, 0.0, a.Width, a.Height), this._transformation);
      ((UIElement) this._layoutRoot).Arrange((Rect) new Rect(-rect.Left + (finalSize.Width - rect.Width) / 2.0, -rect.Top + (finalSize.Height - rect.Height) / 2.0, a.Width, a.Height));
      if (LayoutTransformControl.IsSizeSmaller(a, (Size) ((UIElement) child).RenderSize) && Size.Empty == this._childActualSize)
      {
        this._childActualSize = new Size(child.ActualWidth, child.ActualHeight);
        ((UIElement) this).InvalidateMeasure();
      }
      else
        this._childActualSize = Size.Empty;
      return finalSize;
    }

    private Size ComputeLargestTransformedSize(Size arrangeBounds)
    {
      Size largestTransformedSize = Size.Empty;
      bool flag1 = double.IsInfinity(arrangeBounds.Width);
      if (flag1)
        arrangeBounds.Width = arrangeBounds.Height;
      bool flag2 = double.IsInfinity(arrangeBounds.Height);
      if (flag2)
        arrangeBounds.Height = arrangeBounds.Width;
      double m11 = this._transformation.M11;
      double m12 = this._transformation.M12;
      double m21 = this._transformation.M21;
      double m22 = this._transformation.M22;
      double num1 = Math.Abs(arrangeBounds.Width / m11);
      double num2 = Math.Abs(arrangeBounds.Width / m21);
      double num3 = Math.Abs(arrangeBounds.Height / m12);
      double num4 = Math.Abs(arrangeBounds.Height / m22);
      double num5 = num1 / 2.0;
      double num6 = num2 / 2.0;
      double num7 = num3 / 2.0;
      double num8 = num4 / 2.0;
      double num9 = -(num2 / num1);
      double num10 = -(num4 / num3);
      if (0.0 == arrangeBounds.Width || 0.0 == arrangeBounds.Height)
        largestTransformedSize = new Size(0.0, 0.0);
      else if (flag1 && flag2)
        largestTransformedSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
      else if (!LayoutTransformControl.MatrixHasInverse(this._transformation))
        largestTransformedSize = new Size(0.0, 0.0);
      else if (0.0 == m12 || 0.0 == m21)
      {
        double num11 = flag2 ? double.PositiveInfinity : num4;
        double num12 = flag1 ? double.PositiveInfinity : num1;
        if (0.0 == m12 && 0.0 == m21)
          largestTransformedSize = new Size(num12, num11);
        else if (0.0 == m12)
        {
          double height = Math.Min(num6, num11);
          largestTransformedSize = new Size(num12 - Math.Abs(m21 * height / m11), height);
        }
        else if (0.0 == m21)
        {
          double width = Math.Min(num7, num12);
          largestTransformedSize = new Size(width, num11 - Math.Abs(m12 * width / m22));
        }
      }
      else if (0.0 == m11 || 0.0 == m22)
      {
        double num13 = flag2 ? double.PositiveInfinity : num3;
        double num14 = flag1 ? double.PositiveInfinity : num2;
        if (0.0 == m11 && 0.0 == m22)
          largestTransformedSize = new Size(num13, num14);
        else if (0.0 == m11)
        {
          double height = Math.Min(num8, num14);
          largestTransformedSize = new Size(num13 - Math.Abs(m22 * height / m12), height);
        }
        else if (0.0 == m22)
        {
          double width = Math.Min(num5, num13);
          largestTransformedSize = new Size(width, num14 - Math.Abs(m11 * width / m21));
        }
      }
      else if (num6 <= num10 * num5 + num4)
        largestTransformedSize = new Size(num5, num6);
      else if (num8 <= num9 * num7 + num2)
      {
        largestTransformedSize = new Size(num7, num8);
      }
      else
      {
        double width = (num4 - num2) / (num9 - num10);
        largestTransformedSize = new Size(width, num9 * width + num2);
      }
      return largestTransformedSize;
    }

    private static bool IsSizeSmaller(Size a, Size b) => a.Width + 0.0001 < b.Width || a.Height + 0.0001 < b.Height;

    private static Matrix RoundMatrix(Matrix matrix, int decimalsAfterRound) => new Matrix(Math.Round(matrix.M11, decimalsAfterRound), Math.Round(matrix.M12, decimalsAfterRound), Math.Round(matrix.M21, decimalsAfterRound), Math.Round(matrix.M22, decimalsAfterRound), matrix.OffsetX, matrix.OffsetY);

    private static Rect RectTransform(Rect rectangle, Matrix matrix)
    {
      Point point1 = (Point) matrix.Transform((Point) new Point(rectangle.Left, rectangle.Top));
      Point point2 = (Point) matrix.Transform((Point) new Point(rectangle.Right, rectangle.Top));
      Point point3 = (Point) matrix.Transform((Point) new Point(rectangle.Left, rectangle.Bottom));
      Point point4 = (Point) matrix.Transform((Point) new Point(rectangle.Right, rectangle.Bottom));
      double x = Math.Min(Math.Min(point1.X, point2.X), Math.Min(point3.X, point4.X));
      double y = Math.Min(Math.Min(point1.Y, point2.Y), Math.Min(point3.Y, point4.Y));
      double num1 = Math.Max(Math.Max(point1.X, point2.X), Math.Max(point3.X, point4.X));
      double num2 = Math.Max(Math.Max(point1.Y, point2.Y), Math.Max(point3.Y, point4.Y));
      return new Rect(x, y, num1 - x, num2 - y);
    }

    private static Matrix MatrixMultiply(Matrix matrix1, Matrix matrix2) => new Matrix(matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21, matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22, matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21, matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22, matrix1.OffsetX * matrix2.M11 + matrix1.OffsetY * matrix2.M21 + matrix2.OffsetX, matrix1.OffsetX * matrix2.M12 + matrix1.OffsetY * matrix2.M22 + matrix2.OffsetY);

    private static bool MatrixHasInverse(Matrix matrix) => 0.0 != matrix.M11 * matrix.M22 - matrix.M12 * matrix.M21;
  }
}
