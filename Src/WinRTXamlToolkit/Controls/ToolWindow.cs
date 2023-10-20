// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ToolWindow
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Controls.Common;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Tools;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(GroupName = "CloseButtonStates", Name = "CloseButtonCollapsed")]
  [TemplatePart(Name = "PART_RestoreButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_LayoutGrid", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_SizingGrid", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_TitleGrid", Type = typeof (Grid))]
  [TemplatePart(Name = "PART_Border", Type = typeof (Border))]
  [TemplatePart(Name = "PART_ButtonsPanel", Type = typeof (StackPanel))]
  [TemplatePart(Name = "PART_CloseButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_SnapButton", Type = typeof (Button))]
  [TemplateVisualState(GroupName = "SnapButtonStates", Name = "SnapButtonCollapsed")]
  [TemplatePart(Name = "PART_MinimizeButton", Type = typeof (Button))]
  [TemplatePart(Name = "PART_MaximizeButton", Type = typeof (Button))]
  [TemplateVisualState(GroupName = "SnapButtonStates", Name = "SnapButtonVisible")]
  [TemplateVisualState(GroupName = "RestoreButtonStates", Name = "RestoreButtonCollapsed")]
  [TemplateVisualState(GroupName = "RestoreButtonStates", Name = "RestoreButtonVisible")]
  [TemplateVisualState(GroupName = "MaximizeButtonStates", Name = "MaximizeButtonVisible")]
  [TemplateVisualState(GroupName = "MaximizeButtonStates", Name = "MaximizeButtonCollapsed")]
  [TemplateVisualState(GroupName = "CloseButtonStates", Name = "CloseButtonVisible")]
  public class ToolWindow : ContentControl
  {
    private const string LayoutGridName = "PART_LayoutGrid";
    private const string SizingGridName = "PART_SizingGrid";
    private const string TitleGridName = "PART_TitleGrid";
    private const string BorderName = "PART_Border";
    private const string ButtonsPanelName = "PART_ButtonsPanel";
    private const string CloseButtonName = "PART_CloseButton";
    private const string SnapButtonName = "PART_SnapButton";
    private const string RestoreButtonName = "PART_RestoreButton";
    private const string MinimizeButtonName = "PART_MinimizeButton";
    private const string MaximizeButtonName = "PART_MaximizeButton";
    private Grid _layoutGrid;
    private Grid _sizingGrid;
    private Grid _titleGrid;
    private Border _border;
    private StackPanel _buttonsPanel;
    private Button _closeButton;
    private Button _snapButton;
    private Button _restoreButton;
    private Button _minimizeButton;
    private Button _maximizeButton;
    private CompositeTransform _layoutGridTransform;
    private CompositeTransform _titleTransform;
    private Storyboard _currentSnapStoryboard;
    private Rectangle _topLeftSizingThumb;
    private Rectangle _topCenterSizingThumb;
    private Rectangle _topRightSizingThumb;
    private Rectangle _centerLeftSizingThumb;
    private Rectangle _centerRightSizingThumb;
    private Rectangle _bottomLeftSizingThumb;
    private Rectangle _bottomCenterSizingThumb;
    private Rectangle _bottomRightSizingThumb;
    private FrameworkElement _parent;
    private bool _isAdjustedFlick;
    private bool _isFlickTooLong;
    private bool _isDraggingFromSnapped;
    private double _flickStartX;
    private double _flickStartY;
    private double _flickStartCumulativeX;
    private double _flickStartCumulativeY;
    private double _flickStartAngle;
    private double _naturalFlickDisplacementX;
    private double _naturalFlickDisplacementY;
    private double _flickAdjustedEndX;
    private double _flickAdjustedEndY;
    private double _flickAdjustedEndAngle;
    private DoublePoint _lastWindowPosition;
    private double _lastWindowWidth;
    private double _lastWindowHeight;
    private ToolWindow.Edges _lastSnapEdge;
    private bool _restorePositionOnStateChange = true;
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), (Type) typeof (object), (Type) typeof (ToolWindow), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolWindow.OnTitleChanged)));
    public static readonly DependencyProperty TitleTemplateProperty = DependencyProperty.Register(nameof (TitleTemplate), (Type) typeof (DataTemplate), (Type) typeof (ToolWindow), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolWindow.OnTitleTemplateChanged)));
    public static readonly DependencyProperty TitleBackgroundBrushProperty = DependencyProperty.Register(nameof (TitleBackgroundBrush), (Type) typeof (Brush), (Type) typeof (ToolWindow), new PropertyMetadata((object) null));
    public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof (X), (Type) typeof (double), (Type) typeof (ToolWindow), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ToolWindow.OnXChanged)));
    public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof (Y), (Type) typeof (double), (Type) typeof (ToolWindow), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ToolWindow.OnYChanged)));
    public static readonly DependencyProperty WindowStartupLocationProperty = DependencyProperty.Register(nameof (WindowStartupLocation), (Type) typeof (WindowStartupLocation), (Type) typeof (ToolWindow), new PropertyMetadata((object) WindowStartupLocation.Manual, new PropertyChangedCallback(ToolWindow.OnWindowStartupLocationChanged)));
    public static readonly DependencyProperty WindowMovableAreaProperty = DependencyProperty.Register(nameof (WindowMovableArea), (Type) typeof (WindowMovableArea), (Type) typeof (ToolWindow), new PropertyMetadata((object) WindowMovableArea.UseParentBounds, new PropertyChangedCallback(ToolWindow.OnWindowMovableAreaChanged)));
    public static readonly DependencyProperty WindowMovableAreaEdgeThicknessProperty = DependencyProperty.Register(nameof (WindowMovableAreaEdgeThickness), (Type) typeof (Thickness), (Type) typeof (ToolWindow), new PropertyMetadata((object) new Thickness(0.0, 0.0, 0.0, 0.0), new PropertyChangedCallback(ToolWindow.OnWindowMovableAreaEdgeThicknessChanged)));
    public static readonly DependencyProperty WindowEdgeSnapBehaviorProperty = DependencyProperty.Register(nameof (WindowEdgeSnapBehavior), (Type) typeof (WindowEdgeSnapBehavior), (Type) typeof (ToolWindow), new PropertyMetadata((object) WindowEdgeSnapBehavior.ToTitleBarWithRotation, new PropertyChangedCallback(ToolWindow.OnWindowEdgeSnapBehaviorChanged)));
    public static readonly DependencyProperty CanSnapToEdgeProperty = DependencyProperty.Register(nameof (CanSnapToEdge), (Type) typeof (bool), (Type) typeof (ToolWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolWindow.OnCanSnapToEdgeChanged)));
    public static readonly DependencyProperty CanCloseProperty = DependencyProperty.Register(nameof (CanClose), (Type) typeof (bool), (Type) typeof (ToolWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolWindow.OnCanCloseChanged)));
    public static readonly DependencyProperty CanResizeProperty = DependencyProperty.Register(nameof (CanResize), (Type) typeof (bool), (Type) typeof (ToolWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolWindow.OnCanResizeChanged)));
    public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(nameof (CanMaximize), (Type) typeof (bool), (Type) typeof (ToolWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(ToolWindow.OnCanMaximizeChanged)));
    public static readonly DependencyProperty CanDragAnywhereProperty = DependencyProperty.Register(nameof (CanDragAnywhere), (Type) typeof (bool), (Type) typeof (ToolWindow), new PropertyMetadata((object) false, new PropertyChangedCallback(ToolWindow.OnCanDragAnywhereChanged)));
    public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register(nameof (WindowState), (Type) typeof (WindowStates), (Type) typeof (ToolWindow), new PropertyMetadata((object) WindowStates.Normal, new PropertyChangedCallback(ToolWindow.OnWindowStateChanged)));

    public event CancelEventHandler Closing;

    private bool RaiseClosing()
    {
      CancelEventHandler closing = this.Closing;
      if (closing == null)
        return false;
      CancelEventArgs e = new CancelEventArgs(false);
      closing((object) this, e);
      return e.Cancel;
    }

    public event EventHandler Closed;

    private void RaiseClosed()
    {
      EventHandler closed = this.Closed;
      if (closed == null)
        return;
      EventArgs empty = EventArgs.Empty;
      closed((object) this, empty);
    }

    public object Title
    {
      get => ((DependencyObject) this).GetValue(ToolWindow.TitleProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.TitleProperty, value);
    }

    private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      object oldValue = e.OldValue;
      object title = toolWindow.Title;
      toolWindow.OnTitleChanged(oldValue, title);
    }

    protected virtual void OnTitleChanged(object oldTitle, object newTitle)
    {
    }

    public DataTemplate TitleTemplate
    {
      get => (DataTemplate) ((DependencyObject) this).GetValue(ToolWindow.TitleTemplateProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.TitleTemplateProperty, (object) value);
    }

    private static void OnTitleTemplateChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      DataTemplate oldValue = (DataTemplate) e.OldValue;
      DataTemplate titleTemplate = toolWindow.TitleTemplate;
      toolWindow.OnTitleTemplateChanged(oldValue, titleTemplate);
    }

    protected virtual void OnTitleTemplateChanged(
      DataTemplate oldTitleTemplate,
      DataTemplate newTitleTemplate)
    {
    }

    public Brush TitleBackgroundBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(ToolWindow.TitleBackgroundBrushProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.TitleBackgroundBrushProperty, (object) value);
    }

    public double X
    {
      get => (double) ((DependencyObject) this).GetValue(ToolWindow.XProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.XProperty, (object) value);
    }

    private static void OnXChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      double oldValue = (double) e.OldValue;
      double x = toolWindow.X;
      toolWindow.OnXChanged(oldValue, x);
    }

    protected virtual void OnXChanged(double oldX, double newX)
    {
      if (this._layoutGridTransform == null)
        return;
      this._layoutGridTransform.put_TranslateX(newX);
    }

    public double Y
    {
      get => (double) ((DependencyObject) this).GetValue(ToolWindow.YProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.YProperty, (object) value);
    }

    private static void OnYChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      double oldValue = (double) e.OldValue;
      double y = toolWindow.Y;
      toolWindow.OnYChanged(oldValue, y);
    }

    protected virtual void OnYChanged(double oldY, double newY)
    {
      if (this._layoutGridTransform == null)
        return;
      this._layoutGridTransform.put_TranslateY(newY);
    }

    public WindowStartupLocation WindowStartupLocation
    {
      get => (WindowStartupLocation) ((DependencyObject) this).GetValue(ToolWindow.WindowStartupLocationProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.WindowStartupLocationProperty, (object) value);
    }

    private static void OnWindowStartupLocationChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      WindowStartupLocation oldValue = (WindowStartupLocation) e.OldValue;
      WindowStartupLocation windowStartupLocation = toolWindow.WindowStartupLocation;
      toolWindow.OnWindowStartupLocationChanged(oldValue, windowStartupLocation);
    }

    protected virtual void OnWindowStartupLocationChanged(
      WindowStartupLocation oldWindowStartupLocation,
      WindowStartupLocation newWindowStartupLocation)
    {
    }

    public WindowMovableArea WindowMovableArea
    {
      get => (WindowMovableArea) ((DependencyObject) this).GetValue(ToolWindow.WindowMovableAreaProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.WindowMovableAreaProperty, (object) value);
    }

    private static void OnWindowMovableAreaChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      WindowMovableArea oldValue = (WindowMovableArea) e.OldValue;
      WindowMovableArea windowMovableArea = toolWindow.WindowMovableArea;
      toolWindow.OnWindowMovableAreaChanged(oldValue, windowMovableArea);
    }

    protected virtual void OnWindowMovableAreaChanged(
      WindowMovableArea oldWindowMovableArea,
      WindowMovableArea newWindowMovableArea)
    {
      this.SnapToEdgeIfNecessary();
    }

    public Thickness WindowMovableAreaEdgeThickness
    {
      get => (Thickness) ((DependencyObject) this).GetValue(ToolWindow.WindowMovableAreaEdgeThicknessProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.WindowMovableAreaEdgeThicknessProperty, (object) value);
    }

    private static void OnWindowMovableAreaEdgeThicknessChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      Thickness oldValue = (Thickness) e.OldValue;
      Thickness areaEdgeThickness = toolWindow.WindowMovableAreaEdgeThickness;
      toolWindow.OnWindowMovableAreaEdgeThicknessChanged(oldValue, areaEdgeThickness);
    }

    protected virtual void OnWindowMovableAreaEdgeThicknessChanged(
      Thickness oldWindowMovableAreaEdgeThickness,
      Thickness newWindowMovableAreaEdgeThickness)
    {
    }

    public WindowEdgeSnapBehavior WindowEdgeSnapBehavior
    {
      get => (WindowEdgeSnapBehavior) ((DependencyObject) this).GetValue(ToolWindow.WindowEdgeSnapBehaviorProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.WindowEdgeSnapBehaviorProperty, (object) value);
    }

    private static void OnWindowEdgeSnapBehaviorChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      WindowEdgeSnapBehavior oldValue = (WindowEdgeSnapBehavior) e.OldValue;
      WindowEdgeSnapBehavior edgeSnapBehavior = toolWindow.WindowEdgeSnapBehavior;
      toolWindow.OnWindowEdgeSnapBehaviorChanged(oldValue, edgeSnapBehavior);
    }

    protected virtual void OnWindowEdgeSnapBehaviorChanged(
      WindowEdgeSnapBehavior oldWindowEdgeSnapBehavior,
      WindowEdgeSnapBehavior newWindowEdgeSnapBehavior)
    {
      UIElement uiElement = !this.CanDragAnywhere ? (UIElement) this._titleGrid : (UIElement) this._border;
      if (uiElement != null)
      {
        if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
          uiElement.put_ManipulationMode((ManipulationModes) 3);
        else
          uiElement.put_ManipulationMode((ManipulationModes) 67);
      }
      this.SnapToEdgeIfNecessary();
    }

    public bool CanSnapToEdge
    {
      get => (bool) ((DependencyObject) this).GetValue(ToolWindow.CanSnapToEdgeProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.CanSnapToEdgeProperty, (object) value);
    }

    private static void OnCanSnapToEdgeChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      bool oldValue = (bool) e.OldValue;
      bool canSnapToEdge = toolWindow.CanSnapToEdge;
      toolWindow.OnCanSnapToEdgeChanged(oldValue, canSnapToEdge);
    }

    protected virtual void OnCanSnapToEdgeChanged(bool oldCanSnapToEdge, bool newCanSnapToEdge) => this.UpdateVisualStates(true);

    public bool CanClose
    {
      get => (bool) ((DependencyObject) this).GetValue(ToolWindow.CanCloseProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.CanCloseProperty, (object) value);
    }

    private static void OnCanCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      bool oldValue = (bool) e.OldValue;
      bool canClose = toolWindow.CanClose;
      toolWindow.OnCanCloseChanged(oldValue, canClose);
    }

    protected virtual void OnCanCloseChanged(bool oldCanClose, bool newCanClose) => this.UpdateVisualStates(true);

    public bool CanResize
    {
      get => (bool) ((DependencyObject) this).GetValue(ToolWindow.CanResizeProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.CanResizeProperty, (object) value);
    }

    private static void OnCanResizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      bool oldValue = (bool) e.OldValue;
      bool canResize = toolWindow.CanResize;
      toolWindow.OnCanResizeChanged(oldValue, canResize);
    }

    protected virtual void OnCanResizeChanged(bool oldCanResize, bool newCanResize) => this.UpdateVisualStates(true);

    public bool CanMaximize
    {
      get => (bool) ((DependencyObject) this).GetValue(ToolWindow.CanMaximizeProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.CanMaximizeProperty, (object) value);
    }

    private static void OnCanMaximizeChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      bool oldValue = (bool) e.OldValue;
      bool canMaximize = toolWindow.CanMaximize;
      toolWindow.OnCanMaximizeChanged(oldValue, canMaximize);
    }

    protected virtual void OnCanMaximizeChanged(bool oldCanMaximize, bool newCanMaximize) => this.UpdateVisualStates(true);

    public bool CanDragAnywhere
    {
      get => (bool) ((DependencyObject) this).GetValue(ToolWindow.CanDragAnywhereProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.CanDragAnywhereProperty, (object) value);
    }

    private static void OnCanDragAnywhereChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      bool oldValue = (bool) e.OldValue;
      bool canDragAnywhere = toolWindow.CanDragAnywhere;
      toolWindow.OnCanDragAnywhereChanged(oldValue, canDragAnywhere);
    }

    protected virtual void OnCanDragAnywhereChanged(
      bool oldCanDragAnywhere,
      bool newCanDragAnywhere)
    {
      this.UnhookEvents();
      this.HookUpEvents();
    }

    public WindowStates WindowState
    {
      get => (WindowStates) ((DependencyObject) this).GetValue(ToolWindow.WindowStateProperty);
      set => ((DependencyObject) this).SetValue(ToolWindow.WindowStateProperty, (object) value);
    }

    private static void OnWindowStateChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ToolWindow toolWindow = (ToolWindow) d;
      WindowStates oldValue = (WindowStates) e.OldValue;
      WindowStates windowState = toolWindow.WindowState;
      toolWindow.OnWindowStateChanged(oldValue, windowState);
    }

    protected virtual void OnWindowStateChanged(
      WindowStates oldWindowState,
      WindowStates newWindowState)
    {
      if (oldWindowState == newWindowState)
        return;
      if (this._restorePositionOnStateChange)
      {
        switch (oldWindowState)
        {
          case WindowStates.Maximized:
            if (!this._isDraggingFromSnapped)
            {
              this.X = this._lastWindowPosition.X;
              this.Y = this._lastWindowPosition.Y;
            }
            if (this._lastWindowWidth != 0.0)
            {
              ((FrameworkElement) this).put_Width(this._lastWindowWidth);
              ((FrameworkElement) this).put_Height(this._lastWindowHeight);
              break;
            }
            break;
          case WindowStates.Snapped:
            this.StopCurrentSnapStoryboard();
            if (!this._isDraggingFromSnapped)
            {
              this.AnimateStraightSnapAsync(this._lastWindowPosition.X, this._lastWindowPosition.Y);
              break;
            }
            break;
        }
      }
      this.UpdateVisualStates(true);
    }

    public ToolWindow()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (ToolWindow));
      ToolWindow toolWindow1 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) toolWindow1).add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) toolWindow1).remove_Loaded), new RoutedEventHandler(this.OnLoaded));
      ToolWindow toolWindow2 = this;
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) toolWindow2).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) toolWindow2).remove_Unloaded), new RoutedEventHandler(this.OnUnloaded));
      new PropertyChangeEventSource<Thickness>((DependencyObject) this, "BorderThickness").ValueChanged += new EventHandler<Thickness>(this.OnBorderThicknessChanged);
    }

    public async Task ActivateAsync()
    {
      int maxZIndex = ((IEnumerable<DependencyObject>) ((DependencyObject) this).GetSiblings()).Aggregate<DependencyObject, int>(0, (Func<int, DependencyObject, int>) ((z, dob) => Math.Max(z, Canvas.GetZIndex((UIElement) dob))));
      Canvas.SetZIndex((UIElement) this, maxZIndex + 1);
      if (this._layoutGridTransform == null || this._layoutGridTransform.Rotation == 0.0)
        return;
      await this.AnimateToStraighten();
    }

    public void Close()
    {
      if (!this.CanClose || this.RaiseClosing())
        return;
      if (((FrameworkElement) this).Parent is Panel parent2)
        ((ICollection<UIElement>) parent2.Children).Remove((UIElement) this);
      else if (((FrameworkElement) this).Parent is ContentControl parent1)
        parent1.put_Content((object) null);
      else
        this.Hide();
      this.CleanupSnapProperties();
      this.RaiseClosed();
    }

    public void Hide() => ((UIElement) this).put_Visibility((Visibility) 1);

    public void Show()
    {
      ((UIElement) this).put_Visibility((Visibility) 0);
      this.SnapToEdgeIfNecessary();
    }

    public async Task SnapToEdgeAsync()
    {
      if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
        return;
      if (((FrameworkElement) this).Parent == null)
        await ((FrameworkElement) this).WaitForLoadedAsync();
      if (this.WindowState == WindowStates.Normal)
      {
        this._lastWindowPosition.X = this.X;
        this._lastWindowPosition.Y = this.Y;
      }
      Rect movableAreaBoundaries = this.GetMovableArea();
      double distanceToLeft = this.X.Distance(movableAreaBoundaries.Left);
      double distanceToTop = this.Y.Distance(movableAreaBoundaries.Top);
      double distanceToRight = this.X.Distance(movableAreaBoundaries.Right);
      double distanceToBottom = this.Y.Distance(movableAreaBoundaries.Bottom);
      double minDistance = distanceToLeft;
      double x = movableAreaBoundaries.Left - 1.0;
      double y = this.Y;
      if (distanceToTop < minDistance)
      {
        minDistance = distanceToTop;
        x = this.X;
        y = movableAreaBoundaries.Top - 1.0;
      }
      if (distanceToRight < minDistance)
      {
        minDistance = distanceToRight;
        x = movableAreaBoundaries.Right + 1.0;
        y = this.Y;
      }
      if (distanceToBottom < minDistance)
      {
        x = this.X;
        y = movableAreaBoundaries.Bottom + 1.0;
      }
      double angle;
      DoublePoint desiredPosition = this.GetDesiredPosition(x, y, out angle, out this._lastSnapEdge);
      x = desiredPosition.X;
      y = desiredPosition.Y;
      this._restorePositionOnStateChange = false;
      this.WindowState = WindowStates.Snapped;
      this._restorePositionOnStateChange = true;
      if (this.WindowEdgeSnapBehavior != WindowEdgeSnapBehavior.ToTitleBarWithRotation)
        await this.AnimateStraightSnapAsync(x, y);
      else
        await this.AnimateRotatedSnapAsync(x, y, angle);
    }

    public async Task RestoreAsync()
    {
      ((UIElement) this).put_Visibility((Visibility) 0);
      this.WindowState = WindowStates.Normal;
      await this.ActivateAsync();
    }

    public async Task MinimizeAsync() => await this.SnapToEdgeAsync();

    public void Maximize()
    {
      if (this.WindowState == WindowStates.Normal)
      {
        this._lastWindowPosition.X = this.X;
        this._lastWindowPosition.Y = this.Y;
        this._lastWindowWidth = ((FrameworkElement) this).ActualWidth;
        this._lastWindowHeight = ((FrameworkElement) this).ActualHeight;
      }
      this.X = -((Control) this).BorderThickness.Left;
      this.Y = -((Control) this).BorderThickness.Top;
      if (this._parent != null)
      {
        ((FrameworkElement) this).put_Width(this._parent.ActualWidth + ((Control) this).BorderThickness.Left + ((Control) this).BorderThickness.Right);
        ((FrameworkElement) this).put_Height(this._parent.ActualHeight + ((Control) this).BorderThickness.Top + ((Control) this).BorderThickness.Bottom);
      }
      this.UpdateVisualStates(true);
      this.WindowState = WindowStates.Maximized;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
      this.InitializeStartupLocation();
      this.SnapToEdgeIfNecessary();
      this._parent = ((FrameworkElement) this).Parent as FrameworkElement;
      if (this._parent == null)
        return;
      FrameworkElement parent = this._parent;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(parent.add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(parent.remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._parent != null)
        WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._parent.remove_SizeChanged), new SizeChangedEventHandler(this.OnParentSizeChanged));
      this._parent = (FrameworkElement) null;
    }

    private void OnParentSizeChanged(object sender, SizeChangedEventArgs e) => this.SnapToEdgeIfNecessary();

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.UnhookEvents();
      this._layoutGrid = ((Control) this).GetTemplateChild("PART_LayoutGrid") as Grid;
      this._sizingGrid = ((Control) this).GetTemplateChild("PART_SizingGrid") as Grid;
      this._titleGrid = ((Control) this).GetTemplateChild("PART_TitleGrid") as Grid;
      this._border = ((Control) this).GetTemplateChild("PART_Border") as Border;
      this._buttonsPanel = ((Control) this).GetTemplateChild("PART_ButtonsPanel") as StackPanel;
      this._closeButton = ((Control) this).GetTemplateChild("PART_CloseButton") as Button;
      this._snapButton = ((Control) this).GetTemplateChild("PART_SnapButton") as Button;
      this._restoreButton = ((Control) this).GetTemplateChild("PART_RestoreButton") as Button;
      this._minimizeButton = ((Control) this).GetTemplateChild("PART_MinimizeButton") as Button;
      this._maximizeButton = ((Control) this).GetTemplateChild("PART_MaximizeButton") as Button;
      this.HookUpEvents();
      this.UpdateVisualStates(false);
    }

    private void OnBorderThicknessChanged(object sender, Thickness thickness) => this.UpdateSizingThumbSizes();

    private void OnBorderPointerPressed(object sender, PointerRoutedEventArgs e)
    {
    }

    private async void OnBorderManipulationStarting(
      object sender,
      ManipulationStartedRoutedEventArgs e)
    {
      if (this.WindowState == WindowStates.Normal)
      {
        this._lastWindowPosition.X = this.X;
        this._lastWindowPosition.Y = this.Y;
      }
      else if (this.WindowState == WindowStates.Snapped)
        this._isDraggingFromSnapped = true;
      this._restorePositionOnStateChange = false;
      this.WindowState = WindowStates.Normal;
      this._restorePositionOnStateChange = true;
      await this.ActivateAsync();
      this._isAdjustedFlick = false;
    }

    private void OnBorderManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
      if (this._isFlickTooLong)
        e.Complete();
      else if (!this._isAdjustedFlick)
      {
        this.X += ((Point) e.Delta.Translation).X;
        this.Y += ((Point) e.Delta.Translation).Y;
      }
      else
      {
        double num1 = ((Point) e.Cumulative.Translation).X - this._flickStartCumulativeX;
        double num2 = ((Point) e.Cumulative.Translation).Y - this._flickStartCumulativeY;
        double progress = Math.Abs(num1) <= Math.Abs(num2) ? num2 / this._naturalFlickDisplacementY : num1 / this._naturalFlickDisplacementX;
        this.X = MathEx.Lerp(this._flickStartX + ((Point) e.Cumulative.Translation).X - this._flickStartCumulativeX, MathEx.Lerp(this._flickStartX, this._flickAdjustedEndX, progress), progress);
        this.Y = MathEx.Lerp(this._flickStartY + ((Point) e.Cumulative.Translation).Y - this._flickStartCumulativeY, MathEx.Lerp(this._flickStartY, this._flickAdjustedEndY, progress), progress);
        if (this._layoutGridTransform == null)
          return;
        this._layoutGridTransform.put_Rotation(MathEx.Lerp(this._flickStartAngle, this._flickAdjustedEndAngle, progress));
      }
    }

    private void OnBorderManipulationInertiaStarting(
      object sender,
      ManipulationInertiaStartingRoutedEventArgs e)
    {
      this._flickStartX = this.X;
      this._flickStartY = this.Y;
      this._flickStartCumulativeX = ((Point) e.Cumulative.Translation).X;
      this._flickStartCumulativeY = ((Point) e.Cumulative.Translation).Y;
      int num = this._isDraggingFromSnapped ? 1 : 0;
      if (this._layoutGridTransform != null)
        this._flickStartAngle = this._layoutGridTransform.Rotation;
      double expectedDisplacement = e.GetExpectedDisplacement();
      e.TranslationBehavior.put_DesiredDisplacement(expectedDisplacement);
      this._naturalFlickDisplacementX = e.GetExpectedDisplacementX();
      this._naturalFlickDisplacementY = e.GetExpectedDisplacementY();
      ToolWindow.Edges edge;
      DoublePoint desiredPosition = this.GetDesiredPosition(this._flickStartX + this._naturalFlickDisplacementX, this._flickStartY + this._naturalFlickDisplacementY, out this._flickAdjustedEndAngle, out edge);
      this._flickAdjustedEndX = desiredPosition.X;
      this._flickAdjustedEndY = desiredPosition.Y;
      if (edge == ToolWindow.Edges.None)
        return;
      this._lastSnapEdge = edge;
      this._isAdjustedFlick = true;
      if (this._flickAdjustedEndX != this._naturalFlickDisplacementX && (this._flickAdjustedEndY == this._naturalFlickDisplacementY || this._flickAdjustedEndX.Distance(this._naturalFlickDisplacementX) > this._flickAdjustedEndY.Distance(this._naturalFlickDisplacementY)))
      {
        e.SetDesiredDisplacementX(this._flickAdjustedEndX - this._flickStartX);
        this._naturalFlickDisplacementX = e.GetExpectedDisplacementX();
        this._naturalFlickDisplacementY = e.GetExpectedDisplacementY();
      }
      else if (this._flickAdjustedEndY != this._naturalFlickDisplacementY && (this._flickAdjustedEndX == this._naturalFlickDisplacementX || this._flickAdjustedEndX.Distance(this._naturalFlickDisplacementY) > this._flickAdjustedEndY.Distance(this._naturalFlickDisplacementX)))
      {
        e.SetDesiredDisplacementY(this._flickAdjustedEndY - this._flickStartY);
        this._naturalFlickDisplacementX = e.GetExpectedDisplacementX();
        this._naturalFlickDisplacementY = e.GetExpectedDisplacementY();
      }
      if (e.GetExpectedDisplacementDuration() <= 0.5)
        return;
      this._isFlickTooLong = true;
      this._restorePositionOnStateChange = false;
      this.WindowState = WindowStates.Snapped;
      this._restorePositionOnStateChange = true;
      if (this.WindowEdgeSnapBehavior != WindowEdgeSnapBehavior.ToTitleBarWithRotation)
        this.AnimateStraightSnapAsync(this._flickAdjustedEndX, this._flickAdjustedEndY);
      else
        this.AnimateRotatedSnapAsync(this._flickAdjustedEndX, this._flickAdjustedEndY, this._flickAdjustedEndAngle);
    }

    private void OnBorderManipulationCompleted(
      object sender,
      ManipulationCompletedRoutedEventArgs e)
    {
      if (this._isFlickTooLong)
      {
        this._isFlickTooLong = false;
      }
      else
      {
        this.SnapToEdgeIfNecessary();
        this._isDraggingFromSnapped = false;
      }
    }

    private void SnapToEdgeIfNecessary()
    {
      if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
        return;
      double angle;
      DoublePoint desiredPosition = this.GetDesiredPosition(this.X, this.Y, out angle, out this._lastSnapEdge);
      double x = desiredPosition.X;
      double y = desiredPosition.Y;
      if (this._layoutGridTransform == null || x == this.X && y == this.Y && angle == this._layoutGridTransform.Rotation)
        return;
      this._restorePositionOnStateChange = false;
      this.WindowState = WindowStates.Snapped;
      this._restorePositionOnStateChange = true;
      if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.Straight || this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.StraightToTitleBar)
        this.AnimateStraightSnapAsync(x, y);
      else
        this.AnimateRotatedSnapAsync(x, y, angle);
    }

    private DoublePoint GetDesiredPosition(
      double startx,
      double starty,
      out double angle,
      out ToolWindow.Edges edge)
    {
      angle = 0.0;
      edge = ToolWindow.Edges.None;
      if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
      {
        this._lastWindowPosition.X = startx;
        this._lastWindowPosition.Y = starty;
      }
      Rect movableArea = this.GetMovableArea();
      if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.Straight || this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.StraightToTitleBar)
      {
        double num1 = startx.Max(movableArea.Left).Min(movableArea.Right);
        double num2 = starty.Max(movableArea.Top).Min(movableArea.Bottom);
        DoublePoint desiredPosition;
        desiredPosition.X = num1;
        desiredPosition.Y = num2;
        return desiredPosition;
      }
      double lv1 = startx.Max(movableArea.Left).Min(movableArea.Right);
      double lv2 = starty.Max(movableArea.Top).Min(movableArea.Bottom);
      if (lv1 != startx || lv2 != starty)
      {
        double num = this._titleGrid == null ? 0.0 : ((FrameworkElement) this._titleGrid).ActualHeight + (this._border == null ? 0.0 : this._border.BorderThickness.Top);
        double actualHeight = ((FrameworkElement) this).ActualHeight;
        double actualWidth = ((FrameworkElement) this).ActualWidth;
        if (lv1 != startx && (lv2 == starty || lv1.Distance(startx) > lv2.Distance(starty)))
        {
          if (startx < lv1)
          {
            edge = ToolWindow.Edges.Left;
            lv1 += num - actualWidth / 2.0 - actualHeight / 2.0;
            angle = 90.0;
          }
          else
          {
            edge = ToolWindow.Edges.Right;
            lv1 -= num - actualWidth / 2.0 - actualHeight / 2.0;
            angle = -90.0;
          }
          double rv1 = movableArea.Top + ((FrameworkElement) this).ActualWidth / 2.0 - ((FrameworkElement) this).ActualHeight / 2.0;
          double rv2 = movableArea.Bottom - ((FrameworkElement) this).ActualWidth / 2.0 + ((FrameworkElement) this).ActualHeight / 2.0;
          lv2 = starty.Max(rv1).Min(rv2);
        }
        else if (lv2 != starty && (lv1 == startx || lv2.Distance(starty) > lv1.Distance(startx)))
        {
          if (starty < lv2)
          {
            edge = ToolWindow.Edges.Top;
            angle = 180.0;
            lv2 += num - actualHeight;
          }
          else
          {
            edge = ToolWindow.Edges.Bottom;
            angle = 0.0;
            lv2 -= num - actualHeight;
          }
        }
      }
      DoublePoint desiredPosition1;
      desiredPosition1.X = lv1;
      desiredPosition1.Y = lv2;
      return desiredPosition1;
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e) => this.Close();

    private void OnSnapButtonClick(object sender, RoutedEventArgs e) => this.SnapToEdgeAsync();

    private void OnRestoreButtonClick(object sender, RoutedEventArgs e) => this.RestoreAsync();

    private async void OnMinimizeButtonClick(object sender, RoutedEventArgs e) => await this.MinimizeAsync();

    private void OnMaximizeButtonClick(object sender, RoutedEventArgs e) => this.Maximize();

    private void FilterOutManipulations(UIElement element)
    {
      element.put_ManipulationMode((ManipulationModes) 3);
      WindowsRuntimeMarshal.AddEventHandler<ManipulationStartingEventHandler>((Func<ManipulationStartingEventHandler, EventRegistrationToken>) new Func<ManipulationStartingEventHandler, EventRegistrationToken>(element.add_ManipulationStarting), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(element.remove_ManipulationStarting), new ManipulationStartingEventHandler(this.OnFilteredOutManipulationStarting));
      WindowsRuntimeMarshal.AddEventHandler<ManipulationStartedEventHandler>((Func<ManipulationStartedEventHandler, EventRegistrationToken>) new Func<ManipulationStartedEventHandler, EventRegistrationToken>(element.add_ManipulationStarted), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(element.remove_ManipulationStarted), new ManipulationStartedEventHandler(this.OnFilteredOutManipulationStarted));
    }

    private void UnFilterOutManipulations(UIElement element)
    {
      element.put_ManipulationMode((ManipulationModes) 3);
      WindowsRuntimeMarshal.RemoveEventHandler<ManipulationStartingEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(element.remove_ManipulationStarting), new ManipulationStartingEventHandler(this.OnFilteredOutManipulationStarting));
      WindowsRuntimeMarshal.RemoveEventHandler<ManipulationStartedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(element.remove_ManipulationStarted), new ManipulationStartedEventHandler(this.OnFilteredOutManipulationStarted));
    }

    private void OnFilteredOutManipulationStarting(
      object sender,
      ManipulationStartingRoutedEventArgs e)
    {
      e.put_Handled(true);
    }

    private void OnFilteredOutManipulationStarted(
      object sender,
      ManipulationStartedRoutedEventArgs e)
    {
      e.put_Handled(true);
      e.Complete();
    }

    private void UpdateVisualStates(bool useTransitions)
    {
      if (this.CanClose)
        VisualStateManager.GoToState((Control) this, "CloseButtonVisible", useTransitions);
      else
        VisualStateManager.GoToState((Control) this, "CloseButtonCollapsed", useTransitions);
      switch (this.WindowState)
      {
        case WindowStates.Normal:
          if (this.CanSnapToEdge)
            VisualStateManager.GoToState((Control) this, "SnapButtonVisible", useTransitions);
          else
            VisualStateManager.GoToState((Control) this, "SnapButtonCollapsed", useTransitions);
          if (this.CanMaximize)
            VisualStateManager.GoToState((Control) this, "MaximizeButtonVisible", useTransitions);
          else
            VisualStateManager.GoToState((Control) this, "MaximizeButtonCollapsed", useTransitions);
          VisualStateManager.GoToState((Control) this, "RestoreButtonCollapsed", useTransitions);
          if (this._sizingGrid == null)
            break;
          ((UIElement) this._sizingGrid).put_Visibility(this.CanResize ? (Visibility) 0 : (Visibility) 1);
          break;
        case WindowStates.Maximized:
          if (this.CanSnapToEdge)
            VisualStateManager.GoToState((Control) this, "SnapButtonVisible", useTransitions);
          else
            VisualStateManager.GoToState((Control) this, "SnapButtonCollapsed", useTransitions);
          VisualStateManager.GoToState((Control) this, "RestoreButtonVisible", useTransitions);
          VisualStateManager.GoToState((Control) this, "MaximizeButtonCollapsed", useTransitions);
          break;
        case WindowStates.Snapped:
          VisualStateManager.GoToState((Control) this, "SnapButtonCollapsed", useTransitions);
          VisualStateManager.GoToState((Control) this, "RestoreButtonVisible", useTransitions);
          VisualStateManager.GoToState((Control) this, "MaximizeButtonCollapsed", useTransitions);
          if (this._sizingGrid == null)
            break;
          ((UIElement) this._sizingGrid).put_Visibility((Visibility) 1);
          break;
      }
    }

    private void InitializeStartupLocation()
    {
      if (!(((FrameworkElement) this).Parent is FrameworkElement parent))
        return;
      switch (this.WindowStartupLocation)
      {
        case WindowStartupLocation.CenterOwner:
          this.X = Math.Round(parent.ActualWidth / 2.0 - ((FrameworkElement) this).ActualWidth / 2.0);
          this.Y = Math.Round(parent.ActualHeight / 2.0 - ((FrameworkElement) this).ActualHeight / 2.0);
          break;
        case WindowStartupLocation.CenterScreen:
          Point position = ((UIElement) parent).GetPosition();
          if (Window.Current.Content is FrameworkElement content)
          {
            this.X = Math.Round(content.ActualWidth / 2.0 - ((FrameworkElement) this).ActualWidth / 2.0 - position.X);
            this.Y = Math.Round(content.ActualHeight / 2.0 - ((FrameworkElement) this).ActualHeight / 2.0 - position.Y);
            break;
          }
          break;
      }
      if (this.WindowState != WindowStates.Normal)
        return;
      this._lastWindowPosition.X = this.X;
      this._lastWindowPosition.Y = this.Y;
      this._lastWindowWidth = ((FrameworkElement) this).ActualWidth;
      this._lastWindowHeight = ((FrameworkElement) this).ActualHeight;
    }

    private void HookUpEvents()
    {
      if (this._layoutGrid != null)
      {
        ((UIElement) this._layoutGrid).put_RenderTransformOrigin((Point) new Point(0.5, 0.5));
        ((UIElement) this._layoutGrid).put_RenderTransform((Transform) (this._layoutGridTransform = new CompositeTransform()));
        this._layoutGridTransform.put_TranslateX(this.X);
        this._layoutGridTransform.put_TranslateY(this.Y);
      }
      UIElement uiElement = !this.CanDragAnywhere ? (UIElement) this._titleGrid : (UIElement) this._border;
      if (uiElement != null)
      {
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(uiElement.add_PointerPressed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(uiElement.remove_PointerPressed), new PointerEventHandler(this.OnBorderPointerPressed));
        if (this.WindowEdgeSnapBehavior == WindowEdgeSnapBehavior.None)
          uiElement.put_ManipulationMode((ManipulationModes) 3);
        else
          uiElement.put_ManipulationMode((ManipulationModes) 67);
        WindowsRuntimeMarshal.AddEventHandler<ManipulationStartedEventHandler>((Func<ManipulationStartedEventHandler, EventRegistrationToken>) new Func<ManipulationStartedEventHandler, EventRegistrationToken>(uiElement.add_ManipulationStarted), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(uiElement.remove_ManipulationStarted), new ManipulationStartedEventHandler(this.OnBorderManipulationStarting));
        WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>((Func<ManipulationDeltaEventHandler, EventRegistrationToken>) new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(uiElement.add_ManipulationDelta), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(uiElement.remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnBorderManipulationDelta));
        WindowsRuntimeMarshal.AddEventHandler<ManipulationInertiaStartingEventHandler>((Func<ManipulationInertiaStartingEventHandler, EventRegistrationToken>) new Func<ManipulationInertiaStartingEventHandler, EventRegistrationToken>(uiElement.add_ManipulationInertiaStarting), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(uiElement.remove_ManipulationInertiaStarting), new ManipulationInertiaStartingEventHandler(this.OnBorderManipulationInertiaStarting));
        WindowsRuntimeMarshal.AddEventHandler<ManipulationCompletedEventHandler>((Func<ManipulationCompletedEventHandler, EventRegistrationToken>) new Func<ManipulationCompletedEventHandler, EventRegistrationToken>(uiElement.add_ManipulationCompleted), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(uiElement.remove_ManipulationCompleted), new ManipulationCompletedEventHandler(this.OnBorderManipulationCompleted));
      }
      if (this._titleGrid != null)
      {
        ((UIElement) this._titleGrid).put_RenderTransformOrigin((Point) new Point(0.5, 0.5));
        ((UIElement) this._titleGrid).put_RenderTransform((Transform) (this._titleTransform = new CompositeTransform()));
        Grid titleGrid = this._titleGrid;
        WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) titleGrid).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) titleGrid).remove_SizeChanged), new SizeChangedEventHandler(this.OnTitleGridSizeChanged));
      }
      if (this._closeButton != null)
      {
        Button closeButton = this._closeButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) closeButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) closeButton).remove_Click), new RoutedEventHandler(this.OnCloseButtonClick));
        this.FilterOutManipulations((UIElement) this._closeButton);
      }
      if (this._snapButton != null)
      {
        Button snapButton = this._snapButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) snapButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) snapButton).remove_Click), new RoutedEventHandler(this.OnSnapButtonClick));
        this.FilterOutManipulations((UIElement) this._snapButton);
      }
      if (this._restoreButton != null)
      {
        Button restoreButton = this._restoreButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) restoreButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) restoreButton).remove_Click), new RoutedEventHandler(this.OnRestoreButtonClick));
        this.FilterOutManipulations((UIElement) this._restoreButton);
      }
      if (this._minimizeButton != null)
      {
        Button minimizeButton = this._minimizeButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) minimizeButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) minimizeButton).remove_Click), new RoutedEventHandler(this.OnMinimizeButtonClick));
        this.FilterOutManipulations((UIElement) this._minimizeButton);
      }
      if (this._maximizeButton != null)
      {
        Button maximizeButton = this._maximizeButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) maximizeButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) maximizeButton).remove_Click), new RoutedEventHandler(this.OnMaximizeButtonClick));
        this.FilterOutManipulations((UIElement) this._maximizeButton);
      }
      if (this._sizingGrid == null)
        return;
      Rectangle rectangle1 = new Rectangle();
      ((FrameworkElement) rectangle1).put_HorizontalAlignment((HorizontalAlignment) 0);
      ((FrameworkElement) rectangle1).put_VerticalAlignment((VerticalAlignment) 0);
      ((Shape) rectangle1).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle1).put_ManipulationMode((ManipulationModes) 3);
      this._topLeftSizingThumb = rectangle1;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._topLeftSizingThumb);
      Rectangle rectangle2 = new Rectangle();
      ((FrameworkElement) rectangle2).put_HorizontalAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) rectangle2).put_VerticalAlignment((VerticalAlignment) 0);
      ((Shape) rectangle2).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle2).put_ManipulationMode((ManipulationModes) 2);
      this._topCenterSizingThumb = rectangle2;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._topCenterSizingThumb);
      Rectangle rectangle3 = new Rectangle();
      ((FrameworkElement) rectangle3).put_HorizontalAlignment((HorizontalAlignment) 2);
      ((FrameworkElement) rectangle3).put_VerticalAlignment((VerticalAlignment) 0);
      ((Shape) rectangle3).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle3).put_ManipulationMode((ManipulationModes) 3);
      this._topRightSizingThumb = rectangle3;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._topRightSizingThumb);
      Rectangle rectangle4 = new Rectangle();
      ((FrameworkElement) rectangle4).put_HorizontalAlignment((HorizontalAlignment) 0);
      ((FrameworkElement) rectangle4).put_VerticalAlignment((VerticalAlignment) 3);
      ((Shape) rectangle4).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle4).put_ManipulationMode((ManipulationModes) 1);
      this._centerLeftSizingThumb = rectangle4;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._centerLeftSizingThumb);
      Rectangle rectangle5 = new Rectangle();
      ((FrameworkElement) rectangle5).put_HorizontalAlignment((HorizontalAlignment) 2);
      ((FrameworkElement) rectangle5).put_VerticalAlignment((VerticalAlignment) 3);
      ((Shape) rectangle5).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle5).put_ManipulationMode((ManipulationModes) 1);
      this._centerRightSizingThumb = rectangle5;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._centerRightSizingThumb);
      Rectangle rectangle6 = new Rectangle();
      ((FrameworkElement) rectangle6).put_HorizontalAlignment((HorizontalAlignment) 0);
      ((FrameworkElement) rectangle6).put_VerticalAlignment((VerticalAlignment) 2);
      ((Shape) rectangle6).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle6).put_ManipulationMode((ManipulationModes) 3);
      this._bottomLeftSizingThumb = rectangle6;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._bottomLeftSizingThumb);
      Rectangle rectangle7 = new Rectangle();
      ((FrameworkElement) rectangle7).put_HorizontalAlignment((HorizontalAlignment) 3);
      ((FrameworkElement) rectangle7).put_VerticalAlignment((VerticalAlignment) 2);
      ((Shape) rectangle7).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle7).put_ManipulationMode((ManipulationModes) 2);
      this._bottomCenterSizingThumb = rectangle7;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._bottomCenterSizingThumb);
      Rectangle rectangle8 = new Rectangle();
      ((FrameworkElement) rectangle8).put_HorizontalAlignment((HorizontalAlignment) 2);
      ((FrameworkElement) rectangle8).put_VerticalAlignment((VerticalAlignment) 2);
      ((Shape) rectangle8).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((UIElement) rectangle8).put_ManipulationMode((ManipulationModes) 3);
      this._bottomRightSizingThumb = rectangle8;
      ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Add((UIElement) this._bottomRightSizingThumb);
      this.UpdateSizingThumbSizes();
      this.HookUpSizingThumbManipulations(this._topLeftSizingThumb);
      this.HookUpSizingThumbManipulations(this._topCenterSizingThumb);
      this.HookUpSizingThumbManipulations(this._topRightSizingThumb);
      this.HookUpSizingThumbManipulations(this._centerLeftSizingThumb);
      this.HookUpSizingThumbManipulations(this._centerRightSizingThumb);
      this.HookUpSizingThumbManipulations(this._bottomLeftSizingThumb);
      this.HookUpSizingThumbManipulations(this._bottomCenterSizingThumb);
      this.HookUpSizingThumbManipulations(this._bottomRightSizingThumb);
    }

    private void OnTitleGridSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.WindowState != WindowStates.Snapped)
        return;
      this.SnapToEdgeAsync();
    }

    private void HookUpSizingThumbManipulations(Rectangle thumb) => WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>((Func<ManipulationDeltaEventHandler, EventRegistrationToken>) new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(((UIElement) thumb).add_ManipulationDelta), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) thumb).remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnThumbManipulationDelta));

    private void OnThumbManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
    {
      double x = ((Point) e.Delta.Translation).X;
      double y = ((Point) e.Delta.Translation).Y;
      double min1 = double.IsNaN(((FrameworkElement) this).MinWidth) ? 0.0 : ((FrameworkElement) this).MinWidth;
      double min2 = double.IsNaN(((FrameworkElement) this).MinHeight) ? 0.0 : ((FrameworkElement) this).MinHeight;
      double num1 = ((FrameworkElement) this).MaxWidth;
      double num2 = ((FrameworkElement) this).MaxHeight;
      if (this._parent != null)
      {
        num1 = num1.Min(this._parent.ActualWidth);
        num2 = num2.Min(this._parent.ActualHeight);
      }
      if (this._buttonsPanel != null)
        min1 = ((FrameworkElement) this._buttonsPanel).ActualWidth + ((Control) this).BorderThickness.Left + ((Control) this).BorderThickness.Right;
      if (this._titleGrid != null)
        min2 = ((FrameworkElement) this._titleGrid).ActualHeight + ((Control) this).BorderThickness.Top + ((Control) this).BorderThickness.Bottom;
      if (sender == this._topLeftSizingThumb || sender == this._centerLeftSizingThumb || sender == this._bottomLeftSizingThumb)
      {
        this.X += x;
        ((FrameworkElement) this).put_Width((((FrameworkElement) this).ActualWidth - x).Clamp(min1, num1));
      }
      else if (sender == this._topRightSizingThumb || sender == this._centerRightSizingThumb || sender == this._bottomRightSizingThumb)
        ((FrameworkElement) this).put_Width((((FrameworkElement) this).ActualWidth + x).Clamp(min1, num1));
      if (sender == this._topLeftSizingThumb || sender == this._topCenterSizingThumb || sender == this._topRightSizingThumb)
      {
        this.Y += y;
        ((FrameworkElement) this).put_Height((((FrameworkElement) this).ActualHeight - y).Clamp(min2, num2));
      }
      else
      {
        if (sender != this._bottomLeftSizingThumb && sender != this._bottomCenterSizingThumb && sender != this._bottomRightSizingThumb)
          return;
        ((FrameworkElement) this).put_Height((((FrameworkElement) this).ActualHeight + y).Clamp(min2, num2));
      }
    }

    private void UpdateSizingThumbSizes()
    {
      double left = ((Control) this).BorderThickness.Left;
      double top = ((Control) this).BorderThickness.Top;
      double right = ((Control) this).BorderThickness.Right;
      double bottom = ((Control) this).BorderThickness.Bottom;
      if (this._topLeftSizingThumb == null)
        return;
      ((FrameworkElement) this._topLeftSizingThumb).put_Width(left);
      ((FrameworkElement) this._topLeftSizingThumb).put_Height(top);
      ((FrameworkElement) this._topCenterSizingThumb).put_Margin(new Thickness(left, 0.0, right, 0.0));
      ((FrameworkElement) this._topCenterSizingThumb).put_Height(top);
      ((FrameworkElement) this._topRightSizingThumb).put_Width(right);
      ((FrameworkElement) this._topRightSizingThumb).put_Height(top);
      ((FrameworkElement) this._centerLeftSizingThumb).put_Width(left);
      ((FrameworkElement) this._centerLeftSizingThumb).put_Margin(new Thickness(0.0, top, 0.0, bottom));
      ((FrameworkElement) this._centerRightSizingThumb).put_Width(right);
      ((FrameworkElement) this._centerRightSizingThumb).put_Margin(new Thickness(0.0, top, 0.0, bottom));
      ((FrameworkElement) this._bottomLeftSizingThumb).put_Width(left);
      ((FrameworkElement) this._bottomLeftSizingThumb).put_Height(bottom);
      ((FrameworkElement) this._bottomCenterSizingThumb).put_Margin(new Thickness(left, 0.0, right, 0.0));
      ((FrameworkElement) this._bottomCenterSizingThumb).put_Height(bottom);
      ((FrameworkElement) this._bottomRightSizingThumb).put_Width(left);
      ((FrameworkElement) this._bottomRightSizingThumb).put_Height(bottom);
    }

    private void UnhookEvents()
    {
      if (this._border != null)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._border).remove_PointerPressed), new PointerEventHandler(this.OnBorderPointerPressed));
        WindowsRuntimeMarshal.RemoveEventHandler<ManipulationDeltaEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._border).remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnBorderManipulationDelta));
        WindowsRuntimeMarshal.RemoveEventHandler<ManipulationInertiaStartingEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._border).remove_ManipulationInertiaStarting), new ManipulationInertiaStartingEventHandler(this.OnBorderManipulationInertiaStarting));
        WindowsRuntimeMarshal.RemoveEventHandler<ManipulationCompletedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._border).remove_ManipulationCompleted), new ManipulationCompletedEventHandler(this.OnBorderManipulationCompleted));
      }
      if (this._closeButton != null)
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) this._closeButton).remove_Click), new RoutedEventHandler(this.OnCloseButtonClick));
      if (this._snapButton != null)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) this._snapButton).remove_Click), new RoutedEventHandler(this.OnSnapButtonClick));
        this.UnFilterOutManipulations((UIElement) this._snapButton);
      }
      if (this._sizingGrid != null)
      {
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._topLeftSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._topCenterSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._topRightSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._centerLeftSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._centerRightSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._bottomLeftSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._bottomCenterSizingThumb);
        ((ICollection<UIElement>) ((Panel) this._sizingGrid).Children).Remove((UIElement) this._bottomRightSizingThumb);
      }
      if (this._titleGrid == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this._titleGrid).remove_SizeChanged), new SizeChangedEventHandler(this.OnTitleGridSizeChanged));
    }

    private void CleanupSnapProperties()
    {
      if (this._layoutGridTransform != null)
      {
        this.X = Math.Round(this._layoutGridTransform.TranslateX);
        this.Y = Math.Round(this._layoutGridTransform.TranslateY);
        this._layoutGridTransform.put_Rotation(0.0);
      }
      if (this._titleTransform != null)
      {
        this._titleTransform.put_TranslateX(0.0);
        this._titleTransform.put_TranslateY(0.0);
        this._titleTransform.put_Rotation(0.0);
      }
      if (this._titleGrid == null)
        return;
      ((UIElement) this._titleGrid).put_Opacity(1.0);
    }

    private async Task AnimateStraightSnapAsync(double x, double y)
    {
      if (this._layoutGridTransform == null)
        return;
      Storyboard sb = new Storyboard();
      DoubleAnimation dax = new DoubleAnimation();
      ((Timeline) dax).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) dax, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) dax, "TranslateX");
      dax.put_To((double?) new double?(x));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) dax);
      DoubleAnimation day = new DoubleAnimation();
      ((Timeline) day).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) day, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) day, "TranslateY");
      day.put_To((double?) new double?(y));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) day);
      DoubleAnimation dar = new DoubleAnimation();
      ((Timeline) dar).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) dar, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) dar, "Rotation");
      dar.put_To((double?) new double?(0.0));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) dar);
      this._currentSnapStoryboard = sb;
      await sb.BeginAsync();
      this._currentSnapStoryboard = (Storyboard) null;
      if (this._layoutGridTransform == null)
        return;
      this.X = Math.Round(this._layoutGridTransform.TranslateX);
      this.Y = Math.Round(this._layoutGridTransform.TranslateY);
    }

    private async Task AnimateRotatedSnapAsync(double x, double y, double angle)
    {
      if (this._layoutGridTransform == null)
        return;
      Storyboard sb = new Storyboard();
      DoubleAnimation dax = new DoubleAnimation();
      ((Timeline) dax).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) dax, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) dax, "TranslateX");
      dax.put_To((double?) new double?(x));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) dax);
      DoubleAnimation day = new DoubleAnimation();
      ((Timeline) day).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) day, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) day, "TranslateY");
      day.put_To((double?) new double?(y));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) day);
      DoubleAnimation dar = new DoubleAnimation();
      ((Timeline) dar).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) dar, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) dar, "Rotation");
      dar.put_To((double?) new double?(angle));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) dar);
      if (this._titleTransform != null && this._titleGrid != null && angle == 180.0)
      {
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames1, (DependencyObject) this._titleGrid);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames1, "Opacity");
        DoubleKeyFrameCollection keyFrames1 = animationUsingKeyFrames1.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame1 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.0));
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_Value(1.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame2 = discreteDoubleKeyFrame1;
        ((ICollection<DoubleKeyFrame>) keyFrames1).Add((DoubleKeyFrame) discreteDoubleKeyFrame2);
        DoubleKeyFrameCollection keyFrames2 = animationUsingKeyFrames1.KeyFrames;
        LinearDoubleKeyFrame linearDoubleKeyFrame1 = new LinearDoubleKeyFrame();
        ((DoubleKeyFrame) linearDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.1));
        ((DoubleKeyFrame) linearDoubleKeyFrame1).put_Value(0.0);
        LinearDoubleKeyFrame linearDoubleKeyFrame2 = linearDoubleKeyFrame1;
        ((ICollection<DoubleKeyFrame>) keyFrames2).Add((DoubleKeyFrame) linearDoubleKeyFrame2);
        DoubleKeyFrameCollection keyFrames3 = animationUsingKeyFrames1.KeyFrames;
        LinearDoubleKeyFrame linearDoubleKeyFrame3 = new LinearDoubleKeyFrame();
        ((DoubleKeyFrame) linearDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.2));
        ((DoubleKeyFrame) linearDoubleKeyFrame3).put_Value(1.0);
        LinearDoubleKeyFrame linearDoubleKeyFrame4 = linearDoubleKeyFrame3;
        ((ICollection<DoubleKeyFrame>) keyFrames3).Add((DoubleKeyFrame) linearDoubleKeyFrame4);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames1);
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames2, (DependencyObject) this._titleTransform);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames2, "Rotation");
        DoubleKeyFrameCollection keyFrames4 = animationUsingKeyFrames2.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame3 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.1));
        ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_Value(-180.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame4 = discreteDoubleKeyFrame3;
        ((ICollection<DoubleKeyFrame>) keyFrames4).Add((DoubleKeyFrame) discreteDoubleKeyFrame4);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames2);
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames3, (DependencyObject) this._titleTransform);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames3, "TranslateY");
        DoubleKeyFrameCollection keyFrames5 = animationUsingKeyFrames3.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame5 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.1));
        ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_Value(-((Control) this).BorderThickness.Top);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame6 = discreteDoubleKeyFrame5;
        ((ICollection<DoubleKeyFrame>) keyFrames5).Add((DoubleKeyFrame) discreteDoubleKeyFrame6);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames3);
      }
      this._currentSnapStoryboard = sb;
      await sb.BeginAsync();
      this._currentSnapStoryboard = (Storyboard) null;
      if (this._layoutGridTransform == null)
        return;
      this.X = Math.Round(this._layoutGridTransform.TranslateX);
      this.Y = Math.Round(this._layoutGridTransform.TranslateY);
    }

    private void StopCurrentSnapStoryboard()
    {
      if (this._currentSnapStoryboard == null)
        return;
      if (this._layoutGridTransform != null)
      {
        this.X = Math.Round(this._layoutGridTransform.TranslateX);
        this.Y = Math.Round(this._layoutGridTransform.TranslateY);
      }
      this._currentSnapStoryboard.Stop();
      this.AnimateToStraighten();
    }

    private async Task AnimateToStraighten()
    {
      if (this._layoutGridTransform == null)
        return;
      Storyboard sb = new Storyboard();
      DoubleAnimation dar = new DoubleAnimation();
      ((Timeline) dar).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(0.2));
      Storyboard.SetTarget((Timeline) dar, (DependencyObject) this._layoutGridTransform);
      Storyboard.SetTargetProperty((Timeline) dar, "Rotation");
      dar.put_To((double?) new double?(0.0));
      ((ICollection<Timeline>) sb.Children).Add((Timeline) dar);
      if (this._titleTransform != null && this._titleGrid != null && this._titleTransform.Rotation == -180.0)
      {
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames1 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames1, (DependencyObject) this._titleGrid);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames1, "Opacity");
        DoubleKeyFrameCollection keyFrames1 = animationUsingKeyFrames1.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame1 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.0));
        ((DoubleKeyFrame) discreteDoubleKeyFrame1).put_Value(1.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame2 = discreteDoubleKeyFrame1;
        ((ICollection<DoubleKeyFrame>) keyFrames1).Add((DoubleKeyFrame) discreteDoubleKeyFrame2);
        DoubleKeyFrameCollection keyFrames2 = animationUsingKeyFrames1.KeyFrames;
        LinearDoubleKeyFrame linearDoubleKeyFrame1 = new LinearDoubleKeyFrame();
        ((DoubleKeyFrame) linearDoubleKeyFrame1).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.1));
        ((DoubleKeyFrame) linearDoubleKeyFrame1).put_Value(0.0);
        LinearDoubleKeyFrame linearDoubleKeyFrame2 = linearDoubleKeyFrame1;
        ((ICollection<DoubleKeyFrame>) keyFrames2).Add((DoubleKeyFrame) linearDoubleKeyFrame2);
        DoubleKeyFrameCollection keyFrames3 = animationUsingKeyFrames1.KeyFrames;
        LinearDoubleKeyFrame linearDoubleKeyFrame3 = new LinearDoubleKeyFrame();
        ((DoubleKeyFrame) linearDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.2));
        ((DoubleKeyFrame) linearDoubleKeyFrame3).put_Value(1.0);
        LinearDoubleKeyFrame linearDoubleKeyFrame4 = linearDoubleKeyFrame3;
        ((ICollection<DoubleKeyFrame>) keyFrames3).Add((DoubleKeyFrame) linearDoubleKeyFrame4);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames1);
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames2 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames2, (DependencyObject) this._titleTransform);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames2, "Rotation");
        DoubleKeyFrameCollection keyFrames4 = animationUsingKeyFrames2.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame3 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.1));
        ((DoubleKeyFrame) discreteDoubleKeyFrame3).put_Value(0.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame4 = discreteDoubleKeyFrame3;
        ((ICollection<DoubleKeyFrame>) keyFrames4).Add((DoubleKeyFrame) discreteDoubleKeyFrame4);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames2);
        DoubleAnimationUsingKeyFrames animationUsingKeyFrames3 = new DoubleAnimationUsingKeyFrames();
        Storyboard.SetTarget((Timeline) animationUsingKeyFrames3, (DependencyObject) this._titleTransform);
        Storyboard.SetTargetProperty((Timeline) animationUsingKeyFrames3, "TranslateY");
        DoubleKeyFrameCollection keyFrames5 = animationUsingKeyFrames3.KeyFrames;
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame5 = new DiscreteDoubleKeyFrame();
        ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_KeyTime((KeyTime) (TimeSpan) TimeSpan.FromSeconds(0.1));
        ((DoubleKeyFrame) discreteDoubleKeyFrame5).put_Value(0.0);
        DiscreteDoubleKeyFrame discreteDoubleKeyFrame6 = discreteDoubleKeyFrame5;
        ((ICollection<DoubleKeyFrame>) keyFrames5).Add((DoubleKeyFrame) discreteDoubleKeyFrame6);
        ((ICollection<Timeline>) sb.Children).Add((Timeline) animationUsingKeyFrames3);
      }
      await sb.BeginAsync();
      this.CleanupSnapProperties();
    }

    private Rect GetMovableArea()
    {
      if (!(((FrameworkElement) this).Parent is FrameworkElement parent))
        return new Rect();
      double num = this._titleGrid == null || this.WindowEdgeSnapBehavior != WindowEdgeSnapBehavior.StraightToTitleBar ? ((FrameworkElement) this).ActualHeight : ((FrameworkElement) this._titleGrid).ActualHeight + (this._border == null ? 0.0 : this._border.BorderThickness.Top);
      if (this.WindowMovableArea == WindowMovableArea.UseParentBounds)
        return new Rect(-this.WindowMovableAreaEdgeThickness.Left, -this.WindowMovableAreaEdgeThickness.Top, Math.Max(0.0, parent.ActualWidth + this.WindowMovableAreaEdgeThickness.Left + this.WindowMovableAreaEdgeThickness.Right - ((FrameworkElement) this).ActualWidth), Math.Max(0.0, parent.ActualHeight + this.WindowMovableAreaEdgeThickness.Top + this.WindowMovableAreaEdgeThickness.Bottom - num));
      Point position = ((UIElement) parent).GetPosition();
      return !(Window.Current.Content is FrameworkElement content) ? new Rect() : new Rect(-this.WindowMovableAreaEdgeThickness.Left - position.X, -this.WindowMovableAreaEdgeThickness.Top - position.Y, Math.Max(0.0, content.ActualWidth + this.WindowMovableAreaEdgeThickness.Left + this.WindowMovableAreaEdgeThickness.Right - ((FrameworkElement) this).ActualWidth), Math.Max(0.0, content.ActualHeight + this.WindowMovableAreaEdgeThickness.Top + this.WindowMovableAreaEdgeThickness.Bottom - num));
    }

    private enum Edges
    {
      None,
      Top,
      Right,
      Bottom,
      Left,
    }
  }
}
