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
public class LayerMode : TreeMode
{
   public Boolean ShowGroupContents { get; set; }

   public LayerMode(TreeView tree) : base(tree) 
   {
      this.ShowGroupContents = OutlinerGUP.Instance.Settings.GetValue<Boolean>("LayerMode", "ShowGroupContents", true);

      proc_LayerCreated = new GlobalDelegates.Delegate5(this.LayerCreated);
      proc_LayerDeleted = new GlobalDelegates.Delegate5(this.LayerDeleted);
      proc_LayerRenamed = new GlobalDelegates.Delegate5(this.LayerRenamed);
      proc_LayerParented = new GlobalDelegates.Delegate5(this.LayerParented);
      proc_LayerCurrentChanged = new GlobalDelegates.Delegate5(this.LayerCurrentChanged);
   }


   public override void Start()
   {
      this.RegisterSystemNotification(proc_LayerCreated, SystemNotificationCode.LayerCreated);
      this.RegisterSystemNotification(proc_LayerDeleted, SystemNotificationCode.LayerDeleted);
      this.RegisterSystemNotification(proc_LayerRenamed, SystemNotificationCode.LayerRenamed);
      this.RegisterSystemNotification(proc_LayerParented, LayerNotificationCode.LayerParented);
      this.RegisterSystemNotification(proc_LayerCurrentChanged, LayerTools.LayerTools.LayerCurrentChanged);

      this.RegisterNodeEventCallbackObject(new LayerNodeEventCallbacks(this));

      base.Start();
   }


   #region FillTree, AddNode
   
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

   public override TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);

      ILayerWrapper layerWrapper = wrapper as ILayerWrapper;
      if (layerWrapper != null)
      {
         //Set italic font for default layer.
         if (layerWrapper.IsDefault)
            tn.FontStyle = FontStyle.Italic;

         //Add nodes belonging to this layer.
         foreach (Object node in wrapper.ChildBaseObjects)
         {
            if (this.ShouldAddNode(node))
               this.AddNode(node, tn.Nodes);
         }
      }

      return tn;
   }

   protected override bool ShouldAddNode(Object obj)
   {
      if (obj is IILayer)
         return true;

      IINode inode = obj as IINode;
      if (inode == null)
         return false;

      if (!ShowGroupContents)
         return !inode.IsGroupMember;

      return true;
   }

   protected override TreeNode GetParentTreeNode(IINode node)
   {
      IILayer layer = node.GetReference((int)ReferenceNumbers.NodeLayerRef) as IILayer;
      if (layer == null)
         return null;

      return this.GetFirstTreeNode(layer);
   }

   public override IDragDropHandler CreateDragDropHandler(IMaxNode node)
   {
      if (node is ILayerWrapper)
         return new ILayerDragDropHandler((ILayerWrapper)node);
      else if (node is INodeWrapper)
         return new INodeDragDropHandler(node);
      else
         return base.CreateDragDropHandler(node);
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
               this.Tree.StartTimedSort(newParentCol);
            }
         }
      }
   }

   protected GlobalDelegates.Delegate5 proc_LayerCurrentChanged;
   protected virtual void LayerCurrentChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = MaxUtils.HelperMethods.GetCallParam(info) as IILayer;
      if (layer != null)
         this.InvalidateObject(layer, false, false);
   }

   #endregion


   #region NodeEventCallbacks

   protected class LayerNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      private LayerMode layerMode;
      public LayerNodeEventCallbacks(LayerMode treeMode) : base(treeMode) 
      {
         this.layerMode = treeMode;
      }

      public override void LayerChanged(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.TreeMode.GetFirstTreeNode(node);
            if (tn == null)
               return;

            TreeNode layerTn = this.layerMode.GetParentTreeNode(node);
            if (layerTn == null)
               continue;

            layerTn.Nodes.Add(tn);
            this.Tree.AddToSortQueue(layerTn.Nodes);
         }
         this.Tree.StartTimedSort(true);
      }

      public override void GroupChanged(ITab<UIntPtr> nodes)
      {
         if (this.layerMode.ShowGroupContents)
            return;

         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            if (node.IsGroupMember)
            {
               this.layerMode.RemoveNode(node);
            }
            else
            {
               TreeNode layerTn = this.layerMode.GetParentTreeNode(node);
               if (layerTn == null)
                  continue;

               this.layerMode.AddNode(node, layerTn.Nodes);
               this.Tree.AddToSortQueue(layerTn.Nodes);
            }
         }

         this.Tree.StartTimedSort(true);
      }
   }

   #endregion
}
}
