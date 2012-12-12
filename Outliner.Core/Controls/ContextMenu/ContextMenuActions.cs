﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Plugins;
using Outliner.Scene;
using Outliner.Commands;
using Outliner.MaxUtils;
using Outliner.Controls.Tree;

namespace Outliner.Controls.ContextMenu
{
   [OutlinerPlugin(OutlinerPluginType.ActionProvider)]
   public static class ContextMenuActions
   {
      [OutlinerPredicate]
      public static Boolean SelectionNotEmpty(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         return contextNodes != null && contextNodes.Count() > 0;
      }

      [OutlinerAction]
      public static void Rename(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextTn, "contextTn");

         contextTn.TreeView.BeginNodeTextEdit(contextTn);
      }

      [OutlinerPredicate]
      public static Boolean RenameEnabled(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         if (contextTn != null)
         {
            IMaxNodeWrapper node = HelperMethods.GetMaxNode(contextTn);
            if (node != null)
               return node.CanEditName;
         }

         return false;
      }

      [OutlinerAction]
      public static void Delete(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         throw new NotImplementedException();
      }

      #region Hide & Freeze
      
      [OutlinerAction]
      public static void Hide(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         HideCommand cmd = new HideCommand(contextNodes, true);
         cmd.Execute(true);
      }

      [OutlinerPredicate]
      public static Boolean HideEnabled(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         return contextNodes.Any(n => !n.GetNodeProperty(BooleanNodeProperty.IsHidden));
      }

      [OutlinerAction]
      public static void Unhide(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         HideCommand cmd = new HideCommand(contextNodes, false);
         cmd.Execute(true);
      }

      [OutlinerPredicate]
      public static Boolean UnhideEnabled(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         return contextNodes.Any(n => n.GetNodeProperty(BooleanNodeProperty.IsHidden));
      }

      [OutlinerAction]
      public static void Freeze(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         FreezeCommand cmd = new FreezeCommand(contextNodes, true);
         cmd.Execute(true);
      }

      [OutlinerPredicate]
      public static Boolean FreezeEnabled(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         return contextNodes.Any(n => !n.GetNodeProperty(BooleanNodeProperty.IsFrozen));
      }

      [OutlinerAction]
      public static void Unfreeze(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         FreezeCommand cmd = new FreezeCommand(contextNodes, false);
         cmd.Execute(true);
      }

      [OutlinerPredicate]
      public static Boolean UnfreezeEnabled(TreeNode contextTn, IEnumerable<IMaxNodeWrapper> contextNodes)
      {
         Throw.IfArgumentIsNull(contextNodes, "contextNodes");

         return contextNodes.Any(n => n.GetNodeProperty(BooleanNodeProperty.IsFrozen));
      }

      #endregion
   }
}
