using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.Controls.Layout
{
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

   public override Size GetSize(TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return Size.Empty;

      return new Size(this.Indent * (tn.Level + 1), this.Layout.TreeView.ItemHeight);
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      Rectangle bounds = this.GetBounds(tn);

      using (Pen linePen = new Pen(tree.LineColor))
      {
         Boolean hasParent   = tn.Parent != null;
         Boolean hasChildren = tn.Nodes.Count > 0;
         Boolean isExpanded  = tn.IsExpanded;

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
                     g.DrawLine(linePen, lineX, bounds.Top, lineX, bounds.Bottom);

                  parent = parent.Parent;
               }

               // T / L section.
               Int32 xEnd = bounds.Right - indentHalf;
               Int32 xStart = xEnd - this.Indent;
               Int32 yEnd = (tn.Index == tn.Parent.Nodes.Count - 1) ? yMid : bounds.Bottom;
               g.DrawLine(linePen, xStart, bounds.Top, xStart, yEnd);
               g.DrawLine(linePen, xStart, yMid, xEnd - circleHalf, yMid);
            }

            if (hasChildren && tn.IsExpanded)
            {
               Int32 x = bounds.Right - indentHalf;
               g.DrawLine(linePen, x, yMid + circleHalf, x, bounds.Bottom);
            }

            Rectangle circleBounds = new Rectangle(0, 0, CIRCLE_SIZE, CIRCLE_SIZE);
            circleBounds.X = bounds.Right - CIRCLE_SIZE - ((this.Indent - CIRCLE_SIZE) / 2);
            circleBounds.Y = yMid - circleHalf;

            g.DrawEllipse(linePen, circleBounds);
            
            if (hasChildren && !isExpanded)
               g.FillEllipse(linePen.Brush, circleBounds);
         }
      }
   }

   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn) { }
}
}
