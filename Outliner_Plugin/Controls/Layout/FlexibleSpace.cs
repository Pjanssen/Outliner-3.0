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
   public override Size GetSize(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return Size.Empty;

      Size s = new Size(this.Layout.TreeView.Width, this.Layout.TreeView.ItemHeight);
      foreach (TreeNodeLayoutItem item in this.Layout)
      {
         if (item != this && item.IsVisible(tn))
            s.Width -= item.GetSize(tn).Width + item.PaddingLeft + item.PaddingRight;
      }

      s.Width -= 4;
      if ((NativeMethods.GetVisibleScrollbars(this.Layout.TreeView) & ScrollBars.Vertical) == ScrollBars.Vertical)
         s.Width -= SystemInformation.VerticalScrollBarWidth;

      return s;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      //FlexibleSpace does not draw anything.
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
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
