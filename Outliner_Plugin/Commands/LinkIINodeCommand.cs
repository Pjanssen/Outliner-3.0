using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
   public class LinkIINodeCommand : Command
   {
      private IEnumerable<IMaxNodeWrapper> nodes;
      private IMaxNodeWrapper newParent;
      private Dictionary<IMaxNodeWrapper, IMaxNodeWrapper> oldParents;

      public LinkIINodeCommand(IEnumerable<IMaxNodeWrapper> nodes, IMaxNodeWrapper newParent) 
      {
         this.nodes = nodes;
         this.newParent = newParent;
      }

      public override string Description
      {
         get
         {
            if (this.newParent != null)
               return OutlinerResources.Command_Link;
            else
               return OutlinerResources.Command_Unlink;
         }
      }

      public override void Do()
      {
         if (this.nodes == null)
            return;

         this.oldParents = new Dictionary<IMaxNodeWrapper, IMaxNodeWrapper>(this.nodes.Count());
         foreach (IMaxNodeWrapper node in this.nodes)
            this.oldParents.Add(node, node.Parent);

         if (this.newParent != null)
            this.newParent.AddChildNodes(this.nodes);
         else
            this.nodes.ForEach(n => n.Parent.RemoveChildNode(n));
      }

      public override void Undo()
      {
         if (this.nodes == null || this.oldParents == null)
            return;

         foreach (KeyValuePair<IMaxNodeWrapper, IMaxNodeWrapper> n in this.oldParents)
            n.Value.AddChildNode(n.Key);
      }
   }
}
