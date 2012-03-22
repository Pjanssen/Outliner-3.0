using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Outliner.Controls.Layout
{
public class FlexibleSpace : TreeNodeLayoutItem
{
   public override int GetWidth(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return 0;

      TreeView tree = this.Layout.TreeView;
      Int32 w = tree.Width;
      foreach (TreeNodeLayoutItem item in this.Layout)
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

   public override void HandleClick(MouseEventArgs e, TreeNode tn)
   {
      Keys modKeys = Control.ModifierKeys;
      if ((modKeys & Keys.Control) != Keys.Control && (modKeys & Keys.Shift) != Keys.Shift)
      {
         this.Layout.TreeView.SelectAllNodes(false);
         this.Layout.TreeView.OnSelectionChanged();
      }
   }
}
}
