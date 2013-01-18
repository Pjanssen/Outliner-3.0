using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Controls.Options
{
   public class FilterDragDropHandler : DragDropHandler
   {
      private Filter<MaxNodeWrapper> filter;

      public FilterDragDropHandler(Filter<MaxNodeWrapper> filter) : base(null)
      {
         this.filter = filter;
      }

      public override bool AllowDrag
      {
         get { return true; }
      }

      public override bool IsValidDropTarget(System.Windows.Forms.IDataObject dragData)
      {
         return false;
      }

      public override System.Windows.Forms.DragDropEffects GetDragDropEffect(System.Windows.Forms.IDataObject dragData)
      {
         return TreeView.NoneDragDropEffects;
      }

      public override void HandleDrop(System.Windows.Forms.IDataObject dragData) { }
   }
}
