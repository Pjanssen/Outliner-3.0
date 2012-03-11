using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Controls.Layout
{
public class RenderableButton : ImageButton
{
   public RenderableButton() : base(OutlinerResources.button_render,
                                    OutlinerResources.button_render_disabled)
   { }

   public override bool IsEnabled(System.Windows.Forms.TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      return node.Renderable;
   }

   public override void HandleMouseUp(System.Windows.Forms.MouseEventArgs e, System.Windows.Forms.TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      TreeView tree = this.Layout.TreeView;
      IEnumerable<IMaxNodeWrapper> nodes = null;
      if (HelperMethods.ControlPressed && tree.IsSelectedNode(tn))
         nodes = HelperMethods.GetMaxNodes(tree.SelectedNodes);
      else
         nodes = new List<IMaxNodeWrapper>(1) { node };

      SetRenderableCommand cmd = new SetRenderableCommand(nodes, !node.Renderable);
      cmd.Execute(true);
   }
}
}
