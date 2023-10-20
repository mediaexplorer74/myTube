// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.VisualTreeHelperExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class VisualTreeHelperExtensions
  {
    public static UIElement GetRealWindowRoot(Window window = null)
    {
      if (window == null)
        window = Window.Current;
      if (window == null)
        return (UIElement) null;
      if (window.Content is FrameworkElement content)
      {
        List<DependencyObject> list = ((IEnumerable<DependencyObject>) ((DependencyObject) content).GetAncestors()).ToList<DependencyObject>();
        if (list.Count > 0)
          content = (FrameworkElement) list[list.Count - 1];
      }
      return (UIElement) content;
    }

    public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject => ((IEnumerable<T>) start.GetDescendantsOfType<T>()).FirstOrDefault<T>();

    public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject => (IEnumerable<T>) ((IEnumerable) start.GetDescendants()).OfType<T>();

    public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject start)
    {
      if (start != null)
      {
        Queue<DependencyObject> queue = new Queue<DependencyObject>();
        if (start is Popup popup2)
        {
          if (popup2.Child != null)
          {
            queue.Enqueue((DependencyObject) popup2.Child);
            yield return (DependencyObject) popup2.Child;
          }
        }
        else
        {
          int count = VisualTreeHelper.GetChildrenCount(start);
          for (int i = 0; i < count; ++i)
          {
            DependencyObject child = VisualTreeHelper.GetChild(start, i);
            queue.Enqueue(child);
            yield return child;
          }
        }
        while (queue.Count > 0)
        {
          DependencyObject parent = queue.Dequeue();
          if (parent is Popup popup2)
          {
            if (popup2.Child != null)
            {
              queue.Enqueue((DependencyObject) popup2.Child);
              yield return (DependencyObject) popup2.Child;
            }
          }
          else
          {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; ++i)
            {
              DependencyObject child = VisualTreeHelper.GetChild(parent, i);
              yield return child;
              queue.Enqueue(child);
            }
          }
        }
      }
    }

    public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent)
    {
      Popup popup = parent as Popup;
      if (popup != null && popup.Child != null)
      {
        yield return (DependencyObject) popup.Child;
      }
      else
      {
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; ++i)
        {
          DependencyObject child = VisualTreeHelper.GetChild(parent, i);
          yield return child;
        }
      }
    }

    public static IEnumerable<DependencyObject> GetChildrenByZIndex(this DependencyObject parent)
    {
      int i = 0;
      return (IEnumerable<DependencyObject>) ((IEnumerable) parent.GetChildren()).Cast<FrameworkElement>().Select(child => new
      {
        Index = i++,
        ZIndex = Canvas.GetZIndex((UIElement) child),
        Child = child
      }).OrderBy(indexedChild => indexedChild.ZIndex).ThenBy(indexedChild => indexedChild.Index).Select(indexedChild => indexedChild.Child);
    }

    public static T GetFirstAncestorOfType<T>(this DependencyObject start) where T : DependencyObject => ((IEnumerable<T>) start.GetAncestorsOfType<T>()).FirstOrDefault<T>();

    public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject start) where T : DependencyObject => (IEnumerable<T>) ((IEnumerable) start.GetAncestors()).OfType<T>();

    public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject start)
    {
      for (DependencyObject parent = VisualTreeHelper.GetParent(start); parent != null; parent = VisualTreeHelper.GetParent(parent))
        yield return parent;
    }

    public static IEnumerable<DependencyObject> GetSiblings(this DependencyObject start)
    {
      DependencyObject parent = VisualTreeHelper.GetParent(start);
      if (parent == null)
      {
        yield return start;
      }
      else
      {
        int count = VisualTreeHelper.GetChildrenCount(parent);
        for (int i = 0; i < count; ++i)
        {
          DependencyObject child = VisualTreeHelper.GetChild(parent, i);
          yield return child;
        }
      }
    }

    public static bool IsInVisualTree(this DependencyObject dob)
    {
      if (DesignMode.DesignModeEnabled || Window.Current == null)
        return false;
      UIElement realWindowRoot = VisualTreeHelperExtensions.GetRealWindowRoot();
      return realWindowRoot != null && ((IEnumerable<DependencyObject>) dob.GetAncestors()).Contains<DependencyObject>((DependencyObject) realWindowRoot) || VisualTreeHelper.GetOpenPopups(Window.Current).Any<Popup>((Func<Popup, bool>) (popup => popup.Child != null && ((IEnumerable<DependencyObject>) dob.GetAncestors()).Contains<DependencyObject>((DependencyObject) popup.Child)));
    }

    public static Point GetPosition(this UIElement dob, Point origin = default (Point), UIElement relativeTo = null)
    {
      if (DesignMode.DesignModeEnabled)
        return new Point();
      if (relativeTo == null)
        relativeTo = Window.Current.Content;
      if (relativeTo == null)
        throw new InvalidOperationException("Element not in visual tree.");
      double num1 = relativeTo is FrameworkElement frameworkElement ? frameworkElement.ActualWidth : 0.0;
      double num2 = frameworkElement != null ? frameworkElement.ActualHeight : 0.0;
      Point position = new Point(num1 * origin.X, num2 * origin.X);
      if (dob == relativeTo)
        return position;
      if (!((IEnumerable<DependencyObject>) ((IEnumerable<DependencyObject>) ((DependencyObject) dob).GetAncestors()).ToArray<DependencyObject>()).Contains<DependencyObject>((DependencyObject) relativeTo))
        throw new InvalidOperationException("Element not in visual tree.");
      return (Point) dob.TransformToVisual(relativeTo).TransformPoint((Point) position);
    }

    public static Rect GetBoundingRect(this UIElement dob, UIElement relativeTo = null)
    {
      if (DesignMode.DesignModeEnabled)
        return Rect.Empty;
      if (relativeTo == null)
        relativeTo = (UIElement) (Window.Current.Content as FrameworkElement);
      if (relativeTo == null)
        throw new InvalidOperationException("Element not in visual tree.");
      if (dob == relativeTo)
        return new Rect(0.0, 0.0, relativeTo is FrameworkElement frameworkElement1 ? frameworkElement1.ActualWidth : 0.0, frameworkElement1 != null ? frameworkElement1.ActualHeight : 0.0);
      double x1 = dob is FrameworkElement frameworkElement2 ? frameworkElement2.ActualWidth : 0.0;
      double y1 = frameworkElement2 != null ? frameworkElement2.ActualHeight : 0.0;
      Point point1 = (Point) dob.TransformToVisual(relativeTo).TransformPoint((Point) new Point());
      Point point2 = (Point) dob.TransformToVisual(relativeTo).TransformPoint((Point) new Point(x1, 0.0));
      Point point3 = (Point) dob.TransformToVisual(relativeTo).TransformPoint((Point) new Point(0.0, y1));
      Point point4 = (Point) dob.TransformToVisual(relativeTo).TransformPoint((Point) new Point(x1, y1));
      double x2 = ((IEnumerable<double>) new double[4]
      {
        point1.X,
        point2.X,
        point3.X,
        point4.X
      }).Min();
      double num1 = ((IEnumerable<double>) new double[4]
      {
        point1.X,
        point2.X,
        point3.X,
        point4.X
      }).Max();
      double y2 = ((IEnumerable<double>) new double[4]
      {
        point1.Y,
        point2.Y,
        point3.Y,
        point4.Y
      }).Min();
      double num2 = ((IEnumerable<double>) new double[4]
      {
        point1.Y,
        point2.Y,
        point3.Y,
        point4.Y
      }).Max();
      return new Rect(x2, y2, num1 - x2, num2 - y2);
    }
  }
}
