using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Modes.MaterialMode
{
   public class UnassignedMaterialFilter : Filter<IMaxNodeWrapper>
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return true;
         //if (!(n is OutlinerMaterial))
         //   return FilterResult.Show;

         //if (((OutlinerMaterial)n).IsUnassigned && ((OutlinerMaterial)n).ChildNodesCount == 0)
         //   return FilterResult.Hide;
         //else
         //   return FilterResult.Show;
      }
   }
}