using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Outliner.NodeSorters;
using System.Xml.Serialization;

namespace Outliner.Configuration
{
   public class SorterConfiguration : ConfigurationFile
   {
      protected override string ImageBasePath
      {
         get { return OutlinerPaths.SortersDir; }
      }

      [XmlElement("sorter")]
      public NodeSorter Sorter { get; set; }
   }
}
