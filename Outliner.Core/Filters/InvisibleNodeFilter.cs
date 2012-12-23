using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   /// <summary>
   /// Filters out all nodes that should not show up in the Outliner.
   /// For example, particle helpers, the 3dxConnection camera, etc.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_InvisibleNodes")]
   public class InvisibleNodeFilter : Filter<IMaxNodeWrapper>
   {
      protected override Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         if (data == null || !data.IsValid)
            return false;

         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return true;

         return !IINodeHelpers.IsInvisibleNode(iinodeWrapper.IINode);
      }
   }
}
