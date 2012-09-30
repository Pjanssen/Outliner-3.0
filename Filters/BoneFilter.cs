using System;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Modes;
using MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Bones")]
   [LocalizedDisplayImage(typeof(Resources), "bone")]
   [FilterCategory(FilterCategories.Classes)]
   public class BoneFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return IINodeHelpers.IsBone(iinodeWrapper.IINode);
      }
   }
}
