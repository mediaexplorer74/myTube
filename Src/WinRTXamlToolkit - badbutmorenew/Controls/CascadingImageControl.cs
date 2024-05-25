// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.CascadingImageControl
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "PART_LayoutGrid", Type = typeof (Grid))]
  public sealed class CascadingImageControl : Control
  {
    private const string LayoutGridName = "PART_LayoutGrid";
    private Grid _layoutGrid;
    private bool _isLoaded;
    private static readonly Random Random = new Random();
    public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(nameof (Columns), (Type) typeof (int), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) 3, new PropertyChangedCallback(CascadingImageControl.OnColumnsChanged)));
    public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(nameof (Rows), (Type) typeof (int), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) 3, new PropertyChangedCallback(CascadingImageControl.OnRowsChanged)));
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(nameof (ImageSource), (Type) typeof (ImageSource), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) null, new PropertyChangedCallback(CascadingImageControl.OnImageSourceChanged)));
    public static readonly DependencyProperty ColumnDelayProperty = DependencyProperty.Register(nameof (ColumnDelay), (Type) typeof (TimeSpan), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) TimeSpan.FromSeconds(0.025), new PropertyChangedCallback(CascadingImageControl.OnColumnDelayChanged)));
    public static readonly DependencyProperty RowDelayProperty = DependencyProperty.Register(nameof (RowDelay), (Type) typeof (TimeSpan), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) TimeSpan.FromSeconds(0.05), new PropertyChangedCallback(CascadingImageControl.OnRowDelayChanged)));
    public static readonly DependencyProperty TileDurationProperty = DependencyProperty.Register(nameof (TileDuration), (Type) typeof (TimeSpan), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) TimeSpan.FromSeconds(2.0), new PropertyChangedCallback(CascadingImageControl.OnTileDurationChanged)));
    public static readonly DependencyProperty CascadeDirectionProperty = DependencyProperty.Register(nameof (CascadeDirection), (Type) typeof (CascadeDirection), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) CascadeDirection.Shuffle));
    public static readonly DependencyProperty CascadeInEasingFunctionProperty;
    public static readonly DependencyProperty CascadeSequenceProperty;

    public int Columns
    {
      get => (int) ((DependencyObject) this).GetValue(CascadingImageControl.ColumnsProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.ColumnsProperty, (object) value);
    }

    private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CascadingImageControl cascadingImageControl = (CascadingImageControl) d;
      int oldValue = (int) e.OldValue;
      int columns = cascadingImageControl.Columns;
      cascadingImageControl.OnColumnsChanged(oldValue, columns);
    }

    private void OnColumnsChanged(int oldColumns, int newColumns) => this.Cascade();

    public int Rows
    {
      get => (int) ((DependencyObject) this).GetValue(CascadingImageControl.RowsProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.RowsProperty, (object) value);
    }

    private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CascadingImageControl cascadingImageControl = (CascadingImageControl) d;
      int oldValue = (int) e.OldValue;
      int rows = cascadingImageControl.Rows;
      cascadingImageControl.OnRowsChanged(oldValue, rows);
    }

    private void OnRowsChanged(int oldRows, int newRows) => this.Cascade();

    public ImageSource ImageSource
    {
      get => (ImageSource) ((DependencyObject) this).GetValue(CascadingImageControl.ImageSourceProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.ImageSourceProperty, (object) value);
    }

    private static void OnImageSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingImageControl cascadingImageControl = (CascadingImageControl) d;
      ImageSource oldValue = (ImageSource) e.OldValue;
      ImageSource imageSource = cascadingImageControl.ImageSource;
      cascadingImageControl.OnImageSourceChanged(oldValue, imageSource);
    }

    private void OnImageSourceChanged(ImageSource oldImageSource, ImageSource newImageSource) => this.Cascade();

    public TimeSpan ColumnDelay
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingImageControl.ColumnDelayProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.ColumnDelayProperty, (object) value);
    }

    private static void OnColumnDelayChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingImageControl cascadingImageControl = (CascadingImageControl) d;
      TimeSpan oldValue = (TimeSpan) e.OldValue;
      TimeSpan columnDelay = cascadingImageControl.ColumnDelay;
      cascadingImageControl.OnColumnDelayChanged(oldValue, columnDelay);
    }

    private void OnColumnDelayChanged(TimeSpan oldColumnDelay, TimeSpan newColumnDelay) => this.Cascade();

    public TimeSpan RowDelay
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingImageControl.RowDelayProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.RowDelayProperty, (object) value);
    }

    private static void OnRowDelayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      CascadingImageControl cascadingImageControl = (CascadingImageControl) d;
      TimeSpan oldValue = (TimeSpan) e.OldValue;
      TimeSpan rowDelay = cascadingImageControl.RowDelay;
      cascadingImageControl.OnRowDelayChanged(oldValue, rowDelay);
    }

    private void OnRowDelayChanged(TimeSpan oldRowDelay, TimeSpan newRowDelay) => this.Cascade();

    public TimeSpan TileDuration
    {
      get => (TimeSpan) ((DependencyObject) this).GetValue(CascadingImageControl.TileDurationProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.TileDurationProperty, (object) value);
    }

    private static void OnTileDurationChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      CascadingImageControl cascadingImageControl = (CascadingImageControl) d;
      TimeSpan oldValue = (TimeSpan) e.OldValue;
      TimeSpan tileDuration = cascadingImageControl.TileDuration;
      cascadingImageControl.OnTileDurationChanged(oldValue, tileDuration);
    }

    private void OnTileDurationChanged(TimeSpan oldTileDuration, TimeSpan newTileDuration)
    {
    }

    public CascadeDirection CascadeDirection
    {
      get => (CascadeDirection) ((DependencyObject) this).GetValue(CascadingImageControl.CascadeDirectionProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.CascadeDirectionProperty, (object) value);
    }

    public EasingFunctionBase CascadeInEasingFunction
    {
      get => (EasingFunctionBase) ((DependencyObject) this).GetValue(CascadingImageControl.CascadeInEasingFunctionProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.CascadeInEasingFunctionProperty, (object) value);
    }

    public CascadeSequence CascadeSequence
    {
      get => (CascadeSequence) ((DependencyObject) this).GetValue(CascadingImageControl.CascadeSequenceProperty);
      set => ((DependencyObject) this).SetValue(CascadingImageControl.CascadeSequenceProperty, (object) value);
    }

    public CascadingImageControl()
    {
      this.put_DefaultStyleKey((object) typeof (CascadingImageControl));
      CascadingImageControl cascadingImageControl = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) cascadingImageControl).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) cascadingImageControl).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this._layoutGrid = this.GetTemplateChild("PART_LayoutGrid") as Grid;
      Grid layoutGrid = this._layoutGrid;
      this.Cascade();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this._isLoaded = true;
      this.Cascade();
    }

    public void Cascade()
    {
      if (!this._isLoaded || this._layoutGrid == null)
        return;
      if (this.Rows < 1)
        this.Rows = 1;
      if (this.Columns < 1)
        this.Columns = 1;
      ((ICollection<UIElement>) ((Panel) this._layoutGrid).Children).Clear();
      ((ICollection<RowDefinition>) this._layoutGrid.RowDefinitions).Clear();
      ((ICollection<ColumnDefinition>) this._layoutGrid.ColumnDefinitions).Clear();
      for (int index = 0; index < this.Rows; ++index)
        ((ICollection<RowDefinition>) this._layoutGrid.RowDefinitions).Add(new RowDefinition());
      for (int index = 0; index < this.Columns; ++index)
        ((ICollection<ColumnDefinition>) this._layoutGrid.ColumnDefinitions).Add(new ColumnDefinition());
      Storyboard sb = new Storyboard();
      double num1 = this.RowDelay.TotalSeconds * (double) (this.Rows - 1) + this.ColumnDelay.TotalSeconds * (double) (this.Columns - 1) + this.TileDuration.TotalSeconds;
      CascadeDirection cascadeDirection = this.CascadeDirection;
      if (cascadeDirection == CascadeDirection.Random)
        cascadeDirection = (CascadeDirection) CascadingImageControl.Random.Next(4);
      int num2;
      int num3;
      int num4;
      int num5;
      int num6;
      int num7;
      switch (cascadeDirection)
      {
        case CascadeDirection.TopLeft:
        case CascadeDirection.Shuffle:
          num2 = 0;
          num3 = this.Columns;
          num4 = 1;
          num5 = 0;
          num6 = this.Rows;
          num7 = 1;
          break;
        case CascadeDirection.TopRight:
          num2 = this.Columns - 1;
          num3 = -1;
          num4 = -1;
          num5 = 0;
          num6 = this.Rows;
          num7 = 1;
          break;
        case CascadeDirection.BottomRight:
          num2 = this.Columns - 1;
          num3 = -1;
          num4 = -1;
          num5 = this.Rows - 1;
          num6 = -1;
          num7 = -1;
          break;
        case CascadeDirection.BottomLeft:
          num2 = 0;
          num3 = this.Columns;
          num4 = 1;
          num5 = this.Rows - 1;
          num6 = -1;
          num7 = -1;
          break;
        default:
          throw new InvalidOperationException();
      }
      List<Tuple<int, int>> tupleList = new List<Tuple<int, int>>(this.Rows * this.Columns);
      List<Rectangle> rectangleList = new List<Rectangle>(this.Rows * this.Columns);
      List<PlaneProjection> planeProjectionList = new List<PlaneProjection>(this.Rows * this.Columns);
      for (int index1 = num5; index1 != num6; index1 += num7)
      {
        for (int index2 = num2; index2 != num3; index2 += num4)
        {
          Rectangle rectangle = new Rectangle();
          rectangleList.Add(rectangle);
          Grid.SetRow((FrameworkElement) rectangle, index1);
          Grid.SetColumn((FrameworkElement) rectangle, index2);
          tupleList.Add(new Tuple<int, int>(index2, index1));
          ImageBrush imageBrush = new ImageBrush();
          imageBrush.put_ImageSource(this.ImageSource);
          ((Shape) rectangle).put_Fill((Brush) imageBrush);
          CompositeTransform compositeTransform = new CompositeTransform();
          compositeTransform.put_TranslateX((double) -index2);
          compositeTransform.put_ScaleX((double) this.Columns);
          compositeTransform.put_TranslateY((double) -index1);
          compositeTransform.put_ScaleY((double) this.Rows);
          ((Brush) imageBrush).put_RelativeTransform((Transform) compositeTransform);
          PlaneProjection planeProjection = new PlaneProjection();
          planeProjection.put_CenterOfRotationY(0.0);
          ((UIElement) rectangle).put_Projection((Projection) planeProjection);
          planeProjectionList.Add(planeProjection);
          ((ICollection<UIElement>) ((Panel) this._layoutGrid).Children).Add((UIElement) rectangle);
        }
      }
      List<int> list = new List<int>(this.Rows * this.Columns);
      for (int index = 0; index < this.Rows * this.Columns; ++index)
        list.Add(index);
      if (cascadeDirection == CascadeDirection.Shuffle)
        list = ((IEnumerable<int>) list).Shuffle<int>();
      for (int index3 = 0; index3 < list.Count; ++index3)
      {
        int index4 = list[index3];
        PlaneProjection planeProjection = planeProjectionList[index4];
        Rectangle rectangle = rectangleList[index4];
        int num8 = tupleList[index3].Item1;
        int num9 = tupleList[index3].Item2;
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames1, (DependencyObject) planeProjection);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames1, "RotationX");
        TimeSpan timeSpan = this.CascadeSequence == CascadeSequence.EndTogether ? TimeSpan.FromSeconds(num1) : TimeSpan.FromSeconds((double) num9 * this.RowDelay.TotalSeconds + (double) num8 * this.ColumnDelay.TotalSeconds + this.TileDuration.TotalSeconds);
        DoubleKeyFrameCollection keyFrames1 = animationUsingKeyFrames1.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame1 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.Zero);
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_Value(90.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame2 = discreteDoubleKeyFrame1;
        ((ICollection<DoubleKeyFrame>) keyFrames1).Add((DoubleKeyFrame) discreteDoubleKeyFrame2);
        DoubleKeyFrameCollection keyFrames2 = animationUsingKeyFrames1.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame3 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds((double) num9 * this.RowDelay.TotalSeconds + (double) num8 * this.ColumnDelay.TotalSeconds));
        ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_Value(90.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame4 = discreteDoubleKeyFrame3;
        ((ICollection<DoubleKeyFrame>) keyFrames2).Add((DoubleKeyFrame) discreteDoubleKeyFrame4);
        DoubleKeyFrameCollection keyFrames3 = animationUsingKeyFrames1.KeyFrames;
        EasingDoubleKeyFrame easingDoubleKeyFrame1 = new EasingDoubleKeyFrame();
        ((DoubleKeyFrame) easingDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) timeSpan);
        easingDoubleKeyFrame1.put_EasingFunction(this.CascadeInEasingFunction);
        ((DoubleKeyFrame) easingDoubleKeyFrame1).put_Value(0.0);
        EasingDoubleKeyFrame easingDoubleKeyFrame2 = easingDoubleKeyFrame1;
        ((ICollection<DoubleKeyFrame>) keyFrames3).Add((DoubleKeyFrame) easingDoubleKeyFrame2);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames1);
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames2, (DependencyObject) rectangle);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames2, "Opacity");
        DoubleKeyFrameCollection keyFrames4 = animationUsingKeyFrames2.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame5 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.Zero);
        ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_Value(0.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame6 = discreteDoubleKeyFrame5;
        ((ICollection<DoubleKeyFrame>) keyFrames4).Add((DoubleKeyFrame) discreteDoubleKeyFrame6);
        DoubleKeyFrameCollection keyFrames5 = animationUsingKeyFrames2.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame7 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame7).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds((double) num9 * this.RowDelay.TotalSeconds + (double) num8 * this.ColumnDelay.TotalSeconds));
        ((DoubleKeyFrame) discreteDoubleKeyFrame7).put_Value(0.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame8 = discreteDoubleKeyFrame7;
        ((ICollection<DoubleKeyFrame>) keyFrames5).Add((DoubleKeyFrame) discreteDoubleKeyFrame8);
        DoubleKeyFrameCollection keyFrames6 = animationUsingKeyFrames2.KeyFrames;
        EasingDoubleKeyFrame easingDoubleKeyFrame3 = new EasingDoubleKeyFrame();
        ((DoubleKeyFrame) easingDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) timeSpan);
        easingDoubleKeyFrame3.put_EasingFunction(this.CascadeInEasingFunction);
        ((DoubleKeyFrame) easingDoubleKeyFrame3).put_Value(1.0);
        EasingDoubleKeyFrame easingDoubleKeyFrame4 = easingDoubleKeyFrame3;
        ((ICollection<DoubleKeyFrame>) keyFrames6).Add((DoubleKeyFrame) easingDoubleKeyFrame4);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames2);
      }
      sb.Begin();
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>((Func<EventHandler<object>, EventRegistrationToken>) new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) sb).add_Completed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Timeline) sb).remove_Completed), (EventHandler<object>) ((s, e) => sb.Stop()));
    }

    static CascadingImageControl()
    {
      Type type1 = typeof (EasingFunctionBase);
      Type type2 = typeof (CascadingImageControl);
      ElasticEase elasticEase = new ElasticEase();
      ((EasingFunctionBase) elasticEase).put_EasingMode((EasingMode) 0);
      elasticEase.put_Oscillations(3);
      elasticEase.put_Springiness(0.0);
      PropertyMetadata propertyMetadata = new PropertyMetadata((object) elasticEase);
      CascadingImageControl.CascadeInEasingFunctionProperty = DependencyProperty.Register(nameof (CascadeInEasingFunction), (Type) type1, (Type) type2, propertyMetadata);
      CascadingImageControl.CascadeSequenceProperty = DependencyProperty.Register(nameof (CascadeSequence), (Type) typeof (CascadeSequence), (Type) typeof (CascadingImageControl), new PropertyMetadata((object) CascadeSequence.EqualDuration));
    }
  }
}
