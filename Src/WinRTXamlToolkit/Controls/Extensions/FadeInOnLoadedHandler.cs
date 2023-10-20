// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.FadeInOnLoadedHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.AwaitableUI;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class FadeInOnLoadedHandler
  {
    private static Random _random;
    private Image _image;
    private BitmapImage _source;
    private double _targetOpacity;

    private static Random Random => FadeInOnLoadedHandler._random ?? (FadeInOnLoadedHandler._random = new Random());

    public FadeInOnLoadedHandler(Image image) => this.Attach(image);

    private void Attach(Image image)
    {
      this._image = image;
      this._source = image.Source as BitmapImage;
      if (this._source != null)
      {
        if (((BitmapSource) this._source).PixelWidth > 0)
        {
          ((UIElement) image).put_Opacity(1.0);
          this._image = (Image) null;
          this._source = (BitmapImage) null;
          return;
        }
        BitmapImage source1 = this._source;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(source1.add_ImageOpened), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(source1.remove_ImageOpened), new RoutedEventHandler(this.OnSourceImageOpened));
        BitmapImage source2 = this._source;
        WindowsRuntimeMarshal.AddEventHandler<ExceptionRoutedEventHandler>((Func<ExceptionRoutedEventHandler, EventRegistrationToken>) new Func<ExceptionRoutedEventHandler, EventRegistrationToken>(source2.add_ImageFailed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(source2.remove_ImageFailed), new ExceptionRoutedEventHandler(this.OnSourceImageFailed));
      }
      WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((FrameworkElement) image).add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) image).remove_Unloaded), new RoutedEventHandler(this.OnImageUnloaded));
      this._targetOpacity = ((UIElement) image).Opacity == 0.0 ? 1.0 : ((UIElement) image).Opacity;
      ((UIElement) image).put_Opacity(0.0);
    }

    private void OnSourceImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
      BitmapImage bitmapImage = (BitmapImage) sender;
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(bitmapImage.remove_ImageOpened), new RoutedEventHandler(this.OnSourceImageOpened));
      WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(bitmapImage.remove_ImageFailed), new ExceptionRoutedEventHandler(this.OnSourceImageFailed));
    }

    private async void OnSourceImageOpened(object sender, RoutedEventArgs e)
    {
      BitmapImage source = (BitmapImage) sender;
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(source.remove_ImageOpened), new RoutedEventHandler(this.OnSourceImageOpened));
      WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(source.remove_ImageFailed), new ExceptionRoutedEventHandler(this.OnSourceImageFailed));
      ImageLoadedTransitionTypes transitionType = ImageExtensions.GetImageLoadedTransitionType((DependencyObject) this._image);
      if (transitionType == ImageLoadedTransitionTypes.Random)
        transitionType = (ImageLoadedTransitionTypes) FadeInOnLoadedHandler.Random.Next(0, 5);
      switch (transitionType)
      {
        case ImageLoadedTransitionTypes.FadeIn:
          await ((UIElement) this._image).FadeInCustom(new TimeSpan?(TimeSpan.FromSeconds(1.0)), targetOpacity: this._targetOpacity);
          break;
        default:
          this.SlideIn(transitionType);
          break;
      }
    }

    private async void SlideIn(ImageLoadedTransitionTypes transitionType)
    {
      ((UIElement) this._image).put_Opacity(this._targetOpacity);
      Transform oldTransform = ((UIElement) this._image).RenderTransform;
      TranslateTransform tempTransform = new TranslateTransform();
      ((UIElement) this._image).put_RenderTransform((Transform) tempTransform);
      DoubleAnimation animation = (DoubleAnimation) null;
      switch (transitionType)
      {
        case ImageLoadedTransitionTypes.SlideUp:
          DoubleAnimation doubleAnimation1 = new DoubleAnimation();
          doubleAnimation1.put_From((double?) new double?(((FrameworkElement) this._image).ActualHeight));
          doubleAnimation1.put_To((double?) new double?(0.0));
          ((Timeline) doubleAnimation1).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(1.0));
          DoubleAnimation doubleAnimation2 = doubleAnimation1;
          CubicEase cubicEase1 = new CubicEase();
          ((EasingFunctionBase) cubicEase1).put_EasingMode((EasingMode) 0);
          CubicEase cubicEase2 = cubicEase1;
          doubleAnimation2.put_EasingFunction((EasingFunctionBase) cubicEase2);
          animation = doubleAnimation1;
          Storyboard.SetTargetProperty((Timeline) animation, "Y");
          break;
        case ImageLoadedTransitionTypes.SlideLeft:
          DoubleAnimation doubleAnimation3 = new DoubleAnimation();
          doubleAnimation3.put_From((double?) new double?(((FrameworkElement) this._image).ActualWidth));
          doubleAnimation3.put_To((double?) new double?(0.0));
          ((Timeline) doubleAnimation3).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(1.0));
          DoubleAnimation doubleAnimation4 = doubleAnimation3;
          CubicEase cubicEase3 = new CubicEase();
          ((EasingFunctionBase) cubicEase3).put_EasingMode((EasingMode) 0);
          CubicEase cubicEase4 = cubicEase3;
          doubleAnimation4.put_EasingFunction((EasingFunctionBase) cubicEase4);
          animation = doubleAnimation3;
          Storyboard.SetTargetProperty((Timeline) animation, "X");
          break;
        case ImageLoadedTransitionTypes.SlideDown:
          DoubleAnimation doubleAnimation5 = new DoubleAnimation();
          doubleAnimation5.put_From((double?) new double?(-((FrameworkElement) this._image).ActualHeight));
          doubleAnimation5.put_To((double?) new double?(0.0));
          ((Timeline) doubleAnimation5).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(1.0));
          DoubleAnimation doubleAnimation6 = doubleAnimation5;
          CubicEase cubicEase5 = new CubicEase();
          ((EasingFunctionBase) cubicEase5).put_EasingMode((EasingMode) 0);
          CubicEase cubicEase6 = cubicEase5;
          doubleAnimation6.put_EasingFunction((EasingFunctionBase) cubicEase6);
          animation = doubleAnimation5;
          Storyboard.SetTargetProperty((Timeline) animation, "Y");
          break;
        case ImageLoadedTransitionTypes.SlideRight:
          DoubleAnimation doubleAnimation7 = new DoubleAnimation();
          doubleAnimation7.put_From((double?) new double?(-((FrameworkElement) this._image).ActualWidth));
          doubleAnimation7.put_To((double?) new double?(0.0));
          ((Timeline) doubleAnimation7).put_Duration((Duration) (TimeSpan) TimeSpan.FromSeconds(1.0));
          DoubleAnimation doubleAnimation8 = doubleAnimation7;
          CubicEase cubicEase7 = new CubicEase();
          ((EasingFunctionBase) cubicEase7).put_EasingMode((EasingMode) 0);
          CubicEase cubicEase8 = cubicEase7;
          doubleAnimation8.put_EasingFunction((EasingFunctionBase) cubicEase8);
          animation = doubleAnimation7;
          Storyboard.SetTargetProperty((Timeline) animation, "X");
          break;
      }
      Storyboard.SetTarget((Timeline) animation, (DependencyObject) tempTransform);
      Storyboard sb = new Storyboard();
      ((Timeline) sb).put_Duration(((Timeline) animation).Duration);
      ((ICollection<Timeline>) sb.Children).Add((Timeline) animation);
      FrameworkElement clippingParent = ((FrameworkElement) this._image).Parent as FrameworkElement;
      RectangleGeometry clip = (RectangleGeometry) null;
      if (clippingParent != null)
      {
        clip = ((UIElement) clippingParent).Clip;
        GeneralTransform visual = ((UIElement) this._image).TransformToVisual((UIElement) clippingParent);
        Point point1 = (Point) visual.TransformPoint((Point) new Point(0.0, 0.0));
        point1 = new Point(Math.Max(0.0, point1.X), Math.Max(0.0, point1.Y));
        Point point2 = (Point) visual.TransformPoint((Point) new Point(((FrameworkElement) this._image).ActualWidth, ((FrameworkElement) this._image).ActualHeight));
        point2 = new Point(Math.Min(clippingParent.ActualWidth, point2.X), Math.Min(clippingParent.ActualHeight, point2.Y));
        FrameworkElement frameworkElement = clippingParent;
        RectangleGeometry rectangleGeometry1 = new RectangleGeometry();
        rectangleGeometry1.put_Rect((Rect) new Rect(point1, point2));
        RectangleGeometry rectangleGeometry2 = rectangleGeometry1;
        ((UIElement) frameworkElement).put_Clip(rectangleGeometry2);
      }
      await sb.BeginAsync();
      if (this._image == null)
        return;
      if (clippingParent != null)
        ((UIElement) this._image).put_Clip(clip);
      ((UIElement) this._image).put_RenderTransform(oldTransform);
    }

    private void OnImageUnloaded(object sender, RoutedEventArgs e) => this.Detach();

    internal void Detach()
    {
      if (this._source != null)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._source.remove_ImageOpened), new RoutedEventHandler(this.OnSourceImageOpened));
        WindowsRuntimeMarshal.RemoveEventHandler<ExceptionRoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._source.remove_ImageFailed), new ExceptionRoutedEventHandler(this.OnSourceImageFailed));
      }
      if (this._image == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this._image).remove_Unloaded), new RoutedEventHandler(this.OnImageUnloaded));
      ((UIElement) this._image).CleanUpPreviousFadeStoryboard();
      ((UIElement) this._image).put_Opacity(this._targetOpacity);
      this._image = (Image) null;
    }
  }
}
