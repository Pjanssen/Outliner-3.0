using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Frozen")]
   [FilterCategory(FilterCategories.Properties)]
   public class FrozenFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return false;

         return !data.IsFrozen;
      }
   }
}