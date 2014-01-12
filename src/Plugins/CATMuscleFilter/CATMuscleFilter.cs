using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.Plugins;
using Autodesk.Max;

namespace PJanssen.Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_CATMuscles")]
   public class CATMuscleFilter : Filter<IMaxNode>
   {
      private const uint strandIDA = 0x7CF3882;
      private const uint strandIDB = 0x47F01642;
      private const uint handleIDA = 0x141E37A3;
      private const uint handleIDB = 0x2B362406;

      protected override bool ShowNodeInternal(IMaxNode data)
      {
         if (data == null)
            return false;

         return MaxUtils.ClassIDs.IsClass(data.BaseObject, strandIDA, strandIDB) ||
                MaxUtils.ClassIDs.IsClass(data.BaseObject, handleIDA, handleIDB);
      }
   }
}
