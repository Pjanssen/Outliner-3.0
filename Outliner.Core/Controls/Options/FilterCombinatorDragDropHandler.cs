using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Controls.Options
{
   public class FilterCombinatorDragDropHandler : DragDropHandler
   {
      FilterCombinator<MaxNodeWrapper> filter;

      public FilterCombinatorDragDropHandler(FilterCombinator<MaxNodeWrapper> filter)
         : base(null)
      {
         this.filter = filter;
      }

      public override bool AllowDrag
      {
         get { return false; }
      }

      public override bool IsValidDropTarget(System.Windows.Forms.IDataObject dragData)
      {
         IEnumerable<TreeNode> tns = DragDropHandler.GetNodesFromDataObject(dragData);
         return tns != null && tns.All(tn => tn.Tag is Filter<MaxNodeWrapper> && tn.Tag != filter);
      }

      public override System.Windows.Forms.DragDropEffects GetDragDropEffect(System.Windows.Forms.IDataObject dragData)
      {
         if (this.IsValidDropTarget(dragData))
            return System.Windows.Forms.DragDropEffects.Move;
         else
            return Tree.TreeView.NoneDragDropEffects;
      }

      public override void HandleDrop(System.Windows.Forms.IDataObject dragData)
      {
         if (!this.IsValidDropTarget(dragData))
            return;

         IEnumerable<TreeNode> tns = DragDropHandler.GetNodesFromDataObject(dragData);
         foreach (TreeNode tn in tns)
         {
            Filter<MaxNodeWrapper> draggedFilter = tn.Tag as Filter<MaxNodeWrapper>;
            if (tn.Parent != null)
            {
               FilterCombinator<MaxNodeWrapper> parent = tn.Parent.Tag as FilterCombinator<MaxNodeWrapper>;
               parent.Filters.Remove(draggedFilter);
            }
            this.filter.Filters.Add(draggedFilter);
         }
      }
   }
}
