// Decompiled with JetBrains decompiler
// Type: WinRTXamlToolkit.Controls.TreeViewItem
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Automation.Peers;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace WinRTXamlToolkit.Controls
{
  [TemplateVisualState(Name = "Unselected", GroupName = "SelectionStates")]
  [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof (TreeViewItem))]
  [TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates")]
  [TemplatePart(Name = "Header", Type = typeof (FrameworkElement))]
  [TemplateVisualState(Name = "Expanded", GroupName = "ExpansionStates")]
  [TemplateVisualState(Name = "Collapsed", GroupName = "ExpansionStates")]
  [TemplateVisualState(Name = "HasItems", GroupName = "HasItemsStates")]
  [TemplateVisualState(Name = "NoItems", GroupName = "HasItemsStates")]
  [TemplateVisualState(Name = "Normal", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Pressed", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
  [TemplateVisualState(Name = "Focused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
  [TemplateVisualState(Name = "Selected", GroupName = "SelectionStates")]
  [TemplateVisualState(Name = "SelectedInactive", GroupName = "SelectionStates")]
  [TemplatePart(Name = "ExpanderButton", Type = typeof (ToggleButton))]
  public class TreeViewItem : HeaderedItemsControl, IUpdateVisualState
  {
    private const string ExpanderButtonName = "ExpanderButton";
    private const string HeaderName = "Header";
    private ToggleButton _expanderButton;
    private FrameworkElement _headerElement;
    private VisualStateGroup _expansionStateGroup;
    private bool _allowWrite;
    public static readonly DependencyProperty HasItemsProperty = DependencyProperty.Register(nameof (HasItems), (Type) typeof (bool), (Type) typeof (TreeViewItem), new PropertyMetadata((object) false, new PropertyChangedCallback(TreeViewItem.OnHasItemsPropertyChanged)));
    public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(nameof (IsExpanded), (Type) typeof (bool), (Type) typeof (TreeViewItem), new PropertyMetadata((object) false, new PropertyChangedCallback(TreeViewItem.OnIsExpandedPropertyChanged)));
    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), (Type) typeof (bool), (Type) typeof (TreeViewItem), new PropertyMetadata((object) false, new PropertyChangedCallback(TreeViewItem.OnIsSelectedPropertyChanged)));
    public static readonly DependencyProperty IsSelectionActiveProperty = DependencyProperty.Register(nameof (IsSelectionActive), (Type) typeof (bool), (Type) typeof (TreeViewItem), new PropertyMetadata((object) false, new PropertyChangedCallback(TreeViewItem.OnIsSelectionActivePropertyChanged)));
    private ItemsControl _parentItemsControl;
    private List<object> items;

    private ToggleButton ExpanderButton
    {
      get => this._expanderButton;
      set
      {
        if (this._expanderButton != null)
        {
          WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) this._expanderButton).remove_Click), new RoutedEventHandler(this.OnExpanderClick));
          WindowsRuntimeMarshal.RemoveEventHandler<RoutedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._expanderButton).remove_GotFocus), new RoutedEventHandler(this.OnExpanderGotFocus));
        }
        this._expanderButton = value;
        if (this._expanderButton == null)
          return;
        this._expanderButton.put_IsChecked((bool?) new bool?(this.IsExpanded));
        ToggleButton expanderButton1 = this._expanderButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((ButtonBase) expanderButton1).add_Click), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((ButtonBase) expanderButton1).remove_Click), new RoutedEventHandler(this.OnExpanderClick));
        ToggleButton expanderButton2 = this._expanderButton;
        WindowsRuntimeMarshal.AddEventHandler<RoutedEventHandler>((Func<RoutedEventHandler, EventRegistrationToken>) new Func<RoutedEventHandler, EventRegistrationToken>(((UIElement) expanderButton2).add_GotFocus), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) expanderButton2).remove_GotFocus), new RoutedEventHandler(this.OnExpanderGotFocus));
      }
    }

    internal FrameworkElement HeaderElement
    {
      get => this._headerElement;
      private set
      {
        if (this._headerElement != null)
          WindowsRuntimeMarshal.RemoveEventHandler<PointerEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) this._headerElement).remove_PointerPressed), new PointerEventHandler(this.OnHeaderMouseLeftButtonDown));
        this._headerElement = value;
        if (this._headerElement == null)
          return;
        FrameworkElement headerElement = this._headerElement;
        WindowsRuntimeMarshal.AddEventHandler<PointerEventHandler>((Func<PointerEventHandler, EventRegistrationToken>) new Func<PointerEventHandler, EventRegistrationToken>(((UIElement) headerElement).add_PointerPressed), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(((UIElement) headerElement).remove_PointerPressed), new PointerEventHandler(this.OnHeaderMouseLeftButtonDown));
      }
    }

    private VisualStateGroup ExpansionStateGroup
    {
      get => this._expansionStateGroup;
      set
      {
        if (this._expansionStateGroup != null)
          WindowsRuntimeMarshal.RemoveEventHandler<VisualStateChangedEventHandler>((Action<EventRegistrationToken>) new Action<EventRegistrationToken>(this._expansionStateGroup.remove_CurrentStateChanged), new VisualStateChangedEventHandler(this.OnExpansionStateGroupStateChanged));
        this._expansionStateGroup = value;
        if (this._expansionStateGroup == null)
          return;
        VisualStateGroup expansionStateGroup = this._expansionStateGroup;
        WindowsRuntimeMarshal.AddEventHandler<VisualStateChangedEventHandler>((Func<VisualStateChangedEventHandler, EventRegistrationToken>) new Func<VisualStateChangedEventHandler, EventRegistrationToken>(expansionStateGroup.add_CurrentStateChanged), (Action<EventRegistrationToken>) new Action<EventRegistrationToken>(expansionStateGroup.remove_CurrentStateChanged), new VisualStateChangedEventHandler(this.OnExpansionStateGroupStateChanged));
      }
    }

    internal bool IgnorePropertyChange { get; set; }

    public bool HasItems
    {
      get => (bool) ((DependencyObject) this).GetValue(TreeViewItem.HasItemsProperty);
      private set
      {
        try
        {
          this._allowWrite = true;
          ((DependencyObject) this).SetValue(TreeViewItem.HasItemsProperty, (object) value);
        }
        finally
        {
          this._allowWrite = false;
        }
      }
    }

    private static void OnHasItemsPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeViewItem treeViewItem = d as TreeViewItem;
      if (treeViewItem.IgnorePropertyChange)
      {
        treeViewItem.IgnorePropertyChange = false;
      }
      else
      {
        if (!treeViewItem._allowWrite)
        {
          treeViewItem.IgnorePropertyChange = true;
          ((DependencyObject) treeViewItem).SetValue(TreeViewItem.HasItemsProperty, e.OldValue);
          throw new InvalidOperationException("Properties.Resources.TreeViewItem_OnHasItemsPropertyChanged_InvalidWrite");
        }
        treeViewItem.UpdateVisualState(true);
      }
    }

    public bool IsExpanded
    {
      get => (bool) ((DependencyObject) this).GetValue(TreeViewItem.IsExpandedProperty);
      set => ((DependencyObject) this).SetValue(TreeViewItem.IsExpandedProperty, (object) value);
    }

    private static void OnIsExpandedPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeViewItem element = d as TreeViewItem;
      bool newValue = (bool) e.NewValue;
      if (FrameworkElementAutomationPeer.FromElement((UIElement) element) is TreeViewItemAutomationPeer itemAutomationPeer)
        itemAutomationPeer.RaiseExpandCollapseAutomationEvent((bool) e.OldValue, newValue);
      RoutedEventArgs e1 = new RoutedEventArgs();
      if (newValue)
        element.OnExpanded(e1);
      else
        element.OnCollapsed(e1);
      if (newValue)
      {
        if (element.ExpansionStateGroup != null || !element.UserInitiatedExpansion)
          return;
        element.UserInitiatedExpansion = false;
        element.ParentTreeView?.ItemsControlHelper.ScrollIntoView((FrameworkElement) element);
      }
      else
      {
        if (!element.ContainsSelection)
          return;
        ((Control) element).Focus((FocusState) 3);
      }
    }

    public bool IsSelected
    {
      get => (bool) ((DependencyObject) this).GetValue(TreeViewItem.IsSelectedProperty);
      set => ((DependencyObject) this).SetValue(TreeViewItem.IsSelectedProperty, (object) value);
    }

    private static void OnIsSelectedPropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeViewItem treeViewItem = d as TreeViewItem;
      bool newValue = (bool) e.NewValue;
      if (treeViewItem.IgnorePropertyChange)
      {
        treeViewItem.IgnorePropertyChange = false;
      }
      else
      {
        treeViewItem.Select(newValue);
        if (FrameworkElementAutomationPeer.FromElement((UIElement) treeViewItem) is TreeViewItemAutomationPeer itemAutomationPeer)
          itemAutomationPeer.RaiseAutomationIsSelectedChanged(newValue);
        RoutedEventArgs e1 = new RoutedEventArgs();
        if (newValue)
          treeViewItem.OnSelected(e1);
        else
          treeViewItem.OnUnselected(e1);
      }
    }

    public bool IsSelectionActive
    {
      get => (bool) ((DependencyObject) this).GetValue(TreeViewItem.IsSelectionActiveProperty);
      private set
      {
        try
        {
          this._allowWrite = true;
          ((DependencyObject) this).SetValue(TreeViewItem.IsSelectionActiveProperty, (object) value);
        }
        finally
        {
          this._allowWrite = false;
        }
      }
    }

    private static void OnIsSelectionActivePropertyChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      TreeViewItem treeViewItem = d as TreeViewItem;
      if (treeViewItem.IgnorePropertyChange)
      {
        treeViewItem.IgnorePropertyChange = false;
      }
      else
      {
        if (!treeViewItem._allowWrite)
        {
          treeViewItem.IgnorePropertyChange = true;
          ((DependencyObject) treeViewItem).SetValue(TreeViewItem.IsSelectionActiveProperty, e.OldValue);
          throw new InvalidOperationException("Properties.Resources.TreeViewItem_OnIsSelectionActivePropertyChanged_InvalidWrite");
        }
        treeViewItem.UpdateVisualState(true);
      }
    }

    internal InteractionHelper Interaction { get; private set; }

    private bool ContainsSelection { get; set; }

    private bool CancelGotFocusBubble { get; set; }

    internal bool RequiresContainsSelectionUpdate { get; set; }

    internal bool UserInitiatedExpansion { get; set; }

    internal ItemsControl ParentItemsControl
    {
      get => this._parentItemsControl;
      set
      {
        if (this._parentItemsControl == value)
          return;
        this._parentItemsControl = value;
        TreeView parentTreeView = this.ParentTreeView;
        if (parentTreeView == null)
          return;
        if (this.RequiresContainsSelectionUpdate)
        {
          this.RequiresContainsSelectionUpdate = false;
          this.UpdateContainsSelection(true);
        }
        parentTreeView.CheckForSelectedDescendents(this);
      }
    }

    internal TreeViewItem ParentTreeViewItem => this.ParentItemsControl as TreeViewItem;

    internal TreeView ParentTreeView
    {
      get
      {
        for (TreeViewItem treeViewItem = this; treeViewItem != null; treeViewItem = treeViewItem.ParentTreeViewItem)
        {
          if (treeViewItem.ParentItemsControl is TreeView parentItemsControl)
            return parentItemsControl;
        }
        return (TreeView) null;
      }
    }

    private bool IsRoot => this.ParentItemsControl is TreeView;

    private bool CanExpandOnInput => this.HasItems && ((Control) this).IsEnabled;

    public event RoutedEventHandler Collapsed;

    public event RoutedEventHandler Expanded;

    public event RoutedEventHandler Selected;

    public event RoutedEventHandler Unselected;

    public TreeViewItem()
    {
      ((Control) this).put_DefaultStyleKey((object) typeof (TreeViewItem));
      this.Interaction = new InteractionHelper((Control) this);
    }

    protected virtual AutomationPeer OnCreateAutomationPeer() => (AutomationPeer) new TreeViewItemAutomationPeer(this);

    protected override void OnApplyTemplate()
    {
      this.ItemsControlHelper.OnApplyTemplate();
      this.ExpanderButton = ((Control) this).GetTemplateChild("ExpanderButton") as ToggleButton;
      this.HeaderElement = ((Control) this).GetTemplateChild("Header") as FrameworkElement;
      this.ExpansionStateGroup = VisualStates.TryGetVisualStateGroup((DependencyObject) this, "ExpansionStates");
      this.Interaction.OnApplyTemplateBase();
      base.OnApplyTemplate();
    }

    private void OnExpansionStateGroupStateChanged(object sender, VisualStateChangedEventArgs e)
    {
      if (string.Compare(e.NewState.Name, "Expanded", StringComparison.OrdinalIgnoreCase) != 0)
        return;
      this.BringIntoView();
    }

    private void BringIntoView()
    {
      if (!this.UserInitiatedExpansion)
        return;
      DispatchedHandler dispatchedHandler1 = (DispatchedHandler) null;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      TreeViewItem.\u003C\u003Ec__DisplayClass3 cDisplayClass3 = new TreeViewItem.\u003C\u003Ec__DisplayClass3();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3.\u003C\u003E4__this = this;
      this.UserInitiatedExpansion = false;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass3.parent = this.ParentTreeView;
      // ISSUE: reference to a compiler-generated field
      if (cDisplayClass3.parent == null)
        return;
      CoreDispatcher dispatcher = ((DependencyObject) this).Dispatcher;
      if (dispatchedHandler1 == null)
      {
        // ISSUE: method pointer
        dispatchedHandler1 = new DispatchedHandler((object) cDisplayClass3, __methodptr(\u003CBringIntoView\u003Eb__0));
      }
      DispatchedHandler dispatchedHandler2 = dispatchedHandler1;
      dispatcher.RunAsync((CoreDispatcherPriority) 0, dispatchedHandler2);
    }

    void IUpdateVisualState.UpdateVisualState(bool useTransitions) => this.UpdateVisualState(useTransitions);

    internal virtual void UpdateVisualState(bool useTransitions)
    {
      if (this.IsExpanded)
        VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Expanded");
      else
        VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Collapsed");
      if (this.HasItems)
        VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "HasItems");
      else
        VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "NoItems");
      if (this.IsSelected)
      {
        if (this.IsSelectionActive)
          VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Selected");
        else
          VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "SelectedInactive", "Selected");
      }
      else
        VisualStates.GoToState((Control) this, (useTransitions ? 1 : 0) != 0, "Unselected");
      this.Interaction.UpdateVisualStateBase(useTransitions);
    }

    protected virtual DependencyObject GetContainerForItemOverride() => (DependencyObject) new TreeViewItem();

    protected virtual bool IsItemItsOwnContainerOverride(object item) => item is TreeViewItem;

    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
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
      this.HasItems = ((ICollection<object>) this.Items).Count > 0;
      if (this.items == null)
        this.items = new List<object>();
      List<object> list1 = ((IEnumerable<object>) this.Items).Except<object>((IEnumerable<object>) this.items).ToList<object>();
      if (list1 != null)
      {
        foreach (TreeViewItem treeViewItem in (IEnumerable<TreeViewItem>) list1.OfType<TreeViewItem>())
          treeViewItem.ParentItemsControl = (ItemsControl) this;
      }
      if (this.ContainsSelection)
      {
        TreeView parentTreeView = this.ParentTreeView;
        if (parentTreeView != null && !parentTreeView.IsSelectedContainerHookedUp)
        {
          this.ContainsSelection = false;
          this.Select(true);
        }
      }
      List<object> list2 = this.items.Except<object>((IEnumerable<object>) this.Items).ToList<object>();
      if (list2 != null)
      {
        foreach (TreeViewItem treeViewItem in (IEnumerable<TreeViewItem>) list2.OfType<TreeViewItem>())
          treeViewItem.ParentItemsControl = (ItemsControl) null;
      }
      this.items = ((IEnumerable<object>) this.Items).ToList<object>();
    }

    private void RaiseEvent(RoutedEventHandler handler, RoutedEventArgs args)
    {
      if (handler == null)
        return;
      handler((object) this, args);
    }

    protected virtual void OnExpanded(RoutedEventArgs e) => this.ToggleExpanded(this.Expanded, e);

    protected virtual void OnCollapsed(RoutedEventArgs e) => this.ToggleExpanded(this.Collapsed, e);

    private void ToggleExpanded(RoutedEventHandler handler, RoutedEventArgs args)
    {
      this.ExpanderButton?.put_IsChecked((bool?) new bool?(this.IsExpanded));
      this.UpdateVisualState(true);
      this.RaiseEvent(handler, args);
    }

    protected virtual void OnSelected(RoutedEventArgs e)
    {
      this.UpdateVisualState(true);
      this.RaiseEvent(this.Selected, e);
    }

    protected virtual void OnUnselected(RoutedEventArgs e)
    {
      this.UpdateVisualState(true);
      this.RaiseEvent(this.Unselected, e);
    }

    protected virtual void OnGotFocus(RoutedEventArgs e)
    {
      TreeViewItem parentTreeViewItem = this.ParentTreeViewItem;
      if (parentTreeViewItem != null)
        parentTreeViewItem.CancelGotFocusBubble = true;
      try
      {
        if (!this.Interaction.AllowGotFocus(e) || this.CancelGotFocusBubble)
          return;
        this.Select(true);
        this.IsSelectionActive = true;
        this.UpdateVisualState(true);
        this.Interaction.OnGotFocusBase();
        ((Control) this).OnGotFocus(e);
      }
      finally
      {
        this.CancelGotFocusBubble = false;
      }
    }

    protected virtual void OnLostFocus(RoutedEventArgs e)
    {
      if (this.Interaction.AllowLostFocus(e))
      {
        this.Interaction.OnLostFocusBase();
        ((Control) this).OnLostFocus(e);
      }
      this.IsSelectionActive = false;
      this.UpdateVisualState(true);
    }

    private void OnExpanderGotFocus(object sender, RoutedEventArgs e)
    {
      this.CancelGotFocusBubble = true;
      this.IsSelectionActive = true;
      this.UpdateVisualState(true);
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

    private void OnHeaderMouseLeftButtonDown(object sender, PointerRoutedEventArgs e)
    {
      if (!this.Interaction.AllowMouseLeftButtonDown(e))
        return;
      if (!e.Handled && ((Control) this).IsEnabled)
      {
        if (((Control) this).Focus((FocusState) 3))
          e.put_Handled(true);
        if (this.Interaction.ClickCount % 2 == 0)
        {
          bool flag = !this.IsExpanded;
          this.UserInitiatedExpansion |= flag;
          this.IsExpanded = flag;
          e.put_Handled(true);
        }
      }
      this.Interaction.OnMouseLeftButtonDownBase();
      ((Control) this).OnPointerPressed(e);
    }

    private void OnExpanderClick(object sender, RoutedEventArgs e)
    {
      bool flag = !this.IsExpanded;
      this.UserInitiatedExpansion |= flag;
      this.IsExpanded = flag;
    }

    protected virtual void OnPointerPressed(PointerRoutedEventArgs e)
    {
      if (e == null)
        throw new ArgumentNullException(nameof (e));
      TreeView parentTreeView;
      if (!e.Handled && (parentTreeView = this.ParentTreeView) != null && parentTreeView.HandleMouseButtonDown())
        e.put_Handled(true);
      ((Control) this).OnPointerPressed(e);
    }

    protected virtual void OnPointerReleased(PointerRoutedEventArgs e)
    {
      if (!this.Interaction.AllowMouseLeftButtonUp(e))
        return;
      this.Interaction.OnMouseLeftButtonUpBase();
      ((Control) this).OnPointerReleased(e);
    }

    protected virtual void OnKeyDown(KeyRoutedEventArgs e)
    {
      ((Control) this).OnKeyDown(e);
      if (this.Interaction.AllowKeyDown(e))
      {
        if (e.Handled)
          return;
        VirtualKey logicalKey = InteractionHelper.GetLogicalKey(((FrameworkElement) this).FlowDirection, e.Key);
        switch (logicalKey - 37)
        {
          case 0:
            if (!TreeView.IsControlKeyDown && this.CanExpandOnInput && this.IsExpanded)
            {
              if (FocusManager.GetFocusedElement() != this)
                ((Control) this).Focus((FocusState) 3);
              else
                this.IsExpanded = false;
              e.put_Handled(true);
              break;
            }
            break;
          case 1:
            if (!TreeView.IsControlKeyDown && this.HandleUpKey())
            {
              e.put_Handled(true);
              break;
            }
            break;
          case 2:
            if (!TreeView.IsControlKeyDown && this.CanExpandOnInput)
            {
              if (!this.IsExpanded)
              {
                this.UserInitiatedExpansion = true;
                this.IsExpanded = true;
                e.put_Handled(true);
                break;
              }
              if (this.HandleDownKey())
              {
                e.put_Handled(true);
                break;
              }
              break;
            }
            break;
          case 3:
            if (!TreeView.IsControlKeyDown && this.HandleDownKey())
            {
              e.put_Handled(true);
              break;
            }
            break;
          default:
            switch (logicalKey - 107)
            {
              case 0:
                if (this.CanExpandOnInput && !this.IsExpanded)
                {
                  this.UserInitiatedExpansion = true;
                  this.IsExpanded = true;
                  e.put_Handled(true);
                  break;
                }
                break;
              case 2:
                if (this.CanExpandOnInput && this.IsExpanded)
                {
                  this.IsExpanded = false;
                  e.put_Handled(true);
                  break;
                }
                break;
            }
            break;
        }
      }
      if (!this.IsRoot)
        return;
      this.ParentTreeView?.PropagateKeyDown(e);
    }

    internal bool HandleDownKey() => this.AllowKeyHandleEvent() && this.FocusDown();

    protected virtual void OnKeyUp(KeyRoutedEventArgs e)
    {
      if (!this.Interaction.AllowKeyUp(e))
        return;
      ((Control) this).OnKeyUp(e);
    }

    internal bool HandleUpKey()
    {
      if (this.AllowKeyHandleEvent())
      {
        ItemsControl previousFocusableItem = this.FindPreviousFocusableItem();
        if (previousFocusableItem != null)
          return previousFocusableItem == this.ParentItemsControl && previousFocusableItem == this.ParentTreeView || ((Control) previousFocusableItem).Focus((FocusState) 3);
      }
      return false;
    }

    internal bool HandleScrollByPage(
      bool up,
      ScrollViewer scrollHost,
      double viewportHeight,
      double top,
      double bottom,
      out double currentDelta)
    {
      double closeEdge = 0.0;
      currentDelta = TreeViewItem.CalculateDelta(up, (FrameworkElement) this, scrollHost, top, bottom, out closeEdge);
      if (NumericExtensions.IsGreaterThan(closeEdge, viewportHeight) || NumericExtensions.IsLessThanOrClose(currentDelta, viewportHeight))
        return false;
      bool flag1 = false;
      FrameworkElement headerElement = this.HeaderElement;
      if (headerElement != null && NumericExtensions.IsLessThanOrClose(TreeViewItem.CalculateDelta(up, headerElement, scrollHost, top, bottom, out double _), viewportHeight))
        flag1 = true;
      TreeViewItem treeViewItem1 = (TreeViewItem) null;
      int count = ((ICollection<object>) this.Items).Count;
      bool flag2 = up && this.ContainsSelection;
      for (int index = up ? count - 1 : 0; 0 <= index && index < count; index += up ? -1 : 1)
      {
        if (this.ContainerFromIndex(index) is TreeViewItem treeViewItem2 && ((Control) treeViewItem2).IsEnabled)
        {
          if (flag2)
          {
            if (treeViewItem2.IsSelected)
            {
              flag2 = false;
              continue;
            }
            if (treeViewItem2.ContainsSelection)
              flag2 = false;
            else
              continue;
          }
          double currentDelta1;
          if (treeViewItem2.HandleScrollByPage(up, scrollHost, viewportHeight, top, bottom, out currentDelta1))
            return true;
          if (!NumericExtensions.IsGreaterThan(currentDelta1, viewportHeight))
            treeViewItem1 = treeViewItem2;
          else
            break;
        }
      }
      return treeViewItem1 != null ? (up ? ((Control) treeViewItem1).Focus((FocusState) 3) : treeViewItem1.FocusInto()) : flag1 && ((Control) this).Focus((FocusState) 3);
    }

    private static double CalculateDelta(
      bool up,
      FrameworkElement element,
      ScrollViewer scrollHost,
      double top,
      double bottom,
      out double closeEdge)
    {
      double top1;
      double bottom1;
      element.GetTopAndBottom((FrameworkElement) scrollHost, out top1, out bottom1);
      if (up)
      {
        closeEdge = bottom - bottom1;
        return bottom - top1;
      }
      closeEdge = top1 - top;
      return bottom1 - top;
    }

    private void Select(bool selected)
    {
      TreeView parentTreeView = this.ParentTreeView;
      if (parentTreeView == null || parentTreeView.IsSelectionChangeActive)
        return;
      TreeViewItem parentTreeViewItem = this.ParentTreeViewItem;
      object itemOrContainer = parentTreeViewItem != null ? parentTreeViewItem.ItemFromContainer((DependencyObject) this) : parentTreeView.ItemFromContainer((DependencyObject) this);
      parentTreeView.ChangeSelection(itemOrContainer, this, selected);
    }

    internal void UpdateContainsSelection(bool selected)
    {
      for (TreeViewItem parentTreeViewItem = this.ParentTreeViewItem; parentTreeViewItem != null; parentTreeViewItem = parentTreeViewItem.ParentTreeViewItem)
        parentTreeViewItem.ContainsSelection = selected;
    }

    private bool AllowKeyHandleEvent() => this.IsSelected;

    internal bool FocusDown()
    {
      TreeViewItem nextFocusableItem = this.FindNextFocusableItem(true);
      return nextFocusableItem != null && ((Control) nextFocusableItem).Focus((FocusState) 3);
    }

    internal bool FocusInto()
    {
      TreeViewItem lastFocusableItem = this.FindLastFocusableItem();
      return lastFocusableItem != null && ((Control) lastFocusableItem).Focus((FocusState) 3);
    }

    private TreeViewItem FindNextFocusableItem(bool recurse)
    {
      if (recurse && this.IsExpanded && this.HasItems && this.ContainerFromIndex(0) is TreeViewItem treeViewItem)
        return !((Control) treeViewItem).IsEnabled ? treeViewItem.FindNextFocusableItem(false) : treeViewItem;
      ItemsControl parentItemsControl = this.ParentItemsControl;
      if (parentItemsControl != null)
      {
        int num = parentItemsControl.IndexFromContainer((DependencyObject) this);
        int count = ((ICollection<object>) parentItemsControl.Items).Count;
        while (num++ < count)
        {
          if (parentItemsControl.ContainerFromIndex(num) is TreeViewItem nextFocusableItem && ((Control) nextFocusableItem).IsEnabled)
            return nextFocusableItem;
        }
        TreeViewItem parentTreeViewItem = this.ParentTreeViewItem;
        if (parentTreeViewItem != null)
          return parentTreeViewItem.FindNextFocusableItem(false);
      }
      return (TreeViewItem) null;
    }

    private TreeViewItem FindLastFocusableItem()
    {
      TreeViewItem lastFocusableItem1 = this;
      TreeViewItem lastFocusableItem2 = (TreeViewItem) null;
      int num = -1;
      for (; lastFocusableItem1 != null; lastFocusableItem1 = lastFocusableItem2.ContainerFromIndex(num) as TreeViewItem)
      {
        if (((Control) lastFocusableItem1).IsEnabled)
        {
          if (!lastFocusableItem1.IsExpanded || !lastFocusableItem1.HasItems)
            return lastFocusableItem1;
          lastFocusableItem2 = lastFocusableItem1;
          num = ((ICollection<object>) lastFocusableItem1.Items).Count - 1;
        }
        else if (num > 0)
          --num;
        else
          break;
      }
      return lastFocusableItem2;
    }

    private ItemsControl FindPreviousFocusableItem()
    {
      ItemsControl parentItemsControl = this.ParentItemsControl;
      if (parentItemsControl == null)
        return (ItemsControl) null;
      int num = parentItemsControl.IndexFromContainer((DependencyObject) this);
      while (num-- > 0)
      {
        if (parentItemsControl.ContainerFromIndex(num) is TreeViewItem treeViewItem && ((Control) treeViewItem).IsEnabled)
        {
          TreeViewItem lastFocusableItem = treeViewItem.FindLastFocusableItem();
          if (lastFocusableItem != null)
            return (ItemsControl) lastFocusableItem;
        }
      }
      return parentItemsControl;
    }
  }
}
