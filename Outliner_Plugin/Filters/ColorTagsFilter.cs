using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_ColorTag")]
   [LocalizedDisplayImage(typeof(OutlinerResources), "color_small")]
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

         return (data.ColorTag & this.tags) != 0;
      }
   }
}
