using Autodesk.Max;
using Outliner.Scene;
using Outliner.Modes;
using MaxUtils;

namespace Outliner.Filters
{
   public class XRefFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResults.Show;

         if (IINodeHelpers.IsXref((IINode)data.WrappedNode))
            return FilterResults.Hide;
         else
            return FilterResults.Show;
      }
   }
}
