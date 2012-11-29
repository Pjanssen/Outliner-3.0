using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using Outliner.NodeSorters;
using System.Xml.Serialization;

namespace Outliner.UserFiles
{
   public class SorterConfiguration : UIItemModel
   {
      protected override string ImageBasePath
      {
         get { return OutlinerPaths.SortersDir; }
      }

      [XmlElement("Sorter")]
      public NodeSorter Sorter { get; set; }
   }
}
