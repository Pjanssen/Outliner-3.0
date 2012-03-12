using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Controls.FiltersBase;
using Autodesk.Max;

namespace Outliner.Filters
{
   public class NurbsFilter : Filter<IMaxNodeWrapper>
   {
      public override FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResult.Show;

         if (data.SuperClassID == Autodesk.Max.SClass_ID.Geomobject)
         {
            IINode node = (IINode)data.WrappedNode;
            if (node.ObjectRef != null && node.ObjectRef.IsShapeObject)
               return FilterResult.Hide;
         }

         return FilterResult.Show;
      }
   }
}
