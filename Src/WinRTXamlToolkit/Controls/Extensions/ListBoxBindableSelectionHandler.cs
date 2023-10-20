// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ListBoxBindableSelectionHandler
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public class ListBoxBindableSelectionHandler
  {
    private ListBox _listBox;
    private object _boundSelection;
    private readonly NotifyCollectionChangedEventHandler _handler;

    public ListBoxBindableSelectionHandler(ListBox listBox, object boundSelection)
    {
      this._handler = new NotifyCollectionChangedEventHandler(this.OnBoundSelectionChanged);
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Action<CallSite, ListBoxBindableSelectionHandler, ListBox, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "Attach", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListBoxBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target((CallSite) ListBoxBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1, this, listBox, boundSelection);
    }

    private void Attach(ListBox listBox, object boundSelection)
    {
      this._listBox = listBox;
      ListBox listBox1 = this._listBox;
      WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>((Func<SelectionChangedEventHandler, EventRegistrationToken>) new Func<SelectionChangedEventHandler, EventRegistrationToken>(((Selector) listBox1).add_SelectionChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Selector) listBox1).remove_SelectionChanged), new SelectionChangedEventHandler(this.OnListBoxSelectionChanged));
      this._boundSelection = boundSelection;
      ((ICollection<object>) this._listBox.SelectedItems).Clear();
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, (Type) typeof (IEnumerable), (Type) typeof (ListBoxBindableSelectionHandler)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj in ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3.Target((CallSite) ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3, this._boundSelection))
      {
        if (!((ICollection<object>) this._listBox.SelectedItems).Contains(obj))
          ((ICollection<object>) this._listBox.SelectedItems).Add(obj);
      }
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetDeclaredEvent", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target = ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> pSite4 = ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4;
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5.Target((CallSite) ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5, this._boundSelection);
      object obj2 = target((CallSite) pSite4, obj1, "CollectionChanged");
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6 = CallSite<Action<CallSite, object, object, NotifyCollectionChangedEventHandler>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6.Target((CallSite) ListBoxBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6, obj2, this._boundSelection, this._handler);
    }

    private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      foreach (object removedItem in (IEnumerable<object>) e.RemovedItems)
      {
        // ISSUE: reference to a compiler-generated field
        if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> pSite8 = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8;
        // ISSUE: reference to a compiler-generated field
        if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9, this._boundSelection, removedItem);
        if (target((CallSite) pSite8, obj))
        {
          // ISSUE: reference to a compiler-generated field
          if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea, this._boundSelection, removedItem);
        }
      }
      foreach (object addedItem in (IEnumerable<object>) e.AddedItems)
      {
        // ISSUE: reference to a compiler-generated field
        if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> pSiteb = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb;
        // ISSUE: reference to a compiler-generated field
        if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> pSitec = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec;
        // ISSUE: reference to a compiler-generated field
        if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited, this._boundSelection, addedItem);
        object obj2 = target2((CallSite) pSitec, obj1);
        if (target1((CallSite) pSiteb, obj2))
        {
          // ISSUE: reference to a compiler-generated field
          if (ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnListBoxSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee, this._boundSelection, addedItem);
        }
      }
    }

    private void OnBoundSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        ((ICollection<object>) this._listBox.SelectedItems).Clear();
        // ISSUE: reference to a compiler-generated field
        if (ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, (Type) typeof (IEnumerable), (Type) typeof (ListBoxBindableSelectionHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (object obj1 in ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10, this._boundSelection))
        {
          // ISSUE: reference to a compiler-generated field
          if (ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target1 = ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> pSite11 = ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11;
          // ISSUE: reference to a compiler-generated field
          if (ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target2 = ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> pSite12 = ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12;
          // ISSUE: reference to a compiler-generated field
          if (ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13 = CallSite<Func<CallSite, IList<object>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13, (IList<object>) this._listBox.SelectedItems, obj1);
          object obj3 = target2((CallSite) pSite12, obj2);
          if (target1((CallSite) pSite11, obj3))
          {
            // ISSUE: reference to a compiler-generated field
            if (ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14 = CallSite<Action<CallSite, IList<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14.Target((CallSite) ListBoxBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14, (IList<object>) this._listBox.SelectedItems, obj1);
          }
        }
      }
      else
      {
        if (e.OldItems != null)
        {
          foreach (object oldItem in (IEnumerable) e.OldItems)
          {
            if (((ICollection<object>) this._listBox.SelectedItems).Contains(oldItem))
              ((ICollection<object>) this._listBox.SelectedItems).Remove(oldItem);
          }
        }
        if (e.NewItems == null)
          return;
        foreach (object newItem in (IEnumerable) e.NewItems)
        {
          if (!((ICollection<object>) this._listBox.SelectedItems).Contains(newItem))
            ((ICollection<object>) this._listBox.SelectedItems).Add(newItem);
        }
      }
    }

    internal void Detach()
    {
      WindowsRuntimeMarshal.RemoveEventHandler<SelectionChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Selector) this._listBox).remove_SelectionChanged), new SelectionChangedEventHandler(this.OnListBoxSelectionChanged));
      this._listBox = (ListBox) null;
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetDeclaredEvent", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target = ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> pSite16 = ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16;
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17.Target((CallSite) ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17, this._boundSelection);
      object obj2 = target((CallSite) pSite16, obj1, "CollectionChanged");
      // ISSUE: reference to a compiler-generated field
      if (ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18 = CallSite<Action<CallSite, object, object, NotifyCollectionChangedEventHandler>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, (Type) typeof (ListBoxBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18.Target((CallSite) ListBoxBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18, obj2, this._boundSelection, this._handler);
      this._boundSelection = (object) null;
    }
  }
}
