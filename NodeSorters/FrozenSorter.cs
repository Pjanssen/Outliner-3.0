using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.MaxUtils;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Frozen_DisplayName")]
   [LocalizedDisplayImage(typeof(Resources), "frozen_16", "frozen_24")]
   public class FrozenSorter : NodePropertySorter
   {
      public FrozenSorter() : base(NodeProperty.IsFrozen, false) { }
   }
}
