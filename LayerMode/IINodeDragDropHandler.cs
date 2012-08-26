using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;

namespace Outliner.Modes.Layer
{
   public class IINodeDragDropHandler : DragDropHandler
   {
      public IINodeDragDropHandler(IMaxNodeWrapper data) : base(data) { }

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

      public override void HandleDrop(WinForms::IDataObject dragData)
      {
         //Nothing can be dropped onto an IINode in layer mode.
      }
   }
}
