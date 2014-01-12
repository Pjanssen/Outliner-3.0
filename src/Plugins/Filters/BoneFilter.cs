using System;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
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

         return IINodes.IsBone(iinodeWrapper.INode);
      }
   }
}
