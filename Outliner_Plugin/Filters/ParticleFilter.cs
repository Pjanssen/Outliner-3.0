using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class ParticleFilter : NodeFilter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResult.Show;

         IINode node = (IINode)data.WrappedNode;
         if (node.ObjectRef.IsParticleSystem)
            return FilterResult.Hide;
         else
            return FilterResult.Show;
      }
   }
}
