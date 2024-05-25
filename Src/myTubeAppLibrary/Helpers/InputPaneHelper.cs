// myTube.Helpers.InputPaneHelper


using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace myTube.Helpers
{
  public static class InputPaneHelper
  {
    private static List<InputPaneHelper.ElementAndTransform> elements = new List<InputPaneHelper.ElementAndTransform>();
    private static InputPane pane = InputPane.GetForCurrentView();

    static InputPaneHelper()
    {
      InputPane pane1 = InputPaneHelper.pane;
           
      pane1.Showing += new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>(InputPaneHelper.pane_Showing);

      InputPane pane2 = InputPaneHelper.pane;
     
      pane2.Hiding += new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>(InputPaneHelper.pane_Hiding);
    }

    public static void Register(FrameworkElement el, TranslateTransform trans)
    {
      foreach (TextBox child in Helper.FindChildren<TextBox>((DependencyObject) el, 300))
        InputPaneHelper.elements.Add(new InputPaneHelper.ElementAndTransform()
        {
          Element = (FrameworkElement) child,
          Trans = trans
        });
    }

    public static void Deregister(FrameworkElement el)
    {
      for (int index = 0; index < InputPaneHelper.elements.Count; ++index)
      {
        if (InputPaneHelper.elements[index].Element == el)
        {
          InputPaneHelper.elements.RemoveAt(index);
          break;
        }
      }
      foreach (TextBox child in Helper.FindChildren<TextBox>((DependencyObject) el, 300))
      {
        for (int index = 0; index < InputPaneHelper.elements.Count; ++index)
        {
          if (InputPaneHelper.elements[index].Element == child)
          {
            InputPaneHelper.elements.RemoveAt(index);
            break;
          }
        }
      }
    }

    private static void pane_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
    {
      foreach (InputPaneHelper.ElementAndTransform element in InputPaneHelper.elements)
        Ani.Begin((DependencyObject) element.Trans, "Y", 0.0, 0.5, 5.0);
    }

    private static void pane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
    {
      foreach (InputPaneHelper.ElementAndTransform element in InputPaneHelper.elements)
      {
        if (!(element.Element is TextBox) 
             || element.Element is TextBox && ((Control) (element.Element as TextBox)).FocusState != null)
        {
          double num = InputPaneHelper.pane.OccludedRect.Top 
                        - element.Element.GetBounds(Window.Current.Content).Bottom;
          if (num < 0.0)
          {
            double To = num - 19.0;
            Ani.Begin((DependencyObject) element.Trans, "Y", To, 0.5, 5.0);
          }
        }
      }
    }

    private class ElementAndTransform
    {
      public FrameworkElement Element;
      public TranslateTransform Trans;
    }
  }
}
