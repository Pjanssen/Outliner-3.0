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
[LocalizedDisplayName(typeof(Resources), "Str_PolyCount")]
public class PolycountSorter : NodeSorter
{
   public PolycountSorter() : base() { }
   public PolycountSorter(SortOrder sortOrder) : base(sortOrder) { }

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

      int numFacesX = IINodeHelpers.GetPolyCount(nodeX.IINode);
      int numFacesY = IINodeHelpers.GetPolyCount(nodeY.IINode);

      return numFacesY.CompareTo(numFacesX);
   }
}
}
