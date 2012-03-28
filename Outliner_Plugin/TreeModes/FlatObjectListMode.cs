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
         this.addNode(IMaxNodeWrapper.Create(rootNode.GetChildNode(i)), this.tree.Nodes);

      this.tree.Sort();

      this.tree.EndUpdate();
   }

   protected override TreeNode addNode(IMaxNodeWrapper node, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.addNode(node, parentCol);
      if (tn != null)
      {
         foreach (IMaxNodeWrapper child in node.ChildNodes)
            this.addNode(child, parentCol);
      }

      return tn;
   }
   

   public override void Added(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
         this.addNode(IMaxNodeWrapper.Create(node), this.tree.Nodes);
      this.tree.TimedSort(false);
   }
}
}
