using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Helpers")]
   [FilterCategory(FilterCategories.Classes)]
   public class HelperFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return iinodeWrapper.SuperClassID == SClass_ID.Helper || iinodeWrapper.IINode.IsTarget;
      }
   }
}
