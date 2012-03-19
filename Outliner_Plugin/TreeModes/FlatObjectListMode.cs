using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.TreeModes
{
public class FlatObjectListMode : TreeMode
{
   public FlatObjectListMode(Outliner.Controls.TreeView tree, Autodesk.Max.IInterface ip) 
      : base(tree, ip) 
   {
      this.Filters = new FlatListFilterCollection<IMaxNodeWrapper>(base.Filters);
   }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      IINode rootNode = this.ip.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
         addNode(rootNode.GetChildNode(i));

      this.tree.EndUpdate();
      this.tree.TimedSort(false);
   }

   protected void addNode(IINode node)
   {
      this.addNode(node, this.tree.Nodes, true);
   }
   protected void addNode(IINode node, TreeNodeCollection parentCol, Boolean addChildren)
   {
      if (HelperMethods.IsHiddenNode(node))
         return;

      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(node);
      FilterResult filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(node))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
         tn.FilterResult = filterResult;

         this.nodes.Add(node, tn);
         parentCol.Add(tn);

         if (node.Selected)
            this.tree.SelectNode(tn, true);
      }

      if (addChildren)
      {
         for (int i = 0; i < node.NumberOfChildren; i++)
            addNode(node.GetChildNode(i), parentCol, addChildren);
      }
   }


   public override void Added(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
         this.addNode(node, this.tree.Nodes, false);
      this.tree.TimedSort(false);
   }
}
}
