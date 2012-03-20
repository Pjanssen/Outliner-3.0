using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class HiddenFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (data == null)
            return FilterResults.Hide;

         if (data.IsHidden)
            return FilterResults.Hide;
         else
            return FilterResults.Show;
      }
   }
}