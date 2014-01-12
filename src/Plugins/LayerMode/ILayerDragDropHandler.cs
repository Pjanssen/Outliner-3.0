using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Modes.Layer
{
internal class ILayerDragDropHandler : MaxNodeDragDropHandler
{
   private ILayerWrapper layer;
   public ILayerDragDropHandler(ILayerWrapper data)
      : base(data)
   {
      this.layer = data;
   }

   public override bool AllowDrag
   {
      get { return !this.layer.IsDefault; }
   }

   public override bool IsValidDropTarget(WinForms::IDataObject dragData)
   {
      return this.MaxNode.CanAddChildNodes(GetMaxNodesFromDragData(dragData));
   }

   public override WinForms.DragDropEffects DefaultDragDropEffect
   {
      get { return WinForms::DragDropEffects.Copy; }
   }

   public override void HandleDrop(WinForms::IDataObject dragData)
   {
      if (!this.IsValidDropTarget(dragData))
         return;

      IEnumerable<IMaxNode> draggedNodes = GetMaxNodesFromDragData(dragData);

      AddNodesCommand cmd = new AddNodesCommand( this.MaxNode
                                               , draggedNodes
                                               , Resources.Command_AddToLayer);
      cmd.Execute();
      Viewports.Redraw();
   }
}
}
