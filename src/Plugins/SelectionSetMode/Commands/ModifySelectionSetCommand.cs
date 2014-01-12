using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes.SelectionSet;

namespace PJanssen.Outliner.Commands
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
         Throw.IfNull(nodes, "nodes");
         Throw.IfNull(selSet, "selSet");

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
