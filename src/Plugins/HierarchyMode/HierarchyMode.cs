﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Modes.Hierarchy
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_Hierarchy")]
public class HierarchyMode : TreeMode
{
   public HierarchyMode(TreeView tree) : base(tree)
   {
      Throw.IfNull(tree, "tree");

      tree.DragDropHandler = new TreeViewDragDropHandler();
   }


   public override void Start()
   {
      this.RegisterNodeEventCallbackObject(new HierarchyNodeEventCallbacks(this));

      base.Start();
   }


   #region FillTree, AddNode
   
   protected override void FillTree()
   {
      this.Tree.BeginUpdate();

      IINode rootNode = MaxInterfaces.COREInterface.RootNode;
      this.RegisterNode(rootNode, this.Tree.Root);

      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         AddNode(rootNode.GetChildNode(i), this.Tree.Nodes);

      this.Tree.Sort();
      this.Tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol)
   {
      return this.AddNode(wrapper, parentCol, true);
   }

   public TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol, Boolean recursive)
   {
      Throw.IfNull(wrapper, "wrapper");
      Throw.IfNull(parentCol, "parentCol");

      TreeNode tn = base.AddNode(wrapper, parentCol);

      if (recursive)
      {
         foreach (Object child in wrapper.ChildBaseObjects)
         {
            this.AddNode(child, tn.Nodes);
         }
      }

      return tn;
   }


   protected override IDragDropHandler CreateDragDropHandler(IMaxNode node)
   {
      INodeWrapper iinodeWrapper = node as INodeWrapper;
      if (iinodeWrapper != null)
      {
         if (iinodeWrapper.INode.IsGroupMember || iinodeWrapper.INode.IsGroupHead)
            return new GroupDragDropHandler(node);
         else
            return new INodeDragDropHandler(node);
      }

      return base.CreateDragDropHandler(node);
   }

   protected override TreeNode GetParentTreeNode(IINode node)
   {
      if (node == null)
         return null;

      return this.GetFirstTreeNode(node.ParentNode);
   }

   #endregion


   #region NodeEventCallback
   
   protected class HierarchyNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      private HierarchyMode hierarchyMode;
      public HierarchyNodeEventCallbacks(HierarchyMode treeMode) : base(treeMode) 
      {
         this.hierarchyMode = treeMode;
      }

      public override void LinkChanged(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.TreeMode.GetFirstTreeNode(node);
            if (tn != null)
            {
               TreeNodeCollection newParentCol = null;
               if (node.ParentNode == null || node.ParentNode.IsRootNode)
                  newParentCol = this.Tree.Nodes;
               else
               {
                  TreeNode newParentTn = this.TreeMode.GetFirstTreeNode(node.ParentNode);
                  if (newParentTn != null)
                     newParentCol = newParentTn.Nodes;
                  //TODO add logic for filtered / not yet added node.
               }

               if (newParentCol != null)
               {
                  newParentCol.Add(tn);
                  this.Tree.AddToSortQueue(newParentCol);
               }
            }
         }
         this.Tree.StartTimedSort(true);
      }

      public override void ModelStructured(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.TreeMode.GetFirstTreeNode(node);
            if (tn != null)
               tn.Invalidate();
         }
      }
   }

   #endregion
}
}
