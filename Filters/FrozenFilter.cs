using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;
using Outliner.MaxUtils;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Frozen")]
   [LocalizedDisplayImage(typeof(Resources), "freeze")]
   [FilterCategory(FilterCategories.Properties)]
   public class FrozenFilter : NodePropertyFilter
   {
      public FrozenFilter() : base(BooleanNodeProperty.IsFrozen) 
      {
         this.Invert = true;
      }
   }
}