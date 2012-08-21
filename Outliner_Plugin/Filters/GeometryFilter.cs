using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Modes;
using MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Geometry")]
   [FilterCategory(FilterCategories.Classes)]
   //[FilterImage(OutlinerResources.delete_small)]
   public class GeometryFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         if (iinodeWrapper.SuperClassID != SClass_ID.Geomobject)
            return false;

         IINode iinode = iinodeWrapper.IINode;
         if (iinode.IsTarget || IINodeHelpers.IsBone(iinode)
                             || iinode.ObjectRef.IsParticleSystem)
            return false;

         return true;
      }
   }
}
