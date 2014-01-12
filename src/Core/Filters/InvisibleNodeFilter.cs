using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
{
   /// <summary>
   /// Filters out all nodes that should not show up in the Outliner.
   /// For example, particle helpers, the 3dxConnection camera, etc.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_InvisibleNodes")]
   public class InvisibleNodeFilter : Filter<IMaxNode>
   {
      protected override Boolean ShowNodeInternal(IMaxNode data)
      {
         if (data == null || !data.IsValid)
            return false;

         INodeWrapper iinodeWrapper = data as INodeWrapper;
         if (iinodeWrapper == null)
            return true;

         return !IINodes.IsInvisibleNode(iinodeWrapper.INode);
      }
   }
}
