// Decompiled with JetBrains decompiler
// Type: myTube.AnnotationControl
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using myTube.Helpers;
using RykenTube;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace myTube
{
  public sealed class AnnotationControl : UserControl, IComponentConnector
  {
    private static Popup holdPopup;
    private static RotateTransform popTrans = new RotateTransform();
    private DateTime lastTap = DateTime.MinValue;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private Image promoImage;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private SymbolIcon goIcon;
    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    private bool _contentLoaded;

    static AnnotationControl()
    {
      Popup popup = new Popup();
      ((UIElement) popup).put_IsHitTestVisible(false);
      AnnotationControl.holdPopup = popup;
      TextBlock textBlock = new TextBlock();
      textBlock.put_Text(App.Strings["videos.annotations.tapandhold", "tap and hold to open annotations"].ToLower());
      textBlock.put_FontSize((double) ((IDictionary<object, object>) App.Instance.Resources)[(object) "ImportantFontSize"]);
      textBlock.put_Foreground((Brush) new SolidColorBrush(Colors.White));
      ((FrameworkElement) textBlock).put_Margin(new Thickness(19.0));
      textBlock.put_TextWrapping((TextWrapping) 2);
      textBlock.put_TextAlignment((TextAlignment) 0);
      Grid grid1 = new Grid();
      ((UIElement) grid1).put_RenderTransform((Transform) AnnotationControl.popTrans);
      ((UIElement) grid1).put_RenderTransformOrigin(new Point(0.5, 0.5));
      Grid g = grid1;
      Grid grid2 = g;
      SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.Black);
      ((Brush) solidColorBrush).put_Opacity(0.5);
      ((Panel) grid2).put_Background((Brush) solidColorBrush);
      ((ICollection<UIElement>) ((Panel) g).Children).Add((UIElement) textBlock);
      AnnotationControl.holdPopup.put_Child((UIElement) g);
      ((UIElement) g).UpdateLayout();
      DefaultPage.SetPopupArrangeMethod((DependencyObject) AnnotationControl.holdPopup, (Func<Point>) (() =>
      {
        Popup holdPopup = AnnotationControl.holdPopup;
        Grid grid3 = g;
        Rect bounds1 = Window.Current.Bounds;
        double width1 = bounds1.Width;
        bounds1 = Window.Current.Bounds;
        double width2 = bounds1.Width;
        double num1;
        double num2 = num1 = Math.Min(Math.Min(width1, width2) - 19.0, 450.0);
        ((FrameworkElement) grid3).put_Width(num1);
        double num3 = num2;
        ((FrameworkElement) holdPopup).put_Width(num3);
        ((UIElement) g).UpdateLayout();
        Rect bounds2 = Window.Current.Bounds;
        double x = bounds2.Width / 2.0 - ((FrameworkElement) g).ActualWidth / 2.0;
        bounds2 = Window.Current.Bounds;
        double y = bounds2.Height / 2.0 - ((FrameworkElement) g).ActualHeight / 2.0;
        return new Point(x, y);
      }));
      Accel.OrientChanged += new OrientChangedEventHandler(AnnotationControl.Accel_OrientChanged);
    }

    private static void Accel_OrientChanged(OrientChangedEventArgs e)
    {
      double To = 0.0;
      switch ((int) e.Orientation)
      {
        case 0:
          To = 0.0;
          break;
        case 1:
          To = 90.0;
          break;
        case 3:
          To = 270.0;
          break;
      }
      if (AnnotationControl.holdPopup.IsOpen)
        Ani.Begin((DependencyObject) AnnotationControl.popTrans, "Angle", To, 0.6, 5.0);
      else
        AnnotationControl.popTrans.put_Angle(To);
    }

    private static async void ShowHoldPopup()
    {
      Helper.Write((object) "Showing popup about holding annotations");
      ((UIElement) AnnotationControl.holdPopup).put_Opacity(1.0);
      DefaultPage.Current.ShowPopup(AnnotationControl.holdPopup, new Point(), new Point(0.0, 0.0), FadeType.NoFade, false, Ani.Animation(Ani.DoubleAni((DependencyObject) AnnotationControl.holdPopup, "Opacity", 0.0, 0.5)));
      await Task.Delay(3000);
      DefaultPage.Current.ClosePopup();
    }

    public AnnotationControl()
    {
      this.InitializeComponent();
      WindowsRuntimeMarshal.AddEventHandler<HoldingEventHandler>(new Func<HoldingEventHandler, EventRegistrationToken>(((UIElement) this).add_Holding), new Action<EventRegistrationToken>(((UIElement) this).remove_Holding), new HoldingEventHandler(this.AnnotationControl_Holding));
      WindowsRuntimeMarshal.AddEventHandler<TappedEventHandler>(new Func<TappedEventHandler, EventRegistrationToken>(((UIElement) this).add_Tapped), new Action<EventRegistrationToken>(((UIElement) this).remove_Tapped), new TappedEventHandler(this.AnnotationControl_Tapped));
      WindowsRuntimeMarshal.AddEventHandler<RightTappedEventHandler>(new Func<RightTappedEventHandler, EventRegistrationToken>(((UIElement) this).add_RightTapped), new Action<EventRegistrationToken>(((UIElement) this).remove_RightTapped), new RightTappedEventHandler(this.AnnotationControl_RightTapped));
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>>(new Func<TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>, EventRegistrationToken>(((FrameworkElement) this).add_DataContextChanged), new Action<EventRegistrationToken>(((FrameworkElement) this).remove_DataContextChanged), new TypedEventHandler<FrameworkElement, DataContextChangedEventArgs>((object) this, __methodptr(AnnotationControl_DataContextChanged)));
    }

    private void AnnotationControl_DataContextChanged(
      FrameworkElement sender,
      DataContextChangedEventArgs args)
    {
      if (!(((FrameworkElement) this).DataContext is AnnotationInfo dataContext))
        return;
      if (dataContext.HasAction && dataContext.Type != AnnotationType.Branding && !string.IsNullOrWhiteSpace(dataContext.Text))
        ((UIElement) this.goIcon).put_Visibility((Visibility) 0);
      else
        ((UIElement) this.goIcon).put_Visibility((Visibility) 1);
    }

    private void AnnotationControl_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
      if (e.PointerDeviceType != 2)
        return;
      e.put_Handled(this.performAction());
    }

    private void AnnotationControl_Tapped(object sender, TappedRoutedEventArgs e)
    {
      if (DateTime.Now - this.lastTap > TimeSpan.FromSeconds(2.0))
      {
        this.lastTap = DateTime.Now;
        e.put_Handled(true);
      }
      AnnotationControl.ShowHoldPopup();
    }

    private void AnnotationControl_Holding(object sender, HoldingRoutedEventArgs e)
    {
      if (e.HoldingState != null)
        return;
      e.put_Handled(this.performAction());
    }

    private bool performAction()
    {
      bool flag = false;
      if (((FrameworkElement) this).DataContext is AnnotationInfo dataContext)
      {
        Helper.Write((object) "AnnotationcControl", (object) "Performing annotation function");
        if (dataContext.URL != null)
        {
          flag = true;
          YouTubeURLInfo urlType = YouTubeURLHelper.GetUrlType(dataContext.URL.ToString());
          switch (urlType.Type)
          {
            case YouTubeURLType.Video:
              App.Instance.RootFrame.Navigate(typeof (VideoPage), (object) urlType.ID);
              if (Settings.RotationType == RotationType.Custom)
                DefaultPage.Current.Rotate((SimpleOrientation) 0);
              if (!DefaultPage.Current.Shown && DefaultPage.Current.OverCanvas != null)
              {
                DefaultPage.Current.OverCanvas.ScrollToPage(0, false);
                break;
              }
              break;
            case YouTubeURLType.Playlist:
              App.Instance.RootFrame.Navigate(typeof (PlaylistPage), (object) urlType.ID);
              if (Settings.RotationType == RotationType.Custom)
                DefaultPage.Current.Rotate((SimpleOrientation) 0);
              if (!DefaultPage.Current.Shown && DefaultPage.Current.OverCanvas != null)
              {
                DefaultPage.Current.OverCanvas.ScrollToPage(0, false);
                break;
              }
              break;
            case YouTubeURLType.Channel:
              App.Instance.RootFrame.Navigate(typeof (ChannelPage), (object) urlType.ID);
              if (Settings.RotationType == RotationType.Custom)
                DefaultPage.Current.Rotate((SimpleOrientation) 0);
              if (!DefaultPage.Current.Shown && DefaultPage.Current.OverCanvas != null)
              {
                DefaultPage.Current.OverCanvas.ScrollToPage(0, false);
                break;
              }
              break;
            default:
              Launcher.LaunchUriAsync(dataContext.URL.ToUri(UriKind.Absolute));
              break;
          }
        }
        else if (dataContext.ChannelId != null)
        {
          flag = true;
          App.Instance.RootFrame.Navigate(typeof (ChannelPage), (object) dataContext.ChannelId);
        }
      }
      return flag;
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("ms-appx:///AnnotationControl.xaml"), (ComponentResourceLocation) 0);
      this.promoImage = (Image) ((FrameworkElement) this).FindName("promoImage");
      this.goIcon = (SymbolIcon) ((FrameworkElement) this).FindName("goIcon");
    }

    [GeneratedCode("Microsoft.Windows.UI.Xaml.Build.Tasks", " 4.0.0.0")]
    [DebuggerNonUserCode]
    public void Connect(int connectionId, object target) => this._contentLoaded = true;
  }
}
