using System;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class FrozenFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return FilterResults.Show;

         if (data.IsFrozen)
            return FilterResults.Hide;
         else
            return FilterResults.Show;
      }
   }
}