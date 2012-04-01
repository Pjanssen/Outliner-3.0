using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class HelperFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return FilterResults.Hide;

         if (data.SuperClassID == SClass_ID.Helper)
            return FilterResults.Hide;
         else if (data is IINodeWrapper && data.WrappedNode != null)
         {
            IINode node = (IINode)data.WrappedNode;
            if (node != null && node.IsTarget)
               return FilterResults.Hide;
         }

         return FilterResults.Show;
      }
   }
}
