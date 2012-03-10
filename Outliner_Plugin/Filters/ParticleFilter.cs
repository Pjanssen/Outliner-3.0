using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.Filters
{
   public class ParticleFilter : NodeFilter<IMaxNodeWrapper>
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         if (data.SuperClassID != SClass_ID.ParticleSys)
            return FilterResult.Show;
         else
            return FilterResult.Hide;

         /*
          if (!(n is OutlinerObject))
              return FilterResult.Show;

          OutlinerObject o = (OutlinerObject)n;

          if (o.SuperClass != MaxTypes.Geometry && o.SuperClass != MaxTypes.Helper)
              return FilterResult.Show;

          if (o.Class == MaxTypes.PfSource || o.Class == MaxTypes.PCloud || o.Class == MaxTypes.PArray || 
              o.Class == MaxTypes.PBlizzard || o.Class == MaxTypes.PSpray || o.Class == MaxTypes.PSuperSpray || 
              o.Class == MaxTypes.PSnow || o.Class == MaxTypes.PBirthTexture || o.Class == MaxTypes.PSpeedByIcon ||
              o.Class == MaxTypes.PGroupSelection || o.Class == MaxTypes.PFindTarget || 
              o.Class == MaxTypes.PInitialState || o.Class == MaxTypes.ParticlePaint)
          {
              return FilterResult.Hide;
          }
          else
              return FilterResult.Show;
          */
      }
   }
}
