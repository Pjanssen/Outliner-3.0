using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Outliner.Controls;

namespace Outliner
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

   public static class NodeButtonImages
   {
      public enum Images
      {
         Hide,
         Freeze,
         Add,
         Remove,
         Layer,
         BoxMode,
         Renderable
      }

      internal static ButtonImages GetButtonImages(Images image)
      {
         return images[image];
      }

      private static Dictionary<Images, ButtonImages> images =
         new Dictionary<Images, ButtonImages>()
         {
             {Images.Hide, CreateImages(OutlinerResources.button_hide)}
           , {Images.Freeze, CreateImages(OutlinerResources.button_freeze)}
           , {Images.Add, CreateImages(OutlinerResources.button_add)}
           , {Images.Remove, CreateImages(OutlinerResources.button_remove)}
           , {Images.Layer, CreateImages(OutlinerResources.button_layer)}
           , {Images.BoxMode, CreateImages(OutlinerResources.button_boxmode)}
           , {Images.Renderable, CreateImages(OutlinerResources.button_render)}
         };

      private static ButtonImages CreateImages(Image image)
      {
         Image disabledImage = CreateDisabledImage(image);
         return new ButtonImages(image, disabledImage, 
                                 CreateFilteredImage(image), 
                                 CreateFilteredImage(disabledImage));
      }

      private static Image CreateDisabledImage(Image image)
      {
         if (image == null)
            throw new ArgumentNullException("image");

         Bitmap img = image.Clone() as Bitmap;
         BitmapProcessing.Desaturate(img);
         BitmapProcessing.Opacity(img, 90);
         return img;
      }

      private static Image CreateFilteredImage(Image image)
      {
         if (image == null)
            throw new ArgumentNullException("image");

         Bitmap img = image.Clone() as Bitmap;
         BitmapProcessing.Opacity(img, IconHelperMethods.FILTERED_OPACITY);
         return img;
      }
   }
}
