using System;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Helpers")]
   public class HelperFilter : Filter<IMaxNode>
   {
      override protected Boolean ShowNodeInternal(IMaxNode data)
      {
         INodeWrapper iinodeWrapper = data as INodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return iinodeWrapper.SuperClassID == SClass_ID.Helper || iinodeWrapper.INode.IsTarget;
      }
   }
}
