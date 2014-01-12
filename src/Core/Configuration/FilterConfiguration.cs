using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Configuration
{
   /// <summary>
   /// Defines a Filter Configuration file.
   /// </summary>
   public class FilterConfiguration : ConfigurationFile
   {
      /// <summary>
      /// Initializes a new instance of the FilterConfiguration class.
      /// </summary>
      public FilterConfiguration() : base()
      {
         this.Category = FilterCategory.Custom;
      }

      protected override string ImageBasePath
      {
         get { return OutlinerPaths.FiltersDir; }
      }

      /// <summary>
      /// Gets or sets the UI category in which the filter will be shown.
      /// </summary>
      [XmlElement("FilterCategory")]
      public FilterCategory Category { get; set; }

      /// <summary>
      /// Gets or sets the filter in this configuration.
      /// </summary>
      [XmlElement("Filter")]
      public Filter<IMaxNode> Filter { get; set; }
   }
}
