using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;

namespace Outliner.Commands
{
/// <summary>
/// Turns lights on or off.
/// </summary>
public class ToggleLightCommand : Command
{
   private IEnumerable<IMaxNode> nodes;
   private Boolean on;

   /// <summary>
   /// Initializes a new instance of the ToggleListCommand class.
   /// </summary>
   /// <param name="nodes">The lights to turn on or off.</param>
   /// <param name="on">The new on state.</param>
   public ToggleLightCommand(IEnumerable<IMaxNode> nodes, Boolean on)
   {
      this.nodes = nodes;
      this.on = on;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_ToggleLight; }
   }

   public override void Do()
   {
      foreach (IMaxNode node in this.nodes)
      {
         IINode inode = node.BaseObject as IINode;
         if (inode == null)
            continue;
         ILightObject light = inode.ObjectRef as ILightObject;
         if (light == null)
            continue;

         light.SetUseLight(this.on ? 1 : 0);
      }
   }
}
}
