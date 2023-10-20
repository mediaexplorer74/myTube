// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ImageExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ImageExtensions
  {
    public static readonly DependencyProperty FadeInOnLoadedProperty = DependencyProperty.RegisterAttached("FadeInOnLoaded", (Type) typeof (bool), (Type) typeof (ImageExtensions), new PropertyMetadata((object) false, new PropertyChangedCallback(ImageExtensions.OnFadeInOnLoadedChanged)));
    public static readonly DependencyProperty FadeInOnLoadedHandlerProperty = DependencyProperty.RegisterAttached("FadeInOnLoadedHandler", (Type) typeof (FadeInOnLoadedHandler), (Type) typeof (ImageExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ImageLoadedTransitionTypeProperty = DependencyProperty.RegisterAttached("ImageLoadedTransitionType", (Type) typeof (ImageLoadedTransitionTypes), (Type) typeof (ImageExtensions), new PropertyMetadata((object) ImageLoadedTransitionTypes.FadeIn));
    public static readonly DependencyProperty SourceProperty = DependencyProperty.RegisterAttached("Source", (Type) typeof (object), (Type) typeof (ImageExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageExtensions.OnSourceChanged)));
    public static readonly DependencyProperty CustomSourceProperty = DependencyProperty.RegisterAttached("CustomSource", (Type) typeof (string), (Type) typeof (ImageExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ImageExtensions.OnCustomSourceChanged)));

    public static bool GetFadeInOnLoaded(DependencyObject d) => (bool) d.GetValue(ImageExtensions.FadeInOnLoadedProperty);

    public static void SetFadeInOnLoaded(DependencyObject d, bool value) => d.SetValue(ImageExtensions.FadeInOnLoadedProperty, (object) value);

    private static void OnFadeInOnLoadedChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      DispatchedHandler dispatchedHandler1 = (DispatchedHandler) null;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ImageExtensions.\u003C\u003Ec__DisplayClass3 cDisplayClass3 = new ImageExtensions.\u003C\u003Ec__DisplayClass3();
      bool flag = (bool) d.GetValue(ImageExtensions.FadeInOnLoadedProperty);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3.image = (Image) d;
      if (DesignMode.DesignModeEnabled)
        return;
      if (flag)
      {
        FadeInOnLoadedHandler inOnLoadedHandler = new FadeInOnLoadedHandler((Image) d);
        ImageExtensions.SetFadeInOnLoadedHandler(d, inOnLoadedHandler);
        // ISSUE: reference to a compiler-generated field
        CoreDispatcher dispatcher = ((DependencyObject) cDisplayClass3.image).Dispatcher;
        if (dispatchedHandler1 == null)
        {
          // ISSUE: method pointer
          dispatchedHandler1 = new DispatchedHandler((object) cDisplayClass3, __methodptr(\u003COnFadeInOnLoadedChanged\u003Eb__1));
        }
        DispatchedHandler dispatchedHandler2 = dispatchedHandler1;
        dispatcher.RunAsync((CoreDispatcherPriority) 1, dispatchedHandler2);
      }
      else
      {
        FadeInOnLoadedHandler inOnLoadedHandler = ImageExtensions.GetFadeInOnLoadedHandler(d);
        ImageExtensions.SetFadeInOnLoadedHandler(d, (FadeInOnLoadedHandler) null);
        inOnLoadedHandler.Detach();
        // ISSUE: reference to a compiler-generated field
        ((DependencyObject) cDisplayClass3.image).SetValue(ImageExtensions.SourceProperty, (object) null);
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static FadeInOnLoadedHandler GetFadeInOnLoadedHandler(DependencyObject d) => (FadeInOnLoadedHandler) d.GetValue(ImageExtensions.FadeInOnLoadedHandlerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetFadeInOnLoadedHandler(DependencyObject d, FadeInOnLoadedHandler value) => d.SetValue(ImageExtensions.FadeInOnLoadedHandlerProperty, (object) value);

    public static ImageLoadedTransitionTypes GetImageLoadedTransitionType(DependencyObject d) => (ImageLoadedTransitionTypes) d.GetValue(ImageExtensions.ImageLoadedTransitionTypeProperty);

    public static void SetImageLoadedTransitionType(
      DependencyObject d,
      ImageLoadedTransitionTypes value)
    {
      d.SetValue(ImageExtensions.ImageLoadedTransitionTypeProperty, (object) value);
    }

    public static object GetSource(DependencyObject d) => d.GetValue(ImageExtensions.SourceProperty);

    public static void SetSource(DependencyObject d, object value) => d.SetValue(ImageExtensions.SourceProperty, value);

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (!ImageExtensions.GetFadeInOnLoaded(d))
        return;
      ImageExtensions.GetFadeInOnLoadedHandler(d)?.Detach();
      ImageExtensions.SetFadeInOnLoadedHandler(d, new FadeInOnLoadedHandler((Image) d));
    }

    public static string GetCustomSource(DependencyObject d) => (string) d.GetValue(ImageExtensions.CustomSourceProperty);

    public static void SetCustomSource(DependencyObject d, string value) => d.SetValue(ImageExtensions.CustomSourceProperty, (object) value);

    private static async void OnCustomSourceChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      string newCustomSource = (string) d.GetValue(ImageExtensions.CustomSourceProperty);
      Image image = (Image) d;
      image.put_Source((ImageSource) new BitmapImage((Uri) new Uri("ms-appx:///" + newCustomSource)));
      await ((FrameworkElement) image).WaitForUnloadedAsync();
      image.put_Source((ImageSource) null);
      GC.Collect();
    }
  }
}
