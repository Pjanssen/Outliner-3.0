using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Outliner.Commands;

namespace Outliner.Commands
{
   public class ModifySelectionSetCommand : Command
   {
      private SelectionSetWrapper selSet;
      private IEnumerable<IMaxNodeWrapper> newNodes;
      private IEnumerable<IMaxNodeWrapper> oldNodes;

      public ModifySelectionSetCommand(SelectionSetWrapper selSet,
                                       IEnumerable<IMaxNodeWrapper> newNodes)
      {
         ExceptionHelpers.ThrowIfArgumentIsNull(selSet, "selSet");
         ExceptionHelpers.ThrowIfArgumentIsNull(newNodes, "newNodes");

         this.selSet = selSet;
         this.newNodes = newNodes;
      }

      public override string Description
      {
         get
         {
            return OutlinerResources.Command_ModifySelSet;
         }
      }

      public override void Do()
      {
         this.oldNodes = selSet.WrappedChildNodes;
         selSet.ReplaceNodeset(this.newNodes);
      }

      public override void Undo()
      {
         selSet.ReplaceNodeset(this.oldNodes);
      }
   }
}
