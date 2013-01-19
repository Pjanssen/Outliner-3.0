using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.Filters
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
