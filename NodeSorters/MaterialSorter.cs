using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Sorter_MaterialSorter")]
   public class MaterialSorter : NodeSorter
   {
      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IINodeWrapper nodeX = HelperMethods.GetMaxNode(x) as IINodeWrapper;
         if (nodeX == null || !nodeX.IsValid)
            return 0;

         IINodeWrapper nodeY = HelperMethods.GetMaxNode(y) as IINodeWrapper;
         if (nodeY == null || !nodeY.IsValid)
            return 0;

         IMtl materialX = nodeX.IINode.Mtl;
         IMtl materialY = nodeY.IINode.Mtl;

         if (materialX == materialY)
            return 0;
         else if (materialX == null)
            return 1;
         else if (materialY == null)
            return -1;
         else
            return NativeMethods.StrCmpLogicalW(materialX.Name, materialY.Name);
      }
   }
}
