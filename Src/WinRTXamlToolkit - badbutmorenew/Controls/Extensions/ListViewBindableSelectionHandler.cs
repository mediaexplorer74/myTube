// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ListViewBindableSelectionHandler
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
  public class ListViewBindableSelectionHandler
  {
    private ListViewBase _listView;
    private object _boundSelection;
    private readonly NotifyCollectionChangedEventHandler _handler;

    public ListViewBindableSelectionHandler(ListViewBase listView, object boundSelection)
    {
      this._handler = new NotifyCollectionChangedEventHandler(this.OnBoundSelectionChanged);
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Action<CallSite, ListViewBindableSelectionHandler, ListViewBase, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName | CSharpBinderFlags.ResultDiscarded, "Attach", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListViewBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target((CallSite) ListViewBindableSelectionHandler.ctor\u003Eo__SiteContainer0.\u003C\u003Ep__Site1, this, listView, boundSelection);
    }

    private void Attach(ListViewBase listView, object boundSelection)
    {
      this._listView = listView;
      ListViewBase listView1 = this._listView;
      WindowsRuntimeMarshal.AddEventHandler<SelectionChangedEventHandler>((Func<SelectionChangedEventHandler, EventRegistrationToken>) new Func<SelectionChangedEventHandler, EventRegistrationToken>(((Selector) listView1).add_SelectionChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Selector) listView1).remove_SelectionChanged), new SelectionChangedEventHandler(this.OnListViewSelectionChanged));
      this._boundSelection = boundSelection;
      ((ICollection<object>) this._listView.SelectedItems).Clear();
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, (Type) typeof (IEnumerable), (Type) typeof (ListViewBindableSelectionHandler)));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      foreach (object obj in ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3.Target((CallSite) ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site3, this._boundSelection))
      {
        if (!((ICollection<object>) this._listView.SelectedItems).Contains(obj))
          ((ICollection<object>) this._listView.SelectedItems).Add(obj);
      }
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetDeclaredEvent", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target = ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> pSite4 = ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site4;
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5.Target((CallSite) ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site5, this._boundSelection);
      object obj2 = target((CallSite) pSite4, obj1, "CollectionChanged");
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6 = CallSite<Action<CallSite, object, object, NotifyCollectionChangedEventHandler>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6.Target((CallSite) ListViewBindableSelectionHandler.\u003CAttach\u003Eo__SiteContainer2.\u003C\u003Ep__Site6, obj2, this._boundSelection, this._handler);
    }

    private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      foreach (object removedItem in (IEnumerable<object>) e.RemovedItems)
      {
        // ISSUE: reference to a compiler-generated field
        if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> pSite8 = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site8;
        // ISSUE: reference to a compiler-generated field
        if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9.Target((CallSite) ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Site9, this._boundSelection, removedItem);
        if (target((CallSite) pSite8, obj))
        {
          // ISSUE: reference to a compiler-generated field
          if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Remove", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea.Target((CallSite) ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea, this._boundSelection, removedItem);
        }
      }
      foreach (object addedItem in (IEnumerable<object>) e.AddedItems)
      {
        // ISSUE: reference to a compiler-generated field
        if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> pSiteb = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb;
        // ISSUE: reference to a compiler-generated field
        if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object> target2 = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object>> pSitec = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec;
        // ISSUE: reference to a compiler-generated field
        if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited = CallSite<Func<CallSite, object, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited.Target((CallSite) ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sited, this._boundSelection, addedItem);
        object obj2 = target2((CallSite) pSitec, obj1);
        if (target1((CallSite) pSiteb, obj2))
        {
          // ISSUE: reference to a compiler-generated field
          if (ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee = CallSite<Action<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee.Target((CallSite) ListViewBindableSelectionHandler.\u003COnListViewSelectionChanged\u003Eo__SiteContainer7.\u003C\u003Ep__Sitee, this._boundSelection, addedItem);
        }
      }
    }

    private void OnBoundSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Reset)
      {
        ((ICollection<object>) this._listView.SelectedItems).Clear();
        // ISSUE: reference to a compiler-generated field
        if (ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10 = CallSite<Func<CallSite, object, IEnumerable>>.Create(Binder.Convert(CSharpBinderFlags.None, (Type) typeof (IEnumerable), (Type) typeof (ListViewBindableSelectionHandler)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (object obj1 in ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10.Target((CallSite) ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site10, this._boundSelection))
        {
          // ISSUE: reference to a compiler-generated field
          if (ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target1 = ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> pSite11 = ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site11;
          // ISSUE: reference to a compiler-generated field
          if (ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target2 = ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> pSite12 = ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site12;
          // ISSUE: reference to a compiler-generated field
          if (ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13 == null)
          {
            // ISSUE: reference to a compiler-generated field
            ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13 = CallSite<Func<CallSite, IList<object>, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Contains", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13.Target((CallSite) ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site13, (IList<object>) this._listView.SelectedItems, obj1);
          object obj3 = target2((CallSite) pSite12, obj2);
          if (target1((CallSite) pSite11, obj3))
          {
            // ISSUE: reference to a compiler-generated field
            if (ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14 == null)
            {
              // ISSUE: reference to a compiler-generated field
              ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14 = CallSite<Action<CallSite, IList<object>, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14.Target((CallSite) ListViewBindableSelectionHandler.\u003COnBoundSelectionChanged\u003Eo__SiteContainerf.\u003C\u003Ep__Site14, (IList<object>) this._listView.SelectedItems, obj1);
          }
        }
      }
      else
      {
        if (e.OldItems != null)
        {
          foreach (object oldItem in (IEnumerable) e.OldItems)
          {
            if (((ICollection<object>) this._listView.SelectedItems).Contains(oldItem))
              ((ICollection<object>) this._listView.SelectedItems).Remove(oldItem);
          }
        }
        if (e.NewItems == null)
          return;
        foreach (object newItem in (IEnumerable) e.NewItems)
        {
          if (!((ICollection<object>) this._listView.SelectedItems).Contains(newItem))
            ((ICollection<object>) this._listView.SelectedItems).Add(newItem);
        }
      }
    }

    internal void Detach()
    {
      WindowsRuntimeMarshal.RemoveEventHandler<SelectionChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((Selector) this._listView).remove_SelectionChanged), new SelectionChangedEventHandler(this.OnListViewSelectionChanged));
      this._listView = (ListViewBase) null;
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetDeclaredEvent", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, string, object> target = ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, string, object>> pSite16 = ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site16;
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17 = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetType", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17.Target((CallSite) ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site17, this._boundSelection);
      object obj2 = target((CallSite) pSite16, obj1, "CollectionChanged");
      // ISSUE: reference to a compiler-generated field
      if (ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18 = CallSite<Action<CallSite, object, object, NotifyCollectionChangedEventHandler>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, (Type) typeof (ListViewBindableSelectionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18.Target((CallSite) ListViewBindableSelectionHandler.\u003CDetach\u003Eo__SiteContainer15.\u003C\u003Ep__Site18, obj2, this._boundSelection, this._handler);
      this._boundSelection = (object) null;
    }
  }
}
