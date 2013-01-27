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
   [LocalizedDisplayName(typeof(Resources), "Sorter_INodeHandle")]
   public class INodeHandleSorter : NodeSorter
   {
      public INodeHandleSorter() : base() { }
      public INodeHandleSorter(SortOrder sortOrder) : base(sortOrder) { }

      protected override int InternalCompare(TreeNode x, TreeNode y)
      {
         if (x == y)
            return 0;

         IMaxNode nodeX = TreeMode.GetMaxNode(x);
         if (nodeX == null || !nodeX.IsValid) return 0;

         IMaxNode nodeY = TreeMode.GetMaxNode(y);
         if (nodeY == null || !nodeY.IsValid) return 0;

         Boolean xIsIINodeWrapper = nodeX is INodeWrapper;
         Boolean yIsIINodeWrapper = nodeY is INodeWrapper;

         if (!xIsIINodeWrapper)
            return -1;
         else if (!yIsIINodeWrapper)
            return 1;
         else
            return (int)(((IINode)nodeX.BaseObject).Handle - ((IINode)nodeY.BaseObject).Handle);
      }
   }
}
