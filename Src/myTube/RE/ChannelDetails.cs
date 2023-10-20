// Decompiled with JetBrains decompiler
// Type: myTube.ChannelDetails
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace myTube
{
  public sealed class ChannelDetails : UserControl, IComponentConnector
  {
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private IconTextButton subscribeButton;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Ellipse thumb;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    public ChannelDetails()
    {
      this.InitializeComponent();
      int num = DesignMode.DesignModeEnabled ? 1 : 0;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(ChannelDetails_DataContextChanged)));
    }

    private void ChannelDetails_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(((FrameworkElement) this).DataContext is UserInfo))
        return;
      if (YouTube.IsSubscribedTo(((FrameworkElement) this).DataContext as UserInfo))
      {
        this.subscribeButton.Symbol = (Symbol) 57608;
        this.subscribeButton.Text = App.Strings["channels.unsubscribe", "unsub"].ToLower();
      }
      else
      {
        this.subscribeButton.Symbol = (Symbol) 57609;
        this.subscribeButton.Text = App.Strings["channels.subscribe", "subscribe"].ToLower();
      }
    }

    private async void subscribeButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is UserInfo))
        return;
      UserInfo inf = ((FrameworkElement) this).DataContext as UserInfo;
      int num1 = YouTube.IsSubscribedTo(inf) ? 1 : 0;
      ((UIElement) this.subscribeButton).put_Opacity(0.5);
      ((UIElement) this.subscribeButton).put_IsHitTestVisible(false);
      if (num1 == 0)
      {
        try
        {
          this.subscribeButton.Symbol = (Symbol) 57608;
          this.subscribeButton.Text = App.Strings["channels.unsubscribe", "unsub"].ToLower();
          YouTube.InsertSubscription(await YouTube.Subscribe(inf.ID));
        }
        catch (Exception ex)
        {
          this.subscribeButton.Symbol = (Symbol) 57609;
          this.subscribeButton.Text = App.Strings["channels.subscribe", "subscribe"].ToLower();
          MessageDialog messageDialog = new MessageDialog("We weren't able to subscribe you to this user", "Oops");
          messageDialog.Commands.Add((IUICommand) new UICommand("okay :("));
          messageDialog.ShowAsync();
        }
      }
      else
      {
        try
        {
          string subscriptionId = YouTube.GetSubscriptionID(inf);
          this.subscribeButton.Symbol = (Symbol) 57609;
          this.subscribeButton.Text = App.Strings["channels.subscribe", "subscribe"].ToLower();
          if (subscriptionId != null)
          {
            int num2 = await YouTube.Unsubscribe(subscriptionId) ? 1 : 0;
            YouTube.RemoveSubscribedUser(inf);
            YouTube.CallSubscriptionsLoaded();
          }
        }
        catch (Exception ex)
        {
          this.subscribeButton.Symbol = (Symbol) 57608;
          MessageDialog messageDialog = new MessageDialog("We weren't able to unsubscribe you from this user", "Oops");
          this.subscribeButton.Text = App.Strings["channels.unsubscribe", "unsub"].ToLower();
          messageDialog.Commands.Add((IUICommand) new UICommand("okay :("));
          messageDialog.ShowAsync();
        }
      }
      Ani.Begin((DependencyObject) this.subscribeButton, "Opacity", 1.0, 0.2);
      ((UIElement) this.subscribeButton).put_IsHitTestVisible(true);
      inf = (UserInfo) null;
    }

    private void playlistsButton_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (!(((FrameworkElement) this).DataContext is UserInfo))
        return;
      ((App) Application.Current).RootFrame.Navigate(typeof (PlaylistListPage), (object) (((FrameworkElement) this).DataContext as UserInfo));
    }

    private void Image_Tapped(object sender, TappedRoutedEventArgs e)
    {
      double To1 = Math.Sqrt(2.0) / 2.0;
      double To2 = 1.0 / To1;
      Rect bounds1 = Window.Current.Bounds;
      Rect bounds2 = ((FrameworkElement) this.thumb).GetBounds(Window.Current.Content);
      double To3 = Math.Max(bounds2.Width / bounds1.Width, bounds2.Height / bounds1.Height);
      Point point1 = bounds2.Center();
      double x = point1.X;
      point1 = bounds1.Center();
      double num1 = point1.X - bounds1.Left;
      double To4 = x - num1;
      Point point2 = bounds2.Center();
      double y = point2.Y;
      point2 = bounds1.Center();
      double num2 = point2.Y - bounds1.Top;
      double To5 = y - num2;
      Popup popup1 = new Popup();
      ((FrameworkElement) popup1).put_Width(bounds1.Width);
      ((FrameworkElement) popup1).put_Height(bounds1.Height);
      Popup popup2 = popup1;
      ScaleTransform scaleTransform = new ScaleTransform();
      scaleTransform.put_CenterX(0.5);
      scaleTransform.put_CenterY(0.5);
      scaleTransform.put_ScaleX(1.0);
      scaleTransform.put_ScaleY(1.0);
      ScaleTransform Element1 = scaleTransform;
      Ellipse ellipse1 = new Ellipse();
      ((UIElement) ellipse1).put_RenderTransformOrigin(new Point(0.5, 0.5));
      ImageBrush imageBrush = new ImageBrush();
      imageBrush.put_ImageSource((((Shape) this.thumb).Fill as ImageBrush).ImageSource);
      ((TileBrush) imageBrush).put_Stretch((Stretch) 2);
      ((Brush) imageBrush).put_RelativeTransform((Transform) Element1);
      ((Shape) ellipse1).put_Fill((Brush) imageBrush);
      Ellipse Element2 = ellipse1;
      Ellipse ellipse2 = Element2;
      double num3;
      ((FrameworkElement) Element2).put_Height(num3 = Math.Min(((FrameworkElement) popup2).Width, ((FrameworkElement) popup2).Height));
      double num4 = num3;
      ((FrameworkElement) ellipse2).put_Width(num4);
      Grid grid1 = new Grid();
      ((FrameworkElement) grid1).put_Width(((FrameworkElement) popup2).Width);
      ((FrameworkElement) grid1).put_Height(((FrameworkElement) popup2).Height);
      Grid Element3 = grid1;
      UIElementCollection children = ((Panel) Element3).Children;
      Rectangle rectangle = new Rectangle();
      ((Shape) rectangle).put_Fill((Brush) new SolidColorBrush(Colors.Transparent));
      ((ICollection<UIElement>) children).Add((UIElement) rectangle);
      Grid grid2 = Element3;
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) grid2).add_Tapped), new Action<EventRegistrationToken>(((UIElement) grid2).remove_Tapped), (TappedEventHandler) ((_param1, _param2) => DefaultPage.Current.ClosePopup()));
      ((ICollection<UIElement>) ((Panel) Element3).Children).Add((UIElement) Element2);
      CompositeTransform compositeTransform = new CompositeTransform();
      compositeTransform.put_ScaleX(To3);
      compositeTransform.put_ScaleY(To3);
      compositeTransform.put_TranslateX(To4);
      compositeTransform.put_TranslateY(To5);
      CompositeTransform Element4 = compositeTransform;
      ((UIElement) Element2).put_RenderTransform((Transform) Element4);
      popup2.put_Child((UIElement) Element3);
      double Duration1 = 0.5;
      double startTime = 0.35;
      ExponentialEase Ease1 = Ani.Ease((EasingMode) 2, 7.0);
      Storyboard closeAnimation = Ani.Animation(Ani.DoubleAni((DependencyObject) Element4, "ScaleX", To3, Duration1, (EasingFunctionBase) Ease1), Ani.DoubleAni((DependencyObject) Element4, "ScaleY", To3, Duration1, (EasingFunctionBase) Ease1), Ani.DoubleAni((DependencyObject) Element1, "ScaleX", 1.0, Duration1, (EasingFunctionBase) Ease1), Ani.DoubleAni((DependencyObject) Element1, "ScaleY", 1.0, Duration1, (EasingFunctionBase) Ease1), Ani.DoubleAni((DependencyObject) Element4, "TranslateX", To4, Duration1, (EasingFunctionBase) Ease1), Ani.DoubleAni((DependencyObject) Element4, "TranslateY", To5, Duration1, (EasingFunctionBase) Ease1), Ani.DoubleAni((DependencyObject) Element3, "Opacity", 0.0, Duration1 - startTime, (EasingFunctionBase) null, startTime), Ani.DoubleAni((DependencyObject) this.thumb, "Opacity", 1.0, (Duration1 - startTime) / 2.0, (EasingFunctionBase) null, startTime));
      DefaultPage.Current.ShowPopup(popup2, new Point(0.0, 0.0), new Point(0.0, 0.0), lightDismissed: false, closeAnimation: closeAnimation);
      double Duration2 = 0.4;
      ExponentialEase Ease2 = Ani.Ease((EasingMode) 2, 5.0);
      Ani.Ease((EasingMode) 0, 3.0);
      Ani.Begin((Timeline) Ani.DoubleAni((DependencyObject) Element2, "Opacity", 1.0, 0.05), (Timeline) Ani.DoubleAni((DependencyObject) this.thumb, "Opacity", 0.0, 0.1), (Timeline) Ani.DoubleAni((DependencyObject) Element4, "ScaleX", To2, Duration2, (EasingFunctionBase) Ease2), (Timeline) Ani.DoubleAni((DependencyObject) Element4, "ScaleY", To2, Duration2, (EasingFunctionBase) Ease2), (Timeline) Ani.DoubleAni((DependencyObject) Element1, "ScaleX", To1, Duration2, (EasingFunctionBase) Ease2), (Timeline) Ani.DoubleAni((DependencyObject) Element1, "ScaleY", To1, Duration2, (EasingFunctionBase) Ease2), (Timeline) Ani.DoubleAni((DependencyObject) Element4, "TranslateX", 0.0, Duration2, (EasingFunctionBase) Ease2), (Timeline) Ani.DoubleAni((DependencyObject) Element4, "TranslateY", 0.0, Duration2, (EasingFunctionBase) Ease2));
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///ChannelDetails.xaml"), (ComponentResourceLocation) 0);
      this.subscribeButton = (IconTextButton) ((FrameworkElement) this).FindName("subscribeButton");
      this.thumb = (Ellipse) ((FrameworkElement) this).FindName("thumb");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          UIElement uiElement1 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement1.add_Tapped), new Action<EventRegistrationToken>(uiElement1.remove_Tapped), new TappedEventHandler(this.subscribeButton_Tapped));
          break;
        case 2:
          UIElement uiElement2 = (UIElement) target;
          WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(uiElement2.add_Tapped), new Action<EventRegistrationToken>(uiElement2.remove_Tapped), new TappedEventHandler(this.Image_Tapped));
          break;
      }
      this._contentLoaded = true;
    }
  }
}
