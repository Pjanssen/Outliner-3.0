using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using MaxUtils;

namespace Outliner.Filters
{
   /// <summary>
   /// Filters out all nodes that should not show up in the Outliner.
   /// For example, particle helpers, the 3dxConnection camera, etc.
   /// </summary>
   public class InvisibleNodeFilter : Filter<IMaxNodeWrapper>
   {
      public override bool AlwaysEnabled
      {
         get { return true; }
      }

      public override Boolean ShowNode(IMaxNodeWrapper data)
      {
         if (data == null || !data.IsValid)
            return false;

         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return true;

         return !IINodeHelpers.IsInvisibleNode(iinodeWrapper.IINode);
      }
   }
}
