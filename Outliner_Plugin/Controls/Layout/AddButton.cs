using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.Layout
{
public class AddButton : ImageButton
{
   public AddButton() : base(OutlinerResources.add_button,
                             OutlinerResources.add_button_disabled)
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

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (!this.IsEnabled(tn))
         return;

      //TODO add to layer/selection set.
   }
}
}
