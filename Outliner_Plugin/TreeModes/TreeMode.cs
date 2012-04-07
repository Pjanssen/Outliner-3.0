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
public abstract class TreeMode : Autodesk.Max.Plugins.INodeEventCallback
{
   protected TreeView tree { get; private set; }
   protected Autodesk.Max.IInterface ip { get; private set; }
   private uint sceneEventCbKey;

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

      IGlobal iGlobal = GlobalInterface.Instance;
      iGlobal.RegisterNotification(PostReset, null, SystemNotificationCode.SystemPostReset);
      iGlobal.RegisterNotification(SelectionsetChanged, null, SystemNotificationCode.SelectionsetChanged);

      this.sceneEventCbKey = iGlobal.ISceneEventManager.RegisterCallback(this, false, 100, true);
   }


   /// <summary>
   /// Cleanup of event notifications and callbacks.
   /// </summary>
   public void Unregister()
   {
      IGlobal iGlobal = GlobalInterface.Instance;
      iGlobal.UnRegisterNotification(PostReset, null);
      iGlobal.UnRegisterNotification(SelectionsetChanged, null);

      iGlobal.ISceneEventManager.UnRegisterCallback(this.sceneEventCbKey);

      base.Dispose();
   }

   public abstract void FillTree();


   #region Helper methods
      
   public virtual TreeNode GetTreeNode(Object node)
   {
      TreeNode tn = null;
      if (node != null)
         this.treeNodes.TryGetValue(node, out tn);
      return tn;
   }

   protected virtual TreeNode addNode(IMaxNodeWrapper node, TreeNodeCollection parentCol)
   {
      if (node == null || parentCol == null)
         return null;

      FilterResults filterResult = this.Filters.ShowNode(node);
      if (filterResult != FilterResults.Hide && !this.treeNodes.ContainsKey(node))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(node);
         tn.FilterResult = filterResult;

         this.treeNodes.Add(node, tn);
         parentCol.Add(tn);

         if (node.Selected)
            this.tree.SelectNode(tn, true);

         return tn;
      }
      else
         return null;
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

   #endregion



   #region NodeEvent Callbacks

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
         this.RemoveTreeNode(node);
   }

   public override void NameChanged(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         TreeNode tn = this.GetTreeNode(node);
         IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);
         if (tn != null && wrapper != null)
         {
            tn.Text = wrapper.DisplayName;
            this.tree.AddToSortQueue(tn);
         }
      }
      this.tree.StartTimedSort(true);
   }

   public override void DisplayPropertiesChanged(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         TreeNode tn = this.GetTreeNode(node);
         if (tn != null)
            tn.Invalidate();
      }
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

   public virtual void PostReset(IntPtr param, IntPtr info)
   {
      this.tree.Nodes.Clear();
      this.treeNodes.Clear();
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

      return node.ChildNodes;
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
}
}
