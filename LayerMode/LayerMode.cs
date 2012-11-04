using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Filters;
using Outliner.Controls;
using Outliner.MaxUtils;
using Outliner.LayerTools;
using Outliner.NodeSorters;
using Outliner.Plugins;

namespace Outliner.Modes.Layer
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_DisplayName")]
[LocalizedDisplayImage(typeof(Resources), "layer_mode_16", "layer_mode_24")]
public class LayerMode : TreeMode
{
   public Boolean LayersOnly { get; set; }

   public LayerMode(TreeView tree) : base(tree) 
   {
      this.LayersOnly = false;

      proc_LayerCreated = new GlobalDelegates.Delegate5(this.LayerCreated);
      proc_LayerDeleted = new GlobalDelegates.Delegate5(this.LayerDeleted);
      proc_LayerRenamed = new GlobalDelegates.Delegate5(this.LayerRenamed);
      proc_LayerHiddenChanged = new GlobalDelegates.Delegate5(this.LayerHiddenChanged);
      proc_LayerFrozenChanged = new GlobalDelegates.Delegate5(this.LayerFrozenChanged);
      proc_LayerParented = new GlobalDelegates.Delegate5(this.LayerParented);
      proc_LayerPropChanged = new GlobalDelegates.Delegate5(this.LayerPropChanged);
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
      }

      return tn;
   }

   public override DragDropHandler CreateDragDropHandler(IMaxNodeWrapper node)
   {
      if (node is IILayerWrapper)
         return new IILayerDragDropHandler((IILayerWrapper)node);
      else if (node is IINodeWrapper)
         return new IINodeDragDropHandler(node);
      else
         return base.CreateDragDropHandler(node);
   }


   public override void Start()
   {
      this.RegisterSystemNotification(proc_LayerCreated, SystemNotificationCode.LayerCreated);
      this.RegisterSystemNotification(proc_LayerDeleted, SystemNotificationCode.LayerDeleted);
      this.RegisterSystemNotification(proc_LayerRenamed, SystemNotificationCode.LayerRenamed);
      this.RegisterSystemNotification(proc_LayerHiddenChanged, SystemNotificationCode.LayerHiddenStateChanged);
      this.RegisterSystemNotification(proc_LayerFrozenChanged, SystemNotificationCode.LayerFrozenStateChanged);
      this.RegisterSystemNotification(proc_LayerParented, NestedLayers.LayerParented);
      this.RegisterSystemNotification(proc_LayerPropChanged, NestedLayers.LayerPropertyChanged);

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

   protected GlobalDelegates.Delegate5 proc_LayerCreated;
   protected virtual void LayerCreated(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
         this.AddNode(layer, this.Tree.Nodes);
   }

   protected GlobalDelegates.Delegate5 proc_LayerDeleted;
   protected virtual void LayerDeleted(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
         this.RemoveNode(layer);
   }

   protected GlobalDelegates.Delegate5 proc_LayerRenamed;
   protected virtual void LayerRenamed(IntPtr param, IntPtr info)
   {
      Console.WriteLine(MaxUtils.HelperMethods.GetCallParam(info));
   }

   protected GlobalDelegates.Delegate5 proc_LayerHiddenChanged;
   protected virtual void LayerHiddenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         NodePropertySorter sorter = Tree.NodeSorter as NodePropertySorter;
         Boolean sort = sorter != null && sorter.Property == NodeProperty.IsHidden;
         this.InvalidateObject(layer, true, sort);
      }
   }

   protected GlobalDelegates.Delegate5 proc_LayerFrozenChanged;
   protected virtual void LayerFrozenChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         NodePropertySorter sorter = Tree.NodeSorter as NodePropertySorter;
         Boolean sort = sorter != null && sorter.Property == NodeProperty.IsFrozen;
         this.InvalidateObject(layer, true, sort);
      }
   }

   protected GlobalDelegates.Delegate5 proc_LayerPropChanged;
   protected virtual void LayerPropChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
      {
         //TODO: check which properties should sort.
         this.InvalidateObject(layer, true, false);
      }
   }

   protected GlobalDelegates.Delegate5 proc_LayerParented;
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
