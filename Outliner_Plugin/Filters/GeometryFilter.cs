using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;
using Outliner.TreeModes;

namespace Outliner.Filters
{
   public class GeometryFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResult.Show;

         if (data.SuperClassID != SClass_ID.Geomobject)
            return FilterResult.Show;

         IINode node = (IINode)data.WrappedNode;
         if (node.IsTarget || HelperMethods.IsBone(node) 
                           || node.ObjectRef.IsParticleSystem)
            return FilterResult.Show;
         else
            return FilterResult.Hide;
      }
   }
}
