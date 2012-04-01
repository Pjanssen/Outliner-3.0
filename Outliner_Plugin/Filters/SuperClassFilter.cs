using System;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Filters
{
   public abstract class SuperClassFilter : Filter<IMaxNodeWrapper>
   {
      protected FilterResults ShowNode(IMaxNodeWrapper data, SClass_ID superClass)
      {
         if (data == null)
            return FilterResults.Show;

         if (data.SuperClassID != superClass)
            return FilterResults.Show;
         else
            return FilterResults.Hide;
      }
   }

   public class ShapeFilter : SuperClassFilter
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Shape);
      }
   }

   public class CameraFilter : SuperClassFilter
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Camera);
      }
   }

   public class LightFilter : SuperClassFilter
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Light);
      }
   }

   public class SpacewarpFilter : SuperClassFilter
   {
      override public FilterResults ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.WsmObject);
      }
   }
}
