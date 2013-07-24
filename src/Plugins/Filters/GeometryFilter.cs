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
   public class GeometryFilter : Filter<IMaxNode>
   {
      override protected Boolean ShowNodeInternal(IMaxNode data)
      {
         INodeWrapper iinodeWrapper = data as INodeWrapper;
         if (iinodeWrapper == null)
            return false;

         if (iinodeWrapper.SuperClassID != SClass_ID.Geomobject)
            return false;

         IINode iinode = iinodeWrapper.INode;
         if (iinode.IsTarget || IINodes.IsBone(iinode)
                             || iinode.ObjectRef.IsParticleSystem)
            return false;

         return true;
      }
   }
}
