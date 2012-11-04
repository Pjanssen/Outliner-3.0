using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.LayerTools
{
public static class AutoInheritPropertiesMxs
{
   internal static void Start()
   {
      HelperMethods.RunResourceScript(typeof(AutoInheritPropertiesMxs).Assembly,
                                      "Outliner.LayerTools.AutoInheritProperties.AutoInheritPropertiesMxs.ms");
   }

   private static IILayer getLayer(UInt64 layerHandle)
   {
      IGlobal.IGlobalAnimatable anim = MaxInterfaces.Global.Animatable;
      return anim.GetAnimByHandle(new UIntPtr(layerHandle)) as IILayer;
   }

   private static NodeLayerProperty getProperty(String name)
   {
      Type t = typeof(NodeLayerProperty);
      return (NodeLayerProperty)Enum.Parse(t, name);
   }

   public static Boolean GetAutoInherit(UInt64 layerHandle, String propName)
   {
      IILayer layer = getLayer(layerHandle);
      NodeLayerProperty prop = getProperty(propName);

      return AutoInheritProperties.GetAutoInherit(layer, prop);
   }

   public static void SetAutoInherit(UInt64 layerHandle, String propName, Boolean value)
   {
      IILayer layer = getLayer(layerHandle);
      NodeLayerProperty prop = getProperty(propName);

      AutoInheritProperties.SetAutoInherit(layer, prop, value);
   }

   public static void ClearScene()
   {
      AutoInheritProperties.ClearScene();
   }
}
}
