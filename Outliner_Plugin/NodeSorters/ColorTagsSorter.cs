using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Outliner.LayerTools;
using Autodesk.Max;

namespace Outliner.NodeSorters
{
   public class ColorTagsSorter : NodeSorter
   {
      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         ColorTag tagX = ColorTags.GetTag(nodeX.WrappedNode as IAnimatable);
         ColorTag tagY = ColorTags.GetTag(nodeY.WrappedNode as IAnimatable);

         if (tagX == tagY)
            return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
         else if (tagX == ColorTag.None)
            return 1;
         else if (tagY == ColorTag.None)
            return -1;
         else
            return tagX.CompareTo(tagY);
      }
   }
}
