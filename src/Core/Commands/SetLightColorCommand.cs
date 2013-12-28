using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using System.Drawing;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
/// <summary>
/// Turns lights on or off.
/// </summary>
public class SetLightColorCommand : Command
{
   private IEnumerable<INodeWrapper> nodes;
   private Color color;

   /// <summary>
   /// Initializes a new instance of the SetLightColorCommand class.
   /// </summary>
   /// <param name="nodes">The lights to change the color of.</param>
   /// <param name="on">The new light color.</param>
   public SetLightColorCommand(IEnumerable<IMaxNode> nodes, Color color)
   {
      this.nodes = nodes.OfType<INodeWrapper>().ToList();
      this.color = color;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetLightColor; }
   }

   public override void Do()
   {
      IPoint3 colorPoint3 = MaxInterfaces.Global.Point3.Create(color.B / 255f, color.G / 255f, color.R / 255f);

      foreach (INodeWrapper node in this.nodes)
      {
         ILightObject light = node.INode.ObjectRef as ILightObject;
         if (light == null)
            continue;
         
         light.SetRGBColor(0, colorPoint3);
      }
   }
}
}
