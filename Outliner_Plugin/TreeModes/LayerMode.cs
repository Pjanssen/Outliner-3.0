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
using MaxUtils;
using Outliner.LayerTools;
using Outliner.NodeSorters;

namespace Outliner.TreeModes
{
public class LayerMode : TreeMode
{
   public LayerMode(TreeView tree, Autodesk.Max.IInterface ip)
      : base(tree, ip) 
   {
      this.RegisterSystemNotifications();
      this.RegisterNodeEventCallbacks();
   }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      foreach (IILayer layer in NestedLayers.RootLayers)
      {
         this.AddNode(layer, this.tree.Nodes);
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
      tn.DragDropHandler = new IILayerDragDropHandler(wrapper as IILayerWrapper);
      
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

      tn.DragDropHandler = new IINodeLayerDragDropHandler(wrapper);

      if (this.tree.TreeNodeLayout.UseLayerColors)
      {
         tn.BackColor = Color.FromArgb(170, ColorHelpers.FromMaxColor(wrapper.IILayer.WireColor));
      }

      return tn;
   }


   #region NodeEventCallbacks

   private void RegisterNodeEventCallbacks()
   {
      this.RegisterNodeEventCallbackObject(new LayerNodeEventCallbacks(this));
   }

   protected class LayerNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      public LayerNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in IINodeHelpers.NodeKeysToINodeList(nodes))
         {
            IILayer layer = node.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
            if (layer == null)
               continue;

            TreeNode layerTn = this.treeMode.GetFirstTreeNode(layer);
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
            TreeNode tn = this.treeMode.GetFirstTreeNode(node);
            if (tn == null)
               return;

            IILayer layer = node.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
            if (layer == null)
               continue;

            TreeNode layerTn = this.treeMode.GetFirstTreeNode(layer);
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

   private void RegisterSystemNotifications()
   {
      this.RegisterSystemNotification(this.LayerCreated, SystemNotificationCode.LayerCreated);
      this.RegisterSystemNotification(this.LayerDeleted, SystemNotificationCode.LayerDeleted);
      this.RegisterSystemNotification(this.LayerRenamed, SystemNotificationCode.LayerRenamed);
      this.RegisterSystemNotification(this.LayerHiddenChanged, SystemNotificationCode.LayerHiddenStateChanged);
      this.RegisterSystemNotification(this.LayerFrozenChanged, SystemNotificationCode.LayerFrozenStateChanged);
      this.RegisterSystemNotification(this.layerParented, NestedLayers.LayerParented);
      this.RegisterSystemNotification(this.LayerPropChanged, NestedLayers.LayerPropertyChanged);
   }

   public virtual void LayerCreated(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         this.AddNode(layer, this.tree.Nodes);
      }
   }

   public virtual void LayerDeleted(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
         this.RemoveNode(layer);
   }

   public virtual void LayerRenamed(IntPtr param, IntPtr info)
   {
      Console.WriteLine(MaxUtils.HelperMethods.GetCallParam(info));
   }

   protected virtual void LayerHiddenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         this.InvalidateObject(layer, true);
         if (tree.NodeSorter is AnimatablePropertySorter &&
          ((AnimatablePropertySorter)tree.NodeSorter).Property == AnimatableProperty.IsHidden)
         {
            this.tree.AddToSortQueue(this.GetTreeNodes(layer));
            this.tree.StartTimedSort(true);
         }
      }
   }

   protected virtual void LayerFrozenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         this.InvalidateObject(layer, true);
         if (tree.NodeSorter is AnimatablePropertySorter &&
          ((AnimatablePropertySorter)tree.NodeSorter).Property == AnimatableProperty.IsFrozen)
         {
            this.tree.AddToSortQueue(this.GetTreeNodes(layer));
            this.tree.StartTimedSort(true);
         }
      }
   }

   public virtual void LayerPropChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         this.InvalidateObject(layer, true);
      }
   }

   public virtual void layerParented(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         TreeNode tn = this.GetFirstTreeNode(layer);
         if (tn != null)
         {
            tn.Remove();

            TreeNodeCollection newParentCol = this.tree.Nodes;
            TreeNode newParentTn = this.GetFirstTreeNode(NestedLayers.GetParent(layer));
            if (newParentTn != null)
               newParentCol = newParentTn.Nodes;

            if (newParentCol != null)
            {
               newParentCol.Add(tn);
               this.tree.AddToSortQueue(newParentCol);
            }
         }
      }
   }

   #endregion
}
}
