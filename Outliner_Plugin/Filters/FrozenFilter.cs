using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.FiltersBase;

namespace Outliner.Filters
{
   public class FrozenFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return FilterResult.Show;

         if (data.IsFrozen)
            return FilterResult.Hide;
         else
            return FilterResult.Show;
      }
   }
}