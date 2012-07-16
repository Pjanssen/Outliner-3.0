using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using MaxUtils;

namespace Outliner.LayerTools
{
public class AutoInheritPropertiesMXS
{
   internal static void Start(IGlobal global)
   {
      HelperMethods.RunResourceScript(typeof(AutoInheritPropertiesMXS).Assembly,
                                      "Outliner.LayerTools.AutoInheritProperties.AutoInheritPropertiesMXS.ms");
   }

   private static IILayer getLayer(UInt64 layerHandle)
   {
      IGlobal.IGlobalAnimatable anim = MaxInterfaces.Global.Animatable;
      return anim.GetAnimByHandle(new UIntPtr(layerHandle)) as IILayer;
   }

   private static AutoInheritProperties.InheritProperty getProperty(String name)
   {
      Type t = typeof(AutoInheritProperties.InheritProperty);
      return (AutoInheritProperties.InheritProperty)Enum.Parse(t, name);
   }

   public static Boolean GetAutoInherit(UInt64 layerHandle, String propName)
   {
      IILayer layer = getLayer(layerHandle);
      AutoInheritProperties.InheritProperty prop = getProperty(propName);

      return AutoInheritProperties.GetAutoInherit(layer, prop);
   }

   public static void SetAutoInherit(UInt64 layerHandle, String propName, Boolean value)
   {
      IILayer layer = getLayer(layerHandle);
      AutoInheritProperties.InheritProperty prop = getProperty(propName);

      AutoInheritProperties.SetAutoInherit(layer, prop, value);
   }

   public static void ClearScene()
   {
      AutoInheritProperties.ClearScene();
   }
}
}
