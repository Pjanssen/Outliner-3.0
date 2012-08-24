using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.MaterialMode
{
   public class UnassignedMaterialFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return false;
         //if (!(n is OutlinerMaterial))
         //   return FilterResult.Show;

         //if (((OutlinerMaterial)n).IsUnassigned && ((OutlinerMaterial)n).ChildNodesCount == 0)
         //   return FilterResult.Hide;
         //else
         //   return FilterResult.Show;
      }
   }
}