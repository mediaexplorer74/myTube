// Decompiled with JetBrains decompiler
// Type: myTube.VideoContextMenu
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class VideoContextMenu : UserControl, IComponentConnector
  {
    public DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof (ItemsSource), typeof (List<IconButtonEvent>), typeof (VideoContextMenu), new PropertyMetadata((object) null));
    public DependencyProperty SelectButtonEnabledProperty = DependencyProperty.Register(nameof (SelectButtonEnabled), typeof (bool), typeof (VideoContextMenu), new PropertyMetadata((object) false));
    public DependencyProperty CancelButtonEnabledProperty = DependencyProperty.Register(nameof (CancelButtonEnabled), typeof (bool), typeof (VideoContextMenu), new PropertyMetadata((object) false));
    public FrameworkElement SelectedElement;
    private TaskCompletionSource<object> waitForObjectTcs;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private UserControl userControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private TransitionCollection transitions;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ContentControl closeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Rectangle backgroundRec;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private ItemsControl itemsControl;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public List<IconButtonEvent> ItemsSource
    {
      get => (List<IconButtonEvent>) ((DependencyObject) this).GetValue(this.ItemsSourceProperty);
      set => ((DependencyObject) this).SetValue(this.ItemsSourceProperty, (object) value);
    }

    public bool SelectButtonEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(this.SelectButtonEnabledProperty);
      set => ((DependencyObject) this).SetValue(this.SelectButtonEnabledProperty, (object) value);
    }

    public bool CancelButtonEnabled
    {
      get => (bool) ((DependencyObject) this).GetValue(this.CancelButtonEnabledProperty);
      set => ((DependencyObject) this).SetValue(this.CancelButtonEnabledProperty, (object) value);
    }

    public VideoContextMenu()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Loaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Loaded), new RoutedEventHandler(this.VideoContextMenu_Loaded));
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>(new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) this).add_Unloaded), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_Unloaded), new RoutedEventHandler(this.VideoContextMenu_Unloaded));
    }

    private void VideoContextMenu_Unloaded(object sender, RoutedEventArgs e)
    {
      if (this.waitForObjectTcs == null)
        return;
      this.waitForObjectTcs.TrySetResult((object) null);
    }

    private void VideoContextMenu_Loaded(object sender, RoutedEventArgs e)
    {
    }

    public event EventHandler SelectTapped;

    public event EventHandler CancelTapped;

    public void SetTransitionOffset(double horizontal, double vertical)
    {
      foreach (Transition transition in (IEnumerable<Transition>) this.transitions)
      {
        if (transition is EntranceThemeTransition)
        {
          (transition as EntranceThemeTransition).put_FromHorizontalOffset(horizontal);
          (transition as EntranceThemeTransition).put_FromVerticalOffset(vertical);
          (transition as EntranceThemeTransition).put_IsStaggeringEnabled(true);
        }
      }
    }

    public Task<object> WaitForDataContext()
    {
      if (this.waitForObjectTcs == null)
        this.waitForObjectTcs = new TaskCompletionSource<object>();
      return this.waitForObjectTcs.Task;
    }

    private async void ItemTapped(object sender, TappedRoutedEventArgs e)
    {
      ((UIElement) this).put_IsHitTestVisible(false);
      FrameworkElement s = sender as FrameworkElement;
      if (s == null || !(s.DataContext is IconButtonEvent))
        return;
      ((UIElement) s).put_IsHitTestVisible(false);
      Task selectedTask = (s.DataContext as IconButtonEvent).CallSelected(((FrameworkElement) this).DataContext ?? (object) this.SelectedElement);
      if (this.waitForObjectTcs != null)
      {
        this.waitForObjectTcs.TrySetResult((s.DataContext as IconButtonEvent).DataContext);
        this.waitForObjectTcs = (TaskCompletionSource<object>) null;
      }
      Storyboard ani = new Storyboard();
      foreach (UIElement child in (IEnumerable<UIElement>) this.itemsControl.ItemsPanelRoot.Children)
      {
        if ((child as FrameworkElement).DataContext != s.DataContext)
          ani.Add((Timeline) Ani.DoubleAni((DependencyObject) child, "Opacity", 0.0, 0.1));
        child.put_IsHitTestVisible(false);
      }
      await Task.Delay(100);
      ani.Begin();
      Storyboard storyboard1 = ani;
      WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard1).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard1).remove_Completed), (EventHandler<object>) (async (_param1_1, _param2_1) =>
      {
        await selectedTask;
        Storyboard storyboard2 = Ani.Begin((DependencyObject) s, "Opacity", 0.0, 0.1);
        WindowsRuntimeMarshal.AddEventHandler<EventHandler<object>>(new Func<EventHandler<object>, EventRegistrationToken>(((Timeline) storyboard2).add_Completed), new Action<EventRegistrationToken>(((Timeline) storyboard2).remove_Completed), (EventHandler<object>) ((_param1_2, _param2_2) => Helper.FindParent<Popup>((FrameworkElement) this, 100)?.put_IsOpen(false)));
      }));
      ani = (Storyboard) null;
    }

    private void userControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      double width = e.NewSize.Width;
      if (width > 700.0)
        ((FrameworkElement) this.itemsControl).put_Margin(new Thickness(76.0, 0.0, 76.0, 0.0));
      else if (width > 600.0)
        ((FrameworkElement) this.itemsControl).put_Margin(new Thickness(57.0, 0.0, 57.0, 0.0));
      else if (width > 400.0)
        ((FrameworkElement) this.itemsControl).put_Margin(new Thickness(38.0, 0.0, 38.0, 0.0));
      else
        ((FrameworkElement) this.itemsControl).put_Margin(new Thickness(19.0, 0.0, 19.0, 0.0));
    }

    private void IconTextButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.SelectTapped == null)
        return;
      this.SelectTapped(sender, new EventArgs());
    }

    private void closeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (this.CancelTapped == null)
        return;
      this.CancelTapped(sender, new EventArgs());
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///VideoContextMenu.xaml"), (ComponentResourceLocation) 0);
      this.userControl = (UserControl) ((FrameworkElement) this).FindName("userControl");
      this.transitions = (TransitionCollection) ((FrameworkElement) this).FindName("transitions");
      this.closeButton = (ContentControl) ((FrameworkElement) this).FindName("closeButton");
      this.backgroundRec = (Rectangle) ((FrameworkElement) this).FindName("backgroundRec");
      this.itemsControl = (ItemsControl) ((FrameworkElement) this).FindName("itemsControl");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          FrameworkElement frameworkElement = (FrameworkElement) target;
          WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>(new Func<SizeChangedEventHandler, EventRegistrationToken>(frameworkElement.add_SizeChanged), new Action<EventRegistrationToken>(frameworkElement.remove_SizeChanged), new SizeChangedEventHandler(this.userControl_SizeChanged));
          break;
        case 2:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.ItemTapped));
          break;
        case 3:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.closeButton_Tapped));
          break;
        case 4:
          UIElement uiElement3 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement3.add_Tapped), new Action<EventRegistrationToken>(uiElement3.remove_Tapped), new TappedEventHandler(this.IconTextButton_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
