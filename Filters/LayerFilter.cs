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
      public override bool ShowNode(IMaxNodeWrapper data)
      {
         return data is IILayerWrapper;
      }
   }
}
