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
      private ColorTag tags;

      public ColorTagsFilter() : this(ColorTag.All) { }
      public ColorTagsFilter(ColorTag tags)
      {
         this.tags = tags;
      }

      public ColorTag Tags 
      {
         get { return this.tags; }
         set
         {
            this.tags = value;
            this.OnFilterChanged();
         }
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
