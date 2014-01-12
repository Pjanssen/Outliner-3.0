using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Commands
{
[OutlinerPlugin(OutlinerPluginType.ActionProvider)]
public static class ContextMenuActions
{
   [OutlinerAction]
   public static void AddSelectionToNewSelectionSet(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateSelectionSetCommand cmd = new CreateSelectionSetCommand(contextNodes);
      cmd.Execute();
   }
}
}
