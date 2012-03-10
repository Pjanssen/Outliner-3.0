using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.FiltersBase;

namespace Outliner.TreeModes
{
public class HierarchyMode : TreeMode
{
   public HierarchyMode(Outliner.Controls.TreeView tree, IInterface ip)
      : base(tree, ip)
   { }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IINode rootNode = this.ip.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         addNode(rootNode.GetChildNode(i), this.tree.Nodes);

      this.tree.EndUpdate();
      this.tree.TimedSort(false);
   }

   private void addNode(IINode node, TreeNodeCollection parentCol)
   {
      this.addNode(node, parentCol, true);
   }
   private void addNode(IINode node, TreeNodeCollection parentCol, Boolean addChildren)
   {
      if (HelperMethods.IsPFHelper(node))
         return;

      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(node);
      FilterResult filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(node))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
         ((OutlinerTreeNodeData)tn.Tag).FilterResult = filterResult;

         this.nodes.Add(node, tn);

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
            if (this.nodes.TryGetValue(node, out parentTn))
               parentCol = parentTn.Nodes;
         }
         else
            parentCol = this.tree.Nodes;

         if (parentCol != null)
            this.addNode(node, parentCol, true);
      }
      this.tree.TimedSort(false);
   }
}
}
