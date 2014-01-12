using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Commands
{
   /// <summary>
   /// Removes IMaxNodes from a parent IMaxNode.
   /// </summary>
   public class RemoveNodesCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private IMaxNode target;
      private String description;

      /// <summary>
      /// Initializes a new instance of the RemoveNodesCommand class.
      /// </summary>
      /// <param name="target">The IMaxNode to remove nodes from.</param>
      /// <param name="nodes">The IMaxNodes to remove from the parent node.</param>
      /// <param name="description">The description of the command.</param>
      public RemoveNodesCommand( IMaxNode target
                               , IEnumerable<IMaxNode> nodes
                               , String description)
      {
         Throw.IfNull(target, "target");
         Throw.IfNull(nodes, "nodes");
         Throw.IfNull(description, "description");

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
