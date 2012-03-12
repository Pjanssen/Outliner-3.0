using Outliner.Scene;
using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.TreeModes;

namespace Outliner.Filters
{
    public class BoneFilter : Filter<IMaxNodeWrapper>
    {
        override public FilterResult ShowNode(IMaxNodeWrapper data)
        {
           if (data == null || !(data is IINodeWrapper))
              return FilterResult.Show;

           if (HelperMethods.IsBone((IINode)data.WrappedNode))
              return FilterResult.Hide;
           else
              return FilterResult.Show;
        }
    }

}
