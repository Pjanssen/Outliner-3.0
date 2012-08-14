using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using MaxUtils;
using System.Drawing;

namespace Outliner.LayerTools
{
   public class ColorTagsMXS
   {
      internal static void Start(IGlobal global)
      {
         HelperMethods.RunResourceScript(typeof(ColorTags).Assembly,
                                         "Outliner.LayerTools.ColorTags.ColorTags.ms");
      }

      private static IAnimatable getNode(UInt64 handle)
      {
         IGlobal.IGlobalAnimatable anim = MaxInterfaces.Global.Animatable;
         return anim.GetAnimByHandle(new UIntPtr(handle));
      }

      public static Boolean HasTag(UInt64 nodeHandle)
      {
         IAnimatable node = getNode(nodeHandle);
         return ColorTags.HasTag(node);
      }

      public static byte GetTag(UInt64 nodeHandle)
      {
         IAnimatable node = getNode(nodeHandle);
         ColorTag tag = ColorTags.GetTag(node);
         
         IEnumerable<ColorTag> tags = Enum.GetValues(typeof(ColorTag)).Cast<ColorTag>();
         byte index = 0;
         foreach (ColorTag colorTag in tags)
         {
            if (colorTag == tag)
               return index;

            index++;
         }

         return 0;
      }

      public static Color GetColor(UInt64 nodeHandle)
      {
         IAnimatable node = getNode(nodeHandle);
         return ColorTags.GetColor(node);
      }

      public static void SetTag(UInt64 nodeHandle, byte tagIndex)
      {
         IAnimatable node = getNode(nodeHandle);
         ColorTag tag = (ColorTag)Enum.GetValues(typeof(ColorTag)).GetValue(tagIndex);
         ColorTags.SetTag(node, tag);
      }

      public static void RemoveTag(UInt64 nodeHandle)
      {
         IAnimatable node = getNode(nodeHandle);
         ColorTags.RemoveTag(node);
      }
   }
}
