using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Particles")]
   [LocalizedDisplayImage(typeof(Resources), "particle")]
   [FilterCategory(FilterCategories.Classes)]
   public class ParticleFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return iinodeWrapper.IINode.ObjectRef.IsParticleSystem;
      }
   }
}
