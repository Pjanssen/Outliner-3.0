using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Commands
{
   /// <summary>
   /// Removes IMaxNodes from their parent nodes.
   /// </summary>
   public class UnlinkNodesCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private String description;

      /// <summary>
      /// Initializes a new instance of the UnlinkNodesCommand class.
      /// </summary>
      /// <param name="nodes">The IMaxNodes to unlink.</param>
      /// <param name="description">The description of the command.</param>
      public UnlinkNodesCommand(IEnumerable<IMaxNode> nodes, String description)
      {
         this.nodes = nodes;
         this.description = description;
      }

      public override string Description
      {
         get { return this.description; }
      }

      public override void Do()
      {
         this.nodes.ForEach(RemoveFromParent);
      }

      private static void RemoveFromParent(IMaxNode node)
      {
         IMaxNode parent = node.Parent;
         if (parent != null)
            parent.RemoveChildNode(node);
      }
   }
}
