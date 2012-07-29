using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner.Controls.Tree.Layout
{
public class ExpandButton : TreeNodeLayoutItem
{
   protected const Int32 GUTTERWIDTH = 15;
   protected const Int32 GUTTERHMID = 7;
   protected const Int32 GLYPHSIZE = 9;
   protected const Int32 GLYPHMID = 4;

   protected Boolean fillBackground;

   [XmlAttribute("use_visual_styles")]
   [DefaultValue(true)]
   public Boolean UseVisualStyles { get; set; }

   public ExpandButton()
   {
      this.fillBackground = false;
      this.UseVisualStyles = true;
   }

   public override bool CenterVertically { get { return false; } }
   protected Int32 GetVMiddle(Point pos)
   {
      if (this.Layout == null)
         return 0;

      return pos.Y + (this.Layout.ItemHeight / 2) - 1;
   }

   public override int GetWidth(TreeNode tn)
   {
      return ExpandButton.GUTTERWIDTH;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight;
   }

   protected Rectangle GetGlyphBounds(TreeNode tn)
   {
      Rectangle bounds = this.GetBounds(tn);
      return new Rectangle(bounds.Right - GUTTERHMID - GLYPHMID,
                           this.GetVMiddle(bounds.Location) - GLYPHMID,
                           GLYPHSIZE, GLYPHSIZE);
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (this.Layout == null || graphics == null || tn == null)
         return;
      if (tn.Nodes.Count == 0)
         return;

      Rectangle glyphBounds = this.GetGlyphBounds(tn);

      if (this.UseVisualStyles && Application.RenderWithVisualStyles)
      {
         VisualStyleElement element = (tn.IsExpanded) ? VisualStyleElement.TreeView.Glyph.Opened : VisualStyleElement.TreeView.Glyph.Closed;
         VisualStyleRenderer renderer = new VisualStyleRenderer(element);
         renderer.DrawBackground(graphics, glyphBounds);
      }
      else
      {
         Color lineColor = this.Layout.TreeView.GetNodeForeColor(tn, false);
         using (Pen linePen = new Pen(lineColor))
         {
            glyphBounds.Width -= 1;
            glyphBounds.Height -= 1;
            if (this.fillBackground)
            {
               using (Brush bgBrush = new SolidBrush(this.Layout.TreeView.BackColor))
               {
                  graphics.FillRectangle(bgBrush, glyphBounds);
               }
            }
            graphics.DrawRectangle(linePen, glyphBounds);
            graphics.DrawLine(linePen, glyphBounds.X + GLYPHMID - 2, 
                                       glyphBounds.Y + GLYPHMID,
                                       glyphBounds.X + GLYPHMID + 2,
                                       glyphBounds.Y + GLYPHMID);

            if (!tn.IsExpanded)
            {
               graphics.DrawLine(linePen, glyphBounds.X + GLYPHMID, 
                                          glyphBounds.Y + GLYPHMID - 2,
                                          glyphBounds.X + GLYPHMID,
                                          glyphBounds.Y + GLYPHMID + 2);
            }
         }
      }
   }


   public override void HandleMouseUp(MouseEventArgs e, TreeNode tn)
   {
      if (e == null || tn == null)
         return;

      Rectangle glyphBounds = this.GetGlyphBounds(tn);
      if (glyphBounds.Contains(e.Location))
      {
         if (Control.ModifierKeys.HasFlag(Keys.Control) && !tn.IsExpanded)
         {
            this.Layout.TreeView.BeginUpdate();
            tn.ExpandAll();
            this.Layout.TreeView.EndUpdate();
         }
         else
            tn.IsExpanded = !tn.IsExpanded;
      }
   }
}
}
