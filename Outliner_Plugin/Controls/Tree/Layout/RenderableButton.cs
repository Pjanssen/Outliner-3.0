using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;
using System.Drawing;

namespace Outliner.Controls.Tree.Layout
{
public class RenderableButton : ImageButton
{
   public RenderableButton() : base(OutlinerResources.button_render,
                                    OutlinerResources.button_render_disabled)
   { }

   public override bool IsEnabled(TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return false;

      return node.Renderable;
   }

   private Boolean inheritFromLayer(IMaxNodeWrapper node)
   {
      return node is IINodeWrapper
             && ((IINodeWrapper)node).NodeLayerProperties.RenderByLayer;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Rectangle rBounds = this.GetBounds(tn);
      if (this.inheritFromLayer(node))
      {
         Image img = OutlinerResources.layer_small;
         graphics.DrawImage(img, rBounds.Left + (int)Math.Ceiling((rBounds.Width - img.Width) / 2f),
                                 tn.Bounds.Top + (int)Math.Ceiling((tn.Bounds.Height - img.Height) / 2f),
                                 img.Width, img.Height);
      }
      else
         base.Draw(graphics, tn);
   }

   public override void HandleClick(System.Windows.Forms.MouseEventArgs e, TreeNode tn)
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

   protected override string GetTooltipText(TreeNode tn)
   {
      if (tn == null)
         return null;

      String tooltip = OutlinerResources.Tooltip_Renderable;

      if (this.inheritFromLayer(HelperMethods.GetMaxNode(tn)))
         tooltip += " " + OutlinerResources.Tooltip_Inherited;

      return tooltip;
   }
}
}
