using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   public abstract class SuperClassFilter : Filter<IMaxNodeWrapper>
   {
      protected Boolean ShowNode(IMaxNodeWrapper data, SClass_ID superClass)
      {
         if (data == null)
            return false;

         return data.SuperClassID == superClass;
      }
   }

   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Shapes")]
   [FilterCategory(FilterCategories.Classes)]
   public class ShapeFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Shape);
      }
   }

   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Cameras")]
   [FilterCategory(FilterCategories.Classes)]
   public class CameraFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Camera);
      }
   }

   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Lights")]
   [FilterCategory(FilterCategories.Classes)]
   public class LightFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Light);
      }
   }

   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Spacewarps")]
   [FilterCategory(FilterCategories.Classes)]
   public class SpacewarpFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.WsmObject);
      }
   }
}
