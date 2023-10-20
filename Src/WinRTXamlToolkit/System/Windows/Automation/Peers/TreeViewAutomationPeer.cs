// Decompiled with JetBrains decompiler
// Type: System.Windows.Automation.Peers.TreeViewAutomationPeer
// Assembly: WinRTXamlToolkit, Version=1.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 6647FB17-44D2-42F4-B473-555AE27B4E34
// Assembly location: C:\Users\Admin\Desktop\re\MyTube\WinRTXamlToolkit.dll

using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using WinRTXamlToolkit.Controls;

namespace System.Windows.Automation.Peers
{
  public class TreeViewAutomationPeer : FrameworkElementAutomationPeer, ISelectionProvider
  {
    private TreeView OwnerTreeView => (TreeView) this.Owner;

    bool ISelectionProvider.CanSelectMultiple => false;

    bool ISelectionProvider.IsSelectionRequired => false;

    public TreeViewAutomationPeer(TreeView owner)
      : base((FrameworkElement) owner)
    {
    }

    protected virtual AutomationControlType GetAutomationControlTypeCore() => (AutomationControlType) 23;

    protected virtual string GetClassNameCore() => "TreeView";

    protected virtual object GetPatternCore(PatternInterface patternInterface)
    {
      if (patternInterface == 1)
        return (object) this;
      if (patternInterface == 4)
      {
        ScrollViewer scrollHost = this.OwnerTreeView.ItemsControlHelper.ScrollHost;
        if (scrollHost != null)
        {
          AutomationPeer peerForElement = FrameworkElementAutomationPeer.CreatePeerForElement((UIElement) scrollHost);
          if (peerForElement is IScrollProvider patternCore)
          {
            peerForElement.put_EventsSource((AutomationPeer) this);
            return (object) patternCore;
          }
        }
      }
      return (object) null;
    }

    IRawElementProviderSimple[] ISelectionProvider.GetSelection()
    {
      IRawElementProviderSimple[] elementProviderSimpleArray = (IRawElementProviderSimple[]) null;
      TreeViewItem selectedContainer = this.OwnerTreeView.SelectedContainer;
      if (selectedContainer != null)
      {
        AutomationPeer automationPeer = FrameworkElementAutomationPeer.FromElement((UIElement) selectedContainer);
        if (automationPeer != null)
          elementProviderSimpleArray = new IRawElementProviderSimple[1]
          {
            ((AutomationPeer) this).ProviderFromPeer(automationPeer)
          };
      }
      return elementProviderSimpleArray ?? new IRawElementProviderSimple[0];
    }
  }
}
