using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Filters;

namespace Outliner.Controls.Tree.Layout
{
public abstract class ImageButton : TreeNodeButton
{
   [XmlAttribute("invert_behavior")]
   [DefaultValue(false)]
   public Boolean InvertBehavior { get; set; }

   public abstract Boolean IsEnabled(TreeNode tn);

   private Bitmap enabledImage;
   private Bitmap disabledImage;
   private Bitmap enabledImage_Filtered;
   private Bitmap disabledImage_Filtered;

   protected ImageButton(Bitmap enabledImage, Bitmap disabledImage)
   {
      if (enabledImage == null)
         throw new ArgumentNullException("enabledImage");

      this.InvertBehavior = false;

      this.enabledImage  = enabledImage;
      this.disabledImage = (disabledImage != null) ? disabledImage : enabledImage;

      this.enabledImage_Filtered = (Bitmap)enabledImage.Clone();
      this.disabledImage_Filtered = (Bitmap)this.disabledImage.Clone();
      BitmapProcessing.Opacity(this.enabledImage_Filtered, IconHelperMethods.FILTERED_OPACITY);
      BitmapProcessing.Opacity(this.disabledImage_Filtered, IconHelperMethods.FILTERED_OPACITY);
   }

   public override int GetWidth(TreeNode tn)
   {
      return this.enabledImage.Width;
   }

   public override int GetHeight(TreeNode tn)
   {
      return this.enabledImage.Height;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      Boolean isFiltered = !tn.FilterResult.HasFlag(FilterResults.Show);

      Image img = null;
      if (this.IsEnabled(tn) != this.InvertBehavior)
         img = (isFiltered) ? this.enabledImage_Filtered : this.enabledImage;
      else
         img = (isFiltered) ? this.disabledImage_Filtered : this.disabledImage;

      graphics.DrawImage(img, this.GetBounds(tn));
   }
}
}
