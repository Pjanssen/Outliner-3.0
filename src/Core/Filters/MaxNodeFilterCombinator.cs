using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Plugins;

namespace PJanssen.Outliner.Filters
{
   /// <summary>
   /// The MaxNodeFilterCombinator provides a bound parameter version of the generic 
   /// FilterCombinator class, allowing serialization.
   /// </summary>
   [OutlinerPlugin(OutlinerPluginType.Filter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Filter_Combinator")]
   public class MaxNodeFilterCombinator : FilterCombinator<IMaxNode> 
   {
      public MaxNodeFilterCombinator() : base() { }
   }
}
