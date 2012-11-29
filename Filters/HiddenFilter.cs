using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;
using Outliner.MaxUtils;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Hidden")]
   public class HiddenFilter : NodePropertyFilter
   {
      public HiddenFilter() : base(BooleanNodeProperty.IsHidden)
      {
         this.Invert = true;
      }
   }
}