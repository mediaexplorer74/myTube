// Decompiled with JetBrains decompiler
// Type: System.Windows.Automation.Peers.TreeViewItemAutomationPeer
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;

namespace System.Windows.Automation.Peers
{
  public class TreeViewItemAutomationPeer : 
    FrameworkElementAutomationPeer,
    IExpandCollapseProvider,
    ISelectionItemProvider,
    IScrollItemProvider
  {
    private TreeViewItem OwnerTreeViewItem => (TreeViewItem) this.Owner;

    ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
    {
      get
      {
        TreeViewItem ownerTreeViewItem = this.OwnerTreeViewItem;
        if (!ownerTreeViewItem.HasItems)
          return (ExpandCollapseState) 3;
        return !ownerTreeViewItem.IsExpanded ? (ExpandCollapseState) 0 : (ExpandCollapseState) 1;
      }
    }

    bool ISelectionItemProvider.IsSelected => this.OwnerTreeViewItem.IsSelected;

    IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
    {
      get
      {
        ItemsControl parentItemsControl = this.OwnerTreeViewItem.ParentItemsControl;
        if (parentItemsControl != null)
        {
          AutomationPeer automationPeer = FrameworkElementAutomationPeer.FromElement((UIElement) parentItemsControl);
          if (automationPeer != null)
            return ((AutomationPeer) this).ProviderFromPeer(automationPeer);
        }
        return (IRawElementProviderSimple) null;
      }
    }

    public TreeViewItemAutomationPeer(TreeViewItem owner)
      : base((FrameworkElement) owner)
    {
    }

    protected virtual AutomationControlType GetAutomationControlTypeCore() => (AutomationControlType) 24;

    protected virtual string GetClassNameCore() => "TreeViewItem";

    protected virtual object GetPatternCore(PatternInterface patternInterface) => patternInterface == 6 || patternInterface == 11 || patternInterface == 5 ? (object) this : (object) null;

    internal void RaiseAutomationIsSelectedChanged(bool isSelected) => ((AutomationPeer) this).RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, (object) !isSelected, (object) isSelected);

    internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue) => ((AutomationPeer) this).RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, (object) (ExpandCollapseState) (oldValue ? 1 : 0), (object) (ExpandCollapseState) (newValue ? 1 : 0));

    void IExpandCollapseProvider.Expand()
    {
      if (!((AutomationPeer) this).IsEnabled())
        throw new ElementNotEnabledException();
      TreeViewItem ownerTreeViewItem = this.OwnerTreeViewItem;
      if (!ownerTreeViewItem.HasItems)
        throw new InvalidOperationException("Controls.Properties.Resources.Automation_OperationCannotBePerformed");
      ownerTreeViewItem.IsExpanded = true;
    }

    void IExpandCollapseProvider.Collapse()
    {
      if (!((AutomationPeer) this).IsEnabled())
        throw new ElementNotEnabledException();
      TreeViewItem ownerTreeViewItem = this.OwnerTreeViewItem;
      if (!ownerTreeViewItem.HasItems)
        throw new InvalidOperationException("Controls.Properties.Resources.Automation_OperationCannotBePerformed");
      ownerTreeViewItem.IsExpanded = false;
    }

    void ISelectionItemProvider.AddToSelection()
    {
      TreeViewItem ownerTreeViewItem = this.OwnerTreeViewItem;
      TreeView parentTreeView = ownerTreeViewItem.ParentTreeView;
      if (parentTreeView == null || parentTreeView.SelectedItem != null && parentTreeView.SelectedContainer != this.Owner)
        throw new InvalidOperationException("Controls.Properties.Resources.Automation_OperationCannotBePerformed");
      ownerTreeViewItem.IsSelected = true;
    }

    void ISelectionItemProvider.Select() => this.OwnerTreeViewItem.IsSelected = true;

    void ISelectionItemProvider.RemoveFromSelection() => this.OwnerTreeViewItem.IsSelected = false;

    void IScrollItemProvider.ScrollIntoView()
    {
      TreeViewItem ownerTreeViewItem = this.OwnerTreeViewItem;
      ownerTreeViewItem.ParentTreeView?.ItemsControlHelper.ScrollIntoView((FrameworkElement) ownerTreeViewItem);
    }
  }
}
