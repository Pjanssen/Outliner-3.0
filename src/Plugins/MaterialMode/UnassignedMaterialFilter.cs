using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Modes.MaterialMode
{
   public class UnassignedMaterialFilter : Filter<IMaxNode>
   {
      override protected Boolean ShowNodeInternal(IMaxNode data)
      {
         UnassignedMaterialWrapper unassignedMat = data as UnassignedMaterialWrapper;
         return unassignedMat == null || unassignedMat.ChildNodeCount > 0;
      }
   }
}