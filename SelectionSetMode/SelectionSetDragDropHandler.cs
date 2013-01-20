using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Commands;
using Outliner.MaxUtils;

namespace Outliner.Modes.SelectionSet
{
public class SelectionSetDragDropHandler : DragDropHandler
{
   public SelectionSetDragDropHandler(SelectionSetWrapper data) : base(data) { }

   public override bool AllowDrag
   {
      get { return false; }
   }

   public override bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return false;

      return this.Data.CanAddChildNodes(HelperMethods.GetMaxNodes(draggedNodes));
   }

   public override WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return TreeView.NoneDragDropEffects;

      if (HelperMethods.ShiftPressed)
         return WinForms::DragDropEffects.Copy;
      else
         return WinForms.DragDropEffects.Move;
   }

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return;

      IEnumerable<IMaxNode> draggedMaxNodes = HelperMethods.GetMaxNodes(draggedNodes);
      SelectionSetWrapper targetSelSet = (SelectionSetWrapper)this.Data;

      IEnumerable<IMaxNode> combinedNodes = targetSelSet.ChildNodes.Union(draggedMaxNodes);
      ModifySelectionSetCommand cmd = new ModifySelectionSetCommand(targetSelSet, combinedNodes);
      cmd.Execute(false);

      if (!HelperMethods.ShiftPressed)
      {
         IEnumerable<SelectionSetWrapper> selSets = draggedNodes.Select(tn => HelperMethods.GetMaxNode(tn.Parent))
                                                                .Where(n => n is SelectionSetWrapper)
                                                                .Cast<SelectionSetWrapper>()
                                                                .Where(n => !n.Equals(targetSelSet))
                                                                .Distinct();
         foreach (SelectionSetWrapper selSet in selSets)
         {
            IEnumerable<IMaxNode> newNodes = selSet.ChildNodes.Except(draggedMaxNodes);
            ModifySelectionSetCommand moveCmd = new ModifySelectionSetCommand(selSet, newNodes);
            moveCmd.Execute(false);
         }
      }
   }
}
}
