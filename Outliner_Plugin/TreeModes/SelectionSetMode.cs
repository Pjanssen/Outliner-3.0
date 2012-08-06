using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Filters;
using MaxUtils;

namespace Outliner.TreeModes
{
public class SelectionSetMode : TreeMode
{
   public SelectionSetMode(TreeView tree, Autodesk.Max.IInterface ip)
      : base(tree, ip)
   { }

   public override void FillTree()
   {
      IINamedSelectionSetManager selSetMan = MaxInterfaces.SelectionSetManager;
      for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
      {
         this.AddNode(i, this.tree.Nodes);
      }
   }

   public override TreeNode AddNode(object node, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(node, parentCol);
      IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);

      foreach (Object child in wrapper.ChildNodes)
         this.AddNode(child, tn.Nodes);

      return tn;
   }
}
}
