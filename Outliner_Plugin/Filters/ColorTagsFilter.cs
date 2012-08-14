using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;

namespace Outliner.Filters
{
   public class ColorTagsFilter : Filter<IMaxNodeWrapper>
   {
      public ColorTag Tags { get; set; }

      public ColorTagsFilter() : this(ColorTag.None) { }
      public ColorTagsFilter(ColorTag tags)
      {
         this.Tags = tags;
      }

      public override FilterResults ShowNode(IMaxNodeWrapper data)
      {
         IAnimatable anim = data.WrappedNode as IAnimatable;
         if (anim == null)
            return FilterResults.Hide;

         if (ColorTags.HasTag(anim, this.Tags))
            return FilterResults.Show;
         else
            return FilterResults.Hide;
      }
   }
}
