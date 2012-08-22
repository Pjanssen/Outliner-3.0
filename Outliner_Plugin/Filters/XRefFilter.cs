using System;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Modes;
using MaxUtils;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_XRef")]
   [LocalizedDisplayImage(typeof(Outliner.Controls.TreeIcons_Max), "xref")]
   [FilterCategory(FilterCategories.Classes)]
   public class XRefFilter : Filter<IMaxNodeWrapper>
   {
      override public Boolean ShowNode(IMaxNodeWrapper data)
      {
         IINodeWrapper iinodeWrapper = data as IINodeWrapper;
         if (iinodeWrapper == null)
            return false;

         return IINodeHelpers.IsXref(iinodeWrapper.IINode);
      }
   }
}
