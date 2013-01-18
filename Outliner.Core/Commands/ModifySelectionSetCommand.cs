using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   public class ModifySelectionSetCommand : Command
   {
      private IEnumerable<MaxNodeWrapper> nodes;
      private SelectionSetWrapper selSet;
      private IEnumerable<MaxNodeWrapper> oldNodes;

      public ModifySelectionSetCommand( IEnumerable<MaxNodeWrapper> nodes
                                      , SelectionSetWrapper selSet)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");
         Throw.IfArgumentIsNull(selSet, "selSet");

         this.nodes = nodes.ToList();
         this.selSet = selSet;
      }

      public override string Description
      {
         get
         {
            return OutlinerResources.Command_ModifySelSet;
         }
      }

      protected override void Do()
      {
         this.oldNodes = selSet.WrappedChildNodes;
         selSet.ReplaceNodeset(this.nodes);
      }

      protected override void Undo()
      {
         selSet.ReplaceNodeset(this.oldNodes);
      }
   }
}
