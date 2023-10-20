// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.Extensions.GridViewExtensions
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
  public static class GridViewExtensions
  {
    public static readonly DependencyProperty BindableSelectionProperty = DependencyProperty.RegisterAttached("BindableSelection", (Type) typeof (object), (Type) typeof (GridViewExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(GridViewExtensions.OnBindableSelectionChanged)));
    public static readonly DependencyProperty BindableSelectionHandlerProperty = DependencyProperty.RegisterAttached("BindableSelectionHandler", (Type) typeof (GridViewBindableSelectionHandler), (Type) typeof (GridViewExtensions), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ItemToBringIntoViewProperty = DependencyProperty.RegisterAttached("ItemToBringIntoView", (Type) typeof (object), (Type) typeof (GridViewExtensions), new PropertyMetadata((object) null, new PropertyChangedCallback(GridViewExtensions.OnItemToBringIntoViewChanged)));

    public static ObservableCollection<object> GetBindableSelection(DependencyObject d) => (ObservableCollection<object>) d.GetValue(GridViewExtensions.BindableSelectionProperty);

    public static void SetBindableSelection(DependencyObject d, ObservableCollection<object> value) => d.SetValue(GridViewExtensions.BindableSelectionProperty, (object) value);

    private static void OnBindableSelectionChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      object oldValue = e.OldValue;
      object obj1 = d.GetValue(GridViewExtensions.BindableSelectionProperty);
      // ISSUE: reference to a compiler-generated field
      if (GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (GridViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite1 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
      // ISSUE: reference to a compiler-generated field
      if (GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, (Type) typeof (GridViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, oldValue, (object) null);
      if (target1((CallSite) pSite1, obj2))
      {
        GridViewBindableSelectionHandler selectionHandler = GridViewExtensions.GetBindableSelectionHandler(d);
        GridViewExtensions.SetBindableSelectionHandler(d, (GridViewBindableSelectionHandler) null);
        selectionHandler.Detach();
      }
      // ISSUE: reference to a compiler-generated field
      if (GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, (Type) typeof (GridViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target2 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite3 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site3;
      // ISSUE: reference to a compiler-generated field
      if (GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, (Type) typeof (GridViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4.Target((CallSite) GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site4, obj1, (object) null);
      if (!target2((CallSite) pSite3, obj3))
        return;
      // ISSUE: reference to a compiler-generated field
      if (GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 == null)
      {
        // ISSUE: reference to a compiler-generated field
        GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, Type, GridView, object, GridViewBindableSelectionHandler>>.Create(Binder.InvokeConstructor(CSharpBinderFlags.None, (Type) typeof (GridViewExtensions), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      GridViewBindableSelectionHandler selectionHandler1 = GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5.Target((CallSite) GridViewExtensions.\u003COnBindableSelectionChanged\u003Eo__SiteContainer0.\u003C\u003Ep__Site5, typeof (GridViewBindableSelectionHandler), (GridView) d, obj1);
      GridViewExtensions.SetBindableSelectionHandler(d, selectionHandler1);
    }

    public static GridViewBindableSelectionHandler GetBindableSelectionHandler(DependencyObject d) => (GridViewBindableSelectionHandler) d.GetValue(GridViewExtensions.BindableSelectionHandlerProperty);

    public static void SetBindableSelectionHandler(
      DependencyObject d,
      GridViewBindableSelectionHandler value)
    {
      d.SetValue(GridViewExtensions.BindableSelectionHandlerProperty, (object) value);
    }

    public static object GetItemToBringIntoView(DependencyObject d) => d.GetValue(GridViewExtensions.ItemToBringIntoViewProperty);

    public static void SetItemToBringIntoView(DependencyObject d, object value) => d.SetValue(GridViewExtensions.ItemToBringIntoViewProperty, value);

    private static void OnItemToBringIntoViewChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      object obj = d.GetValue(GridViewExtensions.ItemToBringIntoViewProperty);
      if (obj == null)
        return;
      ((ListViewBase) d).ScrollIntoView(obj);
    }

    public static void ScrollToBottom(this GridView GridView)
    {
      ScrollViewer descendantOfType = ((DependencyObject) GridView).GetFirstDescendantOfType<ScrollViewer>();
      descendantOfType.ChangeView((double?) new double?(), (double?) new double?(descendantOfType.ScrollableHeight), (float?) new float?());
    }
  }
}
