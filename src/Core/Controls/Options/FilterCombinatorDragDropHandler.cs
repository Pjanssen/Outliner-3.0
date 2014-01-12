using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Filters;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Controls.Options
{
   public class FilterCombinatorDragDropHandler : IDragDropHandler
   {
      FilterCombinator<IMaxNode> filter;

      public FilterCombinatorDragDropHandler(FilterCombinator<IMaxNode> filter)
      {
         this.filter = filter;
      }

      public bool AllowDrag
      {
         get { return false; }
      }

      public bool IsValidDropTarget(WinForms.IDataObject dragData)
      {
         IEnumerable<TreeNode> tns = TreeView.GetTreeNodesFromDragData(dragData);
         return tns != null && tns.All(tn => tn.Tag is Filter<IMaxNode> && tn.Tag != filter);
      }

      public WinForms.DragDropEffects GetDragDropEffect(WinForms.IDataObject dragData)
      {
         if (this.IsValidDropTarget(dragData))
            return System.Windows.Forms.DragDropEffects.Move;
         else
            return Tree.TreeView.NoneDragDropEffects;
      }

      public void HandleDrop(System.Windows.Forms.IDataObject dragData)
      {
         if (!this.IsValidDropTarget(dragData))
            return;

         IEnumerable<TreeNode> tns = TreeView.GetTreeNodesFromDragData(dragData);
         foreach (TreeNode tn in tns)
         {
            Filter<IMaxNode> draggedFilter = tn.Tag as Filter<IMaxNode>;
            if (tn.Parent != null)
            {
               FilterCombinator<IMaxNode> parent = tn.Parent.Tag as FilterCombinator<IMaxNode>;
               parent.Filters.Remove(draggedFilter);
            }
            this.filter.Filters.Add(draggedFilter);
         }
      }
   }
}
