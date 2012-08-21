using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Nurbs")]
   [FilterCategory(FilterCategories.Classes)]
   public class NurbsFilter : Filter<IMaxNodeWrapper>
   {
      public override Boolean ShowNode(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return iinodeWrapper.SuperClassID == SClass_ID.Geomobject && iinodeWrapper.IINode.ObjectRef.IsShapeObject;
      }
   }
}
