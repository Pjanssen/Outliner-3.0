using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Outliner.NodeSorters;
using System.Xml.Serialization;

namespace Outliner.Configuration
{
   /// <summary>
   /// Defines a NodeSorter configuration file.
   /// </summary>
   public class SorterConfiguration : ConfigurationFile, ISorterConfiguration
   {
      /// <summary>
      /// Initializes a new instance of the SorterConfiguration class.
      /// </summary>
      public SorterConfiguration() { }

      protected override string ImageBasePath
      {
         get { return OutlinerPaths.SortersDir; }
      }

      protected override System.Drawing.Image Image16Default
      {
         get
         {
            return OutlinerResources.sort_default_16;
         }
      }

      protected override System.Drawing.Image Image24Default
      {
         get
         {
            return OutlinerResources.sort_default_24;
         }
      }

      /// <summary>
      /// Gets or sets the NodeSorter for this configuration.
      /// </summary>
      [XmlElement("sorter")]
      public NodeSorter Sorter { get; set; }
   }
}
