// myTube.CustomFrame

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace myTube
{
  public class CustomFrame : Frame
  {
    private const double TranslateBy = 66.5;
    private const float ScaleBy = 0.0375f;
    private TranslateTransform trans;
    private bool navigating;
    private bool firstNavigate;
    private bool clearBackStack;
    private Queue<PageStackEntry> pageStack = new Queue<PageStackEntry>();
    private int pageStackIndex;
    private bool removeLastEntry;
    private NavigationMode navMode;

    public event EventHandler<NavigationMode> NavigationCalled;

    public bool Animate { get; set; }

    public CustomFrame()
    {
      //WindowsRuntimeMarshal.AddEventHandler<NavigatedEventHandler>(
      //    new Func<NavigatedEventHandler, EventRegistrationToken>(((Frame) this).add_Navigated), 
      //    new Action<EventRegistrationToken>(((Frame) this).remove_Navigated), 
      //    new NavigatedEventHandler(this.CustomFrame_Navigated));
      this.Navigated += this.CustomFrame_Navigated;

      this.trans = new TranslateTransform();
      ((UIElement)this).RenderTransform = (Transform)this.trans;
      this.Animate = true;
      ((ContentControl)this).ContentTransitions = new TransitionCollection();

    }

    public void ClearBackStackAtNavigate() => this.clearBackStack = true;

    public void AddToBackStackAtNavigate(Type sourceType, object param, int index = 0)
    {
      this.pageStackIndex = index;
      this.pageStack.Enqueue(new PageStackEntry(sourceType, param, (NavigationTransitionInfo) null));
    }

    public void RemoveLastBackStackAtNavigate() => this.removeLastEntry = true;

        protected override Size MeasureOverride(Size availableSize) => this.navigating
                ? availableSize : availableSize;//((FrameworkElement) this).MeasureOverride(availableSize);

        protected override Size ArrangeOverride(Size finalSize) => this.navigating
                ? finalSize : finalSize;//((FrameworkElement) this).ArrangeOverride(finalSize);

    private void CustomFrame_Navigated(object sender, NavigationEventArgs e)
    {
      this.navigating = false;
      if (((ContentControl) this).Content == null)
        return;
      if (((ContentControl) this).Content is FrameworkElement)
      {
        Helper.Write((object) nameof (CustomFrame), 
            (object) ("Navigation Mode: " + (object) e.NavigationMode));


        this.navMode = e.NavigationMode;
        if (e.SourcePageType == ((ContentControl) this).Content.GetType())
        {
          this.CustomFrame_Loaded(((ContentControl) this).Content, (RoutedEventArgs) null);
        }
        else
        {
          FrameworkElement content = ((ContentControl) this).Content as FrameworkElement;
         // WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(
         //     new Func<RoutedEventHandler, EventRegistrationToken>(content.add_Loaded), 
         //     new Action<EventRegistrationToken>(content.remove_Loaded), 
         //     new RoutedEventHandler(this.CustomFrame_Loaded));

           
         content.Loaded += this.CustomFrame_Loaded;
        }
      }
      if (this.removeLastEntry)
      {
        this.removeLastEntry = false;
        if (this.BackStack.Count > 0)
          this.BackStack.RemoveAt(this.BackStack.Count - 1);
      }
      if (this.clearBackStack)
      {
        this.clearBackStack = false;
        this.BackStack.Clear();
      }
      while (this.pageStack.Count > 0)
        this.BackStack.Add(this.pageStack.Dequeue());
    }

    private async void CustomFrame_Loaded(object sender, RoutedEventArgs e)
    {
      //WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>(
      //    new Action<EventRegistrationToken>((((ContentControl) this).Content 
      //    as FrameworkElement).remove_Loaded), new RoutedEventHandler(this.CustomFrame_Loaded));
       ((FrameworkElement)((ContentControl)this).Content).Loaded -= CustomFrame_Loaded;


      if (this.firstNavigate && this.Animate)
      {
        ((UIElement) this).Opacity = 1.0;
        this.trans.Y = (this.navMode == NavigationMode.Back ? -66.5 : 66.5);
        Storyboard sb = new Storyboard();
        sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this.trans, "Y", 0.0, 0.55, 
            (EasingFunctionBase) Ani.Ease((EasingMode) 0, 4.0)));
        sb.Begin();
      }
      else
      {
        this.firstNavigate = true;
        this.trans.X = (0.0);
        this.trans.Y = (0.0);
        ((UIElement) this).Opacity = (1.0);
      }
    }

    public bool Navigate(Type type) => this.Navigate(type, (object) null);

    public bool Navigate(Type type, object parameter) 
            => this.Navigate(type, parameter, (NavigationTransitionInfo) null);

    public bool Navigate(Type type, object parameter, NavigationTransitionInfo tranInfo)
    {
      if (!this.navigating)
      {
        if (this.NavigationCalled != null)
          this.NavigationCalled((object) this, (NavigationMode) 0);
        if (this.firstNavigate && this.Animate)
        {
          this.navigating = true;
          ((UIElement) this).RenderTransform = ((Transform) this.trans);
          this.trans.X = (0.0);
          if (((ContentControl) this).Content != null)
          {
            Storyboard sb = new Storyboard();
            sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this.trans, "Y", -49.875, 0.1,
                (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)));
            sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this, "Opacity", 0.0, 0.1));

            sb.Begin();
            Storyboard storyboard = sb;

                //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, 
                //    EventRegistrationToken>(((Timeline) storyboard).add_Completed), 
                //    new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed),
                //    (EventHandler<object>) ((_param1, _param2) => base.Navigate(type, parameter, tranInfo)));
                        
                //((Timeline)storyboard).Completed -= (sender, args) => base.Navigate(type, parameter, tranInfo);
                //base.Navigate(type, parameter, tranInfo);

                storyboard.Completed += (s, e) =>
                {
                    ((Timeline)s).Completed -= (sender, args) => base.Navigate(type, parameter, tranInfo);
                    base.Navigate(type, parameter, tranInfo);
                };

            }
          else
            base.Navigate(type, parameter, tranInfo);
        }
        else
          base.Navigate(type, parameter, tranInfo);
      }
      return true;
    }

    private async void asyncNavigate(
      int delay,
      Type type,
      object parameter,
      NavigationTransitionInfo tranInfo)
    {
      await Task.Delay(delay);
      base.Navigate(type, parameter, tranInfo);
    }

    private async void asyncGoBack(int delay)
    {
      await Task.Delay(delay);
      base.GoBack();
    }

    public void GoBack()
    {
      if (this.navigating)
        return;
      this.navigating = true;
      ((UIElement) this).RenderTransform = (Transform) this.trans;
      if (this.NavigationCalled != null)
        this.NavigationCalled((object) this, (NavigationMode) 1);
      if (this.Animate)
      {
        Storyboard sb = new Storyboard();
        this.trans.Y = (0.0);
        sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this.trans, "Y", 66.5, 0.1, 
            (EasingFunctionBase) Ani.Ease((EasingMode) 1, 3.0)));
        sb.Add((Timeline) Ani.DoubleAni((DependencyObject) this, "Opacity", 0.0, 0.1));
        sb.Begin();
        try
        {
          Storyboard storyboard = sb;

          //WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(
          //    new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard).add_Completed),
          //    new Action<EventRegistrationToken>(((Timeline) storyboard).remove_Completed), 
          //    (EventHandler<object>) ((_param1, _param2) => base.GoBack()));

            storyboard.Completed += (s, e) =>
            {
                ((Timeline)s).Completed -= (sender, args) => base.GoBack();
                base.GoBack();
            };
        }
        catch
        {
        }
      }
      else
        base.GoBack();
    }
  }
}
