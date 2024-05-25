// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.ListBoxExtensions
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
  public static class ListBoxExtensions
  {
    public static readonly DependencyProperty BindableSelectionProperty = DependencyProperty.RegisterAttached("BindableSelection", (Type) typeof (object), (Type) typeof (ListBoxExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ListBoxExtensions.OnBindableSelectionChanged)));
    public static readonly DependencyProperty BindableSelectionHandlerProperty = DependencyProperty.RegisterAttached("BindableSelectionHandler", (Type) typeof (ListBoxBindableSelectionHandler), (Type) typeof (ListBoxExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ItemToBringIntoViewProperty = DependencyProperty.RegisterAttached("ItemToBringIntoView", (Type) typeof (object), (Type) typeof (ListBoxExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(ListBoxExtensions.OnItemToBringIntoViewChanged)));

    public static ObservableCollection<object> GetBindableSelection(DependencyObject d) => (ObservableCollection<object>) d.GetValue(ListBoxExtensions.BindableSelectionProperty);

    public static void SetBindableSelection(DependencyObject d, ObservableCollection<object> value) => d.SetValue(ListBoxExtensions.BindableSelectionProperty, (object) value);

    private static void OnBindableSelectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      object oldValue = e.OldValue;
      object obj1 = d.GetValue(ListBoxExtensions.BindableSelectionProperty);
      // ISSUE: reference to a compiler-generated field
      if (ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListBoxExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite1 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
      // ISSUE: reference to a compiler-generated field
      if (ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, (Type) typeof (ListBoxExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, oldValue, (object) null);
      if (target1((CallSite) pSite1, obj2))
      {
        ListBoxBindableSelectionHandler selectionHandler = ListBoxExtensions.GetBindableSelectionHandler(d);
        ListBoxExtensions.SetBindableSelectionHandler(d, (ListBoxBindableSelectionHandler) null);
        selectionHandler.Detach();
      }
      // ISSUE: reference to a compiler-generated field
      if (ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (ListBoxExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite3 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3;
      // ISSUE: reference to a compiler-generated field
      if (ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, (Type) typeof (ListBoxExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4.Target((CallSite) ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4, obj1, (object) null);
      if (!target2((CallSite) pSite3, obj3))
        return;
      // ISSUE: reference to a compiler-generated field
      if (ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, Type, ListBox, object, ListBoxBindableSelectionHandler>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, (Type) typeof (ListBoxExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ListBoxBindableSelectionHandler selectionHandler1 = ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5.Target((CallSite) ListBoxExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5, typeof (ListBoxBindableSelectionHandler), (ListBox) d, obj1);
      ListBoxExtensions.SetBindableSelectionHandler(d, selectionHandler1);
    }

    public static ListBoxBindableSelectionHandler GetBindableSelectionHandler(DependencyObject d) => (ListBoxBindableSelectionHandler) d.GetValue(ListBoxExtensions.BindableSelectionHandlerProperty);

    public static void SetBindableSelectionHandler(
      DependencyObject d,
      ListBoxBindableSelectionHandler value)
    {
      d.SetValue(ListBoxExtensions.BindableSelectionHandlerProperty, (object) value);
    }

    public static object GetItemToBringIntoView(DependencyObject d) => d.GetValue(ListBoxExtensions.ItemToBringIntoViewProperty);

    public static void SetItemToBringIntoView(DependencyObject d, object value) => d.SetValue(ListBoxExtensions.ItemToBringIntoViewProperty, value);

    private static void OnItemToBringIntoViewChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      object obj = d.GetValue(ListBoxExtensions.ItemToBringIntoViewProperty);
      if (obj == null)
        return;
      ((ListBox) d).ScrollIntoView(obj);
    }

    public static void ScrollToBottom(this ListBox listBox)
    {
      ScrollViewer descendantOfType = ((DependencyObject) listBox).GetFirstDescendantOfType<ScrollViewer>();
      descendantOfType.ChangeView((double?) new double?(), (double?) new double?(descendantOfType.ScrollableHeight), (float?) new float?());
    }
  }
}
