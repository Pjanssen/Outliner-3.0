using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Filters;
using System.Drawing;
using Outliner.Controls;
using Outliner.Controls.Tree.DragDropHandlers;

namespace Outliner.TreeModes
{
public class LayerMode : TreeMode
{
   public LayerMode(TreeView tree, Autodesk.Max.IInterface ip)
      : base(tree, ip) { }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IILayerManager lm = MaxInterfaces.IILayerManager;
      for (int i = 0; i < lm.LayerCount; i++)
      {
         IILayer l = lm.GetLayer(i);
         this.AddNode(l, this.tree.Nodes);
      }

      this.tree.Sort();
      this.tree.EndUpdate();
   }

   public override TreeNode AddNode(object node, TreeNodeCollection parentCol)
   {
      if (node is IILayer)
         return this.addNode((IILayer)node, parentCol);
      else if (node is IINode)
         return this.addNode((IINode)node, parentCol);
      else
         return null;
   }

   private TreeNode addNode(IILayer layer, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(layer, parentCol);
      IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);
      //TODO tn.DragDropHandler = new IILayerDragDropHandler(wrapper);

      if (this.tree.TreeNodeLayout.UseLayerColors)
      {
         tn.BackColor = Color.FromArgb(255, wrapper.WireColor);
      }

      //Add nodes belonging to this layer.
      foreach (Object node in wrapper.ChildNodes)
         this.AddNode(node, tn.Nodes);

      return tn;
   }

   private TreeNode addNode(IINode inode, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(inode, parentCol);
      IINodeWrapper wrapper = HelperMethods.GetMaxNode(tn) as IINodeWrapper;
      if (wrapper == null)
         return tn;

      tn.DragDropHandler = new IINodeDragDropHandler(wrapper);

      if (this.tree.TreeNodeLayout.UseLayerColors)
      {
         tn.BackColor = Color.FromArgb(170, ColorHelpers.FromMaxColor(wrapper.Layer.WireColor));
      }

      return tn;
   }


   #region NodeEventCallbacks

   public override void RegisterNodeEventCallbacks()
   {
      this.RegisterNodeEventCallbackObject(new LayerNodeEventCallbacks(this));

      base.RegisterNodeEventCallbacks();
   }

   protected class LayerNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      public LayerNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in HelperMethods.NodeKeysToINodeList(nodes))
         {
            IILayer layer = node.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
            if (layer == null)
               continue;

            TreeNode layerTn = this.treeMode.GetTreeNode(layer);
            if (layerTn == null)
               continue;

            this.treeMode.AddNode(node, layerTn.Nodes);
            this.tree.AddToSortQueue(layerTn.Nodes);
         }
         this.tree.StartTimedSort(true);
      }

      public override void LayerChanged(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.treeMode.GetTreeNode(node);
            if (tn == null)
               return;

            IILayer layer = node.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
            if (layer == null)
               continue;

            TreeNode layerTn = this.treeMode.GetTreeNode(layer);
            if (layerTn == null)
               continue;

            layerTn.Nodes.Add(tn);
            this.tree.AddToSortQueue(layerTn.Nodes);

            if (this.tree.TreeNodeLayout.UseLayerColors)
               tn.BackColor = Color.FromArgb(170, ColorHelpers.FromMaxColor(layer.WireColor));
         }
         this.tree.StartTimedSort(true);
      }
   }

   #endregion


   #region System notifications

   public override void RegisterSystemNotifications()
   {
      this.RegisterSystemNotification(this.LayerCreated, SystemNotificationCode.LayerCreated);
      this.RegisterSystemNotification(this.LayerDeleted, SystemNotificationCode.LayerDeleted);
      this.RegisterSystemNotification(this.LayerRenamed, SystemNotificationCode.LayerRenamed);
      this.RegisterSystemNotification(this.LayerPropChanged, SystemNotificationCode.LayerHiddenStateChanged);
      this.RegisterSystemNotification(this.LayerPropChanged, SystemNotificationCode.LayerFrozenStateChanged);

      base.RegisterSystemNotifications();
   }

   public virtual void LayerCreated(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null && notifyInfo.CallParam is IILayer)
      {
         IILayer layer = (IILayer)notifyInfo.CallParam;
         this.AddNode(layer, this.tree.Nodes);
      }
   }

   public virtual void LayerDeleted(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null && notifyInfo.CallParam is IILayer)
         this.RemoveTreeNode(notifyInfo.CallParam);
   }

   public virtual void LayerRenamed(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      Console.WriteLine(notifyInfo.CallParam);
   }

   public virtual void LayerPropChanged(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null && notifyInfo.CallParam is IILayer)
      {
         TreeNode tn = this.GetTreeNode(notifyInfo.CallParam);
         if (tn != null)
            this.tree.Invalidate(tn.Bounds);
      }
   }

   #endregion
}
}
