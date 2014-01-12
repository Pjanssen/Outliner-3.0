using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace PJanssen.Outliner.Controls
{
public class OutlinerGroupBox : GroupBox
{
   private Color borderColor;

   public Color BorderColor
   {
      get { return this.borderColor; }
      set { this.borderColor = value; this.Invalidate(); }
   }

   public OutlinerGroupBox()
   {
      this.borderColor = Color.Black;
   }

   protected override void OnPaint(PaintEventArgs e)
   {
      if (e == null)
         throw new ArgumentNullException("e");

      Size tSize = TextRenderer.MeasureText(this.Text, this.Font);

      Rectangle borderRect = this.ClientRectangle;
      borderRect.Y += tSize.Height / 2;
      borderRect.Height -= tSize.Height / 2;
      ControlPaint.DrawBorder(e.Graphics, borderRect, this.borderColor, ButtonBorderStyle.Solid);

      Rectangle textRect = this.ClientRectangle;
      textRect.X += 6;
      textRect.Width = (Int32)(tSize.Width + (this.Text.Length * 0.4f));
      textRect.Height = tSize.Height;
      using (Brush bgBrush = new SolidBrush(this.BackColor),
                   fgBrush = new SolidBrush(this.ForeColor))
      {
         e.Graphics.FillRectangle(bgBrush, textRect);
         e.Graphics.DrawString(this.Text, this.Font, fgBrush, textRect);
      }
   }
}
}
