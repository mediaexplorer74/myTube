// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Interactivity.Behavior
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinRTXamlToolkit.Interactivity
{
  public abstract class Behavior : FrameworkElement
  {
    protected internal DependencyObject _associatedObject;
    protected internal Type _associatedType = typeof (object);

    protected DependencyObject AssociatedObject => this._associatedObject;

    protected Type AssociatedType => this._associatedType;

    public void Attach(DependencyObject dependencyObject)
    {
      if (this.AssociatedObject != null)
        throw new InvalidOperationException("The Behavior is already hosted on a different element.");
      this._associatedObject = dependencyObject;
      if (dependencyObject == null)
        return;
      if (!((Type) this.AssociatedType).GetTypeInfo().IsAssignableFrom(((Type) dependencyObject.GetType()).GetTypeInfo()))
        throw new InvalidOperationException("dependencyObject does not satisfy the Behavior type constraint.");
      if (this.AssociatedObject is FrameworkElement associatedObject)
      {
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(associatedObject.add_Loaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(associatedObject.remove_Loaded), new RoutedEventHandler(this.AssociatedFrameworkElementLoaded));
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(associatedObject.add_Unloaded), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(associatedObject.remove_Unloaded), new RoutedEventHandler(this.AssociatedFrameworkElementUnloaded));
      }
      this.OnAttached();
    }

    public void Detach()
    {
      if (this.AssociatedObject == null)
        return;
      this.OnDetaching();
      if (this.AssociatedObject is FrameworkElement associatedObject)
      {
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(associatedObject.remove_Loaded), new RoutedEventHandler(this.AssociatedFrameworkElementLoaded));
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(associatedObject.remove_Unloaded), new RoutedEventHandler(this.AssociatedFrameworkElementUnloaded));
      }
      this._associatedObject = (DependencyObject) null;
    }

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

    protected virtual void OnLoaded()
    {
    }

    protected virtual void OnUnloaded()
    {
    }

    private void AssociatedFrameworkElementLoaded(object sender, RoutedEventArgs e)
    {
      DependencyProperty dataContextProperty = FrameworkElement.DataContextProperty;
      Binding binding1 = new Binding();
      binding1.put_Path(new PropertyPath("DataContext"));
      binding1.put_Source((object) this._associatedObject);
      Binding binding2 = binding1;
      this.SetBinding(dataContextProperty, (BindingBase) binding2);
      this.OnLoaded();
    }

    private void AssociatedFrameworkElementUnloaded(object sender, RoutedEventArgs e)
    {
      this.OnUnloaded();
      ((DependencyObject) this).ClearValue(FrameworkElement.DataContextProperty);
      this.put_DataContext((object) null);
    }
  }
}
