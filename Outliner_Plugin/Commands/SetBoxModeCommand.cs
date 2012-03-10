using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Commands
{
   public class SetBoxModeCommand : Command
{
   protected List<IINode> nodes;
   protected Boolean boxMode;
   protected Dictionary<IINode, Boolean> prevBoxModes;

   public SetBoxModeCommand(List<IINode> nodes, Boolean boxMode)
   {
      this.nodes = nodes;
      this.boxMode = boxMode;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_SetBoxMode; }
   }

   public override void Do()
   {
      if (this.nodes == null)
         return;

      this.prevBoxModes = new Dictionary<IINode, Boolean>(this.nodes.Count);

      foreach (IINode node in this.nodes)
      {
         this.prevBoxModes[node] = node.BoxMode_ != 0;
         node.BoxMode(this.boxMode);
      }
   }

   public override void Undo()
   {
      if (this.prevBoxModes == null)
         return;

      foreach (KeyValuePair<IINode, Boolean> n in this.prevBoxModes)
      {
         n.Key.BoxMode(n.Value);
      }
   }
}
}
