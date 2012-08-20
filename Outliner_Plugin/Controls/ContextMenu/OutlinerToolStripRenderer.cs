using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Outliner.Controls.ContextMenu
{
public class OutlinerToolStripRenderer : ToolStripProfessionalRenderer
{
   private OutlinerColorTable outlinerColors;
   public OutlinerToolStripRenderer() : this(new OutlinerColorTable()) { }
   public OutlinerToolStripRenderer(OutlinerColorTable colorTable)
      : base(colorTable)
   {
      this.outlinerColors = colorTable;
   }

   protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
   {
      e.ArrowColor = this.outlinerColors.ArrowColor;
      base.OnRenderArrow(e);
   }

   protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
   {
      e.TextColor = this.outlinerColors.TextColor;
      base.OnRenderItemText(e);
   }

   private Padding dropDownMenuItemPaintPadding = new Padding(2, 0, 1, 0);
   // System.Windows.Forms.Layout.LayoutUtils
   private static Rectangle DeflateRect(Rectangle rect, Padding padding)
   {
      rect.X += padding.Left;
      rect.Y += padding.Top;
      rect.Width -= padding.Horizontal;
      rect.Height -= padding.Vertical;
      return rect;
   }

   protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
   {
      if (!e.Item.IsOnDropDown)
         base.OnRenderMenuItemBackground(e);

      ToolStripItem item = e.Item;
      Graphics graphics = e.Graphics;
      Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
      if (item.Selected)
      {
         rectangle = DeflateRect(rectangle, this.dropDownMenuItemPaintPadding);
         using (Brush brush = new LinearGradientBrush(rectangle, this.ColorTable.ButtonSelectedGradientBegin, this.ColorTable.ButtonSelectedGradientEnd, LinearGradientMode.Vertical))
         {
            graphics.FillRectangle(brush, rectangle);
         }
         using (Pen pen2 = new Pen(this.ColorTable.ButtonSelectedBorder))
         {
            graphics.DrawRectangle(pen2, rectangle.X, rectangle.Y, rectangle.Width - 1, rectangle.Height - 1);
         }
      }
   }

   protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
   {
      base.OnRenderSplitButtonBackground(e);

      ToolStripCheckedSplitButton checkedSplitBtn = e.Item as ToolStripCheckedSplitButton;
      if (checkedSplitBtn != null && checkedSplitBtn.Checked)
      {
         Graphics graphics = e.Graphics;
         Rectangle clipRect = checkedSplitBtn.ButtonBounds;
         clipRect.Width += 1;

         if (checkedSplitBtn.Selected)
         {
            this.RenderPressedButtonFill(graphics, clipRect);
         }
         else
         {
            this.RenderCheckedButtonFill(graphics, clipRect);
         }

         if (checkedSplitBtn.Checked)
         {
            using (Pen pen2 = new Pen(this.ColorTable.ButtonSelectedBorder))
            {
               graphics.DrawRectangle(pen2, clipRect.X, clipRect.Y, clipRect.Width - 1, clipRect.Height - 1);
            }
         }
      }
   }

   private bool UseSystemColors
   {
      get
      {
         return this.ColorTable.UseSystemColors || !ToolStripManager.VisualStylesEnabled;
      }
   }

   private void RenderPressedButtonFill(Graphics g, Rectangle bounds)
   {
      if (bounds.Width == 0 || bounds.Height == 0)
      {
         return;
      }
      if (!this.UseSystemColors)
      {
         using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonPressedGradientBegin, this.ColorTable.ButtonPressedGradientEnd, LinearGradientMode.Vertical))
         {
            g.FillRectangle(brush, bounds);
            return;
         }
      }
      Color buttonPressedHighlight = this.ColorTable.ButtonPressedHighlight;
      using (Brush brush2 = new SolidBrush(buttonPressedHighlight))
      {
         g.FillRectangle(brush2, bounds);
      }
   }

   private void RenderCheckedButtonFill(Graphics g, Rectangle bounds)
   {
      if (bounds.Width == 0 || bounds.Height == 0)
      {
         return;
      }
      if (!this.UseSystemColors)
      {
         using (Brush brush = new LinearGradientBrush(bounds, this.ColorTable.ButtonCheckedGradientBegin, this.ColorTable.ButtonCheckedGradientEnd, LinearGradientMode.Vertical))
         {
            g.FillRectangle(brush, bounds);
            return;
         }
      }
      Color buttonCheckedHighlight = this.ColorTable.ButtonCheckedHighlight;
      using (Brush brush2 = new SolidBrush(buttonCheckedHighlight))
      {
         g.FillRectangle(brush2, bounds);
      }
   }
}
}
