using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using MaxUtils;
using System.IO;

namespace Outliner.LayerTools
{
public static class NestedLayersMxs
{
   internal static void Start()
   {
      HelperMethods.RunResourceScript(typeof(NestedLayersMxs).Assembly,
                                      "Outliner.LayerTools.NestedLayers.NestedLayersMxs.ms");
   }

   public const UInt64 RootHandle = 0;

   public static UInt64 GetParent(UInt64 layerHandle)
   {
      IGlobal.IGlobalAnimatable anim = MaxInterfaces.Global.Animatable;
      IILayer layer = anim.GetAnimByHandle(new UIntPtr(layerHandle)) as IILayer;
      IILayer parent = NestedLayers.GetParent(layer);
      if (parent == null)
         return NestedLayersMxs.RootHandle;
      else
         return anim.GetHandleByAnim(parent).ToUInt64();
   }

   public static void SetParent(UInt64 layerHandle, UInt64 parentHandle)
   {
      IGlobal.IGlobalAnimatable anim = MaxInterfaces.Global.Animatable;
      IILayer layer = anim.GetAnimByHandle(new UIntPtr(layerHandle)) as IILayer;
      IILayer parent = (parentHandle != NestedLayersMxs.RootHandle) ?
            anim.GetAnimByHandle(new UIntPtr(parentHandle)) as IILayer
         : null;
      NestedLayers.SetParent(layer, parent);
   }

   public static void ClearScene()
   {
      NestedLayers.ClearScene();
   }
}
}
