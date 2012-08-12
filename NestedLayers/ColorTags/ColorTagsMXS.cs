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
         return ColorTags.GetTag(node);
      }

      public static Color GetColor(UInt64 nodeHandle)
      {
         IAnimatable node = getNode(nodeHandle);
         return ColorTags.GetColor(node);
      }

      public static void SetTag(UInt64 nodeHandle, byte tag)
      {
         IAnimatable node = getNode(nodeHandle);
         ColorTags.SetTag(node, tag);
      }

      public static void RemoveTag(UInt64 nodeHandle)
      {
         IAnimatable node = getNode(nodeHandle);
         ColorTags.RemoveTag(node);
      }
   }
}
