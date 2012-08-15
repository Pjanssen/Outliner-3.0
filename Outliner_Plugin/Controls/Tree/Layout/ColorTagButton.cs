using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;
using System.Drawing.Drawing2D;

namespace Outliner.Controls.Tree.Layout
{
public class ColorTagButton : TreeNodeButton
{
   [XmlAttribute("button_width")]
   [DefaultValue(10)]
   public Int32 ButtonWidth { get; set; }

   public ColorTagButton()
   {
      this.ButtonWidth = 10;
   }

   public override int GetWidth(TreeNode tn)
   {
      return this.ButtonWidth;
   }

   public override int GetHeight(TreeNode tn)
   {
      if (this.Layout == null)
         return 0;

      return this.Layout.ItemHeight - 4;
   }
   
   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      IMaxNodeWrapper node = HelperMethods.GetMaxNode(tn);
      if (node == null)
         return;

      Int32 opacity = this.Layout.TreeView.GetNodeOpacity(tn);
      Boolean hasTag = ColorTags.HasTag(node.WrappedNode as IAnimatable);
      
      Rectangle rBounds = this.GetBounds(tn);
      using (Pen linePen = new Pen(Color.FromArgb(opacity, Color.Black)))
      {
         graphics.DrawLine(linePen, rBounds.Left + 1, rBounds.Top
                                  , rBounds.Right - 2, rBounds.Top);
         graphics.DrawLine(linePen, rBounds.Left + 1, rBounds.Bottom - 1
                                  , rBounds.Right - 2, rBounds.Bottom - 1);
         graphics.DrawLine(linePen, rBounds.Left, rBounds.Top + 1
                                  , rBounds.Left, rBounds.Bottom - 2);
         graphics.DrawLine(linePen, rBounds.Right - 1, rBounds.Top + 1
                                  , rBounds.Right - 1, rBounds.Bottom - 2);         
      }

      if (hasTag)
      {
         Color color = Color.FromArgb(opacity, ColorTags.GetColor(node.WrappedNode as IAnimatable));
         Color gradColor = Color.FromArgb(opacity, Math.Min(color.R + 40, 255)
                                                  , Math.Min(color.G + 40, 255)
                                                  , Math.Min(color.B + 40, 255));
         using (LinearGradientBrush brush = new LinearGradientBrush(rBounds, gradColor, color, LinearGradientMode.Vertical))
         {
            rBounds.Inflate(-1, -1);
            graphics.FillRectangle(brush, rBounds);
         }
      }
   }
}

internal class ColorTagSelector : System.Windows.Forms.Form
{
   internal ColorTagSelector()
   {
      //this.
   }
}
}
