using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class DeleteCommand : Command
{
   private IEnumerable<MaxNodeWrapper> nodes;

   public DeleteCommand(IEnumerable<MaxNodeWrapper> nodes)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.ToList();
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Delete; }
   }

   protected override void Do()
   {
      foreach (MaxNodeWrapper node in this.nodes)
      {
         node.Delete();
      }
   }

   protected override void Undo() { }
}
}
