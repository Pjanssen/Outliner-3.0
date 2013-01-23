using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Controls.Tree;
using Outliner.Filters;
using Outliner.Scene;

namespace Outliner.Controls.Options
{
   public class FilterDragDropHandler : IDragDropHandler
   {
      private Filter<IMaxNode> filter;

      public FilterDragDropHandler(Filter<IMaxNode> filter)
      {
         this.filter = filter;
      }

      public bool AllowDrag
      {
         get { return true; }
      }

      public bool IsValidDropTarget(WinForms.IDataObject dragData)
      {
         return false;
      }

      public WinForms.DragDropEffects GetDragDropEffect(WinForms.IDataObject dragData)
      {
         return TreeView.NoneDragDropEffects;
      }

      public void HandleDrop(WinForms.IDataObject dragData) { }
   }
}
