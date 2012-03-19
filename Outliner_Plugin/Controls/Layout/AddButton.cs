using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;
using Outliner.Commands;
using Autodesk.Max;

namespace Outliner.Controls.Layout
{
public class AddButton : ImageButton
{
   public AddButton() : base(OutlinerResources.button_add,
                             OutlinerResources.button_add_disabled)
   { }

   public override Boolean IsVisible(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      return node is IILayerWrapper || node is SelectionSetWrapper;
   }

   public override bool IsEnabled(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return false;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      ICollection<TreeNode> selTreeNodes = this.Layout.TreeView.SelectedNodes;
      if (node == null || selTreeNodes.Count == 0)
         return false;

      return node.CanAddChildNodes(HelperMethods.GetMaxNodes(selTreeNodes));
   }

   public override void HandleClick(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!this.IsEnabled(tn))
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      if (node is IILayerWrapper)
      {
         IEnumerable<IINode> nodes = HelperMethods.GetWrappedNodes<IINode>(this.Layout.TreeView.SelectedNodes);
         AddToLayerCommand cmd = new AddToLayerCommand(nodes, (IILayer)node.WrappedNode);
         cmd.Execute(true);
      }
   }


   protected override string GetTooltipText(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null || !this.IsEnabled(tn))
         return null;

      if (node is IILayerWrapper)
         return OutlinerResources.Tooltip_Add_Layer;
      else if (node is SelectionSetWrapper)
         return OutlinerResources.Tooltip_Add_SelSet;
      else
         return null;
   }
}
}
