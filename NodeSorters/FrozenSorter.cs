using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using MaxUtils;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Frozen_DisplayName")]
   [LocalizedDisplayImage(typeof(Resources), "frozen_16", "frozen_24")]
   public class FrozenSorter : AnimatablePropertySorter
   {
      public FrozenSorter() : base(AnimatableProperty.IsFrozen, false) { }
   }
}
