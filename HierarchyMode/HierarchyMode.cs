using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Filters;
using Outliner.Controls.Tree;
using MaxUtils;
using Outliner.Plugins;

namespace Outliner.Modes.Hierarchy
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_DisplayName")]
[LocalizedDisplayImage(typeof(Resources), "hierarchy_mode_16", "hierarchy_mode_24")]
public class HierarchyMode : TreeMode
{
   public HierarchyMode(TreeView tree)
      : base(tree)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(tree, "tree");

      tree.DragDropHandler = new TreeViewDragDropHandler();
   }

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

   public override TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol)
   {
      return this.AddNode(wrapper, parentCol, true);
   }

   public TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol, Boolean recursive)
   {
      if (wrapper == null)
         throw new ArgumentNullException("wrapper");
      if (parentCol == null)
         throw new ArgumentNullException("parentCol");

      TreeNode tn = base.AddNode(wrapper, parentCol);

      IINodeWrapper iinodeWrapper = wrapper as IINodeWrapper;
      if (iinodeWrapper != null)
      {
         if (iinodeWrapper.IINode.IsGroupMember || iinodeWrapper.IINode.IsGroupHead)
            tn.DragDropHandler = new GroupDragDropHandler(wrapper);
         else
            tn.DragDropHandler = new IINodeDragDropHandler(wrapper);
      }

      if (recursive)
      {
         foreach (Object child in wrapper.ChildNodes)
         {
            this.AddNode(child, tn.Nodes);
         }
      }

      return tn;
   }


   public override void Start()
   {
      this.RegisterNodeEventCallbackObject(new HierarchyNodeEventCallbacks(this));
      base.Start();
   }


   protected class HierarchyNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      private HierarchyMode hierarchyMode;
      public HierarchyNodeEventCallbacks(HierarchyMode treeMode) : base(treeMode) 
      {
         this.hierarchyMode = treeMode;
      }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNodeCollection parentCol = null;
            TreeNode parentTn = this.treeMode.GetFirstTreeNode(node.ParentNode);
            if (parentTn != null)
               parentCol = parentTn.Nodes;

            if (parentCol != null)
            {
               this.hierarchyMode.AddNode(IMaxNodeWrapper.Create(node), parentCol, false);
               this.tree.AddToSortQueue(parentCol);
            }
         }
         this.tree.StartTimedSort(true);
      }

      public override void LinkChanged(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.treeMode.GetFirstTreeNode(node);
            if (tn != null)
            {
               TreeNodeCollection newParentCol = null;
               if (node.ParentNode == null || node.ParentNode.IsRootNode)
                  newParentCol = this.tree.Nodes;
               else
               {
                  TreeNode newParentTn = this.treeMode.GetFirstTreeNode(node.ParentNode);
                  if (newParentTn != null)
                     newParentCol = newParentTn.Nodes;
                  //TODO add logic for filtered / not yet added node.
               }

               if (newParentCol != null)
               {
                  newParentCol.Add(tn);
                  this.tree.AddToSortQueue(newParentCol);
               }
            }
         }
         this.tree.StartTimedSort(true);
      }

      public override void ModelStructured(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNode tn = this.treeMode.GetFirstTreeNode(node);
            if (tn != null)
               tn.Invalidate();
         }
      }
   }
}
}
