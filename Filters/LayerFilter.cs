using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
{
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(Resources), "Str_Layers")]
   public class LayerFilter : Filter<IMaxNodeWrapper>
   {
      protected override bool ShowNodeInternal(IMaxNodeWrapper data)
      {
         return !(data is IINodeWrapper);
      }
   }
}
