using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.Filters
{
   public class NurbsFilter : Filter<IMaxNodeWrapper>
   {
      public override FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResults.Show;

         if (data.SuperClassID == Autodesk.Max.SClass_ID.Geomobject)
         {
            IINode node = (IINode)data.WrappedNode;
            if (node.ObjectRef != null && node.ObjectRef.IsShapeObject)
               return FilterResults.Hide;
         }

         return FilterResults.Show;
      }
   }
}
