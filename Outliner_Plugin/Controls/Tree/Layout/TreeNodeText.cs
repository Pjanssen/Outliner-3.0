using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.Filters;

namespace Outliner.Controls.Tree.Layout
{
public class TreeNodeText : TreeNodeLayoutItem
{
   public override bool CenterVertically { get { return false; } }

   private Size GetTextSize(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null || tn == null)
         return Size.Empty;

      return TextRenderer.MeasureText(tn.Text, this.Layout.TreeView.Font);
   }

   public override int GetWidth(TreeNode tn)
   {
      return this.GetTextSize(tn).Width;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      TreeViewColors colors = tree.Colors;
      Color bgColor = tree.GetNodeBackColor(tn);
      Color fgColor = tree.GetNodeForeColor(tn);

      if (tn.FilterResult == FilterResults.ShowChildren)
         fgColor = Color.FromArgb(IconHelperMethods.FILTERED_OPACITY, fgColor);

      using (SolidBrush bgBrush = new SolidBrush(bgColor),
                        fgBrush = new SolidBrush(fgColor))
      {
         Rectangle gBounds = this.GetBounds(tn);
         
         if (!this.Layout.FullRowSelect)
            graphics.FillRectangle(bgBrush, gBounds);
         
         graphics.DrawString(tn.Text, tree.Font, fgBrush, 
                      gBounds.X, gBounds.Y + ((gBounds.Height - this.GetTextSize(tn).Height) / 2), StringFormat.GenericDefault);
      }
   }

   public override void HandleClick(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      Keys modKeys           = Control.ModifierKeys;
      TreeView tree          = this.Layout.TreeView;

      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
         tree.SelectAllNodes(false);

      if (HelperMethods.ShiftPressed && tree.LastSelectedNode != null)
         tree.SelectNodesInsideRange(tree.LastSelectedNode, tn);
      else if (HelperMethods.ControlPressed)
         tree.SelectNode(tn, !tree.IsSelectedNode(tn));
      else
         tree.SelectNode(tn, true);

      tree.OnSelectionChanged();
   }

   public override void HandleDoubleClick(MouseEventArgs e, TreeNode tn)
   {
      this.Layout.TreeView.BeginNodeTextEdit(tn, this);
   }
}
}
