using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Outliner.Controls;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Configuration
{
   public class FilterConfiguration : ConfigurationFile
   {
      public FilterConfiguration() : base()
      {
         this.Category = FilterCategory.Custom;
      }

      protected override string ImageBasePath
      {
         get { return OutlinerPaths.FiltersDir; }
      }

      [XmlElement("FilterCategory")]
      public FilterCategory Category { get; set; }

      [XmlElement("Filter")]
      public Filter<IMaxNodeWrapper> Filter { get; set; }
   }
}
