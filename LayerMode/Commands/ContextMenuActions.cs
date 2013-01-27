using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Controls.Tree;
using Outliner.Modes;
using Outliner.Modes.Layer;
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
      CreateNewLayerCommand createCmd = new CreateNewLayerCommand(contextNodes);
      createCmd.Execute(false);
   }

   [OutlinerAction]
   public static void CreateEmptyLayer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateNewLayerCommand createCmd = new CreateNewLayerCommand();
      createCmd.Execute(false);

      MoveCreatedLayer(contextTn, createCmd.CreatedLayer);
   }

   private static void MoveCreatedLayer(TreeNode contextTn, IMaxNode createdLayer)
   {
      if (contextTn != null)
      {
         IMaxNode node = GetSelectedLayer(contextTn);
         if (node != null)
         {
            AddNodesCommand addCmd = new AddNodesCommand( node
                                                        , createdLayer.ToIEnumerable()
                                                        , Resources.Command_AddToLayer);
            addCmd.Execute(true);
         }
      }
   }

   private static IMaxNode GetSelectedLayer(TreeNode contextTn)
   {
      if (contextTn == null)
         return null;

      IMaxNode node = TreeMode.GetMaxNode(contextTn);
      if (node is ILayerWrapper)
         return node;
      else
         return GetSelectedLayer(contextTn.Parent);
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
