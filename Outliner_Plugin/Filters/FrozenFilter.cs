using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Frozen")]
   [LocalizedDisplayImage(typeof(OutlinerResources), "button_freeze")]
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