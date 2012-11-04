using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   public abstract class SuperClassFilter : Filter<IMaxNodeWrapper>
   {
      protected Boolean ShowNodeInternal(IMaxNodeWrapper data, SClass_ID superClass)
      {
         if (data == null)
            return false;

         return data.SuperClassID == superClass;
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Shapes")]
   [LocalizedDisplayImage(typeof(Resources), "shape")]
   [FilterCategory(FilterCategories.Classes)]
   public class ShapeFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.Shape);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Cameras")]
   [LocalizedDisplayImage(typeof(Resources), "camera")]
   [FilterCategory(FilterCategories.Classes)]
   public class CameraFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.Camera);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Lights")]
   [LocalizedDisplayImage(typeof(Resources), "light")]
   [FilterCategory(FilterCategories.Classes)]
   public class LightFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.Light);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Spacewarps")]
   [LocalizedDisplayImage(typeof(Resources), "spacewarp")]
   [FilterCategory(FilterCategories.Classes)]
   public class SpacewarpFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.WsmObject);
      }
   }
}
