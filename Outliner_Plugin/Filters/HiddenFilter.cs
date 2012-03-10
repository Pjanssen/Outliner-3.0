using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class HiddenFilter : NodeFilter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (data.IsHidden)
            return FilterResult.Hide;
         else
            return FilterResult.Show;

         /*
          if (!(n is IDisplayable))
              return FilterResult.Show;

          IDisplayable layer = null; //TODO: n.Layer as IDisplayable;

          if (((IDisplayable)n).IsHidden || (layer != null && layer.IsHidden))
              return FilterResult.Hide;
          else
              return FilterResult.Show;
          */
      }
   }
}