using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls;
using Outliner.Controls.Tree;

namespace Outliner.TreeNodeButtons
{
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
           {Images.Hide, CreateImages(Resources.button_hide)}
         , {Images.Freeze, CreateImages(Resources.button_freeze)}
         , {Images.Add, CreateImages(Resources.button_add)}
         , {Images.Remove, CreateImages(Resources.button_remove)}
         , {Images.Layer, CreateImages(Resources.button_layer)}
         , {Images.BoxMode, CreateImages(Resources.button_boxmode)}
         , {Images.Renderable, CreateImages(Resources.button_render)}
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
      BitmapProcessing.Opacity(img, TreeNode.FilteredNodeOpacity);
      return img;
   }
}
}
