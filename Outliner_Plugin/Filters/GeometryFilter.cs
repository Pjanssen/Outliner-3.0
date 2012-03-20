using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;
using Outliner.TreeModes;

namespace Outliner.Filters
{
   public class GeometryFilter : Filter<IMaxNodeWrapper>
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         if (!(data is IINodeWrapper))
            return FilterResults.Show;

         if (data.SuperClassID != SClass_ID.Geomobject)
            return FilterResults.Show;

         IINode node = (IINode)data.WrappedNode;
         if (node.IsTarget || HelperMethods.IsBone(node) 
                           || node.ObjectRef.IsParticleSystem)
            return FilterResults.Show;
         else
            return FilterResults.Hide;
      }
   }
}
