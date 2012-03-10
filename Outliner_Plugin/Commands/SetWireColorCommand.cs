using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using System.Drawing;

namespace Outliner.Commands
{
public class SetWireColorCommand : Command
{
   protected List<IINode> nodes;
   protected Color color;
   protected Dictionary<IINode, Color> prevColors;

   public SetWireColorCommand(List<IINode> nodes, Color color)
   {
      this.nodes = nodes;
      this.color = color;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetWireColor; }
   }

   public override void Do()
   {
      if (this.nodes == null)
         return;

      this.prevColors = new Dictionary<IINode, Color>(this.nodes.Count);

      foreach (IINode node in this.nodes)
      {
         this.prevColors[node] = node.WireColor;
         node.WireColor = this.color;
      }
   }

   public override void Undo()
   {
      if (this.prevColors == null)
         return;

      foreach (KeyValuePair<IINode, Color> n in this.prevColors)
      {
         n.Key.WireColor = n.Value;
      }
   }
}
}
