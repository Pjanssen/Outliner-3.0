using System;
using Autodesk.Max;
using Outliner.Controls.FiltersBase;
using Outliner.Scene;

namespace Outliner.Filters
{
   public abstract class SuperClassFilter : NodeFilter<IMaxNodeWrapper>
   {
      protected FilterResult ShowNode(IMaxNodeWrapper data, SClass_ID superClass)
      {
         if (data.SuperClassID != superClass)
            return FilterResult.Show;
         else
            return FilterResult.Hide;

         /*
          if (!(n is OutlinerObject))
              return FilterResult.Show;

          if (((OutlinerObject)n).SuperClass == superClass)
              return FilterResult.Hide;
          else
              return FilterResult.Show;
          */
      }
   }

   public class ShapeFilter : SuperClassFilter
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Shape);
      }
   }

   public class CameraFilter : SuperClassFilter
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Camera);
      }
   }

   public class LightFilter : SuperClassFilter
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Light);
      }
   }

   public class SpacewarpFilter : SuperClassFilter
   {
      override public FilterResult ShowNode(IMaxNodeWrapper data)
      {
         return FilterResult.Show;
         // TODO: implement.
         //return base.ShowNode(data, SClass_ID);
      }
   }
}
