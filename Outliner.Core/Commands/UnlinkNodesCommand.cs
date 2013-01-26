using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
   public class UnlinkNodesCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private String description;

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
