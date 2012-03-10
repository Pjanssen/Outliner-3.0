using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Commands
{
   public class FreezeCommand : Command
{
   protected List<IINode> nodes;
   protected Boolean freeze;
   protected Dictionary<IINode, Boolean> prevFrozenStates;

   public FreezeCommand(List<IINode> nodes, Boolean freeze)
   {
      this.nodes = nodes;
      this.freeze = freeze;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Freeze; }
   }

   public override void Do()
   {
      if (this.nodes == null)
         return;

      this.prevFrozenStates = new Dictionary<IINode, Boolean>(this.nodes.Count);

      foreach (IINode node in this.nodes)
      {
         this.prevFrozenStates[node] = node.IsObjectFrozen;
         node.IsFrozen = this.freeze;
      }
   }

   public override void Undo()
   {
      if (this.prevFrozenStates == null)
         return;

      foreach (KeyValuePair<IINode, Boolean> n in this.prevFrozenStates)
      {
         n.Key.IsFrozen = n.Value;
      }
   }
}
}
