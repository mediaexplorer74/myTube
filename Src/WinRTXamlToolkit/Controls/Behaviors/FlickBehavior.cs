// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Behaviors.FlickBehavior
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls.Extensions;
using WinRTXamlToolkit.Interactivity;

namespace WinRTXamlToolkit.Controls.Behaviors
{
  public class FlickBehavior : Behavior<FrameworkElement>
  {
    private Canvas _canvas;
    private Point _startPosition;

    protected override void OnLoaded()
    {
      this._canvas = ((DependencyObject) this.AssociatedObject).GetFirstAncestorOfType<Canvas>();
      if (this._canvas == null)
        throw new InvalidOperationException("FlickBehavior can only be used on elements hosted inside of a Canvas.");
      ((UIElement) this.AssociatedObject).put_ManipulationMode((ManipulationModes) 67);
      FrameworkElement associatedObject1 = this.AssociatedObject;
      WindowsRuntimeMarshal.AddEventHandler<ManipulationStartingEventHandler>((Func<ManipulationStartingEventHandler, EventRegistrationToken>) new Func<ManipulationStartingEventHandler, EventRegistrationToken>(((UIElement) associatedObject1).add_ManipulationStarting), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) associatedObject1).remove_ManipulationStarting), new ManipulationStartingEventHandler(this.OnAssociatedObjectManipulationStarting));
      FrameworkElement associatedObject2 = this.AssociatedObject;
      WindowsRuntimeMarshal.AddEventHandler<ManipulationDeltaEventHandler>((Func<ManipulationDeltaEventHandler, EventRegistrationToken>) new Func<ManipulationDeltaEventHandler, EventRegistrationToken>(((UIElement) associatedObject2).add_ManipulationDelta), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) associatedObject2).remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnAssociatedObjectManipulationDelta));
    }

    protected override void OnUnloaded()
    {
      WindowsRuntimeMarshal.RemoveEventHandler<ManipulationStartingEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this.AssociatedObject).remove_ManipulationStarting), new ManipulationStartingEventHandler(this.OnAssociatedObjectManipulationStarting));
      WindowsRuntimeMarshal.RemoveEventHandler<ManipulationDeltaEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this.AssociatedObject).remove_ManipulationDelta), new ManipulationDeltaEventHandler(this.OnAssociatedObjectManipulationDelta));
      this._canvas = (Canvas) null;
    }

    private void OnAssociatedObjectManipulationStarting(
      object sender,
      ManipulationStartingRoutedEventArgs e)
    {
      this._startPosition = new Point(Canvas.GetLeft((UIElement) this.AssociatedObject), Canvas.GetTop((UIElement) this.AssociatedObject));
    }

    private void OnAssociatedObjectManipulationDelta(
      object sender,
      ManipulationDeltaRoutedEventArgs manipulationDeltaRoutedEventArgs)
    {
      double x = ((Point) manipulationDeltaRoutedEventArgs.Cumulative.Translation).X;
      double y = ((Point) manipulationDeltaRoutedEventArgs.Cumulative.Translation).Y;
      double num1 = this._startPosition.X + x;
      double num2 = this._startPosition.Y + y;
      if (manipulationDeltaRoutedEventArgs.IsInertial)
      {
        while (num1 < 0.0 || num1 > ((FrameworkElement) this._canvas).ActualWidth - this.AssociatedObject.ActualWidth)
        {
          if (num1 < 0.0)
            num1 = -num1;
          if (num1 > ((FrameworkElement) this._canvas).ActualWidth - this.AssociatedObject.ActualWidth)
            num1 = 2.0 * (((FrameworkElement) this._canvas).ActualWidth - this.AssociatedObject.ActualWidth) - num1;
        }
        while (num2 < 0.0 || num2 > ((FrameworkElement) this._canvas).ActualHeight - this.AssociatedObject.ActualHeight)
        {
          if (num2 < 0.0)
            num2 = -num2;
          if (num2 > ((FrameworkElement) this._canvas).ActualHeight - this.AssociatedObject.ActualHeight)
            num2 = 2.0 * (((FrameworkElement) this._canvas).ActualHeight - this.AssociatedObject.ActualHeight) - num2;
        }
      }
      else
      {
        if (num1 < 0.0)
          num1 = 0.0;
        if (num1 > ((FrameworkElement) this._canvas).ActualWidth - this.AssociatedObject.ActualWidth)
          num1 = ((FrameworkElement) this._canvas).ActualWidth - this.AssociatedObject.ActualWidth;
        if (num2 < 0.0)
          num2 = 0.0;
        if (num2 > ((FrameworkElement) this._canvas).ActualHeight - this.AssociatedObject.ActualHeight)
          num2 = ((FrameworkElement) this._canvas).ActualHeight - this.AssociatedObject.ActualHeight;
      }
      Canvas.SetLeft((UIElement) this.AssociatedObject, num1);
      Canvas.SetTop((UIElement) this.AssociatedObject, num2);
    }
  }
}
