using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;
using Outliner.TreeModes;

namespace Outliner.Filters
{
   public class HelperFilter : NodeFilter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (data.SuperClassID == SClass_ID.Helper)
            return FilterResult.Hide;
         else if (data is IINodeWrapper && data.WrappedNode != null)
         {
            IINode node = (IINode)data.WrappedNode;
            if (node != null && node.IsTarget)
               return FilterResult.Hide;
         }

         return FilterResult.Show;
      }
   }
}
