using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;
using Outliner.TreeModes;

namespace Outliner.Filters
{
   public class XRefFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResults.Show;

         if(HelperMethods.IsXref((IINode)data.WrappedNode))
            return FilterResults.Hide;
         else
            return FilterResults.Show;
      }
   }
}
