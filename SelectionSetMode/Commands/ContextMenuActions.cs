using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Commands
{
[OutlinerPlugin(OutlinerPluginType.ActionProvider)]
public static class ContextMenuActions
{
   [OutlinerAction]
   public static void AddSelectionToNewSelectionSet(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateSelectionSetCommand cmd = new CreateSelectionSetCommand(contextNodes);
      cmd.Execute(false);
   }
}
}
