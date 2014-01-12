using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PJanssen.Outliner.Controls.ContextMenu
{
public class OutlinerToolStripRenderer : ToolStripProfessionalRenderer
{
   private ContextMenuColorTable colors;
   public OutlinerToolStripRenderer(ContextMenuColorTable colorTable)
      : base(colorTable)
   {
      this.colors = colorTable;
   }

   protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
   {
      e.ArrowColor = this.colors.Arrow;
      base.OnRenderArrow(e);
   }

   protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
   {
      e.TextColor = this.colors.Text;
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

   protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
   {
      using (Brush bgBrush = new SolidBrush(this.colors.ToolStripDropDownBackground))
      {
         e.Graphics.FillRectangle(bgBrush, e.AffectedBounds);
      }
   }

   protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
   {
      //base.OnRenderToolStripBorder(e);
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

   protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
   {
      //Same as base, but removed checkstate background rendering.

      Rectangle imageRectangle = e.ImageRectangle;
      Image image = e.Image;

      if (imageRectangle != Rectangle.Empty && image != null)
      {
         if (!e.Item.Enabled)
         {
            base.OnRenderItemImage(e);
            return;
         }
         if (e.Item.ImageScaling == ToolStripItemImageScaling.None)
         {
            e.Graphics.DrawImage(image, imageRectangle, new Rectangle(Point.Empty, imageRectangle.Size), GraphicsUnit.Pixel);
            return;
         }
         e.Graphics.DrawImage(image, imageRectangle);
      }
   }

   protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
   {
      ToolStripItem item = e.Item;
      Graphics graphics = e.Graphics;

      if (!item.Selected)
      {
         Rectangle rectangle = new Rectangle(Point.Empty, item.Size);
         rectangle.Inflate(-2, 0);
         using (Brush brush = new LinearGradientBrush(rectangle, this.ColorTable.ButtonCheckedGradientBegin, this.ColorTable.ButtonCheckedGradientEnd, LinearGradientMode.Vertical))
         {
            graphics.FillRectangle(brush, rectangle);
         }
         using (Pen pen2 = new Pen(this.ColorTable.ButtonSelectedBorder))
         {
            graphics.DrawRectangle(pen2, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height - 1);
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
