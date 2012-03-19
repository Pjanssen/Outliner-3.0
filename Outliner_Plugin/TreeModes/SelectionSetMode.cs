using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.FiltersBase;
using Outliner.Controls;

namespace Outliner.TreeModes
{
   public class SelectionSetMode : TreeMode
   {
      public SelectionSetMode(Outliner.Controls.TreeView tree, Autodesk.Max.IInterface ip)
         : base(tree, ip)
      { }

      public override void FillTree()
      {
         IINamedSelectionSetManager selSetMan = GlobalInterface.Instance.INamedSelectionSetManager.Instance;
         for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
         {
            this.addSelSetNode(selSetMan, i);
         }
      }

      private void addSelSetNode(IINamedSelectionSetManager manager, int index)
      {
         IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(new KeyValuePair<IINamedSelectionSetManager, int>(manager, index));
         FilterResult filterResult = this.Filters.ShowNode(wrapper);
         if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(index))
         {
            TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
            tn.FilterResult = filterResult;

            this.nodes.Add(index, tn);
            this.tree.Nodes.Add(tn);

            Int32 nodeCount = manager.GetNamedSelSetItemCount(index);
            if (nodeCount > 0)
            {
               IINodeTab nodes = Autodesk.Max.GlobalInterface.Instance.INodeTabNS.Create();
               manager.GetNamedSelSetList(nodes, index);
               for (int j = 0; j < nodeCount; j++) 
               {
                  this.addNode(nodes[(IntPtr)j], tn.Nodes);
               }
            }
         }
      }

      protected void addNode(IINode node, TreeNodeCollection parentCol)
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
      }
   }
}
