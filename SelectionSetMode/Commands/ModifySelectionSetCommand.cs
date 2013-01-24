using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;
using Outliner.Modes.SelectionSet;

namespace Outliner.Commands
{
   /// <summary>
   /// Replaces the nodes in a selection-set.
   /// </summary>
   public class ModifySelectionSetCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private SelectionSetWrapper selSet;

      public ModifySelectionSetCommand( SelectionSetWrapper selSet
                                      , IEnumerable<IMaxNode> nodes)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");
         Throw.IfArgumentIsNull(selSet, "selSet");

         this.selSet = selSet;
         this.nodes = nodes.ToList();
      }

      public override string Description
      {
         get
         {
            return Resources.Command_ModifySelSet;
         }
      }

      public override void Do()
      {
         selSet.ReplaceNodeset(this.nodes);
      }
   }
}
