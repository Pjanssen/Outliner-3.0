using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Plugins;
using Outliner.Modes;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Sorter_SuperClass")]
   public class SuperClassSorter : NodeSorter
   {
      public SuperClassSorter() : base() { }
      public SuperClassSorter(SortOrder sortOrder) : base(sortOrder) { }

      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNode nodeX = TreeMode.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNode nodeY = TreeMode.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         return nodeX.SuperClassID.CompareTo(nodeY.SuperClassID);
      }
   }
}
