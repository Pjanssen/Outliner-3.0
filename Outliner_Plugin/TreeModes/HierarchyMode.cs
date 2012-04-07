using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Filters;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.DragDropHandlers;

namespace Outliner.TreeModes
{
public class HierarchyMode : TreeMode
{
   public HierarchyMode(TreeView tree, IInterface ip)
      : base(tree, ip)
   {
      tree.DragDropHandler = new TreeHierarchyDragDropHandler();
   }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IINode rootNode = this.ip.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         addNode(rootNode.GetChildNode(i), this.tree.Nodes);

      this.tree.Sort();
      this.tree.EndUpdate();
   }

   private void addNode(IINode node, TreeNodeCollection parentCol)
   {
      this.addNode(node, parentCol, true);
   }
   private void addNode(IINode node, TreeNodeCollection parentCol, Boolean addChildren)
   {
      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(node);
      FilterResults filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResults.Hide && !this.treeNodes.ContainsKey(node))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
         tn.FilterResult = filterResult;
         tn.DragDropHandler = new Outliner.Controls.Tree.DragDropHandlers.IINodeDragDropHandler(wrapper);

         this.treeNodes.Add(node, tn);

         if (addChildren)
         {
            for (int i = 0; i < node.NumberOfChildren; i++)
               addNode(node.GetChildNode(i), tn.Nodes, addChildren);
         }

         parentCol.Add(tn);

         if (node.Selected)
            this.tree.SelectNode(tn, true);
      }
   }

   public override void Added(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         TreeNodeCollection parentCol = null;
         if (node.ParentNode != null && !node.ParentNode.IsRootNode)
         {
            TreeNode parentTn = null;
            if (this.treeNodes.TryGetValue(node, out parentTn))
               parentCol = parentTn.Nodes;
         }
         else
            parentCol = this.tree.Nodes;

         if (parentCol != null)
            this.addNode(node, parentCol, true);
      }
      this.tree.StartTimedSort(false);
   }

   public override void LinkChanged(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         TreeNode tn = this.GetTreeNode(node);
         if (tn != null)
         {
            TreeNodeCollection newParentCol = null;
            if (node.ParentNode == null || node.ParentNode.IsRootNode)
               newParentCol = this.tree.Nodes;
            else
            {
               TreeNode newParentTn = this.GetTreeNode(node.ParentNode);
               if (newParentTn != null)
                  newParentCol = newParentTn.Nodes;
               //TODO add logic for filtered / not yet added node.
            }

            if (newParentCol != null)
            {
               newParentCol.Add(tn);
               this.tree.AddToSortQueue(newParentCol);
               this.tree.StartTimedSort(true);
            }
         }
      }
   }
}
}
