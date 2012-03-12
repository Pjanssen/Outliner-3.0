using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class HiddenFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (data.IsHidden)
            return FilterResult.Hide;
         else
            return FilterResult.Show;
      }
   }
}