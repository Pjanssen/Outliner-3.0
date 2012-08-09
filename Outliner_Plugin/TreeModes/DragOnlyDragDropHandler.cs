using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree.DragDropHandlers;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Commands;

namespace Outliner.TreeModes
{
   public class DragOnlyDragDropHandler : DragDropHandler
   {
      public DragOnlyDragDropHandler(IMaxNodeWrapper data) : base(data) { }

      public override bool AllowDrag
      {
         get { return true; }
      }

      public override bool IsValidDropTarget(WinForms::IDataObject dragData)
      {
         return false;
      }

      public override WinForms::DragDropEffects GetDragDropEffect(WinForms::IDataObject dragData)
      {
         return TreeView.NoneDragDropEffects;
      }

      public override void HandleDrop(System.Windows.Forms.IDataObject dragData) { }
   }
}
