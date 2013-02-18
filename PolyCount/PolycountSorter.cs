using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.NodeSorters;
using Outliner.Modes;

namespace Outliner.PolyCount
{
[OutlinerPlugin(OutlinerPluginType.NodeSorter)]
[LocalizedDisplayName(typeof(Resources), "Sorter_PolyCount")]
public class PolycountSorter : NodeSorter
{
   public PolycountSorter() : base() { }
   public PolycountSorter(SortOrder sortOrder) : base(sortOrder) { }

   protected override int InternalCompare(TreeNode x, TreeNode y)
   {
      if (x == y)
         return 0;

      INodeWrapper nodeX = TreeMode.GetMaxNode(x) as INodeWrapper;
      if (nodeX == null || !nodeX.IsValid) 
         return 0;

      INodeWrapper nodeY = TreeMode.GetMaxNode(y) as INodeWrapper;
      if (nodeY == null || !nodeY.IsValid) 
         return 0;

      int numFacesX = IINodes.GetPolyCount(nodeX.INode);
      int numFacesY = IINodes.GetPolyCount(nodeY.INode);

      return numFacesY.CompareTo(numFacesX);
   }
}
}
