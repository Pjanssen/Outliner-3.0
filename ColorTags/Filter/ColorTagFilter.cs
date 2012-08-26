using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Plugins;
using Outliner.Filters;

namespace Outliner.ColorTags
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_DisplayName")]
   [LocalizedDisplayImage(typeof(Resources), "color_small")]
   [FilterCategory(FilterCategories.Properties)]
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

      public override Boolean ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return false;

         return (data.GetColorTag() & this.tags) != 0;
      }
   }
}
