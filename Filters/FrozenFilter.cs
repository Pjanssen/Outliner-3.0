using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;
using Outliner.MaxUtils;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Frozen")]
   public class FrozenFilter : NodePropertyFilter
   {
      public FrozenFilter() : base(BooleanNodeProperty.IsFrozen) 
      {
         this.Invert = true;
      }
   }
}