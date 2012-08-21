using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Plugins;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin]
   [LocalizedDisplayName(typeof(OutlinerResources), "Sort_Alphabetical")]
   public class AlphabeticalSorter : NodeSorter
   {
      public AlphabeticalSorter() : base() { }
      public AlphabeticalSorter(Boolean invert) : base(invert) { }

      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
      }
   }
}
