using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Outliner.Controls.Tree.Layout
{
public class FlexibleSpace : TreeNodeLayoutItem
{
   public override int GetWidth(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return 0;

      TreeView tree = this.Layout.TreeView;
      Int32 w = tree.Width;
      foreach (TreeNodeLayoutItem item in this.Layout.LayoutItems)
      {
         if (item != this && item.IsVisible(tn))
            w -= item.GetWidth(tn) + item.PaddingLeft + item.PaddingRight;
      }

      w -= 4;
      if (tree.VerticalScroll.Visible)
         w -= SystemInformation.VerticalScrollBarWidth;

      return w;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      //FlexibleSpace does not draw anything.
   }

   public override void HandleMouseDown(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Boolean isSelected = tree.IsSelectedNode(tn);
      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
      {
         if (!isSelected && this.Layout.FullRowSelect)
         {
            tree.SelectAllNodes(false);
            tree.SelectNode(tn, true);
            tree.OnSelectionChanged();
         }
      }
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Boolean isSelected = tree.IsSelectedNode(tn);

      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
      {
         tree.SelectAllNodes(false);
      }

      if (this.Layout.FullRowSelect)
      {
         if (HelperMethods.ShiftPressed && tree.LastSelectedNode != null)
            tree.SelectNodesInsideRange(tree.LastSelectedNode, tn);
         else if (HelperMethods.ControlPressed)
            tree.SelectNode(tn, !isSelected);
         else
            tree.SelectNode(tn, true);
      }

      tree.OnSelectionChanged();
   }
}
}
