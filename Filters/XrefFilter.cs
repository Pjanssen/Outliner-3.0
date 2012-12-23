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
   public class XRefFilter : Filter<IMaxNodeWrapper>
   {
      override protected Boolean ShowNodeInternal(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return IINodeHelpers.IsXref(iinodeWrapper.IINode);
      }
   }
}
