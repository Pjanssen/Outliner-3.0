using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Outliner.Controls.Layout
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
      if (this.Layout == null || this.Layout.TreeView == null)
         return 0;

      return this.Layout.TreeView.ItemHeight;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;
//      if (tn.IsEditing)
//         return;

      TreeView tree = this.Layout.TreeView;
      TreeViewColors colors = tree.Colors;
      Color bgColor = Color.Empty;
      Color fgColor = Color.Empty;
      if ((tn.State & TreeNodeState.Selected) == TreeNodeState.Selected)
      {
         bgColor = colors.SelectionBackColor;
         fgColor = colors.SelectionForeColor;
      }
      else if ((tn.State & TreeNodeState.ParentOfSelected) == TreeNodeState.ParentOfSelected)
      {
         bgColor = colors.ParentBackColor;
         fgColor = colors.ParentForeColor;
      }
      else
      {
         bgColor = colors.NodeBackColor;
         fgColor = colors.NodeForeColor;
      }

      if (tn.FilterResult == FiltersBase.FilterResult.ShowChildren)
         fgColor = Color.FromArgb(IconHelperMethods.FILTERED_OPACITY, fgColor);

      using (SolidBrush bgBrush = new SolidBrush(bgColor),
                        fgBrush = new SolidBrush(fgColor))
      {
         Rectangle gBounds = this.GetBounds(tn);
         
         g.FillRectangle(bgBrush, gBounds);
         g.DrawString(tn.Text, tree.Font, fgBrush, 
                      gBounds.X, gBounds.Y + ((gBounds.Height - this.GetTextSize(tn).Height) / 2), StringFormat.GenericDefault);
      }
   }

   public override void HandleClick(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      Keys modKeys           = Control.ModifierKeys;
      Boolean controlPressed = (modKeys & Keys.Control) == Keys.Control;
      Boolean shiftPressed   = (modKeys & Keys.Shift) == Keys.Shift;
      TreeView tree          = this.Layout.TreeView;

      if (!controlPressed && !shiftPressed)
         tree.SelectAllNodes(false);

      if (shiftPressed && tree.LastSelectedNode != null)
         tree.SelectNodesInsideRange(tree.LastSelectedNode, tn);
      else if (controlPressed)
         tree.SelectNode(tn, !tree.IsSelectedNode(tn));
      else
         tree.SelectNode(tn, true);

      tree.OnSelectionChanged();
   }

   public override void HandleDoubleClick(MouseEventArgs e, TreeNode tn)
   {
//      tn.BeginEdit();
   }
}
}
