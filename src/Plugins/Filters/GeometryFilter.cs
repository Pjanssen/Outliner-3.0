using System;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
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
