using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Resources;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using System.Xml.Serialization;
using PJanssen.Outliner.Controls;

namespace PJanssen.Outliner.TreeNodeButtons
{
public enum IconSet
{
   Blender,
   Max,
   Maya
}

public static class IconHelperMethods
{
   public static ResourceSet GetIconResSet(IconSet iconSet)
   {
      ResourceManager res = null;
      if (iconSet == IconSet.Blender)
         res = IconsBlender.ResourceManager;
      else if (iconSet == IconSet.Max)
         res = IconsMax.ResourceManager;
      else if (iconSet == IconSet.Maya)
         res = IconsMaya.ResourceManager;

      if (res == null)
         return null;

      return res.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);
   }

   public static Dictionary<String, Bitmap> CreateIconSetBitmaps(IconSet iconSet, Boolean invert)
   {
      return CreateIconSetBitmaps(GetIconResSet(iconSet), invert);
   }
   public static Dictionary<String, Bitmap> CreateIconSetBitmaps(ResourceSet resSet, Boolean invert)
   {
      Dictionary<String, Bitmap> icons = new Dictionary<String, Bitmap>();

      if (resSet != null)
      {
         foreach (System.Collections.DictionaryEntry e in resSet)
         {
            if (e.Key is String && e.Value is Bitmap)
            {
               Bitmap b = (Bitmap)e.Value;
               if (invert)
               {
                  BitmapProcessing.Desaturate(b);
                  BitmapProcessing.Invert(b);
                  BitmapProcessing.AdjustBrightness(b, 101);
               }

               Bitmap b_hidden = new Bitmap(b);
               BitmapProcessing.AdjustOpacity(b_hidden, 100);

               Bitmap b_filtered = new Bitmap(b);
               BitmapProcessing.AdjustOpacity(b_filtered, Outliner.Controls.Tree.TreeNode.FilteredNodeOpacity);

               icons.Add((String)e.Key, b);
               icons.Add((String)e.Key + "_hidden", b_hidden);
               icons.Add((String)e.Key + "_filtered", b_filtered);
            }
         }
      }

      return icons;
   }

}
}
