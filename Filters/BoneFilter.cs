using System;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Modes;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Bones")]
   [LocalizedDisplayImage(typeof(Resources), "bone")]
   [FilterCategory(FilterCategory.Classes)]
   public class BoneFilter : Filter<IMaxNodeWrapper>
   {
      protected override Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return IINodeHelpers.IsBone(iinodeWrapper.IINode);
      }
   }
}
