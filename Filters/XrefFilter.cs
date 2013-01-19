using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Modes;
using Outliner.MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
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

         return IINodeHelpers.IsXref(iinodeWrapper.INode);
      }
   }
}
