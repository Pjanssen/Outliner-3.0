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
         if (data.SuperClassID != SClass_ID.Helper)
            return FilterResult.Show;

         if (HelperMethods.ClassIDEquals(data.ClassID, BuiltInClassIDA.TARGET_CLASS_ID))
            return FilterResult.Show;
         else
            return FilterResult.Hide;
      }
   }
}
