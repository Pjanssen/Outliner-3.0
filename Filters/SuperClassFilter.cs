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

         return data.SuperClassID.Equals(superClass);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Shapes")]
   public class ShapeFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.Shape);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Cameras")]
   public class CameraFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.Camera);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Lights")]
   public class LightFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.Light);
      }
   }

   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Spacewarps")]
   public class SpacewarpFilter : SuperClassFilter
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         return base.ShowNodeInternal(data, SClass_ID.WsmObject);
      }
   }
}
