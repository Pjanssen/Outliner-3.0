using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;
using Outliner.TreeModes;

namespace Outliner.Filters
{
   public class XRefFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResult.Show;

         if(HelperMethods.IsXref((IINode)data.WrappedNode))
            return FilterResult.Hide;
         else
            return FilterResult.Show;
      }
   }
}
