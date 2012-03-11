using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;
using Outliner.Scene;

namespace Outliner.Commands
{
public class SetWireColorCommand : SetNodePropertyCommand<Color>
{
   public SetWireColorCommand(IEnumerable<IMaxNodeWrapper> nodes, Color color)
      : base(nodes, color) { }

   public override string Description
   {
      get { return OutlinerResources.Command_SetWireColor; }
   }

   public override Color GetValue(IMaxNodeWrapper node)
   {
      return node.WireColor;
   }

   public override void SetValue(IMaxNodeWrapper node, Color value)
   {
      node.WireColor = value;
   }
}
}
