using Autodesk.Max;

namespace Outliner.Controls.Filters
{
   // TODO : Decide if this filter is necessary.
   /*
    public class UnassignedMaterialFilter : NodeFilter<IINode>
    {
        override public FilterResult ShowNode(IINode data)
        {
            if (!(n is OutlinerMaterial))
                return FilterResult.Show;

            if (((OutlinerMaterial)n).IsUnassigned && ((OutlinerMaterial)n).ChildNodesCount == 0)
                return FilterResult.Hide;
            else
                return FilterResult.Show;
        }
   }
   */
}