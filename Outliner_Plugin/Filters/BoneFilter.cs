using Outliner.Scene;
using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.TreeModes;

namespace Outliner.Filters
{
    public class BoneFilter : Filter<IMaxNodeWrapper>
    {
        override public FilterResults ShowNode(IMaxNodeWrapper data)
        {
           if (data == null || !(data is IINodeWrapper))
              return FilterResults.Show;

           if (HelperMethods.IsBone((IINode)data.WrappedNode))
              return FilterResults.Hide;
           else
              return FilterResults.Show;
        }
    }

}
