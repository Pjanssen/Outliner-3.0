using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Commands
{
   /// <summary>
   /// Adds nodes as childnodes to another node.
   /// </summary>
   public class AddNodesCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private IMaxNode target;
      private String description;

      /// <summary>
      /// Initializes a new instance of the AddNodesCommand.
      /// </summary>
      /// <param name="target">The IMaxNode to add the childnodes to.</param>
      /// <param name="nodes">The IMaxNodes to add to the parent node.</param>
      /// <param name="description">The description of the command.</param>
      public AddNodesCommand( IMaxNode target
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
         this.target.AddChildNodes(nodes);
      }
   }
}
