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
         return base.ShowNode(data, SClass_ID.WsmObject);
      }
   }
}
