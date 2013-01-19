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

      INodeWrapper nodeX = HelperMethods.GetMaxNode(x) as INodeWrapper;
      if (nodeX == null || !nodeX.IsValid) 
         return 0;

      INodeWrapper nodeY = HelperMethods.GetMaxNode(y) as INodeWrapper;
      if (nodeY == null || !nodeY.IsValid) 
         return 0;

      int numFacesX = IINodeHelpers.GetPolyCount(nodeX.INode);
      int numFacesY = IINodeHelpers.GetPolyCount(nodeY.INode);

      return numFacesY.CompareTo(numFacesX);
   }
}
}
