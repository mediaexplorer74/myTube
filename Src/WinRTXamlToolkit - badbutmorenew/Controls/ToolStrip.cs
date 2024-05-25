// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.ToolStrip
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using WinRTXamlToolkit.Controls.Extensions;

namespace WinRTXamlToolkit.Controls
{
  [TemplatePart(Name = "BarPanel", Type = typeof (StackPanel))]
  [TemplatePart(Name = "DropDownPanel", Type = typeof (StackPanel))]
  [ContentProperty(Name = "Elements")]
  [TemplatePart(Name = "BarGrid", Type = typeof (Grid))]
  [TemplatePart(Name = "DropDownPopup", Type = typeof (Popup))]
  [TemplatePart(Name = "DropDownButton", Type = typeof (Button))]
  public sealed class ToolStrip : Control
  {
    private const string BarGridName = "BarGrid";
    private const string BarPanelName = "BarPanel";
    private const string DropDownButtonName = "DropDownButton";
    private const string DropDownPopupName = "DropDownPopup";
    private const string DropDownPanelName = "DropDownPanel";
    private Grid _barGrid;
    private StackPanel _barPanel;
    private Button _dropDownButton;
    private Popup _dropDownPopup;
    private StackPanel _dropDownPanel;
    private List<IToolStripElement> _barElements = new List<IToolStripElement>();
    private List<IToolStripElement> _overflowElements = new List<IToolStripElement>();

    public ObservableCollection<IToolStripElement> Elements { get; private set; }

    public ToolStrip()
    {
      this.put_DefaultStyleKey((object) typeof (ToolStrip));
      this.Elements = new ObservableCollection<IToolStripElement>();
      this.Elements.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnElementsCollectionChanged);
      ToolStrip toolStrip = this;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) toolStrip).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) toolStrip).remove_SizeChanged), new SizeChangedEventHandler(this.OnSizeChanged));
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) => this.ReflowElements();

    private void OnElementsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.ReflowElements();
      if (e.OldItems != null)
      {
        foreach (ButtonBase buttonBase in (IEnumerable<ButtonBase>) e.OldItems.OfType<ButtonBase>())
          WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) buttonBase).add_KeyDown), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) buttonBase).remove_KeyDown), new KeyEventHandler(this.OnButtonKeyDown));
      }
      if (e.NewItems == null)
        return;
      foreach (ButtonBase buttonBase in (IEnumerable<ButtonBase>) e.NewItems.OfType<ButtonBase>())
        WindowsRuntimeMarshal.AddEventHandler<KeyEventHandler>((Func<KeyEventHandler, EventRegistrationToken>) new Func<KeyEventHandler, EventRegistrationToken>(((UIElement) buttonBase).add_KeyDown), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) buttonBase).remove_KeyDown), new KeyEventHandler(this.OnButtonKeyDown));
    }

    private void OnButtonKeyDown(object sender, KeyRoutedEventArgs e)
    {
      if (this._dropDownPanel == null || this._barPanel == null || this._dropDownButton == null || this._dropDownPopup == null)
        return;
      IToolStripElement element;
      ButtonBase button;
      ToolStrip.CastAndVerifyItem(sender, out element, out button);
      if (e.Key == 40 || e.Key == 39)
      {
        if (element.IsInDropDown)
        {
          int num = ((IList<UIElement>) ((Panel) this._dropDownPanel).Children).IndexOf((UIElement) button);
          if (((ICollection<UIElement>) ((Panel) this._dropDownPanel).Children).Count <= num || !(((IList<UIElement>) ((Panel) this._dropDownPanel).Children)[num + 1] is ButtonBase child) || !((Control) child).IsTabStop)
            return;
          ((Control) child).Focus((FocusState) 2);
        }
        else
        {
          int num = ((IList<UIElement>) ((Panel) this._barPanel).Children).IndexOf((UIElement) button);
          if (num < ((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count - 2)
          {
            if (!(((IList<UIElement>) ((Panel) this._barPanel).Children)[num + 1] is ButtonBase child) || !((Control) child).IsTabStop)
              return;
            ((Control) child).Focus((FocusState) 2);
          }
          else
          {
            if (((ICollection<UIElement>) ((Panel) this._dropDownPanel).Children).Count <= 0)
              return;
            this.PositionDropDownPopup();
            this._dropDownPopup.put_IsOpen(true);
            if (!(((IList<UIElement>) ((Panel) this._dropDownPanel).Children)[0] is ButtonBase child) || !((Control) child).IsTabStop)
              return;
            ((Control) child).Focus((FocusState) 2);
          }
        }
      }
      else
      {
        if (e.Key != 38 && e.Key != 37)
          return;
        if (element.IsInDropDown)
        {
          int num = ((IList<UIElement>) ((Panel) this._dropDownPanel).Children).IndexOf((UIElement) button);
          if (num > 0)
          {
            if (!(((IList<UIElement>) ((Panel) this._dropDownPanel).Children)[num - 1] is ButtonBase child) || !((Control) child).IsTabStop)
              return;
            ((Control) child).Focus((FocusState) 2);
          }
          else
          {
            if (((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count <= 1 || !(((IList<UIElement>) ((Panel) this._barPanel).Children)[((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count - 2] is ButtonBase child) || !((Control) child).IsTabStop)
              return;
            ((Control) child).Focus((FocusState) 2);
          }
        }
        else
        {
          int num = ((IList<UIElement>) ((Panel) this._barPanel).Children).IndexOf((UIElement) button);
          if (num <= 0 || !(((IList<UIElement>) ((Panel) this._barPanel).Children)[num - 1] is ButtonBase child) || !((Control) child).IsTabStop)
            return;
          ((Control) child).Focus((FocusState) 2);
        }
      }
    }

    private void ReflowElements()
    {
      if (this._barPanel == null || this._dropDownPanel == null || this._barGrid == null || this._dropDownButton == null)
        return;
      this._barElements.Clear();
      this._overflowElements.Clear();
      int index1 = 0;
      while (index1 < ((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count)
      {
        if (((IList<UIElement>) ((Panel) this._barPanel).Children)[index1] != this._dropDownButton)
          ((IList<UIElement>) ((Panel) this._barPanel).Children).RemoveAt(index1);
        else
          ++index1;
      }
      ((ICollection<UIElement>) ((Panel) this._dropDownPanel).Children).Clear();
      bool flag = true;
      int index2 = 0;
      ((UIElement) this._dropDownButton).put_Visibility((Visibility) 1);
      foreach (object element1 in (Collection<IToolStripElement>) this.Elements)
      {
        IToolStripElement element2;
        ButtonBase button;
        ToolStrip.CastAndVerifyItem(element1, out element2, out button);
        if (flag)
        {
          element2.IsInDropDown = false;
          ((IList<UIElement>) ((Panel) this._barPanel).Children).Insert(index2, (UIElement) button);
          ((UIElement) this._barPanel).Measure((Size) new Size(10000.0, 10000.0));
          if (((Size) ((UIElement) this._barPanel).DesiredSize).Width < ((FrameworkElement) this._barGrid).ActualWidth)
          {
            ++index2;
            continue;
          }
          flag = false;
          ((ICollection<UIElement>) ((Panel) this._barPanel).Children).Remove((UIElement) button);
          ((UIElement) this._dropDownButton).put_Visibility((Visibility) 0);
          index2 = 0;
          ((UIElement) this._barPanel).Measure((Size) new Size(10000.0, 10000.0));
          while (((Size) ((UIElement) this._barPanel).DesiredSize).Width > ((FrameworkElement) this._barGrid).ActualWidth && ((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count > 1)
          {
            IToolStripElement child = (IToolStripElement) ((IList<UIElement>) ((Panel) this._barPanel).Children)[((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count - 2];
            ButtonBase buttonBase = (ButtonBase) child;
            ((IList<UIElement>) ((Panel) this._barPanel).Children).RemoveAt(((ICollection<UIElement>) ((Panel) this._barPanel).Children).Count - 2);
            child.IsInDropDown = true;
            ((IList<UIElement>) ((Panel) this._dropDownPanel).Children).Insert(0, (UIElement) buttonBase);
            ((UIElement) this._barPanel).Measure((Size) new Size(10000.0, 10000.0));
          }
        }
        if (!flag)
        {
          element2.IsInDropDown = true;
          ((ICollection<UIElement>) ((Panel) this._dropDownPanel).Children).Add((UIElement) button);
        }
      }
    }

    private void OnElementAdded(object newItem) => ToolStrip.CastAndVerifyItem(newItem, out IToolStripElement _, out ButtonBase _);

    private static void CastAndVerifyItem(
      object item,
      out IToolStripElement element,
      out ButtonBase button)
    {
      element = item != null ? item as IToolStripElement : throw new ArgumentNullException(nameof (element), "ToolStrip.Elements doesn't accept null elements.");
      if (element == null)
        throw new ArgumentException("ToolStrip.Elements only accepts ButtonBase, IToolStripElement elements.", nameof (element));
      button = item as ButtonBase;
      if (button == null)
        throw new ArgumentException("ToolStrip.Elements only accepts ButtonBase, IToolStripElement elements.", nameof (element));
    }

    private void OnElementRemoved(object oldItem)
    {
    }

    protected virtual void OnApplyTemplate()
    {
      ((FrameworkElement) this).OnApplyTemplate();
      this.UnregisterTemplateParts();
      this.RegisterTemplateParts();
      this.ReflowElements();
    }

    private void RegisterTemplateParts()
    {
      this._barGrid = this.GetTemplateChild("BarGrid") as Grid;
      this._barPanel = this.GetTemplateChild("BarPanel") as StackPanel;
      this._dropDownButton = this.GetTemplateChild("DropDownButton") as Button;
      this._dropDownPopup = this.GetTemplateChild("DropDownPopup") as Popup;
      this._dropDownPanel = this.GetTemplateChild("DropDownPanel") as StackPanel;
      if (this._barGrid != null && this._dropDownPopup != null && ((ICollection<UIElement>) ((Panel) this._barGrid).Children).Contains((UIElement) this._dropDownPopup))
        ((ICollection<UIElement>) ((Panel) this._barGrid).Children).Remove((UIElement) this._dropDownPopup);
      if (this._dropDownButton != null)
      {
        Button dropDownButton = this._dropDownButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) dropDownButton).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) dropDownButton).remove_Click), new RoutedEventHandler(this.OnDropDownButtonClick));
      }
      if (this._dropDownPanel == null)
        return;
      StackPanel dropDownPanel = this._dropDownPanel;
      WindowsRuntimeMarshal.AddEventHandler<SizeChangedEventHandler>((Func<SizeChangedEventHandler, EventRegistrationToken>) new Func<SizeChangedEventHandler, EventRegistrationToken>(((FrameworkElement) dropDownPanel).add_SizeChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) dropDownPanel).remove_SizeChanged), new SizeChangedEventHandler(this.OnDropDownPanelSizeChanged));
    }

    private void UnregisterTemplateParts()
    {
      if (this._dropDownButton != null)
        WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) this._dropDownButton).remove_Click), new RoutedEventHandler(this.OnDropDownButtonClick));
      if (this._dropDownPanel == null)
        return;
      WindowsRuntimeMarshal.RemoveEventHandler<SizeChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((FrameworkElement) this._dropDownPanel).remove_SizeChanged), new SizeChangedEventHandler(this.OnDropDownPanelSizeChanged));
    }

    private void OnDropDownPanelSizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (!this._dropDownPopup.IsOpen)
        return;
      this.PositionDropDownPopup();
    }

    private void PositionDropDownPopup()
    {
      Rect boundingRect = ((UIElement) this._dropDownButton).GetBoundingRect();
      Rect bounds = (Rect) Window.Current.Bounds;
      double actualHeight = ((FrameworkElement) this._dropDownPanel).ActualHeight;
      double actualWidth = ((FrameworkElement) this._dropDownPanel).ActualWidth;
      double num = bounds.Height - boundingRect.Bottom;
      double top = boundingRect.Top;
      ((FrameworkElement) this._dropDownPopup).put_Width(actualWidth);
      ((FrameworkElement) this._dropDownPopup).put_Height(actualHeight);
      if (num >= actualHeight || top < num)
      {
        this._dropDownPopup.put_VerticalOffset(boundingRect.Bottom);
        if (num < actualHeight && top < num)
          ((FrameworkElement) this._dropDownPopup).put_Height(num);
      }
      else
      {
        this._dropDownPopup.put_VerticalOffset(Math.Max(0.0, boundingRect.Top - actualHeight));
        if (top < actualHeight && top > num)
          ((FrameworkElement) this._dropDownPopup).put_Height(top);
      }
      if (boundingRect.Right > actualWidth)
        this._dropDownPopup.put_HorizontalOffset(boundingRect.Right - actualWidth);
      else
        this._dropDownPopup.put_HorizontalOffset(0.0);
    }

    private void OnDropDownButtonClick(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this._dropDownPopup == null || this._dropDownPanel == null || ((ICollection<UIElement>) ((Panel) this._dropDownPanel).Children).Count <= 0)
        return;
      if (this._dropDownPopup.IsOpen)
      {
        this._dropDownPopup.put_IsOpen(false);
      }
      else
      {
        this.PositionDropDownPopup();
        this._dropDownPopup.put_IsOpen(true);
        if (!(((IList<UIElement>) ((Panel) this._dropDownPanel).Children)[0] is ButtonBase child) || !((Control) child).IsTabStop)
          return;
        ((Control) child).Focus((FocusState) 3);
      }
    }
  }
}
