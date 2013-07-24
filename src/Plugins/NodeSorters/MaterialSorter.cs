using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Modes;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Sorter_MaterialSorter")]
   public class MaterialSorter : NodeSorter
   {
      protected override int InternalCompare(IMaxNode nodeX, IMaxNode nodeY)
      {
         INodeWrapper inodeX = nodeX as INodeWrapper;
         if (inodeX == null)
            return 0;

         INodeWrapper inodeY = nodeY as INodeWrapper;
         if (inodeY == null)
            return 0;

         IMtl materialX = inodeX.INode.Mtl;
         IMtl materialY = inodeY.INode.Mtl;

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
