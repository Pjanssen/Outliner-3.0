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
using MaxUtils;
using Outliner.LayerTools;
using Outliner.NodeSorters;

namespace Outliner.Modes.Layer
{
public class LayerMode : TreeMode
{
   public Boolean LayersOnly { get; set; }

   public LayerMode(TreeView tree) : base(tree) 
   {
      this.LayersOnly = false;
   }

   protected override void FillTree()
   {
      this.Tree.BeginUpdate();

      foreach (IILayer layer in NestedLayers.RootLayers)
      {
         this.AddNode(layer, this.Tree.Nodes);
      }

      this.Tree.Sort();
      this.Tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);

      IILayerWrapper layerWrapper = wrapper as IILayerWrapper;
      if (layerWrapper != null)
      {
         //Set italic font for default layer.
         if (layerWrapper.IsDefault)
            tn.FontStyle = FontStyle.Italic;

         //Add nodes belonging to this layer.
         foreach (Object node in wrapper.ChildNodes)
         {
            if (!this.LayersOnly || node is IILayer)
               this.AddNode(node, tn.Nodes);
         }

         tn.DragDropHandler = new IILayerDragDropHandler(layerWrapper);
      }
      else if (wrapper is IINodeWrapper)
      {
         tn.DragDropHandler = new IINodeDragDropHandler(wrapper);
      }

      return tn;
   }


   public override void Start()
   {
      this.RegisterSystemNotification(this.LayerCreated, SystemNotificationCode.LayerCreated);
      this.RegisterSystemNotification(this.LayerDeleted, SystemNotificationCode.LayerDeleted);
      this.RegisterSystemNotification(this.LayerRenamed, SystemNotificationCode.LayerRenamed);
      this.RegisterSystemNotification(this.LayerHiddenChanged, SystemNotificationCode.LayerHiddenStateChanged);
      this.RegisterSystemNotification(this.LayerFrozenChanged, SystemNotificationCode.LayerFrozenStateChanged);
      this.RegisterSystemNotification(this.LayerParented, NestedLayers.LayerParented);
      this.RegisterSystemNotification(this.LayerPropChanged, NestedLayers.LayerPropertyChanged);

      this.RegisterNodeEventCallbackObject(new LayerNodeEventCallbacks(this));

      base.Start();
   }


   #region NodeEventCallbacks

   protected class LayerNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      public LayerNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void Added(ITab<UIntPtr> nodes)
      {
         LayerMode layerMode = this.treeMode as LayerMode;
         if (layerMode == null || layerMode.LayersOnly)
            return;

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
         }
         this.tree.StartTimedSort(true);
      }
   }

   #endregion


   #region System notifications

   protected virtual void LayerCreated(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
         this.AddNode(layer, this.Tree.Nodes);
   }

   protected virtual void LayerDeleted(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
         this.RemoveNode(layer);
   }

   protected virtual void LayerRenamed(IntPtr param, IntPtr info)
   {
      Console.WriteLine(MaxUtils.HelperMethods.GetCallParam(info));
   }

   protected virtual void LayerHiddenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         AnimatablePropertySorter sorter = Tree.NodeSorter as AnimatablePropertySorter;
         Boolean sort = sorter != null && sorter.Property == AnimatableProperty.IsHidden;
         this.InvalidateObject(layer, true, sort);
      }
   }

   protected virtual void LayerFrozenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         AnimatablePropertySorter sorter = Tree.NodeSorter as AnimatablePropertySorter;
         Boolean sort = sorter != null && sorter.Property == AnimatableProperty.IsFrozen;
         this.InvalidateObject(layer, true, sort);
      }
   }

   protected virtual void LayerPropChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         //TODO: check which properties should sort.
         this.InvalidateObject(layer, true, false);
      }
   }

   protected virtual void LayerParented(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         TreeNode tn = this.GetFirstTreeNode(layer);
         if (tn != null)
         {
            tn.Remove();

            TreeNodeCollection newParentCol = this.Tree.Nodes;
            TreeNode newParentTn = this.GetFirstTreeNode(NestedLayers.GetParent(layer));
            if (newParentTn != null)
               newParentCol = newParentTn.Nodes;

            if (newParentCol != null)
            {
               newParentCol.Add(tn);
               this.Tree.AddToSortQueue(newParentCol);
            }
         }
      }
   }

   #endregion
}
}
