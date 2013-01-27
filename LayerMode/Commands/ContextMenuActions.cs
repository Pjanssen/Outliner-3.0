using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Modes;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Commands
{
[OutlinerPlugin(OutlinerPluginType.ActionProvider)]
public static class ContextMenuActions
{
   #region Create new layer
   
   [OutlinerAction]
   public static void AddSelectionToNewLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateNewLayerCommand newLayerCmd = new CreateNewLayerCommand(contextNodes);
      newLayerCmd.Execute(false);
   }

   [OutlinerAction]
   public static void CreateEmptyLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateNewLayerCommand cmd = new CreateNewLayerCommand();
      cmd.Execute(false);
   }

   #endregion

   #region Set current layer

   [OutlinerPredicate]
   public static Boolean IsNotCurrentLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      ILayerWrapper layer = TreeMode.GetMaxNode(contextTn) as ILayerWrapper;
      return layer != null && !layer.IsCurrent;
   }

   [OutlinerAction]
   public static void SetCurrentLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      ILayerWrapper layer = TreeMode.GetMaxNode(contextTn) as ILayerWrapper;
      if (layer == null)
         return;

      SetCurrentLayerCommand cmd = new SetCurrentLayerCommand(layer);
      cmd.Execute(false);
   }

   #endregion

   #region Layer display/render settings

   private static IEnumerable<ILayerWrapper> GetLayers(IEnumerable<IMaxNode> nodes)
   {
      return nodes.Where(n => n is ILayerWrapper)
                  .Cast<ILayerWrapper>();
   }

   [OutlinerAction]
   public static void DisplayAllByLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      foreach (ILayerWrapper layer in GetLayers(contextNodes))
      {
         foreach (IINode node in layer.ChildINodes)
         {
            layer.ILayer.SetDisplayByLayer(true, node);
         }
      }
   }

   [OutlinerAction]
   public static void RenderAllByLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      foreach (ILayerWrapper layer in GetLayers(contextNodes))
      {
         foreach (IINode node in layer.ChildINodes)
         {
            layer.ILayer.SetRenderByLayer(true, node);
         }
      }
   }

   #endregion
}
}
