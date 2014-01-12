using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Controls;

namespace PJanssen.Outliner.Modes.SelectionSet
{
public class SelectionSetDragDropHandler : MaxNodeDragDropHandler
{
   public SelectionSetDragDropHandler(SelectionSetWrapper node) : base(node) { }

   public override bool AllowDrag
   {
      get { return true; }
   }

   public override bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      return this.MaxNode.CanAddChildNodes(GetMaxNodesFromDragData(dragData));
   }

   public override WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return TreeView.NoneDragDropEffects;

      if (ControlHelpers.ShiftPressed)
         return WinForms::DragDropEffects.Copy;
      else
         return WinForms.DragDropEffects.Move;
   }

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = TreeView.GetTreeNodesFromDragData(dragData);
      if (draggedNodes == null)
         return;

      IEnumerable<IMaxNode> draggedMaxNodes = TreeMode.GetMaxNodes(draggedNodes);
      SelectionSetWrapper targetSelSet = (SelectionSetWrapper)this.MaxNode;

      IEnumerable<IMaxNode> combinedNodes = targetSelSet.ChildNodes.Union(draggedMaxNodes);
      ModifySelectionSetCommand cmd = new ModifySelectionSetCommand(targetSelSet, combinedNodes);
      cmd.Execute();

      if (!ControlHelpers.ShiftPressed)
      {
         IEnumerable<SelectionSetWrapper> selSets = draggedNodes.Select(tn => TreeMode.GetMaxNode(tn.Parent))
                                                                .OfType<SelectionSetWrapper>()
                                                                .Where(n => !n.Equals(targetSelSet))
                                                                .Distinct();
         foreach (SelectionSetWrapper selSet in selSets)
         {
            IEnumerable<IMaxNode> newNodes = selSet.ChildNodes.Except(draggedMaxNodes);
            ModifySelectionSetCommand moveCmd = new ModifySelectionSetCommand(selSet, newNodes);
            moveCmd.Execute();
         }
      }
   }
}
}
