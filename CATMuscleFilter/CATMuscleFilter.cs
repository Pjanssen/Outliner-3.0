using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Filters;
using Outliner.Plugins;
using Autodesk.Max;

namespace Outliner.Filters.CATMuscleFilter
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_CATMuscleFilter")]
   public class CATMuscleFilter : Filter<IMaxNodeWrapper>
   {
      private const uint strandIDA = 0x7CF3882;
      private const uint strandIDB = 0x47F01642;
      private const uint handleIDA = 0x141E37A3;
      private const uint handleIDB = 0x2B362406;

      protected override bool ShowNodeInternal(IMaxNodeWrapper data)
      {
         if (data == null)
            return false;

         return MaxUtils.ClassIDHelpers.IsClass(data.WrappedNode, strandIDA, strandIDB) ||
                MaxUtils.ClassIDHelpers.IsClass(data.WrappedNode, handleIDA, handleIDB);
      }
   }
}
