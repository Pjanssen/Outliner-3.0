using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Controls.Tree.Layout
{
public struct ButtonImages
{
   private Image regular;
   private Image disabled;
   private Image regularFiltered;
   private Image disabledFiltered;

   public Image Regular { get { return this.regular; } }
   public Image Disabled { get { return this.disabled; } }
   public Image RegularFiltered { get { return this.regularFiltered; } }
   public Image DisabledFiltered { get { return this.disabledFiltered; } }

   public ButtonImages(Image regular, Image disabled,
                       Image regularFiltered, Image disabledFiltered)
   {
      this.regular = regular;
      this.disabled = disabled;
      this.regularFiltered = regularFiltered;
      this.disabledFiltered = disabledFiltered;
   }
}

public abstract class ImageButton : TreeNodeButton
{
   [XmlAttribute("invert_behavior")]
   [DefaultValue(false)]
   public Boolean InvertBehavior { get; set; }

   protected ImageButton()
   {
      this.InvertBehavior = false;
   }


   public abstract Boolean IsEnabled(TreeNode tn);

   protected static Image CreateDisabledImage(Image image)
   {
      if (image == null)
         throw new ArgumentNullException("image");

      Bitmap img = image.Clone() as Bitmap;
      BitmapProcessing.Desaturate(img);
      BitmapProcessing.Opacity(img, 90);
      return img;
   }

   protected static Image CreateFilteredImage(Image image)
   {
      if (image == null)
         throw new ArgumentNullException("image");

      Bitmap img = image.Clone() as Bitmap;
      BitmapProcessing.Opacity(img, TreeNode.FilteredNodeOpacity);
      return img;
   }


   public ImageButton(ButtonImages images) : this()
   {
      imageEnabled = images.Regular;
      imageEnabled_Filtered = images.RegularFiltered;
      imageDisabled = images.Disabled;
      imageDisabled_Filtered = images.DisabledFiltered;
   }

   protected Image imageEnabled;
   protected virtual Image ImageEnabled { get { return imageEnabled; } }
   protected Image imageDisabled;
   protected virtual Image ImageDisabled
   {
      get
      {
         if (this.imageDisabled == null)
         {
            this.imageDisabled = ImageButton.CreateDisabledImage(this.ImageEnabled);
         }
         return this.imageDisabled;
      }
   }
   protected Image imageEnabled_Filtered;
   protected virtual Image ImageEnabled_Filtered
   {
      get
      {
         if (this.imageEnabled_Filtered == null)
         {
            this.imageEnabled_Filtered = ImageButton.CreateFilteredImage(this.ImageEnabled);
         }
         return this.imageEnabled_Filtered;
      }
   }
   protected Image imageDisabled_Filtered;
   protected virtual Image ImageDisabled_Filtered
   {
      get
      {
         if (this.imageDisabled_Filtered == null)
         {
            this.imageDisabled_Filtered = ImageButton.CreateFilteredImage(this.ImageDisabled);
         }
         return this.imageDisabled_Filtered;
      }
   }



   public override int GetWidth(TreeNode tn)
   {
      return this.ImageEnabled.Width;
   }

   public override int GetHeight(TreeNode tn)
   {
      return this.ImageEnabled.Height;
   }

   public override void Draw(Graphics graphics, TreeNode tn)
   {
      if (graphics == null || tn == null)
         return;

      Boolean isFiltered = !tn.ShowNode;
      Image img = null;
      if (this.IsEnabled(tn) != this.InvertBehavior)
         img = (isFiltered) ? this.ImageEnabled_Filtered : this.ImageEnabled;
      else
         img = (isFiltered) ? this.ImageDisabled_Filtered : this.ImageDisabled;

      this.DrawImage(graphics, tn, img);
   }

   protected void DrawImage(Graphics graphics, TreeNode tn, Image img)
   {
      if (graphics == null)
         throw new ArgumentNullException("graphics");
      if (img == null)
         throw new ArgumentNullException("img");

      Rectangle bounds = this.GetBounds(tn);
      bounds.X += (bounds.Width - img.Width) / 2;
      bounds.Size = img.Size;
      graphics.DrawImage(img, bounds);
   }
}
}
