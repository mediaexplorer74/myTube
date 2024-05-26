// Decompiled with JetBrains decompiler
// Type: myTube.Helpers.InputPaneHelper
// Assembly: myTube.WindowsPhone, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B5B96F9E-0572-4971-BFB4-9D68A15DDB38
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\myTube.WindowsPhone.exe

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
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Func<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>, EventRegistrationToken>(pane1.add_Showing), new Action<EventRegistrationToken>(pane1.remove_Showing), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) null, __methodptr(pane_Showing)));
      InputPane pane2 = InputPaneHelper.pane;
      // ISSUE: method pointer
      WindowsRuntimeMarshal.AddEventHandler<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>>(new Func<TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>, EventRegistrationToken>(pane2.add_Hiding), new Action<EventRegistrationToken>(pane2.remove_Hiding), new TypedEventHandler<InputPane, InputPaneVisibilityEventArgs>((object) null, __methodptr(pane_Hiding)));
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
      using (List<InputPaneHelper.ElementAndTransform>.Enumerator enumerator = InputPaneHelper.elements.GetEnumerator())
      {
        while (enumerator.MoveNext())
          Ani.Begin((DependencyObject) enumerator.Current.Trans, "Y", 0.0, 0.5, 5.0);
      }
    }

    private static void pane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
    {
      using (List<InputPaneHelper.ElementAndTransform>.Enumerator enumerator = InputPaneHelper.elements.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          InputPaneHelper.ElementAndTransform current = enumerator.Current;
          if (!(current.Element is TextBox) || current.Element is TextBox && ((Control) (current.Element as TextBox)).FocusState != null)
          {
            double num = InputPaneHelper.pane.OccludedRect.Top - current.Element.GetBounds((UIElement) DefaultPage.Current).Bottom;
            if (num < 0.0)
            {
              double To = num - 19.0;
              Ani.Begin((DependencyObject) current.Trans, "Y", To, 0.5, 5.0);
            }
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
