using System;
using Autodesk.Max;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Xrefs")]
   public class XRefFilter : Filter<IMaxNode>
   {
      override protected Boolean ShowNodeInternal(IMaxNode data)
      {
         INodeWrapper iinodeWrapper = data as INodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return IINodes.IsXref(iinodeWrapper.INode);
      }
   }
}
