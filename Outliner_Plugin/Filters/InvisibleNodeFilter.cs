using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Filters
{
   /// <summary>
   /// Filters out all nodes that should not show up in the Outliner.
   /// For example, particle helpers, the 3dxConnection camera, etc.
   /// </summary>
   public class InvisibleNodeFilter : Filter<IMaxNodeWrapper>
   {
      public override bool OverrideEnabled
      {
         get { return true; }
         set { }
      }

      public override FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return FilterResults.Hide;

         IINodeWrapper inodeWrapper = data as IINodeWrapper;
         if (inodeWrapper == null)
            return FilterResults.Show;

         if (!inodeWrapper.IsValid || HelperMethods.IsInvisibleNode(inodeWrapper.IINode))
            return FilterResults.Hide;
         else
            return FilterResults.Show;
      }
   }
}
