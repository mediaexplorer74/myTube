// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ViewboxExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ViewboxExtensions
  {
    public static double GetChildScaleX(this Viewbox viewbox)
    {
      if (viewbox.Child == null)
        throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with no child.");
      if (!(viewbox.Child is FrameworkElement child))
        throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not a FrameworkElement.");
      if (child.ActualWidth == 0.0)
        throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not laid out.");
      return ((FrameworkElement) viewbox).ActualWidth / child.ActualWidth;
    }

    public static double GetChildScaleY(this Viewbox viewbox)
    {
      if (viewbox.Child == null)
        throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with no child.");
      if (!(viewbox.Child is FrameworkElement child))
        throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not a FrameworkElement.");
      if (child.ActualHeight == 0.0)
        throw new InvalidOperationException("Can't tell effective scale of a Viewbox child for a Viewbox with a child that is not laid out.");
      return ((FrameworkElement) viewbox).ActualHeight / child.ActualHeight;
    }
  }
}
