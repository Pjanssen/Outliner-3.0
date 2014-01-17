using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.LayerTools;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Modes.Layer
{
/// <summary>
/// Defines a TreeMode which lists all layers and their objects in the scene.
/// </summary>
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_DisplayName")]
public class LayerMode : TreeMode
{
   private bool showGroupContents;

   protected GlobalDelegates.Delegate5 proc_LayerCreated;
   protected GlobalDelegates.Delegate5 proc_LayerDeleted;
   protected GlobalDelegates.Delegate5 proc_LayerRenamed;
   protected GlobalDelegates.Delegate5 proc_LayerParented;
   protected GlobalDelegates.Delegate5 proc_LayerCurrentChanged;

   /// <summary>
   /// Initializes a new instance of the LayerMode class.
   /// </summary>
   /// <param name="tree">The TreeView control to fill.</param>
   public LayerMode(TreeView tree) : base(tree) 
   {
      LayerModeConfigurationSection configuration = OutlinerGUP.Instance.Configuration.GetSection<LayerModeConfigurationSection>("LayerMode");
      this.showGroupContents = configuration.ShowGroupContents;

      proc_LayerRenamed        = new GlobalDelegates.Delegate5(this.LayerRenamed);
      proc_LayerCreated        = new GlobalDelegates.Delegate5(this.LayerCreated);
      proc_LayerDeleted        = new GlobalDelegates.Delegate5(this.LayerDeleted);
      proc_LayerParented       = new GlobalDelegates.Delegate5(this.LayerParented);
      proc_LayerCurrentChanged = new GlobalDelegates.Delegate5(this.LayerCurrentChanged);

      this.Tree.DragDropHandler = new TreeViewDragDropHandler();
   }


   public override void Start()
   {
      this.RegisterSystemNotification(proc_LayerCreated, SystemNotificationCode.LayerCreated);
      this.RegisterSystemNotification(proc_LayerDeleted, SystemNotificationCode.LayerDeleted);
      this.RegisterSystemNotification(proc_LayerRenamed, SystemNotificationCode.LayerRenamed);
      this.RegisterSystemNotification(proc_LayerParented, LayerNotificationCode.LayerParented);
      this.RegisterSystemNotification(proc_LayerCurrentChanged, LayerNotificationCode.LayerCurrentChanged);

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

      if (!this.showGroupContents)
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

   protected override IDragDropHandler CreateDragDropHandler(IMaxNode node)
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

   protected virtual void LayerCreated(IntPtr param, IntPtr info)
   {
      IILayer layer = SystemNotifications.GetCallParam(info) as IILayer;
      if (layer != null)
         this.AddNode(layer, this.Tree.Nodes);
   }

   protected virtual void LayerDeleted(IntPtr param, IntPtr info)
   {
      IILayer layer = SystemNotifications.GetCallParam(info) as IILayer;
      if (layer != null)
         this.RemoveNode(layer);
   }

   protected virtual void LayerRenamed(IntPtr param, IntPtr info)
   {
      Console.WriteLine(SystemNotifications.GetCallParam(info));
   }

   protected virtual void LayerParented(IntPtr param, IntPtr info)
   {
      IILayer layer = SystemNotifications.GetCallParam(info) as IILayer;
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

   protected virtual void LayerCurrentChanged(IntPtr param, IntPtr info)
   {
      IILayer layer = SystemNotifications.GetCallParam(info) as IILayer;
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
         if (this.layerMode.showGroupContents)
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
