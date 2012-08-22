using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Hidden")]
   [LocalizedDisplayImage(typeof(OutlinerResources), "button_hide")]
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