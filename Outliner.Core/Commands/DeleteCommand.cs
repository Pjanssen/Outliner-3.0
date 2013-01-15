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
   private IEnumerable<IMaxNodeWrapper> nodes;

   public DeleteCommand(IEnumerable<IMaxNodeWrapper> nodes)
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
      foreach (IMaxNodeWrapper node in this.nodes)
      {
         node.Delete();
      }
   }

   protected override void Undo() { }
}
}
