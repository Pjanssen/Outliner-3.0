using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class ParticleFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResults.Show;

         IINode node = (IINode)data.WrappedNode;
         if (node.ObjectRef.IsParticleSystem)
            return FilterResults.Hide;
         else
            return FilterResults.Show;
      }
   }
}
