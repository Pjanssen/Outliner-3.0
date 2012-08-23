using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.Plugins;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(OutlinerResources), "Sort_SuperClass")]
   public class SuperClassSorter : NodeSorter
   {
      public SuperClassSorter() : base() { }
      public SuperClassSorter(Boolean invert) : base(invert) { }

      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         SClass_ID sClassIDX = nodeX.SuperClassID;
         SClass_ID sClassIDY = nodeY.SuperClassID;

         if (sClassIDX == sClassIDY)
            return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
         else
            return (int)(sClassIDX - sClassIDY);
      }
   }
}
