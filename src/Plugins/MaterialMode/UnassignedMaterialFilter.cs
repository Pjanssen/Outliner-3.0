using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Modes.Material
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