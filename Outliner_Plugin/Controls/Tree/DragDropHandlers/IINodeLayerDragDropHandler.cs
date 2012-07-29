using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.Tree.DragDropHandlers
{
public class IINodeLayerDragDropHandler : DragDropHandler
{
   public IINodeLayerDragDropHandler(IMaxNodeWrapper data) : base(data) { }

   public override bool AllowDrag
   {
      get { return true; }
   }

   public override bool IsValidDropTarget(IDataObject dragData)
   {
      return false;
   }

   public override DragDropEffects GetDragDropEffect(IDataObject dragData)
   {
      return TreeView.NoneDragDropEffects;
   }

   public override void HandleDrop(IDataObject dragData)
   {
      //Nothing can be dropped onto an IINode in layer mode.
   }
}
}
