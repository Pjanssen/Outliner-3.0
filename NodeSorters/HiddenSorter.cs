using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using MaxUtils;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Hidden_DisplayName")]
   [LocalizedDisplayImage(typeof(Resources), "hidden_16", "hidden_24")]
   public class HiddenSorter : AnimatablePropertySorter
   {
      public HiddenSorter() : base(AnimatableProperty.IsHidden, false) { }
   }
}
