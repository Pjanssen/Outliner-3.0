using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Outliner.NodeSorters;
using System.Xml.Serialization;

namespace Outliner.Configuration
{
   public class SorterConfiguration : ConfigurationFile, ISorterConfiguration
   {
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

      [XmlElement("sorter")]
      public NodeSorter Sorter { get; set; }
   }
}
