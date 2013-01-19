using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Commands;

namespace Outliner.Modes.SelectionSet
{
public class TreeViewDragDropHandler : DragDropHandler
{
   public TreeViewDragDropHandler() : base(null) { }

   public override bool AllowDrag
   {
      get { return false; }
   }

   public override bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      return true;
   }

   public override WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
   {
      if (this.IsValidDropTarget(dragData))
         return WinForms::DragDropEffects.Move;
      else
         return TreeView.NoneDragDropEffects;
   }

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<TreeNode> draggedNodes = DragDropHandler.GetNodesFromDataObject(dragData);
      if (draggedNodes == null)
         return;

      List<SelectionSetWrapper> selSets = new List<SelectionSetWrapper>();
      foreach (TreeNode tn in draggedNodes)
      {
         if (tn.Parent == null)
            continue;

         SelectionSetWrapper selSet = HelperMethods.GetMaxNode(tn.Parent) as SelectionSetWrapper;
         if (selSet != null && !(selSet is AllObjectsSelectionSetWrapper) && !selSets.Contains(selSet))
            selSets.Add(selSet);
      }

      IEnumerable<IMaxNode> draggedMaxNodes = HelperMethods.GetMaxNodes(draggedNodes);
      foreach (SelectionSetWrapper selSet in selSets)
      {
         IEnumerable<IMaxNode> newNodes = selSet.ChildNodes.Except(draggedMaxNodes);
         ModifySelectionSetCommand cmd = new ModifySelectionSetCommand(newNodes.ToList(), selSet);
         cmd.Execute(false);
      }
   }
}
}
