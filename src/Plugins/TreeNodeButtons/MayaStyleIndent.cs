using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WinForms = System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.TreeNodeButtons
{
[OutlinerPlugin(OutlinerPluginType.TreeNodeButton)]
[LocalizedDisplayName(typeof(Resources), "Str_MayaStyleIndent")]
public class MayaStyleIndent : TreeNodeLayoutItem
{
   protected const Int32 CIRCLE_SIZE = 4;

   [XmlAttribute("indent")]
   [DefaultValue(12)]
   public Int32 Indent { get; set; }

   public MayaStyleIndent()
   {
      this.Indent = 12;
   }

   public override TreeNodeLayoutItem Copy()
   {
      MayaStyleIndent newItem = new MayaStyleIndent();

      newItem.PaddingLeft = this.PaddingLeft;
      newItem.PaddingRight = this.PaddingRight;
      newItem.VisibleTypes = this.VisibleTypes;
      newItem.Indent = this.Indent;

      return newItem;
   }

   protected override int GetAutoWidth(TreeNode tn)
   {
      Int32 tnLevel = (tn == null) ? 1 : tn.Level + 1;
      return this.Indent * tnLevel;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null ||
            this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Rectangle bounds = this.GetBounds(tn);

      Color lineColor = tree.GetLineColor(tn);

      using (Pen linePen = new Pen(lineColor))
      {
         Boolean hasParent = tn.Parent != null;
         Boolean hasChildren = tn.Nodes.Count > 0;
         Boolean isExpanded = tn.IsExpanded;

         if (hasParent || hasChildren)
         {
            Int32 indentHalf = this.Indent / 2;
            Int32 circleHalf = CIRCLE_SIZE / 2;
            Int32 yMid = bounds.Top + (bounds.Height / 2);

            if (hasParent)
            {
               // Vertical line segments.
               TreeNode parent = tn.Parent;
               Int32 lineX = bounds.Right - indentHalf - this.Indent;
               while (parent != null && parent.Parent != null)
               {
                  lineX -= this.Indent;

                  if (parent.NextNode != null)
                     graphics.DrawLine(linePen, lineX, bounds.Top, lineX, bounds.Bottom);

                  parent = parent.Parent;
               }

               // T / L section.
               Int32 xEnd = bounds.Right - indentHalf;
               Int32 xStart = xEnd - this.Indent;
               Int32 yEnd = (tn.Index == tn.Parent.Nodes.Count - 1) ? yMid : bounds.Bottom;
               graphics.DrawLine(linePen, xStart, bounds.Top, xStart, yEnd);
               graphics.DrawLine(linePen, xStart, yMid, xEnd - circleHalf, yMid);
            }

            if (hasChildren && tn.IsExpanded)
            {
               Int32 x = bounds.Right - indentHalf;
               graphics.DrawLine(linePen, x, yMid + circleHalf, x, bounds.Bottom);
            }

            Rectangle circleBounds = new Rectangle(0, 0, CIRCLE_SIZE, CIRCLE_SIZE);
            circleBounds.X = bounds.Right - CIRCLE_SIZE - ((this.Indent - CIRCLE_SIZE) / 2);
            circleBounds.Y = yMid - circleHalf;

            graphics.DrawEllipse(linePen, circleBounds);

            if (hasChildren && !isExpanded)
               graphics.FillEllipse(linePen.Brush, circleBounds);
         }
      }
   }
}
}
