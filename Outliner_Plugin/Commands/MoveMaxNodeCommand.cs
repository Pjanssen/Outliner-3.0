using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Commands
{
   /// <summary>
   /// A generic command used to link nodes, move them to a new layer, or 
   /// reparent layers.
   /// </summary>
   public class MoveMaxNodeCommand : Command
   {
      private IEnumerable<IMaxNodeWrapper> nodes;
      private IMaxNodeWrapper newParent;
      private Dictionary<IMaxNodeWrapper, IMaxNodeWrapper> oldParents;
      private String linkDescription;
      private String unlinkDescription;

      public MoveMaxNodeCommand( IEnumerable<IMaxNodeWrapper> nodes
                               , IMaxNodeWrapper newParent
                               , String linkDescription
                               , String unlinkDescription)
      {
         this.nodes = nodes;
         this.newParent = newParent;
         this.linkDescription = linkDescription;
         this.unlinkDescription = unlinkDescription;
      }

      public override string Description
      {
         get
         {
            if (this.newParent != null)
               return this.linkDescription;
            else
               return this.unlinkDescription;
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
