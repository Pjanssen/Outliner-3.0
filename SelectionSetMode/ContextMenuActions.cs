using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Modes.SelectionSet
{
   [OutlinerPlugin(OutlinerPluginType.ActionProvider)]
   public class ContextMenuActions
   {
      [OutlinerPredicate]
      public static Boolean CanDeleteSelectionSet(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         return contextNodes.Any(n => !(n is AllObjectsSelectionSet));
      }
   }
}
