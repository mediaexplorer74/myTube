// myTube.OverCanvas

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using WinRTXamlToolkit.AwaitableUI;

namespace myTube
{
  public partial class OverCanvas : Panel, IScrollSnapPointsInfo
  {
    public static readonly DependencyProperty ArrangeStyleProperty 
            = DependencyProperty.Register(nameof (ArrangeStyle), typeof (ArrangeStyle), typeof (OverCanvas), 
                new PropertyMetadata((object) ArrangeStyle.Panorama, 
                    new PropertyChangedCallback(OverCanvas.OnArrangeStyeChanged)));
    public static readonly DependencyProperty FlipStyleProperty 
            = DependencyProperty.Register(nameof (FlipStyle), typeof (FlipStyle), typeof (OverCanvas),
                new PropertyMetadata((object) FlipStyle.Classic, 
                    new PropertyChangedCallback(OverCanvas.OnFlipStyleChanged)));
    public static readonly DependencyProperty OverCanvasWidthProperty 
            = DependencyProperty.RegisterAttached("OverCanvasWidth", typeof (double), typeof (UIElement), 
                new PropertyMetadata((object) double.NaN, 
                    new PropertyChangedCallback(OverCanvas.OnOverCanvasWidthChanged)));
    public static readonly DependencyProperty ScrolledToOrAdjacentProperty 
            = DependencyProperty.RegisterAttached("ScrolledToOrAdjacent", typeof (Action), typeof (UIElement), 
                new PropertyMetadata((object) null));
    private static readonly DependencyProperty OverCanvasDesiredWidthProperty 
            = DependencyProperty.RegisterAttached("OverCanvasDesiredWidth", typeof (double), typeof (UIElement), 
                new PropertyMetadata((object) 390.0));
    public static readonly DependencyProperty OverCanvasMaxWidthProperty 
            = DependencyProperty.RegisterAttached("OverCanvasMaxWidth", typeof (double), typeof (UIElement), 
                new PropertyMetadata((object) double.MaxValue));
    public static readonly DependencyProperty HorizontalPaddingProperty 
            = DependencyProperty.Register(nameof (HorizontalPadding), typeof (Thickness), typeof (OverCanvas), 
                new PropertyMetadata((object) new Thickness(19.0, 0.0, 19.0, 0.0)));
    public static readonly DependencyProperty TitleProperty 
            = DependencyProperty.Register(nameof (Title), typeof (string), typeof (OverCanvas),
                new PropertyMetadata((object) null));
    public static readonly DependencyProperty ScrollViewerProperty 
            = DependencyProperty.Register("ScrollViewer", typeof (ScrollViewer), typeof (OverCanvas), 
                new PropertyMetadata((object) null, 
                    new PropertyChangedCallback(OverCanvas.OnScrollViewerChanged)));
    public static readonly DependencyProperty OverCanvasTitleProperty 
            = DependencyProperty.RegisterAttached("OverCanvasTitle", typeof (string), typeof (UIElement), 
                new PropertyMetadata((object) "page"));
    public static readonly DependencyProperty OverCanvasWidthTypeProperty 
            = DependencyProperty.RegisterAttached("OverCanvasWidthType", 
                typeof (OverCanvasWidthType), typeof (UIElement),
                new PropertyMetadata((object) OverCanvasWidthType.Pixel));
    public static readonly DependencyProperty PageTitlesProperty 
            = DependencyProperty.Register(nameof (PageTitles),
                typeof (string[]), typeof (OverCanvas), 
                new PropertyMetadata((object) new string[0]));
    public static readonly DependencyProperty SelectedPageProperty 
            = DependencyProperty.Register(nameof (SelectedPage), typeof (int), typeof (OverCanvas), 
                new PropertyMetadata((object) -2, 
                    new PropertyChangedCallback(OverCanvas.OnSelectedPagePropertyChanged)));
    public static readonly DependencyProperty SelectedIndexProperty 
            = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (OverCanvas),
                new PropertyMetadata((object) 0, 
                    new PropertyChangedCallback(OverCanvas.OnSelectedIndexPropertyChanged)));
    public static readonly DependencyProperty ShownProperty 
            = DependencyProperty.Register(nameof (Shown), typeof (bool), typeof (OverCanvas), 
                new PropertyMetadata((object) true,
                    new PropertyChangedCallback(OverCanvas.OnShownPropertyChanged)));
    public static readonly DependencyProperty VerticalOffsetHelperProperty 
            = DependencyProperty.Register("VerticalOffsetHelper", typeof (double), typeof (OverCanvas),
                new PropertyMetadata((object) 0,
                    new PropertyChangedCallback(OverCanvas.OnVerticalOffsetHelperPropertyChanged)));
    public static readonly DependencyProperty HorizontalOffsetHelperProperty 
            = DependencyProperty.Register("HorizontalOffsetHelper", typeof (double), typeof (OverCanvas), 
                new PropertyMetadata((object) 0, 
                    new PropertyChangedCallback(OverCanvas.OnHorizontalOffsetHelperPropertyChanged)));
    public static readonly DependencyProperty TitleBackgroundBrushProperty 
            = DependencyProperty.Register(nameof (TitleBackgroundBrush), typeof (Brush), typeof (OverCanvas), 
                new PropertyMetadata((object) null, 
                    new PropertyChangedCallback(OverCanvas.OnTitleBackgroundBrushPropertyChanged)));
    public static readonly DependencyProperty TitleForegroundBrushProperty 
            = DependencyProperty.Register(nameof (TitleForegroundBrush), typeof (Brush), typeof (OverCanvas), 
                new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverCanvasShownProperty 
            = DependencyProperty.RegisterAttached("OverCanvasShown", 
                typeof (bool), typeof (UIElement), 
                new PropertyMetadata((object) true, 
                    new PropertyChangedCallback(OverCanvas.OverCanvasShownPropertyChanged)));
    public static readonly DependencyProperty SignedInProperty 
            = DependencyProperty.Register(nameof (SignedIn), typeof (bool), typeof (OverCanvas), 
                new PropertyMetadata((object) null, 
                    new PropertyChangedCallback(OverCanvas.OnSignedInPropertyChanged)));
    public static readonly DependencyProperty BannerReadyProperty 
            = DependencyProperty.Register(nameof (BannerReady), typeof (bool), typeof (OverCanvas), 
                new PropertyMetadata((object) false));
    public static readonly DependencyProperty OverCanvasPageProperty 
            = DependencyProperty.Register("OverCanvasPage", typeof (int), typeof (UIElement), 
                new PropertyMetadata((object) 0, 
                    new PropertyChangedCallback(OverCanvas.OverCanvasPagePropertyChanged)));
    public static readonly DependencyProperty TitleVisibilityProperty
            = DependencyProperty.Register("TitleVisiblity", typeof (Visibility), typeof (OverCanvas), 
                new PropertyMetadata((object) (Visibility) 1));

    private int prevPage = -2;
    private bool firstPageChange;
    private bool isScrolling;
    private double lastOffset;
    private bool resettingSnapPoints;
    private const string Tag = "OverCanvas";
    private bool invalidateAfterArrange;
    public const double MinSizeRemaining = 390.0;
    public const double PaddingForMinSizeRemaining = 47.0;
    public const double ScrollPadding = 12.0;
    private float snapWidth;
    private double leftMargin = 36.0;
    private double rightMargin = 120.0;
    private double rightShowThrough = 48.0;
    private double additionalSeparation;
    private bool waitForScrollLayoutUpdate = true;
    private bool callEvents = true;
    private bool callEventsWeakCheck = true;
    private bool allowManipulation;
    private Func<object> searchParam;
    private bool firstSizeChange = true;
    private List<float> snaps = new List<float>();
    private List<float> childrenSnaps = new List<float>();
    private TranslateTransform scrollTransform;
    private bool moving;
    private double lastSizeX;
    private Size arrangeSize;
    private double snapWidthAtMeasure;

    private static void OnFlipStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
            => (d as OverCanvas).Invalidate();

    private static void OnOverCanvasWidthChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement child))
        return;
      Helper.FindParentFromTree<OverCanvas>(child, 10)?.Invalidate();
    }

    private static void OnSignedInPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      OverCanvas sender = d as OverCanvas;
      if (sender.SignInChanged == null)
        return;
      sender.SignInChanged((object) sender, (bool) e.NewValue);
    }

    private static void OverCanvasShownPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      UIElement Element = d as UIElement;
      if (!(bool) e.NewValue)
      {
        Ani.Begin((DependencyObject) Element, "Opacity", 0.0, 0.25);
        Element.IsHitTestVisible = false;
      }
      else
      {
        Ani.Begin((DependencyObject) Element, "Opacity", 1.0, 0.25);
        Element.IsHitTestVisible = true;
      }
    }

    private static void OnTitleBackgroundBrushPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
    }

    private static void titleBrushOpacityChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      OverCanvas overCanvas = d as OverCanvas;
      if (overCanvas.TitleBackgroundBrush == null)
        return;
      overCanvas.TitleBackgroundBrush.Opacity = (double) e.NewValue;
    }

    private static void OnHorizontalOffsetHelperPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (((DependencyObject) (d as OverCanvas)).GetValue(OverCanvas.ScrollViewerProperty) 
                as ScrollViewer).ScrollToHorizontalOffset((double) e.NewValue);
    }

    private static void OnVerticalOffsetHelperPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (((DependencyObject) (d as OverCanvas)).GetValue(OverCanvas.ScrollViewerProperty) 
                as ScrollViewer).ChangeView(new double?(), new double?((double) e.NewValue), new float?(), true);
    }

    private static void OverCanvasPagePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is FrameworkElement frameworkElement))
        return;
      Action scrolledToOrAdjacent = OverCanvas.GetScrolledToOrAdjacent((DependencyObject) frameworkElement);
      if (scrolledToOrAdjacent == null || !(frameworkElement.Parent is OverCanvas parent))
        return;
      int newValue = (int) e.NewValue;
      int oldValue = (int) e.OldValue;

      if (!parent.IsSelectedOrAdjacent(newValue) || parent.IsSelectedOrAdjacent(oldValue) 
                && !parent.callEventsWeakCheck)
        return;

      parent.callEventsWeakCheck = false;
      scrolledToOrAdjacent();
    }

    private static void OnShownPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      OverCanvas sender = d as OverCanvas;
      if (sender.ShownChanged == null)
        return;
      sender.ShownChanged((object) sender, (bool) e.NewValue);
    }

    private static void OnSelectedPagePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is OverCanvas sender))
        return;
      sender.firstPageChange = true;
      int newValue = (int) e.NewValue;
      sender.prevPage = (int) e.OldValue;
      if (sender.SelectedPageChanged != null)
        sender.SelectedPageChanged((object) sender, new OnSelectedPageChangedEventArgs()
        {
          NewPage = (int) e.NewValue,
          OldPage = (int) e.OldValue
        });
      sender.CallSelectedOrAdjacent();
    }

    public void CallSelectedOrAdjacent()
    {
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        int overCanvasPage = OverCanvas.GetOverCanvasPage((DependencyObject) child);
        if (this.IsSelectedOrAdjacent(overCanvasPage, this.SelectedPage) 
                    && (!this.IsSelectedOrAdjacent(overCanvasPage, this.prevPage) 
                    || !this.firstPageChange || this.callEventsWeakCheck))
        {
          Action scrolledToOrAdjacent = OverCanvas.GetScrolledToOrAdjacent((DependencyObject) child);
          if (scrolledToOrAdjacent != null)
            scrolledToOrAdjacent();
        }
      }
      this.callEventsWeakCheck = false;
    }

    private static void OnSelectedIndexPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
    }

    private static async void OnScrollViewerChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is OverCanvas oc))
        return;
      if (e.OldValue != null)
      {
        ScrollViewer oldValue = e.OldValue as ScrollViewer;
        
        oldValue.ViewChanged -= oc.newScroll_ViewChanged;
      
        oldValue.SizeChanged -= oc.newScroll_SizeChanged;

        Helper.Write((object) "Removed old ScrollViewer events on OverCanvas");
        System.Diagnostics.Debug.WriteLine("Removed old ScrollViewer events on OverCanvas");
      }

      if (oc.waitForScrollLayoutUpdate)
        await ((FrameworkElement) oc).WaitForLayoutUpdateAsync();
      ScrollViewer newValue;
      ScrollViewer scrollViewer1 = newValue = e.NewValue as ScrollViewer;

      scrollViewer1.ViewChanged += oc.newScroll_ViewChanged;

      ScrollViewer scrollViewer2 = newValue;
           
      scrollViewer2.SizeChanged += oc.newScroll_SizeChanged;

      ScrollViewer scrollViewer3 = newValue;
      
      scrollViewer3.GotFocus += oc.newScroll_GotFocus;

      newValue.HorizontalSnapPointsType = ((SnapPointsType) 4);
      newValue.VerticalScrollMode = ((ScrollMode) 0);
      newValue.HorizontalScrollMode = ((ScrollMode) 1);
      newValue.HorizontalScrollBarVisibility = ((ScrollBarVisibility) 2);
      newValue.VerticalScrollBarVisibility = ((ScrollBarVisibility) 0);
      newValue.ZoomMode = ((ZoomMode) 0);
      int currentIteration = 0;
      Border child = Helper.FindChild<Border>((DependencyObject) newValue, 100, ref currentIteration);
      if (child != null)
      {
        VisualStateGroup visualStateGroup1 = Enumerable.FirstOrDefault<VisualStateGroup>(
            (IEnumerable<VisualStateGroup>) VisualStateManager.GetVisualStateGroups(
                (FrameworkElement) child));
        if (visualStateGroup1 != null)
        {
          VisualStateGroup visualStateGroup2 = visualStateGroup1;

          visualStateGroup2.CurrentStateChanged += oc.groups_CurrentStateChanged;
        }
      }
      oc.invalidateAfterArrange = true;

      Helper.Write((object) nameof (OverCanvas), 
          (object) "Added new ScrollViewer events to OverCanvas");
    }

    private void newScroll_GotFocus(object sender, RoutedEventArgs e)
    {
      ScrollViewer scrollViewer = 
                (ScrollViewer) ((DependencyObject) this).GetValue(OverCanvas.ScrollViewerProperty);
    }

    public bool IsScrolling => this.isScrolling;

    private void groups_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
    {
      if (e.NewState.Name == "NoIndicator" || e.NewState.Name == "MouseIndicator")
      {
        this.isScrolling = false;
        if (this.ScrollingStopped == null)
          return;
        this.ScrollingStopped((object) this, new OnScrollingStoppedEventArgs()
        {
          Page = this.SelectedPage
        });
      }
      else
        this.isScrolling = true;
    }

    private void newScroll_SizeChanged(object sender, SizeChangedEventArgs e)
    {
    }

    private void newScroll_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
    {
      ScrollViewerDisplayClass CScrollViewerDisplayClass = new OverCanvas.ScrollViewerDisplayClass();
      CScrollViewerDisplayClass.item = this;
      CScrollViewerDisplayClass.s = sender as ScrollViewer;
      if (CScrollViewerDisplayClass.s == null || !e.IsIntermediate)
        return;
      
      double num = this.lastOffset - CScrollViewerDisplayClass.s.HorizontalOffset;
      this.lastOffset = CScrollViewerDisplayClass.s.HorizontalOffset;
      
      if (CScrollViewerDisplayClass.s.HorizontalSnapPointsType != SnapPointsType.MandatorySingle 
                && !this.resettingSnapPoints && Math.Abs(num) < 2.5)
      {
        this.resettingSnapPoints = true;
        
       
        ThreadPoolTimer timer = ThreadPoolTimer.CreateTimer(async (timer1) =>
        {
            //RnD
            //await CScrollViewerDisplayClass.s.ViewChanged(CScrollViewerDisplayClass.s, e);//CnewScroll_ViewChanged_u003Eb__0();
        }, TimeSpan.FromMilliseconds(200.0));
       }
       this.SetIndexBasedOnScroll(CScrollViewerDisplayClass.s.HorizontalOffset);
    }

    public static bool GetOverCanvasShown(DependencyObject obj) 
            => (bool) obj.GetValue(OverCanvas.OverCanvasShownProperty);

    public static void SetOverCanvasShown(DependencyObject obj, bool value) 
            => obj.SetValue(OverCanvas.OverCanvasShownProperty, (object) value);

    public static int GetOverCanvasPage(DependencyObject obj) 
            => (int) obj.GetValue(OverCanvas.OverCanvasPageProperty);

    public static void SetOverCanvasPage(DependencyObject obj, int value) 
            => obj.SetValue(OverCanvas.OverCanvasPageProperty, (object) value);

    public static OverCanvasWidthType GetOverCanvasWidthType(DependencyObject obj) 
            => (OverCanvasWidthType) obj.GetValue(OverCanvas.OverCanvasWidthTypeProperty);

    public static void SetOverCanvasWidthType(DependencyObject obj, OverCanvasWidthType value) 
            => obj.SetValue(OverCanvas.OverCanvasWidthTypeProperty, (object) value);

    public static double GetOverCanvasWidth(DependencyObject obj) 
            => (double) obj.GetValue(OverCanvas.OverCanvasWidthProperty);

    public static void SetOverCanvasWidth(DependencyObject obj, double value) 
            => obj.SetValue(OverCanvas.OverCanvasWidthProperty, (object) value);

    public static double GetOverCanvasMaxWidth(DependencyObject obj) 
            => (double) obj.GetValue(OverCanvas.OverCanvasMaxWidthProperty);

    public static void SetOverCanvasMaxWidth(DependencyObject obj, double value) 
            => obj.SetValue(OverCanvas.OverCanvasMaxWidthProperty, (object) value);

    public static string GetOverCanvasTitle(DependencyObject obj) 
            => (string) obj.GetValue(OverCanvas.OverCanvasTitleProperty);

    public static void SetOverCanvasTitle(DependencyObject obj, string value) 
            => obj.SetValue(OverCanvas.OverCanvasTitleProperty, (object) value);

    public static double GetHorizontalOffsetHelper(DependencyObject obj) 
            => (double) obj.GetValue(OverCanvas.HorizontalOffsetHelperProperty);

    public static void SetHorizontalOffsetHelper(DependencyObject obj, double value) 
            => obj.SetValue(OverCanvas.HorizontalOffsetHelperProperty, (object) value);

    public static Action GetScrolledToOrAdjacent(DependencyObject obj) 
            => (Action) obj.GetValue(OverCanvas.ScrolledToOrAdjacentProperty);

    public static void SetScrolledToOrAdjacent(DependencyObject obj, Action value) 
            => obj.SetValue(OverCanvas.ScrolledToOrAdjacentProperty, (object) value);

    private static void OnArrangeStyeChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      (d as OverCanvas).Invalidate();
    }

    public void Invalidate() => ((UIElement) this).InvalidateMeasure();

    public FlipStyle FlipStyle
    {
      get => (FlipStyle) ((DependencyObject) this).GetValue(OverCanvas.FlipStyleProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.FlipStyleProperty, (object) value);
    }

    public ArrangeStyle ArrangeStyle
    {
      get => (ArrangeStyle) ((DependencyObject) this).GetValue(OverCanvas.ArrangeStyleProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.ArrangeStyleProperty, (object) value);
    }

    public string Title
    {
      get => (string) ((DependencyObject) this).GetValue(OverCanvas.TitleProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.TitleProperty, (object) value);
    }

    public int SelectedPage
    {
      get => (int) ((DependencyObject) this).GetValue(OverCanvas.SelectedPageProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.SelectedPageProperty, (object) value);
    }

    public int SelectedIndex
    {
      get => (int) ((DependencyObject) this).GetValue(OverCanvas.SelectedIndexProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.SelectedIndexProperty, (object) value);
    }

    public bool Shown
    {
      get => (bool) ((DependencyObject) this).GetValue(OverCanvas.ShownProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.ShownProperty, (object) value);
    }

    public Thickness HorizontalPadding
    {
      get => (Thickness) ((DependencyObject) this).GetValue(OverCanvas.HorizontalPaddingProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.HorizontalPaddingProperty, (object) value);
    }

    public bool WaitForScrollLayoutUpdate
    {
      get => this.waitForScrollLayoutUpdate;
      set => this.waitForScrollLayoutUpdate = value;
    }

    public bool CallEventsOnDataContextChanged
    {
      get => this.callEvents;
      set => this.callEvents = value;
    }

    public bool BannerReady
    {
      get => (bool) ((DependencyObject) this).GetValue(OverCanvas.BannerReadyProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.BannerReadyProperty, (object) value);
    }

    public event EventHandler<bool> ShownChanged;

    public event EventHandler<OnScrollingStoppedEventArgs> ScrollingStopped;

    public Visibility TitleVisibility
    {
      get => (Visibility) ((DependencyObject) this).GetValue(OverCanvas.TitleVisibilityProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.TitleVisibilityProperty, (object) value);
    }

    public string[] PageTitles
    {
      get => (string[]) ((DependencyObject) this).GetValue(OverCanvas.PageTitlesProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.PageTitlesProperty, (object) value);
    }

    public Brush TitleBackgroundBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(OverCanvas.TitleBackgroundBrushProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.TitleBackgroundBrushProperty, (object) value);
    }

    public Brush TitleForegroundBrush
    {
      get => (Brush) ((DependencyObject) this).GetValue(OverCanvas.TitleForegroundBrushProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.TitleForegroundBrushProperty, (object) value);
    }

    public event EventHandler<bool> SignInChanged;

    public event EventHandler<OnSelectedPageChangedEventArgs> SelectedPageChanged;

    public bool SignedIn
    {
      get => (bool) ((DependencyObject) this).GetValue(OverCanvas.SignedInProperty);
      set => ((DependencyObject) this).SetValue(OverCanvas.SignedInProperty, (object) value);
    }

    public List<float> Snaps => this.snaps;

    public OverCanvas()
    {
        // These lines of code add 3 (or 4?) event handlers to the different events of the FrameworkElement object.
        // When these events are raised, the corresponding methods will be executed.
        SizeChanged += OverCanvas_SizeChanged;
        DataContextChanged += OverCanvas_DataContextChanged;
        Loaded += OverCanvas_Loaded;
        Unloaded += OverCanvas_Unloaded;
    }

    public void SetSearchParamFunction(Func<object> searchParamFunc) => this.searchParam = searchParamFunc;

    public object GetSearchParam() => this.searchParam != null ? this.searchParam() : (object) null;

    private void OverCanvas_Unloaded(object sender, RoutedEventArgs e) => this.callEventsWeakCheck = true;

    private async void OverCanvas_Loaded(object sender, RoutedEventArgs e)
    {
    }

    private async void OverCanvas_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!this.callEvents)
        return;
      await ((FrameworkElement) this).WaitForLayoutUpdateAsync();
      this.callEventsWeakCheck = true;
      this.CallSelectedOrAdjacent();
    }

    public bool IsSelectedOrAdjacent(UIElement el) => ((ICollection<UIElement>) this.Children).Contains(el) && this.IsSelectedOrAdjacent(OverCanvas.GetOverCanvasPage((DependencyObject) el));

    public bool IsSelectedOrAdjacent(int index) => this.IsSelectedOrAdjacent(index, this.SelectedPage);

    private bool IsSelectedOrAdjacent(int index, int selectedPage) => index >= 0 && Math.Abs(index - selectedPage) <= 1;

    public bool IsSelected(UIElement el) => this.IsSelected(OverCanvas.GetOverCanvasPage((DependencyObject) el));

    public bool IsSelected(int pageIndex) => this.IsSelected(pageIndex, this.SelectedPage);

    private bool IsSelected(int pageIndex, int selectedPage) => pageIndex >= 0 && pageIndex == selectedPage;

    private async void OverCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      ScrollViewer scrollViewer = (ScrollViewer) ((DependencyObject) this).GetValue(OverCanvas.ScrollViewerProperty);
      if (!(e.PreviousSize != e.NewSize))
        return;
      if (this.Shown)
      {
        if (((ICollection<UIElement>) this.Children).Count > this.SelectedIndex && (e.NewSize.Width != e.PreviousSize.Width || !this.isScrolling))
          this.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) ((IList<UIElement>) this.Children)[this.SelectedIndex]), true);
      }
      else if (this.SelectedPage == -1)
        this.ScrollToPage(-1, true);
      else
        this.ScrollToPage(this.snaps.Count - 2, true);
      this.firstSizeChange = false;
    }

    private void SetIndexBasedOnScroll(double scroll)
    {
      int selectedPage = this.SelectedPage;
      int num1 = 0;
      double num2 = 100000000.0;
      for (int index = 0; index < this.snaps.Count; ++index)
      {
        double num3 = Math.Abs((double) this.snaps[index] - scroll);
        if (num3 < num2)
        {
          num2 = num3;
          num1 = index;
        }
      }
      bool flag = this.SelectedPage != num1 - 1 || !this.Shown;
      if (this.snaps.Count <= 0)
        return;
      ScrollViewer scrollViewer = (ScrollViewer) ((DependencyObject) this).GetValue(OverCanvas.ScrollViewerProperty);
      if (scrollViewer != null)
      {
        if (this.FlipStyle == FlipStyle.Classic & flag)
        {
          if (this.snaps.Count > 1 && scrollViewer.HorizontalOffset > (double) this.snaps[this.snaps.Count - 1] + 2.0)
            scrollViewer.ScrollToHorizontalOffset((double) this.snaps[0] + 2.0);
          else if (this.snaps.Count > 0 && scrollViewer.HorizontalOffset < (double) this.snaps[0] - 2.0)
            scrollViewer.ScrollToHorizontalOffset((double) this.snaps[this.snaps.Count - 1] - 2.0);
        }
        else if (this.FlipStyle == FlipStyle.Pivot)
        {
          double Dec = 0.4;
          if (this.snaps.Count > 2 && scroll > (double) this.snaps[this.snaps.Count - 2])
          {
            double Val1 = MyMath.Between((double) this.snaps[this.snaps.Count - 2], (double) this.snaps[this.snaps.Count - 1], Dec) + 2.0;
            if (scrollViewer.HorizontalOffset > Val1)
            {
              scrollViewer.ScrollToHorizontalOffset(MyMath.Between((double) this.snaps[1], (double) this.snaps[0], Dec));
            }
            else
            {
              double o = MyMath.BetweenValue(Val1, (double) this.snaps[this.snaps.Count - 2], scroll);
              if (((UIElement) scrollViewer).Opacity != o)
                ((UIElement) scrollViewer).Opacity = o;
              Helper.Write((object) o);
            }
          }
          else if (this.snaps.Count > 2 && scroll < (double) this.snaps[1])
          {
            double Val1 = MyMath.Between((double) this.snaps[1], (double) this.snaps[0], Dec) - 2.0;
            if (scrollViewer.HorizontalOffset < Val1)
            {
              scrollViewer.ScrollToHorizontalOffset(MyMath.Between((double) this.snaps[this.snaps.Count - 2], (double) this.snaps[this.snaps.Count - 1], Dec));
            }
            else
            {
              double o = MyMath.BetweenValue(Val1, (double) this.snaps[1], scroll);
              ((UIElement) scrollViewer).Opacity = o;
              Helper.Write((object) o);
            }
          }
          else
            ((UIElement) scrollViewer).Opacity = 1.0;
        }
      }
      if (!flag)
        return;
      if (num1 == 0 || num1 == this.snaps.Count - 1)
      {
        int num4 = this.Shown ? 1 : 0;
        this.Shown = false;
        this.SelectedPage = num1 - 1;
      }
      else
      {
        this.Shown = true;
        this.SelectedPage = num1 - 1;
        for (int index = 0; index < ((ICollection<UIElement>) this.Children).Count; ++index)
        {
          if (OverCanvas.GetOverCanvasPage((DependencyObject) ((IList<UIElement>) this.Children)[index]) == this.SelectedPage)
          {
            this.SelectedIndex = index;
            break;
          }
        }
      }
    }

    public void Move(double pixels)
    {
      if ((ScrollViewer) ((DependencyObject) this).GetValue(OverCanvas.ScrollViewerProperty) == null)
        return;
      this.moving = true;
      if (this.scrollTransform == null)
      {
        this.scrollTransform = new TranslateTransform();
        ((UIElement) this).RenderTransform = (Transform) this.scrollTransform;
      }
      TranslateTransform scrollTransform = this.scrollTransform;
      scrollTransform.X = scrollTransform.X + pixels;
    }

    public void EndMove()
    {
      if (!this.moving)
        return;
      if (this.scrollTransform != null)
      {
        if (this.scrollTransform.X > 50.0)
          this.ScrollToPage(this.SelectedPage - 1, false, easeInOut: false);
        else if (this.scrollTransform.X < -50.0)
          this.ScrollToPage(this.SelectedPage + 1, false, easeInOut: false);
        else
          this.ScrollToPage(this.SelectedPage, false, easeInOut: false);
      }
      else
        this.ScrollToPage(this.SelectedPage, false, easeInOut: false);
      this.moving = false;
    }

    public void ScrollToIndex(int index, bool cancelAnimations, bool usePropertyAnimation = false)
    {
      if (((ICollection<UIElement>) this.Children).Count > index)
        this.ScrollToPage(OverCanvas.GetOverCanvasPage((DependencyObject) 
            ((IList<UIElement>) this.Children)[index]), cancelAnimations, usePropertyAnimation);
      else
        this.ScrollToPage(0, cancelAnimations);
    }

    public async void ScrollToPage(
      int page,
      bool cancelAnimations,
      bool usePropertyAnimation = false,
      bool easeInOut = true)
    {
      if (this.snaps.Count <= 0)
        return;
      if (this.scrollTransform == null)
        this.scrollTransform = new TranslateTransform();
      ((UIElement) this).RenderTransform = (Transform) this.scrollTransform;
      page = (page + 1) % this.snaps.Count;
      ScrollViewer scroll = (ScrollViewer) ((DependencyObject) this).GetValue(OverCanvas.ScrollViewerProperty);
      ExponentialEase Ease1 = easeInOut ? Ani.Ease((EasingMode) 2, 5.0) : Ani.Ease((EasingMode) 0, 6.0);
      if (!easeInOut)
        Ani.Ease((EasingMode) 0, 3.0);
      else
        Ani.Ease((EasingMode) 2, 3.0);
      ExponentialEase Ease2 = Ani.Ease((EasingMode) 1, 5.0);
      ExponentialEase easeShortOut = Ani.Ease((EasingMode) 0, 5.0);
      double Duration = 0.4;
      double timeShort = 0.25;
      if (!cancelAnimations)
      {
        if (this.SelectedPage == -1 && page == -1)
        {
          page = this.snaps.Count - 2;
          this.scrollTransform.X = scroll.HorizontalOffset - (double) this.snaps[this.snaps.Count - 1];
        }
        else if (this.SelectedPage == this.snaps.Count - 2 && page == 0 && this.snaps.Count > 1)
        {
          page = 1;
          this.scrollTransform.X = scroll.HorizontalOffset - 0.0;
        }
      }
      if (scroll == null || this.snaps.Count <= 0)
        return;
      if (page == 0)
      {
        if (this.scrollTransform == null)
          this.scrollTransform = new TranslateTransform();
        ((UIElement) this).RenderTransform = (Transform) this.scrollTransform;
        page %= this.snaps.Count;
        if (!cancelAnimations)
        {
          this.SetIndexBasedOnScroll((double) this.snaps[page]);
          if (this.FlipStyle == FlipStyle.Classic)
          {
            Storyboard storyboard = Ani.Begin((DependencyObject) this.scrollTransform, "X", scroll.HorizontalOffset - (double) this.snaps[page] + 2.0, Duration, (EasingFunctionBase) Ease1);
           
            storyboard.Completed += (sender, args) =>
            {
                page %= this.snaps.Count;
                scroll.ChangeView(new double?((double)this.snaps[page]), new double?(), new float?(), true);
                this.scrollTransform.X = 0.0;
                ((UIElement)this).RenderTransform = null;
            };
          }
          else
          {
            if (this.FlipStyle != FlipStyle.Pivot)
              return;
            page = this.snaps.Count - 2;
            this.SetIndexBasedOnScroll((double) this.snaps[page]);
            Storyboard storyboard1 = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.scrollTransform, 
                "X", (double) this.snapWidth / 2.0, timeShort, (EasingFunctionBase) Ease2), 
                (Timeline) Ani.DoubleAni((DependencyObject) this, "Opacity", 0.0, timeShort));
            
            storyboard1.Completed += (sender, args) =>
            {
                this.scrollTransform.X = scroll.HorizontalOffset - (double)this.snaps[page] - (double)this.snapWidth / 2.0;
                Storyboard storyboard2 = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.scrollTransform, "X", 
                    scroll.HorizontalOffset - (double)this.snaps[page], timeShort, (EasingFunctionBase)easeShortOut), 
                    (Timeline)Ani.DoubleAni((DependencyObject)this, "Opacity", 1.0, timeShort));
                storyboard2.Completed += (sender1, args1) =>
                {
                    page %= this.snaps.Count;
                    scroll.ChangeView(new double?((double)this.snaps[page]), new double?(), new float?(), true);
                    this.scrollTransform.X = 0.0;
                    ((UIElement)this).RenderTransform = null;
                };
            };
        }
        }
        else
        {
          page %= this.snaps.Count;
          scroll.ChangeView(new double?((double) this.snaps[page]), new double?(), new float?(), true);
          this.SetIndexBasedOnScroll((double) this.snaps[page]);
          this.scrollTransform.X = 0.0;
          ((UIElement) this).RenderTransform = (Transform) null;
        }
      }
      else if (page > 0 && this.snaps.Count - 1 > page)
      {
        if (!cancelAnimations | usePropertyAnimation)
        {
          if (this.scrollTransform == null)
            this.scrollTransform = new TranslateTransform();
          ((UIElement) this).RenderTransform = (Transform) this.scrollTransform;
          page %= this.snaps.Count;
          this.SetIndexBasedOnScroll((double) this.snaps[page]);
          Storyboard storyboard = Ani.Begin((DependencyObject) this.scrollTransform, "X", 
              scroll.HorizontalOffset - (double) this.snaps[page], Duration, (EasingFunctionBase) Ease1);

       
          storyboard.Completed += (sender, args) =>
          {
            page %= this.snaps.Count;
            scroll.ChangeView(new double?((double)this.snaps[page]), new double?(), new float?(), true);
            this.SetIndexBasedOnScroll((double)this.snaps[page]);
            this.scrollTransform.X = 0.0;
            ((UIElement)this).RenderTransform = null;
          };
        }
        else
        {
          page %= this.snaps.Count;
          scroll.ChangeView(new double?((double) this.snaps[page]), new double?(), new float?(), true);
          this.SetIndexBasedOnScroll((double) this.snaps[page]);
          ((UIElement) this).RenderTransform = (Transform) null;
        }
      }
      else
      {
        page = this.snaps.Count - 1;
        if (this.scrollTransform == null)
        {
          this.scrollTransform = new TranslateTransform();
          ((UIElement) this).RenderTransform = (Transform) this.scrollTransform;
        }
        if (!cancelAnimations)
        {
          this.SetIndexBasedOnScroll((double) this.snaps[page]);
          if (this.FlipStyle == FlipStyle.Classic)
          {
            Storyboard storyboard = Ani.Begin((DependencyObject) this.scrollTransform, "X", 
                scroll.HorizontalOffset - (double) this.snaps[this.snaps.Count - 1] - 2.0, Duration, 
                (EasingFunctionBase) Ease1);
            
            ((Timeline)storyboard).Completed += (s, e) =>
            {
                page %= this.snaps.Count;
                scroll.ChangeView(new double?((double)this.snaps[page]), new double?(), new float?(), true);
                this.scrollTransform.X = 0.0;
                ((UIElement)this).RenderTransform = null;
            };

          }
          else
          {
            if (this.FlipStyle != FlipStyle.Pivot)
              return;
            page = 1;
            this.SetIndexBasedOnScroll((double) this.snaps[page]);
            Storyboard storyboard3 = Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) this.scrollTransform, 
                "X", -(double) this.snapWidth / 2.0, timeShort, (EasingFunctionBase) Ease2),
                (Timeline) Ani.DoubleAni((DependencyObject) this, "Opacity", 0.0, timeShort));

            storyboard3.Completed += (s, e) =>
            {
                this.scrollTransform.X = scroll.HorizontalOffset - (double)this.snaps[page] + (double)this.snapWidth / 2.0;
                Storyboard storyboard4 = Ani.Begin((Timeline)Ani.DoubleAni((DependencyObject)this.scrollTransform, "X", scroll.HorizontalOffset - (double)this.snaps[page], timeShort, (EasingFunctionBase)easeShortOut), (Timeline)Ani.DoubleAni((DependencyObject)this, "Opacity", 1.0, timeShort));
                storyboard4.Completed += (s2, e2) =>
                {
                    page %= this.snaps.Count;
                    scroll.ChangeView(new double?((double)this.snaps[page]), new double?(), new float?(), true);
                    this.scrollTransform.X = 0.0;
                    ((UIElement)this).RenderTransform = null;
                };
            };
          }
        }
        else
        {
          page %= this.snaps.Count;
          scroll.ChangeView(new double?((double) this.snaps[page]), new double?(), new float?(), true);
          this.scrollTransform.X = 0.0;
          ((UIElement) this).RenderTransform = (Transform) null;
        }
      }
    }

    private void CalculateSnapWidth()
    {
      float num;
      try
      {
        FrameworkElement parent1 = (FrameworkElement) VisualTreeHelper.GetParent((DependencyObject) this);
        if (parent1 != null)
        {
          num = (float) parent1.ActualWidth;
          if ((double) num == 0.0)
            num = (float) App.VisibleBounds.Width;
          if (((DependencyObject) this).GetValue(OverCanvas.ScrollViewerProperty) == null)
          {
            ScrollViewer parent2 = Helper.FindParent<ScrollViewer>((FrameworkElement) this, 10);
            if (parent2 != null)
              ((DependencyObject) this).SetValue(OverCanvas.ScrollViewerProperty, (object) parent2);
          }
        }
        else
          num = (float) App.VisibleBounds.Width;
      }
      catch
      {
        num = (float) App.VisibleBounds.Width;
      }
      this.snapWidth = num;
      switch (this.ArrangeStyle)
      {
        case ArrangeStyle.Pivot:
          this.leftMargin = this.rightMargin = this.rightShowThrough = 95.0;
          this.additionalSeparation = 144.0;
          if ((double) this.snapWidth < 900.0)
          {
            this.additionalSeparation = 95.0;
            this.leftMargin = this.rightMargin = this.rightShowThrough = 19.0;
          }
          if ((double) this.snapWidth < 700.0)
          {
            this.additionalSeparation = 95.0;
            this.leftMargin = this.rightMargin = this.rightShowThrough = 19.0;
          }
          if ((double) this.snapWidth < 350.0)
          {
            this.additionalSeparation = 47.5;
            this.leftMargin = this.rightMargin = this.rightShowThrough = 12.0;
            break;
          }
          break;
        case ArrangeStyle.PivotTouching:
          this.leftMargin = this.rightMargin = this.rightShowThrough = this.additionalSeparation = 0.0;
          break;
        default:
          this.leftMargin = 47.5;
          this.rightMargin = 123.5;
          this.rightShowThrough = 47.5;
          this.additionalSeparation = 0.0;
          if ((double) this.snapWidth < 700.0)
          {
            this.leftMargin = 19.0;
            this.rightMargin = 38.0;
            this.rightShowThrough = 9.5;
          }
          if ((double) this.snapWidth < 330.0)
          {
            this.leftMargin = 9.5;
            this.rightMargin = 9.5;
            this.rightShowThrough = 0.0;
            this.additionalSeparation = 47.5;
            break;
          }
          break;
      }
      this.HorizontalPadding = new Thickness(this.leftMargin, 0.0, this.rightMargin, 0.0);
    }

    protected virtual Size ArrangeOverride(Size finalSize)
    {
      double num1 = (double) this.snapWidth + 12.0;
      this.snaps.Clear();
      this.childrenSnaps.Clear();
      this.snaps.Add(12f);
      bool flag = false;
      double num2 = 0.0;
      int num3 = 0;
      List<string> stringList = new List<string>();
      string str = "";
      for (int index = 0; index < ((ICollection<UIElement>) this.Children).Count; ++index)
      {
        if (((IList<UIElement>) this.Children)[index] is FrameworkElement child 
           && ((UIElement) child).Visibility != Visibility.Collapsed)
        {
          double num4 = (double) this.snapWidth - this.rightMargin;
          double width = num4 - this.leftMargin - num2;
          double d = (double) ((DependencyObject) child).GetValue(OverCanvas.OverCanvasDesiredWidthProperty);
          if (!double.IsNaN(d))
          {
            if (width - d + 47.0 >= 390.0)
            {
              width = d;
              flag = true;
            }
            else
              flag = false;
          }
          else
            flag = false;
          string overCanvasTitle;
          if ((overCanvasTitle = OverCanvas.GetOverCanvasTitle((DependencyObject) child)) != null)
            str += overCanvasTitle;
          Rect rect = new Rect(num1 + this.leftMargin + num2, 0.0, width, finalSize.Height);
          ((UIElement) child).Arrange(rect);
          OverCanvas.SetOverCanvasPage((DependencyObject) child, num3);
          if (!this.snaps.Contains((float) num1))
            this.snaps.Add((float) num1);
          if (!this.childrenSnaps.Contains((float) num1))
            this.childrenSnaps.Add((float) num1);
          if (!flag)
          {
            num2 = 0.0;
            ++num3;
            num1 += num4 + (this.rightMargin - this.leftMargin) + this.additionalSeparation - this.rightShowThrough;
            stringList.Add(str);
            str = "";
          }
          else
          {
            num2 += width + 47.0;
            str += " + ";
          }
        }
      }
      if (flag)
        num1 += (double) this.snapWidth - this.rightMargin + (this.rightMargin - this.leftMargin) + this.additionalSeparation - this.rightShowThrough;
      this.snaps.Add((float) num1);
      double snapWidth = (double) this.snapWidth;
      if (this.lastSizeX != finalSize.Width)
        this.lastSizeX = finalSize.Width;
      if (!(((DependencyObject) this).GetValue(OverCanvas.PageTitlesProperty) is string[] strArray))
        ((DependencyObject) this).SetValue(OverCanvas.PageTitlesProperty, (object) stringList.ToArray());
      else if (strArray.Length != stringList.Count)
      {
        ((DependencyObject) this).SetValue(OverCanvas.PageTitlesProperty, (object) stringList.ToArray());
      }
      else
      {
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index] != stringList[index])
          {
            ((DependencyObject) this).SetValue(OverCanvas.PageTitlesProperty, (object) stringList.ToArray());
            break;
          }
        }
      }
      return this.arrangeSize;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      if (double.IsInfinity(availableSize.Height) || double.IsPositiveInfinity(availableSize.Height))
        availableSize.Height = Window.Current.Bounds.Height;
      Size size1 = new Size();
      size1.Height = availableSize.Height;
      this.CalculateSnapWidth();
      this.snapWidthAtMeasure = (double) this.snapWidth;
      double num1 = 0.0;
      for (int index = 0; index < ((ICollection<UIElement>) this.Children).Count; ++index)
      {
        if (((IList<UIElement>) this.Children)[index] is FrameworkElement child 
                    && ((UIElement) child).Visibility != Visibility.Collapsed)
        {
          Size size2 = new Size();
          size2.Height = availableSize.Height;
          double val1 = (double) this.snapWidth - this.rightMargin - this.leftMargin - num1;
          size2.Width = Math.Min(val1, availableSize.Width);
          bool flag;
          if (!double.IsNaN((double) ((DependencyObject) child).GetValue(OverCanvas.OverCanvasWidthProperty)))
          {
            double num2 = this.getDesiredWidth(size2.Width, child, index);
            double overCanvasMaxWidth = OverCanvas.GetOverCanvasMaxWidth((DependencyObject) child);
            if (!double.IsNaN(overCanvasMaxWidth) && num2 > overCanvasMaxWidth)
              num2 = overCanvasMaxWidth;
            if (size2.Width - num2 + 47.0 >= 390.0)
            {
              size2.Width = num2;
              num1 += size2.Width + 47.0;
              flag = true;
            }
            else
            {
              num1 = 0.0;
              flag = false;
            }
          }
          else
          {
            num1 = 0.0;
            flag = false;
          }
          if (((UIElement) child).DesiredSize != size2)
          {
            ((DependencyObject) child).SetValue(OverCanvas.OverCanvasDesiredWidthProperty, (object) size2.Width);
            ((UIElement) child).Measure(size2);
          }
          Size desiredSize = ((UIElement) child).DesiredSize;
          if (!flag)
            size1.Width += (double) this.snapWidth + this.additionalSeparation;
        }
      }
      this.arrangeSize = new Size(size1.Width + (double) this.snapWidth * 2.0 + 24.0, double.IsNaN(availableSize.Height) || double.IsInfinity(availableSize.Height) ? size1.Height : availableSize.Height);
      return this.arrangeSize;
    }

    private double getDesiredWidth(double widthToMeasure, FrameworkElement child, int childIndex)
    {
      double d1 = (double) ((DependencyObject) child).GetValue(OverCanvas.OverCanvasWidthProperty);
      double desiredWidth1;
      if (!double.IsNaN(d1) && OverCanvas.GetOverCanvasWidthType((DependencyObject) child) == OverCanvasWidthType.Star)
      {
        double desiredWidth2 = this.getDesiredWidth(widthToMeasure, child);
        if (d1 < 0.0)
          d1 = 1.0 - Math.Abs(d1);
        double num1 = d1;
        double num2 = widthToMeasure - desiredWidth2;
        for (int index = childIndex + 1; index < ((ICollection<UIElement>) this.Children).Count; ++index)
        {
          FrameworkElement child1 = ((IList<UIElement>) this.Children)[index] as FrameworkElement;
          if (OverCanvas.GetOverCanvasWidthType((DependencyObject) child1) == OverCanvasWidthType.Star)
          {
            double d2 = OverCanvas.GetOverCanvasWidth((DependencyObject) child1);
            if (d2 < 0.0)
              d2 = 1.0 - Math.Abs(d2);
            double desiredWidth3 = this.getDesiredWidth(widthToMeasure, child1);
            num2 -= desiredWidth3 + 47.0;
            if (!double.IsNaN(d2))
              num1 += d2;
            if (num2 < 390.0)
              break;
          }
          else
          {
            num1 = 1.0;
            break;
          }
        }
        desiredWidth1 = ((double) this.snapWidth - this.rightMargin - this.leftMargin - 47.0) * (d1 / num1);
        if (desiredWidth1 < 343.0)
          desiredWidth1 = 343.0;
      }
      else
        desiredWidth1 = this.getDesiredWidth(widthToMeasure, child);
      return desiredWidth1;
    }

    private double getDesiredWidth(double widthToMeasure, FrameworkElement child)
    {
      double d = (double) ((DependencyObject) child).GetValue(OverCanvas.OverCanvasWidthProperty);
      if (!double.IsNaN(d))
      {
        if (OverCanvas.GetOverCanvasWidthType((DependencyObject) child) == OverCanvasWidthType.Star)
        {
          if (d < 0.0)
            d = 1.0 - Math.Abs(d);
          d *= (double) this.snapWidth - this.rightMargin - this.leftMargin - 47.0;
        }
        else if (d < 0.0)
          d = Math.Max(widthToMeasure + d - 47.0, 390.0);
        if (d < 343.0)
          d = 343.0;
      }
      else
        d = widthToMeasure;
      double overCanvasMaxWidth = OverCanvas.GetOverCanvasMaxWidth((DependencyObject) child);
      if (!double.IsNaN(overCanvasMaxWidth) && d > overCanvasMaxWidth)
        d = overCanvasMaxWidth;
      return d;
    }

    public bool AreHorizontalSnapPointsRegular => false;

    public bool AreVerticalSnapPointsRegular => true;

    public IReadOnlyList<float> GetIrregularSnapPoints(
      Orientation orientation,
      SnapPointsAlignment alignment)
    {
      if (orientation != Orientation.Horizontal)
        return (IReadOnlyList<float>) new List<float>();
      int flipStyle = (int) this.FlipStyle;
      return (IReadOnlyList<float>) this.snaps;
    }

    public float GetRegularSnapPoints(
      Orientation orientation,
      SnapPointsAlignment alignment,
      out float offset)
    {
      offset = orientation != Orientation.Horizontal ? this.snapWidth : this.snapWidth;
      return offset;
    }

        //TODO
        /*
    public event EventHandler<object> HorizontalSnapPointsChanged
        {
           add
            {
                EventRegistrationTokenTable<EventHandler<object>>.GetOrCreateEventRegistrationTokenTable(
                    ref this.HorizontalSnapPointsChanged).AddEventHandler(value);
            }

            remove
            {
                EventRegistrationTokenTable<EventHandler<object>>.GetOrCreateEventRegistrationTokenTable(
                    ref this.HorizontalSnapPointsChanged).RemoveEventHandler(value);
            }
     
        }*/

        /*
        public event EventHandler<object> VerticalSnapPointsChanged
        {
            add
            {
                EventRegistrationTokenTable<EventHandler<object>>.GetOrCreateEventRegistrationTokenTable(ref this.VerticalSnapPointsChanged).AddEventHandler(value);

            }

            remove
            {
                EventRegistrationTokenTable<EventHandler<object>>.GetOrCreateEventRegistrationTokenTable(ref this.VerticalSnapPointsChanged).RemoveEventHandler(value);
            }   
        }*/

        private class ScrollViewerDisplayClass
        {
            internal OverCanvas item;
            internal ScrollViewer s;

            public ScrollViewerDisplayClass()
            {
            }
        }

        IReadOnlyList<float> IScrollSnapPointsInfo.GetIrregularSnapPoints(Orientation orientation, SnapPointsAlignment alignment)
        {
            throw new NotImplementedException();
        }

        float IScrollSnapPointsInfo.GetRegularSnapPoints(Orientation orientation, SnapPointsAlignment alignment, out float offset)
        {
            throw new NotImplementedException();
        }

        bool IScrollSnapPointsInfo.AreHorizontalSnapPointsRegular => throw new NotImplementedException();

        bool IScrollSnapPointsInfo.AreVerticalSnapPointsRegular => throw new NotImplementedException();

        event EventHandler<object> IScrollSnapPointsInfo.HorizontalSnapPointsChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<object> IScrollSnapPointsInfo.VerticalSnapPointsChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }
    }
}
