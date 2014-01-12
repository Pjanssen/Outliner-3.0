using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PJanssen.Outliner.ColorTags
{
public class ColorTagToolStripButton : ToolStripButton
{
   public ColorTag ColorTag { get; set; }

   public ColorTagToolStripButton() : this(ColorTag.None) { }
   public ColorTagToolStripButton(ColorTag colorTag)
   {
      this.ColorTag = colorTag;
   }

   protected override void OnPaint(PaintEventArgs e)
   {
      base.OnPaint(e);

      Rectangle rBounds = new Rectangle((this.Width / 2) - 6
                                       , (this.Height / 2) - 8
                                       , 12, 16);
      if (this.ColorTag != ColorTag.WireColor && this.ColorTag != ColorTag.None)
      {
         using (Pen linePen = new Pen(Color.Black))
         {
            e.Graphics.DrawLine(linePen, rBounds.Left + 1, rBounds.Top
                                       , rBounds.Right - 2, rBounds.Top);
            e.Graphics.DrawLine(linePen, rBounds.Left + 1, rBounds.Bottom - 1
                                       , rBounds.Right - 2, rBounds.Bottom - 1);
            e.Graphics.DrawLine(linePen, rBounds.Left, rBounds.Top + 1
                                       , rBounds.Left, rBounds.Bottom - 2);
            e.Graphics.DrawLine(linePen, rBounds.Right - 1, rBounds.Top + 1
                                       , rBounds.Right - 1, rBounds.Bottom - 2);
         }
      }

      if (this.ColorTag == ColorTag.None)
      {
         e.Graphics.DrawImage(Resources.delete_small, rBounds.X + 2, rBounds.Y + 4, 8, 8);
      }
      else if (this.ColorTag == ColorTag.WireColor)
      {
         e.Graphics.DrawImage(Resources.color_small, (this.Bounds.Width - 12) / 2, (this.Bounds.Height - 12) / 2, 12, 12);
      }
      else
      {
         Color color = ColorTags.GetTagColor(this.ColorTag);
         Color gradColor = Color.FromArgb(Math.Min(color.R + 40, 255)
                                          , Math.Min(color.G + 40, 255)
                                          , Math.Min(color.B + 40, 255));
         using (LinearGradientBrush brush = new LinearGradientBrush(rBounds, gradColor, color, LinearGradientMode.Vertical))
         {
            rBounds.Inflate(-1, -1);
            e.Graphics.FillRectangle(brush, rBounds);
         }
      }
   }
}
}
