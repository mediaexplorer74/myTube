// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.TreeView
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation.Peers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using WinRTXamlToolkit.Controls.Common;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (TreeViewItem))]
  [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  public class TreeView : ItemsControl, IUpdateVisualState
  {
    private bool _allowWrite;
    private bool _ignorePropertyChange;
    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof (SelectedItem), (Type) typeof (object), (Type) typeof (TreeView), new PropertyMetadata((object) null, new PropertyChangedCallback(TreeView.OnSelectedItemPropertyChanged)));
    public static readonly DependencyProperty SelectedValueProperty = DependencyProperty.Register(nameof (SelectedValue), (Type) typeof (object), (Type) typeof (TreeView), new PropertyMetadata((object) null, new PropertyChangedCallback(TreeView.OnSelectedValuePropertyChanged)));
    public static readonly DependencyProperty SelectedValuePathProperty = DependencyProperty.Register(nameof (SelectedValuePath), (Type) typeof (string), (Type) typeof (TreeView), new PropertyMetadata((object) string.Empty, new PropertyChangedCallback(TreeView.OnSelectedValuePathPropertyChanged)));
    public static readonly DependencyProperty IsExpandedBindingPathProperty = DependencyProperty.RegisterAttached("IsExpandedBindingPath", (Type) typeof (string), (Type) typeof (TreeView), new PropertyMetadata((object) null, new PropertyChangedCallback(TreeView.OnIsExpandedBindingPathChanged)));
    public static readonly DependencyProperty IsSelectedBindingPathProperty = DependencyProperty.RegisterAttached("IsSelectedBindingPath", (Type) typeof (string), (Type) typeof (TreeView), new PropertyMetadata((object) null, new PropertyChangedCallback(TreeView.OnIsSelectedBindingPathChanged)));
    private List<object> items;

    public object SelectedItem
    {
      get => ((DependencyObject) this).GetValue(TreeView.SelectedItemProperty);
      private set
      {
        try
        {
          this._allowWrite = true;
          ((DependencyObject) this).SetValue(TreeView.SelectedItemProperty, value);
        }
        finally
        {
          this._allowWrite = false;
        }
      }
    }

    private static void OnSelectedItemPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeView treeView = d as TreeView;
      if (treeView._ignorePropertyChange)
      {
        treeView._ignorePropertyChange = false;
      }
      else
      {
        if (!treeView._allowWrite)
        {
          treeView._ignorePropertyChange = true;
          ((DependencyObject) treeView).SetValue(TreeView.SelectedItemProperty, e.OldValue);
          throw new InvalidOperationException("Properties.Resources.TreeView_OnSelectedItemPropertyChanged_InvalidWrite");
        }
        treeView.UpdateSelectedValue(e.NewValue);
      }
    }

    public object SelectedValue
    {
      get => ((DependencyObject) this).GetValue(TreeView.SelectedValueProperty);
      private set
      {
        try
        {
          this._allowWrite = true;
          ((DependencyObject) this).SetValue(TreeView.SelectedValueProperty, value);
        }
        finally
        {
          this._allowWrite = false;
        }
      }
    }

    private static void OnSelectedValuePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeView treeView = d as TreeView;
      if (treeView._ignorePropertyChange)
        treeView._ignorePropertyChange = false;
      else if (!treeView._allowWrite)
      {
        treeView._ignorePropertyChange = true;
        ((DependencyObject) treeView).SetValue(TreeView.SelectedValueProperty, e.OldValue);
        throw new InvalidOperationException("Properties.Resources.TreeView_OnSelectedValuePropertyChanged_InvalidWrite");
      }
    }

    public string SelectedValuePath
    {
      get => ((DependencyObject) this).GetValue(TreeView.SelectedValuePathProperty) as string;
      set => ((DependencyObject) this).SetValue(TreeView.SelectedValuePathProperty, (object) value);
    }

    private static void OnSelectedValuePathPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeView treeView = d as TreeView;
      treeView.UpdateSelectedValue(treeView.SelectedItem);
    }

    public static string GetIsExpandedBindingPath(DependencyObject d) => (string) d.GetValue(TreeView.IsExpandedBindingPathProperty);

    public static void SetIsExpandedBindingPath(DependencyObject d, string value) => d.SetValue(TreeView.IsExpandedBindingPathProperty, (object) value);

    private static void OnIsExpandedBindingPathChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      string oldValue = (string) e.OldValue;
      string str = (string) d.GetValue(TreeView.IsExpandedBindingPathProperty);
    }

    public static string GetIsSelectedBindingPath(DependencyObject d) => (string) d.GetValue(TreeView.IsSelectedBindingPathProperty);

    public static void SetIsSelectedBindingPath(DependencyObject d, string value) => d.SetValue(TreeView.IsSelectedBindingPathProperty, (object) value);

    private static void OnIsSelectedBindingPathChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      string oldValue = (string) e.OldValue;
      string str = (string) d.GetValue(TreeView.IsSelectedBindingPathProperty);
    }

    private void OnItemContainerStylePropertyChanged(object sender, Style style) => this.ItemsControlHelper.UpdateItemContainerStyle(style);

    public TreeViewItem SelectedContainer { get; private set; }

    internal bool IsSelectedContainerHookedUp => this.SelectedContainer != null && this.SelectedContainer.ParentTreeView == this;

    internal bool IsSelectionChangeActive { get; set; }

    internal ItemsControlHelper ItemsControlHelper { get; private set; }

    internal InteractionHelper Interaction { get; private set; }

    internal static bool IsControlKeyDown => false;

    internal static bool IsShiftKeyDown => false;

    public event RoutedPropertyChangedEventHandler<object> SelectedItemChanged;

    public TreeView()
    {
      new PropertyChangeEventSource<Style>((DependencyObject) this, "ItemContainerStyle").ValueChanged += new EventHandler<Style>(this.OnItemContainerStylePropertyChanged);
      ((Control) this).put_DefaultStyleKey((object) typeof (TreeView));
      this.ItemsControlHelper = new ItemsControlHelper((ItemsControl) this);
      this.Interaction = new InteractionHelper((Control) this);
    }

    protected virtual AutomationPeer OnCreateAutomationPeer() => (AutomationPeer) new TreeViewAutomationPeer(this);

    protected virtual void OnApplyTemplate()
    {
      this.ItemsControlHelper.OnApplyTemplate();
      this.Interaction.OnApplyTemplateBase();
      ((FrameworkElement) this).OnApplyTemplate();
    }

    void IUpdateVisualState.UpdateVisualState(bool useTransitions) => this.Interaction.UpdateVisualStateBase(useTransitions);

    protected virtual DependencyObject GetContainerForItemOverride() => (DependencyObject) new TreeViewItem();

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is TreeViewItem;

    protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
      if (element is TreeViewItem d)
      {
        d.ParentItemsControl = (ItemsControl) this;
        string str1 = TreeView.GetIsSelectedBindingPath((DependencyObject) this) ?? TreeView.GetIsSelectedBindingPath((DependencyObject) d);
        string str2 = TreeView.GetIsExpandedBindingPath((DependencyObject) this) ?? TreeView.GetIsExpandedBindingPath((DependencyObject) d);
        if (str1 != null)
        {
          if (((DependencyObject) d).ReadLocalValue(TreeView.IsSelectedBindingPathProperty) == DependencyProperty.UnsetValue)
            TreeView.SetIsSelectedBindingPath((DependencyObject) d, str1);
          TreeViewItem treeViewItem = d;
          DependencyProperty selectedProperty = TreeViewItem.IsSelectedProperty;
          Binding binding1 = new Binding();
          binding1.put_Path(new PropertyPath(str1));
          binding1.put_Mode((BindingMode) 3);
          Binding binding2 = binding1;
          ((FrameworkElement) treeViewItem).SetBinding(selectedProperty, (BindingBase) binding2);
        }
        if (str2 != null)
        {
          if (((DependencyObject) d).ReadLocalValue(TreeView.IsExpandedBindingPathProperty) == DependencyProperty.UnsetValue)
            TreeView.SetIsExpandedBindingPath((DependencyObject) d, str2);
          TreeViewItem treeViewItem = d;
          DependencyProperty expandedProperty = TreeViewItem.IsExpandedProperty;
          Binding binding3 = new Binding();
          binding3.put_Path(new PropertyPath(str2));
          binding3.put_Mode((BindingMode) 3);
          Binding binding4 = binding3;
          ((FrameworkElement) treeViewItem).SetBinding(expandedProperty, (BindingBase) binding4);
        }
      }
      ItemsControlHelper.PrepareContainerForItemOverride(element, this.ItemContainerStyle);
      HeaderedItemsControl.PrepareHeaderedItemsControlContainerForItemOverride(element, item, (ItemsControl) this, this.ItemContainerStyle);
      base.PrepareContainerForItemOverride(element, item);
    }

    protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
    {
      if (element is TreeViewItem treeViewItem)
        treeViewItem.ParentItemsControl = (ItemsControl) null;
      base.ClearContainerForItemOverride(element, item);
    }

    protected virtual void OnItemsChanged(object e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      base.OnItemsChanged(e);
      if (this.items == null)
        this.items = new List<object>();
      List<object> list1 = ((IEnumerable<object>) this.Items).Except<object>((IEnumerable<object>) this.items).ToList<object>();
      if (list1 != null)
      {
        foreach (TreeViewItem treeViewItem in (IEnumerable<TreeViewItem>) list1.OfType<TreeViewItem>())
          treeViewItem.ParentItemsControl = (ItemsControl) this;
      }
      if (this.SelectedItem != null && !this.IsSelectedContainerHookedUp)
        this.SelectFirstItem();
      List<object> list2 = this.items.Except<object>((IEnumerable<object>) this.Items).ToList<object>();
      if (list2 == null)
        return;
      foreach (TreeViewItem treeViewItem in (IEnumerable<TreeViewItem>) list2.OfType<TreeViewItem>())
        treeViewItem.ParentItemsControl = (ItemsControl) null;
    }

    internal void CheckForSelectedDescendents(TreeViewItem item)
    {
      Stack<TreeViewItem> treeViewItemStack = new Stack<TreeViewItem>();
      treeViewItemStack.Push(item);
      while (treeViewItemStack.Count > 0)
      {
        TreeViewItem treeViewItem1 = treeViewItemStack.Pop();
        if (treeViewItem1.IsSelected)
        {
          treeViewItem1.IgnorePropertyChange = true;
          treeViewItem1.IsSelected = false;
          this.ChangeSelection((object) treeViewItem1, treeViewItem1, true);
          if (this.SelectedContainer.ParentItemsControl == null)
            this.SelectedContainer.RequiresContainsSelectionUpdate = true;
        }
        foreach (TreeViewItem treeViewItem2 in (IEnumerable<TreeViewItem>) ((IEnumerable) treeViewItem1.Items).OfType<TreeViewItem>())
          treeViewItemStack.Push(treeViewItem2);
      }
    }

    internal void PropagateKeyDown(KeyRoutedEventArgs e) => ((Control) this).OnKeyDown(e);

    protected virtual void OnKeyDown(KeyRoutedEventArgs e)
    {
      if (!this.Interaction.AllowKeyDown(e))
        return;
      ((Control) this).OnKeyDown(e);
      if (e.Handled)
        return;
      if (TreeView.IsControlKeyDown)
      {
        switch (e.Key - 33)
        {
          case 0:
          case 1:
          case 2:
          case 3:
          case 4:
          case 5:
          case 6:
          case 7:
            if (!this.HandleScrollKeys(e.Key))
              break;
            e.put_Handled(true);
            break;
        }
      }
      else
      {
        switch (e.Key - 33)
        {
          case 0:
          case 1:
            if (this.SelectedContainer != null)
            {
              if (!this.HandleScrollByPage(e.Key == 33))
                break;
              e.put_Handled(true);
              break;
            }
            if (!this.FocusFirstItem())
              break;
            e.put_Handled(true);
            break;
          case 2:
            if (!this.FocusLastItem())
              break;
            e.put_Handled(true);
            break;
          case 3:
            if (!this.FocusFirstItem())
              break;
            e.put_Handled(true);
            break;
          case 5:
          case 7:
            if (this.SelectedContainer != null || !this.FocusFirstItem())
              break;
            e.put_Handled(true);
            break;
        }
      }
    }

    private bool HandleScrollKeys(VirtualKey key)
    {
      ScrollViewer scrollHost = this.ItemsControlHelper.ScrollHost;
      if (scrollHost != null)
      {
        switch (InteractionHelper.GetLogicalKey(((FrameworkElement) this).FlowDirection, key) - 33)
        {
          case 0:
            if (!NumericExtensions.IsGreaterThan(scrollHost.ExtentHeight, scrollHost.ViewportHeight))
              scrollHost.PageLeft();
            else
              scrollHost.PageUp();
            return true;
          case 1:
            if (!NumericExtensions.IsGreaterThan(scrollHost.ExtentHeight, scrollHost.ViewportHeight))
              scrollHost.PageRight();
            else
              scrollHost.PageDown();
            return true;
          case 2:
            scrollHost.ScrollToBottom();
            return true;
          case 3:
            scrollHost.ScrollToTop();
            return true;
          case 4:
            scrollHost.LineLeft();
            return true;
          case 5:
            scrollHost.LineUp();
            return true;
          case 6:
            scrollHost.LineRight();
            return true;
          case 7:
            scrollHost.LineDown();
            return true;
        }
      }
      return false;
    }

    private bool HandleScrollByPage(bool up)
    {
      ScrollViewer scrollHost = this.ItemsControlHelper.ScrollHost;
      if (scrollHost != null)
      {
        double viewportHeight = scrollHost.ViewportHeight;
        double top;
        double bottom;
        (this.SelectedContainer.HeaderElement ?? (FrameworkElement) this.SelectedContainer).GetTopAndBottom((FrameworkElement) scrollHost, out top, out bottom);
        TreeViewItem treeViewItem1 = (TreeViewItem) null;
        TreeViewItem treeViewItem2 = this.SelectedContainer;
        ItemsControl itemsControl = this.SelectedContainer.ParentItemsControl;
        if (itemsControl != null)
        {
          ItemsControl parentItemsControl;
          if (up)
          {
            for (; itemsControl != this && itemsControl is TreeViewItem treeViewItem3; itemsControl = parentItemsControl)
            {
              parentItemsControl = treeViewItem3.ParentItemsControl;
              if (parentItemsControl != null)
                treeViewItem2 = treeViewItem3;
              else
                break;
            }
          }
          int num = itemsControl.IndexFromContainer((DependencyObject) treeViewItem2);
          int count = ((ICollection<object>) itemsControl.Items).Count;
          while (itemsControl != null && treeViewItem2 != null)
          {
            if (((Control) treeViewItem2).IsEnabled)
            {
              double currentDelta;
              if (treeViewItem2.HandleScrollByPage(up, scrollHost, viewportHeight, top, bottom, out currentDelta))
                return true;
              if (NumericExtensions.IsGreaterThan(currentDelta, viewportHeight))
              {
                if (treeViewItem1 == this.SelectedContainer || treeViewItem1 == null)
                  return !up ? this.SelectedContainer.HandleDownKey() : this.SelectedContainer.HandleUpKey();
                break;
              }
              treeViewItem1 = treeViewItem2;
            }
            num += up ? -1 : 1;
            if (0 <= num && num < count)
              treeViewItem2 = itemsControl.ContainerFromIndex(num) as TreeViewItem;
            else if (itemsControl == this)
            {
              treeViewItem2 = (TreeViewItem) null;
            }
            else
            {
              while (itemsControl != null)
              {
                TreeViewItem treeViewItem4 = itemsControl as TreeViewItem;
                itemsControl = treeViewItem4.ParentItemsControl;
                if (itemsControl != null)
                {
                  count = ((ICollection<object>) itemsControl.Items).Count;
                  num = itemsControl.IndexFromContainer((DependencyObject) treeViewItem4) + (up ? -1 : 1);
                  if (0 <= num && num < count)
                  {
                    treeViewItem2 = itemsControl.ContainerFromIndex(num) as TreeViewItem;
                    break;
                  }
                  if (itemsControl == this)
                  {
                    treeViewItem2 = (TreeViewItem) null;
                    itemsControl = (ItemsControl) null;
                  }
                }
              }
            }
          }
        }
        if (treeViewItem1 != null)
        {
          if (up)
          {
            if (treeViewItem1 != this.SelectedContainer)
              return ((Control) treeViewItem1).Focus((FocusState) 3);
          }
          else
            treeViewItem1.FocusInto();
        }
      }
      return false;
    }

    protected virtual void OnKeyUp(KeyRoutedEventArgs e)
    {
      if (!this.Interaction.AllowKeyUp(e))
        return;
      ((Control) this).OnKeyUp(e);
    }

    protected virtual void OnPointerEntered(PointerRoutedEventArgs e)
    {
      if (!this.Interaction.AllowMouseEnter(e))
        return;
      this.Interaction.OnMouseEnterBase();
      ((Control) this).OnPointerEntered(e);
    }

    protected virtual void OnPointerExited(PointerRoutedEventArgs e)
    {
      if (!this.Interaction.AllowMouseLeave(e))
        return;
      this.Interaction.OnMouseLeaveBase();
      ((Control) this).OnPointerExited(e);
    }

    protected virtual void OnPointerMoved(PointerRoutedEventArgs e) => ((Control) this).OnPointerMoved(e);

    protected virtual void OnPointerPressed(PointerRoutedEventArgs e)
    {
      if (!this.Interaction.AllowMouseLeftButtonDown(e))
        return;
      if (!e.Handled && this.HandleMouseButtonDown())
        e.put_Handled(true);
      this.Interaction.OnMouseLeftButtonDownBase();
      ((Control) this).OnPointerPressed(e);
    }

    protected virtual void OnPointerReleased(PointerRoutedEventArgs e)
    {
      if (!this.Interaction.AllowMouseLeftButtonUp(e))
        return;
      this.Interaction.OnMouseLeftButtonUpBase();
      ((Control) this).OnPointerReleased(e);
    }

    internal bool HandleMouseButtonDown()
    {
      if (this.SelectedContainer == null)
        return false;
      if (this.SelectedContainer != FocusManager.GetFocusedElement())
        ((Control) this.SelectedContainer).Focus((FocusState) 3);
      return true;
    }

    protected virtual void OnGotFocus(RoutedEventArgs e)
    {
      if (!this.Interaction.AllowGotFocus(e))
        return;
      this.Interaction.OnGotFocusBase();
      ((Control) this).OnGotFocus(e);
    }

    protected virtual void OnLostFocus(RoutedEventArgs e)
    {
      if (!this.Interaction.AllowLostFocus(e))
        return;
      this.Interaction.OnLostFocusBase();
      ((Control) this).OnLostFocus(e);
    }

    protected virtual void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
    {
      RoutedPropertyChangedEventHandler<object> selectedItemChanged = this.SelectedItemChanged;
      if (selectedItemChanged == null)
        return;
      selectedItemChanged((object) this, e);
    }

    internal void ChangeSelection(object itemOrContainer, TreeViewItem container, bool selected)
    {
      if (this.IsSelectionChangeActive)
        return;
      object oldValue = (object) null;
      object newValue = (object) null;
      bool flag = false;
      TreeViewItem selectedContainer = this.SelectedContainer;
      this.IsSelectionChangeActive = true;
      try
      {
        if (selected && container != this.SelectedContainer)
        {
          oldValue = this.SelectedItem;
          if (this.SelectedContainer != null)
          {
            this.SelectedContainer.IsSelected = false;
            this.SelectedContainer.UpdateContainsSelection(false);
          }
          newValue = itemOrContainer;
          this.SelectedContainer = container;
          this.SelectedContainer.UpdateContainsSelection(true);
          this.SelectedItem = itemOrContainer;
          this.UpdateSelectedValue(itemOrContainer);
          flag = true;
          this.ItemsControlHelper.ScrollIntoView(container.HeaderElement ?? (FrameworkElement) container);
        }
        else if (!selected && container == this.SelectedContainer)
        {
          this.SelectedContainer.UpdateContainsSelection(false);
          this.SelectedContainer = (TreeViewItem) null;
          this.SelectedItem = (object) null;
          this.SelectedValue = (object) null;
          oldValue = itemOrContainer;
          flag = true;
        }
        container.IsSelected = selected;
      }
      finally
      {
        this.IsSelectionChangeActive = false;
      }
      if (!flag)
        return;
      if (this.SelectedContainer != null && AutomationPeer.ListenerExists((AutomationEvents) 8))
        FrameworkElementAutomationPeer.CreatePeerForElement((UIElement) this.SelectedContainer)?.RaiseAutomationEvent((AutomationEvents) 8);
      if (selectedContainer != null && AutomationPeer.ListenerExists((AutomationEvents) 7))
        FrameworkElementAutomationPeer.CreatePeerForElement((UIElement) selectedContainer)?.RaiseAutomationEvent((AutomationEvents) 7);
      this.OnSelectedItemChanged(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue));
    }

    private void UpdateSelectedValue(object item)
    {
      if (item != null)
      {
        string selectedValuePath = this.SelectedValuePath;
        if (string.IsNullOrEmpty(selectedValuePath))
        {
          this.SelectedValue = item;
        }
        else
        {
          Binding binding1 = new Binding();
          binding1.put_Path(new PropertyPath(selectedValuePath));
          binding1.put_Source(item);
          Binding binding2 = binding1;
          ContentControl contentControl = new ContentControl();
          ((FrameworkElement) contentControl).SetBinding(ContentControl.ContentProperty, (BindingBase) binding2);
          this.SelectedValue = contentControl.Content;
          ((DependencyObject) contentControl).ClearValue(ContentControl.ContentProperty);
        }
      }
      else
        ((DependencyObject) this).ClearValue(TreeView.SelectedValueProperty);
    }

    private void SelectFirstItem()
    {
      TreeViewItem container = this.ContainerFromIndex(0) as TreeViewItem;
      bool selected = container != null;
      if (!selected)
        container = this.SelectedContainer;
      this.ChangeSelection(selected ? this.ItemFromContainer((DependencyObject) container) : this.SelectedItem, container, selected);
    }

    private bool FocusFirstItem()
    {
      if (!(this.ContainerFromIndex(0) is TreeViewItem treeViewItem))
        return false;
      return ((Control) treeViewItem).IsEnabled && ((Control) treeViewItem).Focus((FocusState) 3) || treeViewItem.FocusDown();
    }

    private bool FocusLastItem()
    {
      for (int index = ((ICollection<object>) this.Items).Count - 1; index >= 0; --index)
      {
        if (this.ContainerFromIndex(index) is TreeViewItem treeViewItem && ((Control) treeViewItem).IsEnabled)
          return treeViewItem.FocusInto();
      }
      return false;
    }
  }
}
