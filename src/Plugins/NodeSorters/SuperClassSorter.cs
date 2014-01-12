using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Modes;

namespace PJanssen.Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Sorter_SuperClass")]
   public class SuperClassSorter : NodeSorter
   {
      public SuperClassSorter() : base() { }
      public SuperClassSorter(SortOrder sortOrder) : base(sortOrder) { }

      protected override int InternalCompare(IMaxNode nodeX, IMaxNode nodeY)
      {
         return nodeX.SuperClassID.CompareTo(nodeY.SuperClassID);
      }
   }
}
