using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Controls.ContextMenu;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.LayerTools;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.Scene;
using WinForms = System.Windows.Forms;

namespace PJanssen.Outliner.Modes
{
/// <summary>
/// A baseclass for a TreeMode, which controls how the TreeView is filled and updated.
/// </summary>
public abstract class TreeMode
{
   /// <summary>
   /// Gets whether this TreeMode has been started.
   /// </summary>
   public Boolean Started { get; private set; }

   /// <summary>
   /// Gets the TreeView control associated with this TreeMode.
   /// </summary>
   public TreeView Tree { get; private set; }

   private ICollection<Tuple<GlobalDelegates.Delegate5, SystemNotificationCode>> systemNotifications;
   private ICollection<Tuple<uint, TreeModeNodeEventCallbacks>> nodeEventCallbacks;

   private GlobalDelegates.Delegate5 proc_PausePreSystemEvent;
   private GlobalDelegates.Delegate5 proc_ResumePostSystemEvent;
   private GlobalDelegates.Delegate5 proc_SelectionsetChanged;
   private GlobalDelegates.Delegate5 proc_NodeRenamed;
   private GlobalDelegates.Delegate5 proc_LayerHiddenChanged;
   private GlobalDelegates.Delegate5 proc_LayerFrozenChanged;
   private GlobalDelegates.Delegate5 proc_LayerPropChanged;

   protected Dictionary<Object, ICollection<TreeNode>> treeNodes { get; private set; }

   private FilterCombinator<IMaxNode> filters;
   private const Int32 InvisibleNodesFilterIndex = 0;
   private const Int32 OtherFiltersIndex = 1;

   /// <summary>
   /// Initializes a new instance of the TreeMode class.
   /// </summary>
   /// <param name="tree">The TreeView control which this TreeMode operates on.</param>
   protected TreeMode(TreeView tree)
   {
      Throw.IfNull(tree, "tree");

      proc_PausePreSystemEvent = new GlobalDelegates.Delegate5(this.PausePreSystemEvent);
      proc_ResumePostSystemEvent = new GlobalDelegates.Delegate5(this.ResumePostSystemEvent);
      proc_SelectionsetChanged = new GlobalDelegates.Delegate5(this.SelectionSetChanged);
      proc_NodeRenamed = new GlobalDelegates.Delegate5(this.NodeRenamed);
      proc_LayerHiddenChanged = new GlobalDelegates.Delegate5(this.LayerHiddenChanged);
      proc_LayerFrozenChanged = new GlobalDelegates.Delegate5(this.LayerFrozenChanged);
      proc_LayerPropChanged = new GlobalDelegates.Delegate5(this.LayerPropChanged);

      this.Tree = tree;
      this.treeNodes = new Dictionary<Object, ICollection<TreeNode>>();

      this.filters = new FilterCombinator<IMaxNode>(Functor.And);
      this.filters.Filters.Add(new InvisibleNodeFilter());
      this.filters.Filters.Add(new MaxNodeFilterCombinator() { Enabled = false });
      this.filters.FilterChanged += filters_FilterChanged;

      this.Started = false;
   }

   /// <summary>
   /// Fills the associated TreeView with nodes.
   /// </summary>
   protected abstract void FillTree();

   public void Refresh()
   {
      this.Tree.Nodes.Clear();
      this.treeNodes.Clear();

      this.FillTree();
   }

   #region Start, Stop
   
   /// <summary>
   /// Starts the TreeMode: registers system notifications, NodeEventCallbacks, and fills the TreeView.
   /// </summary>
   public virtual void Start()
   {
      if (this.Started)
         return;

      this.RegisterSystemNotification(proc_PausePreSystemEvent, SystemNotificationCode.SystemPreNew);
      this.RegisterSystemNotification(proc_PausePreSystemEvent, SystemNotificationCode.SystemPreReset);
      this.RegisterSystemNotification(proc_PausePreSystemEvent, SystemNotificationCode.FilePreOpen);
      this.RegisterSystemNotification(proc_PausePreSystemEvent, SystemNotificationCode.FilePreMerge);
      this.RegisterSystemNotification(proc_SelectionsetChanged, SystemNotificationCode.SelectionsetChanged);
      this.RegisterSystemNotification(proc_NodeRenamed, SystemNotificationCode.NodeRenamed);
      this.RegisterSystemNotification(proc_NodeRenamed, SystemNotificationCode.LayerRenamed);

      this.RegisterSystemNotification(this.proc_LayerHiddenChanged, SystemNotificationCode.LayerHiddenStateChanged);
      this.RegisterSystemNotification(this.proc_LayerFrozenChanged, SystemNotificationCode.LayerFrozenStateChanged);
      this.RegisterSystemNotification(this.proc_LayerPropChanged, LayerNotificationCode.LayerPropertyChanged);

      this.RegisterNodeEventCallbackObject(new DefaultNodeEventCallbacks(this));

      this.Tree.SelectionChanged += tree_SelectionChanged;
      this.Tree.BeforeNodeTextEdit += tree_BeforeNodeTextEdit;
      this.Tree.AfterNodeTextEdit += tree_AfterNodeTextEdit;
      this.Tree.MouseClick += tree_MouseClick;

      this.FillTree();

      Analytics.TreeModeStarted(this.GetType().Name);

      this.Started = true;
   }

   /// <summary>
   /// Stops the TreeMode: unregistes SystemNotifications, NodeEventCallbacks and clears the TreeView.
   /// </summary>
   public virtual void Stop()
   {
      if (!Started)
         return;

      this.UnregisterSystemNotifications();
      this.UnregisterNodeEventCallbacks();

      this.Tree.SelectionChanged -= tree_SelectionChanged;
      this.Tree.BeforeNodeTextEdit -= tree_BeforeNodeTextEdit;
      this.Tree.AfterNodeTextEdit -= tree_AfterNodeTextEdit;
      this.Tree.MouseClick -= tree_MouseClick;

      this.Tree.Nodes.Clear();
      this.treeNodes.Clear();

      this.Started = false;
   }

   #endregion


   #region Register SystemNotifications and NodeEventCallbacks

   /// <summary>
   /// Registers a SystemNotification proc, which will be automatically unregistered when UnregisterSystemNotifications is called.
   /// </summary>
   protected void RegisterSystemNotification(GlobalDelegates.Delegate5 proc, SystemNotificationCode code)
   {
      if (this.systemNotifications == null)
         this.systemNotifications = new List<Tuple<GlobalDelegates.Delegate5, SystemNotificationCode>>();

      int regResult = MaxInterfaces.Global.RegisterNotification(proc, null, code);
      if (regResult != 0)
         this.systemNotifications.Add(new Tuple<GlobalDelegates.Delegate5, SystemNotificationCode>(proc, code));
   }

   /// <summary>
   /// Unregisters a SystemNotification. 
   /// Be sure not to create new delegates when calling this method, but stored ones used when registering it.
   /// </summary>
   protected void UnregisterSystemNotification(GlobalDelegates.Delegate5 proc, SystemNotificationCode code)
   {
      int unregResult = MaxInterfaces.Global.UnRegisterNotification(proc, null, code);

      if (unregResult != 0 && this.systemNotifications != null)
      {
         this.systemNotifications.Remove(new Tuple<GlobalDelegates.Delegate5, SystemNotificationCode>(proc, code));
      }
   }

   /// <summary>
   /// Unregisters all SystemNotifications registered using RegisterSystemNotification.
   /// </summary>
   protected virtual void UnregisterSystemNotifications()
   {
      if (this.systemNotifications == null)
         return;

      this.systemNotifications.ForEach(n =>
         MaxInterfaces.Global.UnRegisterNotification(n.Item1, null, n.Item2));
      
      this.systemNotifications.Clear();
      this.systemNotifications = null;
   }


   /// <summary>
   /// Registers a NodeEventCallback object, which will be automatically unregistered when UnregisterNodeEventCallbacks is called.
   /// </summary>
   protected void RegisterNodeEventCallbackObject(TreeModeNodeEventCallbacks cb)
   {
      if (nodeEventCallbacks == null)
         this.nodeEventCallbacks = new List<Tuple<uint, TreeModeNodeEventCallbacks>>();
      
      IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
      uint cbKey = sceneEventMgr.RegisterCallback(cb, false, 100, true);

      this.nodeEventCallbacks.Add(new Tuple<uint, TreeModeNodeEventCallbacks>(cbKey, cb));
   }

   /// <summary>
   /// Unregisters all NodeEventCallbacks registered using RegisterNodeEventCallbackObject.
   /// </summary>
   protected virtual void UnregisterNodeEventCallbacks()
   {
      if (this.nodeEventCallbacks == null)
         return;

      IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
      foreach (Tuple<uint, TreeModeNodeEventCallbacks> cb in this.nodeEventCallbacks)
      {
         sceneEventMgr.UnRegisterCallback(cb.Item1);
         cb.Item2.Dispose();
      }
      this.nodeEventCallbacks.Clear();
      this.nodeEventCallbacks = null;
   }

   protected abstract class TreeModeNodeEventCallbacks : Autodesk.Max.Plugins.INodeEventCallback
   {
      protected TreeMode TreeMode { get; private set; }
      protected TreeView Tree { get { return this.TreeMode.Tree; } }
      protected Dictionary<Object, ICollection<TreeNode>> TreeNodes 
      { 
         get { return this.TreeMode.treeNodes; } 
      }
      protected NodeSorter NodeSorter
      {
         get
         {
            if (this.Tree != null)
               return this.Tree.NodeSorter as NodeSorter;
            else
               return null;
         }
      }

      protected TreeModeNodeEventCallbacks(TreeMode treeMode)
      {
         Throw.IfNull(treeMode, "treeMode");

         this.TreeMode = treeMode;
      }

      public override void CallbackBegin()
      {
         this.Tree.BeginUpdate(TreeViewUpdateFlags.Redraw);
      }

      public override void CallbackEnd()
      {
         this.Tree.EndUpdate();
      }
   }

   #endregion


   #region Helper methods

   /// <summary>
   /// Returns the NodeWrapper from the Tag of a TreeNode.
   /// </summary>
   public static IMaxNode GetMaxNode(TreeNode tn)
   {
      if (tn == null)
         return null;

      MaxTreeNode maxTn = tn as MaxTreeNode;
      if (maxTn == null)
         return null;
      else
         return maxTn.MaxNode;
   }

   /// <summary>
   /// Maps GetMaxNode to a list of TreeNodes, returning a list of NodeWrappers.
   /// </summary>
   public static IEnumerable<IMaxNode> GetMaxNodes(IEnumerable<TreeNode> treeNodes)
   {
      return treeNodes.Select(TreeMode.GetMaxNode);
   }

   public virtual IEnumerable<TreeNode> GetTreeNodes(IMaxNode wrapper)
   {
      if (wrapper == null)
         return null;

      return this.GetTreeNodes(wrapper.BaseObject);
   }

   public virtual IEnumerable<TreeNode> GetTreeNodes(Object node)
   {
      ICollection<TreeNode> tns = null;
      if (node != null)
         this.treeNodes.TryGetValue(node, out tns);
      return tns;
   }

   /// <summary>
   /// Returns the first TreeNode found in the TreeNodes dictionary.
   /// Use when it's certain that each node has only a single TreeNode.
   /// </summary>
   public virtual TreeNode GetFirstTreeNode(Object node)
   {
      IEnumerable<TreeNode> tns = this.GetTreeNodes(node);
      if (tns != null)
         return tns.FirstOrDefault();
      else
         return null;
   }

   public virtual TreeNode GetFirstTreeNode(IMaxNode wrapper)
   {
      if (wrapper == null)
         return null;

      return this.GetFirstTreeNode(wrapper.BaseObject);
   }

   public virtual void RegisterNode(Object node, TreeNode tn)
   {
      ICollection<TreeNode> tns;
      if (!this.treeNodes.TryGetValue(node, out tns))
      {
         tns = new List<TreeNode>();
         this.treeNodes.Add(node, tns);
      }
      tns.Add(tn);
   }

   public virtual void RegisterNode(IMaxNode node, TreeNode tn)
   {
      if (node == null)
         return;

      this.RegisterNode(node.BaseObject, tn);
   }

   public virtual void UnregisterNode(Object node, TreeNode tn)
   {
      ICollection<TreeNode> tns;
      if (this.treeNodes.TryGetValue(node, out tns))
      {
         tns.Remove(tn);
         if (tns.Count == 0)
            this.treeNodes.Remove(node);
      }
   }

   public virtual void UnregisterNode(IMaxNode wrapper, TreeNode tn)
   {
      if (wrapper == null)
         return;

      this.UnregisterNode(wrapper.BaseObject, tn);
   }

   public virtual void UnregisterNode(Object node)
   {
      this.treeNodes.Remove(node);
   }

   public virtual void UnregisterNode(IMaxNode wrapper)
   {
      if (wrapper == null)
         return;

      this.UnregisterNode(wrapper.BaseObject);
   }

   #endregion


   #region AddNode, RemoveNode

   /// <summary>
   /// Adds the given basenode to the TreeView, creating a new TreeNode under the given parent TreeNodeCollection.
   /// </summary>
   /// <remarks>This overload uses the IMaxNodeFactories to create a wrapper for the basenode.</remarks>
   /// <param name="node">The node to add to the TreeView.</param>
   /// <param name="parentCol">The TreeNodeCollection to add the new TreeNode to.</param>
   /// <returns>The newly created TreeNode.</returns>
   public virtual TreeNode AddNode(Object node, TreeNodeCollection parentCol)
   {
      Throw.IfNull(node, "node");
      Throw.IfNull(parentCol, "parentCol");

      return this.AddNode(MaxNodeWrapper.Create(node), parentCol);
   }

   /// <summary>
   /// Adds the given wrapped node to the TreeView, creating a new TreeNode under the given parent TreeNodeCollection.
   /// </summary>
   /// <param name="wrapper">The wrapped node to add to the TreeView.</param>
   /// <param name="parentCol">The TreeNodeCollection to add the new TreeNode to.</param>
   /// <returns>The newly created TreeNode.</returns>
   public virtual TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol)
   {
      Throw.IfNull(wrapper, "wrapper");
      Throw.IfNull(parentCol, "parentCol");

      TreeNode tn = new MaxTreeNode(wrapper);
      this.RegisterNode(wrapper, tn);

      tn.ShowNode = this.filters.ShowNode(wrapper);
      tn.DragDropHandler = this.CreateDragDropHandler(wrapper);

      parentCol.Add(tn);

      if (wrapper.IsSelected)
         this.Tree.SelectNode(tn, true);

      return tn;
   }

   /// <summary>
   /// Creates a DragDropHandler object for the given IMaxNode
   /// </summary>
   /// <param name="node">The IMaxNode to create a DragDropHandler for.</param>
   /// <remarks>Override this method in subclasses to add custom dragdrop logic to TreeNodes
   /// created by the TreeMode.</remarks>
   protected virtual IDragDropHandler CreateDragDropHandler(IMaxNode node)
   {
      return null;
   }

   protected virtual Boolean ShouldAddNode(object obj)
   {
      return true;
   }

   protected virtual Boolean ShouldAddNode(IINode node)
   {
      return true;
   }

   protected virtual TreeNode GetParentTreeNode(object obj)
   {
      return this.Tree.Root;
   }

   protected virtual TreeNode GetParentTreeNode(IINode node)
   {
      return this.Tree.Root;
   }

   /// <summary>
   /// Unregisters the given object and removes the associated TreeNode(s) from TreeView.
   /// </summary>
   /// <param name="wrapper">The IMaxNode wrapping the object to remove.</param>
   public virtual void RemoveNode(IMaxNode wrapper)
   {
      this.RemoveNode(wrapper.BaseObject);
   }

   /// <summary>
   /// Unregisters the given object and removes the associated TreeNode(s) from TreeView.
   /// </summary>
   /// <param name="node">The node to remove.</param>
   public virtual void RemoveNode(Object node)
   {
      IEnumerable<TreeNode> tns = this.GetTreeNodes(node);
      if (tns != null)
      {
         foreach (TreeNode tn in tns)
         {
            this.Tree.SelectNode(tn, false);
            tn.Remove();
         }
         this.UnregisterNode(node);
      }
   }

   #endregion


   #region Invalidate, UpdateFilter
   
   public virtual void InvalidateObject(Object obj, Boolean recursive, Boolean sort)
   {
      if (obj != null)
      {
         IEnumerable<TreeNode> tns = this.GetTreeNodes(obj);
         if (tns != null)
         {
            tns.ForEach(tn => tn.Invalidate(recursive));
            if (sort)
               this.Tree.StartTimedSort(tns);
         }
      }
   }

   public virtual void InvalidateTreeNodes(ITab<UIntPtr> nodes, Boolean sort)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         IEnumerable<TreeNode> tns = this.GetTreeNodes(node);
         if (tns != null)
         {
            tns.ForEach(tn => tn.Invalidate());

            if (sort)
               this.Tree.AddToSortQueue(tns);
         }
      }

      if (sort)
         this.Tree.StartTimedSort(true);
   }

   public virtual void UpdateFilter(Object obj)
   {
      if (obj != null)
      {
         IEnumerable<TreeNode> tns = this.GetTreeNodes(obj);
         if (tns != null)
         {
            foreach (TreeNode tn in tns)
            {
               IMaxNode wrapper = TreeMode.GetMaxNode(tn);
               tn.ShowNode = this.filters.ShowNode(wrapper);
            }
         }
      }
   }

   #endregion


   #region System notifications

   
   protected virtual void PausePreSystemEvent(IntPtr param, IntPtr info)
   {
      this.Stop();

      this.RegisterSystemNotification(this.proc_ResumePostSystemEvent, SystemNotificationCode.SystemPostNew);
      this.RegisterSystemNotification(this.proc_ResumePostSystemEvent, SystemNotificationCode.SystemPostReset);
      this.RegisterSystemNotification(this.proc_ResumePostSystemEvent, SystemNotificationCode.FilePostOpen);
      this.RegisterSystemNotification(this.proc_ResumePostSystemEvent, SystemNotificationCode.FilePostMerge);
   }

   
   protected virtual void ResumePostSystemEvent(IntPtr param, IntPtr info)
   {
      this.UnregisterSystemNotifications();
      
      this.Start();
   }


   
   protected virtual void SelectionSetChanged(IntPtr param, IntPtr info)
   {
      this.Tree.SelectAllNodes(false);

      Int32 selNodeCount = MaxInterfaces.COREInterface.SelNodeCount;
      if (selNodeCount > 0)
      {
         for (Int32 i = 0; i < selNodeCount; i++)
         {
            IINode inode = MaxInterfaces.COREInterface.GetSelNode(i);
            if (!inode.IsGroupMember || inode.IsOpenGroupMember)
            {
               IEnumerable<TreeNode> tns = this.GetTreeNodes(inode);
               if (tns != null)
                  tns.ForEach(tn => this.Tree.SelectNode(tn, true));
            }
         }
      }
   }

   protected void NodeRenamed(IntPtr param, IntPtr info)
   {
      Object callParam = MaxUtils.SystemNotifications.GetCallParam(info);
      Boolean sort = NodeSorter.RequiresSort(this.Tree.NodeSorter as NodeSorter, NodeProperty.Name);
      this.InvalidateObject(callParam, false, sort);
   }


   protected virtual void LayerPropertyChanged(IMaxNode layer, NodeProperty property)
   {
      Boolean sort = NodeSorter.RequiresSort(this.Tree.NodeSorter as NodeSorter, property);
      this.InvalidateObject(layer.BaseObject, false, sort);
      foreach (object child in layer.ChildBaseObjects)
      {
         this.InvalidateObject(child, false, sort);
      }
   }
   
   private void LayerHiddenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.SystemNotifications.GetCallParam(info) as IILayer;
      if (layer != null)
         this.LayerPropertyChanged(MaxNodeWrapper.Create(layer), NodeProperty.IsHidden);
   }
   
   private void LayerFrozenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.SystemNotifications.GetCallParam(info) as IILayer;
      if (layer != null)
         this.LayerPropertyChanged(MaxNodeWrapper.Create(layer), NodeProperty.IsFrozen);
   }

   private void LayerPropChanged(IntPtr param, IntPtr info)
   {
      LayerPropertyChangedParam? callParam = MaxUtils.SystemNotifications.GetCallParam(info) as LayerPropertyChangedParam?;
      if (callParam != null)
         this.LayerPropertyChanged(MaxNodeWrapper.Create(callParam.Value.Layer), callParam.Value.Property);
   }

   #endregion


   #region NodeEventCallbacks

   protected class DefaultNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      public DefaultNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in IINodes.NodeKeysToINodeList(nodes))
         {
            TreeNode parentTn = this.TreeMode.GetParentTreeNode(node);
            if (parentTn == null)
               continue;

            this.TreeMode.AddNode(node, parentTn.Nodes);
            this.Tree.AddToSortQueue(parentTn.Nodes);
         }
         this.Tree.StartTimedSort(true);
      }

      public override void Deleted(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
            this.TreeMode.RemoveNode(node);
      }

      public override void NameChanged(ITab<UIntPtr> nodes)
      {
         Boolean sort = NodeSorter.RequiresSort(this.NodeSorter, typeof(AlphabeticalSorter));
         this.TreeMode.InvalidateTreeNodes(nodes, sort);
      }

      public override void WireColorChanged(ITab<UIntPtr> nodes)
      {
         Boolean sort = NodeSorter.RequiresSort(this.NodeSorter, NodeProperty.WireColor);
         this.TreeMode.InvalidateTreeNodes(nodes, sort);
      }

      public override void DisplayPropertiesChanged(ITab<UIntPtr> nodes)
      {
         Boolean sort = NodeSorter.RequiresSort(this.NodeSorter, NodeProperties.DisplayProperties);
         this.TreeMode.InvalidateTreeNodes(nodes, sort);
      }

      public override void RenderPropertiesChanged(ITab<UIntPtr> nodes)
      {
         Boolean sort = NodeSorter.RequiresSort(this.NodeSorter, NodeProperties.RenderProperties);
         this.TreeMode.InvalidateTreeNodes(nodes, sort);
      }
   }

   #endregion


   #region Tree events

   protected virtual void tree_SelectionChanged(object sender, SelectionChangedEventArgs e)
   {
      this.UnregisterSystemNotification(proc_SelectionsetChanged, SystemNotificationCode.SelectionsetChanged);

      IEnumerable<IMaxNode> selNodes = TreeMode.GetMaxNodes(e.Nodes);
      SelectCommand cmd = new SelectCommand(selNodes);
      cmd.Execute();
      Viewports.Redraw();

      this.RegisterSystemNotification(proc_SelectionsetChanged, SystemNotificationCode.SelectionsetChanged);
   }

   protected virtual void tree_BeforeNodeTextEdit(object sender, BeforeNodeTextEditEventArgs e)
   {
      IMaxNode node = TreeMode.GetMaxNode(e.TreeNode);
      if (node == null)
      {
         e.Cancel = true;
         return;
      }

      if (!node.CanEditName)
      {
         WinForms::MessageBox.Show( Tree
                                  , OutlinerResources.Warning_CannotEditName
                                  , OutlinerResources.Warning_CannotEditNameTitle
                                  , WinForms::MessageBoxButtons.OK
                                  , WinForms::MessageBoxIcon.Warning);
         e.Cancel = true;
         return;
      }

      e.EditText = node.Name;

      MaxInterfaces.Global.DisableAccelerators();
   }

   protected virtual void tree_AfterNodeTextEdit(object sender, AfterNodeTextEditEventArgs e)
   {
      IMaxNode node = TreeMode.GetMaxNode(e.TreeNode);
      if (node == null)
         return;

      if (e.NewText != e.OldText)
      {
         RenameCommand cmd = new RenameCommand(new List<IMaxNode>(1) { node }, e.NewText);
         cmd.Execute(false);
      }

      //Note: setting treenode text to displayname and sorting are
      //      handled by nodenamechanged callback.

      MaxInterfaces.Global.EnableAccelerators();
   }


   public virtual ContextMenuModel ContextMenu { get; set; }


   void tree_MouseClick(object sender, WinForms.MouseEventArgs e)
   {
      if ((e.Button & WinForms.MouseButtons.Right) != WinForms.MouseButtons.Right)
         return;

      TreeNode clickedNode = this.Tree.GetNodeAt(e.Location);
      OutlinerSplitContainer container = this.Tree.Parent.Parent as OutlinerSplitContainer;

      WinForms::ContextMenuStrip contextMenu;
      if (this.ContextMenu != null)
         contextMenu = this.ContextMenu.ToContextMenuStrip(this.Tree, clickedNode);
      else
         contextMenu = new WinForms.ContextMenuStrip();

      WinForms::ToolStripDropDown strip = StandardContextMenu.Create(contextMenu, container, this.Tree, this); //this.CreateContextMenu(clickedNode), container, this.Tree, this);
      strip.Show(this.Tree, e.Location);
   }

   #endregion


   #region Filters

   /// <summary>
   /// Gets or sets the filters for this TreeMode.
   /// </summary>
   public MaxNodeFilterCombinator Filters
   {
      get { return this.filters.Filters[OtherFiltersIndex] as MaxNodeFilterCombinator; }
      set
      {
         Throw.IfNull(value, "value");

         this.filters.Filters[OtherFiltersIndex] = value;

         if (this.Started)
            this.EvaluateFilters();
      }
   }

   /// <summary>
   /// Adds a permanent filter to this TreeMode.
   /// </summary>
   /// <remarks>The filter will not be added to the Filters collection.</remarks>
   /// <param name="filter">The filter to add.</param>
   public void AddPermanentFilter(Filter<IMaxNode> filter)
   {
      Throw.IfNull(filter, "filter");
      this.filters.Filters.Add(filter);
   }

   void filters_FilterChanged(object sender, EventArgs e)
   {
      if (this.Started)
         this.EvaluateFilters();
   }

   /// <summary>
   /// Evaluates the filters and adds/removes treenodes based on it.
   /// </summary>
   protected void EvaluateFilters()
   {
      this.Tree.BeginUpdate();
      foreach (KeyValuePair<Object, ICollection<TreeNode>> item in this.treeNodes)
      {
         foreach (TreeNode tn in item.Value)
         {
            IMaxNode node = TreeMode.GetMaxNode(tn);
            if (node != null)
               tn.ShowNode = this.filters.ShowNode(node);
         }
      }
      this.Tree.Sort();
      this.Tree.EndUpdate();
   }

   #endregion
}
}
