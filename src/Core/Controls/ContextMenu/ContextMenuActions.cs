using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Controls.Tree;
using Autodesk.Max;
using PJanssen.Outliner.Modes;

namespace PJanssen.Outliner.Controls.ContextMenu
{
[OutlinerPlugin(OutlinerPluginType.ActionProvider)]
public static class ContextMenuActions
{
   [OutlinerPredicate]
   public static Boolean SelectionNotEmpty(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      return contextNodes != null && contextNodes.Count() > 0;
   }

   #region Rename

   [OutlinerAction]
   public static void Rename(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextTn, "contextTn");

      contextTn.TreeView.BeginNodeTextEdit(contextTn);
   }

   [OutlinerPredicate]
   public static Boolean RenameEnabled(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      if (contextTn != null)
      {
         IMaxNode node = TreeMode.GetMaxNode(contextTn);
         if (node != null)
            return node.CanEditName;
      }

      return false;
   }

   #endregion

   #region Delete

   [OutlinerPredicate]
   public static Boolean CanDeleteSelection(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      return contextNodes.Any(n => n.CanDelete);
   }

   [OutlinerAction]
   public static void Delete(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      if (contextTn != null)
      {
         DeleteCommand cmd = new DeleteCommand(contextNodes);
         cmd.Execute();
         Viewports.Redraw();
      }
   }

   #endregion

   #region Select Childnodes
   
   [OutlinerPredicate]
   public static Boolean SelectionHasChildNodes(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      return contextNodes.Any(n => n.ChildNodeCount > 0);
   }

   [OutlinerAction]
   public static void SelectChildNodes(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      IEnumerable<IMaxNode> nodes = GetChildNodes(contextNodes);

      SelectCommand cmd = new SelectCommand(nodes);
      cmd.Execute();
      Viewports.Redraw();
   }

   private static IEnumerable<IMaxNode> GetChildNodes(IEnumerable<IMaxNode> nodes)
   {
      IEnumerable<IMaxNode> result = nodes;
      foreach (IMaxNode node in nodes)
      {
         IEnumerable<IMaxNode> childNodes = node.ChildNodes;
         result = result.Concat(childNodes)
                        .Concat(GetChildNodes(childNodes));
      }

      return result;
   }
   
   #endregion

   #region Hide & Freeze
      
   [OutlinerAction]
   public static void Hide(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      HideCommand cmd = new HideCommand(contextNodes, true);
      cmd.Execute(true);
   }

   [OutlinerPredicate]
   public static Boolean HideEnabled(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      return contextNodes.Any(n => !n.GetNodeProperty(BooleanNodeProperty.IsHidden));
   }

   [OutlinerAction]
   public static void Unhide(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      HideCommand cmd = new HideCommand(contextNodes, false);
      cmd.Execute(true);
   }

   [OutlinerPredicate]
   public static Boolean UnhideEnabled(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      return contextNodes.Any(n => n.GetNodeProperty(BooleanNodeProperty.IsHidden));
   }

   [OutlinerAction]
   public static void Freeze(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      FreezeCommand cmd = new FreezeCommand(contextNodes, true);
      cmd.Execute(true);
   }

   [OutlinerPredicate]
   public static Boolean FreezeEnabled(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      return contextNodes.Any(n => !n.GetNodeProperty(BooleanNodeProperty.IsFrozen));
   }

   [OutlinerAction]
   public static void Unfreeze(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      FreezeCommand cmd = new FreezeCommand(contextNodes, false);
      cmd.Execute(true);
   }

   [OutlinerPredicate]
   public static Boolean UnfreezeEnabled(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      Throw.IfNull(contextNodes, "contextNodes");

      return contextNodes.Any(n => n.GetNodeProperty(BooleanNodeProperty.IsFrozen));
   }

   #endregion

   #region Add Selection To

   [OutlinerAction]
   public static void AddSelectionToNewContainer(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateContainerCommand cmd = new CreateContainerCommand(contextNodes);
      cmd.Execute();
      Viewports.Redraw();
   }

   [OutlinerAction]
   public static void AddSelectionToNewGroup(TreeNode contextTn, IEnumerable<IMaxNode> contextNodes)
   {
      CreateNewGroupCommand cmd = new CreateNewGroupCommand(contextNodes);
      cmd.Execute();
      Viewports.Redraw();
   }

   #endregion
}
}
