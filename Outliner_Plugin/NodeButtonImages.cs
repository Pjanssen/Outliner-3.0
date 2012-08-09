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
      public Image Regular;
      public Image Disabled;
      public Image RegularFiltered;
      public Image DisabledFiltered;

      public ButtonImages(Image regular, Image disabled,
                          Image regularFiltered, Image disabledFiltered)
      {
         Regular = regular;
         Disabled = disabled;
         RegularFiltered = regularFiltered;
         DisabledFiltered = disabledFiltered;
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
