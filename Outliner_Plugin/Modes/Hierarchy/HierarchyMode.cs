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

namespace Outliner.Modes.Hierarchy
{
public class HierarchyMode : TreeMode
{
   public HierarchyMode(TreeView tree) : base(tree)
   {
      ExceptionHelper.ThrowIfArgumentIsNull(tree, "tree");

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

      foreach (Object child in wrapper.ChildNodes)
      {
         this.AddNode(child, tn.Nodes);
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
      public HierarchyNodeEventCallbacks(TreeMode treeMode) : base(treeMode) { }

      public override void Added(ITab<UIntPtr> nodes)
      {
         foreach (IINode node in nodes.NodeKeysToINodeList())
         {
            TreeNodeCollection parentCol = null;
            if (node.ParentNode != null && !node.ParentNode.IsRootNode)
            {
               TreeNode parentTn = this.treeMode.GetFirstTreeNode(node);
               if (parentTn != null)
                  parentCol = parentTn.Nodes;
            }
            else
               parentCol = this.tree.Nodes;

            if (parentCol != null)
            {
               this.treeMode.AddNode(node, parentCol);
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
