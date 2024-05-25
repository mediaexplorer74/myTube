// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ListViewExtensions
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WinRTXamlToolkit.Controls.Extensions
{
  public static class ListViewExtensions
  {
    public static readonly DependencyProperty BindableSelectionProperty = DependencyProperty.RegisterAttached("BindableSelection", (Type) typeof (object), (Type) typeof (ListViewExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ListViewExtensions.OnBindableSelectionChanged)));
    public static readonly DependencyProperty BindableSelectionHandlerProperty = DependencyProperty.RegisterAttached("BindableSelectionHandler", (Type) typeof (ListViewBindableSelectionHandler), (Type) typeof (ListViewExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ItemToBringIntoViewProperty = DependencyProperty.RegisterAttached("ItemToBringIntoView", (Type) typeof (object), (Type) typeof (ListViewExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ListViewExtensions.OnItemToBringIntoViewChanged)));

    public static ObservableCollection<object> GetBindableSelection(DependencyObject d) => (ObservableCollection<object>) d.GetValue(ListViewExtensions.BindableSelectionProperty);

    public static void SetBindableSelection(DependencyObject d, ObservableCollection<object> value) => d.SetValue(ListViewExtensions.BindableSelectionProperty, (object) value);

    private static void OnBindableSelectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      object oldValue = e.OldValue;
      object obj1 = d.GetValue(ListViewExtensions.BindableSelectionProperty);
      // ISSUE: reference to a compiler-generated field
      if (ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite1 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
      // ISSUE: reference to a compiler-generated field
      if (ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, (Type) typeof (ListViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, oldValue, (object) null);
      if (target1((CallSite) pSite1, obj2))
      {
        ListViewBindableSelectionHandler selectionHandler = ListViewExtensions.GetBindableSelectionHandler(d);
        ListViewExtensions.SetBindableSelectionHandler(d, (ListViewBindableSelectionHandler) null);
        selectionHandler.Detach();
      }
      // ISSUE: reference to a compiler-generated field
      if (ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite3 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3;
      // ISSUE: reference to a compiler-generated field
      if (ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, (Type) typeof (ListViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4.Target((CallSite) ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4, obj1, (object) null);
      if (!target2((CallSite) pSite3, obj3))
        return;
      // ISSUE: reference to a compiler-generated field
      if (ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, Type, ListViewBase, object, ListViewBindableSelectionHandler>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, (Type) typeof (ListViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListViewBindableSelectionHandler selectionHandler1 = ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5.Target((CallSite) ListViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5, typeof (ListViewBindableSelectionHandler), (ListViewBase) d, obj1);
      ListViewExtensions.SetBindableSelectionHandler(d, selectionHandler1);
    }

    public static ListViewBindableSelectionHandler GetBindableSelectionHandler(DependencyObject d) => (ListViewBindableSelectionHandler) d.GetValue(ListViewExtensions.BindableSelectionHandlerProperty);

    public static void SetBindableSelectionHandler(
      DependencyObject d,
      ListViewBindableSelectionHandler value)
    {
      d.SetValue(ListViewExtensions.BindableSelectionHandlerProperty, (object) value);
    }

    public static object GetItemToBringIntoView(DependencyObject d) => d.GetValue(ListViewExtensions.ItemToBringIntoViewProperty);

    public static void SetItemToBringIntoView(DependencyObject d, object value) => d.SetValue(ListViewExtensions.ItemToBringIntoViewProperty, value);

    private static void OnItemToBringIntoViewChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      object obj = d.GetValue(ListViewExtensions.ItemToBringIntoViewProperty);
      if (obj == null)
        return;
      ((ListViewBase) d).ScrollIntoView(obj);
    }

    public static void ScrollToBottom(this ListView listView)
    {
      ScrollViewer descendantOfType = ((DependencyObject) listView).GetFirstDescendantOfType<ScrollViewer>();
      descendantOfType.ChangeView((double?) new double?(), (double?) new double?(descendantOfType.ScrollableHeight), (float?) new float?());
    }
  }
}
