using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
   public class RemoveNodesCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private IMaxNode target;
      private String description;

      public RemoveNodesCommand( IMaxNode target
                               , IEnumerable<IMaxNode> nodes
                               , String description)
      {
         Throw.IfArgumentIsNull(target, "target");
         Throw.IfArgumentIsNull(nodes, "nodes");
         Throw.IfArgumentIsNull(description, "description");

         this.nodes = nodes.ToList();
         this.target = target;
         this.description = description;
      }

      public override string Description
      {
         get { return this.description; }
      }

      public override void Do()
      {
         this.target.RemoveChildNodes(nodes);
      }
   }
}
