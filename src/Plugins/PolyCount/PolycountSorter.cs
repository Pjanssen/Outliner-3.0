using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.NodeSorters;
using PJanssen.Outliner.Modes;

namespace PJanssen.Outliner.PolyCount
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
