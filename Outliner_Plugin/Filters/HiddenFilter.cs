using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Hidden")]
   [FilterCategory(FilterCategories.Properties)]
   public class HiddenFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return false;

         return !data.IsHidden;
      }
   }
}