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
   public class LayerFilter : Filter<IMaxNode>
   {
      protected override bool ShowNodeInternal(IMaxNode data)
      {
         return !(data is INodeWrapper);
      }
   }
}
