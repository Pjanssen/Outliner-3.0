using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   /// <summary>
   /// A generic command used to link nodes, move them to a new layer, or 
   /// reparent layers.
   /// </summary>
   public class MoveMaxNodeCommand : Command
   {
      IEnumerable<IMaxNode> nodes;
      private IMaxNode newParent;
      private String linkDescription;
      private String unlinkDescription;

      /// <summary>
      /// Creates a new MoveMaxNodeCommand.
      /// </summary>
      /// <param name="nodes">The nodes to move.</param>
      /// <param name="newParent">The new parent for the nodes. Use null to unlink.</param>
      /// <param name="linkDescription">The command description for a link action.</param>
      public MoveMaxNodeCommand( IEnumerable<IMaxNode> nodes
                               , IMaxNode newParent
                               , String linkDescription) 
         : this (nodes, newParent, linkDescription, linkDescription)
      { }

      /// <summary>
      /// Creates a new MoveMaxNodeCommand.
      /// </summary>
      /// <param name="nodes">The nodes to move.</param>
      /// <param name="newParent">The new parent for the nodes. Use null to unlink.</param>
      /// <param name="linkDescription">The command description for a link action.</param>
      /// <param name="unlinkDescription">The command description for an unlink action.</param>
      public MoveMaxNodeCommand( IEnumerable<IMaxNode> nodes
                               , IMaxNode newParent
                               , String linkDescription
                               , String unlinkDescription)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");
         Throw.IfArgumentIsNull(linkDescription, "linkDescription");
         Throw.IfArgumentIsNull(unlinkDescription, "unlinkDescription");
         
         this.nodes = nodes.ToList();
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
         if (this.newParent != null)
            this.newParent.AddChildNodes(this.nodes);
         else
            this.nodes.ForEach(n => n.Parent.RemoveChildNode(n));
      }
   }
}
