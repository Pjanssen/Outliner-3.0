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
   public class HideButton : ImageButton
   {
      public HideButton() : base(OutlinerResources.button_hide,
                                 OutlinerResources.button_hide_disabled)
      { }

      public override bool IsEnabled(TreeNode tn)
      {
         IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
         if (node == null)
            return false;

         return node.IsHidden;
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

         HideCommand cmd = new HideCommand(HelperMethods.GetMaxNodes(nodes),
                                           !node.IsHidden);
         cmd.Execute(true);

         if (tree.NodeSorter is HiddenSorter)
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
            if (node.IsHidden)
               return OutlinerResources.Tooltip_Unhide;
            else
               return OutlinerResources.Tooltip_Hide;
         }

         return null;
      }
   }
}
