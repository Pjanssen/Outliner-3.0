using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Filters;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner.Modes.Hierarchy
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_Hierarchy")]
public class HierarchyMode : TreeMode
{
   public HierarchyMode(TreeView tree) : base(tree)
   {
      Throw.IfArgumentIsNull(tree, "tree");

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
      Throw.IfArgumentIsNull(wrapper, "wrapper");
      Throw.IfArgumentIsNull(parentCol, "parentCol");

      TreeNode tn = base.AddNode(wrapper, parentCol);

      if (recursive)
      {
         foreach (Object child in wrapper.ChildNodes)
         {
            this.AddNode(child, tn.Nodes);
         }
      }

      return tn;
   }


   public override DragDropHandler CreateDragDropHandler(IMaxNodeWrapper node)
   {
      IINodeWrapper iinodeWrapper = node as IINodeWrapper;
      if (iinodeWrapper != null)
      {
         if (iinodeWrapper.IINode.IsGroupMember || iinodeWrapper.IINode.IsGroupHead)
            return new GroupDragDropHandler(node);
         else
            return new IINodeDragDropHandler(node);
      }

      return base.CreateDragDropHandler(node);
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
            TreeNode parentTn = this.TreeMode.GetFirstTreeNode(node.ParentNode);
            if (parentTn != null)
               parentCol = parentTn.Nodes;

            if (parentCol != null)
            {
               this.hierarchyMode.AddNode(IMaxNodeWrapper.Create(node), parentCol, false);
               this.Tree.AddToSortQueue(parentCol);
            }
         }
         this.Tree.StartTimedSort(true);
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
}
}
