using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.Controls;
using Outliner.Filters;
// Import System.Windows.Forms with alias to avoid ambiguity 
// between System.Windows.TreeNode and Outliner.Controls.TreeNode.
using WinForms = System.Windows.Forms;

namespace Outliner.TreeModes
{
public abstract class TreeMode //: Autodesk.Max.Plugins.INodeEventCallback
{
   protected TreeView tree { get; private set; }
   protected Autodesk.Max.IInterface ip { get; private set; }
   protected DefaultNodeEventCallbacks defaultCb;
   protected uint defaultCbKey;

   protected Dictionary<Object, TreeNode> treeNodes { get; private set; }

   protected TreeMode(TreeView tree, Autodesk.Max.IInterface ip)
   {
      this.tree = tree;
      this.ip = ip;
      this.treeNodes = new Dictionary<Object, TreeNode>();
      this.Filters = new FilterCollection<IMaxNodeWrapper>();
      this.Filters.Add(new InvisibleNodeFilter());

      this.tree.SelectionChanged += new EventHandler<SelectionChangedEventArgs>(tree_SelectionChanged);
      this.tree.BeforeNodeTextEdit += new EventHandler<BeforeNodeTextEditEventArgs>(tree_BeforeNodeTextEdit);
      this.tree.AfterNodeTextEdit += new EventHandler<AfterNodeTextEditEventArgs>(tree_AfterNodeTextEdit);

      this.RegisterSystemNotifications();
      this.RegisterNodeCallbacks();
   }

   public virtual void RegisterSystemNotifications()
   {
      IGlobal iGlobal = GlobalInterface.Instance;
      iGlobal.RegisterNotification(SystemPreNew, null, SystemNotificationCode.SystemPreNew);
      iGlobal.RegisterNotification(SystemPostNew, null, SystemNotificationCode.SystemPostNew);
      iGlobal.RegisterNotification(SystemPreReset, null, SystemNotificationCode.SystemPreReset);
      iGlobal.RegisterNotification(SystemPostReset, null, SystemNotificationCode.SystemPostReset);
      iGlobal.RegisterNotification(FilePreOpen, null, SystemNotificationCode.FilePreOpen);
      iGlobal.RegisterNotification(FilePostOpen, null, SystemNotificationCode.FilePostOpen);
      iGlobal.RegisterNotification(SelectionsetChanged, null, SystemNotificationCode.SelectionsetChanged);
   }

   /// <summary>
   /// Cleanup of event notifications and callbacks.
   /// </summary>
   public virtual void UnregisterSystemNotifications()
   {
      IGlobal iGlobal = GlobalInterface.Instance;
      iGlobal.UnRegisterNotification(SystemPostReset, null);
      iGlobal.UnRegisterNotification(SelectionsetChanged, null);
   }

   public virtual void RegisterNodeCallbacks()
   {
      this.defaultCb = new DefaultNodeEventCallbacks(this);
      IISceneEventManager sceneEventMgr = GlobalInterface.Instance.ISceneEventManager;
      this.defaultCbKey = sceneEventMgr.RegisterCallback(this.defaultCb, false, 100, true);
   }

   public virtual void UnregisterNodeCallbacks()
   {
      IISceneEventManager sceneEventMgr = GlobalInterface.Instance.ISceneEventManager;
      sceneEventMgr.UnRegisterCallback(this.defaultCbKey);
      this.defaultCb.Dispose();
      this.defaultCb = null;
   }

   public abstract void FillTree();


   #region Helper methods
   
   public virtual TreeNode CreateTreeNode(IMaxNodeWrapper node)
   {
      if (node == null)
         return null;

      MaxTreeNode tn = new MaxTreeNode(node);
      return tn;
   }

   public virtual TreeNode GetTreeNode(Object node)
   {
      TreeNode tn = null;
      if (node != null)
         this.treeNodes.TryGetValue(node, out tn);
      return tn;
   }

   public virtual TreeNode AddNode(Object node, TreeNodeCollection parentCol)
   {
      if (node == null || parentCol == null)
         return null;

      TreeNode tn = null;
      IMaxNodeWrapper wrapper = null;
      if (this.treeNodes.TryGetValue(node, out tn))
         wrapper = HelperMethods.GetMaxNode(tn);
      else
      {
         wrapper = IMaxNodeWrapper.Create(node);
         tn = this.CreateTreeNode(wrapper);
         this.treeNodes.Add(node, tn);
      }

      tn.FilterResult = this.Filters.ShowNode(wrapper);
      if (tn.FilterResult != FilterResults.Hide)
         parentCol.Add(tn);

      if (wrapper.Selected)
         this.tree.SelectNode(tn, true);

      return tn;
   }


   public virtual void RemoveTreeNode(Object node)
   {
      TreeNode tn = this.GetTreeNode(node);
      if (tn != null)
      {
         this.tree.SelectedNodes.Remove(tn);
         tn.Remove();
         this.treeNodes.Remove(node);
      }
   }


   public void ClearTreeNodes()
   {
      this.tree.Nodes.Clear();
      this.treeNodes.Clear();
   }


   public virtual void InvalidateTreeNodes(ITab<UIntPtr> nodes, Boolean invalidateBounds, Boolean sort)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         TreeNode tn = this.GetTreeNode(node);
         if (tn != null)
         {
            if (invalidateBounds)
               tn.InvalidateBounds(false, false);

            tn.Invalidate();

            if (sort)
               this.tree.AddToSortQueue(tn);
         }
      }

      if (sort)
         this.tree.StartTimedSort(true);
   }


   #endregion


   #region System notifications

   public virtual void SelectionsetChanged(IntPtr param, IntPtr info)
   {
      this.tree.SelectAllNodes(false);

      Int32 selNodeCount = this.ip.SelNodeCount;
      if (selNodeCount > 0)
      {
         for (Int32 i = 0; i < selNodeCount; i++)
         {
            TreeNode tn;
            if (this.treeNodes.TryGetValue(ip.GetSelNode(i), out tn))
               this.tree.SelectNode(tn, true);
         }
      }
   }

   public virtual void SystemPreNew(IntPtr param, IntPtr info)
   {
      this.UnregisterNodeCallbacks();
      this.ClearTreeNodes();
   }

   public virtual void SystemPostNew(IntPtr param, IntPtr info)
   {
      this.RegisterNodeCallbacks();
      this.FillTree();
   }

   public virtual void SystemPreReset(IntPtr param, IntPtr info)
   {
      this.UnregisterNodeCallbacks();
      this.ClearTreeNodes();
   }

   public virtual void SystemPostReset(IntPtr param, IntPtr info)
   {
      this.RegisterNodeCallbacks();
      this.FillTree();
   }

   public virtual void FilePreOpen(IntPtr param, IntPtr info)
   {
      this.UnregisterNodeCallbacks();
      this.ClearTreeNodes();
   }

   public virtual void FilePostOpen(IntPtr param, IntPtr info)
   {
      this.RegisterNodeCallbacks();
      this.FillTree();
   }

   #endregion


   #region Tree events

   void tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      SelectCommand cmd = new SelectCommand(HelperMethods.GetMaxNodes(e.Nodes));
      cmd.Execute(true);
   }

   void tree_BeforeNodeTextEdit(object sender, BeforeNodeTextEditEventArgs e)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(e.TreeNode);
      if (node == null)
      {
         e.Cancel = true;
         return;
      }

      if (!node.CanEditName)
      {
         WinForms::MessageBox.Show(tree, 
                                   OutlinerResources.Warning_CannotEditName, 
                                   OutlinerResources.Warning_CannotEditNameTitle, 
                                   WinForms::MessageBoxButtons.OK, 
                                   WinForms::MessageBoxIcon.Warning);
         e.Cancel = true;
         return;
      }

      e.EditText = node.Name;
   }

   void tree_AfterNodeTextEdit(object sender, AfterNodeTextEditEventArgs e)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(e.TreeNode);
      if (node == null)
         return;

      RenameCommand cmd = new RenameCommand(new List<IMaxNodeWrapper>(1) { node }, e.NewText);
      cmd.Execute(false);

      //Note: setting treenode text to displayname and sorting are
      //      handled by nodenamechanged callback.
   }

   #endregion


   #region Filters

   protected IEnumerable<IMaxNodeWrapper> GetChildNodes(IMaxNodeWrapper node)
   {
      if (node == null)
         return null;

      return node.WrappedChildNodes;
   }

   private FilterCollection<IMaxNodeWrapper> _filters;
   public FilterCollection<IMaxNodeWrapper> Filters
   {
      get { return _filters; }
      set
      {
         if (value == null)
            throw new ArgumentNullException("value");

         if (_filters != null)
         {
            _filters.FiltersEnabled -= this.filtersEnabled;
            _filters.FiltersCleared -= this.filtersCleared;
            _filters.FilterAdded -= this.filterAdded;
            _filters.FilterRemoved -= this.filterRemoved;
            _filters.FilterChanged -= this.filterChanged;
         }

         _filters = value;
         _filters.GetChildNodesFn = this.GetChildNodes;

         _filters.FiltersEnabled += this.filtersEnabled;
         _filters.FiltersCleared += this.filtersCleared;
         _filters.FilterAdded += this.filterAdded;
         _filters.FilterRemoved += this.filterRemoved;
         _filters.FilterChanged += this.filterChanged;
      }
   }

   private void filtersEnabled(object sender, EventArgs e)
   {

   }
   private void filtersCleared(object sender, EventArgs e)
   {

   }
   private void filterAdded(object sender, FilterChangedEventArgs<IMaxNodeWrapper> e)
   {

   }
   private void filterRemoved(object sender, FilterChangedEventArgs<IMaxNodeWrapper> e)
   {

   }
   private void filterChanged(object sender, FilterChangedEventArgs<IMaxNodeWrapper> e)
   {

   }

   #endregion


   protected abstract class TreeModeNodeEventCallbacks : Autodesk.Max.Plugins.INodeEventCallback
   {
      protected TreeMode treeMode;
      protected TreeView tree { get { return this.treeMode.tree; } }
      protected Dictionary<Object, TreeNode> treeNodes { get { return this.treeMode.treeNodes; } }

      public TreeModeNodeEventCallbacks(TreeMode treeMode)
      {
         this.treeMode = treeMode;
      }
   }
   protected class DefaultNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      public DefaultNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void CallbackBegin()
      {
         this.tree.BeginUpdate();
      }

      public override void CallbackEnd()
      {
         this.tree.EndUpdate();
      }

      public override void Deleted(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
            this.treeMode.RemoveTreeNode(node);
      }

      public override void NameChanged(ITab<UIntPtr> nodes)
      {
         this.treeMode.InvalidateTreeNodes(nodes, true, true);
      }

      public override void DisplayPropertiesChanged(ITab<UIntPtr> nodes)
      {
         Boolean sort = this.tree.NodeSorter is NodeSorters.FrozenSorter
                      || this.tree.NodeSorter is NodeSorters.HiddenSorter;
         this.treeMode.InvalidateTreeNodes(nodes, false, sort);
      }

      public override void RenderPropertiesChanged(ITab<UIntPtr> nodes)
      {
         this.treeMode.InvalidateTreeNodes(nodes, false, false);
      }
   }
}
}
