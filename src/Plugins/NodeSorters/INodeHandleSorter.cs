using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Modes;

namespace PJanssen.Outliner.NodeSorters
{
   [OutlinerPlugin(OutlinerPluginType.NodeSorter)]
   [LocalizedDisplayName(typeof(Resources), "Sorter_INodeHandle")]
   public class INodeHandleSorter : NodeSorter
   {
      public INodeHandleSorter() : base() { }
      public INodeHandleSorter(SortOrder sortOrder) : base(sortOrder) { }

      protected override int InternalCompare(IMaxNode nodeX, IMaxNode nodeY)
      {
         INodeWrapper inodeX = nodeX as INodeWrapper;
         INodeWrapper inodeY = nodeY as INodeWrapper;
         
         if (inodeX == null)
            return -1;
         else if (inodeY == null)
            return 1;
         else
            return (int)(inodeX.INode.Handle - inodeY.INode.Handle);
      }
   }
}
