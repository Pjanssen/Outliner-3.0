using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.Filters;

namespace Outliner.Controls.Tree.Layout
{
public class TreeNodeText : TreeNodeButton //TreeNodeLayoutItem
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
      int textWidth = this.GetTextSize(tn).Width + 1;
      int maxWidth = this.getMaxWidth(tn);

      return Math.Min(maxWidth, textWidth);
   }

   private int getMaxWidth(TreeNode tn)
   {
      int maxWidth = this.Layout.TreeView.Width - widthOtherItems(tn);
      maxWidth -= 2; //A few pixels for the borders.
      if (this.Layout.TreeView.VerticalScroll.Visible)
         maxWidth -= SystemInformation.VerticalScrollBarWidth;

      return maxWidth;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight;
   }


   private int widthOtherItems(TreeNode tn)
   {
      int w = 0;
      foreach (TreeNodeLayoutItem item in this.Layout.LayoutItems)
      {
         if (item != this && item.IsVisible(tn))
         {
            w += item.PaddingLeft + item.PaddingRight;
            if (!(item is EmptySpace))
               w += item.GetWidth(tn);
         }
      }
      return w;
   }

   protected override bool Clickable(TreeNode tn)
   {
      return false;
   }

   protected override string GetTooltipText(TreeNode tn)
   {
      int textWidth = this.GetTextSize(tn).Width + 1;
      int maxWidth = this.getMaxWidth(tn);

      if (textWidth > maxWidth)
         return tn.Text;
      else
         return null;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Color bgColor = tree.GetNodeBackColor(tn, true);
      Color fgColor = tree.GetNodeForeColor(tn, true);

      if (tn.FilterResult == FilterResults.ShowChildren)
         fgColor = Color.FromArgb(IconHelperMethods.FILTERED_OPACITY, fgColor);

      using (SolidBrush fgBrush = new SolidBrush(fgColor))
      {
         Rectangle gBounds = this.GetBounds(tn);

         Boolean gradient = tn.State == TreeNodeStates.None && !tn.BackColor.IsEmpty;
         tree.drawBackground(graphics, gBounds, bgColor, gradient);

         using (Font f = new Font(tree.Font, tn.FontStyle))
         {
            using (StringFormat format = new StringFormat())
            {
               format.LineAlignment = StringAlignment.Center;
               format.FormatFlags = StringFormatFlags.NoWrap;
               format.Trimming = StringTrimming.EllipsisPath;
               graphics.DrawString(tn.Text, f, fgBrush, gBounds, format);
            }
         }
      }
   }

   private Boolean clickHandledAtMouseDown;

   public override void HandleMouseDown(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      this.clickHandledAtMouseDown = false;
      TreeView tree          = this.Layout.TreeView;
      Boolean isSelected     = tree.IsSelectedNode(tn);
      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
      {
         if (!isSelected)
         {
            tree.SelectAllNodes(false);
            tree.SelectNode(tn, true);
            tree.OnSelectionChanged();
            this.clickHandledAtMouseDown = true;
         }
      }
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      if (this.clickHandledAtMouseDown)
         return;

      TreeView tree          = this.Layout.TreeView;
      Boolean isSelected     = tree.IsSelectedNode(tn);

      if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
      {
         tree.SelectAllNodes(false);
      }

      if (HelperMethods.ShiftPressed && tree.LastSelectedNode != null)
         tree.SelectNodesInsideRange(tree.LastSelectedNode, tn);
      else if (HelperMethods.ControlPressed)
         tree.SelectNode(tn, !isSelected);
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
