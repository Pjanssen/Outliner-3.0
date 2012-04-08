using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.NodeSorters;

namespace Outliner.Controls.Tree.Layout
{
public class FreezeButton : ImageButton
{
   public FreezeButton() : base(OutlinerResources.button_freeze,
                                 OutlinerResources.button_freeze_disabled)
   { }

   public override bool IsEnabled(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      return node.IsFrozen;
   }

   public override void HandleClick(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      TreeView tree = this.Layout.TreeView;
      IEnumerable<TreeNode> nodes = null;
      if (tree.IsSelectedNode(tn) && !HelperMethods.ControlPressed)
         nodes = tree.SelectedNodes;
      else
         nodes = new List<TreeNode>(1) { tn };

      FreezeCommand cmd = new FreezeCommand(HelperMethods.GetMaxNodes(nodes), 
                                            !node.IsFrozen);
      cmd.Execute(true);

      if (tree.NodeSorter is FrozenSorter)
      {
         tree.AddToSortQueue(nodes);
         tree.StartTimedSort(true);
      }
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node != null)
      {
         if (node.IsFrozen)
            return OutlinerResources.Tooltip_Unfreeze;
         else
            return OutlinerResources.Tooltip_Freeze;
      }

      return null;
   }
}
}
