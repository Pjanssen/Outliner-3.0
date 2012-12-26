using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.NodeSorters;

namespace Outliner.Configuration
{
   public interface ISorterConfiguration
   {
      NodeSorter Sorter { get; set; }
   }
}
