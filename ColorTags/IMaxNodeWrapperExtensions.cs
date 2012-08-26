using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.ColorTags
{
   public static class IMaxNodeWrapperExtensions
   {
      public static ColorTag GetColorTag(this IMaxNodeWrapper wrapper)
      {
         return ColorTags.GetTag(wrapper.WrappedNode as IAnimatable);
      }

      public static void SetColorTag(this IMaxNodeWrapper wrapper, ColorTag tag)
      {
         ColorTags.SetTag(wrapper.WrappedNode as IAnimatable, tag);
      }


      public static ColorTag GetColorTag(this SelectionSetWrapper wrapper)
      {
         IEnumerable<IINode> childIINodes = wrapper.ChildIINodes;
         if (childIINodes.Count() == 0)
            return ColorTag.None;
         else
            return childIINodes.Aggregate(ColorTag.All, (tag, n) =>
                                          tag &= ColorTags.GetTag(n));
      }

      public static void SetColorTag(this SelectionSetWrapper wrapper, ColorTag tag)
      {
         wrapper.ChildIINodes.ForEach(n => ColorTags.SetTag(n, tag));
      }
   }
}
