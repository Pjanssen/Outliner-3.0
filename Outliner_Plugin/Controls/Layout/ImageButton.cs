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
public abstract class ImageButton : TreeNodeLayoutItem
{
   [XmlAttribute("invert_behavior")]
   [DefaultValue(false)]
   public Boolean InvertBehavior { get; set; }

   public abstract Boolean IsEnabled(TreeNode tn);

   protected Bitmap enabledImg;
   protected Bitmap disabledImg;
   protected Bitmap enabledImg_Filtered;
   protected Bitmap disabledImg_Filtered;

   public ImageButton(Bitmap enabledImg, Bitmap disabledImg)
   {
      if (enabledImg == null)
         throw new ArgumentNullException("ImageButton enabled image cannot be null");

      this.InvertBehavior = false;

      this.enabledImg  = enabledImg;
      this.disabledImg = (disabledImg != null) ? disabledImg : enabledImg;

      this.enabledImg_Filtered = (Bitmap)enabledImg.Clone();
      this.disabledImg_Filtered = (Bitmap)this.disabledImg.Clone();
      BitmapProcessing.Opacity(this.enabledImg_Filtered, IconHelperMethods.FILTERED_OPACITY);
      BitmapProcessing.Opacity(this.disabledImg_Filtered, IconHelperMethods.FILTERED_OPACITY);
   }

   public override Size GetSize(TreeNode tn)
   {
      return this.enabledImg.Size;
   }

   public override void Draw(Graphics g, TreeNode tn)
   {
      Boolean isFiltered = false;
      TreeNodeData tnData = tn.Tag as TreeNodeData;
      if (tnData != null)
         isFiltered = tnData.FilterResult != FiltersBase.FilterResult.Show;

      Image img = null;
      if (this.IsEnabled(tn) != this.InvertBehavior)
         img = (isFiltered) ? this.enabledImg_Filtered : this.enabledImg;
      else
         img = (isFiltered) ? this.disabledImg_Filtered : this.disabledImg;
      /*
      if (!this.IsEnabled(tn))
         img = (isFiltered) ? this.disabledImg_Filtered : this.disabledImg;
      else
         img = (isFiltered) ? this.enabledImg_Filtered : this.enabledImg;
      */
      g.DrawImage(img, this.GetBounds(tn));
   }
}
}
