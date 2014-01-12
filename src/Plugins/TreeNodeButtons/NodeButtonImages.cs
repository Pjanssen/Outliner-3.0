using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.TreeNodeButtons
{
public static class NodeButtonImages
{
   public enum Images
   {
      Add,
      BoxMode,
      Hide,
      Freeze,
      Layer,
      LockTransform,
      Remove,
      Renderable,
   }

   internal static ButtonImages GetButtonImages(Images image)
   {
      return images[image];
   }

   private static Dictionary<Images, ButtonImages> images =
      new Dictionary<Images, ButtonImages>()
      {
           { Images.Add, CreateImages(Resources.button_add)}
         , { Images.BoxMode, CreateImages(Resources.button_boxmode)}
         , { Images.Freeze, CreateImages(Resources.button_freeze)}
         , { Images.Hide, CreateImages(Resources.button_hide)}
         , { Images.Layer, CreateImages(Resources.button_layer)}
         , { Images.LockTransform, CreateImages(Resources.button_lock)}
         , { Images.Remove, CreateImages(Resources.button_remove)}
         , { Images.Renderable, CreateImages(Resources.button_render)}
      };

   public static ButtonImages CreateImages(Image enabledImage)
   {
      Image disabledImage = CreateDisabledImage(enabledImage);
      return CreateImages(enabledImage, disabledImage);
   }

   public static ButtonImages CreateImages(Image enabledImage, Image disabledImage)
   {
      return new ButtonImages( enabledImage
                             , disabledImage
                             , CreateFilteredImage(enabledImage)
                             , CreateFilteredImage(disabledImage));
   }

   public static Image CreateDisabledImage(Image image)
   {
      if (image == null)
         throw new ArgumentNullException("image");

      Bitmap img = image.Clone() as Bitmap;
      BitmapProcessing.Desaturate(img);
      BitmapProcessing.AdjustOpacity(img, 90);
      return img;
   }

   public static Image CreateFilteredImage(Image image)
   {
      if (image == null)
         throw new ArgumentNullException("image");

      Bitmap img = image.Clone() as Bitmap;
      BitmapProcessing.AdjustOpacity(img, TreeNode.FilteredNodeOpacity);
      return img;
   }
}
}
