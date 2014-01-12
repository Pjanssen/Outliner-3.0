using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Modes.Material
{
   public class INodeDragDropHandler : MaxNodeDragDropHandler
   {
      public INodeDragDropHandler(INodeWrapper node) : base(node) { }

      public override bool AllowDrag
      {
         get { return true; }
      }

      public override bool IsValidDropTarget(IDataObject dragData)
      {
         return false;
      }
   }
}
