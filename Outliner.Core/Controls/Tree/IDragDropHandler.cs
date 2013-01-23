using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.Tree
{
   public interface IDragDropHandler
   {
      /// <summary>
      /// Determines whether the node can be dragged.
      /// </summary>
      Boolean AllowDrag { get; }

      /// <summary>
      /// Returns true if the dragged data can be dropped onto this handler.
      /// </summary>
      Boolean IsValidDropTarget(IDataObject dragData);

      /// <summary>
      /// Returns the DragDropEffect for this node as a drop-target.
      /// </summary>
      DragDropEffects GetDragDropEffect(IDataObject dragData);

      /// <summary>
      /// Called when a selection of nodes is dropped onto this node.
      /// </summary>
      void HandleDrop(IDataObject dragData);
   }
}
