using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_SuperClass")]
   public class SuperClassFilter : Filter<IMaxNodeWrapper>
   {
      public SuperClassFilter() { }
      public SuperClassFilter(SClass_ID superClass)
      {
         this.SuperClass = superClass;
      }

      public SClass_ID SuperClass { get; set; }
      protected override bool ShowNodeInternal(IMaxNodeWrapper data)
      {
         if (data == null)
            return false;

         return data.SuperClassID.Equals(this.SuperClass);
      }
   }
}
