using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.Modes.Layer
{
   internal class INodeDragDropHandler : MaxNodeDragDropHandler
   {
      public INodeDragDropHandler(IMaxNode data) : base(data) { }

      public override bool AllowDrag
      {
         get { return true; }
      }

      public override bool IsValidDropTarget(WinForms::IDataObject dragData)
      {
         return false;
      }
   }
}
