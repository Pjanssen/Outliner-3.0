using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Nurbs")]
   public class NurbsFilter : Filter<IMaxNode>
   {
      protected override Boolean ShowNodeInternal(IMaxNode data)
      {
         INodeWrapper iinodeWrapper = data as INodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return iinodeWrapper.SuperClassID == SClass_ID.Geomobject && iinodeWrapper.INode.ObjectRef.IsShapeObject;
      }
   }
}
