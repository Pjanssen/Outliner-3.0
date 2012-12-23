using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Commands;
using Outliner.Controls.Tree;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Modes.Layer
{
   [OutlinerPlugin(OutlinerPluginType.ActionProvider)]
   public class ContextMenuActions
   {
      [OutlinerAction]
      public static void CreateNewLayer(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         CreateLayerCommand cmd = new CreateLayerCommand();
         cmd.Execute(false);
      }

      [OutlinerPredicate]
      public static Boolean IsNotCurrentLayer(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         IILayerWrapper layer = HelperMethods.GetMaxNode(contextTn) as IILayerWrapper;
         return layer != null && !layer.IsCurrent;
      }

      [OutlinerAction]
      public static void SetCurrentLayer(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         IILayerWrapper layer = HelperMethods.GetMaxNode(contextTn) as IILayerWrapper;
         if (layer == null)
            return;

         SetCurrentLayerCommand cmd = new SetCurrentLayerCommand(layer);
         cmd.Execute(false);
      }
   }
}
