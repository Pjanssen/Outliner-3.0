using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Filter_Layers")]
   //[LocalizedDisplayImage(typeof(Outliner.Controls.TreeIcons_Max), "bone")]
   [FilterCategory(FilterCategories.Other)]
   public class LayerFilter : Filter<IMaxNodeWrapper>
   {
      protected override bool ShowNodeInternal(IMaxNodeWrapper data)
      {
         return !(data is IINodeWrapper);
      }
   }
}
