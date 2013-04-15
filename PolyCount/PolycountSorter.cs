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

   protected override int InternalCompare(IMaxNode nodeX, IMaxNode nodeY)
   {
      INodeWrapper inodeX = nodeX as INodeWrapper;
      if (nodeX == null) 
         return 0;

      INodeWrapper inodeY = nodeY as INodeWrapper;
      if (nodeY == null || !nodeY.IsValid) 
         return 0;

      int numFacesX = IINodes.GetPolyCount(inodeX.INode);
      int numFacesY = IINodes.GetPolyCount(inodeY.INode);

      return numFacesY.CompareTo(numFacesX);
   }
}
}
