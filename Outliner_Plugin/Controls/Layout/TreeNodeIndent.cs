using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace Outliner.Controls.Layout
{
public class TreeNodeIndent : ExpandButton
{
   [XmlAttribute("indent")]
   [DefaultValue(15)]
   public Int32 Indent { get; set; }

   public TreeNodeIndent()
   {
      this.Indent = 15;
   }

   public override int GetWidth(TreeNode tn)
   {
      Int32 tnLevel = (tn == null) ? 0 : tn.Level;
      return GUTTERWIDTH + this.Indent * tnLevel;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      //Draw lines.
      this.DrawLines(tn, g, this.GetBounds(tn));
         
      //Draw expand button.
      base.Draw(g, tn);
   }

   protected void DrawLines(TreeNode tn, Graphics g, Rectangle bounds)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      TreeView tree = this.Layout.TreeView;
      using (Pen linePen = new Pen(tree.Colors.LineColor))
      {
         linePen.DashStyle = DashStyle.Dot;
            
         //Vertical line segments.
         TreeNode parent = tn.Parent;
         Int32 lineX = bounds.Right - ExpandButton.GUTTERHMID;
         while (parent != null)
         {
            lineX -= this.Indent;

            if (parent.NextNode != null)
               g.DrawLine(linePen, lineX, bounds.Top, lineX, bounds.Bottom);

            parent = parent.Parent;
         }

         // T / L section.
         lineX = bounds.Right - ExpandButton.GUTTERHMID;
         Int32 vlineStartY = bounds.Top;
         Int32 vlineEndY = bounds.Bottom;
         Int32 vMiddle = bounds.Top + ((bounds.Bottom - bounds.Top) / 2);
         
         if (vMiddle % 2 != 0)
            vMiddle -= 1;

         if (tn.Parent == null && tn.Index == 0)
            vlineStartY = vMiddle;

         if (tn.NextNode == null)
            vlineEndY = vMiddle;

         g.DrawLine(linePen, lineX, vlineStartY, lineX, vlineEndY);
         g.DrawLine(linePen, lineX, vMiddle, bounds.Right, vMiddle);
      }
   }
}
}
