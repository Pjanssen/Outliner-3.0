using System;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Modes;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Bones")]
   public class BoneFilter : Filter<IMaxNode>
   {
      protected override Boolean ShowNodeInternal(IMaxNode data)
      {
         INodeWrapper iinodeWrapper = data as INodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return IINodeHelpers.IsBone(iinodeWrapper.INode);
      }
   }
}
