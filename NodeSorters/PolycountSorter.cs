using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Controls.Tree;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.NodeSorters
{
[OutlinerPlugin(OutlinerPluginType.NodeSorter)]
[LocalizedDisplayName(typeof(Resources), "Polycount_DisplayName")]
[LocalizedDisplayImage(typeof(Resources), "sort_polycount_16", "sort_polycount_24")]
public class PolycountSorter : NodeSorter
{
   private const int enable = (int)Autodesk.Max.ObjectWrapper.E173.AllEnable;
   private const int nativeType = (int)Autodesk.Max.ObjectWrapper.E172.TriObject;

   protected override int InternalCompare(TreeNode x, TreeNode y)
   {
      if (x == y)
         return 0;

      IMaxNodeWrapper nodeX = HelperMethods.GetMaxNode(x);
      if (nodeX == null || !nodeX.IsValid) return 0;

      IMaxNodeWrapper nodeY = HelperMethods.GetMaxNode(y);
      if (nodeY == null || !nodeY.IsValid) return 0;

      IINodeWrapper iinodeX = nodeX as IINodeWrapper;
      IINodeWrapper iinodeY = nodeY as IINodeWrapper;

      if (iinodeX != null && iinodeY != null)
      {
         int time = MaxInterfaces.COREInterface.Time;
         IObjectWrapper objWrapperX = MaxInterfaces.Global.ObjectWrapper.Create();
         objWrapperX.Init(time, iinodeX.IINode.EvalWorldState(time, true), false, enable, nativeType);
         
         IObjectWrapper objWrapperY = MaxInterfaces.Global.ObjectWrapper.Create();
         objWrapperY.Init(time, iinodeY.IINode.EvalWorldState(time, true), false, enable, nativeType);

         int xNumFaces = objWrapperX.NumFaces;
         int yNumFaces = objWrapperY.NumFaces;
         objWrapperX.Release();
         objWrapperY.Release();
         if (xNumFaces != yNumFaces)
            return yNumFaces.CompareTo(xNumFaces);
      }

      return NativeMethods.StrCmpLogicalW(nodeX.Name, nodeY.Name);
   }
}
}
