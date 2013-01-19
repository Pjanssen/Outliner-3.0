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

         INodeWrapper nodeX = HelperMethods.GetMaxNode(x) as INodeWrapper;
         if (nodeX == null || !nodeX.IsValid)
            return 0;

         INodeWrapper nodeY = HelperMethods.GetMaxNode(y) as INodeWrapper;
         if (nodeY == null || !nodeY.IsValid)
            return 0;

         IMtl materialX = nodeX.INode.Mtl;
         IMtl materialY = nodeY.INode.Mtl;

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
