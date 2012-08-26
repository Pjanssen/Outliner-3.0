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

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Shapes")]
   //[LocalizedDisplayImage(typeof(Outliner.Controls.TreeIcons_Max), "shape")]
   [FilterCategory(FilterCategories.Classes)]
   public class ShapeFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Shape);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Cameras")]
   //[LocalizedDisplayImage(typeof(Outliner.Controls.TreeIcons_Max), "camera")]
   [FilterCategory(FilterCategories.Classes)]
   public class CameraFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Camera);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Lights")]
   //[LocalizedDisplayImage(typeof(Outliner.Controls.TreeIcons_Max), "light")]
   [FilterCategory(FilterCategories.Classes)]
   public class LightFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.Light);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Spacewarps")]
   //[LocalizedDisplayImage(typeof(Outliner.Controls.TreeIcons_Max), "spacewarp")]
   [FilterCategory(FilterCategories.Classes)]
   public class SpacewarpFilter : SuperClassFilter
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         return base.ShowNode(data, SClass_ID.WsmObject);
      }
   }
}
