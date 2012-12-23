using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Modes;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Geometry")]
   public class GeometryFilter : Filter<IMaxNodeWrapper>
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
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
