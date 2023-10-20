// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Interactivity.BehaviorCollection
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.UI.Xaml;

namespace WinRTXamlToolkit.Interactivity
{
  public class BehaviorCollection : ObservableCollection<Behavior>
  {
    protected DependencyObject AssociatedObject { get; private set; }

    public void Attach(DependencyObject dependencyObject)
    {
      this.AssociatedObject = this.AssociatedObject == null ? dependencyObject : throw new InvalidOperationException("The BehaviorCollection is already attached to a different object.");
      foreach (Behavior behavior in (Collection<Behavior>) this)
        behavior.Attach(dependencyObject);
      this.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      this.OnAttached();
    }

    public void Detach()
    {
      this.OnDetaching();
      foreach (Behavior behavior in (Collection<Behavior>) this)
        behavior.Detach();
      this.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
      this.AssociatedObject = (DependencyObject) null;
    }

    protected virtual void OnAttached()
    {
    }

    protected virtual void OnDetaching()
    {
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          IEnumerator enumerator1 = ((IEnumerable) e.NewItems).GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
              ((Behavior) enumerator1.Current).Attach(this.AssociatedObject);
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case NotifyCollectionChangedAction.Remove:
        case NotifyCollectionChangedAction.Reset:
          IEnumerator enumerator2 = ((IEnumerable) e.OldItems).GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
              ((Behavior) enumerator2.Current).Detach();
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
      }
    }
  }
}
