using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.Filters;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Plugins;

namespace Outliner.Controls.Tree.Layout
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(OutlinerResources), "Button_TreeNodeText")]
public class TreeNodeText : TreeNodeButton
{
   public TreeNodeText()
   {
      this.Alignment = StringAlignment.Near;
   }

   public override bool CenterVertically { get { return false; } }

   public override TreeNodeLayoutItem Copy()
   {
      TreeNodeText newItem = new TreeNodeText();
      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      return newItem;
   }

   protected virtual String GetText(TreeNode tn)
   {
      return tn.Text;
   }

   private int MeasureTextWidth(TreeNode tn)
   {
      if (tn == null | tn.TreeView == null)
         return 0;

      TreeView tree = tn.TreeView;
      String text = this.GetText(tn);
      if (text == null || text == "")
         return 0;

      Graphics g = Graphics.FromHwnd(tree.Handle);

      using (Font font = new Font(tree.Font, tn.FontStyle))
      {
         using (StringFormat format = new StringFormat(StringFormat.GenericDefault))
         {
            RectangleF rect = new RectangleF(0, 0, 1000, 1000);
            CharacterRange[] ranges = { new CharacterRange(0, text.Length) };
            Region[] regions = new Region[1];

            format.SetMeasurableCharacterRanges(ranges);
            format.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;

            regions = g.MeasureCharacterRanges(text, font, rect, format);
            rect = regions[0].GetBounds(g);

            return (int)rect.Right + 3 + (int)(text.Length * 0.25);
         }
      }
   }

   public override int GetAutoWidth(TreeNode tn)
   {
      int textWidth = this.MeasureTextWidth(tn);
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
      if (tn == null)
         return String.Empty;

      int textWidth = this.MeasureTextWidth(tn) + 1;
      int maxWidth = this.getMaxWidth(tn);

      if (textWidth > maxWidth)
         return this.GetText(tn);
      else
         return null;
   }

   [XmlAttribute("alignment")]
   [DefaultValue(StringAlignment.Near)]
   public StringAlignment Alignment { get; set; }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Color bgColor = tree.GetNodeBackColor(tn, true);
      Color fgColor = tree.GetNodeForeColor(tn, true);

      using (SolidBrush bgBrush = new SolidBrush(bgColor), 
                        fgBrush = new SolidBrush(fgColor))
      {
         Rectangle gBounds = this.GetBounds(tn);

         graphics.FillRectangle(bgBrush, gBounds);

         using (Font f = new Font(tree.Font, tn.FontStyle))
         {
            using (StringFormat format = new StringFormat())
            {
               format.Alignment = this.Alignment;
               format.LineAlignment = StringAlignment.Center;
               format.FormatFlags = StringFormatFlags.NoWrap;
               format.Trimming = StringTrimming.EllipsisPath;
               graphics.DrawString(this.GetText(tn), f, fgBrush, gBounds, format);
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
      Boolean isSelected     = tn.IsSelected;
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
      Boolean isSelected     = tn.IsSelected;

      if ((e.Button & MouseButtons.Right) != MouseButtons.Right)
      {
         if (!HelperMethods.ControlPressed && !HelperMethods.ShiftPressed)
         {
            tree.SelectAllNodes(false);
         }
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
      TreeView tree = this.Layout.TreeView;
      if (tree.DoubleClickAction == TreeNodeDoubleClickAction.Rename)
         tree.BeginNodeTextEdit(tn, this);
      else if (tree.DoubleClickAction == TreeNodeDoubleClickAction.Expand)
         tn.IsExpanded = !tn.IsExpanded;
   }
}
}
