// Decompiled with JetBrains decompiler
// Type: myTube.AnnotationsPanel
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

using RykenTube;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace myTube
{
  public class AnnotationsPanel : Panel
  {
    protected virtual Size ArrangeOverride(Size finalSize)
    {
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        FrameworkElement frameworkElement = child as FrameworkElement;
        AnnotationInfo dataContext = frameworkElement.DataContext as AnnotationInfo;
        Size desiredSize = ((UIElement) frameworkElement).DesiredSize;
        if (dataContext != null)
          ((UIElement) frameworkElement).Arrange(new Rect(finalSize.Width * dataContext.Rect.X, finalSize.Height * dataContext.Rect.Y, finalSize.Width * dataContext.Rect.Width, finalSize.Height * dataContext.Rect.Height));
        else
          ((UIElement) frameworkElement).Arrange(new Rect(0.0, 0.0, 100.0, 100.0));
      }
      return finalSize;
    }

    protected virtual Size MeasureOverride(Size availableSize)
    {
      if (double.IsNaN(availableSize.Width) || double.IsInfinity(availableSize.Width))
        availableSize.Width = 1280.0;
      if (double.IsNaN(availableSize.Height) || double.IsInfinity(availableSize.Height))
        availableSize.Width = 720.0;
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        FrameworkElement frameworkElement1 = child as FrameworkElement;
        if (frameworkElement1.DataContext is AnnotationInfo dataContext)
        {
          if (dataContext.Rect != Rect.Empty)
          {
            FrameworkElement frameworkElement2 = frameworkElement1;
            double width1 = availableSize.Width;
            Rect rect = dataContext.Rect;
            double width2 = rect.Width;
            double width3 = width1 * width2;
            double height1 = availableSize.Height;
            rect = dataContext.Rect;
            double height2 = rect.Height;
            double height3 = height1 * height2;
            Size size = new Size(width3, height3);
            ((UIElement) frameworkElement2).Measure(size);
          }
        }
        else
          ((UIElement) frameworkElement1).Measure(availableSize);
      }
      if (double.IsNaN(availableSize.Width) || double.IsInfinity(availableSize.Width))
        availableSize.Width = 100.0;
      if (double.IsNaN(availableSize.Height) || double.IsInfinity(availableSize.Height))
        availableSize.Height = 100.0;
      return availableSize;
    }
  }
}
