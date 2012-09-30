using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Commands
{
public class ToggleLightCommand : SetNodePropertyCommand<Boolean>
{
   public ToggleLightCommand(IEnumerable<IMaxNodeWrapper> nodes, Boolean on) 
      : base(nodes, "UseLight", on) { }

   public override string Description
   {
      get { return OutlinerResources.Command_ToggleLight; }
   }

   protected override bool GetValue(Scene.IMaxNodeWrapper node)
   {
      if (node == null)
         return false;

      if (node.SuperClassID != Autodesk.Max.SClass_ID.Light)
         return false;
            
      IINode inode = node.WrappedNode as IINode;
      if (inode == null)
         return false;
      ILightObject light = inode.ObjectRef as ILightObject;
      if (light == null)
         return false;

      return light.UseLight;
   }

   protected override void SetValue(Scene.IMaxNodeWrapper node, bool value)
   {
      if (node == null)
         return;

      if (node.SuperClassID != Autodesk.Max.SClass_ID.Light)
         return;

      IINode inode = node.WrappedNode as IINode;
      if (inode == null)
         return;
      ILightObject light = inode.ObjectRef as ILightObject;
      if (light == null)
         return;

      light.SetUseLight(value ? 1 : 0);
   }
}
}
