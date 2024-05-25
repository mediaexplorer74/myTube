// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.DockPanel
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace WinRTXamlToolkit.Controls
{
  public class DockPanel : Panel
  {
    private static bool _ignorePropertyChange;
    public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register(nameof (LastChildFill), (Type) typeof (bool), (Type) typeof (DockPanel), new PropertyMetadata((object) true, new PropertyChangedCallback(DockPanel.OnLastChildFillPropertyChanged)));
    public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached("Dock", (Type) typeof (Dock), (Type) typeof (DockPanel), new PropertyMetadata((object) Dock.Left, new PropertyChangedCallback(DockPanel.OnDockPropertyChanged)));

    public bool LastChildFill
    {
      get => (bool) ((DependencyObject) this).GetValue(DockPanel.LastChildFillProperty);
      set => ((DependencyObject) this).SetValue(DockPanel.LastChildFillProperty, (object) value);
    }

    private static void OnLastChildFillPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      ((UIElement) (d as DockPanel)).InvalidateArrange();
    }

    public static Dock GetDock(UIElement element) => element != null ? (Dock) ((DependencyObject) element).GetValue(DockPanel.DockProperty) : throw new ArgumentNullException(nameof (element));

    public static void SetDock(UIElement element, Dock dock)
    {
      if (element == null)
        throw new ArgumentNullException(nameof (element));
      ((DependencyObject) element).SetValue(DockPanel.DockProperty, (object) dock);
    }

    private static void OnDockPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (DockPanel._ignorePropertyChange)
      {
        DockPanel._ignorePropertyChange = false;
      }
      else
      {
        UIElement uiElement = (UIElement) d;
        Dock newValue = (Dock) e.NewValue;
        switch (newValue)
        {
          case Dock.Left:
          case Dock.Top:
          case Dock.Right:
          case Dock.Bottom:
            if (!(VisualTreeHelper.GetParent((DependencyObject) uiElement) is DockPanel parent))
              break;
            ((UIElement) parent).InvalidateMeasure();
            break;
          default:
            DockPanel._ignorePropertyChange = true;
            ((DependencyObject) uiElement).SetValue(DockPanel.DockProperty, (object) (Dock) e.OldValue);
            throw new ArgumentException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "DockPanel_OnDockPropertyChanged_InvalidValue", (object) newValue), "value");
        }
      }
    }

    protected virtual Size MeasureOverride(Size constraint)
    {
      double val2_1 = 0.0;
      double val2_2 = 0.0;
      double val1_1 = 0.0;
      double val1_2 = 0.0;
      foreach (UIElement child in (IEnumerable<UIElement>) this.Children)
      {
        Size size = new Size(Math.Max(0.0, constraint.Width - val2_1), Math.Max(0.0, constraint.Height - val2_2));
        child.Measure((Size) size);
        Size desiredSize = (Size) child.DesiredSize;
        switch (DockPanel.GetDock(child))
        {
          case Dock.Left:
          case Dock.Right:
            val1_2 = Math.Max(val1_2, val2_2 + desiredSize.Height);
            val2_1 += desiredSize.Width;
            continue;
          case Dock.Top:
          case Dock.Bottom:
            val1_1 = Math.Max(val1_1, val2_1 + desiredSize.Width);
            val2_2 += desiredSize.Height;
            continue;
          default:
            continue;
        }
      }
      return new Size(Math.Max(val1_1, val2_1), Math.Max(val1_2, val2_2));
    }

    protected virtual Size ArrangeOverride(Size arrangeSize)
    {
      double x = 0.0;
      double y = 0.0;
      double num1 = 0.0;
      double num2 = 0.0;
      UIElementCollection children = this.Children;
      int num3 = ((ICollection<UIElement>) children).Count - (this.LastChildFill ? 1 : 0);
      int num4 = 0;
      foreach (UIElement element in (IEnumerable<UIElement>) children)
      {
        Rect rect = new Rect(x, y, Math.Max(0.0, arrangeSize.Width - x - num1), Math.Max(0.0, arrangeSize.Height - y - num2));
        if (num4 < num3)
        {
          Size desiredSize = (Size) element.DesiredSize;
          switch (DockPanel.GetDock(element))
          {
            case Dock.Left:
              x += desiredSize.Width;
              rect.Width = desiredSize.Width;
              break;
            case Dock.Top:
              y += desiredSize.Height;
              rect.Height = desiredSize.Height;
              break;
            case Dock.Right:
              num1 += desiredSize.Width;
              rect.X = Math.Max(0.0, arrangeSize.Width - num1);
              rect.Width = desiredSize.Width;
              break;
            case Dock.Bottom:
              num2 += desiredSize.Height;
              rect.Y = Math.Max(0.0, arrangeSize.Height - num2);
              rect.Height = desiredSize.Height;
              break;
          }
        }
        element.Arrange((Rect) rect);
        ++num4;
      }
      return arrangeSize;
    }
  }
}
