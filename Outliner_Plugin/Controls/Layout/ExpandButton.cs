using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;

namespace Outliner.Controls.Layout
{
public class ExpandButton : TreeNodeLayoutItem
{
   protected const Int32 GUTTERWIDTH = 15;
   protected const Int32 GUTTERHMID = 7;
   protected const Int32 GLYPHSIZE = 9;
   protected const Int32 GLYPHMID = 4;

   public override bool CenterVertically { get { return false; } }
   protected Int32 GetVMiddle(Point pos)
   {
      if (this.Layout == null)
         return 0;

      return pos.Y + (this.Layout.TreeView.ItemHeight / 2) - 1;
   }

   public override Size GetSize(TreeNode tn)
   {
      if (this.Layout == null)
         return Size.Empty;

      return new Size(ExpandButton.GUTTERWIDTH,
                      this.Layout.TreeView.ItemHeight);
   }

   protected Rectangle GetGlyphBounds(TreeNode tn)
   {
      Rectangle bounds = this.GetBounds(tn);
      return new Rectangle(bounds.Right - GUTTERHMID - GLYPHMID,
                           this.GetVMiddle(bounds.Location) - GLYPHMID,
                           GLYPHSIZE, GLYPHSIZE);
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      Rectangle bounds = this.GetBounds(tn);
      this.DrawGlyph(tn, g, bounds);
   }




   protected void DrawGlyph(TreeNode tn, Graphics g, Rectangle bounds)
   {
      if (this.Layout == null)
         return;
      if (tn.GetNodeCount(false) == 0)
         return;

      Rectangle glyphBounds = this.GetGlyphBounds(tn);
      /*
      if (Application.RenderWithVisualStyles)
      {
         VisualStyleElement element = (tn.IsExpanded) ? VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;
         VisualStyleRenderer renderer = new VisualStyleRenderer(element);
         renderer.DrawBackground(g, glyphBounds);
      }
      else
      {
         */
         using (Pen linePen = new Pen(this.Layout.TreeView.LineColor))
         using (Brush bgBrush = new SolidBrush(this.Layout.TreeView.BackColor))
         {
            glyphBounds.Width -= 1;
            glyphBounds.Height -= 1;
            g.FillRectangle(bgBrush, glyphBounds);
            g.DrawRectangle(linePen, glyphBounds);
            g.DrawLine(linePen, glyphBounds.X + GLYPHMID - 2, 
                                glyphBounds.Y + GLYPHMID,
                                glyphBounds.X + GLYPHMID + 2,
                                glyphBounds.Y + GLYPHMID);

            if (!tn.IsExpanded)
            {
               g.DrawLine(linePen, glyphBounds.X + GLYPHMID, 
                                   glyphBounds.Y + GLYPHMID - 2,
                                   glyphBounds.X + GLYPHMID,
                                   glyphBounds.Y + GLYPHMID + 2);
            }
         }
      //}
   }


   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      Rectangle glyphBounds = this.GetGlyphBounds(tn);
      if (glyphBounds.Contains(e.Location))
      {
         if ((Control.ModifierKeys & Keys.Control) == Keys.Control && !tn.IsExpanded)
         {
            this.Layout.TreeView.BeginUpdate();
            tn.ExpandAll();
            this.Layout.TreeView.EndUpdate();
         }
         else
            tn.Toggle();
      }
   }
}
}
