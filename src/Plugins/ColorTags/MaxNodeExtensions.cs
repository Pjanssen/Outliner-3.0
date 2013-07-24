using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.ColorTags
{
   public static class MaxNodeExtensions
   {
      public static ColorTag GetColorTag(this IMaxNode node)
      {
         if (node.IsAggregate)
            return node.ChildNodes.GetColorTag();
         else
            return ColorTags.GetTag(node.BaseObject as IAnimatable);
      }

      public static void SetColorTag(this IMaxNode node, ColorTag tag)
      {
         if (node.IsAggregate)
            node.ChildNodes.SetColorTag(tag);
         else
            ColorTags.SetTag(node.BaseObject as IAnimatable, tag);
      }


      public static ColorTag GetColorTag(this IEnumerable<IMaxNode> nodes)
      {
         if (nodes.Count() == 0)
            return ColorTag.None;
         
         return nodes.Select(n => n.BaseObject)
                     .Where(n => n is IAnimatable)
                     .Aggregate( ColorTag.All
                               , (tag, n) => tag &= ColorTags.GetTag((IAnimatable)n));
      }

      public static void SetColorTag(this IEnumerable<IMaxNode> nodes, ColorTag tag)
      {
         nodes.Select(n => n.BaseObject)
              .Where(n => n is IAnimatable)
              .ForEach(n => ColorTags.SetTag((IAnimatable)n, tag));
      }
   }
}
